using NAudio.CoreAudioApi;
using NAudio.Wave;
using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_wave;

namespace VVVF_Generator_Porting
{
    internal class Program
    {
        static double M_2PI   = Math.PI * 2;
        static double M_PI    = Math.PI;
        static double M_PI_2  = Math.PI / 2.0;
        static double M_2_PI  = 2.0 / Math.PI;
        static double M_1_PI  = 1.0 / Math.PI;
        static double M_1_2PI = 1.0 / (2.0 * Math.PI);

        static double count = 0;
        static int div_freq = 192 * 1000;
        public static int mascon_off_div = 18000;


        public static Wave_Values get_Calculated_Value(VVVF_Sound_Names name, Control_Values cv)
        {
            //JR EAST
            if (name == VVVF_Sound_Names.SOUND_JRE_209_MITSUBISHI_GTO_3_LEVEL) return calculate_jre_209_mitsubishi_gto_3_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRE_E231_MITSUBISHI_IGBT_3_LEVEL) return calculate_jre_e231_mitsubishi_igbt_3_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRE_E231_1000_HITACHI_IGBT_2_LEVEL) return calculate_jre_e231_1000_hitachi_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRE_E233_MITSUBISHI_IGBT_2_LEVEL) return calculate_jre_e233_mitsubishi_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRE_E233_3000_HITACHI_IGBT_2_LEVEL) return calculate_jre_e233_3000_hitachi_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRE_E235_TOSHIBA_SIC_2_LEVEL) return calculate_jre_e235_toshiba_sic_2_level(cv);

            //JR WEST
            if (name == VVVF_Sound_Names.SOUND_JRW_207_TOSHIBA_GTO_2_LEVEL) return calculate_jrw_207_toshiba_gto_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRW_207_UPDATE_TOSHIBA_IGBT_2_LEVEL) return calculate_jrw_207_update_toshiba_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRW_321_HITACHI_IGBT_2_LEVEL) return calculate_jrw_321_hitachi_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRW_225_5100_MITSUBISHI_IGBT_2_LEVEL) return calculate_jrw_225_5100_mitsubishi_igbt_2_level(cv);

            //TOKYUU
            if (name == VVVF_Sound_Names.SOUND_TOKYUU_9000_HITACHI_GTO_2_LEVEL) return calculate_tokyuu_9000_hitachi_gto_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_TOKYUU_5000_HITACHI_IGBT_2_LEVEL) return calculate_tokyuu_5000_hitachi_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_TOKYUU_1000_1500_UPDATE_TOSHIBA_IGBT_2_LEVEL) return calculate_tokyuu_1000_1500_update_toshiba_igbt_2_level(cv);

            //KINTETSU
            if (name == VVVF_Sound_Names.SOUND_KINTETSU_9820_MITSUBISHI_IGBT_2_LEVEL) return calculate_kintetsu_9820_mitsubishi_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_KINTETSU_9820_HITACHI_IGBT_2_LEVEL) return calculate_kintetsu_9820_hitachi_igbt_2_level(cv);

            //KEIO
            if (name == VVVF_Sound_Names.SOUND_KEIO_8000_HITACHI_GTO_2_LEVEL) return calculate_keio_8000_hitachi_gto_2_level(cv);

            //KEIKYUU
            if (name == VVVF_Sound_Names.SOUND_KEIKYU_N1000_SIEMENS_GTO_2_LEVEL) return calculate_keikyu_n1000_siemens_gto_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_KEIKYU_N1000_SIEMENS_IGBT_2_LEVEL) return calculate_keikyu_n1000_siemens_igbt_2_level(cv);

            //TOBU
            if (name == VVVF_Sound_Names.SOUND_TOUBU_50050_HITACHI_IGBT_2_LEVEL) return calculate_toubu_50050_hitachi_igbt_2_level(cv);

            //KYOTO SUBWAY
            if (name == VVVF_Sound_Names.SOUND_KYOTO_SUBWAY_50_MITSUBISHI_GTO_2_LEVEL) return calculate_kyoto_subway_50_mitsubishi_gto_2_level(cv);

            //KEIHAN
            if (name == VVVF_Sound_Names.SOUND_KEIHAN_13000_TOYO_IGBT_2_LEVEL) return calculate_keihan_13000_toyo_igbt_2_level(cv);

            //TOEI SUBWAY
            if (name == VVVF_Sound_Names.SOUND_TOEI_6300_MITSUBISHI_IGBT_2_LEVEL) return calculate_toei_6300_mitsubishi_igbt_2_level(cv);

            //WMATA
            if (name == VVVF_Sound_Names.SOUND_WMATA_6000_ALSTOM_IGBT_2_LEVEL) return calculate_wmata_6000_alstom_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_WMATA_7000_TOSHIBA_IGBT_2_LEVEL) return calculate_wmata_7000_toshiba_igbt_2_level(cv);

            //custom sounds
            if (name == VVVF_Sound_Names.SOUND_X_TOYO_GTO_2_LEVEL) return calculate_toyo_gto_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_X_TOYO_IGBT_2_LEVEL) return calculate_toyo_igbt_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_X_SILENT_2_LEVEL) return calculate_silent_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_JRE_209_MITSUBISHI_GTO_2_LEVEL) return calculate_jre_209_mitsubishi_gto_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_X_FAMINA_2_LEVEL) return calculate_famina_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_X_REAL_DOREMI_2_LEVEL) return calculate_real_doremi_2_level(cv);
            if (name == VVVF_Sound_Names.SOUND_KEIKYU_NOT_REAL_N1000_SIEMENS_GTO_2_LEVEL) return calculate_not_real_keikyu_n1000_siemens_gto_2_level(cv);

            return calculate_silent_2_level(cv);
        }

        public static String get_Sound_Name(VVVF_Sound_Names name)
        {
            String name_str = name.ToString();
            String[] pattern = name_str.Split("_");

            String final_name = "";
            for(int i = 1; i < pattern.Length - 2;i++)
            {
                final_name += pattern[i];
                if(i + 1 < pattern.Length - 2)
                {
                    final_name += "_";
                }
            }

            final_name += "(" + pattern[pattern.Length - 2] + "-" + pattern[pattern.Length - 1] + ")";

            return final_name;
        }

        public static VVVF_Sound_Names get_Choosed_Sound()
        {
            VVVF_Sound_Names sound_name = VVVF_Sound_Names.SOUND_JRE_E231_1000_HITACHI_IGBT_2_LEVEL;
            Console.WriteLine("Select sound");
            int enum_len = Enum.GetNames(typeof(VVVF_Sound_Names)).Length;
            for (int i = 0; i < enum_len; i++)
            {
                Console.WriteLine(i.ToString() + " : " + get_Sound_Name((VVVF_Sound_Names)i));
            }

            while (true)
            {
                String val = Console.ReadLine();
                int val_i = 0;
                try
                {
                    val_i = Int32.Parse(val);
                }
                catch
                {
                    Console.WriteLine("Invalid value.");
                    continue;
                }
                if (enum_len <= val_i)
                {
                    Console.WriteLine("Invalid value.");
                    continue;
                }
                else
                {
                    sound_name = (VVVF_Sound_Names)val_i;
                    break;
                }
            }
            Console.WriteLine(sound_name.ToString() + " was selected");

            return sound_name;
        }

        // variables for controlling parameters
        public static Boolean do_frequency_change = true;
        public static Boolean brake = false;
        public static Boolean free_run = false;
        public static double wave_stat = 0;
        public static bool mascon_off = false;
        public static int temp_count = 0;

        public static void reset_control_variables()
        {
            do_frequency_change = true;
            brake = false;
            free_run = false;
            wave_stat = 0;
            mascon_off = false;
            temp_count = 0;
        }

        static Boolean check_for_freq_change()
        {
            double pre_sin_angle_freq = sin_angle_freq;
            count++;
            if (count % 60 == 0 && do_frequency_change && sin_angle_freq * M_1_2PI == wave_stat)
            {
                double sin_new_angle_freq = sin_angle_freq;
                if (!brake) sin_new_angle_freq += Math.PI / 400;
                else sin_new_angle_freq -= Math.PI / 400;
                double amp = sin_angle_freq / sin_new_angle_freq;
                sin_angle_freq = sin_new_angle_freq;
                sin_time = amp * sin_time;
            }

            if (temp_count == 0)
            {
                if (sin_angle_freq / 2 / Math.PI > 100 && !brake && do_frequency_change)
                {
                    do_frequency_change = false;
                    mascon_off = true;
                    count = 0;
                }
                else if (count / div_freq > 2 && !do_frequency_change)
                {
                    do_frequency_change = true;
                    mascon_off = false;
                    brake = true;
                    temp_count++;
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
                if (sin_angle_freq / 2 / Math.PI < 0 && brake && do_frequency_change) return false;
            }



            if (!mascon_off)
            {
                if (!free_run)
                    wave_stat = sin_angle_freq * M_1_2PI;
                else
                {
                    wave_stat += (Math.PI * 2) / (double)mascon_off_div;
                    if (sin_angle_freq * M_1_2PI < wave_stat)
                    {
                        wave_stat = sin_angle_freq * M_1_2PI;
                        free_run = false;
                    }
                    else
                    {
                        free_run = true;
                    }
                }
            }
            else
            {
                wave_stat -= (Math.PI * 2) / (double)mascon_off_div;
                if (wave_stat < 0) wave_stat = 0;
                free_run = true;
            }

            return true;
        }

        static void generate_sound(String output_path,VVVF_Sound_Names sound_name)
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
            writer.Write(new byte[]{ 0x00,0x00,0x00,0x00}); //CHUNK SIZE
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
                sin_time += 1.00 / div_freq;
                saw_time += 1.00 / div_freq;

                Control_Values cv_U = new Control_Values
                {
                    brake = brake,
                    mascon_on = !mascon_off,
                    free_run = free_run,
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = wave_stat
                };
                Wave_Values wv_U = get_Calculated_Value(sound_name, cv_U);

                Control_Values cv_V = new Control_Values
                {
                    brake = brake,
                    mascon_on = !mascon_off,
                    free_run = free_run,
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = wave_stat
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

        //only works with windows
        public static class Video_Generate_Values {
            public static Pulse_Mode pulse_mode = Pulse_Mode.P_1;
            public static double sine_amplitude = 0.0;
            public static double base_saw_freq = 0;
            public static double saw_freq_random_range = 0;
        }

        static void generate_video(String output_path,VVVF_Sound_Names sound_name)
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
            int wave_height = 50;

            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));


            if (!vr.IsOpened())
            {
                return;
            }

            Boolean START_F192_WAIT = true;
            if (START_F192_WAIT)
            {
                for(int i = 0; i < 192; i++)
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
                    sin_time = 0;
                    saw_time = 0;
                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                    g.DrawLine(new Pen(Color.Gray), 0, image_height / 2, image_width, image_height / 2);

                    double[] points_U = new double[image_width];
                    double[] points_V = new double[image_width];


                    for (int i = 0; i < image_width; i++)
                    {
                        sin_time += Math.PI / 250000.0;
                        saw_time += Math.PI / 250000.0;

                        Control_Values cv_U = new Control_Values
                        {
                            brake = brake,
                            mascon_on = !mascon_off,
                            free_run = free_run,
                            initial_phase = Math.PI * 2.0 / 3.0 * 0,
                            wave_stat = wave_stat
                        };
                        Wave_Values wv_U = get_Calculated_Value(sound_name, cv_U);
                        Control_Values cv_V = new Control_Values
                        {
                            brake = brake,
                            mascon_on = !mascon_off,
                            free_run = free_run,
                            initial_phase = Math.PI * 2.0 / 3.0 * 1,
                            wave_stat = wave_stat
                        };
                        Wave_Values wv_V = get_Calculated_Value(sound_name, cv_V);

                        double voltage_stat = (double)(wv_U.pwm_value - wv_V.pwm_value);
                        points_U[i] = wv_U.pwm_value;
                        points_V[i] = wv_V.pwm_value;
                    }

                    for (int i = 0; i < image_width - 1; i++)
                    {

                        int curr_val = (int)(-(points_U[i] - points_V[i]) * wave_height + image_height / 2.0);
                        int next_val = (int)(-(points_U[i+1] - points_V[i+1]) * wave_height  + image_height / 2.0);
                        g.DrawLine(new Pen(Color.Black), i, curr_val, ((curr_val != next_val) ? i : i+1), next_val);

                        //g.DrawLine(new Pen(Color.Gray), i, (int)(points_U[i] * wave_height + image_height / 2.0), i + 1, (int)(points_U[i+1] * wave_height + image_height / 2.0));
                        //g.DrawLine(new Pen(Color.Gray), i, (int)(points_V[i] * wave_height + image_height / 2.0), i + 1, (int)(points_V[i + 1] * wave_height + image_height / 2.0));
                    }


                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Form View",mat);
                    Cv2.WaitKey(1);

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();

                    temp = false;
                }
                else if(sound_block_count % movie_div != 0)
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


        private static String[] get_Pulse_Name(Pulse_Mode mode)
        {
            //Not in sync
            if (mode == Pulse_Mode.Not_In_Sync || mode == Pulse_Mode.Asyn_THI)
            {
                double saw_freq = saw_angle_freq / Math.PI / 2.0;
                return new string[] { String.Format("Async - " + saw_freq.ToString("F2")).PadLeft(6) };
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

                return new string[] { "Current Harmonic Minimum" , final_mode_name };
            }
            if (mode.ToString().StartsWith("SHE"))
            {
                String mode_name = mode.ToString();
                bool contain_wide = mode_name.Contains("Wide");
                mode_name = mode_name.Replace("_Wide", "");

                String[] mode_name_type = mode_name.Split("_");

                String final_mode_name = (contain_wide) ? "Wide " : "" + mode_name_type[1] + " Pulse";

                return new string[] { "Selective Harmonic Elimination", final_mode_name };
            }
            else
            {
                String[] mode_name_type = mode.ToString().Split("_");
                String mode_name = "";
                if (mode_name_type[0] == "SP") mode_name = "Shifted ";

                mode_name += mode_name_type[1] + " Pulse";

                return new string[] { mode_name };
            }
        }
        
        private static void generate_opening(int image_width,int image_height, VideoWriter vr)
        {
            //opening
            for (int i = 0; i < 128; i++)
            {
                Bitmap image = new(image_width, image_height);
                Graphics g = Graphics.FromImage(image);

                LinearGradientBrush gb = new LinearGradientBrush(new System.Drawing.Point(0, 0), new System.Drawing.Point(image_width, image_height), Color.FromArgb(0xFF, 0xFF, 0xFF), Color.FromArgb(0xFD , 0xE0, 0xE0));
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
                g.DrawString("Sine Freq[Hz]", title_fnt, title_brush, (int)((i < 40 + 10) ? -1000 : (double)((i > 80 + 10) ? 17 : 17 * (i - (40+10)) / 40.0)), 231);
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
        static void generate_status_video(String output_path,VVVF_Sound_Names sound_name)
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


            bool loop = true, temp = false, video_finished = false, final_show = false ,first_show = true;
            int freeze_count = 0;

            while (loop)
            {
                Control_Values cv_U = new Control_Values
                {
                    brake = brake,
                    mascon_on = !mascon_off,
                    free_run = free_run,
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = wave_stat
                };
                get_Calculated_Value(sound_name, cv_U);

                if (sound_block_count % movie_div == 0 && temp || final_show || first_show)
                {
                    sin_time = 0;
                    saw_time = 0;
                    Bitmap image = new(image_width, image_height);
                    Graphics g = Graphics.FromImage(image);

                    Color gradation_color;
                    if(free_run)
                    {
                        gradation_color = Color.FromArgb(0xE0, 0xFD,0xE0);
                    }else if (!brake)
                    {
                        gradation_color = Color.FromArgb(0xE0, 0xE0, 0xFD);
                    }
                    else
                    {
                        gradation_color = Color.FromArgb(0xFD,0xE0, 0xE0);
                    }


                    LinearGradientBrush gb = new LinearGradientBrush(
                        new System.Drawing.Point(0, 0), 
                        new System.Drawing.Point(image_width, image_height),
                        Color.FromArgb(0xFF,0xFF,0xFF) ,
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

                        g.DrawString(pulse_name[pulse_name.Length - 1], val_fnt, letter_brush, 17, 100);

                        if(pulse_name.Length > 1)
                        {
                            g.DrawString(pulse_name[0], val_mini_fnt, letter_brush, 17, 170);
                        }

                    }
                        

                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 226, image_width, 291 - 226);
                    g.DrawString("Sine Freq[Hz]", title_fnt, title_brush, 17, 231);
                    g.FillRectangle(Brushes.Blue, 0, 291, image_width, 8);
                    double sine_freq = sin_angle_freq / Math.PI / 2;
                    if (!final_show)
                        g.DrawString(String.Format("{0:f2}", sine_freq).PadLeft(6), val_fnt, letter_brush, 17, 323);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, 200, 255)), 0, 447, image_width, 513 - 447);
                    g.DrawString("Sine Amplitude[%]", title_fnt, title_brush, 17, 452);
                    g.FillRectangle(Brushes.Blue, 0, 513, image_width, 8);
                    if (!final_show)
                        g.DrawString(String.Format("{0:f2}", Video_Generate_Values.sine_amplitude*100).PadLeft(6), val_fnt, letter_brush, 17, 548);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), 0, 669, image_width, 735- 669);
                    g.DrawString("Freerun", title_fnt, title_brush, 17, 674);
                    g.FillRectangle(Brushes.LightGray, 0, 735, image_width, 8);
                    if (!final_show)
                        g.DrawString((mascon_off).ToString(), val_fnt, letter_brush, 17, 750);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), 0, 847, image_width, 913 - 847);
                    g.DrawString("Brake", title_fnt, title_brush, 17, 852);
                    g.FillRectangle(Brushes.LightGray, 0, 913, image_width, 8);
                    if (!final_show)
                        g.DrawString(brake.ToString(), val_fnt, letter_brush, 17, 930);




                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);

                    Cv2.ImShow("Wave Status View", mat);
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

                if (first_show)
                {
                    freeze_count++;
                    if(freeze_count > 64)
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

            vr.Release();
            vr.Dispose();
        }
        private static int realtime_sound_calculate(BufferedWaveProvider provider,VVVF_Sound_Names sound_name)
        {
            while (true)
            {

                double pre_sin_angle_freq = sin_angle_freq;
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey();
                    ConsoleKey key = keyinfo.Key;

                    if (key.Equals(ConsoleKey.B))
                    {
                        brake = !brake;
                        Console.WriteLine("\r\n Brake : " + brake);

                    }
                    else if (key.Equals(ConsoleKey.W))
                    {
                        double old_freq = sin_angle_freq;
                        double sin_new_angle_freq = sin_angle_freq;
                        if (!brake) sin_new_angle_freq += Math.PI;
                        else sin_new_angle_freq -= Math.PI;

                        double amp = sin_angle_freq / sin_new_angle_freq;
                        if (sin_new_angle_freq < 0) sin_new_angle_freq = 0;
                        sin_angle_freq = sin_new_angle_freq;
                        sin_time = amp * sin_time;
                        if (!mascon_off) wave_stat = sin_angle_freq / (Math.PI * 2);

                        Console.WriteLine("\r\n CurrentFreq : " + (sin_angle_freq) / Math.PI / 2);
                    }
                    else if (key.Equals(ConsoleKey.S))
                    {
                        double old_freq = sin_angle_freq;
                        double sin_new_angle_freq = sin_angle_freq;
                        if (!brake) sin_new_angle_freq += Math.PI / 2;
                        else sin_new_angle_freq -= Math.PI / 2;

                        double amp = sin_angle_freq / sin_new_angle_freq;
                        if (sin_new_angle_freq < 0) sin_new_angle_freq = 0;
                        sin_angle_freq = sin_new_angle_freq;
                        sin_time = amp * sin_time;
                        if (!mascon_off) wave_stat = sin_angle_freq / (Math.PI * 2);

                        Console.WriteLine("\r\n CurrentFreq : " + (sin_angle_freq) / Math.PI / 2);
                    }
                    else if (key.Equals(ConsoleKey.X))
                    {
                        double old_freq = sin_angle_freq;
                        double sin_new_angle_freq = sin_angle_freq;
                        if (!brake) sin_new_angle_freq += Math.PI / 4;
                        else sin_new_angle_freq -= Math.PI / 4;

                        double amp = sin_angle_freq / sin_new_angle_freq;
                        if (sin_new_angle_freq < 0) sin_new_angle_freq = 0;
                        sin_angle_freq = sin_new_angle_freq;
                        sin_time = amp * sin_time;

                        if (!mascon_off) wave_stat = sin_angle_freq / (Math.PI * 2);

                        Console.WriteLine("\r\n CurrentFreq : " + (sin_angle_freq) / Math.PI / 2);
                    }

                    else if (key.Equals(ConsoleKey.N))
                    {
                        mascon_off = !mascon_off;
                        Console.WriteLine("\r\n Mascon : " + !mascon_off);
                    }
                    else if (key.Equals(ConsoleKey.Enter)) return 0;
                    else if (key.Equals(ConsoleKey.R)) return 1;
                }

                if (!mascon_off)
                {
                    if (!free_run) wave_stat = sin_angle_freq / M_2PI;
                    else
                    {
                        wave_stat += (Math.PI * 2) / (double)(mascon_off_div / 12.0);
                        if (sin_angle_freq / (Math.PI * 2) < wave_stat)
                        {
                            wave_stat = sin_angle_freq / (Math.PI * 2);
                            free_run = false;
                        }
                        else
                            free_run = true;
                    }
                }
                else
                {
                    wave_stat -= (Math.PI * 2) / (double)(mascon_off_div / 12.0);
                    if (wave_stat < 0) wave_stat = 0;
                    free_run = true;
                }

                byte[] add = new byte[20];

                for (int i = 0; i < 20; i++)
                {
                    sin_time += 1.0 / 192000.0;
                    saw_time += 1.0 / 192000.0;

                    Control_Values cv_U = new Control_Values
                    {
                        brake = brake,
                        mascon_on = !mascon_off,
                        free_run = free_run,
                        initial_phase = Math.PI * 2.0 / 3.0 * 0,
                        wave_stat = wave_stat
                    };
                    Wave_Values wv_U = get_Calculated_Value(sound_name, cv_U);
                    Control_Values cv_V = new Control_Values
                    {
                        brake = brake,
                        mascon_on = !mascon_off,
                        free_run = free_run,
                        initial_phase = Math.PI * 2.0 / 3.0 * 1,
                        wave_stat = wave_stat
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

        public static String get_Path()
        {
            String output_path = "";
            while (output_path.Length == 0)
            {
                Console.Write("Enter the export path for vvvf sound : ");
                output_path = Console.ReadLine();
                if (output_path.Length == 0)
                {
                    Console.WriteLine("Error. Reenter a path.");
                }
            }
            return output_path;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Select the function to do. (Use \",\" to use multiple function)");

            Console.WriteLine("1 : Generate Sound");
            Console.WriteLine("2 : Generate Wave form Video");
            Console.WriteLine("3 : Generate Mascon Video");
            Console.WriteLine("4 : Realtime VVVF Sound generation");

            String line = Console.ReadLine();

            String[] split = line.Split(",");

            bool gen_audio = false, gen_video = false, gen_mascon_video = false, realtime = false;

            for(int i = 0; i < split.Length; i++)
            {
                if (split[i] == "1") gen_audio = true;
                if (split[i] == "2") gen_video = true;
                if (split[i] == "3") gen_mascon_video = true;
                if (split[i] == "4") realtime = true;
            }

            
            if(gen_audio || gen_video || gen_mascon_video)
            {
                VVVF_Sound_Names sound_name = get_Choosed_Sound();
                String output_path = get_Path();

                if (gen_audio) generate_sound(output_path, sound_name);
                if (gen_video) generate_video(output_path, sound_name);
                if (gen_mascon_video) generate_status_video(output_path, sound_name);
            }

            if (realtime) realtime_sound();
        }
    }
}
