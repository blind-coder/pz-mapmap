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
		private Random rand;
		private BushInit[] bush;
		private PlantInit[] plant;

		public MMPlotter(int divider, MMTextures textures, bool dolayers, bool bigtree)
		{
			Dictionary<String, List<String>> collageCategory;
			List<String> collageSprites;
			String sheet = "";

			this.collages = new Dictionary<String, Dictionary<String, List<String>>>();
			/* {{{ Trees FIXME */
			collageCategory = new Dictionary<String, List<String>>();

			collageSprites = new List<String>();
			collageSprites.Add("e_americanholly_1_1");
			collageCategory.Add("American Holly Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanholly_1_2");
			collageCategory.Add("American Holly Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_americanholly_1_3");
			collageCategory.Add("American Holly Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_canadianhemlock_1_1");
			collageCategory.Add("Canadian Hemlock Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_canadianhemlock_1_2");
			collageCategory.Add("Canadian Hemlock Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_canadianhemlock_1_3");
			collageCategory.Add("Canadian Hemlock Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_virginiapine_1_1");
			collageCategory.Add("Virgina Pine Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_virginiapine_1_2");
			collageCategory.Add("Virgina Pine Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_virginiapine_1_3");
			collageCategory.Add("Virgina Pine Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirch_1_1");
			collageSprites.Add("e_riverbirch_1_13");
			collageCategory.Add("Riverbirch Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirch_1_2");
			collageSprites.Add("e_riverbirch_1_14");
			collageCategory.Add("Riverbirch Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_riverbirch_1_3");
			collageSprites.Add("e_riverbirch_1_15");
			collageCategory.Add("Riverbirch Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthorn_1_1");
			collageSprites.Add("e_cockspurhawthorn_1_13");
			collageCategory.Add("Cockspurn Hawthorn Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthorn_1_2");
			collageSprites.Add("e_cockspurhawthorn_1_14");
			collageCategory.Add("Cockspurn Hawthorn Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_cockspurhawthorn_1_2");
			collageSprites.Add("e_cockspurhawthorn_1_15");
			collageCategory.Add("Cockspurn Hawthorn Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_dogwood_1_1");
			collageSprites.Add("e_dogwood_1_13");
			collageCategory.Add("Dogwood Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_dogwood_1_2");
			collageSprites.Add("e_dogwood_1_14");
			collageCategory.Add("Dogwood Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_dogwood_1_2");
			collageSprites.Add("e_dogwood_1_15");
			collageCategory.Add("Dogwood Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_carolinasilverbell_1_1");
			collageSprites.Add("e_carolinasilverbell_1_13");
			collageCategory.Add("Carolina Silverbell Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_carolinasilverbell_1_2");
			collageSprites.Add("e_carolinasilverbell_1_14");
			collageCategory.Add("Carolina Silverbell Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_carolinasilverbell_1_2");
			collageSprites.Add("e_carolinasilverbell_1_15");
			collageCategory.Add("Carolina Silverbell Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwood_1_1");
			collageSprites.Add("e_yellowwood_1_13");
			collageCategory.Add("Yellowwood Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwood_1_2");
			collageSprites.Add("e_yellowwood_1_14");
			collageCategory.Add("Yellowwood Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_yellowwood_1_2");
			collageSprites.Add("e_yellowwood_1_15");
			collageCategory.Add("Yellowwood Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbud_1_1");
			collageSprites.Add("e_easternredbud_1_13");
			collageCategory.Add("Eastern Redbug Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbud_1_2");
			collageSprites.Add("e_easternredbud_1_14");
			collageCategory.Add("Eastern Redbug Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_easternredbud_1_2");
			collageSprites.Add("e_easternredbud_1_15");
			collageCategory.Add("Eastern Redbug Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_redmaple_1_1");
			collageSprites.Add("e_redmaple_1_13");
			collageCategory.Add("Red Maple Small", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_redmaple_1_2");
			collageSprites.Add("e_redmaple_1_14");
			collageCategory.Add("Red Maple Medium", collageSprites);
			collageSprites = new List<String>();
			collageSprites.Add("e_redmaple_1_2");
			collageSprites.Add("e_redmaple_1_15");
			collageCategory.Add("Red Maple Large", collageSprites);

			collageSprites = new List<String>();
			collageSprites.Add("e_americanlinden_1_1");
			collageSprites.Add("e_americanlinden_1_13");
			collageCategory.Add("American Linden Small", collageSprites);
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
			this.collages.Add("jumbo_tree", collageCategory);
/* {{{ err... nope
e_americanhollyJUMBO_1_0
e_americanhollyJUMBO_1_1
e_americanhollyJUMBO_1_2
e_americanhollyJUMBO_1_3
e_americanhollyJUMBO_1_4
e_americanlindenJUMBO_1_0
e_americanlindenJUMBO_1_1
e_americanlindenJUMBO_1_10
e_americanlindenJUMBO_1_11
e_americanlindenJUMBO_1_2
e_americanlindenJUMBO_1_3
e_americanlindenJUMBO_1_4
e_americanlindenJUMBO_1_5
e_americanlindenJUMBO_1_6
e_americanlindenJUMBO_1_7
e_americanlindenJUMBO_1_8
e_americanlindenJUMBO_1_9
e_canadianhemlockJUMBO_1_0
e_canadianhemlockJUMBO_1_1
e_canadianhemlockJUMBO_1_2
e_canadianhemlockJUMBO_1_3
e_canadianhemlockJUMBO_1_4
e_carolinasilverbellJUMBO_1_0
e_carolinasilverbellJUMBO_1_1
e_carolinasilverbellJUMBO_1_10
e_carolinasilverbellJUMBO_1_11
e_carolinasilverbellJUMBO_1_2
e_carolinasilverbellJUMBO_1_3
e_carolinasilverbellJUMBO_1_4
e_carolinasilverbellJUMBO_1_5
e_carolinasilverbellJUMBO_1_6
e_carolinasilverbellJUMBO_1_7
e_carolinasilverbellJUMBO_1_8
e_carolinasilverbellJUMBO_1_9
e_cockspurhawthornJUMBO_1_0
e_cockspurhawthornJUMBO_1_1
e_cockspurhawthornJUMBO_1_10
e_cockspurhawthornJUMBO_1_11
e_cockspurhawthornJUMBO_1_2
e_cockspurhawthornJUMBO_1_3
e_cockspurhawthornJUMBO_1_4
e_cockspurhawthornJUMBO_1_5
e_cockspurhawthornJUMBO_1_6
e_cockspurhawthornJUMBO_1_7
e_cockspurhawthornJUMBO_1_8
e_cockspurhawthornJUMBO_1_9
e_dogwoodJUMBO_1_0
e_dogwoodJUMBO_1_1
e_dogwoodJUMBO_1_10
e_dogwoodJUMBO_1_11
e_dogwoodJUMBO_1_2
e_dogwoodJUMBO_1_3
e_dogwoodJUMBO_1_4
e_dogwoodJUMBO_1_5
e_dogwoodJUMBO_1_6
e_dogwoodJUMBO_1_7
e_dogwoodJUMBO_1_8
e_dogwoodJUMBO_1_9
e_easternredbudJUMBO_1_0
e_easternredbudJUMBO_1_1
e_easternredbudJUMBO_1_10
e_easternredbudJUMBO_1_11
e_easternredbudJUMBO_1_2
e_easternredbudJUMBO_1_3
e_easternredbudJUMBO_1_4
e_easternredbudJUMBO_1_5
e_easternredbudJUMBO_1_6
e_easternredbudJUMBO_1_7
e_easternredbudJUMBO_1_8
e_easternredbudJUMBO_1_9
e_redmapleJUMBO_1_0
e_redmapleJUMBO_1_1
e_redmapleJUMBO_1_10
e_redmapleJUMBO_1_11
e_redmapleJUMBO_1_2
e_redmapleJUMBO_1_3
e_redmapleJUMBO_1_4
e_redmapleJUMBO_1_5
e_redmapleJUMBO_1_6
e_redmapleJUMBO_1_7
e_redmapleJUMBO_1_8
e_redmapleJUMBO_1_9
e_riverbirchJUMBO_1_0
e_riverbirchJUMBO_1_1
e_riverbirchJUMBO_1_10
e_riverbirchJUMBO_1_11
e_riverbirchJUMBO_1_2
e_riverbirchJUMBO_1_3
e_riverbirchJUMBO_1_4
e_riverbirchJUMBO_1_5
e_riverbirchJUMBO_1_6
e_riverbirchJUMBO_1_7
e_riverbirchJUMBO_1_8
e_riverbirchJUMBO_1_9
e_virginiapineJUMBO_1_0
e_virginiapineJUMBO_1_1
e_virginiapineJUMBO_1_2
e_virginiapineJUMBO_1_3
e_yellowwoodJUMBO_1_0
e_yellowwoodJUMBO_1_1
e_yellowwoodJUMBO_1_10
e_yellowwoodJUMBO_1_11
e_yellowwoodJUMBO_1_2
e_yellowwoodJUMBO_1_3
e_yellowwoodJUMBO_1_4
e_yellowwoodJUMBO_1_5
e_yellowwoodJUMBO_1_6
e_yellowwoodJUMBO_1_7
e_yellowwoodJUMBO_1_8
e_yellowwoodJUMBO_1_9
}}} */

			/* {{{ Bushes */
			collageCategory = new Dictionary<String, List<String>>();

			sheet = "f_bushes_1_";
			this.bush = new[] { new BushInit("Spicebush", 0.05F, 0.35F, false), new BushInit("Ninebark1", 0.65F, 0.75F, true), new BushInit("Ninebark2", 0.65F, 0.75F, true),
				new BushInit("Blueberry", 0.4F, 0.5F, true), new BushInit("Blackberry", 0.4F, 0.5F, true), new BushInit("Piedmont azalea1", 0.0F, 0.15F, true),
				new BushInit("Piedmont azalea2", 0.0F, 0.15F, true), new BushInit("Arrowwood viburnum", 0.3F, 0.8F, true), new BushInit("Red chokeberry1", 0.9F, 1.0F, true),
				new BushInit("Red chokeberry2" , 0.9F, 1.0F, true), new BushInit("Beautyberry", 0.7F, 0.85F, true), new BushInit("New jersey tea1", 0.4F, 0.8F, true),
				new BushInit("New jersey tea2", 0.4F, 0.8F, true), new BushInit("Wild hydrangea1", 0.2F, 0.35F, true), new BushInit("Wild hydrangea2", 0.2F, 0.35F, true),
				new BushInit("Shrubby St. John's wort", 0.35F, 0.75F, true) };

			for (int id = 1; id <= this.bush.Length; id++){
				int i = id - 1;
				int trunk = i - (int)(Math.Floor(i / 8.0F) * 8); // ?
				BushInit b = this.bush[i];

				int baseId = 0 + trunk; // ?
				int snowId = baseId + 16;
				int springId = snowId + 16;
				int autimnId = springId + 16;
				int summerId = i + 64;
				int bloomId = summerId + 16;

				collageSprites = new List<String>();
				collageSprites.Add(sheet + (baseId + 8));
				collageSprites.Add(sheet + (summerId + 32));
				// Console.WriteLine("Adding foliage: {0}", b.name);
				collageCategory.Add(b.name, collageSprites);
			}
			/* }}} */
			this.collages.Add("vegetation_foliage", collageCategory);
			/* {{{ Groundcover */
			collageCategory = new Dictionary<String, List<String>>();

			sheet = "d_plants_1_";
			this.plant = new[] { new PlantInit("Butterfly Weed", true, 0.05F, 0.25F), new PlantInit("Butterfly Weed", true, 0.05F, 0.25F),
				new PlantInit("Swamp Sunflower", true, 0.2F, 0.45F), new PlantInit("Swamp Sunflower", true, 0.2F, 0.45F),
				new PlantInit("Purple Coneflower", true, 0.1F, 0.35F), new PlantInit("Purple Coneflower", true, 0.1F, 0.35F),
				new PlantInit("Joe-Pye Weed", true, 0.8F, 1.0F), new PlantInit("Blazing Star", true, 0.25F, 0.65F),
				new PlantInit("Wild Bergamot", true, 0.45F, 0.6F), new PlantInit("Wild Bergamot", true, 0.45F, 0.6F),
				new PlantInit("White Beard-tongue", true, 0.2F, 0.65F), new PlantInit("White Beard-tongue", true, 0.2F, 0.65F),
				new PlantInit("Ironweed", true, 0.75F, 0.85F), new PlantInit("White Baneberry", true, 0.4F, 0.8F),
				new PlantInit("Wild Columbine", true, 0.85F, 1.0F), new PlantInit("Wild Columbine", true, 0.85F, 1.0F),
				new PlantInit("Jack-in-the-pulpit", false, 0.0F, 0.0F), new PlantInit("Wild Ginger", true, 0.1F, 0.9F),
				new PlantInit("Wild Ginger", true, 0.1F, 0.9F), new PlantInit("Wild Geranium", true, 0.65F, 0.9F),
				new PlantInit("Alumroot", true, 0.35F, 0.75F), new PlantInit("Wild Blue Phlox", true, 0.15F, 0.55F),
				new PlantInit("Polemonium Reptans", true, 0.4F, 0.6F), new PlantInit("Foamflower", true, 0.45F, 1.0F) };

			for (int id = 1; id <= this.plant.Length; id++){
				int i = id - 1;
				int trunk = i - (int)(Math.Floor(i / 8.0F) * 8); // ?
				int offset = 16;
				if (i >= 8) { offset = 24; }
				if (i >= 16) { offset = 32; }
				PlantInit p = this.plant[i];

				collageSprites = new List<String>();
				collageSprites.Add(sheet + (offset + i));
				collageSprites.Add(sheet + (offset + i + 8));
				// Console.WriteLine("Adding groundcover: {0}", p.name);
				collageCategory.Add(p.name + i, collageSprites);
			}
			/* }}} */
			this.collages.Add("vegetation_groundcover", collageCategory);

			this.rand = new Random();
			this.subDiv = divider;
			this.subWH = (300 / this.subDiv);
			this.startX = (1280 * 30 / this.subDiv / 2) - 64;
			this.dolayers = dolayers;
			this.bigtree = bigtree;
			if (this.dolayers == true)
			{
				this.subCell = new Bitmap(1280 * 30 / divider, (640 * 30 / divider) + 192, textures.format);
				this.startY = 0;
			}
			else
			{
				this.subCell = new Bitmap(1280 * 30 / divider, (640 * 30 / divider) + (192 * 8), textures.format);
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
												bool isJumbo = false;
												String tile = mmtile.tile;
												if (tile.Replace("JUMBO", "") != tile){
													isJumbo = true;
												}
												if (tile != null && this.textures.Textures.ContainsKey(tile)){
													this.textures.Textures[tile].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
													// Console.WriteLine("Drawing {0} at {1}+{2}x{3}+{4}", tile, drawx, mmtile.offX, drawy, mmtile.offY);
													drawCnt++;
												} else {
													//Console.WriteLine("tile {0} not found", tile);
													String needle = tile.Split('_')[0] + "_" + tile.Split('_')[1];
													// Console.WriteLine("Searching for collage: {0}", needle);
													if (this.collages.ContainsKey(needle.Replace("JUMBO", ""))){
														// Console.WriteLine("Drawing from collage instead");
														Dictionary<String, List<String>> collageCategory;
														this.collages.TryGetValue(needle, out collageCategory);

														IEnumerable<String> sprites;
														sprites = collageCategory.ElementAt(rand.Next(0, collageCategory.Count)).Value;
														foreach (String sprite in sprites){
															if (this.textures.Textures.ContainsKey(sprite)){
																String rSprite = sprite;
																if (isJumbo && bigtree){
																	String[] splits = new String[4];
																	splits = sprite.Split('_');
																	rSprite = splits[0]+"_"+splits[1]+"JUMBO_"+splits[2]+"_"+splits[3];
																}
																this.textures.Textures[rSprite].Draw(gfx, drawx + mmtile.offX, drawy + mmtile.offY);
																drawCnt++;
															//} else {
																//Console.WriteLine("Collages contain Unknown texture: {0}", sprite);
															}
														}
													//} else {
														//Console.WriteLine("Unknown texture: {0}", tile);
													}
												}
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
