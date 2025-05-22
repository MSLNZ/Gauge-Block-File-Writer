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
using System.Threading;


namespace Gauge_Block_File_Writer
{
    public delegate void PrintPictureList();
    public delegate void ChangeProgressBar(int progress, bool visable);
    public partial class FileWriterForm : Form
    {

        Boolean mouseClicked;
        Point startPoint = new Point();
        Point endPoint = new Point();
        Rectangle rectCropArea;
        private int gi = 0;
        bool current_red = true;
        bool single_colour = false;
        string cropped_file_path = "";
        string client_name = "";
        private bool called_from_metric;
        private bool called_from_imperial;
        private static readonly string[] _validExtensions = { "jpg", "bmp", "gif", "png" }; //  etc
        Bitmap cropped;
        double width_ratio;
        double height_ratio;
        PrintPictureList pl;
        ChangeProgressBar cpb;
        int[][][] bmpg_array;




        public FileWriterForm()
        {
            InitializeComponent();
            called_from_metric = false;
            called_from_imperial = false;
            mouseClicked = false;
            GaugeBlock.ThermalExp = 9.5 / 1000000;
            pl = new PrintPictureList(PrintPicturesAndColours);
            cpb = new ChangeProgressBar(UpdateProgressBar);

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
                    gaugeListRichTextBox.AppendText((gauge.Size / 25.4).ToString() + "\n");
                    GaugeBlock.Gauges[i].Side = Convert.ToInt16(SideComboBox.Text);
                    SiderichTextBox.AppendText(SideComboBox.Text.ToString() + "\n");
                    i++;
                }
            }

            if (gaugeListRichTextBox.Lines.Count() == numberOfGaugesUpDown.Value + 1) PictureFolder.Enabled = true;
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

