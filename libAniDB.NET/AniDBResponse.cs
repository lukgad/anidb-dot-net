﻿/* Copyright 2012 Aaron Maslen. All rights reserved.
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
using System.Text;

namespace libAniDB.NET
{
	public class AniDBResponse
	{
		public readonly string Tag;
		public readonly ReturnCode Code;
		public readonly string ReturnString;

		public string[][] DataFields { get; private set; }

		public readonly string OriginalString;

		public AniDBResponse(byte[] responseData, Encoding encoding = null)
		{
			OriginalString = (encoding ?? Encoding.ASCII).GetString(responseData);

			string[] responseLines = OriginalString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

			short returnCode;

			string[] response =
				responseLines[0].Split(new[] { ' ' }, short.TryParse(responseLines[0].Split(' ')[0], out returnCode) ? 2 : 3);

			Tag = response.Length == 3 ? response[0] : "";
			Code = (ReturnCode)(response.Length == 3 ? short.Parse(response[1]) : returnCode);
			ReturnString = response.Length == 3 ? response[2] : response[1];

			List<string[]> datafields = new List<string[]>();

			for (int i = 1; i < responseLines.Length; i++)
				datafields.Add(responseLines[i].Split('|'));

			DataFields = datafields.ToArray();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("Tag: {0}\n", Tag);
			sb.AppendFormat("Return Code: {0} ({1})\n", (short)Code, Code);
			sb.AppendFormat("Return String: {0}\n", ReturnString);

			sb.Append("Data fields:");

			for (int i = 0; i < DataFields.Length; i++)
			{
				sb.Append("\n" + i + ":\n");
				for (int j = 0; j < DataFields[i].Length; j++)
					sb.AppendFormat(j != DataFields[i].Length - 1 ? " {0}\n" : " {0}", DataFields[i][j]);
			}

			return sb.ToString();
		}

		public enum ReturnCode : short
		{
			// ReSharper disable InconsistentNaming
			LOGIN_ACCEPTED                           = 200,
LOGIN_ACCEPTED_NEW_VERSION               = 201,
LOGGED_OUT                               = 203,
RESOURCE                                 = 205,
STATS                                    = 206,
TOP                                      = 207,
UPTIME                                   = 208,
ENCRYPTION_ENABLED                       = 209,
MYLIST_ENTRY_ADDED                       = 210,
MYLIST_ENTRY_DELETED                     = 211,
ADDED_FILE                               = 214,
ADDED_STREAM                             = 215,
EXPORT_QUEUED                            = 217,
EXPORT_CANCELLED                         = 218,
ENCODING_CHANGED                         = 219,
FILE                                     = 220,
MYLIST                                   = 221,
MYLIST_STATS                             = 222,
WISHLIST                                 = 223,
NOTIFICATION                             = 224,
GROUP_STATUS                             = 225,
WISHLIST_ENTRY_ADDED                     = 226,
WISHLIST_ENTRY_DELETED                   = 227,
WISHLIST_ENTRY_UPDATED                   = 228,
MULTIPLE_WISHLIST                        = 229,
ANIME                                    = 230,
ANIME_BEST_MATCH                         = 231,
RANDOM_ANIME                             = 232,
ANIME_DESCRIPTION                        = 233,
REVIEW                                   = 234,
CHARACTER                                = 235,
SONG                                     = 236,
ANIMETAG                                 = 237,
CHARACTERTAG                             = 238,
EPISODE                                  = 240,
UPDATED                                  = 243,
TITLE                                    = 244,
CREATOR                                  = 245,
NOTIFICATION_ENTRY_ADDED                 = 246,
NOTIFICATION_ENTRY_DELETED               = 247,
NOTIFICATION_ENTRY_UPDATE                = 248,
MULTIPLE_NOTIFICATION                    = 249,
GROUP                                    = 250,
CATEGORY                                 = 251,
BUDDY_LIST                               = 253,
BUDDY_STATE                              = 254,
BUDDY_ADDED                              = 255,
BUDDY_DELETED                            = 256,
BUDDY_ACCEPTED                           = 257,
BUDDY_DENIED                             = 258,
VOTED                                    = 260,
VOTE_FOUND                               = 261,
VOTE_UPDATED                             = 262,
VOTE_REVOKED                             = 263,
HOT_ANIME                                = 265,
RANDOM_RECOMMENDATION                    = 266,
RANDOM_SIMILAR                           = 267,
NOTIFICATION_ENABLED                     = 270,
NOTIFYACK_SUCCESSFUL_MESSAGE             = 281,
NOTIFYACK_SUCCESSFUL_NOTIFIATION         = 282,
NOTIFICATION_STATE                       = 290,
NOTIFYLIST                               = 291,
NOTIFYGET_MESSAGE                        = 292,
NOTIFYGET_NOTIFY                         = 293,
SENDMESSAGE_SUCCESSFUL                   = 294,
USER_ID                                  = 295,
CALENDAR                                 = 297,

PONG                                     = 300,
AUTHPONG                                 = 301,
NO_SUCH_RESOURCE                         = 305,
API_PASSWORD_NOT_DEFINED                 = 309,
FILE_ALREADY_IN_MYLIST                   = 310,
MYLIST_ENTRY_EDITED                      = 311,
MULTIPLE_MYLIST_ENTRIES                  = 312,
WATCHED                                  = 313,
SIZE_HASH_EXISTS                         = 314,
INVALID_DATA                             = 315,
STREAMNOID_USED                          = 316,
EXPORT_NO_SUCH_TEMPLATE                  = 317,
EXPORT_ALREADY_IN_QUEUE                  = 318,
EXPORT_NO_EXPORT_QUEUED_OR_IS_PROCESSING = 319,
NO_SUCH_FILE                             = 320,
NO_SUCH_ENTRY                            = 321,
MULTIPLE_FILES_FOUND                     = 322,
NO_SUCH_WISHLIST                         = 323,
NO_SUCH_NOTIFICATION                     = 324,
NO_GROUPS_FOUND                          = 325,
NO_SUCH_ANIME                            = 330,
NO_SUCH_DESCRIPTION                      = 333,
NO_SUCH_REVIEW                           = 334,
NO_SUCH_CHARACTER                        = 335,
NO_SUCH_SONG                             = 336,
NO_SUCH_ANIMETAG                         = 337,
NO_SUCH_CHARACTERTAG                     = 338,
NO_SUCH_EPISODE                          = 340,
NO_SUCH_UPDATES                          = 343,
NO_SUCH_TITLES                           = 344,
NO_SUCH_CREATOR                          = 345,
NO_SUCH_GROUP                            = 350,
NO_SUCH_CATEGORY                         = 351,
BUDDY_ALREADY_ADDED                      = 355,
NO_SUCH_BUDDY                            = 356,
BUDDY_ALREADY_ACCEPTED                   = 357,
BUDDY_ALREADY_DENIED                     = 358,
NO_SUCH_VOTE                             = 360,
INVALID_VOTE_TYPE                        = 361,
INVALID_VOTE_VALUE                       = 362,
PERMVOTE_NOT_ALLOWED                     = 363,
ALREADY_PERMVOTED                        = 364,
HOT_ANIME_EMPTY                          = 365,
RANDOM_RECOMMENDATION_EMPTY              = 366,
RANDOM_SIMILAR_EMPTY                     = 367,
NOTIFICATION_DISABLED                    = 370,
NO_SUCH_ENTRY_MESSAGE                    = 381,
NO_SUCH_ENTRY_NOTIFICATION               = 382,
NO_SUCH_MESSAGE                          = 392,
NO_SUCH_NOTIFY                           = 393,
NO_SUCH_USER                             = 394,
CALENDAR_EMPTY                           = 397,
NO_CHANGES                               = 399,
			
NOT_LOGGED_IN                            = 403,
NO_SUCH_MYLIST_FILE                      = 410,
NO_SUCH_MYLIST_ENTRY                     = 411,
MYLIST_UNAVAILABLE                       = 412,
			
LOGIN_FAILED                             = 500,
LOGIN_FIRST                              = 501,
ACCESS_DENIED                            = 502,
CLIENT_VERSION_OUTDATED                  = 503,
CLIENT_BANNED                            = 504,
ILLEGAL_INPUT_OR_ACCESS_DENIED           = 505,
INVALID_SESSION                          = 506,
NO_SUCH_ENCRYPTION_TYPE                  = 509,
ENCODING_NOT_SUPPORTED                   = 519,
BANNED                                   = 555,
UNKNOWN_COMMAND                          = 598,

INTERNAL_SERVER_ERROR                    = 600,
ANIDB_OUT_OF_SERVICE                     = 601,
SERVER_BUSY                              = 602,
NO_DATA                                  = 603,
TIMEOUT             = 604,
API_VIOLATION                            = 666,

PUSHACK_CONFIRMED                        = 701,
NO_SUCH_PACKET_PENDING                   = 702,
			// ReSharper restore InconsistentNaming
		}
	}
}
