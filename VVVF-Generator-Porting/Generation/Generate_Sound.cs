using System;
using System.IO;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_sound_definition;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.Generation.Generate_Common;

namespace VVVF_Generator_Porting.Generation
{
    public class Generate_Sound
    {
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
    }
}
