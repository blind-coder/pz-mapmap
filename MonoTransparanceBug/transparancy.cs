using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
//using System.Windows.Media.Imaging;

namespace TransparencyTest
{
	class Program
	{
		public static void Main(string[] args)
		{
			Bitmap subCell;
			Bitmap window;
			Bitmap grass;

			Image tmpimgwindow = Image.FromFile("window.png");
			Bitmap bitmapwindow = new Bitmap(tmpimgwindow);
			window = bitmapwindow.Clone(new Rectangle(0, 0, tmpimgwindow.Width, tmpimgwindow.Height), tmpimgwindow.PixelFormat);

			Image tmpimggrass = Image.FromFile("grass.png");
			Bitmap bitmapgrass = new Bitmap(tmpimggrass);
			grass = bitmapgrass.Clone(new Rectangle(0, 0, tmpimggrass.Width, tmpimggrass.Height), tmpimggrass.PixelFormat);

			subCell = new Bitmap(100, 100);

			using (Graphics gfx = Graphics.FromImage(subCell))
			{
				Rectangle cloneRect = new Rectangle(0, 0, grass.Width, grass.Height);
				Rectangle targRect = new Rectangle(25, 25, grass.Width, grass.Height);
				gfx.DrawImage(grass, targRect, cloneRect, GraphicsUnit.Pixel);

				cloneRect = new Rectangle(0, 0, window.Width, window.Height);
				targRect = new Rectangle(25, 25, window.Width, window.Height);
				gfx.DrawImage(window, targRect, cloneRect, GraphicsUnit.Pixel);
			}
			subCell.Save("transparencytest.png", System.Drawing.Imaging.ImageFormat.Png);
		}
	}
}
