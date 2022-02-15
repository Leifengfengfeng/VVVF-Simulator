using System;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_sound_definition;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.my_math;
using static VVVF_Generator_Porting.Generation.Generate_Common;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_PWM_Data
    {
        public static void generate_pwm_data(String output_path, VVVF_Sound_Names sound_name)
        {
            reset_control_variables();
            reset_all_variables();

            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            String appear_sound_name = get_Sound_Name(sound_name);

            String fileName = output_path + "\\" + appear_sound_name + "-" + gen_time + ".csv";

            int wave_samples = 3000;
            int[] wave_data = new int[wave_samples];

            Control_Values cv_U = new Control_Values
            {
                brake = is_Braking(),
                mascon_on = !is_Mascon_Off(),
                free_run = is_Free_Running(),
                initial_phase = Math.PI * 2.0 / 3.0 * 0,
                wave_stat = 35
            };

            for (int i = 0; i < wave_samples; i++)
            {
                add_Sine_Time(1.00 / wave_samples / 2);
                add_Saw_Time(1.00 / wave_samples / 2);
                Wave_Values wv_U = get_Calculated_Value(sound_name, cv_U);
                switch (wv_U.pwm_value)
                {
                    case 0:
                        wave_data[i] = -1;
                        break;
                    case 2:
                        wave_data[i] = 1;
                        break;
                    default:
                        wave_data[i] = 0;
                        break;
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.Write(string.Join(",", wave_data));
            }
        }
    }
}
