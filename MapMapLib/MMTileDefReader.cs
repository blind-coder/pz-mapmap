/*******************************************************************
 * Author: Benjamin "blindcoder" Schieder
 * Based on work by: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MapMapLib {
	public class MMTileDefReader {
		private BinaryReader binReader;
		private Int32 byteRead;
		public Dictionary<Int32, String> tileDefs;

		public MMTileDefReader() {
			this.byteRead = 0;
			this.tileDefs = new Dictionary<Int32, String>();
		}

		public Dictionary<Int32, String> Load(string datafile) {/*{{{*/
			if (File.Exists(datafile)) {
				Console.WriteLine("Reading tiledef {0}", datafile);
				string[] f;
				f = datafile.Split(new Char[] {'/'});
				f = f[f.Length-1].Split(new Char[] {'_'});

				this.binReader = new BinaryReader(File.Open(datafile, FileMode.Open));
				this.ReadPack(Convert.ToInt32(f[0]));
			}
			return tileDefs;
		}/*}}}*/

		private Byte ReadByte(){/*{{{*/
			byteRead++;
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return binReader.ReadByte();
		}/*}}}*/
		private Int16 ReadInt16(){/*{{{*/
			byteRead+=2;
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return binReader.ReadInt16();
		}/*}}}*/
		private Int32 ReadInt32(){/*{{{*/
			byteRead+=4;
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return binReader.ReadInt32();
		}/*}}}*/
		private Int64 ReadInt64(){/*{{{*/
			byteRead+=8;
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return binReader.ReadInt64();
		}/*}}}*/
		private Single ReadSingle(){/*{{{*/
			byteRead+=4;
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return binReader.ReadSingle();
		}/*}}}*/
		private Double ReadDouble(){/*{{{*/
			byteRead+=8;
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return binReader.ReadDouble();
		}/*}}}*/
		private string ReadString(){/*{{{*/
			String retVal = "";
			Char c = ' ';
			while (c != '\n'){
				c = binReader.ReadChar();
				byteRead++;
				if (c != '\n'){
					retVal = String.Concat(retVal, c);
				}
			}
			// Console.WriteLine("Read String: '{0}'", retVal);
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return retVal;
		}/*}}}*/

		private void ReadPack(Int32 fileNumber) {
			Console.WriteLine("Filenumber: {0}", fileNumber);
			ReadInt32(); // TDEF
			Console.WriteLine("Tiledef Version: {0}", ReadInt32());
			Int32 numTileSheets = ReadInt32();
			Console.WriteLine("Got {0} tilesheets", numTileSheets);
			
			for (Int32 idxTileSheet = 0; idxTileSheet < numTileSheets; idxTileSheet++){
				String name = ReadString();
				// Console.WriteLine("Name: {0}", name);
				ReadString();
				// Console.WriteLine("ImageName: {0}", imageName);
				ReadInt32(); // wTiles
				ReadInt32(); // hTiles
				Int32 tilesetNumber = ReadInt32();
				// Console.WriteLine("Tileset number: {0}", tilesetNumber);
				Int32 numTiles = ReadInt32();
				// Console.WriteLine("Num tiles: {0}", numTiles);

				for (Int32 idxTile = 0; idxTile < numTiles; idxTile++){
					Int32 spritenumber = -1;
					if (fileNumber < 2){
						spritenumber = fileNumber * 100 * 1000 + 10000 + tilesetNumber * 1000 + idxTile;
					} else {
						spritenumber = fileNumber * 512 * 512 + tilesetNumber * 512 + idxTile;
					}
					/*if (spritenumber == 111000){
						Console.WriteLine("{0} = {1} * 100 * 1000 + 10000 + {2} * 1000 + {3}", spritenumber, fileNumber, tilesetNumber, idxTile);
						Console.WriteLine("Sprite name '{0}_{1}' ID {2}", name, idxTile, spritenumber);
					}*/

					if (this.tileDefs.ContainsValue(name+"_"+idxTile)){
						Int32 key = this.tileDefs.FirstOrDefault(x => x.Value == name+"_"+idxTile).Key;
						// Console.WriteLine("Duplicate texture {0}! Ignore ID {1} use instead ID {2}", name+"_"+idxTile, spritenumber, key);
					}
					if (this.tileDefs.ContainsKey(spritenumber)){
						String s;
						this.tileDefs.TryGetValue(spritenumber, out s);
						// Console.WriteLine("Duplicate key {0}! Old: {1} New: {2}", spritenumber, s, name+"_"+idxTile);
						this.tileDefs.Remove(spritenumber);
					}
					this.tileDefs.Add(spritenumber, name+"_"+idxTile);

					Int32 nProps = ReadInt32();
					for (Int32 idxProp = 0; idxProp < nProps; idxProp++){
						/* String prop = */ ReadString();
						/* String val = */ ReadString();
						// Console.WriteLine("{0} = {1}", prop, val);
					}
				}
			}
			return;
		}
	}
}
