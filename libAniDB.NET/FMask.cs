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
using System.Collections.Generic;

namespace libAniDB.NET
{
	public sealed partial class AniDBFile : IAniDBFile
	{
		public class FMask
		{
			[Flags]
			public enum Byte1 : byte
			{
				//Byte 1
				AID = 64,
				EID = 32,
				GID = 16,
				MyListID = 8,
				OtherEpisodes = 4,
				IsDeprecated = 2,
				State = 1,
				None = 0
			}

			[Flags]
			public enum Byte2 : byte
			{
				//Byte 2
				Size = 128,
				ED2K = 64,
				MD5 = 32,
				SHA1 = 16,
				CRC32 = 8,
				VideoColorDepth = 2,
				None = 0
			}

			[Flags]
			public enum Byte3 : byte
			{
				//Byte 3
				Quality = 128,
				Source = 64,
				AudioCodecs = 32,
				AudioBitrates = 16,
				VideoCodec = 8,
				VideoBitrate = 4,
				VideoResolution = 2,
				FileExtension = 1,
				None = 0
			}

			[Flags]
			public enum Byte4 : byte
			{
				//Byte 4
				DubLanguage = 128,
				SubLanguage = 64,
				Length = 32,
				Description = 16,
				AiredDate = 8,
				AniDBFileName = 1,
				None = 0
			}

			[Flags]
			public enum Byte5 : byte
			{
				//Byte 5
				MyListState = 128,
				MyListFileState = 64,
				MyListViewed = 32,
				MyListViewDate = 16,
				MyListStorage = 8,
				MyListSource = 4,
				MyListOther = 2,
				None = 0
			}

			[Flags]
			public enum FMaskValues : long
			{
				//Byte 1
				AID = (long)Byte1.AID << 8 * 4,
				EID = (long)Byte1.EID << 8 * 4,
				GID = (long)Byte1.GID << 8 * 4,
				MyListID = (long)Byte1.MyListID << 8 * 4,
				OtherEpisodes = (long)Byte1.OtherEpisodes << 8 * 4,
				IsDeprecated = (long)Byte1.IsDeprecated << 8 * 4,
				State = (long)Byte1.State << 8 * 4,

				//Byte 2
				Size = (long)Byte2.Size << 8 * 3,
				ED2K = (long)Byte2.ED2K << 8 * 3,
				MD5 = (long)Byte2.MD5 << 8 * 3,
				SHA1 = (long)Byte2.SHA1 << 8 * 3,
				CRC32 = (long)Byte2.CRC32 << 8 * 3,
				VideoColorDepth = (long)Byte2.VideoColorDepth << 8 * 3,

				//Byte 3
				Quality = (long)Byte3.Quality << 8 * 2,
				Source = (long)Byte3.Source << 8 * 2,
				AudioCodecs = (long)Byte3.AudioCodecs << 8 * 2,
				AudioBitrates = (long)Byte3.AudioBitrates << 8 * 2,
				VideoCodec = (long)Byte3.VideoCodec << 8 * 2,
				VideoBitrate = (long)Byte3.VideoBitrate << 8 * 2,
				VideoResolution = (long)Byte3.VideoResolution << 8 * 2,
				FileExtension = (long)Byte3.FileExtension << 8 * 2,

				//Byte 4
				DubLanguage = (long)Byte4.DubLanguage << 8 * 1,
				SubLanguage = (long)Byte4.SubLanguage << 8 * 1,
				Length = (long)Byte4.Length << 8 * 1,
				Description = (long)Byte4.Description << 8 * 1,
				AiredDate = (long)Byte4.AiredDate << 8 * 1,
				AniDBFileName = (long)Byte4.AniDBFileName << 8 * 1,

				//Byte 5
				MyListState = (long)Byte5.MyListState << 8 * 0,
				MyListFileState = (long)Byte5.MyListFileState << 8 * 0,
				MyListViewed = (long)Byte5.MyListViewed << 8 * 0,
				MyListViewDate = (long)Byte5.MyListViewDate << 8 * 0,
				MyListStorage = (long)Byte5.MyListStorage << 8 * 0,
				MyListSource = (long)Byte5.MyListSource << 8 * 0,
				MyListOther = (long)Byte5.MyListOther << 8 * 0,

				None = 0
			}

			public FMaskValues Mask { get; private set; }

			public string MaskString
			{
				get { return Mask.ToString("x").Remove(0, 6); }
			}

			public FMask(Byte1 byte1 = Byte1.None, Byte2 byte2 = Byte2.None, Byte3 byte3 = Byte3.None,
			             Byte4 byte4 = Byte4.None, Byte5 byte5 = Byte5.None)
			{
				Mask = (FMaskValues)(((long)byte1 << 8 * 4) |
				                     ((long)byte2 << 8 * 3) |
				                     ((long)byte3 << 8 * 2) |
				                     ((long)byte4 << 8) |
				                     ((long)byte5));
			}

