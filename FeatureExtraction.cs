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
    class FeatureExtraction
    {
        private Bitmap image;

        public void SetImage(Bitmap image)
        {
            this.image = image;
        }

        public static Bitmap upperCentre(Bitmap image)
        {
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

                for (int row = 0; row < image.Height; row++)
                    for (int col = 0; col < image.Width; col += 1)
                    {

                    }
            }
            image.UnlockBits(bmpData);
            output.UnlockBits(outputData);

            return output;
        }
    }
}
