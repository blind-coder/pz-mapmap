/*******************************************************************
 * Author: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public Main()
		{
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			this.OutputDir = baseDir + OutputDir;
			if (!Directory.Exists(this.OutputDir)) Directory.CreateDirectory(this.OutputDir);

			this.tex = new MMTextures();
			this.mapsources = new List<string>();
			this.gfxsources = new List<string>();
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
			MMPlotter plotter = new MMPlotter(this.divider, this.tex, this.dolayers);
			foreach (string mapPath in this.mapsources) {
				if (Directory.Exists(mapPath)) {
					string[] packs = Directory.GetFiles(mapPath, "*.lotpack");
					foreach (string file in packs) {
						string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
						string[] fileparts = filename.Split(new Char[] { '.' });
						string[] nameparts = fileparts[0].Split(new Char[] { '_' });
						int cellx = Convert.ToInt32(nameparts[1]);
						int celly = Convert.ToInt32(nameparts[2]);
						if (cellx >= this.minX && cellx <= this.maxX  && celly >= this.minY && celly <= this.maxY) {
							string headerFile = nameparts[1] + "_" + nameparts[2] + ".lotheader";
							string headerPath = mapPath + Path.DirectorySeparatorChar + headerFile;
							if (File.Exists(headerPath)) { // lotpack
								Console.WriteLine("Working on cell: {0} - {1}", nameparts[1], nameparts[2]);
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
								plotter.PlotData(mapdata, this.OutputDir, cellx, celly);
							}
						}
					}

				}
			}
		}

		private void readTexturePacks()
		{
			foreach (string packPath in this.gfxsources) {
				this.tex.Load(packPath);
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
							string output = Path.GetFullPath(args[id + 1]);
							if (output != null)
							{
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
					}
				}
				Console.WriteLine("Boundaries: minx {0} maxx {1} miny {2} maxy {3}", this.minX, this.maxX, this.minY, this.maxY);
				//var choice = Console.ReadKey(true);
				Console.Clear();
				Console.WriteLine("Starting programm...");
				return true;
			}
			return false;
		}
	}
}
