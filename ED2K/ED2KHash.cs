/* Copyright 2012 Aaron Maslen. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 *  1. Redistributions of source code must retain the above copyright
 *     notice, this list of conditions and the following disclaimer.
 * 
 *  2. Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
 * AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL
 * THE FOUNDATION OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace ED2K
{
	public class ED2KHashTask
	{
		public const uint ChunkSize = 9728000;

		private readonly string _filePath;

		public ED2KHashTask(string filePath)
		{
			_filePath = filePath;
		}

		public ED2KHashTask(string filePath, int maxSimultaneousChunks) : this(filePath)
		{
			MaxSimultaneousChunks = maxSimultaneousChunks;
		}

		public uint ChunkCount
		{
			get
			{
				return (uint)((new FileInfo(_filePath)).Length / ChunkSize + 1);
			}
		}

		public uint ReadChunks { get; private set; }
		public uint CompletedChunks { get; private set; }

		public int MaxSimultaneousChunks { get; private set; }

		private class Chunk : IDisposable
		{
			public Chunk(byte[] data, bool last)
			{
				_data = data;
				Last = last;
			}

			public readonly bool Last;
			private byte[] _data;

			public MD4Digest Digest()
			{
				return MD4Context.GetDigest(_data);
			}

			#region Implementation of IDisposable

			public void Dispose()
			{
				_data = null;
			}

			#endregion
		}

		private string _hashString;

		public async Task<string> GetED2KHash()
		{
			if (_hashString != null)
				return _hashString;
			
			var queue = MaxSimultaneousChunks <= 0
					        ? new BlockingCollection<Task<Tuple<MD4Digest, bool>>>(
						            new ConcurrentQueue<Task<Tuple<MD4Digest, bool>>>())
					        : new BlockingCollection<Task<Tuple<MD4Digest, bool>>>(
						            new ConcurrentQueue<Task<Tuple<MD4Digest, bool>>>(), MaxSimultaneousChunks);

			Task.Run(() =>
			{
				var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
				for (uint i = 0; i < ChunkCount; i++)
				{
					byte[] data = new byte[fs.Length - fs.Position > ChunkSize
							                    ? ChunkSize
							                    : fs.Length - fs.Position];
					fs.Read(data, 0, data.Length);
					var last = fs.Position == fs.Length;
					var index = i;
					Debug.Print("=>" + index);
					queue.Add(Task.Run(() =>
					{
						Debug.Print("<=" + index);
						using (var chunk = new Chunk(data, last))
							return Tuple.Create(chunk.Digest(), chunk.Last);
					}));

					ReadChunks = i + 1;
				}
			});
			
			uint completedChunks = 0;
			MD4Context md4Context = new MD4Context();
			Task<Tuple<MD4Digest, bool>> currentChunk;
			do
			{
				currentChunk = queue.Take();

				var chunkHash = (await currentChunk).Item1.ToArray();
				md4Context.Update(chunkHash, 0, chunkHash.Length);
				completedChunks++;
				CompletedChunks = completedChunks;
			} while (!currentChunk.Result.Item2); //While not last

			_hashString = md4Context.GetDigest().ToString();
			return _hashString;
		}
	}

	[Obsolete("Appears broken on large files, and the Task version seems to be just as fast")]
	public class ED2KHash
	{
		private class Chunk
		{
			private readonly byte[] _data;

			public Chunk(byte[] data)
			{
				if(data.Length > ChunkSize)
					throw new ArgumentException("Data is larger than chunk size");

				_data = data;
				Done = new EventWaitHandle(false, EventResetMode.ManualReset);
			}

			public MD4Digest MD4Digest { get; private set; }

			public EventWaitHandle Done { get; private set; }

			public bool Last;

			public void CalculateHash()
			{
				Done.Reset();
				MD4Digest = MD4Context.GetDigest(_data);
				Done.Set();
			}
		}

		private readonly  BlockingCollection<Chunk> _processQueue;
		private readonly FileStream _fileStream;

		public const int ChunkSize = 9728000;

		public ED2KHash(string filePath, int maxSimultaneousChunks)
		{
			_fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

			_processQueue = new BlockingCollection<Chunk>(new ConcurrentQueue<Chunk>(), maxSimultaneousChunks);
		}

		private string _hash;
		public string Hash
		{
			get
			{
				if (_hash == null)
					CalculateHash();

				return _hash;
			}
		}

		public void CalculateHash()
		{
			Complete = 0;

			MD4Context md4Context = new MD4Context();

			if (ChunkCount == 1)
			{
				byte[] data = new byte[_fileStream.Length];
				_fileStream.Read(data, 0, data.Length);

				Chunk newChunk = new Chunk(data);
						
				newChunk.CalculateHash();

				_hash = newChunk.MD4Digest.ToString();

				Complete = 1;
				return;
			}

			Parallel.Invoke(new Action[]
					            {
					                () =>
					                	{
											for (int i = 0; i < ChunkCount; i++)
											{
												byte[] data = new byte[_fileStream.Length - _fileStream.Position > ChunkSize
																		? ChunkSize
																		: _fileStream.Length - _fileStream.Position];

												_fileStream.Read(data, 0, data.Length);

												Chunk newChunk = new Chunk(data) {Last = _fileStream.Position == _fileStream.Length};
												ThreadPool.QueueUserWorkItem(o => newChunk.CalculateHash());

												//Blocks if the queue is full
												_processQueue.Add(newChunk);
											}
					                	},
									() =>
										{
											Chunk currentChunk;
											do
											{
												//Blocks if the queue is empty
												currentChunk = _processQueue.Take();

												//Blocks if the chunk hash is still calculating
												currentChunk.Done.WaitOne();

												byte[] chunkHash = currentChunk.MD4Digest.ToArray();

												md4Context.Update(chunkHash, 0, chunkHash.Length);

												Complete++;
											} while (!currentChunk.Last);
										}
					            });

			_hash = md4Context.GetDigest().ToString();
		}

		public int ChunkCount {
			get
			{
				return (int) _fileStream.Length / ChunkSize + 1;
			}
		}

		public int Complete { get; private set; }
	}
}
