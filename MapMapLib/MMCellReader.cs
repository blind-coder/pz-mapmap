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
	public class MMCellReader
	{
		private MMCellData cellData;

		public MMCellReader()
		{
			this.cellData = new MMCellData();
		}

		public MMCellData Read(string datafile, string headerfile)
		{
			//this.cellData.Reset();
			this.cellData = new MMCellData();
			List<string> tiles = new List<string>();
			if (File.Exists(headerfile))
			{
				using (BinaryReader binReader = new BinaryReader(File.Open(headerfile, FileMode.Open)))
				{
					tiles = this.ReadHeader(binReader);
				}
			}
			if (tiles.Count > 0 && File.Exists(datafile))
			{
				using (BinaryReader binReader = new BinaryReader(File.Open(datafile, FileMode.Open)))
				{
					this.cellData = this.ReadPack(binReader, tiles);
				}
			}
			return this.cellData;
		}

		private MMCellData ReadPack(BinaryReader binReader, List<string> tiles)
		{
			MMGridSquare gs;
			for (int cx = 0; cx < 30; cx++)
			{
				for (int cy = 0; cy < 30; cy++)
				{
					int chunkwx = cx * 10;
					int chunkwy = cy * 10;
					int skip = 0;
					int index = cx * 30 + cy;
					binReader.BaseStream.Seek(4 + index * 8, SeekOrigin.Begin);
					int pos = binReader.ReadInt32();
					binReader.BaseStream.Seek(pos, SeekOrigin.Begin);
					for (int z = 0; z < 8; z++)
					{
						for (int x = 0; x < 10; x++)
						{
							for (int y = 0; y < 10; y++)
							{
								if (skip > 0)
								{
									skip--;
								}
								else
								{
									int count = binReader.ReadInt32();
									if (count == -1)
									{
										skip = binReader.ReadInt32();
										if (skip > 0)
										{
											skip--;
											continue;
										}
									}
									if (count > 1)
									{
										int room = binReader.ReadInt32();
										gs = this.cellData.SetSquare(chunkwx + x, chunkwy + y, z);
										gs.SetRoomID(room);
										for (int n = 1; n < count; n++)
										{
											int d = binReader.ReadInt32();

											string tilename = tiles[d];
											gs.AddTile(tilename);
										}
									}
								}
							}
						}
					}
				}
			}
			return this.cellData;
		}

		private string ReadLine(BinaryReader binReader)
		{
			StringBuilder result = new StringBuilder();
			char lastChar = binReader.ReadChar();
			while (lastChar != '\n')
			{
				result.Append(lastChar);
				lastChar = binReader.ReadChar();
			}
			return result.ToString();
		}

		private List<string> ReadHeader(BinaryReader binReader)
		{
			/*int version =*/ binReader.ReadInt32();
			int max = binReader.ReadInt32();
			List<string> tiles = new List<string>();

			for (int i = 0; i < max; ++i)
			{
				string tileName = this.ReadLine(binReader);
				tiles.Add(tileName);
			}
			return tiles;
		}

	}
}
