using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ED2K;
using NUnit.Framework;

namespace libAniDB.NET.Tests
{
	//[TestFixture]
	//class ED2KTests
	//{
	//	private const string TestFilePath = @"";

	//	[Test]
	//	public void VerifyNewHashAlgorithm()
	//	{
	//		var newHash = (new ED2KHashTask(TestFilePath)).GetED2KHash();
	//		newHash.Wait();

	//		Debug.Print("Actual: " + newHash.Result);

	//		//Old hash seems broken on large files O_o
	//		var oldHash = new ED2KHash(TestFilePath, 2);
	//		oldHash.CalculateHash();

	//		Debug.Print("Expected: " + oldHash.Hash);
			
	//		Assert.That(newHash.Result, Is.EqualTo(oldHash.Hash));
	//	}
	//}
}
