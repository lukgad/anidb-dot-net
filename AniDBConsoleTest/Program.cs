using System;
using libAniDB.NET;

namespace AniDBConsoleTest
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			using (AniDB aniDB = new AniDB(9001))
			{
				Console.WriteLine("Before");

				var request = aniDB.Ping(true);
					Console.WriteLine("Request:");
					Console.WriteLine(request);
				request.Response.Wait();
				var response = request.Response.Result;
					Console.WriteLine("Response:");
					Console.WriteLine(response.OriginalString);
					Console.WriteLine(response);
				
				Console.WriteLine("After");

				Console.ReadKey();

				request = aniDB.ChangeEncoding("UTF-8");
					Console.WriteLine("Request:");
					Console.WriteLine(request);
					request.Response.Wait();
				response = request.Response.Result;
					Console.WriteLine("Response:");
					Console.WriteLine(response.OriginalString);
					Console.WriteLine(response);

				Console.ReadKey();

				//return;

				//var fileHash = new ED2KHash(args[0], 2);

				//new Thread(fileHash.CalculateHash).Start();

				//do
				//{
				//	Console.WriteLine(
				//					  "{0," + fileHash.ChunkCount.ToString(CultureInfo.CurrentCulture).Length + 
				//					  "}/{1," + fileHash.ChunkCount.ToString(CultureInfo.CurrentCulture).Length +
				//					  "}", fileHash.Complete, fileHash.ChunkCount);
				//	Thread.Sleep(250);
				//} while (fileHash.Complete < fileHash.ChunkCount);

				//Console.WriteLine(fileHash.Hash);

				//Console.ReadKey();

				//ManualResetEvent waitHandle = new ManualResetEvent(false);

				//aniDB.Auth("aaron552", args[1], (res) =>
				//									   {
				//										Console.WriteLine(res.ToString());
				//										waitHandle.Set();
				//									   });

				

				//waitHandle.WaitOne();
				//waitHandle.Reset();

				//if (((AniDB)aniDB).SessionKey == "")
				//{
				//	Console.ReadKey();
				//	return;
				//}

				//AniDBFile.FMask fMask = new AniDBFile.FMask(AniDBFile.FMask.FMaskValues.AID |
				//											AniDBFile.FMask.FMaskValues.EID |
				//											AniDBFile.FMask.FMaskValues.GID |
				//											AniDBFile.FMask.FMaskValues.VideoCodec |
				//											AniDBFile.FMask.FMaskValues.VideoBitrate |
				//											AniDBFile.FMask.FMaskValues.AudioCodecs |
				//											AniDBFile.FMask.FMaskValues.AudioBitrates |
				//											AniDBFile.FMask.FMaskValues.DubLanguage |
				//											AniDBFile.FMask.FMaskValues.SubLanguage);

				//AniDBFile.AMask aMask = new AniDBFile.AMask(AniDBFile.AMask.AMaskValues.KanjiName |
				//											AniDBFile.AMask.AMaskValues.RomanjiName |
				//											AniDBFile.AMask.AMaskValues.EnglishName |
				//											AniDBFile.AMask.AMaskValues.ShortNameList |
				//											AniDBFile.AMask.AMaskValues.GroupShortName |
				//											AniDBFile.AMask.AMaskValues.GroupName |
				//											AniDBFile.AMask.AMaskValues.EpNo |
				//											AniDBFile.AMask.AMaskValues.TotalEpisodes |
				//											AniDBFile.AMask.AMaskValues.HighestEpisodeNumber |
				//											AniDBFile.AMask.AMaskValues.Year);

				//aniDB.File((new FileInfo(args[0])).Length, fileHash.Hash, fMask, aMask,
				//		   (res) =>
				//		   {
				//			try
				//			{
				//				Console.WriteLine(new AniDBFile(res, fMask, aMask));
				//			}
				//			catch (Exception e)
				//			{
				//				Console.WriteLine(e.ToString());
				//				Console.WriteLine(res);
				//			}
				//			waitHandle.Set();
				//		   });

				//waitHandle.WaitOne();
				//waitHandle.Reset();

				//aniDB.Logout((res) =>
				//			 {
				//				Console.WriteLine(res.ToString());
				//				waitHandle.Set();
				//			 });

				//waitHandle.WaitOne();

				//Console.ReadKey();
			}
		}
	}
}