            if (gaugeListRichTextBox.Lines.Count() == numberOfGaugesUpDown.Value + 1) PictureFolder.Enabled = true;
        }

        public void UpdateProgressBar(int v, bool visable)
        {
            if (!this.InvokeRequired)
            {
                pbar.Value = v;
                pbar.Visible = visable;
                pbar.BringToFront();

            }
            else
            {
                object[] textobj = { v, visable };
                this.BeginInvoke(cpb, textobj);
            }
        }

        private void PictureFolder_Click(object sender, EventArgs e)
        {

            Thread proc_picture_info;
            //check if the number of lines in the rich textbox matches the chosen number of gauges.  Remember that there are two line of text at the start.
            if (GaugeBlock.NumGauges != gaugeListRichTextBox.Lines.Length - 1)
            {
                MessageBox.Show("Number of chosen gauges doesn't match selected number of gauges");
                return;
            }
            using (var fbd = new FolderBrowserDialog())
            {
                string uncropped_file_path = "";
                //fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.SelectedPath = @"L:\Jobs";

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath;
                    uncropped_file_path = path;  //save the file path
                    cropped_file_path = uncropped_file_path + "\\cropped";
                    if (!Directory.Exists(cropped_file_path)) Directory.CreateDirectory(cropped_file_path);

                    //get the list of file paths refering to the pictures
                    string[] files = Directory.GetFiles(path);
                    DateTime[] lastWriteTimes = new DateTime[files.Length];
                    for (int i = 0; i < files.Length; i++)
                        lastWriteTimes[i] = new FileInfo(files[i]).LastWriteTime;
                    Array.Sort(lastWriteTimes, files);
                    pbar.Minimum = 0;
                    pbar.Maximum = files.Length;
                    pbar.Value = 0;

                    pbar.Name = "Processing Images";
                    Controls.Add(pbar);
                    pbar.Enabled = true;
                    pbar.Style = ProgressBarStyle.Continuous;
                    pbar.Location = new System.Drawing.Point(500, 77);
                    pbar.Width = 700;
                    pbar.Height = 40;
                    pbar.BringToFront();

                    proc_picture_info = new Thread(new ParameterizedThreadStart(ProcFiles));
                    proc_picture_info.IsBackground = true;

                    proc_picture_info.Start(files);
                }
            }
        }



        private void ProcFiles(object stateInfo)
        {
            string[] files = (string[])stateInfo;
            bool set = false;

            foreach (string filename in files)
            {

                FileInfo fi = new FileInfo(filename);
                string ext = fi.Extension.ToLower();
                foreach (string a in _validExtensions)
                {
                    if (ext.Contains(a))
                    {
                        //we have found a valid picture file
                        Bitmap image = new Bitmap(Image.FromFile(filename));
                        GaugeImage gi = new GaugeImage(image, false);
                        if (set == false)
                        {
                            set = true;
                            GaugeImage.setPBar(ref cpb);//set a delegate to update the GUI progress bar (just once)
                        }
                        gi.Filename = filename; //give the gauge image a file name
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
                single_colour = true;
                //we either have all red or all green images, but not both
                if (!GaugeImage.CheckSingleColourImages()) MessageBox.Show("An Image was detected which was not the correct colour");
                return;
            }
            else if (GaugeImage.Number == GaugeBlock.NumGauges * 2)
            {
                single_colour = false;
                if (!GaugeImage.CheckGreenRed()) MessageBox.Show("Images in the file are not sequenced correctly");
            }
            UpdateProgressBar(pbar.Maximum, false);

            //invoke the GUI to print the picture files.
            PrintPicturesAndColours();


        }
        private void PrintPicturesAndColours()
        {
            if (!this.InvokeRequired)
            {
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
                gaugeFileName.Text = GaugeBlock.Gauges[gi].RedImage.Filename.ToString();
                BtnCrop.Enabled = true;
                LoadSourceImage.Enabled = true;
            }
            else
            {
                this.BeginInvoke(pl);
            }
        }

        private void LoadImageToCrop(Image i)
        {
            GaugeImage.CurrentImage = i;
            int image_w = i.Width;
            int image_h = i.Height;
            int pic_w = srcPicBox.Width;
            int pic_h = srcPicBox.Height;
            width_ratio = (double)image_w / pic_w;
            height_ratio = (double)image_h / pic_h;


            //Load up the first image into the picture box, remembering to scale it first
            srcPicBox.Image = i;
            Bitmap sourceBitmap = new Bitmap(srcPicBox.Image, pic_w, pic_h);
            srcPicBox.Image = sourceBitmap;
            srcPicBox.Refresh();
        }

        private void NumberOfGaugesUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (numberOfGaugesUpDown.Value == 0)
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
                GaugeBlock.NumGauges = (int)numberOfGaugesUpDown.Value;
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
            //Bitmap sourceBitmap = new Bitmap(srcPicBox.Image, srcPicBox.Width, srcPicBox.Height);
            Bitmap sourceBitmap = new Bitmap(GaugeImage.CurrentImage, GaugeImage.CurrentImage.Width, GaugeImage.CurrentImage.Height);


            Graphics g = desinationPictureBox.CreateGraphics();
            Rectangle rt = new Rectangle((int)(rectCropArea.X * width_ratio), (int)(rectCropArea.Y * height_ratio), (int)(rectCropArea.Width * width_ratio), (int)(rectCropArea.Height * height_ratio));
            //Rectangle rt = new Rectangle(rectCropArea.X, rectCropArea.Y, rectCropArea.Width, rectCropArea.Height);
            try
            {
                cropped = sourceBitmap.Clone(rt, sourceBitmap.PixelFormat);
            }
            catch (System.ArgumentException s)
            {
                MessageBox.Show(s.Message);
                return;
            }
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

            if (current_red && GaugeBlock.Gauges[gi] != null) {
                if (PlatenNumericUpDown.Value < 10)
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-R.bmp";
                    else fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-R.bmp";
                }
                else
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-R.bmp";
                    else fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-R.bmp";
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
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gi].RedCropped.Filename, ImageFormat.Bmp);
                    croppedImageFileName.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gi].RedCropped.Filename;
                }
                else
                {
                    GaugeBlock.Gauges[gaugei].RedCropped.Filename = fname;
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].RedCropped.Filename);
                    croppedImageFileName.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].RedCropped.Filename;
                }
                Save_Cropped_Image.Enabled = false;
                gaugei = 0;
                exists = false;

            }
            else if (!current_red && GaugeBlock.Gauges[gi] != null)
            {
                if (PlatenNumericUpDown.Value < 10)
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-G.bmp";
                    else fname = ClientName.Text.ToString() + "-0" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-G.bmp";
                }
                else
                {
                    if (gi < 9) fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_0" + (gi + 1).ToString() + "-G.bmp";
                    else fname = ClientName.Text.ToString() + "-" + PlatenNumericUpDown.Value.ToString() + "_" + (gi + 1).ToString() + "-G.bmp";
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
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gi].GreenCropped.Filename, ImageFormat.Bmp);
                    croppedImageFileName.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gi].GreenCropped.Filename;
                }
                else
                {
                    GaugeBlock.Gauges[gaugei].GreenCropped.Filename = fname;
                    cropped.Save(cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].GreenCropped.Filename);
                    croppedImageFileName.Text = cropped_file_path + "\\" + GaugeBlock.Gauges[gaugei].GreenCropped.Filename;
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
            foreach (GaugeBlock gauge in GaugeBlock.Gauges)
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
            if (single_colour)
            {
                if (CroppedRichTextBox.Lines.Count() >= numberOfGaugesUpDown.Value) WriteTextFileButton.Enabled = true;
            }
            else
            {
                if (CroppedRichTextBox.Lines.Count() >= numberOfGaugesUpDown.Value * 2) WriteTextFileButton.Enabled = true;
            }

        }

        private void LoadSourceImage_Click(object sender, EventArgs e)
        {
            if (current_red && GaugeBlock.Gauges[gi] != null)
            {
                current_red = false;
                //load the next image to be cropped
                LoadImageToCrop(GaugeBlock.Gauges[gi].GreenImage.Image_);
                gaugeFileName.Text = GaugeBlock.Gauges[gi].GreenImage.Filename.ToString();
            }
            else if (!current_red && GaugeBlock.Gauges[gi] != null)
            {
                current_red = true;

                //if the image is not null load the next image to be cropped
                if (GaugeBlock.Gauges[gi + 1] != null)
                {
                    LoadImageToCrop(GaugeBlock.Gauges[gi + 1].RedImage.Image_);
                    gaugeFileName.Text = GaugeBlock.Gauges[gi + 1].RedImage.Filename.ToString();

                    //increment the gauge index
                    gi++;
                }
                else
                {
                    gi = 0;
                    LoadImageToCrop(GaugeBlock.Gauges[gi].RedImage.Image_);
                    gaugeFileName.Text = GaugeBlock.Gauges[gi].RedImage.Filename.ToString();

                }

            }
            else gi = 0;
        }

        private void ThermExpUpDown_ValueChanged(object sender, EventArgs e)
        {
            GaugeBlock.ThermalExp = Convert.ToDouble(ThermExpUpDown.Value) / 1000000;
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
            foreach (GaugeBlock gauge in GaugeBlock.Gauges)
            {
                if (gauge == null) break;
                string size = "";
                if (Metric.Checked) size = (gauge.Size).ToString();
                else size = (gauge.Size / 25.4).ToString();
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
                string red_file = cropped_file_path + "\\" + gauge.RedCropped.Filename.ToString();
                string green_file = cropped_file_path + "\\" + gauge.GreenCropped.Filename.ToString();
                if (single_colour)
                {
                    if (GaugeImage.SetColour == GaugeColor.Red) writer.WriteLine(size + "," + serial_number + "," + date_red + "," + client_name + "," + platen + "," + observer + "," + side + "," + thermexp + "," + units + "," + t_red + "," + t_red_platen + "," + p_red + "," + h_red + "," + red_file);
                    else writer.WriteLine(size + "," + serial_number + "," + date_green + "," + client_name + "," + platen + "," + observer + "," + side + "," + thermexp + "," + units + "," + t_green + "," + t_green_platen + "," + p_green + "," + h_green + "," + green_file);
                }
                else writer.WriteLine(size + "," + serial_number + "," + date_red + "," + date_green + "," + client_name + "," + platen + "," + observer + "," + side + "," + thermexp + "," + units + "," + t_red + "," + t_green + "," + t_red_platen + "," + t_green_platen + "," + p_red + "," + p_green + "," + h_red + "," + h_green + "," + red_file + "," + green_file);
            }
            writer.Close();
        }

        private void FileWriterForm_Load(object sender, EventArgs e)
        {
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            this.Size = new System.Drawing.Size(Convert.ToInt32(0.5 * workingRectangle.Width), Convert.ToInt32(0.5 * workingRectangle.Height));
            this.Location = new System.Drawing.Point(10, 10);
        }

        private void backgroundRed_Click(object sender, EventArgs e)
        {

        }

        private void backgroundGreen_Click(object sender, EventArgs e)
        {
            Thread proc_picture_info;
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = @"L:\Jobs";

                DialogResult result = ofd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {


                    Bitmap green_image = new Bitmap(Image.FromFile(ofd.FileName));
                    
                    proc_picture_info = new Thread(new ParameterizedThreadStart(procGreenBackground));
                    proc_picture_info.IsBackground = true;

                    proc_picture_info.Start(green_image);
                }
            }
        }
        private void procGreenBackground_(object stateInfo)
        {

            Bitmap green_image_background = (Bitmap)stateInfo;
            Bitmap greyscale_green_background = MakeGrayscale3(green_image_background);
            string path = "C:\\Users\\c.young\\Documents\\image_corrected.bmp";
            greyscale_green_background.Save(path);



        }
        private void procGreenBackground(object stateInfo)
        {

            Bitmap green_image_background = (Bitmap)stateInfo;
            Bitmap greyscale_green_background = MakeGrayscale3(green_image_background);
            greyscale_green_background.Save("C:\\Users\\c.young\\Documents\\background_greyscale.bmp");
            BitmapData srcData = greyscale_green_background.LockBits(new Rectangle(0, 0, greyscale_green_background.Width, greyscale_green_background.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format48bppRgb);

            int stride = srcData.Stride;
            IntPtr Scan0 = srcData.Scan0;

            ulong[] totals = new ulong[] { 0, 0, 0 };
            ulong[] totals_black = new ulong[] { 0, 0, 0 }; //all black pixels are not part of the calculated average

            int width_background = greyscale_green_background.Width;
            int height_background = greyscale_green_background.Height;


            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height_background; y++)
                {
                    for (int x = 0; x < width_background; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            //the stride byte offset between start of one line (row) of pixels to the start of the next.  If the scan lines are padded, the value is a few bytes more than what's needed for the pixels in the scan line.
                            //If the bitmap is stored upside down in memory(i.e.the bottom scan line first), the stride value is negative.If you would read such a bitmap without using the stride value, you would get just garbage after the first scan line, or a memory access error.
                            int idx = (y * stride) + x * 6 + (2*color);
                            //get the pixel value
                            byte ls_byte = p[idx];
                            byte ms_byte = p[idx+1];
                            ushort t = (ushort) ( ((ms_byte & 0xFF)<<8) | (ls_byte & 0xFF));
                            ushort t_ = (ushort)(((0xFF & 0xFF) << 8) | (0xFF & 0xFF));
                            if (t == 0) totals_black[color]++;
                            else totals[color] += t;
                        }
                    }
                }
            }

            ushort avgB = (ushort) (totals[0] / ((ulong) (width_background * height_background) - totals_black[0]));
            ushort avgG = (ushort) (totals[1] / ((ulong) (width_background * height_background) - totals_black[1]));
            ushort avgR = (ushort) (totals[2] / ((ulong) (width_background * height_background) - totals_black[2]));

            Bitmap image_to_correct_BMP = new Bitmap(Image.FromFile(@"C: \Users\c.young\Documents\P1050864.JPG"));
            Bitmap image_to_correct_BMP_grayscale = MakeGrayscale3(image_to_correct_BMP);
            image_to_correct_BMP_grayscale.Save("C:\\Users\\c.young\\Documents\\image_to_correct_greyscale.bmp");
            BitmapData cor_bmp = image_to_correct_BMP_grayscale.LockBits(new Rectangle(0, 0, image_to_correct_BMP_grayscale.Width, image_to_correct_BMP_grayscale.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format48bppRgb);
            int stride__ = cor_bmp.Stride;
            IntPtr Scan0__ = cor_bmp.Scan0;

            int width_cor = image_to_correct_BMP_grayscale.Width;
            int height_cor = image_to_correct_BMP_grayscale.Height;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* q = (byte*)(void*)Scan0__;

                for (int y = 0; y < height_cor; y++)
                {
                    for (int x = 0; x < width_cor; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride__) + x * 6 + (2*color);
                            short residual = 0; 

                            byte ls_byte_p = p[idx];
                            byte ms_byte_p = p[idx + 1];
                            ushort t_p = (ushort)(((ms_byte_p & 0xFF) << 8) | (ls_byte_p & 0xFF));

                            byte ls_byte_q = q[idx];
                            byte ms_byte_q = q[idx + 1];
                            ushort t_q = (ushort)(((ms_byte_q & 0xFF) << 8) | (ls_byte_q & 0xFF));
                            ushort corrected_val = 0;
                            switch (color)
                            {
                                case 0:
                                    residual = (short) (t_p - avgB);
                                    corrected_val = (ushort) (t_q - residual);
                                    q[idx + 1] = (byte)((corrected_val >> 8) & 0x00FF);
                                    q[idx] = (byte)(corrected_val & 0x00FF);
                                    break;
                                case 1:
                                    residual = (short)(t_p - avgG);
                                    corrected_val = (ushort)(t_q - residual);
                                    q[idx + 1] = (byte)((corrected_val >> 8) & 0x00FF);
                                    q[idx] = (byte)(corrected_val & 0x00FF);
                                    break;
                                case 2:
                                    residual = (short)(t_p - avgR);
                                    corrected_val = (ushort)(t_q - residual);
                                    q[idx + 1] = (byte)((corrected_val >> 8) & 0x00FF);
                                    q[idx] = (byte)(corrected_val & 0x00FF);
                                    break;
                                case 3:
                                    //maintain the Alpha (i.e do nothing
                                    break;
                            }
                        }
                    }
                }
            }

            greyscale_green_background.UnlockBits(srcData);
            image_to_correct_BMP_grayscale.UnlockBits(cor_bmp);

            string path = "C:\\Users\\c.young\\Documents\\image_corrected.bmp";
            image_to_correct_BMP_grayscale.Save(path);



        }
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
           
            //create an empty bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height,PixelFormat.Format48bppRgb);  //to avoid loss of resolution we need to convert to 48bpprgb format.  This allows for 16bit grayscale

            //lock the original bitmap in memory
            BitmapData originalData = original.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
               ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            //lock the new bitmap in memory
            BitmapData newData = newBitmap.LockBits(new Rectangle(0, 0, original.Width, original.Height),ImageLockMode.WriteOnly, PixelFormat.Format48bppRgb);
            int stride = originalData.Stride;
            IntPtr Scan0 = originalData.Scan0;

            int stride_ = newData.Stride;
            IntPtr Scan0_ = newData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* q = (byte*)(void*)Scan0_;

                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        double sum = 0;
                        
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;
                            switch (color)
                            {
                                case 0:
                                    sum += (double)(((p[idx] * 0.114) / 255) * 65536);
                                    break;
                                case 1:
                                    sum += (double)(((p[idx] * 0.587) / 255) * 65536);
                                    break;
                                case 2:
                                    sum += (double)(((p[idx] * 0.299) / 255) * 65536);
                                    break;
                            }
                            

                            
                        }
                        //sum = sum / 3; //average value of blue green and red from the original image
                        ushort gsvalue = (ushort)(sum);

                        for (int color = 0; color < 3; color++)
                        {
                            int idx2 = (y * stride_) + x * 6 + (2*color);  //total of 6 bytes per pixel - two for each color
                            q[idx2+1] = (byte)((gsvalue >> 8) & 0x00FF);
                            q[idx2] = (byte)(gsvalue & 0x00FF);
                        }
                    }
                }

                //unlock the bitmaps
                newBitmap.UnlockBits(newData);
                original.UnlockBits(originalData);

                return newBitmap;
            }
        }
    }
}
