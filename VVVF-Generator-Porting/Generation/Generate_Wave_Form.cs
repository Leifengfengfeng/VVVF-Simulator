using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.Generation.Generate_Common;
using VVVF_Generator_Porting.Yaml_VVVF_Sound;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_Wave_Form
    {
        public static void generate_wave_U_V(String output_path, Yaml_Sound_Data sound_data)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = "";
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

                        for (int j = 0; j < 2; j++)
                        {
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

        public static void generate_wave_UVW(String output_path, Yaml_Sound_Data sound_data)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = "";
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            bool temp = true;
            Int32 sound_block_count = 0;

            int image_width = 1500;
            int image_height = 1000;
            int movie_div = 3000;

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

                    


                    for (int i = 0; i < image_width * calculate_div; i++)
                    {
                        int[] points_U = new int[2];
                        int[] points_V = new int[2];
                        int[] points_W = new int[2];

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
                            Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(cv_U, sound_data);
                            points_U[j] = wv_U.pwm_value;

                            Control_Values cv_V = new Control_Values
                            {
                                brake = is_Braking(),
                                mascon_on = !is_Mascon_Off(),
                                free_run = is_Free_Running(),
                                initial_phase = Math.PI * 2.0 / 3.0 * 1,
                                wave_stat = get_Control_Frequency()
                            };
                            Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(cv_V, sound_data);
                            points_V[j] = wv_V.pwm_value;

                            Control_Values cv_W = new Control_Values
                            {
                                brake = is_Braking(),
                                mascon_on = !is_Mascon_Off(),
                                free_run = is_Free_Running(),
                                initial_phase = Math.PI * 2.0 / 3.0 * 2,
                                wave_stat = get_Control_Frequency()
                            };
                            Wave_Values wv_W = Yaml_VVVF_Wave.calculate_Yaml(cv_W, sound_data);
                            points_W[j] = wv_W.pwm_value;

                            if (j == 0)
                            {
                                add_Saw_Time(Math.PI / (120000.0 * calculate_div));
                                add_Sine_Time(Math.PI / (120000.0 * calculate_div));
                            }
                        }

                        //U
                        g.DrawLine(new Pen(Color.Black),
                            (int)Math.Round(i / (double)calculate_div),
                            points_U[0] * -100 + 300,
                            (int)Math.Round(((points_U[0] != points_U[1]) ? i : i + 1) / (double)calculate_div),
                            points_U[1] * -100 + 300
                        ) ;

                        //V
                        g.DrawLine(new Pen(Color.Black),
                            (int)Math.Round(i / (double)calculate_div),
                            points_V[0] * -100 + 600,
                            (int)Math.Round(((points_V[0] != points_V[1]) ? i : i + 1) / (double)calculate_div),
                            points_V[1] * -100 + 600
                        );

                        //W
                        g.DrawLine(new Pen(Color.Black),
                            (int)Math.Round(i / (double)calculate_div),
                            points_W[0] * -100 + 900,
                            (int)Math.Round(((points_W[0] != points_W[1]) ? i : i + 1) / (double)calculate_div),
                            points_W[1] * -100 + 900
                        );

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

        public static void generate_taroimo_like_wave_U_V(String output_path, Yaml_Sound_Data sound_data)
        {
            reset_control_variables();
            reset_all_variables();

            set_Allowed_Random_Freq_Move(false);

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = "";
            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".avi";

            bool temp = true;
            Int32 sound_block_count = 0;

            int image_width = 2880;
            int image_height = 540;

            int movie_div = 3000;
            int wave_height = 100;
            int calculate_div = 30;

            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));


            if (!vr.IsOpened())
            {
                return;
            }

            Boolean START_F64_WAIT = true;
            if (START_F64_WAIT)
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

                    for (int i = 0; i < (image_width-100) * calculate_div; i++)
                    {
                        Wave_Values[] values = new Wave_Values[4];

                        for (int j = 0; j < 2; j++)
                        {
                            Control_Values cv_U = new Control_Values
                            {
                                brake = is_Braking(),
                                mascon_on = !is_Mascon_Off(),
                                free_run = is_Free_Running(),
                                initial_phase = Math.PI / 6.0,
                                wave_stat = get_Control_Frequency()
                            };
                            Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(cv_U, sound_data);
                            Control_Values cv_V = new Control_Values
                            {
                                brake = is_Braking(),
                                mascon_on = !is_Mascon_Off(),
                                free_run = is_Free_Running(),
                                initial_phase = Math.PI / 6.0 + Math.PI * 2.0 / 3.0 * 1,
                                wave_stat = get_Control_Frequency()
                            };
                            Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(cv_V, sound_data);

                            if (j == 0)
                            {
                                add_Saw_Time(2 / (60.0 * calculate_div * (image_width - 100)));
                                add_Sine_Time(2 / (60.0 * calculate_div * (image_width - 100)));
                            }

                            values[j * 2] = wv_U;
                            values[j * 2 + 1] = wv_V;
                        }


                        int curr_val = (int)(-(values[0].pwm_value - values[1].pwm_value) * wave_height + image_height / 2.0);
                        int next_val = (int)(-(values[2].pwm_value - values[3].pwm_value) * wave_height + image_height / 2.0);
                        g.DrawLine(new Pen(Color.Black,2), (int)(i / (double)calculate_div) + 50 , curr_val, (int)(((curr_val != next_val) ? i : i + 1) / (double)calculate_div) + 50, next_val);
                    }

                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    byte[] img = ms.GetBuffer();
                    Mat mat = OpenCvSharp.Mat.FromImageData(img);
                    vr.Write(mat);
                    mat.Dispose();
                    ms.Dispose();

                    MemoryStream resized_ms = new MemoryStream();
                    Bitmap resized = new Bitmap(image, image_width / 2, image_height / 2);
                    resized.Save(resized_ms, ImageFormat.Png);
                    byte[] resized_img = resized_ms.GetBuffer();
                    Mat resized_mat = OpenCvSharp.Mat.FromImageData(resized_img);
                    Cv2.ImShow("Wave Form", resized_mat);
                    Cv2.WaitKey(1);
                    resized_mat.Dispose();
                    resized_ms.Dispose();
                    

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

                    vr.Write(mat);

                    g.Dispose();
                    image.Dispose();
                }
            }

            vr.Release();
            vr.Dispose();
        }
    }
}
