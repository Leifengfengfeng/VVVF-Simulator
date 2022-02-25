using System;
using static VVVF_Simulator.vvvf_wave_calculate;
using static VVVF_Simulator.VVVF_Control_Values;

namespace VVVF_Simulator.Generation
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
            public static Carrier_Freq carrier_freq_data = new(20,0);
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
        public static bool Check_For_Freq_Change(VVVF_Control_Values control)
        {
            count++;

            //This is core of control. Never to change.
            if (count % 60 == 0 && control.is_Do_Freq_Change() && control.get_Sine_Angle_Freq() * M_1_2PI == control.get_Control_Frequency())
            {
                double sin_new_angle_freq = control.get_Sine_Angle_Freq();
                 
                // But you can change 400 here. If you wanna accelerate more slower, change 400 to like 800;
                if (!control.is_Braking()) sin_new_angle_freq += Math.PI / 450;
                else sin_new_angle_freq -= Math.PI / 450;

                double amp = control.get_Sine_Angle_Freq() / sin_new_angle_freq;

                control.set_Sine_Angle_Freq(sin_new_angle_freq);

                if (control.is_Allowed_Sine_Time_Change())
                    control.multi_Sine_Time(amp);
            }

            if (control.get_Temp_Count() == 0)
            {
                if (control.get_Sine_Angle_Freq() * M_1_2PI > 110 && !control.is_Braking() && control.is_Do_Freq_Change())
                {
                    control.set_Do_Freq_Change(false);
                    control.set_Mascon_Off(true);
                    count = 0;
                }
                else if (count / div_freq > 2 && !control.is_Do_Freq_Change())
                {
                    control.set_Do_Freq_Change(true);
                    control.set_Mascon_Off(false);
                    control.set_Braking(true);
                    control.set_Temp_Count(control.get_Temp_Count() + 1);
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
                if (control.get_Sine_Angle_Freq() * M_1_2PI < 0 && control.is_Braking() && control.is_Do_Freq_Change()) return false;
            }



            //This is also core of controlling. This should never changed.
            if (!control.is_Mascon_Off())
            {
                if (!control.is_Free_Running())
                    control.set_Control_Frequency(control.get_Sine_Angle_Freq() * M_1_2PI);
                else
                {
                    control.add_Control_Frequency((Math.PI * 2) / (double)control.get_Mascon_Off_Div());

                    if (control.get_Sine_Angle_Freq() * M_1_2PI < control.get_Control_Frequency())
                    {
                        control.set_Control_Frequency(control.get_Sine_Angle_Freq() * M_1_2PI);
                        control.set_Free_Running(false);
                    }
                    else
                    {
                        control.set_Free_Running(true);
                    }
                }
            }
            else
            {
                control.add_Control_Frequency(- (Math.PI * 2) / (double)control.get_Mascon_Off_Div());
                if (control.get_Control_Frequency() < 0) control.set_Control_Frequency(0);
                control.set_Free_Running(true);
            }

            return true;
        }

        

       
    }
}
