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

namespace libAniDB.NET
{
	public partial class AniDB : IAniDB
	{
		//--- Auth ---\\

		public AniDBRequest Auth(string user, string pass, bool nat = false,
		                 bool comp = false, int mtu = 0, bool imgServer = false)
		{
			var parValues =
				new Dictionary<string, string>
				{
					{ "user", user },
					{ "pass", pass },
					{ "protover", ProtocolVersion.ToString(CultureInfo.InvariantCulture) },
					{ "client", ClientName },
					{ "clientver", ClientVer.ToString(CultureInfo.InvariantCulture) },
				};
			if (nat)
				parValues.Add("nat", "1");
			if (comp)
				parValues.Add("comp", "1");
			if (_encoding != null)
				parValues.Add("enc", _encoding.WebName);
			if (mtu > 0)
				parValues.Add("mtu", mtu.ToString(CultureInfo.InvariantCulture));
			if (imgServer)
				parValues.Add("imgserver", "1");

			return QueueCommand("AUTH", parValues);
		}

		public AniDBRequest Logout()
		{
			return QueueCommand("LOGOUT");
		}


		public AniDBRequest Encrypt(string user)
		{
			throw new NotImplementedException();
		}

		//--- Misc ---\\
		
		public AniDBRequest Ping(bool nat = false)
		{
			var parValues = new Dictionary<string, string>();

			if (nat)
				parValues.Add("nat", "1");

			return QueueCommand("PING", parValues);
		}

		public AniDBRequest ChangeEncoding(string name)
		{
			return QueueCommand("ENCODING", new KeyValuePair<string, string>("name", name));
		}

		public AniDBRequest Uptime()
		{
			return QueueCommand("UPTIME");
		}

		public AniDBRequest Version()
		{
			return QueueCommand("VERSION");
		}

		//--- Data ---\\

		public AniDBRequest Anime(int aID, Anime.AMask aMask = null)
		{
			var parValues = new Dictionary<string, string>
			                                       { { "aid", aID.ToString(CultureInfo.InvariantCulture) } };

			if (aMask != null)
				parValues.Add("amask", aMask.MaskString);

			return QueueCommand("ANIME", parValues);
		}

		public AniDBRequest Anime(string aName, Anime.AMask aMask = null)
		{
			var parValues = new Dictionary<string, string> { { "aname", aName } };

			if (aMask != null)
				parValues.Add("amask", aMask.MaskString);

			return QueueCommand("ANIME", parValues);
		}


		public AniDBRequest AnimeDesc(int aID, int partNo)
		{
			return QueueCommand("ANIMEDESC",
			                             new KeyValuePair<string, string>("aid", aID.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("partNo", partNo.ToString(CultureInfo.InvariantCulture)));
		}


		public AniDBRequest Calendar()
		{
			return QueueCommand("CALENDAR");
		}


		public AniDBRequest Character(int charID)
		{
			return QueueCommand("CHARACTER",
			                             new KeyValuePair<string, string>("charid", charID.ToString(CultureInfo.InvariantCulture)));
		}


		public AniDBRequest Creator(int creatorID)
		{
			return QueueCommand("CREATOR",
			                             new KeyValuePair<string, string>("creatorid",
			                                                              creatorID.ToString(CultureInfo.InvariantCulture)));
		}


		public AniDBRequest Episode(int eID)
		{
			return QueueCommand("eID",
			                             new KeyValuePair<string, string>("eid", eID.ToString(CultureInfo.InvariantCulture)));
		}

		public AniDBRequest Episode(string aName, int epNo)
		{
			return QueueCommand("EPISODE",
			                             new KeyValuePair<string, string>("aname", aName),
			                             new KeyValuePair<string, string>("epno", epNo.ToString(CultureInfo.InvariantCulture)));
		}

		public AniDBRequest Episode(int aID, int epNo)
		{
			return QueueCommand("EPISODE",
			                             new KeyValuePair<string, string>("aid", aID.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("epno", epNo.ToString(CultureInfo.InvariantCulture)));
		}


		public AniDBRequest File(int fID, AniDBFile.FMask fMask, AniDBFile.AMask aMask)
		{
			return QueueCommand("FILE",
			                             new KeyValuePair<string, string>("fid", fID.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("fmask", fMask.MaskString),
			                             new KeyValuePair<string, string>("amask", aMask.MaskString));
		}

		public AniDBRequest File(long size, string ed2K, AniDBFile.FMask fMask, AniDBFile.AMask aMask)
		{
			return QueueCommand("FILE",
			                             new KeyValuePair<string, string>("size", size.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("ed2k", ed2K),
			                             new KeyValuePair<string, string>("fmask", fMask.MaskString),
			                             new KeyValuePair<string, string>("amask", aMask.MaskString));
		}

		public AniDBRequest File(string aName, string gName, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask)
		{
			return QueueCommand("FILE",
			                             new KeyValuePair<string, string>("aname", aName),
			                             new KeyValuePair<string, string>("gname", gName),
			                             new KeyValuePair<string, string>("epno", epNo.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("fmask", fMask.MaskString),
			                             new KeyValuePair<string, string>("amask", aMask.MaskString));
		}

		public AniDBRequest File(string aName, int gID, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask)
		{
			return QueueCommand("FILE",
			                             new KeyValuePair<string, string>("aname", aName),
			                             new KeyValuePair<string, string>("gid", gID.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("epno", epNo.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("fmask", fMask.MaskString),
			                             new KeyValuePair<string, string>("amask", aMask.MaskString));
		}

		public AniDBRequest File(int aID, string gName, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask)
		{
			return QueueCommand("FILE",
			                             new KeyValuePair<string, string>("aid", aID.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("gname", gName),
			                             new KeyValuePair<string, string>("epno", epNo.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("fmask", fMask.MaskString),
			                             new KeyValuePair<string, string>("amask", aMask.MaskString));
		}

		public AniDBRequest File(int aID, int gID, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask)
		{
			return QueueCommand("FILE",
			                             new KeyValuePair<string, string>("aid", aID.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("gid", gID.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("epno", epNo.ToString(CultureInfo.InvariantCulture)),
			                             new KeyValuePair<string, string>("fmask", fMask.MaskString),
			                             new KeyValuePair<string, string>("amask", aMask.MaskString));
		}


		public AniDBRequest Group(int gID)
		{
			return QueueCommand("GROUP",
			                             new KeyValuePair<string, string>("gid", gID.ToString(CultureInfo.InvariantCulture)));
		}

		public AniDBRequest Group(string gName)
		{
			return QueueCommand("GROUP",
			                             new KeyValuePair<string, string>("gname", gName));
		}


		public AniDBRequest GroupStatus(int aID, int state = 0)
		{
			var parValues = new Dictionary<string, string>
			                                       {
			                                       	{ "aid", aID.ToString(CultureInfo.InvariantCulture) }
			                                       };

			if (state > 0)
				parValues.Add("state", state.ToString(CultureInfo.InvariantCulture));

			return QueueCommand("GROUPSTATUS", parValues);
		}
	}
}
