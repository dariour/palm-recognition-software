using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApp1
{
    class Blur
    {
        private Bitmap image;

        public void SetImage(Bitmap image)
        {
            this.image = image;
        }

        public static Bitmap GaussianBlur(Bitmap image)
        {
            double[,] blurkernel = {{0.023528, 0.033969, 0.038393, 0.033969, 0.023528},
                                    {0.033969, 0.049045, 0.055432, 0.049045, 0.033969},
                                    {0.038393, 0.055432, 0.062651, 0.055432, 0.038393},
                                    {0.033969, 0.049045, 0.055432, 0.049045, 0.033969},
                                    {0.023528, 0.033969, 0.038393, 0.033969, 0.023528}};
            int width = image.Width;
            int height = image.Height;

            Bitmap output = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);

            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                                   PixelFormat.Format8bppIndexed);

            BitmapData outputData = output.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly,
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
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer(),
                outputPtr = (byte*)outputData.Scan0.ToPointer();

                for (int row = 2; row < image.Height - 2; row++)
                    for (int col = 2; col < image.Width - 2; col += 1)
                    {
                        outputPtr[row * outputStride + col] = (byte)(
                            bmpPtr[(row - 2) * bmpStride + (col - 2)] * blurkernel[0, 0] +
                            bmpPtr[(row - 2) * bmpStride + (col - 1)] * blurkernel[0, 1] +
                            bmpPtr[(row - 2) * bmpStride + col] * blurkernel[0, 2] +
                            bmpPtr[(row - 2) * bmpStride + (col + 1)] * blurkernel[0, 3] +
                            bmpPtr[(row - 2) * bmpStride + (col - 2)] * blurkernel[0, 4] +

                            bmpPtr[(row - 1) * bmpStride + (col - 2)] * blurkernel[1, 0] +
                            bmpPtr[(row - 1) * bmpStride + (col - 1)] * blurkernel[1, 1] +
                            bmpPtr[(row - 1) * bmpStride + col] * blurkernel[1, 2] +
                            bmpPtr[(row - 1) * bmpStride + (col + 1)] * blurkernel[1, 3] +
                            bmpPtr[(row - 1) * bmpStride + (col - 2)] * blurkernel[1, 4] +

                            bmpPtr[(row) * bmpStride + (col - 2)] * blurkernel[2, 0] +
                            bmpPtr[(row) * bmpStride + (col - 1)] * blurkernel[2, 1] +
                            bmpPtr[(row) * bmpStride + col] * blurkernel[2, 2] +
                            bmpPtr[(row) * bmpStride + (col + 1)] * blurkernel[2, 3] +
                            bmpPtr[(row) * bmpStride + (col + 2)] * blurkernel[2, 4] +

                            bmpPtr[(row + 1) * bmpStride + (col - 2)] * blurkernel[3, 0] +
                            bmpPtr[(row + 1) * bmpStride + (col - 1)] * blurkernel[3, 1] +
                            bmpPtr[(row + 1) * bmpStride + col] * blurkernel[3, 2] +
                            bmpPtr[(row + 1) * bmpStride + (col + 1)] * blurkernel[3, 3] +
                            bmpPtr[(row + 1) * bmpStride + (col + 2)] * blurkernel[3, 4] +

                            bmpPtr[(row + 2) * bmpStride + (col - 2)] * blurkernel[4, 0] +
                            bmpPtr[(row + 2) * bmpStride + (col - 1)] * blurkernel[4, 1] +
                            bmpPtr[(row + 2) * bmpStride + col] * blurkernel[4, 2] +
                            bmpPtr[(row + 2) * bmpStride + (col + 1)] * blurkernel[4, 3] +
                            bmpPtr[(row + 2) * bmpStride + (col + 2)] * blurkernel[4, 4]
                            );
                    }
            }
            image.UnlockBits(bmpData);
            output.UnlockBits(outputData);

            return output;
        }
    }
}
