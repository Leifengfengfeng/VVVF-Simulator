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

        private static void filled_corner_curved_rectangle(Graphics g, Brush br , int start_x, int start_y, int end_x, int end_y , int round)
        {
            g.FillRectangle(br, (int)Math.Round(start_x + round / 2.0), start_y, (int)Math.Round(end_x - round / 2.0), end_y);
            g.FillRectangle(br, start_x, (int)Math.Round(start_y + round / 2.0), (int)Math.Round(start_x + round / 2.0), (int)Math.Round(end_y - round / 2.0));
            g.FillRectangle(br, end_x, (int)Math.Round(start_y + round / 2.0), (int)Math.Round(end_x - round / 2.0), (int)Math.Round(end_y - round / 2.0));

        }

        public static void generate_status_taroimo_like_video(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

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

                    Color rikkou;
                    if (is_Free_Running())
                    {
                        rikkou = Color.FromArgb(153, 204, 0);
                    }
                    else if (!is_Braking())
                    {
                        rikkou = Color.FromArgb(0, 204, 255);
                    }
                    else
                    {
                        rikkou = Color.FromArgb(153, 255, 0);
                    }

                    filled_corner_curved_rectangle(g, new SolidBrush(rikkou), 30, 45, 449, 163, 20);

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

    }
}
