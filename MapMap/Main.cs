/*******************************************************************
 * Author: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading;
using MapMapLib;
using System.IO;

namespace MapMap
{
	class Main
	{
		private string OutputDir = "output" + Path.DirectorySeparatorChar;
		private MMTextures tex;
		private List<string> mapsources;
		private List<string> gfxsources;
		private List<string> gfxdirs;
		private List<string> tilesources;
		private Dictionary<Int32, String> tileDefs;
		private bool dolayers = true;
		private bool packMode = false;
		private bool unpack = false;
		private int minX = -99999;
		private int maxX = 99999;
		private int minY = -99999;
		private int maxY = 99999;
		private int divider = 3;
		private bool bigtree = true;
		private static int numThreads = 0;
		private int maxThreads = 1;
		private int scale = 1;

		private MMCellData childMapData;

		public Main()
		{
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			this.OutputDir = baseDir + OutputDir;
			if (!Directory.Exists(this.OutputDir)) Directory.CreateDirectory(this.OutputDir);

			this.tex = new MMTextures();
			this.mapsources = new List<string>();
			this.gfxsources = new List<string>();
			this.gfxdirs = new List<string>();
			this.tilesources = new List<string>();
			this.tileDefs = new Dictionary<Int32, String>();
		}

		public void Run(string[] args)
		{
			if (this.parseArgs(args))
			{
				this.readTexturePacks();
				this.readTileDefs();
				if (this.packMode == true)
					this.packMapData();
				else
					this.parseMapData();
				Console.WriteLine("Done...");
			}
			else
			{
				Console.WriteLine("No arguments supplied, aborting...");
			}
		}

		private void packMapData()
		{
			MMPack packer = new MMPack();
			packer.CountSims(this.mapsources, this.minX, this.maxX, this.minY, this.maxY);
		}

		private void parseMapData()
		{
			MMCellReader cellReader = new MMCellReader();
			MMBinReader binReader = new MMBinReader();

			List<int>[] waterCells = new List<int>[60]; // TODO временно
			waterCells[0] = new List<int> { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[1] = new List<int> { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[2] = new List<int> { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[3] = new List<int> { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[4] = new List<int> { 0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[5] = new List<int> { 0, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[6] = new List<int> { 0, 1, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[7] = new List<int> { 0, 1, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[8] = new List<int> { 0, 1, 2, 7, 8, 9, 10, 11, 12 };
			waterCells[9] = new List<int> { 0, 1, 5, 9, 10, 11, 12 };
			waterCells[10] = new List<int> { 0, 1, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			waterCells[11] = new List<int> { 0, 1, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			waterCells[12] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 };
			waterCells[13] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 };
			waterCells[14] = new List<int> { 0, 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 };
			waterCells[15] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 };
			waterCells[16] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			waterCells[17] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			waterCells[18] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			waterCells[19] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			waterCells[20] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[21] = new List<int> { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[22] = new List<int> { 0, 1, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[23] = new List<int> { 0, 1, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[24] = new List<int> { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[25] = new List<int> { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[26] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
			waterCells[27] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[28] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			waterCells[29] = new List<int> { 0, 1, 2, 3, 6, 7, 8, 9, 11, 12, 13, 14 };
			waterCells[30] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14 };
			waterCells[31] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
			waterCells[32] = new List<int> { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11 };
			waterCells[33] = new List<int> { 0, 1, 2, 3, 7, 8, 9, 10 };
			waterCells[34] = new List<int> { 0, 1, 2, 7, 8, 9, 10, 11, 12 };
			waterCells[35] = new List<int> { 0, 1, 2, 7, 8, 9, 10, 11, 12 };
			waterCells[36] = new List<int> { 0, 1, 2, 8, 9, 10, 11 };
			waterCells[37] = new List<int> { 0, 1 };
			waterCells[38] = new List<int> { 0, 1 };
			waterCells[39] = new List<int> { 0, 1, 4, 5 };
			waterCells[40] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
			waterCells[41] = new List<int> { 0, 1, 3, 6 };
			waterCells[42] = new List<int> { 0, 1 };
			waterCells[43] = new List<int> { 0, 1 };
			waterCells[44] = new List<int> { 0, 1, 2 };
			waterCells[45] = new List<int> { 0, 1, 2, 3 };
			waterCells[46] = new List<int> { 0, 1, 2, 3 };
			waterCells[47] = new List<int> { 0, 1 };
			waterCells[48] = new List<int> { 0, 1 };
			waterCells[49] = new List<int> { 0 };
			waterCells[50] = new List<int> { 0 };
			waterCells[51] = new List<int> { 0 };
			waterCells[53] = new List<int> { 0 };
			waterCells[54] = new List<int> { 0 };
			waterCells[55] = new List<int> { 0 };

			// MMPlotter plotter = new MMPlotter(this.divider, this.tex, this.dolayers, this.bigtree);
			foreach (string mapPath in this.mapsources) {
				if (Directory.Exists(mapPath)) {
					string[] packs = Directory.GetFiles(mapPath, "*.lotpack");
					packs = packs.OrderBy(x => x).ToArray();
					foreach (string file in packs) {
						string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
						string[] fileparts = filename.Split(new Char[] { '.' });
						string[] nameparts = fileparts[0].Split(new Char[] { '_' });
						int cellx = Convert.ToInt32(nameparts[1]);
						int celly = Convert.ToInt32(nameparts[2]);
						if (waterCells[cellx] != null && waterCells[cellx].BinarySearch(celly) > 0) {
							Console.WriteLine("Skipping cell: {0} - {1}", cellx, celly);
							continue;
						}
						if (cellx >= this.minX && cellx <= this.maxX  && celly >= this.minY && celly <= this.maxY) {
							string headerFile = nameparts[1] + "_" + nameparts[2] + ".lotheader";
							string headerPath = mapPath + Path.DirectorySeparatorChar + headerFile;
							if (File.Exists(headerPath)) { // lotpack
								Console.WriteLine("Working on cell: {0} - {1}", cellx, celly);
								bool allSubCellExist = true;
								for (int subx = 0; subx < this.divider; subx++)
								{
									for (int suby = 0; suby < this.divider; suby++)
									{
										for (int z = 0; z < 8; z++)
										{
											if (this.dolayers)
											{
												if (!File.Exists(this.OutputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_layer_" + z + ".png"))
												{
													allSubCellExist = false;
													goto LoopEnd;
												}
											}
											else
											{
												if (!File.Exists(this.OutputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_full.png"))
												{
													allSubCellExist = false;
													goto LoopEnd;
												}
											}
										}
									}
								}
								LoopEnd: if (allSubCellExist) {
									Console.WriteLine("All subcells exist, skipping cell: {0} - {1}", cellx, celly);
									continue;
								}
								MMCellData mapdata = cellReader.Read(file, headerPath);

								foreach (string savePath in this.mapsources) {
									string[] saves = Directory.GetFiles(savePath, "map_*_*.bin");
									foreach (string binfile in saves){ // map_*_*.bin
										string binfilename = binfile.Substring(binfile.LastIndexOf(Path.DirectorySeparatorChar) + 1);
										string[] binfileparts = binfilename.Split(new Char[] { '.' });
										string[] binnameparts = binfileparts[0].Split(new Char[] { '_' });
										int gsX = Convert.ToInt32(binnameparts[1]);
										int gsY = Convert.ToInt32(binnameparts[2]);
										int binToCellX = (int)Math.Floor(gsX * 10D / 300);
										int binToCellY = (int)Math.Floor(gsY * 10D / 300);
										if (binToCellX == cellx && binToCellY == celly){
											// Console.WriteLine("Working on map_bin: {0} - {1}", binnameparts[1], binnameparts[2]);
											binReader.Read(binfile, mapdata, tileDefs, gsX * 10 % 300, gsY * 10 % 300);
										}
									}
								}

								while (MapMap.Main.numThreads >= maxThreads){
									Thread.Sleep(500);
								}
								MapMap.Main.numThreads++;
								Console.WriteLine("Threads: {0}/{1}", MapMap.Main.numThreads, maxThreads);
								MMPlotter plotter = new MMPlotter(this.divider, this.tex, this.dolayers, this.bigtree, this.scale);
								//ThreadStart childref = new ThreadStart(this.RunPlotter);
								//Thread childThread = new Thread(childref);
								Thread childThread = new Thread(() => RunPlotter(plotter, mapdata, this.OutputDir, cellx, celly));
								childThread.Start();
							}
						}
					}

				}
			}
		}

		private static void RunPlotter(MMPlotter childPlotter, MMCellData childMapData, String OutputDir, int childCellX, int childCellY){
			Console.WriteLine("Thread {0}x{1} started", childCellX, childCellY);
			childPlotter.PlotData(childMapData, OutputDir, childCellX, childCellY);
			MapMap.Main.numThreads--;
			Console.WriteLine("Thread {0}x{1} finished", childCellX, childCellY);
		}

		private void readTexturePacks()
		{
			foreach (string packPath in this.gfxsources) {
				this.tex.Load(packPath);
			}
			foreach (string dir in this.gfxdirs) {
				this.tex.LoadTextureDir(dir);
			}
		}

		private void readTileDefs() {
			MMTileDefReader t = new MMTileDefReader();
			foreach (string packPath in this.tilesources) {
				string[] tiledefs  = Directory.GetFiles(packPath, "*.tiles");
				foreach (string file in tiledefs){
					t.Load(file);
				}
			}
			this.tileDefs = t.tileDefs;
		}

		private bool parseArgs(string[] args)
		{
			if (args.Length > 1)
			{
				int len = Convert.ToInt32(Math.Floor(args.Length / 2.0));
				for (int i = 0; i < len; i++)
				{
					int id = i * 2;
					switch (args[id])
					{
						case "-output":
							// TODO проверка на наличие слэша в конце
							string output = Path.GetFullPath(args[id + 1]);
							if (output != null)
							{
								if (!output.EndsWith("\\"))
								{
									output = output + "\\";
								}
								if (!Directory.Exists(this.OutputDir)) Directory.CreateDirectory(this.OutputDir);
								this.OutputDir = output;
							}
							break;
						case "-mapsource":
							this.mapsources.Add( Path.GetFullPath(args[id + 1]) );
							break;
						case "-gfxsource":
							this.gfxsources.Add( Path.GetFullPath(args[id + 1]) );
							break;
						case "-gfxdir":
							this.gfxdirs.Add(Path.GetFullPath(args[id + 1]));
							break;
						case "-tiledef":
							this.tilesources.Add(Path.GetFullPath(args[id + 1]));
							break;
						case "-dolayers":
							this.dolayers = Convert.ToBoolean(args[id + 1]);
							break;
						case "-packmode":
							this.packMode = Convert.ToBoolean(args[id + 1]);
							break;
						case "-unpack":
							this.unpack = Convert.ToBoolean(args[id + 1]);
							if (this.unpack){
								//
							}
							break;
						case "-divider":
							int div = Convert.ToInt32(args[id + 1]);
							if (div >= 1 && div <= 30)
							{
								decimal d = 30 / Convert.ToInt32(args[id + 1]);
								if (d % 1 == 0)
									this.divider = div;
							}
							break;
						case "-bigtree":
							this.bigtree = Convert.ToBoolean(args[id + 1]);
							break;
						case "-maxthreads":
							this.maxThreads = Convert.ToInt32(args[id + 1]);
							break;
						case "-minx":
							this.minX = Convert.ToInt32(args[id + 1]);
							break;
						case "-maxx":
							this.maxX = Convert.ToInt32(args[id + 1]);
							break;
						case "-miny":
							this.minY = Convert.ToInt32(args[id + 1]);
							break;
						case "-maxy":
							this.maxY = Convert.ToInt32(args[id + 1]);
							break;
						case "-scale":
							this.scale = Convert.ToInt32(args[id + 1]);
							break;
					}
				}
				Console.Clear();
				Console.WriteLine("Boundaries: minx {0} maxx {1} miny {2} maxy {3}", this.minX, this.maxX, this.minY, this.maxY);
				Console.WriteLine("Threads: {0}", this.maxThreads);
				Console.WriteLine("Starting programm...");
				return true;
			}
			return false;
		}
	}
}
