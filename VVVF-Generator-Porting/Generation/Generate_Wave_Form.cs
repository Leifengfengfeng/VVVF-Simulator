using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_sound_definition;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.Generation.Generate_Common;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_Wave_Form
    {
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

        public static void generate_taroimo_like_wave_U_V(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            set_Allowed_Random_Freq_Move(false);

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
    }
}