			public FMask(FMaskValues fMaskValues)
			{
				Mask = fMaskValues;
			}

			public override string ToString()
			{
				return MaskString;
			}
		}

		internal static Dictionary<FMask.FMaskValues, FieldData> FMaskFields =
			new Dictionary<FMask.FMaskValues, FieldData>
				{
					{ FMask.FMaskValues.AID, new FieldData<int>("AID") },
					{ FMask.FMaskValues.EID, new FieldData<int>("EID") },
					{ FMask.FMaskValues.GID, new FieldData<int>("GID") },
					{ FMask.FMaskValues.MyListID, new FieldData<int>("MyList ID") },
					{ FMask.FMaskValues.OtherEpisodes, new FieldData<IDictionary<int, byte>>("Other Episodes") },
					{ FMask.FMaskValues.IsDeprecated, new FieldData<bool>("Deprecated") },
					{ FMask.FMaskValues.State, new FieldData<StateMask>("State") },
					{ FMask.FMaskValues.Size, new FieldData<long>("Size") },
					{ FMask.FMaskValues.ED2K, new FieldData<string>("ED2K Hash") },
					{ FMask.FMaskValues.MD5, new FieldData<string>("MD5 Hash") },
					{ FMask.FMaskValues.SHA1, new FieldData<string>("SHA1 Hash") },
					{ FMask.FMaskValues.CRC32, new FieldData<string>("CRC32 Hash") },
					{ FMask.FMaskValues.VideoColorDepth, new FieldData<string>("Video Color Depth") },
					{ FMask.FMaskValues.Quality, new FieldData<string>("Quality") },
					{ FMask.FMaskValues.Source, new FieldData<string>("Source") },
					{ FMask.FMaskValues.AudioCodecs, new FieldData<IList<string>>("Audio Codecs") },
					{ FMask.FMaskValues.AudioBitrates, new FieldData<IList<int>>("Audio Bitrates") },
					{ FMask.FMaskValues.VideoCodec, new FieldData<string>("Video Codec") },
					{ FMask.FMaskValues.VideoBitrate, new FieldData<int>("Video Bitrate") },
					{ FMask.FMaskValues.VideoResolution, new FieldData<string>("Video Resolution") },
					{ FMask.FMaskValues.FileExtension, new FieldData<string>("File Extension") },
					{ FMask.FMaskValues.DubLanguage, new FieldData<IList<string>>("Dub Language(s)") },
					{ FMask.FMaskValues.SubLanguage, new FieldData<IList<string>>("Sub Language(s)") },
					{ FMask.FMaskValues.Length, new FieldData<int>("Length") },
					{ FMask.FMaskValues.Description, new FieldData<string>("Description") },
					{ FMask.FMaskValues.AiredDate, new FieldData<int>("Aired") },
					{ FMask.FMaskValues.AniDBFileName, new FieldData<string>("AniDB File Name") },
					{ FMask.FMaskValues.MyListState, new FieldData<int>("MyList State") },
					{ FMask.FMaskValues.MyListFileState, new FieldData<int>("MyList File State") },
					{ FMask.FMaskValues.MyListViewed, new FieldData<int>("MyList Viewed") },
					{ FMask.FMaskValues.MyListViewDate, new FieldData<int>("MyList View Date") },
					{ FMask.FMaskValues.MyListStorage, new FieldData<string>("MyList Storage") },
					{ FMask.FMaskValues.MyListSource, new FieldData<string>("MyList Source") },
					{ FMask.FMaskValues.MyListOther, new FieldData<string>("MyListOther") }
				};

		#region FMask public properties

