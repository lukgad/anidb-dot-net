using System.Text;
using NUnit.Framework;

namespace libAniDB.NET.Tests
{
	[TestFixture]
	class AniDBTests
	{
		[Test]
		public void SetCorrectSessionKey()
		{
			AniDB aniDB = new AniDB(9000);

			aniDB.HandleResponse(new AniDBResponse(Encoding.ASCII.GetBytes("abc123 200 Jxqxb LOGIN ACCEPTED"), Encoding.ASCII));

			Assert.That(aniDB.SessionKey, Is.EqualTo("Jxqxb"));
		}
	}
}
