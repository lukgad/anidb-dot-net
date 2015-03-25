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

using System.Collections.Concurrent;
using System.Threading;

namespace libAniDB.NET
{
	/// <summary>
	/// Implementation of the "token bucket" rate-limiting algorithm. Allows for a "bursty" rate that normalises over time to a 
	/// lesser rate. Items are enqueued with the EnqueueItem method and a callback (TokenBucketCallback) is invoked when they are
	/// dequeued.
	/// </summary>
	/// <typeparam name="T">Type of object to be enqueued.</typeparam>
	internal class TokenBucket<T>
	{
		private uint _outDelay;

		/// <summary>
		/// Gets or sets the periodic delay before the output callback is called. This controls the maximum "bursty" rate.
		/// </summary>
		public uint OutDelay
		{
			get { return _outDelay; }
			set
			{
				_outDelay = value;
				_outTimer.Change(value, value);
			}
		}

		private uint _tokenAddDelay;

		/// <summary>
		/// Gets or sets the periodic delay before adding a token to the bucket. This controls the minimum rate.
		/// </summary>
		public uint TokenAddDelay
		{
			get { return _tokenAddDelay; }
			set
			{
				_tokenAddDelay = value;
				_tokenAddTimer.Change(value, value);
			}
		}

		/// <summary>
		/// Gets or sets the maximum tokens allowed in the bucket. This controls the length of a "burst".
		/// </summary>
		public uint TokenCapacity { get; set; }

		private readonly Timer _outTimer;
		private readonly Timer _tokenAddTimer;

		private readonly ConcurrentQueue<T> _outputQueue;

		private uint _tokens;

		/// <summary>
		/// Invoked by TokenBucket when an item is removed.
		/// </summary>
		/// <param name="output">Dequeued item</param>
		public delegate void TokenBucketCallBack(T output);

		/// <summary>
		/// Constructor for the TokenBucket class.
		/// </summary>
		/// <param name="outDelay">Periodic delay before the output callback is called. This controls the maximum "bursty" rate.</param>
		/// <param name="tokenAddDelay">Periodic delay before adding a token to the bucket. This controls the minimum rate.</param>
		/// <param name="tokenCapacity">Maximum number of tokens allowed in the bucket. This controls the length of a "burst".</param>
		/// <param name="startFilled">If true, allows a burst at the start, otherwise starts at the minimum rate.</param>
		/// <param name="outputCallBack">Callback to be invoked periodically.</param>
		public TokenBucket(uint outDelay, uint tokenAddDelay, uint tokenCapacity, bool startFilled,
		                   TokenBucketCallBack outputCallBack)
		{
			_outputQueue = new ConcurrentQueue<T>();

			TokenCapacity = tokenCapacity;

			_tokens = startFilled ? TokenCapacity : 0;

			_outDelay = outDelay;
			_tokenAddDelay = tokenAddDelay;

			//I could probably do this with one timer, but I cbf figuring it out
			_outTimer = new Timer(Output, outputCallBack, outDelay, outDelay);
			_tokenAddTimer = new Timer(TokenAdd, null, tokenAddDelay, tokenAddDelay);
		}

		private void Output(object state)
		{
			T outputObject;

			do
			{
				if (_outputQueue.IsEmpty)
					return;
			} while (!_outputQueue.TryDequeue(out outputObject)); //TODO: This is probably bad... fix it

			_tokens--;

			((TokenBucketCallBack)state)(outputObject);
		}

		private void TokenAdd(object state)
		{
			if (_tokens < TokenCapacity)
				_tokens++;
		}

		/// <summary>
		/// Adds an item to the bucket's input queue.
		/// </summary>
		/// <param name="input">Item to be enqueued.</param>
		public void Input(T input)
		{
			_outputQueue.Enqueue(input);
		}
	}
}
