using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Gauge_Block_File_Writer
{

    enum EnviroParam { Temperature, Pressure, Humidity };
    class GaugeBlock
    {
        private static GaugeBlock[] gauges = new GaugeBlock[1];
        private static int gauge_index = 0;
        private double size = 0;
        private static int expected_num_gauges = 0;
        private GaugeImage red_img;
        private GaugeImage green_img;
        private GaugeImage red_cropped;
        private GaugeImage green_cropped;
        private double temperature_red;
        private double temperature_red_p;
        private double temperature_green;
        private double temperature_green_p;
        private double pressure_red;
        private double pressure_green;
        private double humidity_red;
        private double humidity_green;
        private short side;
        private static double thermal_expansion;
        private DateTime red_date;
        private DateTime green_date;

        //when we add a new gauge block we need to add it to the gauge block array
        public GaugeBlock(double size_)//,ref int line_remove_index)
        {
            

            bool is_in_list = false;
            int index = 0;

            for(int i = 0; i < gauges.Length; i++)
            {
                if (gauges[i] != null)
                {
                    if (gauges[i].size == size_)
                    {
                        is_in_list = true;
                        index = i;
                        break;
                    }
                }
            }
            //this is the same gauge selected a second time so treat this as a signal to delete the entry
            if (is_in_list)
            {
                RemoveAt(index);
                //line_remove_index = index;

            }

            //this is a new gauge to add
            else
            {
                gauges[gauge_index] = this;
                gauge_index++;
                Array.Resize(ref gauges, gauge_index + 1);
                Size = size_;
            }
        }

        public double Size
        {
            set { size = value; }
            get { return size; }
        
        }
        public short Side
        {
            set { side = value; }
            get { return side; }
        }
        public static double ThermalExp
        {
            set { thermal_expansion = value; }
            get { return thermal_expansion; }
        }
        private void RemoveAt(int index)
        {
            //move all elements above the index back one position
            for (int i = index+1; i < gauges.Length; i++) gauges[i - 1] = gauges[i];
            Array.Resize(ref gauges, gauges.Length-1);
            gauge_index--;
        }
        public static int NumGauges
        {
            set { expected_num_gauges = value; }
            get { return expected_num_gauges; }
        }

        public static GaugeBlock[] Gauges
        {
            get { return gauges; }
        }

        public GaugeImage RedImage
        {
            set { red_img = value; }
            get { return red_img; }
        }
        public GaugeImage GreenImage
        {
            set { green_img = value; }
            get { return green_img; }
        }
        public GaugeImage RedCropped
        {
            set { red_cropped = value; }
            get { return red_cropped; }
        }
        public GaugeImage GreenCropped
        {
            set { green_cropped = value; }
            get { return green_cropped; }
        }
        public DateTime RedDate
        {
            set { red_date = value; }
            get { return red_date; }
        }
        public DateTime GreenDate
        {
            set { green_date = value; }
            get { return green_date; }
        }
        public double TRed
        {
            get { return temperature_red; }
            set { temperature_red = value; }
        }
        public double TRedPlaten
        {
            get { return temperature_red_p; }
            set { temperature_red_p = value; }
        }
        public double TGreenPlaten
        {
            get { return temperature_green_p; }
            set { temperature_green_p = value; }
        }
        public double TGreen
        {
            get { return temperature_green; }
            set { temperature_green = value; }
        }
        public double PRed
        {
            get { return pressure_red; }
            set { pressure_red = value; }
        }
        public double PGreen
        {
            get { return pressure_green; }
            set { pressure_green = value; }
        }
        public double HRed
        {
            get { return humidity_red; }
            set { humidity_red = value; }
        }
        public double HGreen
        {
            get { return humidity_green; }
            set { humidity_green = value; }
        }

        public bool CalculateTPH()
        {
            if (ReadFile(EnviroParam.Temperature) && ReadFile(EnviroParam.Pressure) && ReadFile(EnviroParam.Humidity)) return true;
            else return false;
        }

        public bool ReadFile(EnviroParam p)
        {

            int month_g = green_date.Month;
            int year_g = green_date.Year;
            int month_r = red_date.Month;
            int year_r = red_date.Year;
            DateTime previous_date = DateTime.Now;

            bool first_reading = true;
            bool value_set = false;

            double previous_reading = 0;


            string path_r = "";
            string path_g = "";
            string path_r_platen = "";
            string path_g_platen = "";
            string[] paths;

            switch (p)
            {

                case EnviroParam.Temperature:
                    path_r = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Temperature Monitoring Data\\Hilger Lab\\" + year_r.ToString() + "\\" + year_r.ToString() + "-" + month_r.ToString() + "\\" + "Air in Beam Path .txt";
                    path_g = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Temperature Monitoring Data\\Hilger Lab\\" + year_g.ToString() + "\\" + year_g.ToString() + "-" + month_g.ToString() + "\\" + "Air in Beam Path .txt";
                    path_r_platen = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Temperature Monitoring Data\\Hilger Lab\\" + year_r.ToString() + "\\" + year_r.ToString() + "-" + month_r.ToString() + "\\" + "Hilger platen.txt";
                    path_g_platen = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Temperature Monitoring Data\\Hilger Lab\\" + year_g.ToString() + "\\" + year_g.ToString() + "-" + month_g.ToString() + "\\" + "Hilger platen.txt";
                    paths = new string[4] { path_r, path_g, path_r_platen, path_g_platen };
                    break;
                case EnviroParam.Pressure:
                    path_r = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Pressure Monitoring Data\\Ground_floor_at_1m_height\\" + year_r.ToString() + "\\" + year_r.ToString() + "-" + month_r.ToString() + "\\" + "MSLE.L.105.txt";
                    path_g = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Pressure Monitoring Data\\Ground_floor_at_1m_height\\" + year_g.ToString() + "\\" + year_g.ToString() + "-" + month_g.ToString() + "\\" + "MSLE.L.105.txt";
                    paths = new string[2] { path_r, path_g };
                    break;
                case EnviroParam.Humidity:
                    path_r = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Humidity Monitoring Data\\HILGERLAB\\" + year_r.ToString() + "\\" + year_r.ToString() + "-" + month_r.ToString() + "\\" + "MSLE.L.105.txt";
                    path_g = "C:\\Users\\c.young\\OneDrive - Callaghan Innovation\\Humidity Monitoring Data\\HILGERLAB\\" + year_g.ToString() + "\\" + year_g.ToString() + "-" + month_g.ToString() + "\\" + "MSLE.L.105.txt";
                    paths = new string[2] { path_r, path_g };
                    break;
                default:
                    paths = null;
                    break;
            }
            StreamReader reader;
            FileStream fs;
            if (paths == null) return false;
            int w = 0;
            foreach (string path in paths)
            {
                w++; //for w == 1 or 3 the image is red, for w == 2 or 4 the image is green.
                try
                {
                    //check if the file path exists.
                    if (System.IO.File.Exists(path))
                    {
                        fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        using (reader = new StreamReader(fs))
                        {
                            string line;
                            string line1;
                            double reading;
                            DateTime date_read;
                            int comma_index;
                            int comma_index2;

                            first_reading = true;
                            value_set = false;
                            while (reader.Peek() >= 0 && value_set == false)
                            {
                                //read the line to a string

                                line = reader.ReadLine();
                                if (line.Equals("Automatically Generated File!"))
                                {
                                    while (reader.Peek() >= 0)
                                    {
                                        line = reader.ReadLine();
                                        if (line != "") break;
                                    }
                                }

                                comma_index = line.IndexOf(","); //the first comma in the string is where the reading ends
                                line1 = line.Substring(comma_index + 1);
                                comma_index2 = line1.IndexOf(","); //the second comma is the string where the channel reading ends

                                //for temperature files the third comma in the string is where the date reading ends.
                                if (p == EnviroParam.Temperature)
                                {
                                    line1 = line1.Substring(comma_index2 + 1);
                                    comma_index2 = line1.IndexOf(",");
                                }
                                try
                                {
                                    reading = Convert.ToDouble(line.Remove(comma_index));
                                    date_read = Convert.ToDateTime(line1.Remove(comma_index2));

                                }
                                catch (FormatException)
                                {
                                    return false;
                                }

                                if (!first_reading)
                                {
                                    int first_comp;
                                    int second_comp;
                                    if ((w == 1) || (w == 3))
                                    {
                                        first_comp = DateTime.Compare(red_date, date_read);
                                        second_comp = DateTime.Compare(red_date, previous_date);
                                    }
                                    else
                                    {
                                        first_comp = DateTime.Compare(green_date, date_read);
                                        second_comp = DateTime.Compare(green_date, previous_date);
                                    }
                                    if (first_comp <= 0 && second_comp >= 0)
                                    {
                                        //the value we want is in between these two date values.
                                        TimeSpan span1;
                                        TimeSpan span2;
                                        if ((w == 1) || (w == 3))
                                        {
                                            span1 = date_read.Subtract(red_date);
                                            span2 = red_date.Subtract(previous_date);
                                        }
                                        else
                                        {
                                            span1 = date_read.Subtract(green_date);
                                            span2 = green_date.Subtract(previous_date);
                                        }
                                        if (span1.Ticks > span2.Ticks)
                                        {
                                            switch (p)
                                            {
                                                case EnviroParam.Temperature:
                                                    if (path.Contains("platen") && w == 3)
                                                    {
                                                        TRedPlaten = previous_reading;
                                                        value_set = true;
                                                    }
                                                    else if (path.Contains("platen") && w == 4)
                                                    {
                                                        TGreenPlaten = previous_reading;
                                                        value_set = true;
                                                    }
                                                    else if (w == 1)
                                                    {
                                                        TRed = previous_reading;
                                                        value_set = true;
                                                    }
                                                    else if (w == 2)
                                                    {
                                                        TGreen = previous_reading;
                                                        value_set = true;
                                                    }
                                                    else return false;
                                                    break;
                                                case EnviroParam.Pressure:
                                                    if (w == 1)
                                                    {
                                                        PRed = previous_reading;
                                                        value_set = true;
                                                    }
                                                    else
                                                    {
                                                        PGreen = previous_reading;
                                                        value_set = true;
                                                    }
                                                    break;
                                                case EnviroParam.Humidity:
                                                    if (w == 1)
                                                    {
                                                        HRed = previous_reading;
                                                        value_set = true;
                                                    }
                                                    else
                                                    {
                                                        HGreen = previous_reading;
                                                        value_set = true;
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch (p)
                                            {
                                                case EnviroParam.Temperature:
                                                    if (path.Contains("platen") && w == 3)
                                                    {
                                                        TRedPlaten = reading;
                                                        value_set = true;

                                                    }
                                                    else if (path.Contains("platen") && w == 4)
                                                    {
                                                        TGreenPlaten = reading;
                                                        value_set = true;

                                                    }
                                                    else if (w == 1)
                                                    {
                                                        TRed = reading;
                                                        value_set = true;
                                                    }
                                                    else if (w == 2)
                                                    {
                                                        TGreen = reading;
                                                        value_set = true;
                                                    }
                                                    else return false;
                                                    break;
                                                case EnviroParam.Pressure:
                                                    if (w == 1)
                                                    {
                                                        PRed = reading;
                                                        value_set = true;
                                                    }
                                                    else
                                                    {
                                                        PGreen = reading;
                                                        value_set = true;
                                                    }
                                                    break;
                                                case EnviroParam.Humidity:
                                                    if (w == 1)
                                                    {
                                                        HRed = reading;
                                                        value_set = true;
                                                    }
                                                    else
                                                    {
                                                        HGreen = reading;
                                                        value_set = true;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                                first_reading = false;
                                previous_date = date_read;
                                previous_reading = reading;
                            }
                        }
                    }
                    else return false;

                }
                catch (System.IO.IOException e)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
