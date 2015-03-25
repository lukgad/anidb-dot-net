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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace libAniDB.NET
{
	/// <summary>
	/// Implementation of the IAniDB interface
	/// </summary>
	public partial class AniDB : IDisposable
	{
		public const int ProtocolVersion = 3;

		public string SessionKey { get; private set; }

		public int Timeout { get; set; }

		private readonly ConcurrentDictionary<string, AniDBRequest> _sentRequests;

		private readonly TokenBucket<AniDBRequest> _sendBucket;

		private const uint MinSendDelay = 2000;
		private const uint AvgSendDelay = 4000;
		private const int BurstLength = 60000;

		private readonly Encoding _encoding;

		public readonly string ClientName;
		public readonly int ClientVer;

		private readonly UdpClient _udpClient;

		public AniDB(int localPort, string clientName = "libanidbdotnet", int clientVer = 1, Encoding encoding = null,
		             string remoteHostName = "api.anidb.net", int remotePort = 9000)
		{
			Timeout = 20000;

			ClientName = clientName;
			ClientVer = clientVer;

			_encoding = encoding ?? Encoding.ASCII;

			_sentRequests = new ConcurrentDictionary<string, AniDBRequest>();

			_udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, localPort));
			_udpClient.Connect(remoteHostName, remotePort);

			SessionKey = "";

			RecievePackets();

			//Magical maths that turns the burst length into a number of tokens (in this case, 30)
			_sendBucket = new TokenBucket<AniDBRequest>(MinSendDelay, AvgSendDelay,
			                                            BurstLength / (AvgSendDelay - MinSendDelay), true,
														SendPacket);
		}

		private AniDBRequest QueueCommand(string command, IEnumerable<KeyValuePair<string, string>> parValues)
		{
			if (SessionKey != "")
				parValues = parValues.ToDictionary(p => p.Key, p => p.Value);
			
			((Dictionary<string, string>) parValues).Add("s", SessionKey);

			var request = new AniDBRequest(command, parValues);

			if(_sentRequests.ContainsKey(request.Tag))
				throw new ArgumentException("A request with that tag has already been sent");

			_sendBucket.Input(request);

			return request;
		}
		
		private AniDBRequest QueueCommand(string command, params KeyValuePair<string, string>[] args)
		{
			IEnumerable<KeyValuePair<string, string>> parValues = args;
			return QueueCommand(command, parValues);
		}

		private void SendPacket(AniDBRequest request)
		{
			//In theory this should never fail (GUIDs are supposed to be globally unique)
			if (!_sentRequests.TryAdd(request.Tag, request))
			{
				request.OnResponse(null);
				return;
			}

			byte[] requestBytes = request.ToByteArray(_encoding);

			_udpClient.Send(requestBytes, requestBytes.Count());
			Debug.Print(requestBytes.ToString());

			request.Timeout.Elapsed += (o, a) =>
				                           {
					                           AniDBRequest r;
					                           _sentRequests.TryRemove(request.Tag, out r);
				                           };
			request.Timeout.Interval = Timeout;
			request.Timeout.Enabled = true;
		}

		private async void RecievePackets()
		{
			while(true)
			{
				try
				{
					var result = await _udpClient.ReceiveAsync();

					byte[] responseBytes = result.Buffer;
					
					if (responseBytes == null)
						continue;

					var response = new AniDBResponse(responseBytes, _encoding);
					new Task(() => HandleResponse(response)).Start();
				}
				catch(ObjectDisposedException ode)
				{
					break;
				}
			}
		}

		public void HandleResponse(AniDBResponse response)
		{
			if (response.Code == AniDBResponse.ReturnCode.LOGIN_ACCEPTED ||
				response.Code == AniDBResponse.ReturnCode.LOGIN_ACCEPTED_NEW_VERSION)
					SessionKey = response.ReturnString.Split(new [] {' '}, 2)[0];

			if (response.Code == AniDBResponse.ReturnCode.LOGGED_OUT ||
				response.Code == AniDBResponse.ReturnCode.LOGIN_FAILED ||
				response.Code == AniDBResponse.ReturnCode.LOGIN_FIRST)
					SessionKey = "";

			AniDBRequest request;
			if (_sentRequests.TryRemove(response.Tag, out request))
			{
				request.Timeout.Stop();
				request.OnResponse(response);
			}
		}

		public void Dispose()
		{
			_udpClient.Close();
		}
	}
}
