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
		public class AMask
		{
			[Flags]
			public enum Byte1 : byte
			{
				TotalEpisodes = 128,
				HighestEpisodeNumber = 64,
				Year = 32,
				Type = 16,
				RelatedAIDList = 8,
				RelatedAIDTypeList = 4,
				CategoryList = 2,
				None = 0
			}

			[Flags]
			public enum Byte2 : byte
			{
				RomanjiName = 128,
				KanjiName = 64,
				EnglishName = 32,
				OtherName = 16,
				ShortNameList = 8,
				SynonymList = 4,
				None = 0
			}

			[Flags]
			public enum Byte3 : byte
			{
				EpNo = 128,
				EpName = 64,
				EpRomanjiName = 32,
				EpKanjiName = 16,
				EpisodeRating = 8,
				EpisodeVoteCount = 4,
				None = 0
			}

			[Flags]
			public enum Byte4 : byte
			{
				GroupName = 128,
				GroupShortName = 64,
				DateAIDRecordUpdated = 1,
				None = 0
			}

			[Flags]
			public enum AMaskValues : uint
			{
				TotalEpisodes = (uint) Byte1.TotalEpisodes << 8*3,
				HighestEpisodeNumber = (uint) Byte1.HighestEpisodeNumber << 8*3,
				Year = (uint) Byte1.Year << 8*3,
				Type = (uint) Byte1.Type << 8*3,
				RelatedAIDList = (uint) Byte1.RelatedAIDList << 8*3,
				RelatedAIDTypeList = (uint) Byte1.RelatedAIDTypeList << 8*3,
				CategoryList = (uint) Byte1.CategoryList << 8*3,

				RomanjiName = (uint) Byte2.RomanjiName << 8*2,
				KanjiName = (uint) Byte2.KanjiName << 8*2,
				EnglishName = (uint) Byte2.EnglishName << 8*2,
				OtherName = (uint) Byte2.OtherName << 8*2,
				ShortNameList = (uint) Byte2.ShortNameList << 8*2,
				SynonymList = (uint) Byte2.SynonymList << 8*2,

				EpNo = (uint) Byte3.EpNo << 8*1,
				EpName = (uint) Byte3.EpName << 8*1,
				EpRomanjiName = (uint) Byte3.EpRomanjiName << 8*1,
				EpKanjiName = (uint) Byte3.EpKanjiName << 8*1,
				EpisodeRating = (uint) Byte3.EpisodeRating << 8*1,
				EpisodeVoteCount = (uint) Byte3.EpisodeVoteCount << 8*1,

				GroupName = (uint) Byte4.GroupName << 8*0,
				GroupShortName = (uint) Byte4.GroupShortName << 8*0,
				DateAIDRecordUpdated = (uint) Byte4.DateAIDRecordUpdated << 8*0,
				None = 0
			}

			public AMaskValues Mask { get; private set; }

			public string MaskString
			{
				get { return Mask.ToString("x"); }
			}

			public AMask(Byte1 byte1 = Byte1.None, Byte2 byte2 = Byte2.None, Byte3 byte3 = Byte3.None,
			             Byte4 byte4 = Byte4.None)
			{
				Mask = (AMaskValues) (((int) byte1 << 8*3) |
				                      ((int) byte2 << 8*2) |
				                      ((int) byte3 << 8*1) |
				                      ((int) byte4));
			}

			public AMask(AMaskValues aMaskValues)
			{
				Mask = aMaskValues;
			}

			public override string ToString()
			{
				return MaskString;
			}
		}

		internal readonly Dictionary<AMask.AMaskValues, FieldData> AMaskFields =
			new Dictionary<AMask.AMaskValues, FieldData>
				{
					{AMask.AMaskValues.TotalEpisodes, new FieldData<int?>("Total Episodes")},
					{AMask.AMaskValues.HighestEpisodeNumber, new FieldData<int?>("Highest Episode Number")},
					{AMask.AMaskValues.Year, new FieldData<string>("Year")},
					{AMask.AMaskValues.Type, new FieldData<string>("Type")},
					{AMask.AMaskValues.RelatedAIDList, new FieldData<IList<int?>>("Related AIDs")},
					{AMask.AMaskValues.RelatedAIDTypeList, new FieldData<IList<Anime.AIDRelationType>>("Related AID Types")},
					{AMask.AMaskValues.CategoryList, new FieldData<IList<string>>("Categories")},
					{AMask.AMaskValues.RomanjiName, new FieldData<string>("Romanji Name")},
					{AMask.AMaskValues.KanjiName, new FieldData<string>("Kanji Name")},
					{AMask.AMaskValues.EnglishName, new FieldData<string>("English Name")},
					{AMask.AMaskValues.OtherName, new FieldData<IList<string>>("Other Names")},
					{AMask.AMaskValues.ShortNameList, new FieldData<IList<string>>("Short Names")},
					{AMask.AMaskValues.SynonymList, new FieldData<IList<string>>("Synonyms")},
					{AMask.AMaskValues.EpNo, new FieldData<string>("Episode No.")},
					{AMask.AMaskValues.EpName, new FieldData<string>("Episode Name")},
					{AMask.AMaskValues.EpRomanjiName, new FieldData<string>("Romanji Name")},
					{AMask.AMaskValues.EpKanjiName, new FieldData<string>("Kanji Name")},
					{AMask.AMaskValues.EpisodeRating, new FieldData<int?>("Episode Rating")},
					{AMask.AMaskValues.EpisodeVoteCount, new FieldData<int?>("Episode Vote Count")},
					{AMask.AMaskValues.GroupName, new FieldData<string>("Group Name")},
					{AMask.AMaskValues.GroupShortName, new FieldData<string>("Group Short Name")},
					{AMask.AMaskValues.DateAIDRecordUpdated, new FieldData<int?>("Date AID Record Updated")}
				};

		#region AMask public properties

		public int? Episodes
		{
			get { return GetAMaskValue<int?>(AMask.AMaskValues.TotalEpisodes); }
			set { SetAMaskValue(AMask.AMaskValues.TotalEpisodes, value); }
		}

		public int? HighestEpisodeNumber
		{
			get { return GetAMaskValue<int?>(AMask.AMaskValues.HighestEpisodeNumber); }
			set { SetAMaskValue(AMask.AMaskValues.HighestEpisodeNumber, value); }
		}

		public string Year
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.Year); }
			set { SetAMaskValue(AMask.AMaskValues.Year, value); }
		}

		public string Type
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.Type); }
			set { SetAMaskValue(AMask.AMaskValues.Type, value); }
		}

		public IList<int> RelatedAIDList
		{
			get { return GetAMaskValue<List<int>>(AMask.AMaskValues.RelatedAIDList); }
			set { SetAMaskValue(AMask.AMaskValues.RelatedAIDList, value); }
		}

		public IList<Anime.AIDRelationType> RelatedAIDTypeList
		{
			get { return GetAMaskValue<List<Anime.AIDRelationType>>(AMask.AMaskValues.RelatedAIDTypeList); }
			set { SetAMaskValue(AMask.AMaskValues.RelatedAIDTypeList, value); }
		}

		public IList<string> CategoryList
		{
			get { return GetAMaskValue<List<string>>(AMask.AMaskValues.CategoryList); }
			set { SetAMaskValue(AMask.AMaskValues.CategoryList, value); }
		}

		public string RomanjiName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.RomanjiName); }
			set { SetAMaskValue(AMask.AMaskValues.RomanjiName, value); }
		}

		public string KanjiName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.KanjiName); }
			set { SetAMaskValue(AMask.AMaskValues.KanjiName, value); }
		}

		public string EnglishName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.EnglishName); }
			set { SetAMaskValue(AMask.AMaskValues.EnglishName, value); }
		}

		public IList<string> OtherName
		{
			get { return GetAMaskValue<List<string>>(AMask.AMaskValues.OtherName); }
			set { SetAMaskValue(AMask.AMaskValues.OtherName, value); }
		}

		public IList<string> ShortNameList
		{
			get { return GetAMaskValue<List<string>>(AMask.AMaskValues.ShortNameList); }
			set { SetAMaskValue(AMask.AMaskValues.ShortNameList, value); }
		}

		public IList<string> SynonymList
		{
			get { return GetAMaskValue<List<string>>(AMask.AMaskValues.SynonymList); }
			set { SetAMaskValue(AMask.AMaskValues.SynonymList, value); }
		}

		public string EpisodeNumber
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.EpNo); }
			set { SetAMaskValue(AMask.AMaskValues.EpNo, value); }
		}

		public string EpisodeName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.EpName); }
			set { SetAMaskValue(AMask.AMaskValues.EpName, value); }
		}

		public string EpisodeRomanjiName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.EpRomanjiName); }
			set { SetAMaskValue(AMask.AMaskValues.EpRomanjiName, value); }
		}

		public string EpisodeKanjiName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.EpKanjiName); }
			set { SetAMaskValue(AMask.AMaskValues.EpKanjiName, value); }
		}

		public int? EpisodeRating
		{
			get { return GetAMaskValue<int?>(AMask.AMaskValues.EpisodeRating); }
			set { SetAMaskValue(AMask.AMaskValues.EpisodeRating, value); }
		}

		public int? EpisodeVoteCount
		{
			get { return GetAMaskValue<int?>(AMask.AMaskValues.EpisodeVoteCount); }
			set { SetAMaskValue(AMask.AMaskValues.EpisodeVoteCount, value); }
		}

		public string GroupName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.GroupName); }
			set { SetAMaskValue(AMask.AMaskValues.GroupName, value); }
		}

		public string GroupShortName
		{
			get { return GetAMaskValue<string>(AMask.AMaskValues.GroupShortName); }
			set { SetAMaskValue(AMask.AMaskValues.GroupShortName, value); }
		}

		public int? DateAIDRecordUpdated
		{
			get { return GetAMaskValue<int?>(AMask.AMaskValues.DateAIDRecordUpdated); }
			set { SetAMaskValue(AMask.AMaskValues.DateAIDRecordUpdated, value); }
		}

		#endregion

		internal T GetAMaskValue<T>(AMask.AMaskValues a)
		{
			return ((FieldData<T>) AMaskFields[a]).Value;
		}

		internal void SetAMaskValue<T>(AMask.AMaskValues a, T value)
		{
			((FieldData<T>) AMaskFields[a]).Value = value;
		}
	}
}
