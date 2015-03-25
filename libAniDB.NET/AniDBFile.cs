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
using System.Globalization;
using System.Linq;

namespace libAniDB.NET
{
	public interface IAniDBFile
	{
		int? FID { get; set; }
		int? AID { get; set; }
		int? EID { get; set; }
		int? GID { get; set; }
		int? MyListID { get; set; }
		IDictionary<int, byte> OtherEpisodes { get; set; }
		bool? Deprecated { get; set; }
		AniDBFile.StateMask? State { get; set; }
		long? Size { get; set; }
		string ED2K { get; set; }
		string MD5 { get; set; }
		string SHA1 { get; set; }
		string CRC32 { get; set; }
		string VideoColorDepth { get; set; }
		string Quality { get; set; }
		string Source { get; set; }
		IList<string> AudioCodecs { get; set; }
		IList<int> AudioBitrates { get; set; }
		string VideoCodec { get; set; }
		int? VideoBitrate { get; set; }
		string VideoResolution { get; set; }
		string FileExtension { get; set; }
		IList<string> DubLanguage { get; set; }
		IList<string> SubLanguage { get; set; }
		int? Length { get; set; }
		string Description { get; set; }
		int? AirDate { get; set; }
		string AniDBFileName { get; set; }
		int? MyListState { get; set; }
		int? MyListFileState { get; set; }
		int? MyListViewed { get; set; }
		int? MyListViewDate { get; set; }
		string MyListStorage { get; set; }
		string MyListSource { get; set; }
		string MyListOther { get; set; }
		int? Episodes { get; set; }
		int? HighestEpisodeNumber { get; set; }
		string Year { get; set; }
		string Type { get; set; }
		IList<int> RelatedAIDList { get; set; }
		IList<Anime.AIDRelationType> RelatedAIDTypeList { get; set; }
		IList<string> CategoryList { get; set; }
		string RomanjiName { get; set; }
		string KanjiName { get; set; }
		string EnglishName { get; set; }
		IList<string> OtherName { get; set; }
		IList<string> ShortNameList { get; set; }
		IList<string> SynonymList { get; set; }
		string EpisodeNumber { get; set; }
		string EpisodeName { get; set; }
		string EpisodeRomanjiName { get; set; }
		string EpisodeKanjiName { get; set; }
		int? EpisodeRating { get; set; }
		int? EpisodeVoteCount { get; set; }
		string GroupName { get; set; }
		string GroupShortName { get; set; }
		int? DateAIDRecordUpdated { get; set; }
	}

	public sealed partial class AniDBFile : IAniDBFile
	{
		[Flags]
		public enum StateMask : short
		{
			NoneUnset = 0,
			CrcOk = 1,
			CrcErr = 2,
			V2 = 4,
			V3 = 8,
			V4 = 16,
			V5 = 32,
			Uncensored = 64,
			Censored = 128
		}

		public int? FID { get; set; }

		//protected void DecodeData(AniDBFile fileResponse)
		//{
		//	if (FID == null || FID == -1)
		//		FID = fileResponse.FID;

		//	else if (fileResponse.FID != FID)
		//		throw new ArgumentException("Data to add is for a different file");

		//	foreach (AniDBFile.FMask.FMaskValues f in fileResponse.FMaskFields.Keys)
		//		SetFMaskValue(f, fileResponse.FMaskFields[f]);

		//	foreach (AniDBFile.AMask.AMaskValues a in fileResponse.AMaskFields.Keys)
		//		SetAMaskValue(a, fileResponse.AMaskFields[a]);
		//}

		public override string ToString()
		{
			string result = "FID: " + (FID != null ? FID.Value.ToString(CultureInfo.InvariantCulture) : "") + "\n";

			foreach (var f in FMaskFields.Keys)
			{
				result += FMaskFields[f].Name + ": ";

				if (!FMaskFields.ContainsKey(f))
				{
					result += "\n";
					continue;
				}

				if (FMaskFields[f].DataType == typeof (IList<string>))
				{
					result += "\n";

					if (FMaskFields[f].GetValue() == null)
						continue;

					foreach (string s in ((FieldData<IList<string>>)FMaskFields[f]).Value)
						result += " " + s + "\n";
				}
				else if (FMaskFields[f].DataType == typeof (IList<int>))
				{
					result += "\n";

					if (FMaskFields[f].GetValue() == null)
						continue;

					foreach (int i in ((FieldData<IList<int>>)FMaskFields[f]).Value)
						result += " " + i + "\n";
				}
				else if (FMaskFields[f].DataType == typeof (IDictionary<int, byte>))
				{
					result += "\n";

					if (FMaskFields[f].GetValue() == null)
						continue;

					foreach (int i in ((FieldData<IDictionary<int, byte>>)FMaskFields[f]).Value.Keys)
						result += " " + i + " " + ((FieldData<IDictionary<int, byte>>)FMaskFields[f]).Value[i] + "\n";
				}
				else result += FMaskFields[f].GetValue() + "\n";
			}

			foreach (AMask.AMaskValues a in AMaskFields.Keys)
			{
				result += AMaskFields[a].Name + ": ";

				if (!AMaskFields.ContainsKey(a))
				{
					result += "\n";
					continue;
				}

				if (AMaskFields[a].DataType == typeof (IList<string>))
				{
					result += "\n";

					if (AMaskFields[a].GetValue() == null)
						continue;

					foreach (string s in ((FieldData<IList<string>>)AMaskFields[a]).Value)
						result += " " + s + "\n";
				}
				else if (AMaskFields[a].DataType == typeof(IList<int>))
				{
					result += "\n";

					if (AMaskFields[a].GetValue() == null)
						continue;

					foreach (int i in ((FieldData<IList<int>>)AMaskFields[a]).Value)
						result += " " + i + "\n";
				}
				else result += AMaskFields[a].GetValue() + "\n";
			}

			return result;
		}

