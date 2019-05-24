using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;


namespace Gauge_Block_File_Writer
{
    public partial class FileWriterForm : Form
    {
        
        Boolean mouseClicked;
        Point startPoint = new Point();
        Point endPoint = new Point();
        Rectangle rectCropArea;
        private int gi = 0;
        bool current_red = true;
        string cropped_file_path = "";
        string client_name = "";
        private bool called_from_metric;
        private bool called_from_imperial;
        private static readonly string[] _validExtensions = { "jpg", "bmp", "gif", "png" }; //  etc
        Bitmap cropped;
        double width_ratio;
        double height_ratio;




        public FileWriterForm()
        {
            InitializeComponent();
            called_from_metric = false; 
            called_from_imperial = false;
            mouseClicked = false;
            GaugeBlock.ThermalExp = 9.5/1000000;
        }

        private void Metric_CheckedChanged(object sender, EventArgs e)
        {
            if (!called_from_imperial)
            {
                //we dont want both of these checked at once
                if (Imperial.Checked)
                {
                    called_from_metric = true;
                    Imperial.Checked = false;
                    
                }
                else if (!Imperial.Checked)
                {
                    called_from_metric = false;
                }

                //if metric is checked 
                if (Metric.Checked)
                {
                    //disable the imperial combo box list
                    ImperialComboBox.Enabled = false;
                    ImperialComboBox.SendToBack();
                    //show the metric combo box list
                    MetricComboBox.Enabled = true;
                    MetricComboBox.BringToFront();
                }
            }
            else called_from_imperial = false;
            
        }

        private void Imperial_CheckedChanged(object sender, EventArgs e)
        {
            if (!called_from_metric)
            {
                //we dont want both of these checked at once
                if (Metric.Checked)
                {
                    called_from_imperial = true;
                    Metric.Checked = false;
                }


                //if metric is checked 
                if (Imperial.Checked)
                {
                    //hide the metric combo box list
                    MetricComboBox.Enabled = false;
                    MetricComboBox.SendToBack();
                    //show the imperial combo box list
                    ImperialComboBox.Enabled = true;
                    ImperialComboBox.BringToFront();
                   
                }
            }
            else called_from_metric = false;
            
            
        }

        private void ImperialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int remove_index = -1;
            
            gaugeListRichTextBox.Clear();
            SiderichTextBox.Clear();
            
            double size = Convert.ToDouble(ImperialComboBox.SelectedItem.ToString());

            GaugeBlock new_gauge = new GaugeBlock(size * 25.4);

            int i = 0;
            foreach (GaugeBlock gauge in GaugeBlock.Gauges)
            {
                if (gauge != null)
                {
                    gaugeListRichTextBox.AppendText((gauge.Size/25.4).ToString() + "\n");
                    GaugeBlock.Gauges[i].Side = Convert.ToInt16(SideComboBox.Text);
                    SiderichTextBox.AppendText(SideComboBox.Text.ToString() + "\n");
                    i++;
                }
            }
            
            if (gaugeListRichTextBox.Lines.Count() == numberOfGaugesUpDown.Value+1) PictureFolder.Enabled = true;
        }

        private void MetricComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            gaugeListRichTextBox.Clear();
            SiderichTextBox.Clear();
            double size = Convert.ToDouble(MetricComboBox.SelectedItem.ToString());
            GaugeBlock new_gauge = new GaugeBlock(size);
            
            int i = 0;
            foreach (GaugeBlock gauge in GaugeBlock.Gauges)
            {

                if (gauge != null)
                {
                    gaugeListRichTextBox.AppendText(gauge.Size.ToString() + "\n");
                    GaugeBlock.Gauges[i].Side = Convert.ToInt16(SideComboBox.Text);
                    SiderichTextBox.AppendText(SideComboBox.Text.ToString() + "\n");
                    i++;
                }
            }
            
