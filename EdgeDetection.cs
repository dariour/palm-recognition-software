using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.Drawing;
using System.Drawing.Imaging;

namespace PalmRecognition
{
    class EdgeDetection
    {
        private Bitmap image;

        public void SetImage(Bitmap image)
        {
            this.image = image;
        }

        public static Bitmap Sobel(Bitmap image)
        {
            int[,] xkernel = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] ykernel = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            int width = image.Width;
            int height = image.Height;
            int xresult, yresult, totalresult;

            Bitmap output = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);

            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                                   PixelFormat.Format8bppIndexed);

            BitmapData outputData = output.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly,
                                   PixelFormat.Format8bppIndexed);

            int bmpStride = bmpData.Stride;
            int outputStride = outputData.Stride;

            //Build a grayscale color Palette
            ColorPalette palette = output.Palette;
            for (int i = 0; i < 255; i++)
            {
                Color tmp = Color.FromArgb(255, i, i, i);
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            output.Palette = palette;

            unsafe
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer();
                byte* outputPtr = (byte*)outputData.Scan0.ToPointer();

                for (int row = 1; row < image.Height - 1; row++)
                    for (int col = 1; col < image.Width - 1; col += 1)
                    {
                        xresult = (bmpPtr[(row - 1) * bmpStride + (col - 1)] * xkernel[0, 0] +
                            bmpPtr[(row - 1) * bmpStride + col] * xkernel[0, 1] +
                            bmpPtr[(row - 1) * bmpStride + (col + 1)] * xkernel[0, 2] +
    
                            bmpPtr[(row) * bmpStride + (col - 1)] * xkernel[1, 0] +
                            bmpPtr[(row) * bmpStride + col] * xkernel[1, 1] +
                            bmpPtr[(row) * bmpStride + (col + 1)] * xkernel[1, 2] +

                            bmpPtr[(row + 1) * bmpStride + (col - 1)] * xkernel[2, 0] +
                            bmpPtr[(row + 1) * bmpStride + col] * xkernel[2, 1] +
                            bmpPtr[(row + 1) * bmpStride + (col + 1)] * xkernel[2, 2]);

                        yresult = (bmpPtr[(row - 1) * bmpStride + (col - 1)] * ykernel[0, 0] +
                            bmpPtr[(row - 1) * bmpStride + col] * ykernel[0, 1] +
                            bmpPtr[(row - 1) * bmpStride + (col + 1)] * ykernel[0, 2] +

                            bmpPtr[(row) * bmpStride + (col - 1)] * ykernel[1, 0] +
                            bmpPtr[(row) * bmpStride + col] * ykernel[1, 1] +
                            bmpPtr[(row) * bmpStride + (col + 1)] * ykernel[1, 2] +

                            bmpPtr[(row + 1) * bmpStride + (col - 1)] * ykernel[2, 0] +
                            bmpPtr[(row + 1) * bmpStride + col] * ykernel[2, 1] +
                            bmpPtr[(row + 1) * bmpStride + (col + 1)] * ykernel[2, 2]);

                        totalresult = (byte)Math.Sqrt((xresult * xresult) + (yresult * yresult));
                        outputPtr[row * bmpStride + col] = (byte)totalresult;
                    }
            }
            image.UnlockBits(bmpData);
            output.UnlockBits(outputData);

            return output;
        }

        

    }
}