		public AniDBFile()
		{
			FID = null;
		}

		public AniDBFile(AniDBResponse fileResponse, FMask fMask, AMask aMask) : this()
		{
			if (fileResponse.Code != AniDBResponse.ReturnCode.FILE)
				throw new ArgumentException("Response is not a FILE response");

			List<string> dataFields = new List<string>();
			foreach (string[] sa in fileResponse.DataFields)
				dataFields.AddRange(sa);

			FID = int.Parse(dataFields[0]);

			int currentIndex = 1;

			for (int i = 39; /* 8*5 - 1 ie. 40 bits */ i >= 0; i--)
			{
				if (currentIndex >= dataFields.Count) break;

				FMask.FMaskValues flag = (FMask.FMaskValues)((long)Math.Pow(2, i));

				if (!fMask.Mask.HasFlag(flag)) continue;

				//Parse value
				object field = null;

				if (dataFields[currentIndex] != "")
					if (FMaskFields[flag].DataType == typeof(string))
						field = dataFields[currentIndex];
					else if (FMaskFields[flag].DataType == typeof(int))
						field = int.Parse(dataFields[currentIndex]);
					else if (FMaskFields[flag].DataType == typeof(short))
						field = short.Parse(dataFields[currentIndex]);
					else if (FMaskFields[flag].DataType == typeof(bool))
						field = short.Parse(dataFields[currentIndex]) == 1;
					else if (FMaskFields[flag].DataType == typeof(long))
						field = long.Parse(dataFields[currentIndex]);
					else if (FMaskFields[flag].DataType == typeof(StateMask))
						field = short.Parse(dataFields[currentIndex]);
					else if (FMaskFields[flag].DataType == typeof(IDictionary<int, byte>))
					{
						string[] splitString = dataFields[currentIndex].Split('\'');
						Dictionary<int, byte> otherEpisodes = new Dictionary<int, byte>();

						if (dataFields[currentIndex] != "")
							for (int j = 0; j < splitString.Length; j += 2)
								otherEpisodes.Add(int.Parse(splitString[j]), byte.Parse(splitString[j + 1]));

						field = otherEpisodes;
					}
					else if (FMaskFields[flag].DataType == typeof(IList<string>))
						field = new List<string>(dataFields[currentIndex].Split('\''));
					else if (FMaskFields[flag].DataType == typeof(IList<int>))
						field = dataFields[currentIndex].Split('\'').Select(int.Parse).ToList();

				FMaskFields[flag].SetValue(field);

				currentIndex++;
			}

			for (int i = 31; i >= 0; i--)
			{
				if (currentIndex >= dataFields.Count) break;

				AMask.AMaskValues flag = (AMask.AMaskValues)((uint)Math.Pow(2, i));

				if (!aMask.Mask.HasFlag(flag)) continue;

				object field = null;

				if (AMaskFields[flag].DataType == typeof (int))
					field = int.Parse(dataFields[currentIndex]);
				else if (AMaskFields[flag].DataType == typeof(string))
					field = dataFields[currentIndex];
				else if (AMaskFields[flag].DataType == typeof(IList<string>))
					field = new List<string>(dataFields[currentIndex].Split('\''));
				else if (AMaskFields[flag].DataType == typeof(IList<int>))
					field = dataFields[currentIndex].Split('\'').Select(int.Parse).ToList();
				else if (AMaskFields[flag].DataType == typeof(IList<Anime.AIDRelationType>))
				{
					field = new List<Anime.AIDRelationType>();

					foreach (string s in dataFields[currentIndex].Split('\''))
						((List<Anime.AIDRelationType>)field).Add((Anime.AIDRelationType)int.Parse(s));
				}

				AMaskFields[flag].SetValue(field);

				currentIndex++;
			}
		}
	}
}
