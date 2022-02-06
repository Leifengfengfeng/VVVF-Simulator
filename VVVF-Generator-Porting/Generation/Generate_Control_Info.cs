using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_sound_definition;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.Generation.Generate_Common;
using System.Drawing.Drawing2D;
using Point = System.Drawing.Point;
using System.Windows.Forms;
using Size = System.Drawing.Size;
using System.Collections.Generic;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_Control_Info
    {

        private static String[] get_Pulse_Name(Pulse_Mode mode)
        {
            //Not in sync
            if (mode == Pulse_Mode.Async || mode == Pulse_Mode.Asyn_THI)
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
        public static void generate_status_video(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);
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
                get_Calculated_Value(sound_name, cv_U);

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

        private static void center_text_with_filled_corner_curved_rectangle(Graphics g, String str, Brush str_br, Font fnt, Brush br,
                                                                 Point start, Point end, int round_radius, Point str_compen)
        {
            SizeF strSize = g.MeasureString(str, fnt);

            int width = end.X - start.X;
            int height = end.Y - start.Y;

            filled_corner_curved_rectangle(g, br, start, end, round_radius);

            g.DrawString(str, fnt, str_br, start.X + width / 2 - strSize.Width / 2 + str_compen.X, start.Y + height / 2 - strSize.Height / 2 + str_compen.Y);

            

        }

        private static void line_corner_curved_rectangle(Graphics g, Pen pen, Point start, Point end, int round_radius)
        {
            int width = (int)(end.X - start.X);
            int height = (int)(end.Y - start.Y);

            g.DrawLine(pen, start.X + round_radius, start.Y, end.X -  round_radius, start.Y);
            g.DrawLine(pen, start.X + round_radius, end.Y, end.X -  round_radius, end.Y);

            g.DrawLine(pen, start.X, start.Y + round_radius, start.X, end.Y - round_radius);
            g.DrawLine(pen, end.X, start.Y + round_radius, end.X, end.Y - round_radius);

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
            g.DrawLine(pen, end.X - round_radius, start.Y, start.X + width / 2 + strSize.Width / 2 + 10, start.Y);

            g.DrawString(str, fnt, str_br, start.X + width / 2 - strSize.Width / 2 + str_compen.X, start.Y - fnt.Height / 2 + str_compen.Y);

            g.DrawLine(pen, start.X + round_radius, end.Y, end.X - round_radius, end.Y);

            g.DrawLine(pen, start.X, start.Y + round_radius, start.X, end.Y - round_radius);
            g.DrawLine(pen, end.X, start.Y + round_radius, end.X, end.Y - round_radius);

            g.DrawArc(pen, start.X, start.Y, round_radius * 2, round_radius * 2, -90, -90);
            g.DrawArc(pen, start.X, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2, -180, -90);
            g.DrawArc(pen, start.X + width - round_radius * 2, start.Y, round_radius * 2, round_radius * 2, 0, -90);
            g.DrawArc(pen, start.X + width - round_radius * 2, start.Y + height - round_radius * 2, round_radius * 2, round_radius * 2, 0, 90);

        }

        private static void center_text_with_line_corner_curved_rectangle(Graphics g, String str, Brush str_br, Font fnt, Pen pen,
                                                                 Point start, Point end, int round_radius, Point str_compen)
            {

            SizeF strSize = g.MeasureString(str, fnt);
            int width = end.X - start.X;
            int height = end.Y - start.Y;
            line_corner_curved_rectangle(g, pen, start, end, round_radius);
            g.DrawString(str, fnt, str_br, start.X + width / 2 - strSize.Width / 2 + str_compen.X, start.Y + height / 2 - strSize.Height / 2 + str_compen.Y);


        }

        public static double get_wave_form_voltage_rage(VVVF_Sound_Names sound_name)
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
                add_Sine_Time(1.0 / (hex_div) * ((get_Control_Frequency() == 0) ? 0 : 1 / get_Control_Frequency()));
                add_Saw_Time(1.0 / (hex_div) * ((get_Control_Frequency() == 0) ? 0 : 1 / get_Control_Frequency()));

                Control_Values cv_U = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_U = get_Calculated_Value(sound_name, cv_U);

                Control_Values cv_V = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_V = get_Calculated_Value(sound_name, cv_V);

                Control_Values cv_W = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 2,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_W = get_Calculated_Value(sound_name, cv_W);

                double move_x = -0.5 * wv_W.pwm_value - 0.5 * wv_V.pwm_value + wv_U.pwm_value;
                double move_y = -0.866025403784438646763 * wv_W.pwm_value + 0.866025403784438646763 * wv_V.pwm_value;
                double int_move_x = 200 * move_x / (double)hex_div_seed;
                double int_move_y = 200 * move_y / (double)hex_div_seed;

                y_coordinate[(int)Math.Round(hexagon_coordinate[0])].Add((int)Math.Round(hexagon_coordinate[1]));
                y_coordinate[(int)Math.Round(hexagon_coordinate[0] + int_move_x)].Add((int)Math.Round(hexagon_coordinate[1] + int_move_y));

                hexagon_coordinate[0] = hexagon_coordinate[0] + int_move_x;
                hexagon_coordinate[1] = hexagon_coordinate[1] + int_move_y;

                


            }

            int total_dots = 0;
            for(int i = 0; i < 500; i++)
            {
                List<Int32> y_dots = y_coordinate[i];
                int repeat = y_dots.Count / 2;
                for(int j = 0; j < repeat; j++)
                {
                    total_dots += Math.Abs(y_dots[j] - y_dots[j + 1]);
                }
            }

            double voltage = total_dots / 208386.0 * 100;
            return voltage;
        }


        public static void generate_status_taroimo_like_video(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            set_Allowed_Random_Freq_Move(false);

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);
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

            Font fnt_default = new Font(
               new FontFamily("Fugaz One"),
               80,
               FontStyle.Regular,
               GraphicsUnit.Pixel);
            Font fnt_num = new Font(
               new FontFamily("Meiryo"),
               135,
               FontStyle.Regular,
               GraphicsUnit.Pixel);
            Font fnt_syncmode = new Font(
               new FontFamily("Meiryo"),
               60,
               FontStyle.Regular,
               GraphicsUnit.Pixel);
            Font fnt_unit = new Font(
               new FontFamily("Fugaz One"),
               60,
               FontStyle.Regular,
               GraphicsUnit.Pixel);

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
                get_Calculated_Value(sound_name, cv_U);

                if (sound_block_count % movie_div == 0 && temp || final_show || first_show)
                {
                    Bitmap info_image = new(image_width, image_height);
                    Graphics info_g = Graphics.FromImage(info_image);

                    Bitmap control_stat_image = new(image_width, image_height);
                    Graphics control_stat_g = Graphics.FromImage(control_stat_image);

                    set_Sine_Time(0);
                    set_Saw_Time(0);

                    Color control_color,control_str_color;
                    PointF text_point;
                    String status_str = "NON";
                    if (is_Free_Running())
                    {
                        control_color = Color.FromArgb(153, 204, 0);
                        control_str_color = Color.FromArgb(28, 68, 0);

                        text_point = new PointF(290, 150);
                        status_str = "Cruising";
                    }
                    else if (!is_Braking())
                    {
                        control_color = Color.FromArgb(0, 204, 255);
                        control_str_color = Color.FromArgb(0, 56, 112);

                        text_point = new PointF(239, 150);
                        status_str = "Accelerate";
                    }
                    else
                    {
                        control_color = Color.FromArgb(255, 153, 0);
                        control_str_color = Color.FromArgb(70, 22, 0);

                        text_point = new PointF(297, 150);
                        status_str = "Braking";
                    }

                    if (final_show)
                    {
                        control_color = Color.FromArgb(242, 45, 45);
                        control_str_color = Color.Black;

                        text_point = new PointF(297, 150);
                        status_str = "Braking";
                    }

                    info_g.FillPolygon(new SolidBrush(control_color), new PointF[] {
                        new PointF(464 * 2,105 * 2),
                        new PointF(446 * 2,89 * 2),
                        new PointF(446 * 2,120 * 2)
                    });
                    control_stat_g.FillRectangle(new SolidBrush(control_color), 474 * 2, 0 * 2, 7 * 2, 810 * 2);
                    filled_corner_curved_rectangle(control_stat_g, new SolidBrush(control_color), new Point(30 * 2, 45 * 2), new Point(449 * 2, 163 * 2), 20);
                    control_stat_g.DrawString(status_str,fnt_default,new SolidBrush(control_str_color), text_point);

                    title_str_with_line_corner_curved_rectangle(info_g ,"Carrier", new SolidBrush(Color.FromArgb(190, 190, 190)), fnt_default, new Pen(Color.FromArgb(144, 144, 144), 4), new Point(30 * 2, 225 * 2), new Point(449 * 2, 428 * 2), 20, new Point(0, 0));

                    String[] pulse_mode_name = get_Pulse_Name(Video_Generate_Values.pulse_mode);
                    if (Video_Generate_Values.pulse_mode == Pulse_Mode.Async || Video_Generate_Values.pulse_mode == Pulse_Mode.Asyn_THI)
                    {
                        String carrier_freq_str = String.Format("{0:f0}", Video_Generate_Values.carrier_freq_data.base_freq);
                        SizeF freq_str_size = info_g.MeasureString(carrier_freq_str, fnt_num);
                        SizeF hz_str_size = info_g.MeasureString("Hz", fnt_unit);

                        int total_width = (int)(freq_str_size.Width + hz_str_size.Width);

                        info_g.DrawString(carrier_freq_str, fnt_num, new SolidBrush(Color.White), image_width / 2 - total_width / 2, 250 * 2);
                        info_g.DrawString("Hz", fnt_unit, new SolidBrush(Color.White), image_width / 2 - total_width / 2 + freq_str_size.Width, 292 * 2);

                        center_text_with_line_corner_curved_rectangle(info_g, "Async Mode", new SolidBrush(Color.FromArgb(190, 190, 190)), fnt_syncmode, new Pen(Color.FromArgb(144, 144, 144), 4), new Point(47 * 2, 359 * 2), new Point(431 * 2, 410 * 2), 20, new Point(0, 5));
                    }
                    else
                    {
                        String carrier_freq_str = pulse_mode_name[0];
                        SizeF freq_str_size = info_g.MeasureString(carrier_freq_str, fnt_num);
                        info_g.DrawString(carrier_freq_str, fnt_num, new SolidBrush(Color.White), 920 / 2 - freq_str_size.Width / 2, 250 * 2);
                        center_text_with_filled_corner_curved_rectangle(info_g, "Sync Mode", new SolidBrush(Color.FromArgb(52, 52, 52)), fnt_syncmode, new SolidBrush(Color.White), new Point(47 * 2, 359 * 2), new Point(431 * 2, 410 * 2), 20, new Point(0, 5));
                    }


                    title_str_with_line_corner_curved_rectangle(info_g, "Output", new SolidBrush(Color.FromArgb(190, 190, 190)), fnt_default , new Pen(Color.FromArgb(144, 144, 144),4), new Point(30 * 2, 474 * 2), new Point( 449 * 2, 731 * 2), 20, new Point(0,0));
                    
                    if(Video_Generate_Values.pulse_mode == Pulse_Mode.Async || Video_Generate_Values.pulse_mode == Pulse_Mode.Asyn_THI)
                        center_text_with_line_corner_curved_rectangle(info_g, "Freq", new SolidBrush(Color.White), fnt_default, new Pen(Color.White, 4), new Point(47 * 2, 507 * 2), new Point(182 * 2, 602 * 2), 10, new Point(0, 5));
                    else 
                        center_text_with_filled_corner_curved_rectangle(info_g, "Freq", new SolidBrush(Color.FromArgb(52,52,52)), fnt_default, new SolidBrush(Color.White), new Point(47 * 2, 507 * 2), new Point(182 * 2, 602 * 2), 10, new Point(0, 5));
                    
                    if(!(Video_Generate_Values.pulse_mode == Pulse_Mode.Async || Video_Generate_Values.pulse_mode == Pulse_Mode.Asyn_THI)){
                        int connect_sync_box_size = 12;
                        for(int r = 0; r < 9; r++)
                        {
                            // 560 + 49 - 4 NO.5
                            info_g.FillRectangle(new SolidBrush(Color.White), 47 * 2 + 67*2 - connect_sync_box_size/2, 410 * 2 - connect_sync_box_size/2 + connect_sync_box_size*2 * r, connect_sync_box_size, connect_sync_box_size);
                        }
                    }

                    double sine_freq = Video_Generate_Values.sine_angle_freq / Math.PI / 2;
                    if (!final_show)
                    {
                        int base_pos = 620;
                        String sine_freq_str = String.Format("{0:f1}", sine_freq);
                        SizeF freq_str_size = info_g.MeasureString(sine_freq_str, fnt_num);
                        SizeF hz_str_size = info_g.MeasureString("Hz", fnt_unit);

                        int total_width = (int)(freq_str_size.Width + hz_str_size.Width);

                        info_g.DrawString(sine_freq_str, fnt_num, new SolidBrush(Color.White), base_pos - total_width / 2, 505 * 2);
                        info_g.DrawString("Hz", fnt_unit, new SolidBrush(Color.White), base_pos - total_width / 2 + freq_str_size.Width, 545 * 2);
                    }

                    center_text_with_line_corner_curved_rectangle(info_g, "Volt", new SolidBrush(Color.White), fnt_default, new Pen(Color.White, 4), new Point(47 * 2, 618 * 2), new Point(182 * 2, 713 * 2), 10, new Point(0, 5));
                    if (!final_show)
                    {
                        int base_pos = 620;

                        double voltage = get_wave_form_voltage_rage(sound_name);
                        String sine_freq_str = String.Format("{0:f1}", (voltage + pre_voltage) / 2.0);
                        SizeF freq_str_size = info_g.MeasureString(sine_freq_str, fnt_num);
                        SizeF hz_str_size = info_g.MeasureString("%", fnt_unit);

                        int total_width = (int)(freq_str_size.Width + hz_str_size.Width);

                        info_g.DrawString(sine_freq_str, fnt_num, new SolidBrush(Color.White), base_pos - total_width / 2, 620 * 2);
                        info_g.DrawString("%", fnt_unit, new SolidBrush(Color.White), base_pos - total_width / 2 + freq_str_size.Width, 1200 + 115);

                        pre_voltage = voltage;
                    }
                    //line_corner_curved_rectangle(g, new Pen(rikkou), 30 * 2, 45 * 2, 449 * 2, 163 * 2, 20);

                    final_g.DrawImage(background,0,0);

                    if (is_Free_Running())
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

                    if (final_show || first_show)
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
