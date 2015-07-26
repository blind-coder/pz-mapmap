/*******************************************************************
 * Author: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
//using System.Windows.Media.Imaging;

namespace MapMapLib
{
	public class MMPlotter
	{
		private int subDiv;
		private int subWH;
		private int startX;
		private int startY;
		private bool dolayers;
		private Bitmap subCell;
		private MMTextures textures;

		public MMPlotter(int divider, MMTextures textures, bool dolayers)
		{
			this.subDiv = divider;
			this.subWH = (300 / this.subDiv);
			this.startX = ((640 * (30 / this.subDiv)) / 2) - 32;
			this.dolayers = dolayers;
			if (this.dolayers == true)
			{
				this.subCell = new Bitmap(640 * (30 / divider), (320 * (30 / divider)) + 96, textures.format);
				this.startY = 0;
			}
			else
			{
				this.subCell = new Bitmap(640 * (30 / divider), (320 * (30 / divider)) + (96 * 8), textures.format);
				this.startY = 96 * 7;
			}
			this.textures = textures;
		}

		public void PlotData( MMCellData cellData, string outputDir, int cellx, int celly)
		{
			using (Graphics gfx = Graphics.FromImage(this.subCell))
			{
				int drawx = 0;
				int drawy = 0;
				MMGridSquare gs;
				for (int subx = 0; subx < this.subDiv; subx++)
				{
					for (int suby = 0; suby < this.subDiv; suby++)
					{
						for (int z = 0; z < 8; z++)
						{
							int drawCnt = 0;
							for (int x = 0; x < this.subWH; x++)
							{
								for (int y = 0; y < this.subWH; y++)
								{
									drawx = this.startX + (x - y) * 32;
									if (this.dolayers == true)
										drawy = this.startY + (x + y) * 16;
									else
										drawy = (this.startY + (x + y) * 16) - (96 * z);

									gs = cellData.GetSquare(x+(subx*this.subWH), y+(suby*this.subWH), z);

									if (gs != null)
									{
										foreach (string tile in gs.GetTiles())
										{
											if (tile != null && this.textures.Textures.ContainsKey(tile))
											{
												this.textures.Textures[tile].Draw(gfx, drawx, drawy);
												drawCnt++;
											 } else {
												 Console.WriteLine("Unknown texture: {0}", tile);
											}
										}
									}
								}
							}
							if (this.dolayers == true && drawCnt > 0)
							{
								this.subCell.Save(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_layer_" + z + ".png", System.Drawing.Imaging.ImageFormat.Png);
								gfx.Clear(Color.Transparent);
							}
						}
						if (this.dolayers == false)
						{
							this.subCell.Save(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_full.png", System.Drawing.Imaging.ImageFormat.Png);
							gfx.Clear(Color.Transparent);
						}
					}
				}
			}
		}
	}
}
