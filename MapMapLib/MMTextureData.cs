/*******************************************************************
 * Author: Kees "TurboTuTone" Bekkema
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
//using System.Windows.Media.Imaging;

namespace MapMapLib
{
	public class MMTextureData
	{
		public int w;
		public int h;
		public int x;
		public int y;
		public int offx;
		public int offy;
		public int ow;
		public int oh;
		public string name;
		public Bitmap data;

		private Object drawlock = new Object();

		public MMTextureData(int x, int y, int w, int h, int offx, int offy, int ow, int oh, string name)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			this.offx = offx;
			this.offy = offy;
			this.ow = ow;
			this.oh = oh;
			// Console.WriteLine("{0}: w: {1} h: {2} ow: {3} oh: {4}", name, w, h, ow, oh);
			this.name = name;
		}

		public void SetData(Bitmap source)
		{
			Rectangle cloneRect = new Rectangle(this.x, this.y, this.w, this.h);
			System.Drawing.Imaging.PixelFormat format = source.PixelFormat;
			this.data = source.Clone(cloneRect, format);
			// this.data.Save("foo/"+this.name+".png", System.Drawing.Imaging.ImageFormat.Png);
		}

		public void Draw(Graphics target, int bx, int by)
		{
			Rectangle cloneRect = new Rectangle(0, 0, this.w, this.h);
			Rectangle targRect = new Rectangle(bx + this.offx, by + this.offy, this.w, this.h);
			lock (this.drawlock){
				target.DrawImage(this.data, targRect, cloneRect, GraphicsUnit.Pixel);
			}
		}
	}
}
