using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libAniDB.NET
{
	public interface IGroup
	{
		int? GID { get; set; }
		int? Rating { get; set; }
		int? Votes { get; set; }
		int? AnimeCount { get; set; }
		int? FileCount { get; set; }
		string Name { get; set; }
		string ShortName { get; set; }
		string IrcChannel { get; set; }
		string IrcServer { get; set; }
		string Url { get; set; }
		string PicName { get; set; }
		int? FoundedDate { get; set; }
		int? DisbandedDate { get; set; }
		Group.DateFlag? DateFlags { get; set; }
		int? LastReleaseDate { get; set; }
		int? LastActivityDate { get; set; }
		Dictionary<int, Group.RelationType> GroupRelations { get; set; }
	}

	public class Group : IGroup
	{
		[Flags]
		public enum DateFlag : short
		{
			None = 0,
			FoundedDateUnknownDay = 1,
			FoundedDateUnknownMonthDay = 2,
			DisbandedDateUnknownDay = 4,
			DisbandedDateUnknownMonthDay = 8,
			FoundedDateUnknownYear = 16,
			DisbandedDateUnknownYear = 32
		}

		public enum RelationType
		{
			ParticipantIn = 1,
			ParentOf = 2,
			MergedFrom = 4,
			NowKnownAs = 5,
			Other = 6
		}

		#region Implementation of IGroup

		public int? GID { get; set; }
		public int? Rating { get; set; }
		public int? Votes { get; set; }
		public int? AnimeCount { get; set; }
		public int? FileCount { get; set; }
		public string Name { get; set; }
		public string ShortName { get; set; }
		public string IrcChannel { get; set; }
		public string IrcServer { get; set; }
		public string Url { get; set; }
		public string PicName { get; set; }
		public int? FoundedDate { get; set; }
		public int? DisbandedDate { get; set; }
		public DateFlag? DateFlags { get; set; }
		public int? LastReleaseDate { get; set; }
		public int? LastActivityDate { get; set; }
		public Dictionary<int, RelationType> GroupRelations { get; set; }

		#endregion

		public Group(AniDBResponse groupResponse)
		{
			if(groupResponse.Code != AniDBResponse.ReturnCode.GROUP)
				throw new ArgumentException("Response is not a GROUP response");

			GID = int.Parse(groupResponse.DataFields[0][0]);
			Rating = int.Parse(groupResponse.DataFields[0][1]);
			Votes = int.Parse(groupResponse.DataFields[0][2]);
			AnimeCount = int.Parse(groupResponse.DataFields[0][3]);
			FileCount = int.Parse(groupResponse.DataFields[0][4]);
			Name = groupResponse.DataFields[0][5];
			ShortName = groupResponse.DataFields[0][6];
			IrcChannel = groupResponse.DataFields[0][7];
			IrcServer = groupResponse.DataFields[0][8];
			Url = groupResponse.DataFields[0][9];
			PicName = groupResponse.DataFields[0][10];
			FoundedDate = int.Parse(groupResponse.DataFields[0][11]);
			DisbandedDate = int.Parse(groupResponse.DataFields[0][12]);
			DateFlags = (DateFlag) short.Parse(groupResponse.DataFields[0][13]);
			LastReleaseDate = int.Parse(groupResponse.DataFields[0][14]);
			LastActivityDate = int.Parse(groupResponse.DataFields[0][15]);

			GroupRelations = new Dictionary<int, RelationType>();
			for(int i = 16; i < groupResponse.DataFields.Length; i++)
			{
				string[] relation = groupResponse.DataFields[0][i].Split('\'');

				GroupRelations.Add(int.Parse(relation[1]), (RelationType)int.Parse(relation[0]));
			}
		}
	}
}
