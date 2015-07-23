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
	public class MMBinReader {
		// private MMCellData cellData;
		private BinaryReader binReader;
		private Int32 byteRead;
		private MMCellData celldata;
		private MMGridSquare currentGS;
		private Dictionary<Int32, String> tileDefs;
		private Int32 offX;
		private Int32 offY;
		private List<Int32> delayedSprites;

		public MMBinReader() {
			// this.cellData = new MMCellData();
			byteRead = 0;
		}

		public void Read(string datafile, MMCellData celldata, Dictionary<Int32, String> tileDefs, Int32 offX, Int32 offY) {
			this.delayedSprites = new List<Int32>();
			this.celldata = celldata;
			this.tileDefs = tileDefs;
			this.offX = offX;
			this.offY = offY;
			if (File.Exists(datafile)) {
				this.binReader = new BinaryReader(File.Open(datafile, FileMode.Open));
				this.ReadPack();
			}
			return;
		}

		// map_*_*.bin are big-endian, need to convert here
		private Byte ReadByte(){/*{{{*/
			byteRead++;
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return binReader.ReadByte();
		}/*}}}*/
		private Int16 ReadInt16(){/*{{{*/
			byteRead+=2;
			byte[] a16 = new byte[2];
			a16 = binReader.ReadBytes(2);
			Array.Reverse(a16);
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return BitConverter.ToInt16(a16, 0);
		}/*}}}*/
		private Int32 ReadInt32(){/*{{{*/
			byteRead+=4;
			byte[] a32 = new byte[4];
			a32 = binReader.ReadBytes(4);
			Array.Reverse(a32);
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return BitConverter.ToInt32(a32, 0);
		}/*}}}*/
		private Int64 ReadInt64(){/*{{{*/
			byteRead+=8;
			byte[] a64 = new byte[8];
			a64 = binReader.ReadBytes(8);
			Array.Reverse(a64);
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return BitConverter.ToInt64(a64, 0);
		}/*}}}*/
		private Single ReadSingle(){/*{{{*/
			byteRead+=4;
			byte[] a32 = new byte[4];
			a32 = binReader.ReadBytes(4);
			Array.Reverse(a32);
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return BitConverter.ToSingle(a32, 0);
		}/*}}}*/
		private Double ReadDouble(){/*{{{*/
			byteRead+=8;
			byte[] a64 = new byte[8];
			a64 = binReader.ReadBytes(8);
			Array.Reverse(a64);
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return BitConverter.ToDouble(a64, 0);
		}/*}}}*/
		private string ReadString(){/*{{{*/
			Int16 len = ReadInt16();
			byteRead+=len;
			// Console.WriteLine("Got string length: {0}", len);
			String retVal = "";
			for (; len > 0; len--){
				retVal = String.Concat(retVal, binReader.ReadChar());
				Console.WriteLine(retVal);
			}
			// Console.WriteLine("Read String: '{0}'", retVal);
			// Console.WriteLine("Byte read now: {0}", byteRead);
			return retVal;
		}/*}}}*/

		private void ReadContainer(){/*{{{*/
			ReadString(); // type
			ReadByte(); // explored
			Int32 numItems = ReadInt16();
			// Console.WriteLine("Contains {0} items", numItems);
			for (; numItems > 0; numItems--){/*{{{*/
				Int16 dataLen = ReadInt16(); // dataLen
				binReader.ReadBytes(dataLen);
				/*
				ReadString(); // Base.Axe

				// InventoryItem.load()
				ReadInt32(); // uses
				ReadInt64(); // ID
				if (ReadByte() == 1){
					// Console.WriteLine("Has Kahlua Table");
					ReadKahluaTable();
				}
				if (ReadByte() == 1){
					ReadSingle(); // UseDelta
				}
				ReadByte(); // Condition
				ReadByte(); // Activated
				ReadInt16(); // HaveBeenRepaired
				if (ReadByte() != 0){
					ReadString(); // Name
				}
				if (ReadByte() == 1){
					Int32 numBytes = ReadInt32(); // size
					for (; numBytes > 0; numBytes--){ // byteData
						ReadByte();
					}
				}
				Int32 extraItems = ReadInt32();
				for (; extraItems > 0; extraItems--){
					ReadString();
				}
				ReadByte(); // Customname
				ReadSingle(); // Weight
				ReadInt32(); // KeyID
				ReadByte(); // TaintedWater
				ReadInt32(); // RemoteControlID
				ReadInt32(); // RemoteRange
				*/
			}/*}}}*/
			ReadByte(); // HasBeenLooted
		}/*}}}*/
		private void ReadKahluaTable2(Byte b){/*{{{*/
			if (b == 0){
				ReadString();
			} else if (b == 1){
				ReadDouble();
			} else if (b == 3){
				ReadByte();
			} else if (b == 2){
				// Console.WriteLine("Nested table start");
				ReadKahluaTable();
				// Console.WriteLine("Nested table end");
			}
		}/*}}}*/
		private void ReadKahluaTable(){/*{{{*/
			Int32 numItems = ReadInt32();
			// Console.WriteLine("Kahlua Table has {0} items", numItems);
			for (; numItems > 0; numItems--){
				Byte b = ReadByte();
				ReadKahluaTable2(b);
				b = ReadByte();
				ReadKahluaTable2(b);
			}
		}/*}}}*/

		public static String[] FloorBloodTypes = {
			"blood_floor_small_01",
			"blood_floor_small_02",
			"blood_floor_small_03",
			"blood_floor_small_04",
			"blood_floor_small_05",
			"blood_floor_small_06",
			"blood_floor_small_07",
			"blood_floor_small_08",
			"blood_floor_med_01",
			"blood_floor_med_02",
			"blood_floor_med_03",
			"blood_floor_med_04",
			"blood_floor_med_05",
			"blood_floor_med_06",
			"blood_floor_med_07",
			"blood_floor_med_08",
			"blood_floor_large_01",
			"blood_floor_large_02",
			"blood_floor_large_03",
			"blood_floor_large_04",
			"blood_floor_large_05" };

		private void ReadBloodSplat(){/*{{{*/
			ReadByte(); //x
			ReadByte(); //y
			ReadByte(); //z
			/* Byte b = */ ReadByte();
			// TODO this.AddTile(FloorBloodTypes[b]);
			ReadSingle();
		}/*}}}*/

		/* Iso class hashCodes {{{ */
		const  Int32  Barbecue            =  -1687755011;
		const  Int32  Barricade           =  -319056439;
		const  Int32  Crate               =  65368995;
		const  Int32  Curtain             =  -1503318414;
		const  Int32  DeadBody            =  567032902;
		const  Int32  Door                =  2136014;
		const  Int32  Fire                =  2189910;
		const  Int32  Fireplace           =  1733194865;
		const  Int32  IsoGenerator        =  -2000077842;
		const  Int32  IsoObject           =  1843662852;
		const  Int32  IsoTrap             =  -534229710;
		const  Int32  Jukebox             =  407329702;
		const  Int32  LightSwitch         =  -843393078;
		const  Int32  MolotovCocktail     =  1967605434;
		const  Int32  Player              =  -1901885695;
		const  Int32  Pushable            =  1841007508;
		const  Int32  Radio               =  78717915;
		const  Int32  Stove               =  80218429;
		const  Int32  Survivor            =  -1535938282;
		const  Int32  Thumpable           =  -947923682;
		const  Int32  Tree                =  2615230;
		const  Int32  WheelieBin          =  -642739216;
		const  Int32  Window              =  -1703884784;
		const  Int32  WoodenWall          =  -1245461728;
		const  Int32  WorldInventoryItem  =  840017981;
		const  Int32  ZombieGiblets       =  1243319378;
		const  Int32  Zombie              =  -1612488122;
		/* }}} */

		private void AddTile(String spriteID){/*{{{*/
			this.currentGS.AddTile(spriteID);
		}/*}}}*/
		private void AddTile(Int32 spriteID){/*{{{*/
			String s;
			this.tileDefs.TryGetValue(spriteID, out s);
			this.currentGS.AddTile(s);
		}/*}}}*/
		private void AddDelayedSprites(int fromHere){/*{{{*/
			for (int i = fromHere; i<this.delayedSprites.Count; i++){
				this.AddTile(this.delayedSprites[i]);
			}
		}/*}}}*/
		private void ReadGenericIsoObject(bool addSprite){/*{{{*/
			this.delayedSprites.Clear();
			Int32 spriteID = ReadInt32(); // spriteID
			if (addSprite){
				this.AddTile(spriteID);
			} else {
				this.delayedSprites.Add(spriteID);
			}
			/* string spritename = */ ReadString(); // spriteName
			// Console.WriteLine("Object sprite: {0} (ID: {1})", spritename, spriteID);
			Byte animSprites = ReadByte(); // numAnimSprites;
			// Console.WriteLine("Found {0} animSprites", animSprites);
			for (; animSprites > 0; animSprites--){ // we skip these
				Int32 animSpriteID = ReadInt32(); // sprite.ID // wtf
				if (addSprite){
					this.AddTile(animSpriteID);
				} else {
					this.delayedSprites.Add(animSpriteID);
				}
				if (ReadByte() == 0){
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
				}
				ReadByte();
				ReadByte();
			}
			if (ReadByte() != 0){
				ReadString(); // Object name
			}
			if (ReadByte() != 0){
				// Console.WriteLine("Object has container");
				ReadContainer();
			}
			if (ReadByte() == 1){
				// Console.WriteLine("Object has Kahlua Table");
				ReadKahluaTable();
			}
			ReadByte(); // outline on mouseover
			if (ReadByte() == 1){
				/* read overlay sprite */
				String ovsprite = ReadString(); // spritename
				Int32 ovspriteID = this.tileDefs.FirstOrDefault(x => x.Value == ovsprite).Key;
				if (addSprite){
					this.AddTile(ovspriteID);
				} else {
					this.delayedSprites.Add(ovspriteID);
				}
				// Console.WriteLine("Overlaysprite: '{0}'", ovsprite);
				if (ReadByte() == 1){
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
				}
			}
			ReadByte(); // special tooltip
			ReadInt32(); // keyid
			Byte numSplats = ReadByte(); // wallbloodsplats
			for (; numSplats > 0; numSplats--){
				ReadSingle(); // age
				ReadInt32(); // id
			}
			return;
		}/*}}}*/
		private void ReadIsoBarbecue(){/*{{{*/
			ReadGenericIsoObject(false); 
			Byte hasPropaneTank = ReadByte(); // haspropanetank
			ReadInt32(); // fuel amount
			ReadByte(); // islit
			ReadSingle(); // lastupdate
			ReadInt32(); // minutes since extinguished
			if (ReadByte() == 1){
				Int32 spriteID = ReadInt32(); // normalSprite
				if (hasPropaneTank == 1){
					this.AddTile(spriteID);
				}
			}
			if (ReadByte() == 1){
				Int32 spriteID = ReadInt32(); // notanksprite
				if (hasPropaneTank == 0){
					this.AddTile(spriteID);
				}
			}
			AddDelayedSprites(0);
		}/*}}}*/
		private void ReadIsoBarricade(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadInt32(); // health
			ReadInt32(); // maxHealth
			ReadInt32(); // BarricideStrength
		}/*}}}*/
		private void ReadBodyDamage(){/*{{{*/
			for (int i=0; i<18; i++){/*{{{*/
				ReadByte(); // bitten
				ReadByte(); // scratched
				Byte bandaged = ReadByte(); // bandaged
				ReadByte(); // bleeding
				ReadByte(); // deep wound
				ReadByte(); // fake infected
				ReadSingle(); // health
				ReadInt32(); // unused
				if (bandaged == 1){
					ReadSingle(); // bandagelife
				}
				if (ReadByte() == 1){ // infected
					ReadSingle(); // infectionlevel
				}
				ReadSingle(); // bytetime
				ReadSingle(); // scratchtime
				ReadSingle(); // bleedtime
				ReadSingle(); // alcohollevel
				ReadSingle(); // additional pain
				ReadSingle(); // deepwoundtime
				ReadByte(); // haveglass
				ReadByte(); // getBandageXP
				ReadByte(); // stitched
				ReadSingle(); // stitchtime
				ReadByte(); // stitchXP
				ReadByte(); // splintXP
				ReadSingle(); // fracturetime
				if (ReadByte() == 1){ // splinted
					ReadSingle(); // splintfactor
				}
				ReadByte(); // have bullet
				ReadSingle(); // burntime
				ReadByte(); // needburnwash
				ReadSingle(); // lasttimeburnwash
				ReadString(); // SplintItem
				ReadString(); // BandageType
			}/*}}}*/
			ReadSingle(); // infectionlevel
			ReadSingle(); // fakeinfectionleve
			ReadSingle(); // wetnes
			ReadSingle(); // catchacold
			ReadByte(); // hascold
			ReadSingle(); // coldstrength
			ReadSingle(); // unhappyness
			ReadSingle(); // boredom
			ReadSingle(); // foodsickness
			ReadSingle(); // poisonlevel
			ReadSingle(); // temperature
			ReadByte(); // reducefakeinfectionlevel
			ReadSingle(); // healthfromfoodtimer
		}/*}}}*/
		private void ReadIsoCrate(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadInt32(); // container ID
		}/*}}}*/
		private void ReadIsoCurtain(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadByte(); // open
			ReadByte(); // north
			ReadInt32(); // health
			ReadInt32(); // BarricideStrength
			ReadInt32(); // sprite ID for !open
		}/*}}}*/
		private void ReadIsoDeadBody(){/*{{{*/
			ReadSingle();
			ReadSingle();
			ReadSingle();
			ReadSingle();
			ReadSingle();
			ReadInt32();
			if (ReadByte() == 1){
				ReadKahluaTable();
			}
			ReadByte(); // wasZombie
			if (ReadByte() == 1){ // bServer
				ReadInt16();
			} else {
				this.AddTile(ReadString()); // legsprite
				ReadSingle();
				ReadSingle();
				ReadSingle();
				ReadSingle();
				if (ReadByte() == 1){
					this.AddTile(ReadString()); // torsosprite
				}
				if (ReadByte() == 1){
					this.AddTile(ReadString()); // headsprite
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
				}
				if (ReadByte() == 1){
					this.AddTile(ReadString()); // bottomsprite
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
				}
				if (ReadByte() == 1){
					this.AddTile(ReadString()); // shoesprite
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
				}
				if (ReadByte() == 1){
					this.AddTile(ReadString()); // topsprite
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
				}
				Int32 extraSprites = ReadInt32();
				for (; extraSprites > 0; extraSprites--){
					this.AddTile(ReadString()); // extrasprite
					ReadSingle();
					ReadSingle();
					ReadSingle();
					ReadSingle();
				}
			}
			if (ReadByte() == 1){ // Survivordesc/*{{{*/
				ReadInt32();
				ReadString(); ReadString(); ReadString(); ReadString();
				ReadString(); ReadString(); ReadString(); ReadString();
				ReadString(); ReadString(); ReadString(); ReadString();
				ReadString();
				ReadInt32(); // gender
				ReadString();
				ReadSingle(); ReadSingle(); ReadSingle();
				ReadSingle(); ReadSingle(); ReadSingle();
				ReadSingle(); ReadSingle(); ReadSingle();
				ReadSingle(); ReadSingle(); ReadSingle();
				if (ReadInt32() == 1){
					ReadInt32();
					ReadString();
					Int32 numPerks = ReadInt32();
					for (; numPerks > 0; numPerks--){
						ReadInt32();
						ReadInt32();
					}
				}
			}/*}}}*/
			if (ReadByte() == 1){
				ReadInt32();
				ReadContainer();
				ReadInt16();
				ReadInt16();
				ReadInt16();
			}
			ReadSingle(); // deathtime
			ReadSingle(); // reanimate time
		}/*}}}*/
		private void ReadIsoDoor(){/*{{{*/
			ReadGenericIsoObject(false);
			Byte open = ReadByte(); // open
			ReadByte(); // locked
			ReadByte(); // north
			ReadInt32(); // barricaded
			ReadInt32(); // health
			ReadInt32(); // maxhealth
			ReadInt32(); // BarricideStrength
			ReadInt16(); // BarricideMaxStrength
			Int32 closed = ReadInt32(); // closedsprite
			Int32 opened = ReadInt32(); // openedsprite
			this.AddTile(open == 1 ? opened : closed);
			// Console.WriteLine("IsoDoor sprite: {0}", open == 1 ? opened : closed);
			ReadInt32(); // keyid
			ReadByte(); // locked by key
			AddDelayedSprites(0);
		}/*}}}*/
		private void ReadIsoFire(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadInt32(); // Life
			ReadInt32(); // SpreadDelay
			ReadInt32(); // LifeStage
			ReadInt32(); // LifeStageTimer
			ReadInt32(); // LifeStageDuration
			ReadInt32(); // Energy
			ReadInt32(); // numFlameParticles
			ReadInt32(); // SpreadTimer
			ReadInt32(); // Age
			ReadByte(); // perm
			ReadByte(); // age
		}/*}}}*/
		private void ReadIsoFireplace(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadInt32(); // fuelamount
			ReadByte(); // lit
			ReadSingle(); // lastupdate
			ReadInt32(); // minutes since extinguished
		}/*}}}*/
		private void ReadIsoGenerator(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadByte(); // isconnected
			ReadByte(); // activated
			ReadInt32(); // fuel
			ReadInt32(); // condition
			ReadInt32(); // lasthour
		}/*}}}*/
		private void ReadIsoTrap(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadInt32(); // sensorrange
			ReadInt32(); // firepower
			ReadInt32(); // firerange
			ReadInt32(); // explosionpower
			ReadInt32(); // explosionrange
			ReadInt32(); // smokerange
			ReadInt32(); // noiserange 
			ReadInt32(); // extradamage
			ReadInt32(); // remotecontrolid
		}/*}}}*/
		private void ReadIsoLightSwitch(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadByte(); // lightroom
			ReadInt32(); // roomid
			ReadByte(); // activated
		}/*}}}*/
		private void ReadIsoMovingObject(){/*{{{*/
			ReadSingle(); // offsetX
			ReadSingle(); // offsetY
			ReadSingle(); // X
			ReadSingle(); //Y
			ReadSingle(); // Z
			if (ReadByte() == 1){
				ReadKahluaTable();
			}
		}/*}}}*/
		private void ReadIsoGameCharacter(bool zombie){/*{{{*/
			ReadIsoMovingObject();

			if (ReadByte() == 1){
				ReadSurvivorDesc();
			}
			ReadContainer();

			ReadByte(); // asleep
			ReadSingle(); // wakeuptime
			if (!zombie){
				ReadInt32(); // perkstopick
				ReadStats();
				ReadBodyDamage();
				ReadXP();
				ReadInt32(); //  primary hand item
				ReadInt32(); //  secondary hand item
				ReadByte(); // onfire
				ReadSingle(); // depress
				ReadSingle(); // depressfirsttaken
				ReadSingle(); // betaeffect
				ReadSingle(); // betadelta
				ReadSingle(); // paineffect
				ReadSingle(); // paindelta
				ReadSingle(); // sleeppilleffect
				ReadSingle(); // sleeppilldelta
				Int32 numBooks = ReadInt32(); // numbooks
				for (; numBooks > 0; numBooks--){
					ReadString(); // book read
					ReadInt32(); // pages read
				}
				ReadSingle(); // reduce infection power
				Int32 numRecipes = ReadInt32(); // 
				for (; numRecipes > 0; numRecipes--){
					ReadString(); // recipe learned
				}
			}
		}/*}}}*/
		private void ReadIsoPlayer(bool zombie){/*{{{*/
			ReadByte();
			ReadInt32();

			ReadIsoGameCharacter(zombie);

			ReadDouble(); // hourssurvived
			ReadInt32(); // zombies killed

			if (ReadByte() == 1){
				ReadString(); // top palette
				ReadString(); // top cloth
				ReadSingle(); // tint.r
				ReadSingle(); // tint.g
				ReadSingle(); // tint.b
			}
			if (ReadByte() == 1){
				ReadString(); // shoe palette
				ReadString(); // shoe cloth
			}
			if (ReadByte() == 1){
				ReadString(); // ??? palette
				ReadString(); // ??? cloth
			}
			if (ReadByte() == 1){
				ReadString(); // bottom palette
				ReadString(); // bottom cloth
				ReadSingle(); // tint.r
				ReadSingle(); // tint.g
				ReadSingle(); // tint.b
			}

			ReadInt16(); // equipped item ids
			ReadInt16();
			ReadInt16();
			ReadInt16();
			ReadInt16();
			ReadInt16();

			ReadInt32(); // survivor kills
			ReadInt32();
		}/*}}}*/
		private void ReadIsoPushable(bool wheeliebin){/*{{{*/
			ReadIsoMovingObject();
			if (!wheeliebin){
				Int32 spriteID = ReadInt32(); // sprite
				this.AddTile(spriteID);
			}
			if (ReadByte() == 1){
				ReadContainer();
			}
		}/*}}}*/
		private void ReadIsoStove(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadByte(); // activated
		}/*}}}*/
		private void ReadIsoSurvivor(){/*{{{*/
			ReadIsoGameCharacter(false);
		}/*}}}*/
		private void ReadSurvivorDesc(){/*{{{*/
			ReadInt32(); // ID
			ReadString(); // forename
			ReadString(); // surname
			ReadString(); // legs
			ReadString(); // torso
			ReadString(); // head
			ReadString(); // top
			ReadString(); // bottom
			ReadString(); // shoes
			ReadString(); // shoespal
			ReadString(); // bottomspal
			ReadString(); // toppal
			ReadString(); // skinpal
			ReadString(); // hair
			ReadByte(); // female
			ReadString(); // profession
			ReadSingle(); ReadSingle(); ReadSingle();  // hair r/g/b
			ReadSingle(); ReadSingle(); ReadSingle();  // top r/g/b
			ReadSingle(); ReadSingle(); ReadSingle();  // trouser r/g/b
			ReadSingle(); ReadSingle(); ReadSingle();  // skin r/g/b

			Int32 size;
			if (ReadInt32() == 1){
				size = ReadInt32(); // size
				for (; size > 0; size--){
					ReadString();
				}
			}
			size = ReadInt32(); // size
			for (; size > 0; size--){ // XP Boost Map
				ReadInt32();
				ReadInt32();
			}
		}/*}}}*/
		private void ReadStats(){/*{{{*/
			ReadSingle(); // Anger      
			ReadSingle(); // boredom    
			ReadSingle(); // endurance  
			ReadSingle(); // fatigue    
			ReadSingle(); // fitness    
			ReadSingle(); // hunger     
			ReadSingle(); // morale     
			ReadSingle(); // stress     
			ReadSingle(); // Fear       
			ReadSingle(); // Panic      
			ReadSingle(); // Sanity     
			ReadSingle(); // Sickness   
			ReadSingle(); // Boredom    
			ReadSingle(); // Pain       
			ReadSingle(); // Drunkenness
			ReadSingle(); // thirst     
		}/*}}}*/
		private void ReadIsoThumpable(){/*{{{*/
			ReadGenericIsoObject(false);
			Byte open = ReadByte(); // open
			ReadByte(); // locked
			ReadByte(); // north
			ReadInt32(); // barricaded
			ReadInt32(); // health
			ReadInt32(); // maxhealth
			ReadInt32(); // BarricideStrength
			ReadInt16(); // BarricideMaxStrength
			Int32 closed = ReadInt32(); // closed sprite
			Int32 opened = -1;
			if (ReadInt32() == 1){
				opened = ReadInt32(); // opened sprite
			}
			this.AddTile(open == 1 ? opened : closed);
			ReadInt32(); // thump damage
			ReadString(); // name
			ReadByte(); // isDoor
			ReadByte(); // isDoorFrame
			ReadByte(); // iscorner
			ReadByte(); // isstairs
			ReadByte(); // iscontainer
			ReadByte(); // isFloor
			ReadByte(); // canBarricade
			ReadByte(); // canPassThru
			ReadByte(); // dismantable
			ReadByte(); // canbeplastered
			ReadByte(); // paintable
			ReadSingle(); // crossSpeed

			if (ReadByte() == 1){
				ReadKahluaTable();
			}

			if (ReadByte() == 1){
				ReadKahluaTable(); // modData
			}

			ReadByte(); // blockallTheSquare
			ReadByte(); // isThumpable
			ReadByte(); // isHoppable

			ReadInt32(); // ligthsourcelife
			ReadInt32(); // lightsourceradius
			ReadInt32(); // xoff
			ReadInt32(); // yoff
			ReadString(); // lightsourcefuel
			ReadSingle(); // lifedelta
			ReadSingle(); // lifeleft
			ReadByte(); // lightsourceon
			ReadByte(); // haveFuel

			ReadInt32(); // keyID
			ReadByte(); // lockedbykey
			ReadByte(); // lockedbypadlock
			ReadByte(); // canbelockedbypadlock
			ReadInt32(); // lockedbykeycode
			AddDelayedSprites(0);
		}/*}}}*/
		private void ReadIsoTree(){/*{{{*/
			ReadGenericIsoObject(true);
			ReadInt32(); // logYield
			ReadInt32(); // damage
		}/*}}}*/
		private void ReadIsoWindow(){/*{{{*/
			ReadGenericIsoObject(false);
			Byte open = ReadByte(); // open
			ReadByte(); // north
			ReadInt32(); // barricaded
			ReadInt32(); // health
			ReadInt32(); // BarricideStrength
			ReadInt16(); // BarricideMaxStrength
			ReadByte(); // locked
			ReadByte(); // permalocked
			Byte destroyed = ReadByte(); // destroyed
			Byte glassremoved = ReadByte(); // glass removed
			Int32 opened = -1;
			Int32 closed = -1;
			Int32 smashed = -1;
			Int32 noglass = -1;
			if (ReadByte() == 1){
				opened = ReadInt32(); // open sprite
			}
			if (ReadByte() == 1){
				closed = ReadInt32(); // closed sprite
			}
			if (ReadByte() == 1){
				smashed = ReadInt32(); // smashed sprite
			}
			if (ReadByte() == 1){
				noglass = ReadInt32(); // glass removed sprite
			}
			this.AddTile(open == 1 ? opened : destroyed == 1 ? smashed : glassremoved == 1 ? noglass : closed);
			AddDelayedSprites(0);
			ReadInt32(); // maxhealth
		}/*}}}*/
		private void ReadIsoWoodenWall(){/*{{{*/
			Byte open = ReadByte(); // open
			ReadByte(); // locked
			ReadByte(); // north
			ReadInt32(); // Barricaded
			ReadInt32(); // health
			ReadInt32(); // maxhealth
			ReadInt32(); // BarricideStrength
			ReadInt16(); // BarricideMaxStrength
			Int32 opened = ReadInt32(); // closed sprite
			Int32 closed = ReadInt32(); // open sprite
			this.AddTile(open == 1 ? opened : closed);
		}/*}}}*/
		private void ReadIsoWorldInventoryItem(){/*{{{*/
			ReadSingle(); // xoff
			ReadSingle(); // yoff
			ReadSingle(); // zoff
			ReadSingle(); // offX ???
			ReadSingle(); // offY ???

			Int16 len = ReadInt16(); // dataLen
			//String type = ReadString(); // object Type

			//len -= (Int16)type.Length;
			//for (; len > 0; len--){
				//ReadByte();
			//}
			binReader.ReadBytes(len);
		}/*}}}*/
		private void ReadXP(){/*{{{*/
			Int32 numTraits = ReadInt32();
			for (; numTraits > 0; numTraits--){
				ReadString(); // traitname
			}
			ReadInt32(); // totalXP
			ReadInt32(); // level
			ReadInt32(); // lastlevel
			Int32 numMaps = ReadInt32();
			for (; numMaps > 0; numMaps--){
				ReadInt32();
				ReadInt32();
			}
			numMaps = ReadInt32();
			for (; numMaps > 0; numMaps--){
				ReadInt32();
			}
			numMaps = ReadInt32();
			for (; numMaps > 0; numMaps--){
				ReadInt32();
				ReadInt32(); // level
			}
			numMaps = ReadInt32();
			for (; numMaps > 0; numMaps--){
				ReadInt32();
				ReadSingle(); // multiplier
				ReadByte(); // minlevel
				ReadByte(); // maxlevel
			}
		}/*}}}*/
		private void ReadIsoZombie(){/*{{{*/
			ReadIsoGameCharacter(true);
			ReadInt32(); // palette
			ReadSingle(); // pathspeed
			ReadInt32(); // time since seen flesh
			ReadInt32(); // fake dead
		}/*}}}*/
		private void ReadIsoObject(){/*{{{*/
			Byte b;

			b = ReadByte(); // serialize
			if (b == 0){
				// Console.WriteLine("Object not saved");
				return;
			}
			Int32 classID = ReadInt32();
			// Console.WriteLine("Class ID: {0}", classID); // classID
			switch (classID){
				case Barbecue: ReadIsoBarbecue(); break;
				case Barricade: ReadIsoBarricade(); break;
				case Crate: ReadIsoCrate(); break;
				case Curtain: ReadIsoCurtain(); break;
				case DeadBody: ReadIsoDeadBody(); break;
				case Door: ReadIsoDoor(); break;
				case Fire: ReadIsoFire(); break;
				case Fireplace: ReadIsoFireplace(); break;
				case IsoGenerator: ReadIsoGenerator(); break;
				case IsoObject: ReadGenericIsoObject(true); break;
				case IsoTrap: ReadIsoTrap(); break;
				case Jukebox: ReadGenericIsoObject(true); break;
				case LightSwitch: ReadIsoLightSwitch(); break;
				case MolotovCocktail: ReadGenericIsoObject(true); break;
				case Player: ReadIsoPlayer(false); break;
				case Pushable: ReadIsoPushable(false); break;
				case Radio: ReadGenericIsoObject(true); break;
				case Stove: ReadIsoStove(); break;
				case Survivor: ReadIsoSurvivor(); break;
				case Thumpable: ReadIsoThumpable(); break;
				case Tree: ReadIsoTree(); break;
				case WheelieBin: ReadIsoPushable(true); break;
				case Window: ReadIsoWindow(); break;
				case WoodenWall: ReadIsoWoodenWall(); break;
				case WorldInventoryItem: ReadIsoWorldInventoryItem(); break;
				case ZombieGiblets: ReadIsoMovingObject(); break;
				case Zombie: ReadIsoZombie(); break;
				default: ReadGenericIsoObject(true);
				break;
			}

		}/*}}}*/

		private void ReadGenericErosionData(){/*{{{*/
			ReadByte(); // stage
			ReadByte(); // dispSeason
			ReadByte(); // flags
		}/*}}}*/
		private void ReadErosionDataAreas(){/*{{{*/
			Byte regionID = ReadByte();
			Byte catID = ReadByte();

			if (regionID == 0){
				if (catID == 0){
					ReadGenericErosionData();
					ReadByte(); // gameObj
					ReadByte(); // maxStage
					ReadInt16(); // spawnTime
				} else if (catID == 1){
					ReadGenericErosionData();
					ReadByte(); // gameObj
					ReadByte(); // maxStage
					ReadInt16(); // spawnTime
				} else if (catID == 2){
					ReadGenericErosionData();
					ReadByte(); // gameObj
					ReadInt16(); // spawnTime
				} else if (catID == 3){
					ReadGenericErosionData();
					ReadByte(); // gameObj
					ReadByte(); // maxStage
					ReadInt16(); // spawnTime
					ReadByte(); // notGrass
				} else {
					// Console.WriteLine("Unknown Region {0} Category {1}", regionID, catID);
				}
			} else if (regionID == 1){
				if (catID == 0){
					ReadGenericErosionData();
					ReadByte(); // gameObj
					ReadByte(); // maxStage
					ReadInt16(); // spawnTime
					ReadInt32(); // curID
					ReadByte(); // hasGrass
				} else {
					// Console.WriteLine("Unknown Region {0} Category {1}", regionID, catID);
				}
			} else if (regionID == 2){
				if (catID == 0){
					ReadGenericErosionData();
					ReadByte(); // gameObj
					ReadByte(); // maxStage
					ReadInt16(); // spawnTime
					ReadInt32(); // curID
					if (ReadByte() == 1){
						ReadByte(); // gameObj
						ReadInt16(); // spawnTime
						ReadInt32(); // curID
					}
				} else if (catID == 1){
					ReadGenericErosionData();
					ReadByte(); // gameObj
					ReadInt16(); // spawnTime
					ReadInt32(); // curID
					ReadSingle(); // alpha
					if (ReadByte() == 1){
						ReadByte(); // gameObj
						ReadInt16(); // spawnTime
						ReadInt32(); // curID
						ReadSingle(); // alpha
					}
				} else {
					// Console.WriteLine("Unknown Region {0} Category {1}", regionID, catID);
				}
			} else if (regionID == 3){
				if (catID == 0){
					ReadGenericErosionData();
					ReadByte(); // gameObj
				} else {
					// Console.WriteLine("Unknown Region {0} Category {1}", regionID, catID);
				}
			}
		}/*}}}*/

		private void ReadGridSquare(){/*{{{*/
			ReadInt16(); // hourLastSeen

			Int32 numIsoObjects = ReadInt32();
			// Console.WriteLine("Got {0} objects", numIsoObjects);
			for (; numIsoObjects > 0; numIsoObjects--){
				ReadByte(); // bSpecial
				ReadByte(); // bWorld
				ReadIsoObject();
			}

			Int32 numBodies = ReadInt32();
			// Console.WriteLine("Got {0} bodies", numBodies);
			for (; numBodies > 0; numBodies--){
				ReadIsoObject();
			}

			if (ReadByte() != 0){
				// Console.WriteLine("Got a Kahlua table");
				ReadKahluaTable();
			}

			ReadByte(); // flags

			if (ReadByte() == 1){
				// Console.WriteLine("Got ErosionData");
				ReadByte(); // donothing
				ReadSingle(); // noisemain
				ReadByte(); // soil
				ReadSingle(); // magicnum
				Byte count = ReadByte();
				// Console.WriteLine("Got {0} areas", count);
				for (; count > 0; count--){
					ReadErosionDataAreas();
				}
			} else {
				// Console.WriteLine("ErosionData is unitilised");
			}

			if (ReadByte() == 1){
				// Console.WriteLine("Found a trap");
				ReadInt32();
				ReadInt32();
				ReadInt32();
			}

			ReadByte(); // has electricity
		}/*}}}*/

		private void ReadPack() {
			// MMGridSquare gs;
			Int32 version = ReadInt32();
			if (version != 68 && version != 67){
				// Console.WriteLine("Cannot handle map version {0}!", version);
				return;
			}
			ReadInt32(); // size of map data
			ReadInt64(); // CRC of map data

			Int32 numSplats = ReadInt32();
			// Console.WriteLine("{0} blodsplats on floor", numSplats);
			for (; numSplats > 0; numSplats--){
				ReadBloodSplat();
			}
				
			for (Int32 cz = 0; cz < 8; cz++) {
				for (Int32 cx = 0; cx < 10; cx++) {
					for (Int32 cy = 0; cy < 10; cy++) {
						// Console.WriteLine("{0}x{1}x{2}", cx, cy, cz);
						Byte issaved = ReadByte();
						if (issaved == 0) {
							// Console.WriteLine("GridSquare not saved");
						} else {
							// Console.WriteLine("GridSquare saved ({0})", issaved);
							this.currentGS = this.celldata.GetSquare(cx+this.offX, cy+this.offY, cz);
							this.currentGS.ResetTiles();
							ReadGridSquare();
						}
					}
				}
			}

			if (ReadByte() == 1){
				// Console.WriteLine("Got more erosiondata");
				ReadInt32();
				ReadInt32();
				ReadSingle();
				ReadSingle();
				ReadByte();
			}

			return;
		}
	}
}
