using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Photo_Shop
{
    internal class Image
    {
        public Bitmap? Img { get; }
        public string? Name { get; }
        public byte[]? BytesImg { get; }  

        public Image(string file)
        {
            Name = NameGenerator(10);
            Img = new Bitmap(file);
            BytesImg = getByteImg(Img);
        }

       //public Image CopyImage()
       //{
       //    Image img = new Image(this.Name);
       //}
        private static void writeImageBytes(Bitmap img, byte[] bytes)
        {
            var data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.WriteOnly, img.PixelFormat);
            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
            img.UnlockBits(data);

        }
        private static byte[] getImgBytes24(Bitmap img_out)
        {
            byte[] bytes = new byte[img_out.Width * img_out.Height * 3];
            var data = img_out.LockBits(new Rectangle(0, 0, img_out.Width, img_out.Height), ImageLockMode.ReadOnly, img_out.PixelFormat);
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            img_out.UnlockBits(data);
            return bytes;
        }
        private static byte[] getByteImg(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            byte[] input_bytes1 = new byte[0];
            byte[] bytes = new byte[w * h * 3];

            using (Bitmap img_out1 = new Bitmap(w, h, PixelFormat.Format24bppRgb))
            {
                img_out1.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using (var g = Graphics.FromImage(img_out1))
                {
                    g.DrawImageUnscaled(img, 0, 0);
                }
                input_bytes1 = getImgBytes24(img_out1);

                Parallel.For(0, h, (i) =>
                {
                    var index = i * w;
                    for (int j = 0; j < w; j++)
                    {
                        var idj = index + j;
                        bytes[3 * idj + 2] = input_bytes1[3 * idj + 2];
                        bytes[3 * idj + 1] = input_bytes1[3 * idj + 1];
                        bytes[3 * idj + 0] = input_bytes1[3 * idj + 0];

                    }
                });
                Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                img_ret.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                writeImageBytes(img_ret, bytes);
                return bytes;
            }



        }
        public string NameGenerator(int x)
        {
            Random random = new Random();
            string pass = "";
            var r = new Random();
            while (pass.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }
            return pass;
        }
    }
}
