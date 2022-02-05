using System;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_wave_control;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_Common
    {
        static readonly double M_1_2PI = 1.0 / (2.0 * Math.PI);

        public static double count = 0;
        public static readonly int div_freq = 192 * 1000;

        public static class Video_Generate_Values
        {
            public static Pulse_Mode pulse_mode = Pulse_Mode.P_1;
            public static double sine_amplitude = 0.0;
            public static Carrier_Freq carrier_freq_data;
            public static double dipolar = -1;
            public static double sine_angle_freq = 0.0;
        }


        /// <summary>
        /// この関数は、音声生成や、動画生成時の、マスコンの制御状態等を記述する関数です。
        /// この関数を呼ぶたびに、更新されます。
        /// 
        /// This is a function which will control a acceleration or brake when generating audio or video.
        /// It will be updated everytime this function colled.
        /// </summary>
        /// <returns></returns>
        public static Boolean check_for_freq_change()
        {
            count++;

            //This is core of control. Never to change.
            if (count % 60 == 0 && is_Do_Freq_Change() && get_Sine_Angle_Freq() * M_1_2PI == get_Control_Frequency())
            {
                double sin_new_angle_freq = get_Sine_Angle_Freq();
                 
                // But you can change 400 here. If you wanna accelerate more slower, change 400 to like 800;
                if (!is_Braking()) sin_new_angle_freq += Math.PI / 400;
                else sin_new_angle_freq -= Math.PI / 400;

                double amp = get_Sine_Angle_Freq() / sin_new_angle_freq;

                set_Sine_Angle_Freq(sin_new_angle_freq);

                if (is_Allowed_Sine_Time_Change())
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



            //This is also core of controlling. This should never changed.
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

        

       
    }
}
