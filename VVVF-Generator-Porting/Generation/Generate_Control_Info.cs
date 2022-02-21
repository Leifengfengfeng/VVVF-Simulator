using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.Generation.Generate_Common;
using System.Drawing.Drawing2D;
using Point = System.Drawing.Point;
using System.Windows.Forms;
using Size = System.Drawing.Size;
using System.Collections.Generic;
using VVVF_Generator_Porting.Yaml_VVVF_Sound;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_Control_Info
    {

        private static String[] get_Pulse_Name(Pulse_Mode mode)
        {
            //Not in sync
            if (mode == Pulse_Mode.Async || mode == Pulse_Mode.Async_THI)
            {
                string[] names = new string[3];
                int count = 0;

                names[count] = String.Format("Async - " + Video_Generate_Values.carrier_freq_data.base_freq.ToString("F2")).PadLeft(6);
                count++;

                if (Video_Generate_Values.carrier_freq_data.range != 0)
                {
                    names[count] = String.Format("Random ± " + Video_Generate_Values.carrier_freq_data.range.ToString("F2")).PadLeft(6);
                    count++;
                }

                if (Video_Generate_Values.dipolar != -1)
                {
                    names[count] = String.Format("Dipolar : " + Video_Generate_Values.dipolar.ToString("F0")).PadLeft(6);
                    count++;
                }
                return names;

            }

            //Abs
            if (mode == Pulse_Mode.P_Wide_3)
                return new string[] { "Wide 3 Pulse" };

            if (mode.ToString().StartsWith("CHM"))
            {
                String mode_name = mode.ToString();
                bool contain_wide = mode_name.Contains("Wide");
                mode_name = mode_name.Replace("_Wide", "");

                String[] mode_name_type = mode_name.Split("_");

                String final_mode_name = ((contain_wide) ? "Wide " : "") + mode_name_type[1] + " Pulse";

                return new string[] { final_mode_name, "Current Harmonic Minimum" };
            }
            if (mode.ToString().StartsWith("SHE"))
            {
                String mode_name = mode.ToString();
                bool contain_wide = mode_name.Contains("Wide");
                mode_name = mode_name.Replace("_Wide", "");

                String[] mode_name_type = mode_name.Split("_");

                String final_mode_name = (contain_wide) ? "Wide " : "" + mode_name_type[1] + " Pulse";

                return new string[] { final_mode_name, "Selective Harmonic Elimination" };
            }
            else
            {
                String[] mode_name_type = mode.ToString().Split("_");
                String mode_name = "";
                if (mode_name_type[0] == "SP") mode_name = "Shifted ";

                mode_name += mode_name_type[1] + " Pulse";

                if (Video_Generate_Values.dipolar == -1) return new string[] { mode_name };
                else return new string[] { mode_name, "Dipolar : " + Video_Generate_Values.dipolar.ToString("F1") };
            }
        }

        private static void generate_opening(int image_width, int image_height, VideoWriter vr)
        {
            //opening
            for (int i = 0; i < 128; i++)
            {
                Bitmap image = new(image_width, image_height);
                Graphics g = Graphics.FromImage(image);

                LinearGradientBrush gb = new LinearGradientBrush(new System.Drawing.Point(0, 0), new System.Drawing.Point(image_width, image_height), Color.FromArgb(0xFF, 0xFF, 0xFF), Color.FromArgb(0xFD, 0xE0, 0xE0));
                g.FillRectangle(gb, 0, 0, image_width, image_height);

                FontFamily simulator_title = new FontFamily("Fugaz One");
                Font simulator_title_fnt = new Font(
                    simulator_title,
                    40,
                    FontStyle.Bold,
                    GraphicsUnit.Pixel);
                Font simulator_title_fnt_sub = new Font(
                    simulator_title,
                    20,
                    FontStyle.Bold,
                    GraphicsUnit.Pixel);

                FontFamily title_fontFamily = new FontFamily("Fugaz One");
                Font title_fnt = new Font(
                    title_fontFamily,
                    40,
                    FontStyle.Regular,
                    GraphicsUnit.Pixel);

                Brush title_brush = Brushes.Black;

                g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 0, (int)(image_width * ((i > 30) ? 1 : i / 30.0)), 68 - 0);
                g.DrawString("Pulse Mode", title_fnt, title_brush, (int)((i < 40) ? -1000 : (double)((i > 80) ? 17 : 17 * (i - 40) / 40.0)), 8);
                g.FillRectangle(Brushes.Blue, 0, 68, (int)(image_width * ((i > 30) ? 1 : i / 30.0)), 8);

                g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 226, (int)(image_width * ((i > 30) ? 1 : i / 30.0)), 291 - 226);
                g.DrawString("Sine Freq[Hz]", title_fnt, title_brush, (int)((i < 40 + 10) ? -1000 : (double)((i > 80 + 10) ? 17 : 17 * (i - (40 + 10)) / 40.0)), 231);
                g.FillRectangle(Brushes.Blue, 0, 291, (int)(image_width * ((i > 30) ? 1 : i / 30.0)), 8);

                g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 447, (int)(image_width * (double)(((i > 30) ? 1 : i / 30.0))), 513 - 447);
                g.DrawString("Sine Amplitude[%]", title_fnt, title_brush, (int)((i < 40 + 20) ? -1000 : (i > 80 + 20) ? 17 : 17 * (i - (40 + 20)) / 40.0), 452);
                g.FillRectangle(Brushes.Blue, 0, 513, (int)(image_width * ((i > 30) ? 1 : i / 30.0)), 8);

                g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), 0, 669, (int)(image_width * (double)(((i > 30) ? 1 : i / 30.0))), 735 - 669);
                g.DrawString("Freerun", title_fnt, title_brush, (int)((i < 40 + 30) ? -1000 : (i > 80 + 30) ? 17 : 17 * (i - (40 + 30)) / 40.0), 674);
                g.FillRectangle(Brushes.LightGray, 0, 735, (int)(image_width * ((i > 30) ? 1 : i / 30.0)), 8);

                g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), 0, 847, (int)(image_width * (double)(((i > 30) ? 1 : i / 30.0))), 913 - 847);
                g.DrawString("Brake", title_fnt, title_brush, (int)((i < 40 + 40) ? -1000 : (i > 80 + 40) ? 17 : 17 * (i - (40 + 40)) / 40.0), 852);
                g.FillRectangle(Brushes.LightGray, 0, 913, (int)(image_width * ((i > 30) ? 1 : i / 30.0)), 8);

                g.FillRectangle(new SolidBrush(Color.FromArgb((int)(0xB0 * ((i > 96) ? (128 - i) / 36.0 : 1)), 0x00, 0x00, 0x00)), 0, 0, image_width, image_height);
                int transparency = (int)(0xFF * ((i > 96) ? (128 - i) / 36.0 : 1));
                g.DrawString("C# VVVF Simulator", simulator_title_fnt, new SolidBrush(Color.FromArgb(transparency, 0xFF, 0xFF, 0xFF)), 50, 420);
                g.DrawLine(new Pen(new SolidBrush(Color.FromArgb(transparency, 0xA0, 0xA0, 0xFF))), 0, 464, (int)((i > 20) ? image_width : image_width * i / 20.0), 464);
                g.DrawString("presented by JOTAN", simulator_title_fnt_sub, new SolidBrush(Color.FromArgb(transparency, 0xE0, 0xE0, 0xFF)), 135, 460);

                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                byte[] img = ms.GetBuffer();
                Mat mat = OpenCvSharp.Mat.FromImageData(img);

                Cv2.ImShow("Wave Status View", mat);
                Cv2.WaitKey(1);

                vr.Write(mat);
            }


        }
        public static void generate_status_video(String output_path, Yaml_Sound_Data sound_data)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = "";
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            Int32 sound_block_count = 0;

            int image_width = 500;
            int image_height = 1080;
            int movie_div = 3000;
            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));

            if (!vr.IsOpened())
            {
                return;
            }

            generate_opening(image_width, image_height, vr);


            bool loop = true, temp = false, video_finished = false, final_show = false, first_show = true;
            int freeze_count = 0;

            Bitmap image = new(image_width, image_height);
            Graphics g = Graphics.FromImage(image);

            while (loop)
            {
                Control_Values cv_U = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = get_Control_Frequency()
                };
                Yaml_VVVF_Wave.calculate_Yaml(cv_U , sound_data);

                if (sound_block_count % movie_div == 0 && temp || final_show || first_show)
                {
                    set_Sine_Time(0);
                    set_Saw_Time(0);

                    Color gradation_color;
                    if (is_Free_Running())
                    {
                        gradation_color = Color.FromArgb(0xE0, 0xFD, 0xE0);
                    }
                    else if (!is_Braking())
                    {
                        gradation_color = Color.FromArgb(0xE0, 0xE0, 0xFD);
                    }
                    else
                    {
                        gradation_color = Color.FromArgb(0xFD, 0xE0, 0xE0);
                    }


                    LinearGradientBrush gb = new LinearGradientBrush(
                        new System.Drawing.Point(0, 0),
                        new System.Drawing.Point(image_width, image_height),
                        Color.FromArgb(0xFF, 0xFF, 0xFF),
                        gradation_color
                    );

                    g.FillRectangle(gb, 0, 0, image_width, image_height);

                    FontFamily title_fontFamily = new FontFamily("Fugaz One");
                    Font title_fnt = new Font(
                       title_fontFamily,
                       40,
                       FontStyle.Regular,
                       GraphicsUnit.Pixel);

                    FontFamily val_fontFamily = new FontFamily("Arial Rounded MT Bold");
                    Font val_fnt = new Font(
                       val_fontFamily,
                       50,
                       FontStyle.Regular,
                       GraphicsUnit.Pixel);

                    FontFamily val_mini_fontFamily = new FontFamily("Arial Rounded MT Bold");
                    Font val_mini_fnt = new Font(
                       val_mini_fontFamily,
                       25,
                       FontStyle.Regular,
                       GraphicsUnit.Pixel);

                    Brush title_brush = Brushes.Black;
                    Brush letter_brush = Brushes.Black;

                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 0, image_width, 68 - 0);
                    g.DrawString("Pulse Mode", title_fnt, title_brush, 17, 8);
                    g.FillRectangle(Brushes.Blue, 0, 68, image_width, 8);
                    if (!final_show)
                    {
                        String[] pulse_name = get_Pulse_Name(Video_Generate_Values.pulse_mode);

                        g.DrawString(pulse_name[0], val_fnt, letter_brush, 17, 100);

                        if (pulse_name.Length > 1)
                        {
                            if (pulse_name.Length == 2)
                            {
                                g.DrawString(pulse_name[1], val_mini_fnt, letter_brush, 17, 170);
                            }
                            else if (pulse_name.Length == 3)
                            {
                                g.DrawString(pulse_name[1], val_mini_fnt, letter_brush, 17, 160);
                                g.DrawString(pulse_name[2], val_mini_fnt, letter_brush, 17, 180);
                            }
                        }

                    }


                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 226, image_width, 291 - 226);
                    g.DrawString("Sine Freq[Hz]", title_fnt, title_brush, 17, 231);
                    g.FillRectangle(Brushes.Blue, 0, 291, image_width, 8);
                    double sine_freq = Video_Generate_Values.sine_angle_freq / Math.PI / 2;
                    if (!final_show)
                        g.DrawString(String.Format("{0:f2}", sine_freq).PadLeft(6), val_fnt, letter_brush, 17, 323);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 447, image_width, 513 - 447);
                    g.DrawString("Sine Amplitude[%]", title_fnt, title_brush, 17, 452);
                    g.FillRectangle(Brushes.Blue, 0, 513, image_width, 8);
                    if (!final_show)
                        g.DrawString(String.Format("{0:f2}", Video_Generate_Values.sine_amplitude * 100).PadLeft(6), val_fnt, letter_brush, 17, 548);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), 0, 669, image_width, 735 - 669);
                    g.DrawString("Freerun", title_fnt, title_brush, 17, 674);
                    g.FillRectangle(Brushes.LightGray, 0, 735, image_width, 8);
                    if (!final_show)
                        g.DrawString(is_Mascon_Off().ToString(), val_fnt, letter_brush, 17, 750);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), 0, 847, image_width, 913 - 847);
                    g.DrawString("Brake", title_fnt, title_brush, 17, 852);
                    g.FillRectangle(Brushes.LightGray, 0, 913, image_width, 8);
                    if (!final_show)
                        g.DrawString(is_Braking().ToString(), val_fnt, letter_brush, 17, 930);




                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Status View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    temp = false;
                }
                else if (sound_block_count % movie_div != 0)
                {
                    temp = true;
                }

                if (first_show)
                {
                    freeze_count++;
                    if (freeze_count > 64)
                    {
                        freeze_count = 0;
                        first_show = false;
                    }
                    continue;
                }
                sound_block_count++;

                video_finished = !check_for_freq_change();
                if (video_finished)
                {
                    final_show = true;
                    freeze_count++;
                }
                if (freeze_count > 64) loop = false;
            }

            g.Dispose();
            image.Dispose();

            vr.Release();
            vr.Dispose();
        }

        private static void filled_corner_curved_rectangle(Graphics g, Brush br , Point start, Point end, int round_radius)
        {
            int width = end.X - start.X;
            int height = end.Y - start.Y;

            g.FillRectangle(br, start.X + round_radius, start.Y, width - 2 * round_radius, height);
            g.FillRectangle(br, start.X, start.Y + round_radius, round_radius, height - 2 * round_radius);
            g.FillRectangle(br, end.X - round_radius, start.Y + round_radius, round_radius, height - 2 * round_radius);

            g.FillEllipse(br, start.X, start.Y, round_radius * 2, round_radius * 2);
            g.FillEllipse(br, start.X, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2);
            g.FillEllipse(br, start.X + width - round_radius * 2, start.Y, round_radius * 2, round_radius * 2);
            g.FillEllipse(br, start.X + width - round_radius * 2, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2);

        }

        private static Point center_text_with_filled_corner_curved_rectangle(Graphics g, String str, Brush str_br, Font fnt, Brush br,
                                                                 Point start, Point end, int round_radius, Point str_compen)
        {
            SizeF strSize = g.MeasureString(str, fnt);

            int width = end.X - start.X;
            int height = end.Y - start.Y;

            filled_corner_curved_rectangle(g, br, start, end, round_radius);

            Point str_pos = new Point((int)Math.Round(start.X + width / 2 - strSize.Width / 2 + str_compen.X), (int)Math.Round(start.Y + height / 2 - strSize.Height / 2 + str_compen.Y));

            g.DrawString(str, fnt, str_br, str_pos);

            return str_pos;




        }

        private static void line_corner_curved_rectangle(Graphics g, Pen pen, Point start, Point end, int round_radius)
        {
            int width = (int)(end.X - start.X);
            int height = (int)(end.Y - start.Y);

            g.DrawLine(pen, start.X + round_radius, start.Y, end.X -  round_radius + 1, start.Y);
            g.DrawLine(pen, start.X + round_radius, end.Y, end.X -  round_radius + 1, end.Y);

            g.DrawLine(pen, start.X, start.Y + round_radius, start.X, end.Y - round_radius + 1);
            g.DrawLine(pen, end.X, start.Y + round_radius, end.X, end.Y - round_radius + 1);

            g.DrawArc(pen, start.X, start.Y, round_radius * 2, round_radius * 2 , -90, -90);
            g.DrawArc(pen, start.X, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2 , -180 , -90);
            g.DrawArc(pen, start.X + width - round_radius * 2, start.Y, round_radius * 2, round_radius * 2 , 0 , -90);
            g.DrawArc(pen, start.X + width - round_radius * 2, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2 , 0 , 90);

        }

        private static void title_str_with_line_corner_curved_rectangle(Graphics g, String str , Brush str_br, Font fnt, Pen pen,
                                                                 Point start, Point end, int round_radius, Point str_compen)
        {

            SizeF strSize = g.MeasureString(str, fnt);

            int width = end.X - start.X;
            int height = end.Y - start.Y;

            g.DrawLine(pen, start.X + round_radius, start.Y, start.X + width / 2 - strSize.Width / 2 - 10, start.Y);
            g.DrawLine(pen, end.X - round_radius + 1, start.Y, start.X + width / 2 + strSize.Width / 2 + 10, start.Y);

            g.DrawString(str, fnt, str_br, start.X + width / 2 - strSize.Width / 2 + str_compen.X, start.Y - fnt.Height / 2 + str_compen.Y);

            g.DrawLine(pen, start.X + round_radius, end.Y, end.X - round_radius + 1, end.Y);

            g.DrawLine(pen, start.X, start.Y + round_radius, start.X, end.Y - round_radius + 1);
            g.DrawLine(pen, end.X, start.Y + round_radius, end.X, end.Y - round_radius + 1);

            g.DrawArc(pen, start.X, start.Y, round_radius * 2, round_radius * 2, -90, -90);
            g.DrawArc(pen, start.X, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2, -180, -90);
            g.DrawArc(pen, start.X + width - round_radius * 2, start.Y, round_radius * 2, round_radius * 2, 0, -90);
            g.DrawArc(pen, start.X + width - round_radius * 2, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2, 0, 90);

        }

        private static Point center_text_with_line_corner_curved_rectangle(Graphics g, String str, Brush str_br, Font fnt, Pen pen,
                                                                 Point start, Point end, int round_radius, Point str_compen)
            {

            SizeF strSize = g.MeasureString(str, fnt);
            int width = end.X - start.X;
            int height = end.Y - start.Y;
            line_corner_curved_rectangle(g, pen, start, end, round_radius);

            Point string_pos = new Point((int)Math.Round(start.X + width / 2 - strSize.Width / 2 + str_compen.X), (int)Math.Round(start.Y + height / 2 - strSize.Height / 2 + str_compen.Y));
            g.DrawString(str, fnt, str_br, string_pos);

            return string_pos;


        }

        /// <summary>
        /// 通常の大きさ(size=1)の時、横 160,縦200
        /// </summary>
        /// <param name="g"></param>
        /// <param name="start"></param>
        /// <param name="c"></param>
        /// <param name="hole_c"></param>
        /// <param name="locked"></param>
        /// <param name="size"></param>
        public static void draw_key(Graphics g,Point start, Color c, Color hole_c, Boolean locked, double size)
        {
            Pen default_pen = new Pen(c, (int)Math.Round(20 * size));
            g.DrawArc(default_pen, (int)Math.Round(start.X + 42 * size), (int)Math.Round(start.Y + 20 * size) , (int)Math.Round(74 * size) , (int)Math.Round(74 * size), -180, 180);
            g.DrawLine(default_pen, (int)Math.Round(start.X + 42 * size), (int)Math.Round(start.Y + 56 * size), (int)Math.Round(start.X + 42 * size), (int)Math.Round(start.Y + 103 * size));

            if (locked)
                g.DrawLine(default_pen, (int)Math.Round(start.X + 116 * size), (int)Math.Round(start.Y + 56 * size), (int)Math.Round(start.X + 116 * size), (int)Math.Round(start.Y + 103 * size));
            else
                g.FillEllipse(new SolidBrush(c), (int)Math.Round(start.X + 106 * size), (int)Math.Round(start.Y + 47 * size), (int)Math.Round((20) * size), (int)Math.Round((20) * size));

            filled_corner_curved_rectangle(g, new SolidBrush(c), new Point((int)Math.Round(start.X + 8 * size), (int)Math.Round(start.Y + 90 * size)), new Point((int)Math.Round(start.X + 151 * size), (int)Math.Round(start.Y + 193 * size)), (int)Math.Round((10) * size));

            g.FillEllipse(new SolidBrush(hole_c), (int)Math.Round(start.X + 64 * size), (int)Math.Round(start.Y + 124 * size), (int)Math.Round((28) * size), (int)Math.Round((28) * size));
            g.DrawLine(new Pen(hole_c, (int)Math.Round((15)* size)), (int)Math.Round(start.X + 78.5 * size), (int)Math.Round(start.Y + 138 * size), (int)Math.Round(start.X + 78.5 * size), (int)Math.Round(start.Y + 173 * size));
        }

        public static double get_wave_form_voltage_rate_with_surface(Yaml_Sound_Data sound_data)
        {
            int hex_div_seed = 10000;
            int hex_div = 6 * hex_div_seed;
            double[] hexagon_coordinate = new double[] { 100, 500 };

            List<List< Int32 >> y_coordinate = new List<List<Int32>>();

            for(int i = 0; i < 1000; i++)
            {
                y_coordinate.Add(new List<Int32>());
            }

            for (int i = 0; i < hex_div; i++)
            {
                add_Sine_Time(1.0 / (hex_div) * ((get_Sine_Freq() == 0) ? 0 : 1 / get_Sine_Freq()));
                add_Saw_Time(1.0 / (hex_div) * ((get_Sine_Freq() == 0) ? 0 : 1 / get_Sine_Freq()));

                Control_Values cv_U = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(cv_U, sound_data);

                Control_Values cv_V = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(cv_V, sound_data);

                Control_Values cv_W = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 2,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_W = Yaml_VVVF_Wave.calculate_Yaml(cv_W, sound_data);

                double move_x = -0.5 * wv_W.pwm_value - 0.5 * wv_V.pwm_value + wv_U.pwm_value;
                double move_y = -0.866025403784438646763 * wv_W.pwm_value + 0.866025403784438646763 * wv_V.pwm_value;
                double int_move_x = 200 * move_x / (double)hex_div_seed;
                double int_move_y = 200 * move_y / (double)hex_div_seed;

                List<Int32> x1 = y_coordinate[(int)Math.Round(hexagon_coordinate[0])];
                if(!x1.Contains((int)Math.Round(hexagon_coordinate[1])))
                    x1.Add((int)Math.Round(hexagon_coordinate[1]));

                List<Int32> x2 = y_coordinate[(int)Math.Round(hexagon_coordinate[0] + int_move_x)];
                if(!x2.Contains((int)Math.Round(hexagon_coordinate[1] + int_move_y)))
                    x2.Add((int)Math.Round(hexagon_coordinate[1] + int_move_y));

                hexagon_coordinate[0] = hexagon_coordinate[0] + int_move_x;
                hexagon_coordinate[1] = hexagon_coordinate[1] + int_move_y;
            }

            int total_dots = 0;
            for(int i = 0; i < 1000; i++)
            {
                List<Int32> y_dots = y_coordinate[i];
                int repeat = y_dots.Count / 2;
                for(int j = 0; j < repeat; j++)
                {
                    total_dots += Math.Abs(y_dots[2 * j] - y_dots[2 * j + 1]);
                }
            }

            double voltage = total_dots/374763.0 * 100;
            if (voltage > 100)
                voltage = 100;
            return voltage;
        }

        public static double get_wave_form_voltage_rate_with_radius(Yaml_Sound_Data sound_data)
        {
            double hex_div_seed = 10000 * ((get_Sine_Freq() > 0 && get_Sine_Freq() < 1) ? 1 / get_Sine_Freq() : 1);
            int hex_div = (int)Math.Round(6 * hex_div_seed);
            double[] hexagon_coordinate = new double[] { 100, 500 };

            double min_x = 2000, max_x = 0;

            for (int i = 0; i < hex_div; i++)
            {
                add_Sine_Time(1.0 / (hex_div) * ((get_Sine_Freq() == 0) ? 0 : 1 / get_Sine_Freq()));
                add_Saw_Time(1.0 / (hex_div) * ((get_Sine_Freq() == 0) ? 0 : 1 / get_Sine_Freq()));

                Control_Values cv_U = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(cv_U, sound_data);

                Control_Values cv_V = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(cv_V, sound_data);

                Control_Values cv_W = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 2,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_W = Yaml_VVVF_Wave.calculate_Yaml(cv_W, sound_data);

                double move_x = -0.5 * wv_W.pwm_value - 0.5 * wv_V.pwm_value + wv_U.pwm_value;
                double move_y = -0.866025403784438646763 * wv_W.pwm_value + 0.866025403784438646763 * wv_V.pwm_value;
                double int_move_x = 200 * move_x / hex_div_seed;
                double int_move_y = 200 * move_y / hex_div_seed;

                hexagon_coordinate[0] = hexagon_coordinate[0] + int_move_x;
                hexagon_coordinate[1] = hexagon_coordinate[1] + int_move_y;

                if (min_x > Math.Round(hexagon_coordinate[0]))
                    min_x = hexagon_coordinate[0];
                if (max_x < Math.Round(hexagon_coordinate[0]))
                    max_x = hexagon_coordinate[0];
            }

            double voltage = Math.Round((max_x - min_x) / 8.0,1);
            
            if (voltage > 100)
                voltage = 100;

            return voltage;
        }

        private static String[] get_Taroimo_Pulse_Name(Pulse_Mode mode)
        {
  
            if (mode == Pulse_Mode.P_Wide_3)
                return new string[] { "Wide 3"};

            if (mode.ToString().StartsWith("CHM"))
            {
                String mode_name = mode.ToString();
                bool contain_wide = mode_name.Contains("Wide");
                mode_name = mode_name.Replace("_Wide", "");

                String[] mode_name_type = mode_name.Split("_");

                return new string[] { (contain_wide ? "Wide " : "") + mode_name_type[1] };
            }
            if (mode.ToString().StartsWith("SHE"))
            {
                String mode_name = mode.ToString();
                bool contain_wide = mode_name.Contains("Wide");
                mode_name = mode_name.Replace("_Wide", "");

                String[] mode_name_type = mode_name.Split("_");

                return new string[] { (contain_wide ? "Wide " : "") + mode_name_type[1] };
            }
            else
            {
                String[] mode_name_type = mode.ToString().Split("_");
                //((mode_name_type[0] == "SP") ? "Shifted " : "") + 
                return new string[] { mode_name_type[1] };
            }
        }

        public static void generate_status_taroimo_like_video(String output_path, Yaml_Sound_Data sound_data)
        {
            reset_control_variables();
            reset_all_variables();

            set_Allowed_Random_Freq_Move(false);

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = "";
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            Int32 sound_block_count = 0;

            int image_width = 960;
            int image_height = 1620;
            int movie_div = 3000;
            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));

            if (!vr.IsOpened())
            {
                return;
            }


            bool loop = true, temp = false, video_finished = false, final_show = false, first_show = true;
            int freeze_count = 0;

            bool lang_jpn, font_jpn;
            while (true)
            {
                Console.WriteLine("Language mode (EN / JA) : ");
                String mode = Console.ReadLine();
                if (mode.ToLower().Equals("ja"))
                {
                    lang_jpn = true;
                    font_jpn = true;
                    break;
                }
                else if (mode.ToLower().Equals("en"))
                {
                    lang_jpn = false;
                    font_jpn = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid value.");
                }
            }

            if (!lang_jpn)
            {
                while (true)
                {
                    Console.WriteLine("Font mode (EN / JA) : ");
                    String mode = Console.ReadLine();
                    if (mode.ToLower().Equals("ja"))
                    {
                        font_jpn = true;
                        break;
                    }
                    else if (mode.ToLower().Equals("en"))
                    {
                        font_jpn = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid value.");
                    }
                }
            }

            Font[] jpn_fonts = new Font[] {
                new(new FontFamily("VDL ロゴＧ R"), 75, FontStyle.Regular, GraphicsUnit.Pixel), //topic
                new(new FontFamily("DIN 2014 Light"), 173, FontStyle.Regular, GraphicsUnit.Pixel), //value 
                new(new FontFamily("FOT-UD角ゴ_ラージ Pr6N M"), 52, FontStyle.Regular, GraphicsUnit.Pixel), //syncmode 
                new(new FontFamily("DIN 2014 Light"), 96, FontStyle.Regular, GraphicsUnit.Pixel), //unit
                new(new FontFamily("FOT-UD角ゴ_ラージ Pr6N M"), 78, FontStyle.Regular, GraphicsUnit.Pixel), //pulse unit
                new(new FontFamily("VDL ロゴＧ R"), 115, FontStyle.Regular, GraphicsUnit.Pixel), //control stat
            };
            Font[] eng_fonts = new Font[] {
                new(new FontFamily("Fugaz One"), 80, FontStyle.Regular, GraphicsUnit.Pixel),
                new(new FontFamily("Meiryo"), 135, FontStyle.Regular, GraphicsUnit.Pixel),
                new(new FontFamily("Meiryo"), 60, FontStyle.Regular, GraphicsUnit.Pixel),
                new Font(new FontFamily("Meiryo"), 60, FontStyle.Regular, GraphicsUnit.Pixel),
                new(new FontFamily("Fugaz One"), 80, FontStyle.Regular, GraphicsUnit.Pixel),
            };
            Font[] lng_fonts = font_jpn ? jpn_fonts : eng_fonts;
            //Control stat , Carrier , Async , Sync , Output, Freq ,Volt , Carrier_Num, Key , Carrier_Unit
            Point[] jpn_f_jpn_str_compen = new Point[] { 
                new Point(0, 43), // control stat
                new Point(2, 19), // "Carrier"
                new Point(36, 5), // "Async"
                new Point(36, 5), // "Sync"
                new Point(0, 25), // "Output"
                new Point(6, 30), // "Freq"
                new Point(0, 30), // "Volt"
                new Point(20, -4),// Carrier_Num
                new Point(-4, 5) , // Key
                new Point(0, -24), // Carrier_Unit
                new Point(16, -8), // Sine Freq (10
                new Point(-11, -16), // Sine Freq Unit (11
                new Point(6, -7), // Voltage (12
                new Point(-20, -7), // Voltage Unit (13
            };
            Point[] eng_f_eng_str_compen = new Point[] { new Point(0, 0), new Point(0, 0), new Point(40, 5), new Point(40, 5), new Point(0, 0), new Point(0, 5), new Point(0, 5) , new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0) };
            Point[] eng_f_jpn_str_compen = new Point[] {
                new Point(0, 0), // control stat
                new Point(0, 19), // "Carrier"
                new Point(36, -1), // "Async"
                new Point(36, -1), // "Sync"
                new Point(0, 25), // "Output"
                new Point(0, 0), // "Freq"
                new Point(0, 0), // "Volt"
                new Point(20, -4),// Carrier_Num
                new Point(-4, 5) , // Key
                new Point(0, -24), // Carrier_Unit
                new Point(16, -8), // Sine Freq (10
                new Point(-11, -16), // Sine Freq Unit (11
                new Point(6, -7), // Voltage (12
                new Point(-20, -7), // Voltage Unit (13
            };
            Point[] lng_str_compen = lang_jpn ? jpn_f_jpn_str_compen : font_jpn ? eng_f_jpn_str_compen : eng_f_eng_str_compen;

            String[] jpn_f_jpn_words = new String[] { "惰行", "力行", "制動", "停止", "キャリア", "非同期モード", "パルス", "同期モード", "出力", "周波数", "電圧" };
            String[] eng_f_eng_words = new String[] { "Cruising", "Accelerate", "Braking", "Stopping", "Carrier", "Async Mode", "Pulse", "Sync Mode", "Output", "Freq", "Volt" };
            String[] eng_f_jpn_words = new String[] { "Cruise", "Accel", "Brake", "Stop", "Carrier", "Async Mode", "Pulse", "Sync Mode", "Output", "Freq", "Volt" };

            String[] lng_words = lang_jpn ? jpn_f_jpn_words : font_jpn ? eng_f_jpn_words : eng_f_eng_words;

            Bitmap background = new(image_width, image_height);
            Graphics backgound_g = Graphics.FromImage(background);
            for(int x = 0; x < 22; x++)
            {
                backgound_g.FillPolygon(new SolidBrush(Color.FromArgb(51, 51, 51)),new PointF[]
                {
                    new PointF(0 , 60 * 2 * x),
                    new PointF(0 , 60 * (2*x + 1)),
                    new PointF(image_width , 60 * (2*x + 1) - image_width),
                    new PointF(image_width, 60 * 2 * x - image_width),
                });

                backgound_g.FillPolygon(new SolidBrush(Color.FromArgb(47, 47, 47)), new PointF[]
                {
                    new PointF(0 , 60 * (2*x + 1)),
                    new PointF(0 , 60 * (2*x + 2)),
                    new PointF(image_width, 60 * (2*x + 2)- image_width),
                    new PointF(image_width, 60 * (2*x + 1)- image_width),
                });
            }

            backgound_g.Dispose();

            Bitmap final_image = new(image_width, image_height);
            Graphics final_g = Graphics.FromImage(final_image);

            double pre_voltage = 0.0;

            

            while (loop)
            {
                Control_Values cv_U = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = get_Control_Frequency()
                };
                Yaml_VVVF_Wave.calculate_Yaml(cv_U, sound_data);

                if (sound_block_count % movie_div == 0 && temp || final_show || first_show)
                {
                    Bitmap info_image = new(image_width, image_height);
                    Graphics info_g = Graphics.FromImage(info_image);

                    Bitmap control_stat_image = new(image_width, image_height);
                    Graphics control_stat_g = Graphics.FromImage(control_stat_image);

                    set_Sine_Time(0);
                    set_Saw_Time(0);

                    Color control_color,control_str_color;
                    String status_str;
                    if (is_Free_Running())
                    {
                        control_color = Color.FromArgb(153, 204, 0);
                        control_str_color = Color.FromArgb(28, 68, 0);
                        status_str = lng_words[0];
                    }
                    else if (!is_Braking())
                    {
                        control_color = Color.FromArgb(0, 204, 255);
                        control_str_color = Color.FromArgb(0, 56, 112);
                        status_str = lng_words[1];
                    }
                    else
                    {
                        control_color = Color.FromArgb(255, 153, 0);
                        control_str_color = Color.FromArgb(70, 22, 0);
                        status_str = lng_words[2];
                    }

                    if (final_show)
                    {
                        control_color = Color.FromArgb(242, 45, 45);
                        control_str_color = Color.Black;
                        status_str = lng_words[3];
                    }

                    // Triangle
                    control_stat_g.FillPolygon(new SolidBrush(control_color), new PointF[] {
                        new PointF(464 * 2,105 * 2),
                        new PointF(446 * 2,89 * 2),
                        new PointF(446 * 2,120 * 2)
                    });

                    // Rikkou
                    control_stat_g.FillRectangle(new SolidBrush(control_color), 474 * 2, 0 * 2, 7 * 2, 810 * 2);
                    center_text_with_filled_corner_curved_rectangle(
                        control_stat_g, 
                        status_str, 
                        new SolidBrush(control_str_color), 
                        lng_fonts[5], 
                        new SolidBrush(control_color), 
                        new Point(30 * 2, 45 * 2), 
                        new Point(449 * 2, 163 * 2), 
                        20,
                        lng_str_compen[0]
                    );

                    // Carrier
                    title_str_with_line_corner_curved_rectangle(
                        info_g ,
                        lng_words[4], 
                        new SolidBrush(Color.FromArgb(144, 144, 144)), 
                        lng_fonts[0], 
                        new Pen(Color.FromArgb(109, 109, 109), 4), 
                        new Point(30 * 2, 225 * 2), 
                        new Point(449 * 2, 428 * 2),
                        20,
                        lng_str_compen[1]
                    );

                    // Connect dots
                    if (!(Video_Generate_Values.pulse_mode == Pulse_Mode.Async || Video_Generate_Values.pulse_mode == Pulse_Mode.Async_THI))
                    {
                        int connect_sync_box_size = 12;
                        for (int r = 0; r < 9; r++)
                        {
                            // 560 + 49 - 4 NO.5
                            info_g.FillRectangle(new SolidBrush(Color.White), 47 * 2 + 67 * 2 - connect_sync_box_size / 2, 411 * 2 - connect_sync_box_size / 2 + connect_sync_box_size * 2 * r , connect_sync_box_size, connect_sync_box_size);
                        }
                    }
                    else
                    {
                        int connect_sync_box_size = 12;
                        for (int r = 0; r < 9; r++)
                        {
                            info_g.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), 47 * 2 + 67 * 2 - connect_sync_box_size / 2, 411 * 2 - connect_sync_box_size / 2 + connect_sync_box_size * 2 * r + ((r == 0) ? 4 : 0), connect_sync_box_size, connect_sync_box_size - ((r == 0 || r == 8) ? 4 : 0));
                        }
                    }

                    // Carrier Freq Show
                    if (Video_Generate_Values.pulse_mode == Pulse_Mode.Async || Video_Generate_Values.pulse_mode == Pulse_Mode.Async_THI)
                    {
                        String carrier_freq_str = String.Format("{0:f0}", Video_Generate_Values.carrier_freq_data.base_freq);
                        SizeF freq_str_size = info_g.MeasureString(carrier_freq_str, lng_fonts[1]);
                        SizeF hz_str_size = info_g.MeasureString("Hz", lng_fonts[4]);

                        int total_width = (int)(freq_str_size.Width + hz_str_size.Width);

                        // What Carrier Freq Show
                        info_g.DrawString(
                            carrier_freq_str, 
                            lng_fonts[1], 
                            new SolidBrush(Color.White),
                            image_width / 2 - total_width / 2 + lng_str_compen[7].X, 
                            250 * 2 + lng_str_compen[7].Y
                        );
                        // Hz
                        info_g.DrawString(
                            "Hz",
                            lng_fonts[4], 
                            new SolidBrush(Color.White), 
                            image_width / 2 - total_width / 2 + freq_str_size.Width + lng_str_compen[9].X, 
                            292 * 2 + lng_str_compen[9].Y
                        );

                        // Async Mode
                        Point letter = center_text_with_line_corner_curved_rectangle(
                            info_g,
                            lng_words[5],
                            new SolidBrush(Color.FromArgb(144, 144, 144)),
                            lng_fonts[2], 
                            new Pen(Color.FromArgb(109, 109, 109), 4), 
                            new Point(47 * 2, 359 * 2), 
                            new Point(431 * 2, 410 * 2), 
                            20,
                            lng_str_compen[2]
                        );

                        // Draw unlocked key
                        draw_key(info_g, new Point(letter.X - 50 + lng_str_compen[8].X, letter.Y + 18 + lng_str_compen[8].Y), Color.FromArgb(103, 103, 103), Color.FromArgb(52, 52, 52), false, 0.25);
                    }
                    else
                    {
                        String[] pulse_mode_name = get_Taroimo_Pulse_Name(Video_Generate_Values.pulse_mode);

                        SizeF freq_str_size = info_g.MeasureString(pulse_mode_name[0], lng_fonts[1]);
                        SizeF freq_other_str_size = info_g.MeasureString(lng_words[6], lng_fonts[4]);

                        double total_width = freq_str_size.Width + freq_other_str_size.Width;

                        // Pulse mode name
                        info_g.DrawString(
                            pulse_mode_name[0], 
                            lng_fonts[1], 
                            new SolidBrush(Color.White),
                            (int)Math.Round(920 / 2 - total_width / 2) + lng_str_compen[7].X,
                            250 * 2 + lng_str_compen[7].Y
                        );

                        //Draw "pulse"
                        info_g.DrawString(
                            lng_words[6],
                            lng_fonts[4],
                            new SolidBrush(Color.White),
                            (int)Math.Round(920 / 2 - total_width / 2 + freq_str_size.Width) + lng_str_compen[9].X,
                            580 + lng_str_compen[9].Y
                        );

                        // sync mode
                        Point letter = center_text_with_filled_corner_curved_rectangle(
                            info_g,
                            lng_words[7],
                            new SolidBrush(Color.FromArgb(52, 52, 52)), 
                            lng_fonts[2],
                            new SolidBrush(Color.White), 
                            new Point(47 * 2, 359 * 2), 
                            new Point(431 * 2, 410 * 2),
                            20,
                            lng_str_compen[3]
                        );

                        draw_key(info_g, new Point(letter.X - 50 + lng_str_compen[8].X, letter.Y + 18 + lng_str_compen[8].Y), Color.FromArgb(52, 52, 52), Color.White, true, 0.25);
                    }

                    //OUTPUT
                    title_str_with_line_corner_curved_rectangle(
                        info_g,
                        lng_words[8], 
                        new SolidBrush(Color.FromArgb(144, 144, 144)), 
                        lng_fonts[0], new Pen(Color.FromArgb(109, 109, 109),4),
                        new Point(30 * 2, 474 * 2), 
                        new Point( 449 * 2, 731 * 2),
                        20,
                        lng_str_compen[4]
                    );
                    
                    // " Freq "
                    if(Video_Generate_Values.pulse_mode == Pulse_Mode.Async || Video_Generate_Values.pulse_mode == Pulse_Mode.Async_THI)
                        center_text_with_line_corner_curved_rectangle(
                            info_g,
                            lng_words[9], 
                            new SolidBrush(Color.White),
                            lng_fonts[0], 
                            new Pen(Color.White, 4), 
                            new Point(47 * 2, 507 * 2),
                            new Point(182 * 2, 602 * 2),
                            10,
                            lng_str_compen[5]
                        );
                    else 
                        center_text_with_filled_corner_curved_rectangle(
                            info_g,
                            lng_words[9],
                            new SolidBrush(Color.FromArgb(52,52,52)), 
                            lng_fonts[0], 
                            new SolidBrush(Color.White),
                            new Point(47 * 2, 507 * 2), 
                            new Point(182 * 2, 602 * 2),
                            10,
                            lng_str_compen[5]
                        );

                    //Sine Freq
                    double sine_freq = Video_Generate_Values.sine_angle_freq / Math.PI / 2;
                    if (!final_show)
                    {
                        int base_pos = 620;
                        String sine_freq_str = String.Format("{0:f1}", sine_freq);
                        SizeF freq_str_size = info_g.MeasureString(sine_freq_str, lng_fonts[1]);
                        SizeF hz_str_size = info_g.MeasureString("Hz", lng_fonts[3]);

                        int total_width = (int)(freq_str_size.Width + hz_str_size.Width);

                        info_g.DrawString(sine_freq_str, lng_fonts[1], new SolidBrush(Color.White), base_pos - total_width / 2 + lng_str_compen[10].X, 505 * 2 + lng_str_compen[10].Y);
                        info_g.DrawString("Hz", lng_fonts[3], new SolidBrush(Color.White), base_pos - total_width / 2 + freq_str_size.Width + lng_str_compen[11].X, 545 * 2 + lng_str_compen[11].Y);
                    }

                    //" Voltage "
                    center_text_with_line_corner_curved_rectangle(
                        info_g,
                        lng_words[10],
                        new SolidBrush(Color.White),
                        lng_fonts[0], 
                        new Pen(Color.White, 4),
                        new Point(47 * 2, 618 * 2),
                        new Point(182 * 2, 713 * 2), 
                        10,
                        lng_str_compen[6]
                    );

                    if (!final_show)
                    {
                        int base_pos = 620;

                        double voltage = get_wave_form_voltage_rate_with_radius(sound_data);
                        pre_voltage = Math.Round((voltage + pre_voltage) / 2.0, 1);
                        pre_voltage = (pre_voltage > 99.5) ? 100 : pre_voltage;
                        String sine_freq_str = String.Format("{0:f1}", pre_voltage);
                        SizeF freq_str_size = info_g.MeasureString(sine_freq_str, lng_fonts[1]);
                        SizeF hz_str_size = info_g.MeasureString("%", lng_fonts[3]);

                        int total_width = (int)(freq_str_size.Width + hz_str_size.Width);

                        info_g.DrawString(sine_freq_str, lng_fonts[1], new SolidBrush(Color.White), base_pos - total_width / 2 + lng_str_compen[12].X, 620 * 2 + lng_str_compen[12].Y);
                        info_g.DrawString("%", lng_fonts[3], new SolidBrush(Color.White), base_pos - total_width / 2 + freq_str_size.Width + lng_str_compen[13].X, 1200 + 115 + lng_str_compen[13].Y);
                    }

                    final_g.DrawImage(background,0,0);

                    if (first_show || final_show)
                    {
                        ColorMatrix cm = new ColorMatrix();
                        cm.Matrix00 = 1;
                        cm.Matrix11 = 1;
                        cm.Matrix22 = 1;
                        cm.Matrix33 = 0.5F;
                        cm.Matrix44 = 1;

                        ImageAttributes ia = new ImageAttributes();
                        ia.SetColorMatrix(cm);

                        final_g.DrawImage(control_stat_image, new Rectangle(0, 0, image_width, image_height),
                            0, 0, image_width, image_height, GraphicsUnit.Pixel, ia);
                    }
                    else
                    {
                        final_g.DrawImage(control_stat_image, 0, 0);
                    }

                    control_stat_image.Dispose();
                    control_stat_g.Dispose();

                    if (final_show || first_show || is_Free_Running())
                    {
                        ColorMatrix cm = new ColorMatrix();
                        cm.Matrix00 = 1;
                        cm.Matrix11 = 1;
                        cm.Matrix22 = 1;
                        cm.Matrix33 = 0.5F;
                        cm.Matrix44 = 1;

                        ImageAttributes ia = new ImageAttributes();
                        ia.SetColorMatrix(cm);

                        final_g.DrawImage(info_image, new Rectangle(0, 0, image_width, image_height),
                            0, 0, image_width, image_height, GraphicsUnit.Pixel, ia);

                    }
                    else
                    {
                        final_g.DrawImage(info_image, 0, 0);
                    }

                    MemoryStream ms = new MemoryStream();
                    final_image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);
                    vr.Write(mat);
                    ms.Dispose();
                    mat.Dispose();

                    MemoryStream resized_ms = new MemoryStream();
                    Bitmap resized = new Bitmap(final_image, image_width / 2, image_height / 2);
                    resized.Save(resized_ms, ImageFormat.Png);
                    byte[] resized_img = resized_ms.GetBuffer();
                    Mat resized_mat = OpenCvSharp.Mat.FromImageData(resized_img);
                    Cv2.ImShow("Generation", resized_mat);
                    Cv2.WaitKey(1);
                    resized_mat.Dispose();
                    resized_ms.Dispose();

                    info_g.Dispose();
                    info_image.Dispose();

                    temp = false;
                }
                else if (sound_block_count % movie_div != 0)
                {
                    temp = true;
                }

                if (first_show)
                {
                    freeze_count++;
                    if (freeze_count > 64)
                    {
                        freeze_count = 0;
                        first_show = false;
                    }
                    continue;
                }
                sound_block_count++;

                video_finished = !check_for_freq_change();
                if (video_finished)
                {
                    final_show = true;
                    freeze_count++;
                }
                if (freeze_count > 64) loop = false;
            }

            final_g.Dispose();
            final_image.Dispose();

            vr.Release();
            vr.Dispose();
        }

    }
}
