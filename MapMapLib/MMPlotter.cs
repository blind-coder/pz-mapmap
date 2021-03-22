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
		private bool bigtree;
		private Bitmap subCell;
		private MMTextures textures;
		private Dictionary<String, Dictionary<String, List<String>>> collages;
		private Dictionary<String, List<String>> collagesJUMBO;
		private Dictionary<String, string> treesExchange;
		private Random rand;
		private BushInit[] bush;
		private PlantInit[] plant;
		private Int32 scale;

		private Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
		{
			Bitmap result = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(result))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.DrawImage(sourceBMP, 0, 0, width, height);
			}
			return result;
		}

		public MMPlotter(int divider, MMTextures textures, bool dolayers, bool bigtree, Int32 scale)
		{
			Dictionary<String, List<String>> collageCategory;
			Dictionary<String, List<String>> collageJUMBOCategory;
			List<String> collageSprites;
			String sheet = "";
			//treesExchange = new Dictionary<string, string>();
			//this.treesExchange.Add("vegetation_trees_01_8", "e_americanholly_1_3");
   //         this.treesExchange.Add("vegetation_trees_01_9", "e_americanholly_1_7");
   //         this.treesExchange.Add("vegetation_trees_01_10", "e_canadianhemlock_1_3");
   //         this.treesExchange.Add("vegetation_trees_01_11", "e_canadianhemlock_1_2");
			//this.treesExchange.Add("vegetation_trees_01_12", "e_canadianhemlock_1_3");
			//this.treesExchange.Add("vegetation_trees_01_13", "e_canadianhemlock_1_2");
			//this.treesExchange.Add("vegetation_trees_01_17", "e_canadianhemlock_1_2");
			//this.treesExchange.Add("vegetation_trees_01_24", "e_canadianhemlock_1_2");
			//this.treesExchange.Add("vegetation_trees_01_25", "e_canadianhemlock_1_2");

			this.collages = new Dictionary<String, Dictionary<String, List<String>>>();
			//this.collagesJUMBO = new Dictionary<String, List<String>>();
			collageJUMBOCategory = new Dictionary<String, List<String>>();
			collageSprites = new List<String>();
			collageSprites.Add("e_americanhollyJUMBO_1_0");
			collageJUMBOCategory.Add("American Holly Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanhollyJUMBO_1_1");
			collageJUMBOCategory.Add("American Holly Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_americanlindenJUMBO_1_0");
			collageSprites.Add("e_americanlindenJUMBO_1_6");
			collageJUMBOCategory.Add("American Linder Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanlindenJUMBO_1_1");
			collageSprites.Add("e_americanlindenJUMBO_1_7");
			collageJUMBOCategory.Add("American Linder Large", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanlindenJUMBO_1_0");
			collageSprites.Add("e_americanlindenJUMBO_1_8");
			collageJUMBOCategory.Add("American Linder Medium Autumn", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanlindenJUMBO_1_1");
			collageSprites.Add("e_americanlindenJUMBO_1_9");
			collageJUMBOCategory.Add("American Linder Large Autumn", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_canadianhemlockJUMBO_1_0");
			collageJUMBOCategory.Add("Canadian Hemlock Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_canadianhemlockJUMBO_1_1");
			collageJUMBOCategory.Add("Canadian Hemlock Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_carolinasilverbellJUMBO_1_0");
			collageSprites.Add("e_carolinasilverbellJUMBO_1_6");
			collageJUMBOCategory.Add("Carolina Silverbell Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_carolinasilverbellJUMBO_1_1");
			collageSprites.Add("e_carolinasilverbellJUMBO_1_7");
			collageJUMBOCategory.Add("Carolina Silverbell Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthornJUMBO_1_0");
			collageSprites.Add("e_cockspurhawthornJUMBO_1_6");
			collageJUMBOCategory.Add("Cockspurn Hawthorn Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthornJUMBO_1_1");
			collageSprites.Add("e_cockspurhawthornJUMBO_1_7");
			collageJUMBOCategory.Add("Cockspurn Hawthorn Large", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthornJUMBO_1_0");
			collageSprites.Add("e_cockspurhawthornJUMBO_1_8");
			collageJUMBOCategory.Add("Cockspurn Hawthorn Medium Autumn", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthornJUMBO_1_1");
			collageSprites.Add("e_cockspurhawthornJUMBO_1_9");
			collageJUMBOCategory.Add("Cockspurn Hawthorn Large Autumn", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_dogwoodJUMBO_1_0");
			collageSprites.Add("e_dogwoodJUMBO_1_6");
			collageJUMBOCategory.Add("Dogwood Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_dogwoodJUMBO_1_1");
			collageSprites.Add("e_dogwoodJUMBO_1_7");
			collageJUMBOCategory.Add("Dogwood Large", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_dogwoodJUMBO_1_0");
			collageSprites.Add("e_dogwoodJUMBO_1_8");
			collageJUMBOCategory.Add("Dogwood Medium Autumn", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_dogwoodJUMBO_1_1");
			collageSprites.Add("e_dogwoodJUMBO_1_9");
			collageJUMBOCategory.Add("Dogwood Large Autumn", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbudJUMBO_1_0");
			collageSprites.Add("e_easternredbudJUMBO_1_6");
			collageJUMBOCategory.Add("Eastern Redbug Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbudJUMBO_1_1");
			collageSprites.Add("e_easternredbudJUMBO_1_7");
			collageJUMBOCategory.Add("Eastern Redbug Large", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbudJUMBO_1_0");
			collageSprites.Add("e_easternredbudJUMBO_1_8");
			collageJUMBOCategory.Add("Eastern Redbug Medium Autumn", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbudJUMBO_1_1");
			collageSprites.Add("e_easternredbudJUMBO_1_9");
			collageJUMBOCategory.Add("Eastern Redbug Large Autumn", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_redmapleJUMBO_1_0");
			collageSprites.Add("e_redmapleJUMBO_1_6");
			collageJUMBOCategory.Add("Red Maple Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_redmapleJUMBO_1_1");
			collageSprites.Add("e_redmapleJUMBO_1_7");
			collageJUMBOCategory.Add("Red Maple Large", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_redmapleJUMBO_1_0");
			collageSprites.Add("e_redmapleJUMBO_1_8");
			collageJUMBOCategory.Add("Red Maple Medium Autumn", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_redmapleJUMBO_1_1");
			collageSprites.Add("e_redmapleJUMBO_1_9");
			collageJUMBOCategory.Add("Red Maple Large Autumn", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirchJUMBO_1_0");
			collageSprites.Add("e_riverbirchJUMBO_1_6");
			collageJUMBOCategory.Add("Riverbirch Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirchJUMBO_1_1");
			collageSprites.Add("e_riverbirchJUMBO_1_7");
			collageJUMBOCategory.Add("Riverbirch Large", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirchJUMBO_1_0");
			collageSprites.Add("e_riverbirchJUMBO_1_8");
			collageJUMBOCategory.Add("Riverbirch Medium Autumn", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirchJUMBO_1_1");
			collageSprites.Add("e_riverbirchJUMBO_1_9");
			collageJUMBOCategory.Add("Riverbirch Large Autumn", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_virginiapineJUMBO_1_0");
			collageJUMBOCategory.Add("Virgina Pine Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_virginiapineJUMBO_1_1");
			collageJUMBOCategory.Add("Virgina Pine Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwoodJUMBO_1_0");
			collageSprites.Add("e_yellowwoodJUMBO_1_6");
			collageJUMBOCategory.Add("Yellowwood Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwoodJUMBO_1_1");
			collageSprites.Add("e_yellowwoodJUMBO_1_7");
			collageJUMBOCategory.Add("Yellowwood Large", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwoodJUMBO_1_0");
			collageSprites.Add("e_yellowwoodJUMBO_1_8");
			collageJUMBOCategory.Add("Yellowwood Medium Autumn", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwoodJUMBO_1_1");
			collageSprites.Add("e_yellowwoodJUMBO_1_9");
			collageJUMBOCategory.Add("Yellowwood Large Autumn", collageSprites);


			this.collagesJUMBO = collageJUMBOCategory;
			this.collages.Add("jumbo_tree", collageJUMBOCategory);

			// TODO добавить все деревья

			collageCategory = new Dictionary<String, List<String>>();

			//collageSprites = new List<String>();
			//collageSprites.Add("e_americanholly_1_1");
			//collageCategory.Add("American Holly Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanholly_1_2");
			collageCategory.Add("American Holly Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanholly_1_3");
			collageCategory.Add("American Holly Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_canadianhemlock_1_1");
			//collageCategory.Add("Canadian Hemlock Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_canadianhemlock_1_2");
			collageCategory.Add("Canadian Hemlock Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_canadianhemlock_1_3");
			collageCategory.Add("Canadian Hemlock Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_virginiapine_1_1");
			//collageCategory.Add("Virgina Pine Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_virginiapine_1_2");
			collageCategory.Add("Virgina Pine Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_virginiapine_1_3");
			collageCategory.Add("Virgina Pine Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_riverbirch_1_1");
			//collageSprites.Add("e_riverbirch_1_13");
			//collageCategory.Add("Riverbirch Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirch_1_2");
			collageSprites.Add("e_riverbirch_1_14");
			collageCategory.Add("Riverbirch Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirch_1_3");
			collageSprites.Add("e_riverbirch_1_15");
			collageCategory.Add("Riverbirch Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_cockspurhawthorn_1_1");
			//collageSprites.Add("e_cockspurhawthorn_1_13");
			//collageCategory.Add("Cockspurn Hawthorn Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthorn_1_2");
			collageSprites.Add("e_cockspurhawthorn_1_14");
			collageCategory.Add("Cockspurn Hawthorn Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthorn_1_2");
			collageSprites.Add("e_cockspurhawthorn_1_15");
			collageCategory.Add("Cockspurn Hawthorn Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_dogwood_1_1");
			//collageSprites.Add("e_dogwood_1_13");
			//collageCategory.Add("Dogwood Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_dogwood_1_2");
			collageSprites.Add("e_dogwood_1_14");
			collageCategory.Add("Dogwood Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_dogwood_1_2");
			collageSprites.Add("e_dogwood_1_15");
			collageCategory.Add("Dogwood Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_carolinasilverbell_1_1");
			//collageSprites.Add("e_carolinasilverbell_1_13");
			//collageCategory.Add("Carolina Silverbell Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_carolinasilverbell_1_2");
			collageSprites.Add("e_carolinasilverbell_1_14");
			collageCategory.Add("Carolina Silverbell Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_carolinasilverbell_1_2");
			collageSprites.Add("e_carolinasilverbell_1_15");
			collageCategory.Add("Carolina Silverbell Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_yellowwood_1_1");
			//collageSprites.Add("e_yellowwood_1_13");
			//collageCategory.Add("Yellowwood Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwood_1_2");
			collageSprites.Add("e_yellowwood_1_14");
			collageCategory.Add("Yellowwood Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwood_1_2");
			collageSprites.Add("e_yellowwood_1_15");
			collageCategory.Add("Yellowwood Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_easternredbud_1_1");
			//collageSprites.Add("e_easternredbud_1_13");
			//collageCategory.Add("Eastern Redbug Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbud_1_2");
			collageSprites.Add("e_easternredbud_1_14");
			collageCategory.Add("Eastern Redbug Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbud_1_2");
			collageSprites.Add("e_easternredbud_1_15");
			collageCategory.Add("Eastern Redbug Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_redmaple_1_1");
			//collageSprites.Add("e_redmaple_1_13");
			//collageCategory.Add("Red Maple Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_redmaple_1_2");
			collageSprites.Add("e_redmaple_1_14");
			collageCategory.Add("Red Maple Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_redmaple_1_2");
			collageSprites.Add("e_redmaple_1_15");
			collageCategory.Add("Red Maple Large", collageSprites);

			//collageSprites = new List<String>();
			//collageSprites.Add("e_americanlinden_1_1");
			//collageSprites.Add("e_americanlinden_1_13");
			//collageCategory.Add("American Linden Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanlinden_1_2");
			collageSprites.Add("e_americanlinden_1_14");
			collageCategory.Add("American Linden Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanlinden_1_2");
			collageSprites.Add("e_americanlinden_1_15");
			collageCategory.Add("American Linden Large", collageSprites);
			/* }}} */
			this.collages.Add("vegetation_trees", collageCategory);


			collageCategory = new Dictionary<String, List<String>>();
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_16");
			collageSprites.Add("d_plants_1_24");
			collageCategory.Add("1", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_17");
			collageSprites.Add("d_plants_1_25");
			collageCategory.Add("2", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_18");
			collageSprites.Add("d_plants_1_26");
			collageCategory.Add("3", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_19");
			collageSprites.Add("d_plants_1_27");
			collageCategory.Add("4", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_20");
			collageSprites.Add("d_plants_1_28");
			collageCategory.Add("5", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_21");
			collageSprites.Add("d_plants_1_29");
			collageCategory.Add("6", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_22");
			collageSprites.Add("d_plants_1_30");
			collageCategory.Add("7", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_23");
			collageSprites.Add("d_plants_1_31");
			collageCategory.Add("8", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_32");
			collageSprites.Add("d_plants_1_40");
			collageCategory.Add("9", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_33");
			collageSprites.Add("d_plants_1_41");
			collageCategory.Add("10", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_34");
			collageSprites.Add("d_plants_1_42");
			collageCategory.Add("11", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_35");
			collageSprites.Add("d_plants_1_43");
			collageCategory.Add("12", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_36");
			collageSprites.Add("d_plants_1_44");
			collageCategory.Add("13", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_37");
			collageSprites.Add("d_plants_1_45");
			collageCategory.Add("14", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_38");
			collageSprites.Add("d_plants_1_46");
			collageCategory.Add("15", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("d_plants_1_39");
			collageSprites.Add("d_plants_1_47");
			collageCategory.Add("16", collageSprites);
			this.collages.Add("d_plants", collageCategory);

			this.rand = new Random();
			this.subDiv = divider;
			this.scale = scale;
			this.subWH = (300 / this.subDiv);
			this.startX = (1280 * 30 / this.subDiv / 2) - 64;
			this.dolayers = dolayers;
			this.bigtree = bigtree;
			if (this.dolayers == true)
			{
				this.subCell = new Bitmap(1280 * 30 / divider, (640 * 30 / divider) + 192, System.Drawing.Imaging.PixelFormat.Format32bppPArgb); //textures.format);
				this.startY = 0;
			}
			else
			{
				this.subCell = new Bitmap(1280 * 30 / divider, (640 * 30 / divider) + (192 * 8), System.Drawing.Imaging.PixelFormat.Format32bppPArgb); //textures.format);
				this.startY = 192 * 7;
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
							if (this.dolayers){
								if (File.Exists(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_layer_" + z + ".png")){
									//Console.WriteLine("Skipping {0}x{1}_{2}x{3} layer {4} because file exists", cellx, celly, subx, suby, z);
									continue;
								}
							} else {
								if (File.Exists(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_full.png")){
									//Console.WriteLine("Skipping {0}x{1}_{2}x{3} layer full because file exists", cellx, celly, subx, suby);
									continue;
								}
							}
							int drawCnt = 0;
							for (int x = 0; x < this.subWH; x++)
							{
								for (int y = 0; y < this.subWH; y++)
								{
									drawx = this.startX + (x - y) * 64;
									if (this.dolayers == true)
										drawy = this.startY + (x + y) * 32;
									else
										drawy = (this.startY + (x + y) * 32) - (192 * z);

									gs = cellData.GetSquare(x+(subx*this.subWH), y+(suby*this.subWH), z);

									if (gs != null){
										for (Int32 i = MMGridSquare.FLOOR; i <= MMGridSquare.TOP; i++){
											foreach (MMTile mmtile in gs.GetTiles(i)){
												String tile = mmtile.tile;
												//Console.WriteLine("tile: {0}", tile); // vegetation_trees_01_10  vegetation_trees_01_10 blends_natural_01_33
												
												if (tile != null)
                                                {
													if (tile.StartsWith("vegetation_groundcover"))
													{
														// TODO рисовать цветы
														Dictionary<String, List<String>> collageCategory;
														IEnumerable<String> sprites;
														this.collages.TryGetValue("d_plants", out collageCategory);
														sprites = collageCategory.ElementAt(rand.Next(0, collageCategory.Count)).Value;
														foreach (String sprite in sprites)
														{
															if (this.textures.Textures.ContainsKey(sprite))
															{
																this.textures.Textures[sprite].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
																//Console.WriteLine("Drawing {0} at {1}+{2}x{3}+{4}", sprite, drawx, mmtile.offX, drawy, mmtile.offY);
																drawCnt++;
															}
														}
													}
													else if (tile.StartsWith("vegetation_foliage"))
													{
														int r = this.rand.Next(16);
														if (this.textures.Textures.ContainsKey("f_bushes_1_" + r))
                                                        {
															this.textures.Textures["f_bushes_1_" + r].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
															r += 16 * 4;
															if (this.textures.Textures.ContainsKey("f_bushes_1_" + r))
															{
																this.textures.Textures["f_bushes_1_" + r].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
															}
														}
														drawCnt++;
													}
													else if (tile.StartsWith("f_bushes_1_"))
													{
														if (this.textures.Textures.ContainsKey(tile))
														{
															this.textures.Textures[tile].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
														}
														else
														{
															Console.WriteLine("557 ERROR {0} didn't find.", tile);
														}
														int k = int.Parse(tile.Replace("f_bushes_1_", ""));
														k += 16 * 6;
														if (this.textures.Textures.ContainsKey("f_bushes_1_" + k))
														{
															this.textures.Textures["f_bushes_1_" + k].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
														}
														else
														{
															//Console.WriteLine("567 ERROR {0} didn't find.", "f_bushes_1_" + k);
														}
														// 25% шанс появления цветов
														int r = this.rand.Next(4);
														if (r == 1)
                                                        {
															k += 16;
															if (this.textures.Textures.ContainsKey("f_bushes_1_" + k))
															{
																this.textures.Textures["f_bushes_1_" + k].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
															}
														}
														drawCnt++;
													}
													else if (tile != null && (this.textures.Textures.ContainsKey(tile)))
													{
														if (tile.StartsWith("e_") && !tile.StartsWith("e_exterior") && !tile.StartsWith("e_newgrass") && !tile.StartsWith("e_roof"))
														{
															this.textures.Textures[tile].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY-18);
															string templateName = tile.Substring(0, tile.Length - 1);
															int k = int.Parse(tile.Replace(templateName, ""));
															k += 8;
															tile = templateName + k;
															if (this.textures.Textures.ContainsKey(tile))
															{
																this.textures.Textures[tile].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY - 18);
															}
															//Console.WriteLine("Drawing {0} at {1}+{2}x{3}+{4}", tile, drawx, mmtile.offX, drawy, mmtile.offY);
														}
														else
														{
															this.textures.Textures[tile].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
														}
														drawCnt++;
													}
													else if (((tile.Replace("JUMBO", "") != tile) || (tile.StartsWith("jumbo_tree_"))) && bigtree)
													{
														Dictionary<String, List<String>> collageCategory;
														IEnumerable<String> sprites;
														this.collages.TryGetValue("jumbo_tree", out collageCategory);
														sprites = collageCategory.ElementAt(rand.Next(0, collageCategory.Count)).Value;
														foreach (String sprite in sprites)
														{
															if (this.textures.Textures.ContainsKey(sprite))
															{
																this.textures.Textures[sprite].Draw(gfx, drawx + mmtile.offX-128, drawy + mmtile.offY-256);
																//Console.WriteLine("Drawing {0} at {1}+{2}x{3}+{4}", sprite, drawx, mmtile.offX, drawy, mmtile.offY);
																drawCnt++;
															}
														}
													}
													else if (tile.StartsWith("vegetation_trees"))
													{
														//Console.WriteLine("tile {0} not found", tile);
														//String needle = tile.Split('_')[0] + "_" + tile.Split('_')[1];
														Dictionary<String, List<String>> collageCategory;
														this.collages.TryGetValue("vegetation_trees", out collageCategory);

														IEnumerable <String> sprites;
														sprites = collageCategory.ElementAt(rand.Next(0, collageCategory.Count)).Value;
														foreach (String sprite in sprites)
														{
															//Console.WriteLine("sprite: {0}", sprite); // e_americanholly_1_2 e_americanlinden_1_2 e_dogwood_1_2
															if (this.textures.Textures.ContainsKey(sprite))
															{
																this.textures.Textures[sprite].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
																drawCnt++;
															}
														}
													} else
                                                    {
														if (!tile.StartsWith("industry_bunker_01") &&
															!tile.StartsWith("normandy_panneau_common") &&
															!tile.StartsWith("normandy_panneau_misc") &&
															!tile.StartsWith("normandy_panneau_cherbourg"))
														{
															// TODO: найти паки с этими тайлами
															// TODO: прикрутить логирование в файл
															Console.WriteLine("643 ERROR {0} не обработан", tile);
														}
													}
												}
											}
										}
									}
								}
							}
							if (this.dolayers == true && drawCnt > 0)
							{
								Bitmap resized = ResizeBitmap(this.subCell, this.subCell.Width / this.scale, this.subCell.Height / this.scale);
								resized.Save(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_layer_" + z + ".png", System.Drawing.Imaging.ImageFormat.Png);
								//this.subCell.Save(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_layer_" + z + ".png", System.Drawing.Imaging.ImageFormat.Png);
								gfx.Clear(Color.Transparent);
							}
						}
                        if (this.dolayers == false)
                        {
							if (!File.Exists(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_full.png"))
							{
                                try
                                {
									//Console.WriteLine("File don't exist: " + this.subCell.Width + " " + this.subCell.Height);
									Bitmap resized = ResizeBitmap(this.subCell, this.subCell.Width / this.scale, this.subCell.Height / this.scale);
									resized.Save(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_full.png", System.Drawing.Imaging.ImageFormat.Png);
									//this.subCell.Save(outputDir + "cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_full.png", System.Drawing.Imaging.ImageFormat.Png);
								} catch
                                {
									using (StreamWriter sw = new StreamWriter("MapMap.log", false, System.Text.Encoding.Default))
									{
										sw.WriteLine("Resized exception: cell_" + cellx + "_" + celly + "_subcell_" + subx + "_" + suby + "_full.png");
									}
								}
								gfx.Clear(Color.Transparent);
							}
                        }
                    }
                }
			}
		}
	}

	class BushInit {
		public String name;
		public float bloomstart;
		public float bloomend;
		public bool hasFlower;

		public BushInit(String _name, float _bloomstart, float _bloomend, bool _hasFlower){
			this.name = _name;
			this.bloomstart = _bloomstart;
			this.bloomend = _bloomend;
			this.hasFlower = _hasFlower;
		}
	}

	class PlantInit
	{
		public String name;
		public bool hasFlower;
		public float bloomstart;
		public float bloomend;

		public PlantInit(String _name, bool _hasFlower, float _bloomstart, float _bloomend){
			this.name = _name;
			this.hasFlower = _hasFlower;
			this.bloomstart = _bloomstart;
			this.bloomend = _bloomend;
		}
	}

}
