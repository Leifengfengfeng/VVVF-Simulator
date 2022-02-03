using NAudio.CoreAudioApi;
using NAudio.Wave;
using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_sound_definition;
using static VVVF_Generator_Porting.vvvf_wave_control;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_Common
    {
        static readonly double M_2PI = Math.PI * 2;
        static readonly double M_PI = Math.PI;
        static readonly double M_PI_2 = Math.PI / 2.0;
        static readonly double M_2_PI = 2.0 / Math.PI;
        static readonly double M_1_PI = 1.0 / Math.PI;
        static readonly double M_1_2PI = 1.0 / (2.0 * Math.PI);

        static double count = 0;
        static readonly int div_freq = 192 * 1000;

        public static class Video_Generate_Values
        {
            public static Pulse_Mode pulse_mode = Pulse_Mode.P_1;
            public static double sine_amplitude = 0.0;
            public static Carrier_Freq carrier_freq_data;
            public static double dipolar = -1;
        }

        // giajeoigae
        private static Boolean check_for_freq_change()
        {
            count++;
            if (count % 60 == 0 && is_Do_Freq_Change() && get_Sine_Angle_Freq() * M_1_2PI == get_Control_Frequency())
            {
                double sin_new_angle_freq = get_Sine_Angle_Freq();

                if (!is_Braking()) sin_new_angle_freq += Math.PI / 400;
                else sin_new_angle_freq -= Math.PI / 400;

                double amp = get_Sine_Angle_Freq() / sin_new_angle_freq;

                set_Sine_Angle_Freq(sin_new_angle_freq);
                multi_Sine_Time(amp);
            }

            if (get_Temp_Count() == 0)
            {
                if (get_Sine_Angle_Freq() * M_1_2PI > 90 && !is_Braking() && is_Do_Freq_Change())
                {
                    set_Do_Freq_Change(false);
                    set_Mascon_Off(true);
                    count = 0;
                }
                else if (count / div_freq > 2 && !is_Do_Freq_Change())
                {
                    set_Do_Freq_Change(true);
                    set_Mascon_Off(false);
                    set_Braking(true);
                    set_Temp_Count(get_Temp_Count() + 1);
                }
            }
            /*
            else if (temp_count == 1)
            {
                if (sin_angle_freq / 2 / Math.PI < 20 && brake && do_frequency_change)
                {
                    do_frequency_change = false;
                    mascon_off = true;
                    count = 0;
                }
                else if (count / div_freq > 1 && !do_frequency_change)
                {
                    do_frequency_change = true;
                    mascon_off = false;
                    brake = false;
                    temp_count++;
                }
            }
            else if (temp_count == 2)
            {
                if (sin_angle_freq / 2 / Math.PI > 45 && !brake && do_frequency_change)
                {
                    do_frequency_change = false;
                    mascon_off = true;

                    count = 0;
                }
                else if (count / div_freq > 1 && !do_frequency_change)
                {
                    do_frequency_change = true;
                    mascon_off = false;
                    brake = true;
                    temp_count++;
                }
            }
            */
            else
            {
                if (get_Sine_Angle_Freq() * M_1_2PI < 0 && is_Braking() && is_Do_Freq_Change()) return false;
            }



            if (!is_Mascon_Off())
            {
                if (!is_Free_Running())
                    set_Control_Frequency(get_Sine_Angle_Freq() * M_1_2PI);
                else
                {
                    add_Control_Frequency((Math.PI * 2) / (double)get_Mascon_Off_Div());

                    if (get_Sine_Angle_Freq() * M_1_2PI < get_Control_Frequency())
                    {
                        set_Control_Frequency(get_Sine_Angle_Freq() * M_1_2PI);
                        set_Free_Running(false);
                    }
                    else
                    {
                        set_Free_Running(true);
                    }
                }
            }
            else
            {
                add_Control_Frequency(- (Math.PI * 2) / (double)get_Mascon_Off_Div());
                if (get_Control_Frequency() < 0) set_Control_Frequency(0);
                set_Free_Running(true);
            }

            return true;
        }

        public static void generate_sound(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            Int32 sound_block_count = 0;
            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);


            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".wav";

            BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Create));

            //WAV FORMAT DATA
            writer.Write(0x46464952); // RIFF
            writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); //CHUNK SIZE
            writer.Write(0x45564157); //WAVE
            writer.Write(0x20746D66); //fmt 
            writer.Write(16);
            writer.Write(new byte[] { 0x01, 0x00 }); // LINEAR PCM
            writer.Write(new byte[] { 0x01, 0x00 }); // MONORAL
            writer.Write(div_freq); // SAMPLING FREQ
            writer.Write(div_freq); // BYTES IN 1SEC
            writer.Write(new byte[] { 0x01, 0x00 }); // Block Size = 1
            writer.Write(new byte[] { 0x08, 0x00 }); // 1 Sample bits
            writer.Write(0x61746164);
            writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); //WAVE SIZE

            bool loop = true;

            while (loop)
            {
                add_Sine_Time(1.00 / div_freq);
                add_Saw_Time(1.00 / div_freq);

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

                for (int i = 0; i < 1; i++)
                {
                    double pwm_value = wv_U.pwm_value - wv_V.pwm_value;
                    byte sound_byte = 0x80;
                    if (pwm_value == 2) sound_byte = 0xB0;
                    else if (pwm_value == 1) sound_byte = 0x98;
                    else if (pwm_value == -1) sound_byte = 0x68;
                    else if (pwm_value == -2) sound_byte = 0x50;
                    writer.Write(sound_byte);
                }
                sound_block_count++;

                loop = check_for_freq_change();

            }



            writer.Seek(4, SeekOrigin.Begin);
            writer.Write(sound_block_count + 36);

            writer.Seek(40, SeekOrigin.Begin);
            writer.Write(sound_block_count);

            writer.Close();
        }

        public static void generate_wave_U_V(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            bool temp = true;
            Int32 sound_block_count = 0;

            int image_width = 2000;
            int image_height = 500;

            int movie_div = 3000;
            int wave_height = 100;
            int calculate_div = 10;

            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));


            if (!vr.IsOpened())
            {
                return;
            }

            Boolean START_F192_WAIT = true;
            if (START_F192_WAIT)
            {
                for (int i = 0; i < 192; i++)
                {
                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                    g.DrawLine(new Pen(Color.Gray), 0, image_height / 2, image_width, image_height / 2);
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();
                }
            }

            Boolean loop = true;
            while (loop)
            {


                if (sound_block_count % movie_div == 0 && temp)
                {
                    set_Sine_Time(0);
                    set_Saw_Time(0);

                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                    g.DrawLine(new Pen(Color.Gray), 0, image_height / 2, image_width, image_height / 2);

                    for (int i = 0; i < image_width * calculate_div; i++)
                    {
                        Wave_Values[] values = new Wave_Values[4];

                        for(int j = 0; j < 2; j++)
                        {
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

                            if (j == 0)
                            {
                                add_Saw_Time(Math.PI / (120000.0 * calculate_div));
                                add_Sine_Time(Math.PI / (120000.0 * calculate_div));
                            }

                            values[j * 2] = wv_U;
                            values[j * 2 + 1] = wv_V;
                        }


                        int curr_val = (int)(-(values[0].pwm_value - values[1].pwm_value) * wave_height + image_height / 2.0);
                        int next_val = (int)(-(values[2].pwm_value - values[3].pwm_value) * wave_height + image_height / 2.0);
                        g.DrawLine(new Pen(Color.Black), (int)(i / (double)calculate_div), curr_val, (int)(((curr_val != next_val) ? i : i + 1) / (double)calculate_div), next_val);
                    }

                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();

                    temp = false;
                }
                else if (sound_block_count % movie_div != 0)
                {
                    temp = true;
                }

                sound_block_count++;

                loop = check_for_freq_change();

            }

            Boolean END_F64_WAIT = true;
            if (END_F64_WAIT)
            {
                for (int i = 0; i < 64; i++)
                {
                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                    g.DrawLine(new Pen(Color.Gray), 0, image_height / 2, image_width, image_height / 2);
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();
                }
            }

            vr.Release();
            vr.Dispose();
        }

        public static void generate_wave_UVW(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            bool temp = true;
            Int32 sound_block_count = 0;

            int image_width = 1500;
            int image_height = 1000;
            int movie_div = 3000;

            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));


            if (!vr.IsOpened())
            {
                return;
            }

            Boolean START_F192_WAIT = true;
            if (START_F192_WAIT)
            {
                for (int i = 0; i < 192; i++)
                {
                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();
                }
            }

            Boolean loop = true;
            while (loop)
            {


                if (sound_block_count % movie_div == 0 && temp)
                {
                    set_Saw_Time(0);
                    set_Sine_Time(0);

                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);

                    int[] points_U = new int[image_width];
                    int[] points_V = new int[image_width];
                    int[] points_W = new int[image_width];


                    for (int i = 0; i < image_width; i++)
                    {
                        add_Sine_Time(Math.PI / 25000.0);
                        add_Saw_Time(Math.PI / 25000.0);

                        Control_Values cv_U = new Control_Values
                        {
                            brake = is_Braking(),
                            mascon_on = !is_Mascon_Off(),
                            free_run = is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 0,
                            wave_stat = get_Control_Frequency()
                        };
                        Wave_Values wv_U = get_Calculated_Value(sound_name, cv_U);
                        points_U[i] = wv_U.pwm_value;

                        Control_Values cv_V = new Control_Values
                        {
                            brake = is_Braking(),
                            mascon_on = !is_Mascon_Off(),
                            free_run = is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 1,
                            wave_stat = get_Control_Frequency()
                        };
                        Wave_Values wv_V = get_Calculated_Value(sound_name, cv_V);
                        points_V[i] = wv_V.pwm_value;

                        Control_Values cv_W = new Control_Values
                        {
                            brake = is_Braking(),
                            mascon_on = !is_Mascon_Off(),
                            free_run = is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 2,
                            wave_stat = get_Control_Frequency()
                        };
                        Wave_Values wv_W = get_Calculated_Value(sound_name, cv_W);
                        points_W[i] = wv_W.pwm_value;

                    }

                    for (int i = 0; i < image_width - 1; i++)
                    {

                        for (int ix = 0; ix < 3; ix++)
                        {
                            int curr_val = 0;
                            int next_val = 0;
                            if (ix == 0)
                            {
                                curr_val = points_U[i];
                                next_val = points_U[i + 1];
                            }
                            else if (ix == 1)
                            {
                                curr_val = points_V[i];
                                next_val = points_V[i + 1];
                            }
                            else
                            {
                                curr_val = points_W[i];
                                next_val = points_W[i + 1];
                            }

                            curr_val *= -100;
                            next_val *= -100;

                            curr_val += 300 * (ix + 1);
                            next_val += 300 * (ix + 1);

                            g.DrawLine(new Pen(Color.Black), i, curr_val, ((curr_val != next_val) ? i : i + 1), next_val);
                        }


                        //g.DrawLine(new Pen(Color.Gray), i, (int)(points_U[i] * wave_height + image_height / 2.0), i + 1, (int)(points_U[i+1] * wave_height + image_height / 2.0));
                        //g.DrawLine(new Pen(Color.Gray), i, (int)(points_V[i] * wave_height + image_height / 2.0), i + 1, (int)(points_V[i + 1] * wave_height + image_height / 2.0));
                    }


                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();

                    temp = false;
                }
                else if (sound_block_count % movie_div != 0)
                {
                    temp = true;
                }

                sound_block_count++;

                loop = check_for_freq_change();

            }

            Boolean END_F64_WAIT = true;
            if (END_F64_WAIT)
            {
                for (int i = 0; i < 64; i++)
                {
                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();
                }
            }

            vr.Release();
            vr.Dispose();
        }

        public static void generate_wave_hexagon_explain(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            int movie_div = 3000;

            int image_width = 1300;
            int image_height = 500;

            int pwm_image_width = 750;
            int pwm_image_height = 500;

            int hexagon_image_size = 1000;

            int hex_div_seed = 10000;
            int hex_div = 6 * hex_div_seed;

            Boolean draw_zero_vector_circle = true;
            while (true)
            {
                try
                {
                    Console.WriteLine("Draw a circle which shows zero vector? ( true / false )");
                    draw_zero_vector_circle = Boolean.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid value.");
                }
            }

            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));
            if (!vr.IsOpened())
            {
                return;
            }

            Boolean START_F192_WAIT = false;
            if (START_F192_WAIT)
            {
                Bitmap free_image = new(image_width, image_height);
                Graphics free_g = Graphics.FromImage(free_image);
                free_g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                MemoryStream free_ms = new MemoryStream();
                free_image.Save(free_ms, ImageFormat.Png);
                byte[] free_img = free_ms.GetBuffer();
                Mat free_mat = OpenCvSharp.Mat.FromImageData(free_img);

                for (int i = 0; i < 192; i++)
                {
                    vr.Write(free_mat);
                }
                free_g.Dispose();
                free_image.Dispose();
            }

            set_Sine_Time(0);
            set_Saw_Time(0);

            while (true)
            {
                try
                {
                    Console.WriteLine("Enter the Freq.");

                    double freq = Double.Parse(Console.ReadLine());
                    set_Control_Frequency(freq);
                    set_Sine_Angle_Freq(freq * M_2PI);

                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid value.");
                }
            }

            Bitmap PWM_wave_image = new(pwm_image_width, pwm_image_height);
            Graphics PWM_wave_g = Graphics.FromImage(PWM_wave_image);
            PWM_wave_g.FillRectangle(new SolidBrush(Color.White), 0, 0, pwm_image_width, pwm_image_height);

            Bitmap whole_image = new(image_width, image_height);
            Graphics whole_g = Graphics.FromImage(whole_image);
            whole_g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);

            Bitmap hexagon_image = new(hexagon_image_size, hexagon_image_size);
            Graphics hexagon_g = Graphics.FromImage(hexagon_image);
            hexagon_g.FillRectangle(new SolidBrush(Color.White), 0, 0, hexagon_image_size, hexagon_image_size);

            Boolean drawn_circle = false;
            Bitmap zero_circle_image = new(hexagon_image_size, hexagon_image_size);
            Graphics zero_circle_g = Graphics.FromImage(zero_circle_image);

            int[] points_U = new int[hex_div];
            int[] points_V = new int[hex_div];
            int[] points_W = new int[hex_div];

            double[] x_min_max = new double[] { 500, 0 };
            double[] hexagon_coordinate = new double[] { 100, 500 };

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
                points_U[i] = wv_U.pwm_value;

                Control_Values cv_V = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_V = get_Calculated_Value(sound_name, cv_V);
                points_V[i] = wv_V.pwm_value;

                Control_Values cv_W = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 2,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_W = get_Calculated_Value(sound_name, cv_W);
                points_W[i] = wv_W.pwm_value;

                double move_x = 0;
                double move_y = 0;
                if (!(wv_U.pwm_value == wv_V.pwm_value && wv_V.pwm_value == wv_W.pwm_value))
                {
                    move_x = -0.5 * wv_W.pwm_value - 0.5 * wv_V.pwm_value + wv_U.pwm_value;
                    move_y = -0.866025403784438646763 * wv_W.pwm_value + 0.866025403784438646763 * wv_V.pwm_value;
                }
                double int_move_x = 200 * move_x / (double)hex_div_seed;
                double int_move_y = 200 * move_y / (double)hex_div_seed;
                hexagon_coordinate[0] = hexagon_coordinate[0] + int_move_x;
                hexagon_coordinate[1] = hexagon_coordinate[1] + int_move_y;

                if (x_min_max[0] > hexagon_coordinate[0]) x_min_max[0] = hexagon_coordinate[0];
                if (x_min_max[1] < hexagon_coordinate[0]) x_min_max[1] = hexagon_coordinate[0];

            }

            hexagon_coordinate = new double[] { 100, 500 };
            double moved_x = (1000 - (x_min_max[1] - x_min_max[0])) / 2.0 - 100;

            int jump_add = hex_div / pwm_image_width;
            for (int i = 0; i < pwm_image_width - 1; i++)
            {
                for (int ix = 0; ix < 3; ix++)
                {
                    int curr_val = 0;
                    int next_val = 0;
                    if (ix == 0)
                    {
                        curr_val = points_U[i * jump_add];
                        next_val = points_U[(i + 1) * jump_add];
                    }
                    else if (ix == 1)
                    {
                        curr_val = points_V[i * jump_add];
                        next_val = points_V[(i + 1) * jump_add];
                    }
                    else
                    {
                        curr_val = points_W[i * jump_add];
                        next_val = points_W[(i + 1) * jump_add];
                    }

                    curr_val *= -50;
                    next_val *= -50;

                    curr_val += 150 * (ix + 1);
                    next_val += 150 * (ix + 1);

                    PWM_wave_g.DrawLine(new Pen(Color.Black), i, curr_val, ((curr_val != next_val) ? i : i + 1), next_val);
                }
            }

            whole_g.DrawImage(PWM_wave_image, 0, 0);

            bool only_wave_show = true;
            if (only_wave_show)
            {
                MemoryStream ms = new MemoryStream();
                whole_image.Save(ms, ImageFormat.Png);
                byte[] img = ms.GetBuffer();
                Mat mat = OpenCvSharp.Mat.FromImageData(img);

                Cv2.ImShow("Wave Form View", mat);


                for (int i = 0; i < 60; i++)
                {
                    vr.Write(mat);
                    Cv2.WaitKey(5);
                }
            }

            Font text_font = new Font(
                new FontFamily("Fugaz One"),
                40,
                FontStyle.Bold,
                GraphicsUnit.Pixel);

            for (int i = 0; i < hex_div; i++)
            {
                whole_g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                whole_g.DrawImage(PWM_wave_image, 0, 0);
                whole_g.DrawLine(new Pen(Color.Red), (int)Math.Round((double)i / (double)hex_div * (double)pwm_image_width), 0, (int)Math.Round((double)i / (double)hex_div * (double)pwm_image_width), pwm_image_height);

                int pwm_U = points_U[i];
                int pwm_V = points_V[i];
                int pwm_W = points_W[i];

                whole_g.DrawString(pwm_U.ToString(), text_font, (pwm_U > 0) ? new SolidBrush(Color.Blue) : new SolidBrush(Color.Red), pwm_image_width + 5, 75); ;
                whole_g.DrawString(pwm_V.ToString(), text_font, (pwm_V > 0) ? new SolidBrush(Color.Blue) : new SolidBrush(Color.Red), pwm_image_width + 5, 225);
                whole_g.DrawString(pwm_W.ToString(), text_font, (pwm_W > 0) ? new SolidBrush(Color.Blue) : new SolidBrush(Color.Red), pwm_image_width + 5, 375);

                double move_x = 0;
                double move_y = 0;
                if (!(pwm_U == pwm_V && pwm_V == pwm_W))
                {
                    move_x = -0.5 * pwm_W - 0.5 * pwm_V + pwm_U;
                    move_y = -0.866025403784438646763 * pwm_W + 0.866025403784438646763 * pwm_V;
                }

                double int_move_x = 200 * move_x / (double)hex_div_seed;
                double int_move_y = 200 * move_y / (double)hex_div_seed;

                hexagon_g.DrawLine(new Pen(Color.Black),
                    (int)(hexagon_coordinate[0] + moved_x),
                    (int)(hexagon_coordinate[1]),
                    (int)(hexagon_coordinate[0] + moved_x + int_move_x),
                    (int)(hexagon_coordinate[1] + int_move_y)
                );

                if (move_x == 0 && move_y == 0 && draw_zero_vector_circle)
                {
                    if (!drawn_circle)
                    {
                        drawn_circle = true;
                        zero_circle_g.FillEllipse(new SolidBrush(Color.White),
                            (int)(hexagon_coordinate[0] - 2 + moved_x),
                            (int)hexagon_coordinate[1] - 2,
                            4,
                            4
                        );
                        zero_circle_g.DrawEllipse(new Pen(Color.Black),
                            (int)(hexagon_coordinate[0] - 2 + moved_x),
                            (int)hexagon_coordinate[1] - 2,
                            4,
                            4
                        );
                    }

                }
                else
                    drawn_circle = false;

                Bitmap hexagon_image_with_dot = new(hexagon_image_size, hexagon_image_size);
                Graphics hexagon_g_with_dot = Graphics.FromImage(hexagon_image_with_dot);
                //hexagon_g_with_dot.FillRectangle(new SolidBrush(Color.White), 0, 0, hexagon_image_size, hexagon_image_size);
                hexagon_g_with_dot.FillRectangle(new SolidBrush(Color.Red),
                    (int)(hexagon_coordinate[0] + moved_x - 5),
                    (int)(hexagon_coordinate[1] - 5),
                    (int)(10),
                    (int)(10)
                );

                hexagon_coordinate[0] = hexagon_coordinate[0] + int_move_x;
                hexagon_coordinate[1] = hexagon_coordinate[1] + int_move_y;

                if (i % 10 == 0 || i + 1 == hex_div)
                {
                    Bitmap resized_hexagon = new Bitmap(450, 450);
                    Graphics resized_hexagon_g = Graphics.FromImage(resized_hexagon);
                    resized_hexagon_g.FillRectangle(new SolidBrush(Color.White), 0, 0, 450, 450);
                    resized_hexagon_g.DrawImage(new Bitmap(hexagon_image, 450, 450), 0, 0);
                    resized_hexagon_g.DrawImage(new Bitmap(hexagon_image_with_dot, 450, 450), 0, 0);
                    resized_hexagon_g.DrawImage(new Bitmap(zero_circle_image, 450, 450), 0, 0);

                    whole_g.DrawImage(resized_hexagon, 820, 25);

                    MemoryStream ms = new MemoryStream();
                    whole_image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);


                    for (int frame = 0; frame < 1; frame++)
                    {
                        vr.Write(mat);
                        Cv2.WaitKey(1);
                    }

                    resized_hexagon_g.Dispose();
                    resized_hexagon.Dispose();
                }

                hexagon_g_with_dot.Dispose();
                hexagon_image_with_dot.Dispose();

            }



            Boolean END_F64_WAIT = true;
            if (END_F64_WAIT)
            {
                MemoryStream free_ms = new MemoryStream();
                whole_image.Save(free_ms, ImageFormat.Png);
                byte[] free_img = free_ms.GetBuffer();
                Mat free_mat = OpenCvSharp.Mat.FromImageData(free_img);

                for (int i = 0; i < 64; i++)
                {
                    vr.Write(free_mat);
                }
            }

            PWM_wave_g.Dispose();
            PWM_wave_image.Dispose();
            whole_g.Dispose();
            whole_image.Dispose();
            hexagon_g.Dispose();
            hexagon_image.Dispose();
            zero_circle_g.Dispose();
            zero_circle_image.Dispose();

            vr.Release();
            vr.Dispose();
        }
        public static void generate_wave_hexagon(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            Boolean draw_zero_vector_circle = true;
            while (true)
            {
                try
                {
                    Console.WriteLine("Draw a circle which shows zero vector? ( true / false )");
                    draw_zero_vector_circle = Boolean.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid value.");
                }
            }

            bool temp = true;
            Int32 sound_block_count = 0;


            int image_width = 1000;
            int image_height = 1000;

            int hex_div_seed = 10000;
            int hex_div = 6 * hex_div_seed;

            // around 60fps
            int movie_div = 3000;

            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));


            if (!vr.IsOpened())
            {
                return;
            }

            Boolean START_F192_WAIT = false;
            if (START_F192_WAIT)
            {
                Bitmap image = new(image_width, image_height);
                Graphics g = Graphics.FromImage(image);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                byte[] img = ms.GetBuffer();
                Mat mat = OpenCvSharp.Mat.FromImageData(img);

                Cv2.ImShow("Wave Form View", mat);
                Cv2.WaitKey(1);
                for (int i = 0; i < 192; i++)
                {
                    vr.Write(mat);
                }
                g.Dispose();
                image.Dispose();
            }

            Boolean loop = true;
            while (loop)
            {


                if (sound_block_count % movie_div == 0 && temp)
                {
                    set_Sine_Time(0);
                    set_Saw_Time(0);

                    Bitmap hexagon_image = new(image_width, image_height);
                    Graphics hexagon_g = Graphics.FromImage(hexagon_image);

                    Boolean drawn_circle = false;
                    Bitmap zero_circle_image = new(image_width, image_height);
                    Graphics zero_circle_g = Graphics.FromImage(zero_circle_image);

                    double[] hexagon_coordinate = new double[] { 100, 500 };
                    double[] x_min_max = new double[2] { 500, 0 };

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


                        hexagon_g.DrawLine(new Pen(Color.Black),
                            (int)hexagon_coordinate[0],
                            (int)hexagon_coordinate[1],
                            (int)(hexagon_coordinate[0] + int_move_x),
                            (int)(hexagon_coordinate[1] + int_move_y)
                        );

                        if (move_x == 0 && move_y == 0 && draw_zero_vector_circle)
                        {
                            if (!drawn_circle)
                            {
                                drawn_circle = true;
                                zero_circle_g.FillEllipse(new SolidBrush(Color.White),
                                    (int)hexagon_coordinate[0] - 2,
                                    (int)hexagon_coordinate[1] - 2,
                                    4,
                                    4
                                );
                                zero_circle_g.DrawEllipse(new Pen(Color.Black),
                                    (int)hexagon_coordinate[0] - 2,
                                    (int)hexagon_coordinate[1] - 2,
                                    4,
                                    4
                                );
                            }

                        }
                        else
                            drawn_circle = false;

                        hexagon_coordinate[0] = hexagon_coordinate[0] + int_move_x;
                        hexagon_coordinate[1] = hexagon_coordinate[1] + int_move_y;

                        if (x_min_max[0] > hexagon_coordinate[0]) x_min_max[0] = hexagon_coordinate[0];
                        if (x_min_max[1] < hexagon_coordinate[0]) x_min_max[1] = hexagon_coordinate[0];

                    }

                    Bitmap final_image = new(image_width, image_height);
                    Graphics final_g = Graphics.FromImage(final_image);
                    final_g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);

                    double moved_x = (1000 - (x_min_max[1] - x_min_max[0])) / 2.0;
                    final_g.DrawImage(hexagon_image, (int)moved_x - 100, 0);
                    final_g.DrawImage(zero_circle_image, (int)moved_x - 100, 0);

                    MemoryStream ms = new MemoryStream();
                    final_image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View", mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    hexagon_g.Dispose();
                    final_g.Dispose();
                    zero_circle_g.Dispose();

                    hexagon_image.Dispose();
                    final_image.Dispose();
                    zero_circle_image.Dispose();

                    temp = false;
                }
                else if (sound_block_count % movie_div != 0)
                {
                    temp = true;
                }

                sound_block_count++;

                loop = check_for_freq_change();

            }

            Boolean END_F64_WAIT = true;
            if (END_F64_WAIT)
            {
                Bitmap image = new(image_width, image_height);
                Graphics g = Graphics.FromImage(image);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                byte[] img = ms.GetBuffer();
                Mat mat = OpenCvSharp.Mat.FromImageData(img);

                Cv2.ImShow("Wave Form View", mat);
                Cv2.WaitKey(1);
                for (int i = 0; i < 64; i++)
                {
                    vr.Write(mat);
                }
                g.Dispose();
                image.Dispose();
            }

            vr.Release();
            vr.Dispose();
        }

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
                    double sine_freq = get_Sine_Angle_Freq() / Math.PI / 2;
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
        private static int realtime_sound_calculate(BufferedWaveProvider provider, VVVF_Sound_Names sound_name)
        {
            while (true)
            {

                double pre_sin_angle_freq = get_Sine_Angle_Freq();
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey();
                    ConsoleKey key = keyinfo.Key;

                    if (key.Equals(ConsoleKey.B))
                    {
                        set_Braking(!is_Braking());;
                        Console.WriteLine("\r\n Brake : " + is_Braking());

                    }
                    else if (key.Equals(ConsoleKey.W) || key.Equals(ConsoleKey.S) || key.Equals(ConsoleKey.X))
                    {
                        double change_amo = Math.PI;

                        if (key.Equals(ConsoleKey.S))
                            change_amo /= 2.0;
                        else if (key.Equals(ConsoleKey.X))
                            change_amo /= 4.0;

                        if (is_Braking())
                            change_amo = -change_amo;

                        double sin_new_angle_freq = get_Sine_Angle_Freq();
                        sin_new_angle_freq += change_amo;

                        double amp = get_Sine_Angle_Freq() / sin_new_angle_freq;
                        if (sin_new_angle_freq < 0) sin_new_angle_freq = 0;

                        set_Sine_Angle_Freq(sin_new_angle_freq);
                        multi_Sine_Time(amp);

                        if (!is_Mascon_Off())
                            set_Control_Frequency(get_Sine_Angle_Freq() / (M_2PI));

                        Console.WriteLine("\r\n CurrentFreq : " + get_Control_Frequency());
                    }
                    else if (key.Equals(ConsoleKey.N))
                    {
                        toggle_Mascon_Off();
                        Console.WriteLine("\r\n Mascon : " + !is_Mascon_Off());
                    }
                    else if (key.Equals(ConsoleKey.Enter)) return 0;
                    else if (key.Equals(ConsoleKey.R)) return 1;
                }

                if (!is_Mascon_Off())
                {
                    if (!is_Free_Running())
                        set_Control_Frequency(get_Sine_Angle_Freq() / M_2PI);
                    else
                    {
                        add_Control_Frequency(M_2PI / (get_Mascon_Off_Div() / 12.0));

                        if(get_Sine_Angle_Freq() < get_Control_Frequency() * M_2PI)
                        {
                            set_Free_Running(false);
                            set_Control_Frequency(get_Sine_Angle_Freq() * M_1_2PI);
                        }
                        else
                        {
                            set_Free_Running(true);
                        }
                    }
                }
                else
                {
                    add_Control_Frequency(- (Math.PI * 2) / (double)(get_Mascon_Off_Div() / 12.0));
                    if (get_Control_Frequency() < 0) set_Control_Frequency(0);
                    set_Free_Running(true);
                }

                byte[] add = new byte[20];

                for (int i = 0; i < 20; i++)
                {
                    add_Sine_Time(1.0 / 192000.0);
                    add_Saw_Time(1.0 / 192000.0);

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

                    double pwm_value = wv_U.pwm_value - wv_V.pwm_value;
                    byte sound_byte = 0x80;

                    if (pwm_value == 2) sound_byte = 0xB0;
                    else if (pwm_value == 1) sound_byte = 0x98;
                    else if (pwm_value == -1) sound_byte = 0x68;
                    else if (pwm_value == -2) sound_byte = 0x50;

                    /*
                    if (voltage_stat == 0) d = 0x80;
                    else if (voltage_stat > 0) d = 0xC0;
                    else d = 0x40;
                    */
                    add[i] = sound_byte;
                }


                int bufsize = 20;

                provider.AddSamples(add, 0, bufsize);
                while (provider.BufferedBytes > 16000) ;
            }
        }
        public static void realtime_sound()
        {
            while (true)
            {
                VVVF_Sound_Names sound_name = get_Choosed_Sound();
                reset_control_variables();
                reset_all_variables();

                var bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(192000, 8, 1));
                var mmDevice = new MMDeviceEnumerator()
                    .GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                IWavePlayer wavPlayer = new WasapiOut(mmDevice, AudioClientShareMode.Shared, false, 50);



                wavPlayer.Init(bufferedWaveProvider);

                wavPlayer.Play();

                Console.WriteLine("Press R to Select New Sound...");
                Console.WriteLine("Press ENTER to exit...");

                int stat = realtime_sound_calculate(bufferedWaveProvider, sound_name);

                wavPlayer.Stop();

                if (stat == 0) break;
            }


        }
    }
}
