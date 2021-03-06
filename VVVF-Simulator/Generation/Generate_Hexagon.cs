using OpenCvSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static VVVF_Simulator.vvvf_wave_calculate;
using static VVVF_Simulator.VVVF_Control_Values;
using static VVVF_Simulator.Generation.Generate_Common;
using static VVVF_Simulator.my_math;
using VVVF_Simulator.Yaml_VVVF_Sound;

namespace VVVF_Simulator.Generation
{
    public class Generate_Hexagon
    {

        public static bool generate_wave_hexagon_explain(String output_path, Yaml_Sound_Data sound_data, bool circle, double d)
        {
            VVVF_Control_Values control = new();
            control.reset_control_variables();
            control.reset_all_variables();

            int movie_div = 3000;

            int image_width = 1300;
            int image_height = 500;

            int pwm_image_width = 750;
            int pwm_image_height = 500;

            int hexagon_image_size = 1000;

            int hex_div_seed = 10000;
            int hex_div = 6 * hex_div_seed;

            Boolean draw_zero_vector_circle = circle;

            VideoWriter vr = new (output_path, FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));
            if (!vr.IsOpened()) return false;

            Boolean START_F192_WAIT = false;
            if (START_F192_WAIT)
            {
                Bitmap free_image = new(image_width, image_height);
                Graphics free_g = Graphics.FromImage(free_image);
                free_g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                MemoryStream free_ms = new();
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

            control.set_Sine_Time(0);
            control.set_Saw_Time(0);

            control.set_Control_Frequency(d);
            control.set_Sine_Angle_Freq(d * M_2PI);

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

            double[] x_min_max = new double[] { 50000, 0 };
            double[] hexagon_coordinate = new double[] { 100, 500 };

            for (int i = 0; i < hex_div; i++)
            {
                control.add_Sine_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() == 0) ? 0 : 1 / control.get_Sine_Freq()));
                control.add_Saw_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() == 0) ? 0 : 1 / control.get_Sine_Freq()));

                Control_Values cv_U = new()
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(control, cv_U, sound_data);
                points_U[i] = wv_U.pwm_value;

                Control_Values cv_V = new()
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(control, cv_V, sound_data);
                points_V[i] = wv_V.pwm_value;

                Control_Values cv_W = new()
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 2,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_W = Yaml_VVVF_Wave.calculate_Yaml(control, cv_W, sound_data);
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
            double moved_x = (image_width - x_min_max[1] - x_min_max[0]) / 2.0;

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

            return true;
        }
        public static void generate_wave_hexagon(String fileName, Yaml_Sound_Data sound_data, bool circle)
        {
            VVVF_Control_Values control = new();
            control.reset_control_variables();
            control.reset_all_variables();

            Boolean draw_zero_vector_circle = circle;

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
                MemoryStream ms = new();
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
                    control.set_Sine_Time(0);
                    control.set_Saw_Time(0);

                    Bitmap hexagon_image = new(image_width, image_height);
                    Graphics hexagon_g = Graphics.FromImage(hexagon_image);

                    Boolean drawn_circle = false;
                    Bitmap zero_circle_image = new(image_width, image_height);
                    Graphics zero_circle_g = Graphics.FromImage(zero_circle_image);

                    double[] hexagon_coordinate = new double[] { 100, 500 };
                    double[] x_min_max = new double[2] { 10000, 0 };

                    for (int i = 0; i < hex_div; i++)
                    {

                        control.add_Sine_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() * M_1_2PI == 0) ? 0 : 1 / control.get_Sine_Freq()));
                        control.add_Saw_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() == 0) ? 0 : 1 / control.get_Sine_Freq()));

                        Control_Values cv_U = new Control_Values
                        {
                            brake = control.is_Braking(),
                            mascon_on = !control.is_Mascon_Off(),
                            free_run = control.is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 0,
                            wave_stat = control.get_Control_Frequency()
                        };
                        Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(control, cv_U, sound_data);

                        Control_Values cv_V = new Control_Values
                        {
                            brake = control.is_Braking(),
                            mascon_on = !control.is_Mascon_Off(),
                            free_run = control.is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 1,
                            wave_stat = control.get_Control_Frequency()
                        };
                        Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(control, cv_V, sound_data);

                        Control_Values cv_W = new Control_Values
                        {
                            brake = control.is_Braking(),
                            mascon_on = !control.is_Mascon_Off(),
                            free_run = control.is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 2,
                            wave_stat = control.get_Control_Frequency()
                        };
                        Wave_Values wv_W = Yaml_VVVF_Wave.calculate_Yaml(control, cv_W, sound_data);

                        double move_x = -0.5 * wv_W.pwm_value - 0.5 * wv_V.pwm_value + wv_U.pwm_value;
                        double move_y = -0.866025403784438646763 * wv_W.pwm_value + 0.866025403784438646763 * wv_V.pwm_value;

                        double int_move_x = 200 * move_x / (double)hex_div_seed;
                        double int_move_y = 200 * move_y / (double)hex_div_seed;


                        hexagon_g.DrawLine(new Pen(Color.Black),
                            (int)Math.Round(hexagon_coordinate[0]),
                            (int)Math.Round(hexagon_coordinate[1]),
                            (int)Math.Round(hexagon_coordinate[0] + int_move_x),
                            (int)Math.Round(hexagon_coordinate[1] + int_move_y)
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

                    double moved_x = (image_width - x_min_max[1] - x_min_max[0]) / 2.0;
                    final_g.DrawImage(hexagon_image, (int)Math.Round(moved_x), 0);
                    final_g.DrawImage(zero_circle_image, (int)Math.Round(moved_x), 0);

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

                loop = Check_For_Freq_Change(control);

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

        public static void generate_wave_hexagon_taroimo_like(String fileName, Yaml_Sound_Data sound_data, Boolean circle)
        {
            VVVF_Control_Values control = new();
            control.reset_control_variables();
            control.reset_all_variables();

            control.set_Allowed_Random_Freq_Move(false);

            Boolean draw_zero_vector_circle = circle;

            bool temp = true;
            Int32 sound_block_count = 0;


            int image_width = 960;
            int image_height = 540;

            int hex_div_seed = 10000;
            int hex_div = 6 * hex_div_seed;

            // around 60fps
            int movie_div = 3000;

            VideoWriter vr = new VideoWriter(fileName, OpenCvSharp.FourCC.H264, div_freq / movie_div, new OpenCvSharp.Size(image_width, image_height));


            Bitmap max_hexagon = new Bitmap(image_width, image_height);
            Graphics graphic_max_hexagon = Graphics.FromImage(max_hexagon);

            graphic_max_hexagon.FillRectangle(new SolidBrush(Color.FromArgb(226, 226, 226)), 0, 0, image_width, image_height);
            graphic_max_hexagon.FillPolygon(new SolidBrush(Color.White), new PointF[] {
                new PointF(image_width / 2 - 240 , image_height / 2), new PointF(image_width / 2 - 120, image_height / 2 + 208) ,
                new PointF(image_width / 2 + 120 , image_height / 2 + 208) , new PointF(image_width / 2 + 240 , image_height / 2) ,
                new PointF(image_width / 2 + 120 , image_height / 2 - 208) , new PointF(image_width / 2 - 120 , image_height / 2 - 208) ,
            });
            int outline_edit = 0;
            graphic_max_hexagon.DrawPolygon(new Pen(Color.FromArgb(180, 180, 180)), new PointF[] { 
                new PointF(image_width / 2 - (120 * 2 + outline_edit) , image_height / 2 + outline_edit), 
                new PointF(image_width / 2 - (120 + outline_edit)     , image_height / 2 + (208 + outline_edit)) ,
                new PointF(image_width / 2 + (120 + outline_edit)     , image_height / 2 + (208 + outline_edit)) , 
                new PointF(image_width / 2 + (120 * 2 + outline_edit) , image_height / 2 + outline_edit) ,
                new PointF(image_width / 2 + (120 + outline_edit)     , image_height / 2 - (208 + outline_edit)) , 
                new PointF(image_width / 2 - (120 + outline_edit)     , image_height / 2 - (208 + outline_edit)) ,
            });
            

            graphic_max_hexagon.Dispose();

            if (!vr.IsOpened())
            {
                return;
            }

            Boolean START_F64_WAIT = true;
            if (START_F64_WAIT)
            {
                Bitmap image = new(image_width, image_height);
                Graphics g = Graphics.FromImage(image);

                g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                g.DrawImage(max_hexagon, 0, 0);

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

            Boolean loop = true;
            while (loop)
            {


                if (sound_block_count % movie_div == 0 && temp)
                {
                    control.set_Sine_Time(0);
                    control.set_Saw_Time(0);

                    Bitmap hexagon_image = new(image_width, image_height);
                    Graphics hexagon_g = Graphics.FromImage(hexagon_image);

                    Boolean drawn_circle = false;
                    Bitmap zero_circle_image = new(image_width, image_height);
                    Graphics zero_circle_g = Graphics.FromImage(zero_circle_image);

                    double[] hexagon_coordinate = new double[] { 100, image_height/2 };
                    double[] x_min_max = new double[2] { 100000, 0 };

                    for (int i = 0; i < hex_div; i++)
                    {

                        control.add_Sine_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() == 0) ? 0 : 1 / control.get_Sine_Freq()));
                        control.add_Saw_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() == 0) ? 0 : 1 / control.get_Sine_Freq()));

                        Control_Values cv_U = new Control_Values
                        {
                            brake = control.is_Braking(),
                            mascon_on = !control.is_Mascon_Off(),
                            free_run = control.is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 0,
                            wave_stat = control.get_Control_Frequency()
                        };
                        Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(control, cv_U, sound_data);

                        Control_Values cv_V = new Control_Values
                        {
                            brake = control.is_Braking(),
                            mascon_on = !control.is_Mascon_Off(),
                            free_run = control.is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 1,
                            wave_stat = control.get_Control_Frequency()
                        };
                        Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(control, cv_V, sound_data);

                        Control_Values cv_W = new Control_Values
                        {
                            brake = control.is_Braking(),
                            mascon_on = !control.is_Mascon_Off(),
                            free_run = control.is_Free_Running(),
                            initial_phase = Math.PI * 2.0 / 3.0 * 2,
                            wave_stat = control.get_Control_Frequency()
                        };
                        Wave_Values wv_W = Yaml_VVVF_Wave.calculate_Yaml(control, cv_W, sound_data);

                        double move_x = -0.5 * wv_W.pwm_value - 0.5 * wv_V.pwm_value + wv_U.pwm_value;
                        double move_y = -0.866025403784438646763 * wv_W.pwm_value + 0.866025403784438646763 * wv_V.pwm_value;

                        double int_move_x = 120 * move_x / (double)hex_div_seed;
                        double int_move_y = 120 * move_y / (double)hex_div_seed;

                        hexagon_g.DrawLine(new Pen(Color.Black),
                            (int)Math.Round(hexagon_coordinate[0]),
                            (int)Math.Round(hexagon_coordinate[1]),
                            (int)Math.Round(hexagon_coordinate[0] + int_move_x),
                            (int)Math.Round(hexagon_coordinate[1] + int_move_y)
                        );

                        if (move_x == 0 && move_y == 0 && draw_zero_vector_circle)
                        {
                            if (!drawn_circle)
                            {
                                drawn_circle = true;
                                double radius = 5 * ((control.get_Control_Frequency() > 40) ? 1 : (control.get_Control_Frequency() / 40.0));
                                zero_circle_g.FillEllipse(new SolidBrush(Color.White),
                                    (int)Math.Round(hexagon_coordinate[0] - radius),
                                    (int)Math.Round(hexagon_coordinate[1] - radius),
                                    (int)Math.Round(radius * 2),
                                    (int)Math.Round(radius * 2)
                                );
                                zero_circle_g.DrawEllipse(new Pen(Color.Black),
                                    (int)Math.Round(hexagon_coordinate[0] - radius),
                                    (int)Math.Round(hexagon_coordinate[1] - radius),
                                    (int)Math.Round(radius * 2),
                                    (int)Math.Round(radius * 2)
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

                    double moved_x = (image_width - x_min_max[1] - x_min_max[0]) / 2.0;
                    final_g.DrawImage(max_hexagon, 0, 0);
                    final_g.DrawImage(hexagon_image, (int)Math.Round(moved_x), 0);
                    final_g.DrawImage(zero_circle_image, (int)Math.Round(moved_x), 0);
                    

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

                loop = Check_For_Freq_Change(control);

            }

            Boolean END_F64_WAIT = true;
            if (END_F64_WAIT)
            {
                Bitmap image = new(image_width, image_height);
                Graphics g = Graphics.FromImage(image);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, image_width, image_height);
                g.DrawImage(max_hexagon, 0, 0);
                MemoryStream ms = new();
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
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"> Path for png file </param>
        /// <param name="sound_data">SOUND DATA</param>
        /// <param name="circle">Setting whether see zero vector circle or not</param>
        /// <param name="d">Frequency you want to see</param>
        public static void generate_wave_hexagon_picture(String fileName, Yaml_Sound_Data sound_data, Boolean circle, double d)
        {
            VVVF_Control_Values control = new();

            control.reset_control_variables();
            control.reset_all_variables();

            Boolean draw_zero_vector_circle = circle;

            control.set_Sine_Angle_Freq(d * M_2PI);
            control.set_Control_Frequency(d);

            int image_width = 1000;
            int image_height = 1000;

            int hex_div_seed = 10000;
            int hex_div = 6 * hex_div_seed;
            control.set_Sine_Time(0);
            control.set_Saw_Time(0);

            Bitmap hexagon_image = new(image_width, image_height);
            Graphics hexagon_g = Graphics.FromImage(hexagon_image);

            Boolean drawn_circle = false;
            Bitmap zero_circle_image = new(image_width, image_height);
            Graphics zero_circle_g = Graphics.FromImage(zero_circle_image);

            double[] hexagon_coordinate = new double[] { 100, 500 };
            double[] x_min_max = new double[2] { 500, 0 };

            for (int i = 0; i < hex_div; i++)
            {

                control.add_Sine_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() * M_1_2PI == 0) ? 0 : 1 / control.get_Sine_Freq()));
                control.add_Saw_Time(1.0 / (hex_div) * ((control.get_Sine_Freq() == 0) ? 0 : 1 / control.get_Sine_Freq()));

                Control_Values cv_U = new()
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(control, cv_U, sound_data);

                Control_Values cv_V = new()
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(control, cv_V, sound_data);

                Control_Values cv_W = new()
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 2,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_W = Yaml_VVVF_Wave.calculate_Yaml(control, cv_W, sound_data);

                double move_x = -0.5 * wv_W.pwm_value - 0.5 * wv_V.pwm_value + wv_U.pwm_value;
                double move_y = -0.866025403784438646763 * wv_W.pwm_value + 0.866025403784438646763 * wv_V.pwm_value;

                double int_move_x = 200 * move_x / (double)hex_div_seed;
                double int_move_y = 200 * move_y / (double)hex_div_seed;


                hexagon_g.DrawLine(new Pen(Color.Black),
                    (int)Math.Round(hexagon_coordinate[0]),
                    (int)Math.Round(hexagon_coordinate[1]),
                    (int)Math.Round(hexagon_coordinate[0] + int_move_x),
                    (int)Math.Round(hexagon_coordinate[1] + int_move_y)
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

            double moved_x = (image_width - x_min_max[1] - x_min_max[0]) / 2.0;
            final_g.DrawImage(hexagon_image, (int)Math.Round(moved_x), 0);
            final_g.DrawImage(zero_circle_image, (int)Math.Round(moved_x), 0);

            MemoryStream ms = new MemoryStream();
            final_image.Save(ms, ImageFormat.Png);
            byte[] img = ms.GetBuffer();
            Mat mat = Mat.FromImageData(img);

            final_image.Save(fileName,ImageFormat.Png);


            Cv2.ImShow("Hexagon", mat);
            Cv2.WaitKey();

            hexagon_g.Dispose();
            final_g.Dispose();
            zero_circle_g.Dispose();

            hexagon_image.Dispose();
            final_image.Dispose();
            zero_circle_image.Dispose();
        }
    }


}
