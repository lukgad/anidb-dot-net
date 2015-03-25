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
	public interface IAnime
	{
		int? AID { get; set; }
		Anime.DateFlag? DateFlags { get; set; }
		string Year { get; set; }
		string Type { get; set; }
		IList<int> RelatedAIDList { get; set; }
		IList<Anime.AIDRelationType> RelatedAIDTypeList { get; set; }
		IList<string> CategoryList { get; set; }
		IList<string> CategoryWeightList { get; set; }
		string RomanjiName { get; set; }
		string KanjiName { get; set; }
		string EnglishName { get; set; }
		IList<string> OtherName { get; set; }
		IList<string> ShortNameList { get; set; }
		IList<string> SynonymList { get; set; }
		int? Episodes { get; set; }
		int? HighestEpisodeNumber { get; set; }
		int? SpecialEpisodeCount { get; set; }
		int? AirDate { get; set; }
		int? EndDate { get; set; }
		string Url { get; set; }
		string PicName { get; set; }
		IList<string> CategoryIdList { get; set; }
		int? Rating { get; set; }
		int? VoteCount { get; set; }
		int? TempRating { get; set; }
		int? TempVoteCount { get; set; }
		int? AverageReviewRating { get; set; }
		int? ReviewCount { get; set; }
		IList<string> AwardList { get; set; }
		bool? IsR18Restricted { get; set; }
	}

	public sealed class Anime : IAnime
	{
		[Flags]
		public enum DateFlag
		{
			None = 0,
			StartDateUnknownDay = 1,
			StartDateUnknownMonthDay = 2,
			EndDateUnknownDay = 4,
			EndDateUnknownMonthDay = 8,
			AnimeEnded = 16,
			StartDateUnknownYear = 32,
			EndDateUnknownYear = 64,
		}

		public class AMask
		{
			[Flags]
			public enum Byte1 : byte
			{
				AID = 128,
				DateFlags = 64,
				Year = 32,
				Type = 16,
				RelatedAIDList = 8,
				RelatedAIDTypeList = 4,
				CategoryList = 2,
				CategoryWeightList = 1,
				None = 0,
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
				None = 0,
			}

			[Flags]
			public enum Byte3 : byte
			{
				Episodes = 128,
				HighestEpisodeNumber = 64,
				SpecialEpCount = 32,
				AirDate = 16,
				EndDate = 8,
				Url = 4,
				PicName = 2,
				CategoryIdList = 1,
				None = 0,
			}

			[Flags]
			public enum Byte4 : byte
			{
				Rating = 128,
				VoteCount = 64,
				TempRating = 32,
				TempVoteCount = 16,
				AverageReviewRating = 8,
				ReviewCount = 4,
				AwardList = 2,
				IsR18Restricted = 1,
				None = 0,
			}

			[Flags]
			public enum Byte5 : byte
			{
				AnimePlanetId = 128,
				AnnId = 64,
				AllCinemaId = 32,
				AnimeNfoId = 16,
				DateRecordUpdated = 1,
				None = 0,
			}

			[Flags]
			public enum Byte6 : byte
			{
				CharacterIdList = 128,
				CreatorIdList = 64,
				MainCreatorIdList = 32,
				MainCreatorNameList = 16,
				None = 0,
			}

			[Flags]
			public enum Byte7 : byte
			{
				SpecialsCount = 128,
				CreditsCount = 64,
				OtherCount = 32,
				TrailerCount = 16,
				ParodyCount = 8,
				None = 0,
			}

			[Flags]
			public enum AMaskValues : ulong
			{
				AID = (ulong)Byte1.AID << 8 * 6,
				DateFlags = (ulong)Byte1.DateFlags << 8 * 6,
				Year = (ulong)Byte1.Year << 8 * 6,
				Type = (ulong)Byte1.Type << 8 * 6,
				RelatedAIDList = (ulong)Byte1.RelatedAIDList << 8 * 6,
				RelatedAIDTypeList = (ulong)Byte1.RelatedAIDTypeList << 8 * 6,
				CategoryList = (ulong)Byte1.CategoryList << 8 * 6,
				CategoryWeightList = (ulong)Byte1.CategoryWeightList << 8 * 6,

				RomanjiName = (ulong)Byte2.RomanjiName << 8 * 5,
				KanjiName = (ulong)Byte2.KanjiName << 8 * 5,
				EnglishName = (ulong)Byte2.EnglishName << 8 * 5,
				OtherName = (ulong)Byte2.OtherName << 8 * 5,
				ShortNameList = (ulong)Byte2.ShortNameList << 8 * 5,
				SynonymList = (ulong)Byte2.SynonymList << 8 * 5,

				Episodes = (ulong)Byte3.Episodes << 8 * 4,
				HighestEpisodeNumber = (ulong)Byte3.HighestEpisodeNumber << 8 * 4,
				SpecialEpisodeCount = (ulong)Byte3.SpecialEpCount << 8 * 4,
				AirDate = (ulong)Byte3.AirDate << 8 * 4,
				EndDate = (ulong)Byte3.EndDate << 8 * 4,
				Url = (ulong)Byte3.Url << 8 * 4,
				PicName = (ulong)Byte3.PicName << 8 * 4,
				CategoryIdList = (ulong)Byte3.CategoryIdList << 8 * 4,

				Rating = (ulong)Byte4.Rating << 8 * 3,
				VoteCount = (ulong)Byte4.VoteCount << 8 * 3,
				TempRating = (ulong)Byte4.TempRating << 8 * 3,
				TempVoteCount = (ulong)Byte4.TempVoteCount << 8 * 3,
				AverageReviewRating = (ulong)Byte4.AverageReviewRating << 8 * 3,
				ReviewCount = (ulong)Byte4.ReviewCount << 8 * 3,
				AwardList = (ulong)Byte4.AwardList << 8 * 3,
				IsR18Restricted = (ulong)Byte4.IsR18Restricted << 8 * 3,

				AnimePlanetId = (ulong)Byte5.AnimePlanetId << 8 * 2,
				AnnId = (ulong)Byte5.AnnId << 8 * 2,
				AllCinemaId = (ulong)Byte5.AllCinemaId << 8 * 2,
				AnimeNfoId = (ulong)Byte5.AnimeNfoId << 8 * 2,
				DateRecordUpdated = (ulong)Byte5.DateRecordUpdated << 8 * 2,

				CharacterIdList = (ulong)Byte6.CharacterIdList << 8 * 1,
				CreatorIdList = (ulong)Byte6.CreatorIdList << 8 * 1,
				MainCreatorIdList = (ulong)Byte6.MainCreatorIdList << 8 * 1,
				MainCreatorNameList = (ulong)Byte6.MainCreatorNameList << 8 * 1,

				SpecialsCount = (ulong)Byte7.SpecialsCount,
				CreditsCount = (ulong)Byte7.CreditsCount,
				OtherCount = (ulong)Byte7.OtherCount,
				TrailerCount = (ulong)Byte7.TrailerCount,
				ParodyCount = (ulong)Byte7.ParodyCount,

				None = 0,
			}

			public AMaskValues Mask { get; private set; }

			public string MaskString
			{
				get { return Mask.ToString("x").Remove(0, 2); }
			}

			public AMask(Byte1 byte1 = Byte1.None, Byte2 byte2 = Byte2.None, Byte3 byte3 = Byte3.None,
			             Byte4 byte4 = Byte4.None, Byte5 byte5 = Byte5.None, Byte6 byte6 = Byte6.None,
			             Byte7 byte7 = Byte7.None)
			{
				Mask = (AMaskValues)
				       ((ulong)byte1 << 8 * 6 |
				        (ulong)byte2 << 8 * 5 |
				        (ulong)byte3 << 8 * 4 |
				        (ulong)byte4 << 8 * 3 |
				        (ulong)byte5 << 8 * 2 |
				        (ulong)byte6 << 8 * 1 |
				        (ulong)byte7 << 8 * 0);
			}

			public AMask(AMaskValues aMaskValues)
			{
				Mask = aMaskValues;
			}
		}

		public enum AIDRelationType
		{
			Sequel = 1,
			Prequel = 2,
			SameSetting = 11,
			AlternativeSetting = 21,
			AlternativeVersion = 32,
			MusicVideo = 41,
			Character = 42,
			SideStory = 51,
			ParentStory = 52,
			Summary = 61,
			FullStory = 62,
			Other = 11,
		}

		private static readonly Dictionary<AMask.AMaskValues, FieldData> AMaskDefs =
			new Dictionary<AMask.AMaskValues, FieldData>
				{
					{ AMask.AMaskValues.AID, new FieldData<int?>("AID") },
					{ AMask.AMaskValues.DateFlags, new FieldData<DateFlag>("Date Flags") },
					{ AMask.AMaskValues.Year, new FieldData<string>("Year") },
					{ AMask.AMaskValues.Type, new FieldData<string>("Type") },
					{ AMask.AMaskValues.RelatedAIDList, new FieldData<IList<int>>("Related AID List") },
					{ AMask.AMaskValues.RelatedAIDTypeList, new FieldData<IList<AIDRelationType>>("Related AID Type List") },
					{ AMask.AMaskValues.CategoryList, new FieldData<IList<string>>("Category List") },
					{ AMask.AMaskValues.CategoryWeightList, new FieldData<List<string>>("Category Weight List") },
					{ AMask.AMaskValues.RomanjiName, new FieldData<string>("Romanji Name") },
					{ AMask.AMaskValues.KanjiName, new FieldData<string>("Kanji Name") },
					{ AMask.AMaskValues.EnglishName, new FieldData<string>("English Name") },
					{ AMask.AMaskValues.OtherName, new FieldData<IList<string>>("Other Names") },
					{ AMask.AMaskValues.ShortNameList, new FieldData<IList<string>>("Short Name List") },
					{ AMask.AMaskValues.SynonymList, new FieldData<IList<string>>("Synonym List") },
					{ AMask.AMaskValues.Episodes, new FieldData<int?>("Episodes") },
					{ AMask.AMaskValues.HighestEpisodeNumber, new FieldData<int?>("Highest Episode Number") },
					{ AMask.AMaskValues.SpecialEpisodeCount, new FieldData<int?>("Special Episodes") },
					{ AMask.AMaskValues.AirDate, new FieldData<int?>("Air Date") },
					{ AMask.AMaskValues.EndDate, new FieldData<int?>("End Date") },
					{ AMask.AMaskValues.Url, new FieldData<string>("URL") },
					{ AMask.AMaskValues.PicName, new FieldData<string>("Pic Name") },
					{ AMask.AMaskValues.CategoryIdList, new FieldData<IList<string>>("Category ID List") },
					{ AMask.AMaskValues.Rating, new FieldData<int?>("Rating") },
					{ AMask.AMaskValues.VoteCount, new FieldData<int?>("Vote Count") },
					{ AMask.AMaskValues.TempRating, new FieldData<int?>("Temp rating") },
					{ AMask.AMaskValues.TempVoteCount, new FieldData<int?>("Temp vote count") },
					{ AMask.AMaskValues.AverageReviewRating, new FieldData<int?>("Average Review Rating") },
					{ AMask.AMaskValues.ReviewCount, new FieldData<int?>("Review Count") },
					{ AMask.AMaskValues.AwardList, new FieldData<IList<string>>("Award List") },
					{ AMask.AMaskValues.IsR18Restricted, new FieldData<bool?>("18+ Restricted") },
					{ AMask.AMaskValues.AnimePlanetId, new FieldData<int?>("Anime Planet ID") },
					{ AMask.AMaskValues.AnnId, new FieldData<int?>("ANN ID") },
					{ AMask.AMaskValues.AllCinemaId, new FieldData<int?>("AllCinema ID") },
					{ AMask.AMaskValues.AnimeNfoId, new FieldData<string>("AnimeNFO ID") },
					{ AMask.AMaskValues.DateRecordUpdated, new FieldData<int?>("Date Record Updated") },
					{ AMask.AMaskValues.CharacterIdList, new FieldData<IList<int>>("Character ID List") },
					{ AMask.AMaskValues.CreatorIdList, new FieldData<IList<int>>("Creator ID List") },
					{ AMask.AMaskValues.MainCreatorIdList, new FieldData<IList<int>>("Main Creator ID List") },
					{ AMask.AMaskValues.MainCreatorNameList, new FieldData<IList<string>>("Main Creator Name List") },
					{ AMask.AMaskValues.SpecialsCount, new FieldData<int?>("Specials Count") },
					{ AMask.AMaskValues.CreditsCount, new FieldData<int?>("Credits Count") },
					{ AMask.AMaskValues.OtherCount, new FieldData<int?>("Other Count") },
					{ AMask.AMaskValues.TrailerCount, new FieldData<int?>("Trailer Count") },
					{ AMask.AMaskValues.ParodyCount, new FieldData<int?>("Parody Count") },
				};

		#region AMask Public Fields

		public int? AID
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.AID); }

			set { SetAMaskValue(AMask.AMaskValues.AID, value); }
		}

		public DateFlag? DateFlags
		{
			get { return (DateFlag?) GetAMaskValue(AMask.AMaskValues.DateFlags); }
			set { SetAMaskValue(AMask.AMaskValues.DateFlags, value); }
		}

		public string Year
		{
			get { return (string)GetAMaskValue(AMask.AMaskValues.Year); }

			set { SetAMaskValue(AMask.AMaskValues.Year, value); }
		}

		public string Type
		{
			get { return (string)GetAMaskValue(AMask.AMaskValues.Type); }

			set { SetAMaskValue(AMask.AMaskValues.Type, value); }
		}

		public IList<int> RelatedAIDList
		{
			get { return (List<int>)GetAMaskValue(AMask.AMaskValues.RelatedAIDList); }

			set { SetAMaskValue(AMask.AMaskValues.RelatedAIDList, value); }
		}

		public IList<AIDRelationType> RelatedAIDTypeList
		{
			get { return (List<AIDRelationType>)GetAMaskValue(AMask.AMaskValues.RelatedAIDTypeList); }

			set { SetAMaskValue(AMask.AMaskValues.RelatedAIDTypeList, value); }
		}

		public IList<string> CategoryList
		{
			get { return (List<string>)GetAMaskValue(AMask.AMaskValues.CategoryList); }

			set { SetAMaskValue(AMask.AMaskValues.CategoryList, value); }
		}

		public IList<string> CategoryWeightList
		{
			get { return (List<string>)GetAMaskValue(AMask.AMaskValues.CategoryWeightList); }

			set { SetAMaskValue(AMask.AMaskValues.CategoryWeightList, value); }
		}

		public string RomanjiName
		{
			get { return (string)GetAMaskValue(AMask.AMaskValues.RomanjiName); }

			set { SetAMaskValue(AMask.AMaskValues.RomanjiName, value); }
		}

		public string KanjiName
		{
			get { return (string)GetAMaskValue(AMask.AMaskValues.KanjiName); }

			set { SetAMaskValue(AMask.AMaskValues.KanjiName, value); }
		}

		public string EnglishName
		{
			get { return (string)GetAMaskValue(AMask.AMaskValues.EnglishName); }

			set { SetAMaskValue(AMask.AMaskValues.EnglishName, value); }
		}

		public IList<string> OtherName
		{
			get { return (List<string>)GetAMaskValue(AMask.AMaskValues.OtherName); }

			set { SetAMaskValue(AMask.AMaskValues.OtherName, value); }
		}

		public IList<string> ShortNameList
		{
			get { return (List<string>)GetAMaskValue(AMask.AMaskValues.ShortNameList); }

			set { SetAMaskValue(AMask.AMaskValues.ShortNameList, value); }
		}

		public IList<string> SynonymList
		{
			get { return (List<string>)GetAMaskValue(AMask.AMaskValues.SynonymList); }

			set { SetAMaskValue(AMask.AMaskValues.SynonymList, value); }
		}

		public int? Episodes
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.Episodes); }

			set { SetAMaskValue(AMask.AMaskValues.Episodes, value); }
		}

		public int? HighestEpisodeNumber
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.HighestEpisodeNumber); }

			set { SetAMaskValue(AMask.AMaskValues.HighestEpisodeNumber, value); }
		}

		public int? SpecialEpisodeCount
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.SpecialEpisodeCount); }

			set { SetAMaskValue(AMask.AMaskValues.SpecialEpisodeCount, value); }
		}

		public int? AirDate
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.AirDate); }

			set { SetAMaskValue(AMask.AMaskValues.AirDate, value); }
		}

		public int? EndDate
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.EndDate); }

			set { SetAMaskValue(AMask.AMaskValues.EndDate, value); }
		}

		public string Url
		{
			get { return (string)GetAMaskValue(AMask.AMaskValues.Url); }

			set { SetAMaskValue(AMask.AMaskValues.Url, value); }
		}

		public string PicName
		{
			get { return (string)GetAMaskValue(AMask.AMaskValues.PicName); }

			set { SetAMaskValue(AMask.AMaskValues.PicName, value); }
		}

		public IList<string> CategoryIdList
		{
			get { return (List<string>)GetAMaskValue(AMask.AMaskValues.CategoryIdList); }

			set { SetAMaskValue(AMask.AMaskValues.CategoryIdList, value); }
		}

		public int? Rating
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.Rating); }

			set { SetAMaskValue(AMask.AMaskValues.Rating, value); }
		}

		public int? VoteCount
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.VoteCount); }

			set { SetAMaskValue(AMask.AMaskValues.VoteCount, value); }
		}

		public int? TempRating
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.TempRating); }

			set { SetAMaskValue(AMask.AMaskValues.TempRating, value); }
		}

		public int? TempVoteCount
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.TempVoteCount); }

			set { SetAMaskValue(AMask.AMaskValues.TempVoteCount, value); }
		}

		public int? AverageReviewRating
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.AverageReviewRating); }

			set { SetAMaskValue(AMask.AMaskValues.AverageReviewRating, value); }
		}

		public int? ReviewCount
		{
			get { return (int?)GetAMaskValue(AMask.AMaskValues.ReviewCount); }

			set { SetAMaskValue(AMask.AMaskValues.ReviewCount, value); }
		}

		public IList<string> AwardList
		{
			get { return (List<string>)GetAMaskValue(AMask.AMaskValues.AwardList); }

			set { SetAMaskValue(AMask.AMaskValues.AwardList, value); }
		}

		public bool? IsR18Restricted
		{
			get { return (bool?)GetAMaskValue(AMask.AMaskValues.IsR18Restricted); }

			set { SetAMaskValue(AMask.AMaskValues.IsR18Restricted, value); }
		}

		#endregion

		private object GetAMaskValue(AMask.AMaskValues a)
		{
			return AMaskDefs[a].GetValue();
		}

		private void SetAMaskValue(AMask.AMaskValues a, object value)
		{
			AMaskDefs[a].SetValue(value);
		}

		public Anime() {}

		public Anime(AniDBResponse response, AMask aMask) : this()
		{
			if (response.Code != AniDBResponse.ReturnCode.ANIME && response.Code != AniDBResponse.ReturnCode.ANIME_BEST_MATCH)
				throw new ArgumentException("Response is not an ANIME response");

			List<string> dataFields = new List<string>();
			foreach (string[] s in response.DataFields)
				dataFields.AddRange(s);

			int currentIndex = 0;

			for (int i = 55; i >= 0; i--)
			{
				if (currentIndex >= dataFields.Count) break;

				AMask.AMaskValues flag = (AMask.AMaskValues)((ulong)Math.Pow(2, i));

				object field = null;

				if (!aMask.Mask.HasFlag(flag)) continue;

				if (AMaskDefs[flag].DataType == typeof (string))
					field = dataFields[currentIndex];
				else if (AMaskDefs[flag].DataType == typeof(int?))
					field = int.Parse(dataFields[currentIndex]);
				else if (AMaskDefs[flag].DataType == typeof(bool?))
					field = bool.Parse(dataFields[currentIndex]);
				else if (AMaskDefs[flag].DataType == typeof(IList<string>))
					//TODO: Make sure these are the only possibilities (and are the right choices)

					field = new List<string>(dataFields[currentIndex].Split(flag == AMask.AMaskValues.CategoryList ? ',' : '\''));
				else if (AMaskDefs[flag].DataType == typeof(IList<AIDRelationType>))
				{
					field = new List<AIDRelationType>();

					foreach (string s in dataFields[currentIndex].Split('\''))
						((List<AIDRelationType>)field).Add((AIDRelationType)int.Parse(s));
				}
				else if (AMaskDefs[flag].DataType == typeof(DateFlag))
					field = (DateFlag) int.Parse(dataFields[currentIndex]);

				currentIndex++;

				AMaskDefs[flag].SetValue(field);

			}
		}

		public override string ToString()
		{
			string result = "";

			foreach(var a in AMaskDefs.Keys)
			{
				result += AMaskDefs[a].Name + ": ";

				if (!AMaskDefs.ContainsKey(a))
				{
					result += "\n";
					continue;
				}

				if (AMaskDefs[a].DataType == typeof(IList<string>))
				{
					result += "\n";

					if (AMaskDefs[a].GetValue() == null)
						continue;

					foreach (string s in (List<string>)AMaskDefs[a].GetValue())
						result += " " + s + "\n";
				}
				else if (AMaskDefs[a].DataType == typeof(IList<int>))
				{
					result += "\n";

					if (AMaskDefs[a].GetValue() == null)
						continue;

					foreach (int i in (List<int>)AMaskDefs[a].GetValue())
						result += " " + i + "\n";
				}
				else if (AMaskDefs[a].DataType == typeof(IList<AIDRelationType>))
				{
					result += "\n";

					if (AMaskDefs[a].GetValue() == null)
						continue;

					foreach (int i in (List<AIDRelationType>)AMaskDefs[a].GetValue())
						result += " " + i + "\n";
				}
				else result += (AMaskDefs[a].GetValue()) + "\n";
			}

			return result;
		}
	}
}
