using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Photo_Shop
{
    internal struct Image
    {
        public Bitmap Img { get; private set; }
        public byte[] BytesImg { get; private set; }  
        public Image(string file)
        {
            Img = new Bitmap(file);
            BytesImg = GetByteImg(Img);
        }
        public Image(Bitmap img)
        {
            Img = new Bitmap(img);
            BytesImg = GetByteImg(Img);
        }
        public Image(Image img)
        {
            Img = new Bitmap(img.Img);
            BytesImg = img.BytesImg;
        }
        public void ChangeImg(Bitmap img)
        {
            Img = new Bitmap(img);
            BytesImg = GetByteImg(Img);
        }
        private static void WriteImageBytes(Bitmap img, byte[] bytes)
        {
            var data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.WriteOnly, img.PixelFormat);
            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
            img.UnlockBits(data);

        }
        private static byte[] GetImgBytes24(Bitmap img_out)
        {
            byte[] bytes = new byte[img_out.Width * img_out.Height * 3];
            var data = img_out.LockBits(new Rectangle(0, 0, img_out.Width, img_out.Height), ImageLockMode.ReadOnly, img_out.PixelFormat);
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            img_out.UnlockBits(data);
            return bytes;
        }
        private static byte[] GetByteImg(Bitmap img)
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
                input_bytes1 = GetImgBytes24(img_out1);

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
                WriteImageBytes(img_ret, bytes);
                return bytes;
            }



        }
        public static Image Operation(Image image1, Image image2, int type)
        {
            int w;
            int h;
            Bitmap img1 = image1.Img;
            Bitmap img2 = image2.Img;
            byte[] input_bytes1 = image1.BytesImg;
            byte[] input_bytes2 = image2.BytesImg;

            if (img1?.Height * img1?.Width > img2?.Height * img2?.Width)
            {
                w = img1.Width;
                h = img1.Height;
                img2 = ResizeImage(img2, w, h);
                input_bytes2 = GetByteImg(img2);
            }
            else
            {
                w = img2.Width;
                h = img2.Height;
                img1 = ResizeImage(img1, w, h);
                input_bytes1 = GetByteImg(img1);
            }
            byte[] bytes = new byte[w * h * 3];

            switch (type)
            {
                case 1:
                    Parallel.For(0, h, (i) =>
                    {
                        var index = i * w;
                        for (int j = 0; j < w; j++)
                        {
                            var idj = index + j;
                            bytes[3 * idj + 2] = (byte)Clamp(input_bytes1[3 * idj + 2] + input_bytes2[3 * idj + 2], 0, 255);
                            bytes[3 * idj + 1] = (byte)Clamp(input_bytes1[3 * idj + 1] + input_bytes2[3 * idj + 1], 0, 255);
                            bytes[3 * idj + 0] = (byte)Clamp(input_bytes1[3 * idj + 0] + input_bytes2[3 * idj + 0], 0, 255);

                        }
                    });
                    break;
                case 2:
                    Parallel.For(0, h, (i) =>
                    {
                        var index = i * w;
                        for (int j = 0; j < w; j++)
                        {
                            var idj = index + j;
                            bytes[3 * idj + 2] = (byte)Clamp(input_bytes1[3 * idj + 2] - input_bytes2[3 * idj + 2], 0, 255);
                            bytes[3 * idj + 1] = (byte)Clamp(input_bytes1[3 * idj + 1] - input_bytes2[3 * idj + 1], 0, 255);
                            bytes[3 * idj + 0] = (byte)Clamp(input_bytes1[3 * idj + 0] - input_bytes2[3 * idj + 0], 0, 255);

                        }
                    });
                    break;
                case 3:
                    Parallel.For(0, h, (i) =>
                    {
                        var index = i * w;
                        for (int j = 0; j < w; j++)
                        {
                            var idj = index + j;
                            bytes[3 * idj + 2] = (byte)Clamp((input_bytes1[3 * idj + 2] + input_bytes2[3 * idj + 2]) / 2, 0, 255);
                            bytes[3 * idj + 1] = (byte)Clamp((input_bytes1[3 * idj + 1] + input_bytes2[3 * idj + 1]) / 2, 0, 255);
                            bytes[3 * idj + 0] = (byte)Clamp((input_bytes1[3 * idj + 0] + input_bytes2[3 * idj + 0]) / 2, 0, 255);

                        }
                    });
                    break;
                case 4:
                    Parallel.For(0, h, (i) =>
                    {
                        var index = i * w;
                        for (int j = 0; j < w; j++)
                        {
                            var idj = index + j;
                            bytes[3 * idj + 2] = (byte)Clamp(input_bytes1[3 * idj + 2] * input_bytes2[3 * idj + 2], 0, 255);
                            bytes[3 * idj + 1] = (byte)Clamp(input_bytes1[3 * idj + 1] * input_bytes2[3 * idj + 1], 0, 255);
                            bytes[3 * idj + 0] = (byte)Clamp(input_bytes1[3 * idj + 0] * input_bytes2[3 * idj + 0], 0, 255);

                        }
                    });
                    break;
                case 5:
                    Parallel.For(0, h, (i) =>
                    {
                        var index = i * w;
                        for (int j = 0; j < w; j++)
                        {
                            var idj = index + j;
                            bytes[3 * idj + 2] = input_bytes1[3 * idj + 2] < input_bytes2[3 * idj + 2] ? input_bytes1[3 * idj + 2] : input_bytes2[3 * idj + 2];
                            bytes[3 * idj + 1] = input_bytes1[3 * idj + 1] < input_bytes2[3 * idj + 1] ? input_bytes1[3 * idj + 1] : input_bytes2[3 * idj + 1];
                            bytes[3 * idj + 0] = input_bytes1[3 * idj + 0] < input_bytes2[3 * idj + 0] ? input_bytes1[3 * idj + 0] : input_bytes2[3 * idj + 0];

                        }
                    });
                    break;
                case 6:
                    Parallel.For(0, h, (i) =>
                    {
                        var index = i * w;
                        for (int j = 0; j < w; j++)
                        {
                            var idj = index + j;
                            bytes[3 * idj + 2] = input_bytes1[3 * idj + 2] > input_bytes2[3 * idj + 2] ? input_bytes1[3 * idj + 2] : input_bytes2[3 * idj + 2];
                            bytes[3 * idj + 1] = input_bytes1[3 * idj + 1] > input_bytes2[3 * idj + 1] ? input_bytes1[3 * idj + 1] : input_bytes2[3 * idj + 1];
                            bytes[3 * idj + 0] = input_bytes1[3 * idj + 0] > input_bytes2[3 * idj + 0] ? input_bytes1[3 * idj + 0] : input_bytes2[3 * idj + 0];

                        }
                    });
                    break;
            }
            Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            img_ret.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);
            WriteImageBytes(img_ret, bytes);
            Image outImg = new Image(img_ret);
            return outImg;

        } // Объединенные операции
        public static Image OperationSum(Image image1, Image image2)
        {
            int w;
            int h;
            Bitmap img1 = image1.Img;
            Bitmap img2 = image2.Img;
            byte[] input_bytes1 = image1.BytesImg;
            byte[] input_bytes2 = image2.BytesImg;

            if (img1?.Height * img1?.Width > img2?.Height * img2?.Width)
            {
                w = img1 == null ? 0 : img1.Width;
                h = img1 == null ? 0 : img1.Height;
                img2 = ResizeImage(img2, w, h);
                input_bytes2 = GetByteImg(img2);
            }
            else
            {
                w = img2 == null ? 0 : img2.Width;
                h = img2 == null ? 0 : img2.Height;
                img1 = ResizeImage(img1, w, h);
                input_bytes1 = GetByteImg(img1);
            }
            byte[] bytes = new byte[w * h * 3];

            Parallel.For(0, h, (i) =>
            {
                var index = i * w;
                for (int j = 0; j < w; j++)
                {
                    var idj = index + j;
                    bytes[3 * idj + 2] = (byte)Clamp(input_bytes1[3 * idj + 2] + input_bytes2[3 * idj + 2], 0, 255);
                    bytes[3 * idj + 1] = (byte)Clamp(input_bytes1[3 * idj + 1] + input_bytes2[3 * idj + 1], 0, 255);
                    bytes[3 * idj + 0] = (byte)Clamp(input_bytes1[3 * idj + 0] + input_bytes2[3 * idj + 0], 0, 255);
            
                }
            });
            Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            img_ret.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);
            WriteImageBytes(img_ret, bytes);
            Image outImg = new Image(img_ret);
            return outImg;
        }
        public static Image OperationMult(Image image1, Image image2)
        {
            int w;
            int h;
            Bitmap img1 = image1.Img;
            Bitmap img2 = image2.Img;
            byte[] input_bytes1 = image1.BytesImg;
            byte[] input_bytes2 = image2.BytesImg;

            if (img1?.Height * img1?.Width > img2?.Height * img2?.Width)
            {
                w = img1 == null ? 0 : img1.Width;
                h = img1 == null ? 0 : img1.Height;
                img2 = ResizeImage(img2, w, h);
                input_bytes2 = GetByteImg(img2);
            }
            else
            {
                w = img2 == null ? 0 : img2.Width;
                h = img2 == null ? 0 : img2.Height;
                img1 = ResizeImage(img1, w, h);
                input_bytes1 = GetByteImg(img1);
            }
            byte[] bytes = new byte[w * h * 3];

            Parallel.For(0, h, (i) =>
            {
                var index = i * w;
                for (int j = 0; j < w; j++)
                {
                    var idj = index + j;
                    bytes[3 * idj + 2] = (byte)Clamp(input_bytes1[3 * idj + 2] * input_bytes2[3 * idj + 2], 0, 255);
                    bytes[3 * idj + 1] = (byte)Clamp(input_bytes1[3 * idj + 1] * input_bytes2[3 * idj + 1], 0, 255);
                    bytes[3 * idj + 0] = (byte)Clamp(input_bytes1[3 * idj + 0] * input_bytes2[3 * idj + 0], 0, 255);

                }
            });
            Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            img_ret.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);
            WriteImageBytes(img_ret, bytes);
            Image outImg = new Image(img_ret);
            return outImg;
        }
        public static Image OperationMax(Image image1, Image image2)
        {
            int w;
            int h;
            Bitmap img1 = image1.Img;
            Bitmap img2 = image2.Img;
            byte[] input_bytes1 = image1.BytesImg;
            byte[] input_bytes2 = image2.BytesImg;

            if (img1?.Height * img1?.Width > img2?.Height * img2?.Width)
            {
                w = img1 == null ? 0 : img1.Width;
                h = img1 == null ? 0 : img1.Height;
                img2 = ResizeImage(img2, w, h);
                input_bytes2 = GetByteImg(img2);
            }
            else
            {
                w = img2 == null ? 0 : img2.Width;
                h = img2 == null ? 0 : img2.Height;
                img1 = ResizeImage(img1, w, h);
                input_bytes1 = GetByteImg(img1);
            }
            byte[] bytes = new byte[w * h * 3];

            Parallel.For(0, h, (i) =>
            {
                var index = i * w;
                for (int j = 0; j < w; j++)
                {
                    var idj = index + j;
                    bytes[3 * idj + 2] = input_bytes1[3 * idj + 2] > input_bytes2[3 * idj + 2] ? input_bytes1[3 * idj + 2] : input_bytes2[3 * idj + 2];
                    bytes[3 * idj + 1] = input_bytes1[3 * idj + 1] > input_bytes2[3 * idj + 1] ? input_bytes1[3 * idj + 1] : input_bytes2[3 * idj + 1];
                    bytes[3 * idj + 0] = input_bytes1[3 * idj + 0] > input_bytes2[3 * idj + 0] ? input_bytes1[3 * idj + 0] : input_bytes2[3 * idj + 0];

                }
            });
            Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            img_ret.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);
            WriteImageBytes(img_ret, bytes);
            Image outImg = new Image(img_ret);
            return outImg;
        }
        public static Image OperationMin(Image image1, Image image2)
        {
            int w;
            int h;
            Bitmap img1 = image1.Img;
            Bitmap img2 = image2.Img;
            byte[] input_bytes1 = image1.BytesImg;
            byte[] input_bytes2 = image2.BytesImg;

            if (img1?.Height * img1?.Width > img2?.Height * img2?.Width)
            {
                w = img1 == null ? 0 : img1.Width;
                h = img1 == null ? 0 : img1.Height;
                img2 = ResizeImage(img2, w, h);
                input_bytes2 = GetByteImg(img2);
            }
            else
            {
                w = img2 == null ? 0 : img2.Width;
                h = img2 == null ? 0 : img2.Height;
                img1 = ResizeImage(img1, w, h);
                input_bytes1 = GetByteImg(img1);
            }
            byte[] bytes = new byte[w * h * 3];

            Parallel.For(0, h, (i) =>
            {
                var index = i * w;
                for (int j = 0; j < w; j++)
                {
                    var idj = index + j;
                    bytes[3 * idj + 2] = input_bytes1[3 * idj + 2] < input_bytes2[3 * idj + 2] ? input_bytes1[3 * idj + 2] : input_bytes2[3 * idj + 2];
                    bytes[3 * idj + 1] = input_bytes1[3 * idj + 1] < input_bytes2[3 * idj + 1] ? input_bytes1[3 * idj + 1] : input_bytes2[3 * idj + 1];
                    bytes[3 * idj + 0] = input_bytes1[3 * idj + 0] < input_bytes2[3 * idj + 0] ? input_bytes1[3 * idj + 0] : input_bytes2[3 * idj + 0];

                }
            });
            Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            img_ret.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);
            WriteImageBytes(img_ret, bytes);
            Image outImg = new Image(img_ret);
            return outImg;
        }
        public static Image OperationAvg(Image image1, Image image2)
        {
            int w;
            int h;
            Bitmap img1 = image1.Img;
            Bitmap img2 = image2.Img;
            byte[] input_bytes1 = image1.BytesImg;
            byte[] input_bytes2 = image2.BytesImg;

            if (img1?.Height * img1?.Width > img2?.Height * img2?.Width)
            {
                w = img1 == null ? 0 : img1.Width;
                h = img1 == null ? 0 : img1.Height;
                img2 = ResizeImage(img2, w, h);
                input_bytes2 = GetByteImg(img2);
            }
            else
            {
                w = img2 == null ? 0 : img2.Width;
                h = img2 == null ? 0 : img2.Height;
                img1 = ResizeImage(img1, w, h);
                input_bytes1 = GetByteImg(img1);
            }
            byte[] bytes = new byte[w * h * 3];

            Parallel.For(0, h, (i) =>
            {
                var index = i * w;
                for (int j = 0; j < w; j++)
                {
                    var idj = index + j;
                    bytes[3 * idj + 2] = (byte)Clamp((input_bytes1[3 * idj + 2] + input_bytes2[3 * idj + 2]) / 2, 0, 255);
                    bytes[3 * idj + 1] = (byte)Clamp((input_bytes1[3 * idj + 1] + input_bytes2[3 * idj + 1]) / 2, 0, 255);
                    bytes[3 * idj + 0] = (byte)Clamp((input_bytes1[3 * idj + 0] + input_bytes2[3 * idj + 0]) / 2, 0, 255);

                }
            });
            Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            img_ret.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);
            WriteImageBytes(img_ret, bytes);
            Image outImg = new(img_ret);
            return outImg;
        }
        public static Image OperationDiff(Image image1, Image image2)
        {
            int w;
            int h;
            Bitmap img1 = image1.Img;
            Bitmap img2 = image2.Img;
            byte[] input_bytes1 = image1.BytesImg;
            byte[] input_bytes2 = image2.BytesImg;

            if (img1?.Height * img1?.Width > img2?.Height * img2?.Width)
            {
                w = img1 == null ? 0 : img1.Width;
                h = img1 == null ? 0 : img1.Height;
                img2 = ResizeImage(img2, w, h);
                input_bytes2 = GetByteImg(img2);
            }
            else
            {
                w = img2 == null ? 0 : img2.Width;
                h = img2 == null ? 0 : img2.Height;
                img1 = ResizeImage(img1, w, h);
                input_bytes1 = GetByteImg(img1);
            }
            byte[] bytes = new byte[w * h * 3];

            Parallel.For(0, h, (i) =>
            {
                var index = i * w;
                for (int j = 0; j < w; j++)
                {
                    var idj = index + j;
                    bytes[3 * idj + 2] = (byte)Clamp(input_bytes1[3 * idj + 2] - input_bytes2[3 * idj + 2], 0, 255);
                    bytes[3 * idj + 1] = (byte)Clamp(input_bytes1[3 * idj + 1] - input_bytes2[3 * idj + 1], 0, 255);
                    bytes[3 * idj + 0] = (byte)Clamp(input_bytes1[3 * idj + 0] - input_bytes2[3 * idj + 0], 0, 255);

                }
            });
            Bitmap img_ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            img_ret.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);
            WriteImageBytes(img_ret, bytes);
            Image outImg = new Image(img_ret);
            return outImg;
        }
        private static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
       
    }
}
