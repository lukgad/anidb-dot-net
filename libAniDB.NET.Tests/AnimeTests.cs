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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace libAniDB.NET.Tests
{
	[TestFixture]
	internal class AnimeTests
	{
		private const string Request = "ANIME aid=1&amask=b2f0e0fc000000&s=xxxxx";

		private const string ResponseString =
			"230 ANIME\n1|1999-1999|TV Series|Space,Future,Plot Continuity,SciFi,Space Travel,Shipboard,Other Planet,Novel,Genetic Modification,Action,Romance,Military,Large Breasts,Gunfights,Adventure,Human Enhancement,Nudity|Seikai no Monshou|星界の紋章|Crest of the Stars||13|13|3|853|3225|756|110|875|11";

		private AniDBResponse _response;
		private Anime.AMask _aMask;
		private Anime _anime;

		[SetUp]
		public void SetUp()
		{
			_response = new AniDBResponse(Encoding.UTF8.GetBytes(ResponseString));

			//I think I'm probably overengineering this; why not just store the amask as 
			// a const string instead of the entire request? It's the only part (of the request)
			// that's relevant to testing the Anime class
			_aMask =
				new Anime.AMask(
					(Anime.AMask.AMaskValues)
					ulong.Parse(new Regex(@"(?<=amask\=)\w+((?=\&.+)|$)").Match(Request).Value, NumberStyles.HexNumber));

			_anime = new Anime(_response, _aMask);

			for (int i = 0; i < _response.DataFields[0].Length; i++)
				Console.WriteLine(String.Format("{0,3}", "[" + i) + "] " + _response.DataFields[0][i]);

			Console.WriteLine();

			Console.WriteLine(_aMask.MaskString);

			for (int i = 55; i >= 0; i--)
			{
				Anime.AMask.AMaskValues flag = (Anime.AMask.AMaskValues)(ulong)Math.Pow(2, i);
				if (_aMask.Mask.HasFlag(flag))
					Console.WriteLine(flag);
			}
		}

		[Test]
		public void AID()
		{
			Assert.That(_anime.AID, Is.EqualTo(int.Parse(_response.DataFields[0][0])));
		}

		[Test]
		public void Year()
		{
			Assert.That(_anime.Year, Is.EqualTo(_response.DataFields[0][1]));
		}

		[Test]
		public void Type()
		{
			Assert.That(_anime.Type, Is.EqualTo(_response.DataFields[0][2]));
		}

		[Test]
		public void CategoryList()
		{
			Assert.That(_anime.CategoryList, Is.Not.Null);

			string[] sa = _response.DataFields[0][3].Split(',');
			for (int i = 0; i < sa.Length; i++)
				Assert.That(_anime.CategoryList[i], Is.EqualTo(sa[i]));
		}

		[Test]
		public void RomanjiName()
		{
			Assert.That(_anime.RomanjiName, Is.EqualTo(_response.DataFields[0][4]));
		}

		[Test]
		public void KanjiName()
		{
			Assert.That(_anime.KanjiName, Is.EqualTo(_response.DataFields[0][5]));
		}

		[Test]
		public void EnglishName()
		{
			Assert.That(_anime.EnglishName, Is.EqualTo(_response.DataFields[0][6]));
		}

		[Test]
		public void OtherName()
		{
			//TODO: Fix this, this can (probably will) be null...
			Assert.That(_anime.OtherName, Is.Not.Null);
		}

		[Test]
		public void Episodes()
		{
			Assert.That(_anime.Episodes, Is.EqualTo(int.Parse(_response.DataFields[0][8])));
		}

		[Test]
		public void HighestEpisodeNumber()
		{
			Assert.That(_anime.HighestEpisodeNumber, Is.EqualTo(int.Parse(_response.DataFields[0][9])));
		}

		[Test]
		public void SpecialEpisodeCount()
		{
			Assert.That(_anime.SpecialEpisodeCount, Is.EqualTo(int.Parse(_response.DataFields[0][10])));
		}

		[Test]
		public void Rating()
		{
			Assert.That(_anime.Rating, Is.EqualTo(int.Parse(_response.DataFields[0][11])));
		}

		[Test]
		public void VoteCount()
		{
			Assert.That(_anime.VoteCount, Is.EqualTo(int.Parse(_response.DataFields[0][12])));
		}

		[Test]
		public void TempRating()
		{
			Assert.That(_anime.TempRating, Is.EqualTo(int.Parse(_response.DataFields[0][13])));
		}

		[Test]
		public void TempVoteCount()
		{
			Assert.That(_anime.TempVoteCount, Is.EqualTo(int.Parse(_response.DataFields[0][14])));
		}

		[Test]
		public void AverageReviewRating()
		{
			Assert.That(_anime.AverageReviewRating, Is.EqualTo(int.Parse(_response.DataFields[0][15])));
		}

		[Test]
		public void ReviewCount()
		{
			Assert.That(_anime.ReviewCount, Is.EqualTo(int.Parse(_response.DataFields[0][16])));
		}
	}

	[TestFixture]
	internal class AMaskTests
	{
		[Test]
		public void TestMaskStringLength()
		{
			Assert.That(new Anime.AMask(Anime.AMask.AMaskValues.None).MaskString.Length == 14);
		}

		//[Test]
		//public void TestAllMaskValues()
		//{
		//	Anime.AMask.AMaskValues allValues = Anime.AMaskNames.Keys.Aggregate(
		//																		Anime.AMask.AMaskValues.None,
		//																		(current, a) => current | a);

		//	const ulong expectedValue = (
		//									((ulong)(128 | 32 | 16 | 8 | 4 | 2 | 1) << 8 * 6) |
		//									((ulong)(128 | 64 | 32 | 16 | 8 | 4) << 8 * 5) |
		//									((ulong)(128 | 64 | 32 | 16 | 8 | 4 | 2 | 1) << 8 * 4) |
		//									((ulong)(128 | 64 | 32 | 16 | 8 | 4 | 2 | 1) << 8 * 3) |
		//									((ulong)(128 | 64 | 32 | 16 | 1) << 8 * 2) |
		//									((ulong)(128 | 64 | 32 | 16) << 8 * 1) |
		//									((128 | 64 | 32 | 16 | 8)));

		//	Console.WriteLine("Expected:\n" + ((ulong)allValues).ToString("x") + "\nActual:\n" + expectedValue.ToString("x"));

		//	Assert.That((ulong)allValues == expectedValue);
		//}
	}
}
