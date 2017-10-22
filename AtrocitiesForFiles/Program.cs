using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace AtrocitiesForFiles {
	internal static class Program {
		private const string ExportFolder = "Atrocities";
		private static void Main(string[] args) {
			Console.Title="Atrocities for files";
			if(args==null) {
				Console.WriteLine("No arguments (for file paths) provdied)");
				goto End;
			}
			switch(args.Length) {
				default:
					List<string> validFilePaths = new List<string>();
					List<string> usedFiles = new List<string>();
					foreach(string arg in args) {
						if(File.Exists(arg)) {
							validFilePaths.Add(arg);
						}
					}
					Directory.CreateDirectory(ExportFolder);
					foreach(string file in validFilePaths) {
						string name = Path.GetFileNameWithoutExtension(file);
						string extension = Path.GetExtension(file);
						string saveName = name;
						int counter = 1;
						while(usedFiles.Contains(saveName)) {
							saveName=$"{name} {++counter}";
						}
						usedFiles.Add(saveName);
						ConvertFileByBytes(file,$@"{ExportFolder}\{saveName}{extension}");
					}
					break;
				case 0:
					Console.WriteLine("An argument was provided, but it is empty.");
					break;
			}
			End:
			Console.Write($"Press any key to exit..");
			Console.ReadKey(true);
		}
		private static void ConvertFileByBytes(string sourceFile,string savePath) {
			Console.WriteLine($"Starting byte conversion for {sourceFile}");
			byte[] bytes = File.ReadAllBytes(sourceFile);
			int dimension = (int)Math.Ceiling(Math.Sqrt(bytes.Length));
			int totalSize = dimension*dimension;
			Bitmap output = new Bitmap(dimension,dimension);
			int index = 0;
			while(index<bytes.Length) {
				Color color = Color.FromArgb(byte.MaxValue,bytes[index],bytes[index],bytes[index]);
				output.SetPixel(index%dimension,index/dimension,color);
				index+=1;
			}
			while(index<totalSize) {
				output.SetPixel(index%dimension,index/dimension,Color.Transparent);
				index+=1;
			}
			output.Save($"{savePath}.png",ImageFormat.Png);
			Console.WriteLine($"Finished byte conversion for {sourceFile}");
		}
		private static void ConvertFileByBits(string sourceFile,string savePath) {
			Console.WriteLine($"Starting bit conversion for {sourceFile}");
			BitArray bits = new BitArray(File.ReadAllBytes(sourceFile));
			int dimension = (int)Math.Ceiling(Math.Sqrt(bits.Count));
			int totalSize = dimension*dimension;
			Bitmap output = new Bitmap(dimension,dimension);
			int index = 0;
			while(index<bits.Count) {
				Color color = bits.Get(index) ? Color.White : Color.Black;
				output.SetPixel(index%dimension,index/dimension,color);
				index+=1;
			}
			while(index<totalSize) {
				output.SetPixel(index%dimension,index/dimension,Color.Gray);
				index+=1;
			}
			output.Save($"{savePath}.png",ImageFormat.Png);
			Console.WriteLine($"Finished bit conversion for {sourceFile}");
		}
	}
}
