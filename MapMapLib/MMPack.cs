/*******************************************************************
 * Author: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MapMapLib
{
	public class MMPack
	{
		private Dictionary<string, MMGridSquare> sims;
		private Dictionary<string, int> simsCount;

		public MMPack()
		{
			this.sims = new Dictionary<string, MMGridSquare>();
			this.simsCount = new Dictionary<string, int>();
		}

		// Sims = simular tiles
		public void CountSims(List<string> sourceDir, int minx, int maxx, int miny, int maxy)
		{
			MMCellReader cellReader = new MMCellReader();
			foreach (string mapPath in sourceDir)
			{
				if (Directory.Exists(mapPath))
				{
					string[] packs = Directory.GetFiles(mapPath, "*.lotpack");
					foreach (string file in packs)
					{
						string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
						string[] fileparts = filename.Split(new Char[] { '.' });
						string[] nameparts = fileparts[0].Split(new Char[] { '_' });
						int cellx = Convert.ToInt32(nameparts[1]);
						int celly = Convert.ToInt32(nameparts[2]);
						if (cellx >= minx && cellx < maxx && celly >= miny && celly < maxy)
						{
							string headerFile = nameparts[1] + "_" + nameparts[2] + ".lotheader";
							string headerPath = mapPath + Path.DirectorySeparatorChar + headerFile;
							if (File.Exists(headerPath))
							{
								Console.WriteLine("Working on cell: {0} - {1}", nameparts[1], nameparts[2]);
								MMCellData mapdata = cellReader.Read(file, headerPath);
								for (int x = 0; x < 900; x++)
								{
									for (int y = 0; y < 900; y++)
									{
										MMGridSquare gs = mapdata.GetSquare(0, x, y);
										for (Int32 i = MMGridSquare.TOP; i <= MMGridSquare.BOTTOM; i++){
											List<MMTile> tiles = gs.GetTiles(i);
											if (tiles.Count > 0 && tiles.Count < 3){
												string id = tiles[0].tile + tiles[1].tile;
												if (this.simsCount.ContainsKey(id)){
													this.simsCount[id]++;
												} else {
													this.simsCount.Add(id, 1);
													this.sims.Add(id, gs);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

	}
}
