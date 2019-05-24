namespace Gauge_Block_File_Writer
{
    partial class FileWriterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PictureFolder = new System.Windows.Forms.Button();
            this.Imperial = new System.Windows.Forms.CheckBox();
            this.Metric = new System.Windows.Forms.CheckBox();
            this.MetricComboBox = new System.Windows.Forms.ComboBox();
            this.ImperialComboBox = new System.Windows.Forms.ComboBox();
            this.numberOfGaugesUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gaugeListRichTextBox = new System.Windows.Forms.RichTextBox();
            this.gaugeListText = new System.Windows.Forms.Label();
            this.uncroppedRichTextBox = new System.Windows.Forms.RichTextBox();
            this.uncroppedImageListLabel = new System.Windows.Forms.Label();
            this.gaugeBlockGroupBox = new System.Windows.Forms.GroupBox();
            this.SideComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Sidelabel7 = new System.Windows.Forms.Label();
            this.SiderichTextBox = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ThermExpUpDown = new System.Windows.Forms.NumericUpDown();
            this.PlatenNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ClientNamelabel = new System.Windows.Forms.Label();
            this.ClientName = new System.Windows.Forms.TextBox();
            this.srcPicBox = new System.Windows.Forms.PictureBox();
            this.BtnCrop = new System.Windows.Forms.Button();
            this.desinationPictureBox = new System.Windows.Forms.PictureBox();
            this.Save_Cropped_Image = new System.Windows.Forms.Button();
            this.GaugeFileNameLabel = new System.Windows.Forms.Label();
            this.LoadSourceImage = new System.Windows.Forms.Button();
            this.CroppedGaugeNameLabel = new System.Windows.Forms.Label();
            this.CroppedRichTextBox = new System.Windows.Forms.RichTextBox();
            this.CroppedImageListlabel = new System.Windows.Forms.Label();
            this.WriteTextFileButton = new System.Windows.Forms.Button();
            this.OutputCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfGaugesUpDown)).BeginInit();
            this.gaugeBlockGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThermExpUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlatenNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.desinationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureFolder
            // 
            this.PictureFolder.Enabled = false;
            this.PictureFolder.Location = new System.Drawing.Point(17, 841);
            this.PictureFolder.Name = "PictureFolder";
            this.PictureFolder.Size = new System.Drawing.Size(206, 34);
            this.PictureFolder.TabIndex = 0;
            this.PictureFolder.Text = "Choose Uncroped Picture Folder";
            this.PictureFolder.UseVisualStyleBackColor = true;
            this.PictureFolder.Click += new System.EventHandler(this.PictureFolder_Click);
            // 
            // Imperial
            // 
            this.Imperial.AutoSize = true;
            this.Imperial.Enabled = false;
            this.Imperial.Location = new System.Drawing.Point(17, 139);
            this.Imperial.Name = "Imperial";
            this.Imperial.Size = new System.Drawing.Size(62, 17);
            this.Imperial.TabIndex = 3;
            this.Imperial.Text = "Imperial";
            this.Imperial.UseVisualStyleBackColor = true;
            this.Imperial.CheckedChanged += new System.EventHandler(this.Imperial_CheckedChanged);
            // 
            // Metric
            // 
            this.Metric.AutoSize = true;
            this.Metric.Enabled = false;
            this.Metric.Location = new System.Drawing.Point(17, 162);
            this.Metric.Name = "Metric";
            this.Metric.Size = new System.Drawing.Size(55, 17);
            this.Metric.TabIndex = 2;
            this.Metric.Text = "Metric";
            this.Metric.UseVisualStyleBackColor = true;
            this.Metric.CheckedChanged += new System.EventHandler(this.Metric_CheckedChanged);
            // 
            // MetricComboBox
            // 
            this.MetricComboBox.Enabled = false;
            this.MetricComboBox.FormattingEnabled = true;
            this.MetricComboBox.Items.AddRange(new object[] {
            "0.5",
            "1",
            "1.0005",
            "1.001",
            "1.002",
            "1.003",
            "1.004",
            "1.005",
            "1.006",
            "1.007",
            "1.008",
            "1.009",
            "1.01",
            "1.02",
            "1.03",
            "1.04",
            "1.05",
            "1.06",
            "1.07",
            "1.08",
            "1.09",
            "1.1",
            "1.11",
            "1.12",
            "1.13",
            "1.14",
            "1.15",
            "1.16",
            "1.17",
            "1.18",
            "1.19",
            "1.2",
            "1.21",
            "1.22",
            "1.23",
            "1.24",
            "1.25",
            "1.26",
            "1.27",
            "1.28",
            "1.29",
            "1.3",
            "1.31",
            "1.32",
            "1.33",
            "1.34",
            "1.35",
            "1.36",
            "1.37",
            "1.38",
            "1.39",
            "1.4",
            "1.41",
            "1.42",
            "1.43",
            "1.44",
            "1.45",
            "1.46",
            "1.47",
            "1.48",
            "1.49",
            "1.5",
            "2",
            "2.5",
            "3",
            "3.5",
            "4",
            "4.5",
            "5",
            "5.5",
            "6",
            "6.5",
            "7",
            "7.5",
            "8",
            "8.5",
            "9",
            "9.5",
            "10",
            "10.5",
            "11",
            "11.5",
            "12",
            "12.5",
            "13",
            "13.5",
            "14",
            "14.5",
            "15",
            "15.5",
            "16",
            "16.5",
            "17",
            "17.5",
            "18",
            "18.5",
            "19",
            "19.5",
            "20",
            "20.5",
            "21",
            "21.5",
            "22",
            "22.5",
            "23",
            "23.5",
            "24",
            "24.5",
            "25",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.MetricComboBox.Location = new System.Drawing.Point(14, 185);
            this.MetricComboBox.Name = "MetricComboBox";
            this.MetricComboBox.Size = new System.Drawing.Size(96, 21);
            this.MetricComboBox.TabIndex = 1;
            this.MetricComboBox.Text = "Choose Gauge";
            this.MetricComboBox.SelectedIndexChanged += new System.EventHandler(this.MetricComboBox_SelectedIndexChanged);
            // 
            // ImperialComboBox
            // 
            this.ImperialComboBox.Enabled = false;
            this.ImperialComboBox.FormattingEnabled = true;
            this.ImperialComboBox.Items.AddRange(new object[] {
            "0.05",
            "0.1",
            "0.1001",
            "0.1002",
            "0.1003",
            "0.1004",
            "0.1005",
            "0.1006",
            "0.1007",
            "0.1008",
            "0.1009",
            "0.101",
            "0.102",
            "0.103",
            "0.104",
            "0.105",
            "0.106",
            "0.107",
            "0.108",
            "0.109",
            "0.11",
            "0.111",
            "0.112",
            "0.113",
            "0.114",
            "0.115",
            "0.116",
            "0.117",
            "0.118",
            "0.119",
            "0.12",
            "0.121",
            "0.122",
            "0.123",
            "0.124",
            "0.125",
            "0.126",
            "0.127",
            "0.128",
            "0.129",
            "0.13",
            "0.131",
            "0.132",
            "0.133",
            "0.134",
            "0.135",
            "0.136",
            "0.137",
            "0.138",
            "0.139",
            "0.14",
            "0.141",
            "0.142",
            "0.143",
            "0.144",
            "0.145",
            "0.146",
            "0.147",
            "0.148",
            "0.149",
            "0.15",
            "0.16",
            "0.17",
            "0.18",
            "0.19",
            "0.2",
            "0.21",
            "0.25",
            "0.3",
            "0.315",
            "0.35",
            "0.4",
            "0.42",
            "0.45",
            "0.5",
            "0.55",
            "0.6",
            "0.605",
            "0.65",
            "0.7",
            "0.71",
            "0.75",
            "0.8",
            "0.815",
            "0.85",
            "0.9",
            "0.92",
            "0.95",
            "1",
            "2",
            "3",
            "4"});
            this.ImperialComboBox.Location = new System.Drawing.Point(14, 185);
            this.ImperialComboBox.Name = "ImperialComboBox";
            this.ImperialComboBox.Size = new System.Drawing.Size(96, 21);
            this.ImperialComboBox.TabIndex = 4;
            this.ImperialComboBox.Text = "Choose Gauge";
            this.ImperialComboBox.SelectedIndexChanged += new System.EventHandler(this.ImperialComboBox_SelectedIndexChanged);
            // 
            // numberOfGaugesUpDown
            // 
            this.numberOfGaugesUpDown.Location = new System.Drawing.Point(14, 44);
            this.numberOfGaugesUpDown.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numberOfGaugesUpDown.Name = "numberOfGaugesUpDown";
            this.numberOfGaugesUpDown.Size = new System.Drawing.Size(93, 20);
            this.numberOfGaugesUpDown.TabIndex = 5;
            this.numberOfGaugesUpDown.ValueChanged += new System.EventHandler(this.NumberOfGaugesUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Number of Gauges";
            // 
            // gaugeListRichTextBox
            // 
            this.gaugeListRichTextBox.Location = new System.Drawing.Point(15, 292);
            this.gaugeListRichTextBox.Name = "gaugeListRichTextBox";
            this.gaugeListRichTextBox.Size = new System.Drawing.Size(102, 504);
            this.gaugeListRichTextBox.TabIndex = 7;
            this.gaugeListRichTextBox.Text = "";
            // 
            // gaugeListText
            // 
            this.gaugeListText.AutoSize = true;
            this.gaugeListText.Location = new System.Drawing.Point(19, 252);
            this.gaugeListText.Name = "gaugeListText";
            this.gaugeListText.Size = new System.Drawing.Size(88, 13);
            this.gaugeListText.TabIndex = 8;
            this.gaugeListText.Text = "Gauge Block List";
            // 
            // uncroppedRichTextBox
            // 
            this.uncroppedRichTextBox.Location = new System.Drawing.Point(396, 700);
            this.uncroppedRichTextBox.Name = "uncroppedRichTextBox";
            this.uncroppedRichTextBox.Size = new System.Drawing.Size(820, 245);
            this.uncroppedRichTextBox.TabIndex = 9;
            this.uncroppedRichTextBox.Text = "";
            // 
            // uncroppedImageListLabel
            // 
            this.uncroppedImageListLabel.AutoSize = true;
            this.uncroppedImageListLabel.Location = new System.Drawing.Point(403, 684);
            this.uncroppedImageListLabel.Name = "uncroppedImageListLabel";
            this.uncroppedImageListLabel.Size = new System.Drawing.Size(111, 13);
            this.uncroppedImageListLabel.TabIndex = 10;
            this.uncroppedImageListLabel.Text = "Uncropped Image List";
            // 
            // gaugeBlockGroupBox
            // 
            this.gaugeBlockGroupBox.Controls.Add(this.SideComboBox);
            this.gaugeBlockGroupBox.Controls.Add(this.label7);
            this.gaugeBlockGroupBox.Controls.Add(this.Sidelabel7);
            this.gaugeBlockGroupBox.Controls.Add(this.SiderichTextBox);
            this.gaugeBlockGroupBox.Controls.Add(this.label6);
            this.gaugeBlockGroupBox.Controls.Add(this.label5);
            this.gaugeBlockGroupBox.Controls.Add(this.label4);
            this.gaugeBlockGroupBox.Controls.Add(this.label3);
            this.gaugeBlockGroupBox.Controls.Add(this.ThermExpUpDown);
            this.gaugeBlockGroupBox.Controls.Add(this.PlatenNumericUpDown);
            this.gaugeBlockGroupBox.Controls.Add(this.label2);
            this.gaugeBlockGroupBox.Controls.Add(this.ClientNamelabel);
            this.gaugeBlockGroupBox.Controls.Add(this.ClientName);
            this.gaugeBlockGroupBox.Controls.Add(this.numberOfGaugesUpDown);
            this.gaugeBlockGroupBox.Controls.Add(this.label1);
            this.gaugeBlockGroupBox.Controls.Add(this.ImperialComboBox);
            this.gaugeBlockGroupBox.Controls.Add(this.MetricComboBox);
            this.gaugeBlockGroupBox.Controls.Add(this.Imperial);
            this.gaugeBlockGroupBox.Controls.Add(this.Metric);
            this.gaugeBlockGroupBox.Controls.Add(this.gaugeListText);
            this.gaugeBlockGroupBox.Controls.Add(this.gaugeListRichTextBox);
            this.gaugeBlockGroupBox.Controls.Add(this.PictureFolder);
            this.gaugeBlockGroupBox.Location = new System.Drawing.Point(12, 6);
            this.gaugeBlockGroupBox.Name = "gaugeBlockGroupBox";
            this.gaugeBlockGroupBox.Size = new System.Drawing.Size(238, 904);
            this.gaugeBlockGroupBox.TabIndex = 11;
            this.gaugeBlockGroupBox.TabStop = false;
            this.gaugeBlockGroupBox.Text = "Gauge Block Information";
            // 
            // SideComboBox
            // 
            this.SideComboBox.FormattingEnabled = true;
            this.SideComboBox.Items.AddRange(new object[] {
            "1",
            "2"});
            this.SideComboBox.Location = new System.Drawing.Point(121, 186);
            this.SideComboBox.Name = "SideComboBox";
            this.SideComboBox.Size = new System.Drawing.Size(96, 21);
            this.SideComboBox.TabIndex = 26;
            this.SideComboBox.Text = "1";
            this.SideComboBox.SelectedIndexChanged += new System.EventHandler(this.SideComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(122, 265);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "(in order measured)";
            // 
            // Sidelabel7
            // 
            this.Sidelabel7.AutoSize = true;
            this.Sidelabel7.Location = new System.Drawing.Point(122, 252);
            this.Sidelabel7.Name = "Sidelabel7";
            this.Sidelabel7.Size = new System.Drawing.Size(28, 13);
            this.Sidelabel7.TabIndex = 24;
            this.Sidelabel7.Text = "Side";
            // 
            // SiderichTextBox
            // 
            this.SiderichTextBox.Location = new System.Drawing.Point(123, 292);
            this.SiderichTextBox.Name = "SiderichTextBox";
            this.SiderichTextBox.Size = new System.Drawing.Size(102, 504);
            this.SiderichTextBox.TabIndex = 25;
            this.SiderichTextBox.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(120, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Side";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 265);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "(in order measured)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "( x 10-6 m/m/K )";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Thermal Expansion";
            // 
            // ThermExpUpDown
            // 
            this.ThermExpUpDown.DecimalPlaces = 1;
            this.ThermExpUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ThermExpUpDown.Location = new System.Drawing.Point(123, 106);
            this.ThermExpUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ThermExpUpDown.Name = "ThermExpUpDown";
            this.ThermExpUpDown.Size = new System.Drawing.Size(100, 20);
            this.ThermExpUpDown.TabIndex = 22;
            this.ThermExpUpDown.Value = new decimal(new int[] {
            95,
            0,
            0,
            65536});
            this.ThermExpUpDown.ValueChanged += new System.EventHandler(this.ThermExpUpDown_ValueChanged);
            // 
            // PlatenNumericUpDown
            // 
            this.PlatenNumericUpDown.Location = new System.Drawing.Point(15, 106);
            this.PlatenNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PlatenNumericUpDown.Name = "PlatenNumericUpDown";
            this.PlatenNumericUpDown.Size = new System.Drawing.Size(92, 20);
            this.PlatenNumericUpDown.TabIndex = 19;
            this.PlatenNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Platen Number";
            // 
            // ClientNamelabel
            // 
            this.ClientNamelabel.AutoSize = true;
            this.ClientNamelabel.Location = new System.Drawing.Point(120, 28);
            this.ClientNamelabel.Name = "ClientNamelabel";
            this.ClientNamelabel.Size = new System.Drawing.Size(67, 13);
            this.ClientNamelabel.TabIndex = 19;
            this.ClientNamelabel.Text = "Client Name ";
            // 
            // ClientName
            // 
            this.ClientName.Location = new System.Drawing.Point(123, 44);
            this.ClientName.Name = "ClientName";
            this.ClientName.Size = new System.Drawing.Size(100, 20);
            this.ClientName.TabIndex = 19;
            this.ClientName.TextChanged += new System.EventHandler(this.ClientName_TextChanged);
            // 
            // srcPicBox
            // 
            this.srcPicBox.Location = new System.Drawing.Point(396, 50);
            this.srcPicBox.Name = "srcPicBox";
            this.srcPicBox.Size = new System.Drawing.Size(820, 584);
            this.srcPicBox.TabIndex = 14;
            this.srcPicBox.TabStop = false;
            this.srcPicBox.Paint += new System.Windows.Forms.PaintEventHandler(this.SrcPicBox_Paint);
            this.srcPicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseClick);
            this.srcPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseDown);
            this.srcPicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseMove);
            this.srcPicBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseUp);
            // 
            // BtnCrop
            // 
            this.BtnCrop.Location = new System.Drawing.Point(810, 649);
            this.BtnCrop.Name = "BtnCrop";
            this.BtnCrop.Size = new System.Drawing.Size(106, 23);
            this.BtnCrop.TabIndex = 16;
            this.BtnCrop.Text = "Crop";
            this.BtnCrop.UseVisualStyleBackColor = true;
            this.BtnCrop.Click += new System.EventHandler(this.BtnCrop_Click);
            // 
            // desinationPictureBox
            // 
            this.desinationPictureBox.Location = new System.Drawing.Point(1351, 50);
            this.desinationPictureBox.Name = "desinationPictureBox";
            this.desinationPictureBox.Size = new System.Drawing.Size(452, 450);
            this.desinationPictureBox.TabIndex = 17;
            this.desinationPictureBox.TabStop = false;
            // 
            // Save_Cropped_Image
            // 
            this.Save_Cropped_Image.Location = new System.Drawing.Point(1510, 513);
            this.Save_Cropped_Image.Name = "Save_Cropped_Image";
            this.Save_Cropped_Image.Size = new System.Drawing.Size(174, 23);
            this.Save_Cropped_Image.TabIndex = 18;
            this.Save_Cropped_Image.Text = "Save Cropped Image";
            this.Save_Cropped_Image.UseVisualStyleBackColor = true;
            this.Save_Cropped_Image.Click += new System.EventHandler(this.Save_Cropped_Image_Click);
            // 
            // GaugeFileNameLabel
            // 
            this.GaugeFileNameLabel.AutoSize = true;
            this.GaugeFileNameLabel.Location = new System.Drawing.Point(403, 34);
            this.GaugeFileNameLabel.Name = "GaugeFileNameLabel";
            this.GaugeFileNameLabel.Size = new System.Drawing.Size(89, 13);
            this.GaugeFileNameLabel.TabIndex = 19;
            this.GaugeFileNameLabel.Text = "Gauge File Name";
            // 
            // LoadSourceImage
            // 
            this.LoadSourceImage.Location = new System.Drawing.Point(654, 649);
            this.LoadSourceImage.Name = "LoadSourceImage";
            this.LoadSourceImage.Size = new System.Drawing.Size(112, 23);
            this.LoadSourceImage.TabIndex = 20;
            this.LoadSourceImage.Text = "Load Next Image";
            this.LoadSourceImage.UseVisualStyleBackColor = true;
            this.LoadSourceImage.Click += new System.EventHandler(this.LoadSourceImage_Click);
            // 
            // CroppedGaugeNameLabel
            // 
            this.CroppedGaugeNameLabel.AutoSize = true;
            this.CroppedGaugeNameLabel.Location = new System.Drawing.Point(1359, 34);
            this.CroppedGaugeNameLabel.Name = "CroppedGaugeNameLabel";
            this.CroppedGaugeNameLabel.Size = new System.Drawing.Size(240, 13);
            this.CroppedGaugeNameLabel.TabIndex = 21;
            this.CroppedGaugeNameLabel.Text = "Cropped Gauge File Name (Displays Post Saving)";
            // 
            // CroppedRichTextBox
            // 
            this.CroppedRichTextBox.Location = new System.Drawing.Point(1351, 571);
            this.CroppedRichTextBox.Name = "CroppedRichTextBox";
            this.CroppedRichTextBox.Size = new System.Drawing.Size(454, 310);
            this.CroppedRichTextBox.TabIndex = 22;
            this.CroppedRichTextBox.Text = "";
            // 
            // CroppedImageListlabel
            // 
            this.CroppedImageListlabel.AutoSize = true;
            this.CroppedImageListlabel.Location = new System.Drawing.Point(1359, 555);
            this.CroppedImageListlabel.Name = "CroppedImageListlabel";
            this.CroppedImageListlabel.Size = new System.Drawing.Size(98, 13);
            this.CroppedImageListlabel.TabIndex = 23;
            this.CroppedImageListlabel.Text = "Cropped Image List";
            // 
            // WriteTextFileButton
            // 
            this.WriteTextFileButton.Enabled = false;
            this.WriteTextFileButton.Location = new System.Drawing.Point(1415, 902);
            this.WriteTextFileButton.Name = "WriteTextFileButton";
            this.WriteTextFileButton.Size = new System.Drawing.Size(106, 23);
            this.WriteTextFileButton.TabIndex = 24;
            this.WriteTextFileButton.Text = "Write Text File";
            this.WriteTextFileButton.UseVisualStyleBackColor = true;
            this.WriteTextFileButton.Click += new System.EventHandler(this.WriteTextFileButton_Click);
            // 
            // OutputCheckBox
            // 
            this.OutputCheckBox.AutoSize = true;
            this.OutputCheckBox.Location = new System.Drawing.Point(1602, 906);
            this.OutputCheckBox.Name = "OutputCheckBox";
            this.OutputCheckBox.Size = new System.Drawing.Size(201, 17);
            this.OutputCheckBox.TabIndex = 26;
            this.OutputCheckBox.Text = "Output both platen temp and air temp";
            this.OutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // FileWriterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1850, 1008);
            this.Controls.Add(this.OutputCheckBox);
            this.Controls.Add(this.WriteTextFileButton);
            this.Controls.Add(this.CroppedImageListlabel);
            this.Controls.Add(this.CroppedRichTextBox);
            this.Controls.Add(this.CroppedGaugeNameLabel);
            this.Controls.Add(this.LoadSourceImage);
            this.Controls.Add(this.GaugeFileNameLabel);
            this.Controls.Add(this.Save_Cropped_Image);
            this.Controls.Add(this.desinationPictureBox);
            this.Controls.Add(this.BtnCrop);
            this.Controls.Add(this.srcPicBox);
            this.Controls.Add(this.uncroppedImageListLabel);
            this.Controls.Add(this.uncroppedRichTextBox);
            this.Controls.Add(this.gaugeBlockGroupBox);
            this.Name = "FileWriterForm";
            this.Text = "Gauge Block File Writer";
            ((System.ComponentModel.ISupportInitialize)(this.numberOfGaugesUpDown)).EndInit();
            this.gaugeBlockGroupBox.ResumeLayout(false);
            this.gaugeBlockGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThermExpUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlatenNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.desinationPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PictureFolder;
        private System.Windows.Forms.CheckBox Imperial;
        private System.Windows.Forms.CheckBox Metric;
        private System.Windows.Forms.ComboBox MetricComboBox;
        private System.Windows.Forms.ComboBox ImperialComboBox;
        private System.Windows.Forms.NumericUpDown numberOfGaugesUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox gaugeListRichTextBox;
        private System.Windows.Forms.Label gaugeListText;
        private System.Windows.Forms.RichTextBox uncroppedRichTextBox;
        private System.Windows.Forms.Label uncroppedImageListLabel;
        private System.Windows.Forms.GroupBox gaugeBlockGroupBox;
        private System.Windows.Forms.PictureBox srcPicBox;
        private System.Windows.Forms.Button BtnCrop;
        private System.Windows.Forms.PictureBox desinationPictureBox;
        private System.Windows.Forms.Button Save_Cropped_Image;
        private System.Windows.Forms.Label ClientNamelabel;
        private System.Windows.Forms.TextBox ClientName;
        private System.Windows.Forms.NumericUpDown PlatenNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label GaugeFileNameLabel;
        private System.Windows.Forms.Button LoadSourceImage;
        private System.Windows.Forms.Label CroppedGaugeNameLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ThermExpUpDown;
        private System.Windows.Forms.RichTextBox CroppedRichTextBox;
        private System.Windows.Forms.Label CroppedImageListlabel;
        private System.Windows.Forms.RichTextBox SiderichTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Sidelabel7;
        private System.Windows.Forms.ComboBox SideComboBox;
        private System.Windows.Forms.Button WriteTextFileButton;
        private System.Windows.Forms.CheckBox OutputCheckBox;
    }
}

