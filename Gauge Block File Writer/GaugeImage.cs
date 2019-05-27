using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace Gauge_Block_File_Writer
{
    public class NotAlternatingImagesException : Exception
    {
        public NotAlternatingImagesException(string message)
           : base(message)
        {
        }
    }
    public class DisorderedImagesException : Exception
    {
        public DisorderedImagesException(string message)
           : base(message)
        {
        }
    }
    
    enum GaugeColor { Red,Green,Undefined};

    class GaugeImage
    {
        private static GaugeImage[] g_images = new GaugeImage[0];
        private static GaugeImage[] cropped_images = null;
        private static Image current_image;
        private static int image_index = -1;
        private static int crop_index = -1;
        Image gauge_image;
        DateTime d;
        string filename="";
        private GaugeColor clr;
        private static GaugeColor set_clr = GaugeColor.Red;
        
        
        
        public GaugeImage(Image img,bool cropped)
        {
            if (!cropped)
            {
                gauge_image = img;
                image_index++;
                Array.Resize(ref g_images, image_index + 1);
                g_images[image_index] = this;
            }
            else
            {
                gauge_image = img;
                crop_index++;
                Array.Resize(ref cropped_images, crop_index + 1);
                cropped_images[crop_index] = this;
            }
            
        }

        public static Image CurrentImage
        {
            get { return current_image; }
            set { current_image = value; }
        }

        public static GaugeColor SetColour
        {
            get { return set_clr; }
        }
       
        public Color GetDominantColor()
        {
            Bitmap bmp = (Bitmap)gauge_image;
            BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            int[] totals = new int[] { 0, 0, 0 };

            int width = bmp.Width;
            int height = bmp.Height;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;
                            totals[color] += p[idx];
                        }
                    }
                }
            }

            int avgB = totals[0] / (width * height);
            int avgG = totals[1] / (width * height);
            int avgR = totals[2] / (width * height);

            bmp.UnlockBits(srcData);

            return Color.FromArgb(avgR, avgG, avgB);
        }

        public DateTime DateT
        {
            set { d = value; }
            get { return d; }
        }

        public Image Image_
        {
            get { return gauge_image; }
            set { gauge_image = value; }
        }

        public static GaugeImage[] GetCroppedImages
        {
            get { return cropped_images; }
        }
        public static int Number
        {
            get { return image_index+1; }
            set { Number = value; }

        }
        public void InitCroppedArraySize(int size)
        {
            cropped_images = new GaugeImage[size];
        }

        public static GaugeImage[] GaugeImages
        {
            get { return g_images; }
        }
        public static void Dispose()
        {
            g_images = new GaugeImage[0];
            image_index = -1;
        }
        public string Filename
        {
            set { filename = value; }
            get { return filename; }
        }
        public GaugeColor Colour
        {
            set { clr = value; }
            get { return clr; }
        }

        

        public static bool CheckSingleColourImages()
        {
            int j = 0;
            GaugeColor colour = GaugeColor.Undefined;
            foreach (GaugeBlock gauge in GaugeBlock.Gauges)
            {
                if (gauge == null) break;//the last gauge in the list is empty

                //check to see if the image is red
                Color clr = GaugeImages[j].GetDominantColor();
                if (clr.R > clr.B && clr.R > clr.G && (colour == GaugeColor.Red||colour==GaugeColor.Undefined))
                {
                    //check the first image-if it's red then the remaining images must also be red.
                    if (j == 0)
                    {
                        colour = GaugeColor.Red;
                    }
                    
                    gauge.RedImage = GaugeImages[j];
                    gauge.RedDate = GaugeImages[j].DateT;
                    GaugeImages[j].Colour = GaugeColor.Red;
                    gauge.CalculateTPH();
                    set_clr = GaugeColor.Red;
                    j++;
                }
                else if (clr.G >= clr.B && clr.G > clr.R && (colour == GaugeColor.Green || colour == GaugeColor.Undefined))
                {
                    //check the first image-if it's green then the remaining images must also be green.
                    if (j == 0)
                    {
                        colour = GaugeColor.Green;
                    }
                    
                    gauge.GreenImage = GaugeImages[j];
                    gauge.GreenDate = GaugeImages[j].DateT;
                    GaugeImages[j].Colour = GaugeColor.Green;
                    gauge.CalculateTPH();
                    set_clr = GaugeColor.Green;
                    j++;
                }
                else
                {
                    //throw out the gauge images as the're not right.
                    Dispose();
                    return false;
                }
            }
            return true;
        }

        public static bool CheckGreenRed()
        {
            //In this case we should have both green and red images and we can assume that we have either 
            //1) taken images as red/green/red/green/red/green ....... 
            //2) or green/red/green/red/green/red .......  
            //3) or green/red/red/green/green/red/red/green ......
            //4) or red/green/green/red/red/green/green/red/red/green....
            //5) or all red followed by all green 
            //6) or all green followed by all red.
            //The code below attempts to figure this out.
            int j = 0;
            int k = 1;
            

            //first assume an alternating pattern (options 1 to 4, bail out if this pattern is not detected)
            try
            {
                foreach (GaugeBlock gauge in GaugeBlock.Gauges)
                {
                    if (gauge == null) break;
                    //check to see if the image is red
                    Color clrj = GaugeImages[j].GetDominantColor();
                    Color clrk = GaugeImages[k].GetDominantColor();

                    //the jth colour is red.....
                    if (clrj.R > clrj.B && clrj.R > clrj.G)
                    {

                        
                        gauge.RedImage = GaugeImages[j];
                        gauge.RedDate = GaugeImages[j].DateT;
                        GaugeImages[j].Colour = GaugeColor.Red;
                        
                        j = j + 2;

                        //check to make sure the second color is green
                        if (clrk.G > clrk.B && clrk.G > clrk.R)
                        {
                            
                            gauge.GreenImage = GaugeImages[k];
                            GaugeImages[k].Colour = GaugeColor.Green;
                            gauge.GreenDate = GaugeImages[k].DateT;
                           
                            k = k + 2;
                            
                        }
                        else throw new NotAlternatingImagesException("");
                    }
                    //the jth colour is green
                    else if (clrj.G > clrj.B && clrj.G > clrj.R)
                    {
                        
                        gauge.GreenImage = GaugeImages[j];
                        GaugeImages[j].Colour = GaugeColor.Green;
                        gauge.GreenDate = GaugeImages[j].DateT;
                        
                        j = j + 2;
                        //check to make sure the second color is red
                        if (clrk.R > clrk.B && clrk.R > clrk.G)
                        {
                            
                            gauge.RedImage = GaugeImages[k];
                            GaugeImages[k].Colour = GaugeColor.Red;
                            gauge.RedDate = GaugeImages[k].DateT;
                            
                            k = k + 2;
                        }
                        else throw new NotAlternatingImagesException("");
                    }
                    gauge.CalculateTPH();
                }
            }
            catch (NotAlternatingImagesException)
            {
                //the pattern wasn't alternating pattern
                //now we assume the pattern is all red images first and then all green images or visa versa
                try
                {
                    j = 0;
                    k = GaugeImage.Number / 2;
                    foreach (GaugeBlock gauge in GaugeBlock.Gauges)
                    {
                        if (gauge == null) break;
                        //check to see if the image is red
                        Color clrj = GaugeImage.GaugeImages[j].GetDominantColor();
                        Color clrk = GaugeImage.GaugeImages[k].GetDominantColor();

                        //Check if the first colour is red.....
                        if (clrj.R > clrj.B && clrj.R > clrj.G)
                        {
                            
                            gauge.RedImage = GaugeImages[j];
                            GaugeImages[j].Colour = GaugeColor.Red;
                            gauge.RedDate = GaugeImages[j].DateT;
                            

                            j++;
                            //check to make sure the second color is green
                            if (clrk.G >= clrk.B && clrk.G >= clrk.R)
                            {
                                
                                gauge.GreenImage = GaugeImages[k];
                                GaugeImages[k].Colour = GaugeColor.Green;
                                gauge.GreenDate = GaugeImages[k].DateT;
                                
                                k++;
                            }
                            else throw new DisorderedImagesException("");
                        }
                        //the first colour is green
                        else if (clrj.G >= clrj.B && clrj.G > clrj.R)
                        {
                            
                            gauge.GreenImage = GaugeImages[j];
                            GaugeImages[j].Colour = GaugeColor.Green;
                            gauge.GreenDate = GaugeImages[j].DateT;
                            
                            j++;

                            //check to make sure the second color is red
                            if (clrk.R > clrk.B && clrk.R > clrk.G)
                            {
                                
                                gauge.RedImage = GaugeImages[k];
                                GaugeImages[k].Colour = GaugeColor.Red;
                                gauge.RedDate = GaugeImages[k].DateT;
                                
                                k++;
                            }
                            else throw new DisorderedImagesException("");
                        }
                        gauge.CalculateTPH();
                    }
                }
                catch (DisorderedImagesException)
                {
                    Dispose();
                    
                    return false;
                }
                
            }
            
            return true;
        }
        public static int CropIndex
        {
            get { return crop_index; }
            set { crop_index = value; }
        }
    }
}
