using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace CrimesAgainstJavaScript {
	internal static class Program {
		private static void Main(string[] args) {
			if(args==null) {
				Console.WriteLine("No arguments (for file path) provdied!");
				goto End;
			}
			switch(args.Length) {
				case 0:
					Console.WriteLine("Please supply a file path in this program's runtime arguments!");
					goto End;
				case 1:
					if(File.Exists(args[0])) {
						Bitmap bitmap = new Bitmap(args[0]);
						int dimension = bitmap.Width;
						if(dimension!=bitmap.Height) {
							Console.WriteLine("The supplied image isn't square and at the time of writing this, it is impossible for the compiler to export an image that isn't square. If something has since changed, Sam is an idiot for not having the foresight that he would inevitably change something. Although, it appears he did so he is just being lazy and negligent.");
							goto End;
						}


						List<byte> bytes = new List<byte>();

						int byteLength = dimension*dimension;
						int index = 0;
						while(index < byteLength) {
							Color color = bitmap.GetPixel(index%dimension,index/dimension);
							if(color.A==byte.MinValue) {
								break;
							}
							bytes.Add(color.R);
							index+=1;
						}
						string saveName = Path.GetFileNameWithoutExtension(args[0])+".RENAME ME";
						File.WriteAllBytes(saveName,bytes.ToArray());
						Console.WriteLine($"Saved {args[0]} as {saveName}");
					} else {
						Console.WriteLine("Somehow (because you're an idiot) this file doesn't exist!");
					}
					goto End;
				default:
					Console.WriteLine("I can only convert one file at a time!");
					goto End;
			}

			End:
			Console.Write("Press any key to exit..");
			Console.ReadKey(true);
		}
	}
}
