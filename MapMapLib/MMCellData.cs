/*******************************************************************
 * Author: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapMapLib
{
	public class MMTile
	{
		public String tile;
		public Int32 offX;
		public Int32 offY;
		public Int32 x;
		public Int32 y;
		public Int32 z;

		public MMTile(String tilename){
			this.tile = tilename;
			this.offX = this.offY = this.x = this.y = this.z = 0;
		}
		public MMTile(String tilename, Int32 offx, Int32 offy, Int32 x, Int32 y, Int32 z){
			this.tile = tilename;
			this.offX = offx;
			this.offY = offy;
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	public class MMGridSquare
	{
		private static List <MMTile>[] elsewhere;
		private List <MMTile> top, middle, bottom;
		private int roomID = 0;
		private bool hasContainer = false; //for future use
		private string container;
		private Int32 x;
		private Int32 y;
		private Int32 z;
		public const Int32 TOP = 0;
		public const Int32 MIDDLE = 1;
		public const Int32 BOTTOM = 2;

		public MMGridSquare(Int32 x, Int32 y, Int32 z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.top = new List<MMTile>();
			this.bottom = new List<MMTile>();
			this.middle = new List<MMTile>();
			if (elsewhere == null){
				elsewhere = new List <MMTile>[3]; 
				elsewhere[MMGridSquare.TOP] = new List <MMTile>();
				elsewhere[MMGridSquare.MIDDLE] = new List <MMTile>();
				elsewhere[MMGridSquare.BOTTOM] = new List <MMTile>();
			}
		}

		public void AddTile(Int32 which, string tile, Int32 offsetX, Int32 offsetY){
			//check for container here?
			if (offsetX > 64 || offsetY > 32){
				// elsewhere[which].Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z)); // TODO XXX FIXME
				return;
			}
			switch (which){
				case TOP:
					this.top.Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z));
					break;
				case MIDDLE:
					this.middle.Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z));
					break;
				case BOTTOM:
				default:
					this.bottom.Add(new MMTile(tile, offsetX, offsetY, this.x, this.y, this.z));
					break;
			};
		}
		public void AddTile(string tile, Int32 offsetX, Int32 offsetY) {
			if (tile == null)
				return;
			if (tile.Contains("wall") ||
					tile.Contains("carpentry_02_80") || tile.Contains("carpentry_02_81") // Log walls
					){
				this.AddTile(TOP, tile, offsetX, offsetY);
				return;
			}
			this.AddTile(MIDDLE, tile, offsetX, offsetY);
		}

		public void AddTile(string tile)
		{
			//check for container here?
			this.AddTile(tile, 0, 0);
		}

		public List<MMTile> GetTiles(Int32 which) {
			foreach (MMTile mt in elsewhere[which]){
				Int32 dx = (mt.x - mt.y) * 32;
				Int32 dy = (mt.x + mt.y) * 16;
				dx += mt.offX;
				dy += mt.offY;

				Int32 tx = (dx + 2 * dy) / 64;
				Int32 ty = (dx - 2 * dy) / -64;
				if (tx == this.x && ty == this.y && mt.z == this.z){
					this.AddTile(which, mt.tile, mt.offX % 64, mt.offY % 32);
				}
			}
			switch (which){
				case TOP:
					return this.top;
				case MIDDLE:
					return this.middle;
				case BOTTOM:
				default:
					return this.bottom;
			}
			return null;
		}

		public void SetRoomID(int roomid)
		{
			this.roomID = roomid;
		}

		public int GetRoomID()
		{
			return this.roomID;
		}

		public bool HasContainer()
		{
			return hasContainer;
		}

		public string GetContainerType()
		{
			return this.container;
		}

		public void Reset()
		{
			this.roomID = 0;
			this.hasContainer = false;
			this.container = null;
			this.ResetTiles();
		}
		
		public void ResetTiles(){
			this.top.Clear();
			this.middle.Clear();
			this.bottom.Clear();
		}
	}

	public class MMCellData
	{
		private MMGridSquare[,,] squares;

		public MMCellData()
		{
			this.squares = new MMGridSquare[8,900,900];
			for (int z = 0; z < 8; z++)
				for (int x = 0; x < 900; x++)
					for (int y = 0; y < 900; y++)
						this.squares[z, x, y] = new MMGridSquare(x, y, z);
		}

		public MMGridSquare SetSquare(int x, int y, int z)
		{
			MMGridSquare gs = this.squares[z, x, y];
			return gs;
		}

		public MMGridSquare GetSquare(int x, int y, int z)
		{
			return this.squares[z, x, y];
		}

		public void Reset()
		{
			for (int z = 0; z < 8; z++)
				for (int x = 0; x < 900; x++)
					for (int y = 0; y < 900; y++)
						this.squares[z, x, y].Reset();
		}
	}
}