		public int? AID
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.AID); }
			set { SetFMaskValue(FMask.FMaskValues.AID, value); }
		}

		public int? EID
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.EID); }
			set { SetFMaskValue(FMask.FMaskValues.EID, value); }
		}

		public int? GID
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.GID); }
			set { SetFMaskValue(FMask.FMaskValues.GID, value); }
		}

		public int? MyListID
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.MyListID); }
			set { SetFMaskValue(FMask.FMaskValues.MyListID, value); }
		}

		public IDictionary<int, byte> OtherEpisodes
		{
			get { return GetFMaskValue<Dictionary<int, byte>>(FMask.FMaskValues.OtherEpisodes); }
			set { SetFMaskValue(FMask.FMaskValues.OtherEpisodes, value); }
		}

		public bool? Deprecated
		{
			get { return GetFMaskValue<bool?>(FMask.FMaskValues.IsDeprecated); }
			set { SetFMaskValue(FMask.FMaskValues.IsDeprecated, value); }
		}

		public StateMask? State
		{
			get { return GetFMaskValue<StateMask?>(FMask.FMaskValues.State); }
			set { SetFMaskValue(FMask.FMaskValues.State, value); }
		}

		public long? Size
		{
			get { return GetFMaskValue<long?>(FMask.FMaskValues.Size); }
			set { SetFMaskValue(FMask.FMaskValues.Size, value); }
		}

		public string ED2K
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.ED2K); }
			set { SetFMaskValue(FMask.FMaskValues.ED2K, value); }
		}

		public string MD5
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.MD5); }
			set { SetFMaskValue(FMask.FMaskValues.MD5, value); }
		}

		public string SHA1
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.SHA1); }
			set { SetFMaskValue(FMask.FMaskValues.SHA1, value); }
		}

		public string CRC32
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.CRC32); }
			set { SetFMaskValue(FMask.FMaskValues.CRC32, value); }
		}

		public string VideoColorDepth
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.VideoColorDepth); }
			set { SetFMaskValue(FMask.FMaskValues.VideoColorDepth, value); }
		}

		public string Quality
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.Quality); }
			set { SetFMaskValue(FMask.FMaskValues.Quality, value); }
		}

		public string Source
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.Source); }
			set { SetFMaskValue(FMask.FMaskValues.Source, value); }
		}

		public IList<string> AudioCodecs
		{
			get { return GetFMaskValue<List<string>>(FMask.FMaskValues.AudioCodecs); }
			set { SetFMaskValue(FMask.FMaskValues.AudioCodecs, value); }
		}

		public IList<int> AudioBitrates
		{
			get { return GetFMaskValue<List<int>>(FMask.FMaskValues.AudioBitrates); }
			set { SetFMaskValue(FMask.FMaskValues.AudioBitrates, value); }
		}

		public string VideoCodec
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.VideoCodec); }
			set { SetFMaskValue(FMask.FMaskValues.VideoCodec, value); }
		}

		public int? VideoBitrate
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.VideoBitrate); }
			set { SetFMaskValue(FMask.FMaskValues.VideoBitrate, value); }
		}

		public string VideoResolution
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.VideoResolution); }
			set { SetFMaskValue(FMask.FMaskValues.VideoResolution, value); }
		}

		public string FileExtension
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.FileExtension); }
			set { SetFMaskValue(FMask.FMaskValues.FileExtension, value); }
		}

		public IList<string> DubLanguage
		{
			get { return GetFMaskValue <List<string>>(FMask.FMaskValues.DubLanguage); }
			set { SetFMaskValue(FMask.FMaskValues.DubLanguage, value); }
		}

		public IList<string> SubLanguage
		{
			get { return GetFMaskValue<List<string>>(FMask.FMaskValues.SubLanguage); }
			set { SetFMaskValue(FMask.FMaskValues.SubLanguage, value); }
		}

		public int? Length
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.Length); }
			set { SetFMaskValue(FMask.FMaskValues.Length, value); }
		}

		public string Description
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.Description); }
			set { SetFMaskValue(FMask.FMaskValues.Description, value); }
		}

		public int? AirDate
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.AiredDate); }
			set { SetFMaskValue(FMask.FMaskValues.AiredDate, value); }
		}

		public string AniDBFileName
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.AniDBFileName); }
			set { SetFMaskValue(FMask.FMaskValues.AniDBFileName, value); }
		}

		public int? MyListState
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.MyListState); }
			set { SetFMaskValue(FMask.FMaskValues.MyListState, value); }
		}

		public int? MyListFileState
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.MyListFileState); }
			set { SetFMaskValue(FMask.FMaskValues.MyListFileState, value); }
		}

		public int? MyListViewed
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.MyListViewed); }
			set { SetFMaskValue(FMask.FMaskValues.MyListViewed, value); }
		}

		public int? MyListViewDate
		{
			get { return GetFMaskValue<int?>(FMask.FMaskValues.MyListViewDate); }
			set { SetFMaskValue(FMask.FMaskValues.MyListViewDate, value); }
		}

		public string MyListStorage
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.MyListStorage); }
			set { SetFMaskValue(FMask.FMaskValues.MyListStorage, value); }
		}

		public string MyListSource
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.MyListSource); }
			set { SetFMaskValue(FMask.FMaskValues.MyListSource, value); }
		}

		public string MyListOther
		{
			get { return GetFMaskValue<string>(FMask.FMaskValues.MyListOther); }
			set { SetFMaskValue(FMask.FMaskValues.MyListOther, value); }
		}

		#endregion

		internal T GetFMaskValue<T>(FMask.FMaskValues f)
		{
			return ((FieldData<T>)FMaskFields[f]).Value;
		}

		internal void SetFMaskValue<T>(FMask.FMaskValues f, T value)
		{
			((FieldData<T>) FMaskFields[f]).Value = value;
		}
	}
}