            if(gaugeListRichTextBox.Lines.Count() == numberOfGaugesUpDown.Value+1) PictureFolder.Enabled = true;
        }

       

        private void PictureFolder_Click(object sender, EventArgs e)
        {
            //check if the number of lines in the rich textbox matches the chosen number of gauges.  Remember that there are two line of text at the start.
            if(GaugeBlock.NumGauges!= gaugeListRichTextBox.Lines.Length-1)
            {
                MessageBox.Show("Number of chosen gauges doesn't match selected number of gauges");
                return;
            }
            using (var fbd = new FolderBrowserDialog())
            {
                string uncropped_file_path = "";
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.SelectedPath = "I:\\MSL\\Private\\LENGTH\\Hilger";

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath;
                    uncropped_file_path = path;  //save the file path
                    cropped_file_path = uncropped_file_path + "\\cropped";
                    if(!Directory.Exists(cropped_file_path)) Directory.CreateDirectory(cropped_file_path);
                    
                    foreach (string filename in Directory.GetFiles(path))
                    {
                        FileInfo fi = new FileInfo(filename);
                        string ext = fi.Extension.ToLower();
                        foreach (string a in _validExtensions)
                        {
                            if (ext.Contains(a))
                            {
                                //we have found a valid picture file
                                Image image = Image.FromFile(filename);
                                GaugeImage gi = new GaugeImage(image,false);
                                gi.Filename = filename; //give the gauge image
                                gi.DateT = fi.LastWriteTime; //give the gauge image a datetime
                                break;
                            }
                        }
                    }
                    if (GaugeImage.Number == 0)
                    {
                        MessageBox.Show("The choosen directory contains no image files");
                        return;
                    }
                    else if (GaugeImage.Number != GaugeBlock.NumGauges && GaugeImage.Number != GaugeBlock.NumGauges * 2)
                    {
                        MessageBox.Show("For selected number of gauges the number of image files in the chosen directory is incorrect");
                        return;
                    }
                    else if (GaugeImage.Number == GaugeBlock.NumGauges)
                    {
                        //we either have all red or all green images, but not both
                        if (!GaugeImage.CheckSingleColourImages()) MessageBox.Show("An Image was detected which was not the correct colour");
                        return;
                    }
                    else if (GaugeImage.Number == GaugeBlock.NumGauges * 2)
                    {
                        if (!GaugeImage.CheckGreenRed()) MessageBox.Show("Images in the file are not sequenced correctly");
                    }
                }

                else return;

                //if we get to here print the image files with their colours to the gui
                foreach (GaugeImage imge in GaugeImage.GaugeImages)
                {
                    if (imge.Colour == GaugeColor.Red)
                    {
                        uncroppedRichTextBox.AppendText(imge.Filename + " RED\n");
                    }
                    else uncroppedRichTextBox.AppendText(imge.Filename + " GREEN\n");
                }
                
                //load the first gauge blocks red image into the uncropped picture box
                LoadImageToCrop(GaugeBlock.Gauges[gi].RedImage.Image_);
                GaugeFileNameLabel.Text = GaugeBlock.Gauges[gi].RedImage.Filename.ToString();
                
            }
        }

        private void LoadImageToCrop(Image i)
        {
            GaugeImage.CurrentImage = i;
            int image_w = i.Width;
            int image_h = i.Height;
            int pic_w = srcPicBox.Width;
            int pic_h = srcPicBox.Height;
            width_ratio = image_w / pic_w;
            height_ratio = image_h / pic_h;
        
            
            //Load up the first image into the picture box, remembering to scale it first
            srcPicBox.Image = i;
            Bitmap sourceBitmap = new Bitmap(srcPicBox.Image, pic_w, pic_h);
            srcPicBox.Image = sourceBitmap;
            srcPicBox.Refresh();
        }

        private void NumberOfGaugesUpDown_ValueChanged(object sender, EventArgs e)
        {
            if(numberOfGaugesUpDown.Value == 0)
            {
                //grey all other form controls out
                Imperial.Enabled = false;
                Metric.Enabled = false;
                ImperialComboBox.Enabled = false;
                MetricComboBox.Enabled = false;
                PictureFolder.Enabled = false;     
            }
            else 
            {
                //enable the check boxes
                Imperial.Enabled = true;
                Metric.Enabled = true;
                ImperialComboBox.Enabled = false;
                MetricComboBox.Enabled = false;
                PictureFolder.Enabled = false;
                GaugeBlock.NumGauges = (int) numberOfGaugesUpDown.Value;
            }
        }
        private void SrcPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClicked = true;

            startPoint.X = e.X;
            startPoint.Y = e.Y;
            // Display coordinates 

            endPoint.X = -1;
            endPoint.Y = -1;

            rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
        }
        private void SrcPicBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClicked = false;

            if (endPoint.X != -1)
            {
                Point currentPoint = new Point(e.X, e.Y);
            }
            endPoint.X = -1;
            endPoint.Y = -1;
            startPoint.X = -1;
            startPoint.Y = -1;

        }
        private void SrcPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point ptCurrent = new Point(e.X, e.Y);

            if (mouseClicked)
            {
                endPoint = ptCurrent;

                if (e.X > startPoint.X && e.Y > startPoint.Y)
                {
                    rectCropArea.Width = e.X - startPoint.X;
                    rectCropArea.Height = e.Y - startPoint.Y;
                }
                else if (e.X < startPoint.X && e.Y > startPoint.Y)
                {
                    rectCropArea.Width = startPoint.X - e.X;
                    rectCropArea.Height = e.Y - startPoint.Y;
                    rectCropArea.X = e.X;
                    rectCropArea.Y = startPoint.Y;
                }
                else if (e.X > startPoint.X && e.Y < startPoint.Y)
                {
                    rectCropArea.Width = e.X - startPoint.X;
                    rectCropArea.Height = startPoint.Y - e.Y;
                    rectCropArea.X = startPoint.X;
                    rectCropArea.Y = e.Y;
                }
                else
                {
                    rectCropArea.Width = startPoint.X - e.X;
                    rectCropArea.Height = startPoint.Y - e.Y;
                    rectCropArea.X = e.X;
                    rectCropArea.Y = e.Y;
                }
                srcPicBox.Refresh();
            }
        }

        private void SrcPicBox_MouseClick(object sender, MouseEventArgs e)
        {
            // to remove the dashes 
            srcPicBox.Refresh();
        }

        private void SrcPicBox_Paint(object sender, PaintEventArgs e)
        {
            Pen drawLine = new Pen(Color.Red);
            drawLine.DashStyle = DashStyle.Dash;
            e.Graphics.DrawRectangle(drawLine, rectCropArea);
        }

        private void BtnCrop_Click(object sender, EventArgs e)
        {
            srcPicBox.Refresh();
            Bitmap sourceBitmap = new Bitmap(srcPicBox.Image, srcPicBox.Width, srcPicBox.Height);
            //Bitmap sourceBitmap = new Bitmap(GaugeImage.CurrentImage, GaugeImage.CurrentImage.Width, GaugeImage.CurrentImage.Height);


            Graphics g = desinationPictureBox.CreateGraphics();
            //Rectangle rt = new Rectangle((int) (rectCropArea.X*width_ratio),(int) (rectCropArea.Y*height_ratio), (int) (rectCropArea.Width*width_ratio), (int) (rectCropArea.Height*height_ratio));
            Rectangle rt = new Rectangle(rectCropArea.X, rectCropArea.Y, rectCropArea.Width, rectCropArea.Height);

            cropped = sourceBitmap.Clone(rt, sourceBitmap.PixelFormat);
            desinationPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            desinationPictureBox.Image = cropped;
            desinationPictureBox.Refresh();
            sourceBitmap.Dispose();
            Save_Cropped_Image.Enabled = true;
        }

        private void Save_Cropped_Image_Click(object sender, EventArgs e)
        {
            bool exists = false;
            int gaugei = 0;
            string fname = "";
            if (desinationPictureBox.Image == null) return;

            //Image image_ = desinationPictureBox.Image;

            if (current_red && GaugeBlock.Gauges[gi] !=null) {
                if (PlatenNumericUpDown.Value < 10)
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-R.jpg";
                    else fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-R.jpg";
                }
                else
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-R.jpg";
                    else fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-R.jpg";
                }

                //search through the existing gauges to see if we already have this cropped image
                foreach (GaugeBlock gauge in GaugeBlock.Gauges)
                {
                    if (gauge == null) break;
                    gaugei++;
                    if (gauge.RedCropped != null)
                    {
                        if (gauge.RedCropped.Filename == fname)
                        {
                            exists = true;
                            break;
                        }
                    }
                }
              
                if (!exists)
                {
                    GaugeBlock.Gauges[gi].RedCropped = new GaugeImage(cropped, true); //add a new image
                    GaugeBlock.Gauges[gi].RedCropped.Filename = fname;
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gi].RedCropped.Filename,ImageFormat.Jpeg);
                    CroppedGaugeNameLabel.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gi].RedCropped.Filename;
                }
                else
                {
                    GaugeBlock.Gauges[gaugei].RedCropped.Filename = fname;
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].RedCropped.Filename);
                    CroppedGaugeNameLabel.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].RedCropped.Filename;
                }
                Save_Cropped_Image.Enabled = false;
                gaugei = 0;
                exists = false;

            }
            else if(!current_red && GaugeBlock.Gauges[gi] != null)
            {
                if (PlatenNumericUpDown.Value < 10)
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-G.jpg";
                    else fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-G.jpg";
                }
                else
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-G.jpg";
                    else fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-G.jpg";
                }
                //search through the existing gauges to see if we already have this cropped image
                foreach (GaugeBlock gauge in GaugeBlock.Gauges)
                {
                    if (gauge == null) break;
                    gaugei++;
                    if (gauge.GreenCropped != null)
                    {
                        if (gauge.GreenCropped.Filename == fname)
                        {
                            exists = true;
                            break;
                        }
                    }
                }
                if (!exists)
                {
                    GaugeBlock.Gauges[gi].GreenCropped = new GaugeImage(cropped, true); //add a new image
                    GaugeBlock.Gauges[gi].GreenCropped.Filename = fname;
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gi].GreenCropped.Filename,ImageFormat.Jpeg);
                    CroppedGaugeNameLabel.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gi].GreenCropped.Filename;
                }
                else
                {
                    GaugeBlock.Gauges[gaugei].GreenCropped.Filename = fname;
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].GreenCropped.Filename);
                    CroppedGaugeNameLabel.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].GreenCropped.Filename;
                }
                Save_Cropped_Image.Enabled = false;
                gaugei = 0;
                exists = false;

            }
            else
            {
                MessageBox.Show("All gauges cropped");
            }
            CroppedRichTextBox.Clear();
            foreach(GaugeBlock gauge in GaugeBlock.Gauges)
            {
                if (gauge != null)
                {
                    if (gauge.RedCropped != null)
                    {
                        CroppedRichTextBox.AppendText(gauge.RedCropped.Filename + "\n");
                    }
                    if (gauge.GreenCropped != null)
                    {
                        CroppedRichTextBox.AppendText(gauge.GreenCropped.Filename + "\n");
                    }
                }
            }
            if (CroppedRichTextBox.Lines.Count() == numberOfGaugesUpDown.Value) WriteTextFileButton.Enabled = true;
        }

        private void LoadSourceImage_Click(object sender, EventArgs e)
        {
            if (current_red && GaugeBlock.Gauges[gi] != null)
            {
                current_red = false;
                //load the next image to be cropped
                LoadImageToCrop(GaugeBlock.Gauges[gi].GreenImage.Image_);
                GaugeFileNameLabel.Text = GaugeBlock.Gauges[gi].GreenImage.Filename.ToString();
            }
            else if (!current_red && GaugeBlock.Gauges[gi] != null)
            {
                current_red = true;

                //if the image is not null load the next image to be cropped
                if (GaugeBlock.Gauges[gi + 1] != null)
                {
                    LoadImageToCrop(GaugeBlock.Gauges[gi + 1].RedImage.Image_);
                    GaugeFileNameLabel.Text = GaugeBlock.Gauges[gi + 1].RedImage.Filename.ToString();

                    //increment the gauge index
                    gi++;
                }
                else
                {
                    gi = 0;
                    LoadImageToCrop(GaugeBlock.Gauges[gi].RedImage.Image_);
                    GaugeFileNameLabel.Text = GaugeBlock.Gauges[gi].RedImage.Filename.ToString();
                    
                }
               
            }
            else gi = 0;
        }

        private void ThermExpUpDown_ValueChanged(object sender, EventArgs e)
        {
            GaugeBlock.ThermalExp = Convert.ToDouble(ThermExpUpDown.Value)/1000000;
        }

        private void ClientName_TextChanged(object sender, EventArgs e)
        {
            client_name = ClientName.Text;
        }

     

        private void SideComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void WriteTextFileButton_Click(object sender, EventArgs e)
        {
            StreamWriter writer = File.CreateText(cropped_file_path + "\\python_input_file.txt");
            foreach(GaugeBlock gauge in GaugeBlock.Gauges)
            {
                if (gauge == null) break;
                string size = (gauge.Size/25.4).ToString();
                string serial_number = "na";
                string date_red = gauge.RedDate.ToOADate().ToString();
                string date_green = gauge.GreenDate.ToOADate().ToString();
                string client_name = ClientName.Text;
                string platen = PlatenNumericUpDown.Value.ToString();
                string observer = "CMY";
                string side = gauge.Side.ToString();
                string thermexp = GaugeBlock.ThermalExp.ToString();
                string units = "";
                if (Metric.Checked == true) units = "Metric";
                else units = "Imperial";
                string t_red = gauge.TRed.ToString();
                string t_green = gauge.TGreen.ToString();
                string t_red_platen = gauge.TRedPlaten.ToString();
                string t_green_platen = gauge.TGreenPlaten.ToString();
                string p_red = gauge.PRed.ToString();
                string p_green = gauge.PGreen.ToString();
                string h_red = gauge.HRed.ToString();
                string h_green = gauge.HGreen.ToString();
                string position = "0";
                string red_file = cropped_file_path + "\\" + gauge.RedCropped.Filename.ToString();
                string green_file = cropped_file_path + "\\" + gauge.GreenCropped.Filename.ToString();
                if(OutputCheckBox.Checked) writer.WriteLine(size + "," + serial_number + "," + date_red + "," + date_green + "," + client_name + "," + platen + "," + observer + "," + side + "," + thermexp + "," + units + "," + t_red + "," + t_green + "," + t_red_platen + "," + t_green_platen + "," + p_red + "," + p_green + "," + h_red + "," + h_green + "," + position + "," + red_file + "," + green_file);
                else writer.WriteLine(size + "," + serial_number + "," + date_red + "," + date_green + "," + client_name + "," + platen + "," + observer + "," + side + "," + thermexp + "," + units  + "," + t_red_platen + "," + t_green_platen + "," + p_red + "," + p_green + "," + h_red + "," + h_green + "," + position + "," + red_file + "," + green_file);
            }
            writer.Close();
        }
    }
}
