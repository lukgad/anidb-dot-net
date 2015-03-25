using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libAniDB.NET
{
	public interface IEpisode
	{
		int? EID { get; set; }
		int? AID { get; set; }
		int? Length { get; set; }
		int? Rating { get; set; }
		int? Votes { get; set; }
		int? EpisodeNumber { get; set; }
		char? SpecialPrefix { get; }
		string EnglishName { get; set; }
		string RomanjiName { get; set; }
		string KanjiName { get; set; }
		int? AirDate { get; set; }
		Episode.Special? SpecialType { get; set; }
	}

	public class Episode : IEpisode
	{
		public Episode(AniDBResponse response)
		{
			if(response.Code != AniDBResponse.ReturnCode.EPISODE)
				throw new ArgumentException("Response is not an EPISODE response");

			List<string> dataFields = new List<string>();
			foreach (string[] s in response.DataFields)
				dataFields.AddRange(s);

			EID = int.Parse(dataFields[0]);
			AID = int.Parse(dataFields[1]);
			Length = int.Parse(dataFields[2]);
			Rating = int.Parse(dataFields[3]);
			Votes = int.Parse(dataFields[4]);

			EpisodeNumber = int.Parse(Char.IsDigit(dataFields[5][0]) ? dataFields[5] : dataFields[5].Remove(0, 1));

			EnglishName = dataFields[6];
			RomanjiName = dataFields[7];
			KanjiName = dataFields[8];
			AirDate = int.Parse(dataFields[9]);
			SpecialType = (Special)int.Parse(dataFields[10]);
			//TODO: Sanity check that SpecialType matches SpecialTypeNo
		}

		public int? EID { get; set; }
		public int? AID { get; set; }
		public int? Length { get; set; }
		public int? Rating { get; set; }
		public int? Votes { get; set; }
		public int? EpisodeNumber { get; set; }
		public char? SpecialPrefix
		{
			get { return SpecialType != null ? SpecialChar((Special)SpecialType) : null; }
		}
		public string EnglishName { get; set; }
		public string RomanjiName { get; set; }
		public string KanjiName { get; set; }
		public int? AirDate { get; set; }
		public Special? SpecialType { get; set; }

		public enum Special : int
		{
			RegularEpisode = 1,
			Special = 2,
			Credit = 3,
			Trailer = 4,
			Parody = 5,
			Other = 6
		}

		public static char? SpecialChar(Special s)
		{
			switch (s)
			{
				case Special.RegularEpisode:
					return null;
				case Special.Special:
					return 'S';
				case Special.Credit:
					return 'C';
				case Special.Trailer:
					return 'T';
				case Special.Parody:
					return 'P';
				case Special.Other:
					return 'O';
				default:
					return null;
			}
		}

		public static Special SpecialTypeFromPrefix(char c)
		{
			switch (c)
			{
				case 'S':
					return Special.Special;
				case 'C':
					return Special.Credit;
				case 'T':
					return Special.Trailer;
				case 'P':
					return Special.Parody;
				case 'O':
					return Special.Other;
				case '0':
					return Special.RegularEpisode;
				default:
					throw new ArgumentException("Unknown Episode Type Prefix");
			}
		}
	}
}
