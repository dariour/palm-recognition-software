using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace PalmRecognition
{
    class Treshold
    {
        private Bitmap image;

        public void SetImage(Bitmap image)
        {
            this.image = image;
        }

        public static int[] getHistogram(Bitmap image)
        {
            int[] histogram = new int[256];

            //Lock the images
            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly,
                                   PixelFormat.Format8bppIndexed);
            unsafe
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer();

                for (int row = 0; row < image.Height; row++)
                    for (int col = 0; col < image.Width; col += 1)
                        histogram[bmpPtr[row * bmpData.Stride + col]]++;
            }

            for (int i = 0; i < 256; i++) System.Console.WriteLine("Shade {0}: {1}", i, histogram[i]);

            image.UnlockBits(bmpData);

            return histogram;
        }

        public static int getTreshholdValue(Bitmap image)
        {
            int dataLength = image.Height * image.Width;

            int[] histogram = getHistogram(image);

            float sum = 0;
            for(int i = 0; i < 256; i++)
            {
                sum = sum + (i * histogram[i]);
            }

            float sumBackground = 0;
            int weightBackground = 0, weightForeground = 0;
            float maxVariance = 0, threshold = 0;

            for(int i = 0; i < 256; i++)
            {
                weightBackground = weightBackground + histogram[i];
                if (weightBackground == 0) continue;

                weightForeground = dataLength - weightBackground;
                if (weightForeground == 0) break;

                sumBackground = sumBackground + (i * histogram[i]);

                float meanBackground = sumBackground / weightBackground;
                float meanForeground = (sum - sumBackground) / weightForeground;

                //Between Class Variance
                float betweenVariance = weightBackground * weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);

                //If new max found
                if(betweenVariance > maxVariance)
                {
                    maxVariance = betweenVariance;
                    threshold = i;
                }
            }
            System.Console.WriteLine(threshold);
            return (int)threshold;
        }

        public static Bitmap applyThresholdToImage(Bitmap image, int threshold)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap output = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);


            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                                   PixelFormat.Format8bppIndexed);

            BitmapData outputData = output.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly,
                                   PixelFormat.Format8bppIndexed);

            unsafe
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer();
                byte* outputPtr = (byte*)outputData.Scan0.ToPointer();

                for (int row = 0; row < image.Height; row++)
                    for (int col = 0; col < image.Width; col += 1)
                    {
                        if (bmpPtr[row * bmpData.Stride + col] > threshold) outputPtr[row * bmpData.Stride + col] = 255;
                        else outputPtr[row * bmpData.Stride + col] = 0;
                    }     
            }
            image.UnlockBits(bmpData);
            output.UnlockBits(outputData);

            return output;
        }
    }
}
