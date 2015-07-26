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

		public MMTile(String tilename){
			this.tile = tilename;
			this.offX = this.offY = 0;
		}
		public MMTile(String tilename, Int32 x, Int32 y){
			this.tile = tilename;
			this.offX = x;
			this.offY = y;
		}
	}

	public class MMGridSquare
	{
		private List <MMTile> tiles;
		private int roomID = 0;
		private bool hasContainer = false; //for future use
		private string container;

		public MMGridSquare()
		{
			this.tiles = new List<MMTile>();
		}

		public void AddTile(string tile, Int32 offsetX, Int32 offsetY)
		{
			//check for container here?
			this.tiles.Add(new MMTile(tile, offsetX, offsetY));
		}

		public void AddTile(string tile)
		{
			//check for container here?
			this.AddTile(tile, 0, 0);
		}

		public List<MMTile> GetTiles()
		{
			return this.tiles;
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
			this.tiles.Clear();
		}
		
		public void ResetTiles(){
			this.tiles.Clear();
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
						this.squares[z, x, y] = new MMGridSquare();
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
