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

namespace libAniDB.NET
{
	public interface IAniDB
	{
		///// <summary>
		///// True if encryption is enabled
		///// </summary>
		//bool EncryptionEnabled { get; }

		/// <summary>
		/// Auth with the AniDB server.
		/// </summary>
		/// <param name="user">User name</param>
		/// <param name="pass">Password</param>
		/// <param name="nat">Returns the detected</param>
		/// <param name="comp">Enable compression</param>
		/// <param name="mtu">Maximum Transmission Unit size of responses (in bytes). Valid values are in the range 400-1400</param>
		/// <param name="imgServer">Gets an image server domain name</param>
		/// <remarks>
		/// Possible Replies:
		/// <list type="bullet">
		///  <item><description>200 {str session_key} LOGIN ACCEPTED</description></item>
		///  <item><description>201 {str session_key} LOGIN ACCEPTED - NEW VERSION AVAILABLE</description></item>
		///  <item><description>500 LOGIN FAILED</description></item>
		///  <item><description>503 CLIENT VERSION OUTDATED</description></item>
		///  <item><description>504 CLIENT BANNED - {str reason}</description></item>
		///  <item><description>505 ILLEGAL INPUT OR ACCESS DENIED</description></item>
		///  <item><description>601 ANIDB OUT OF SERVICE - TRY AGAIN LATER</description></item>
		/// <listheader><description>when nat=1</description></listheader>
		///  <item><description>200 {str session_key} {str ip}:{int2 port} LOGIN ACCEPTED</description></item>
		///  <item><description>201 {str session_key} {str ip}:{int2 port} LOGIN ACCEPTED - NEW VERSION AVAILABLE</description></item>
		/// <listheader><description>when imgserver=1</description></listheader>
		///  <item><description>200 {str session_key} LOGIN ACCEPTED<br/>
		///   {str image server name}</description></item>
		///  <item><description>201 {str session_key} LOGIN ACCEPTED - NEW VERSION AVAILABLE<br/>
		///   {str image server name}</description></item>
		/// </list>
		/// </remarks>
		AniDBRequest Auth(string user, string pass, bool nat = false, bool comp = false,
				  int mtu = 0, bool imgServer = false);

		/// <summary>
		/// Logs out, must be logged in
		/// </summary>
		/// <remarks>
		/// Possible Replies:
		/// <list type="bullet">
		///  <item><description>203 LOGGED OUT</description></item>
		///  <item><description>403 NOT LOGGED IN</description></item>
		/// </list>
		/// </remarks>
		AniDBRequest Logout();

		/// <summary>
		/// Not Yet Implemented
		/// </summary>
		/// <param name="user">Use this user's API key to encrypt the session</param>
		AniDBRequest Encrypt(string user);

		/// <summary>
		/// Ping Command. Can be used to detect whether the outgoing port number has been changed by a NAT router (set nat to true) 
		/// or to keep a "connection" alive.
		/// </summary>
		/// <param name="nat">If true(default), returns the outgoing port number.</param>
		/// <remarks>
		/// This command does not require a session.<br/>
		/// Possible Replies:
		/// <list type="bullet">
		///  <item><description>300 PONG<br/>
		///   {int4 port}</description></item>
		/// </list>
		/// </remarks>
		AniDBRequest Ping(bool nat = false);

		/// <summary>
		/// Sets preferred encoding per session. The preferred way to do this is to use the enc argument for AUTH.
		/// This command is mostly for testing.
		/// </summary>
		/// <param name="name">Encoding name</param>
		/// <remarks>
		/// This command does not require a session.<br/>
		/// Possible Replies:
		/// <list type="bullet">
		///  <item><description>219 ENCODING CHANGED</description></item>
		///  <item><description>519 ENCODING NOT SUPPORTED</description></item>
		/// </list>
		/// </remarks>
		AniDBRequest ChangeEncoding(string name);

		/// <summary>
		/// Retrive server uptime. The preferred way to check that the session is OK.
		/// </summary>
		/// <remarks>
		/// Possible Replies:
		/// <list type="bullet">
		///  <item><description>208 UPTIME<br/>
		///   {int4 udpserver uptime in milliseconds}</description></item>
		/// </list>
		/// </remarks>
		AniDBRequest Uptime();

		/// <summary>
		/// Retrieve Server Version
		/// </summary>
		/// <remarks>
		/// This command does not require a session.<br/>
		/// Possible Replies:
		/// <list type="bullet">
		///  <item><description>998 VERSION<br/>
		///   {str server version}</description></item>
		/// </list>
		/// </remarks>
		AniDBRequest Version();


		AniDBRequest Anime(int aID, Anime.AMask aMask = null);
		AniDBRequest Anime(string aName, Anime.AMask aMask = null);

		AniDBRequest AnimeDesc(int aID, int partNo);

		AniDBRequest Calendar();

		AniDBRequest Character(int charID);

		AniDBRequest Creator(int creatorID);

		AniDBRequest Episode(int eID);
		AniDBRequest Episode(string aName, int epNo);
		AniDBRequest Episode(int aID, int epNo);

		AniDBRequest File(int fID, AniDBFile.FMask fMask, AniDBFile.AMask aMask);
		AniDBRequest File(long size, string ed2K, AniDBFile.FMask fMask, AniDBFile.AMask aMask);
		AniDBRequest File(string aName, string gName, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask);
		AniDBRequest File(string aName, int gID, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask);
		AniDBRequest File(int aID, string gName, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask);
		AniDBRequest File(int aID, int gID, int epNo, AniDBFile.FMask fMask, AniDBFile.AMask aMask);

		AniDBRequest Group(int gID);
		AniDBRequest Group(string gName);

		AniDBRequest GroupStatus(int aID, int state = 0);
	}
}
