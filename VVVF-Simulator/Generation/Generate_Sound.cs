using System;
using System.IO;
using static VVVF_Simulator.vvvf_wave_calculate;
using static VVVF_Simulator.VVVF_Control_Values;
using static VVVF_Simulator.Generation.Generate_Common;
using static VVVF_Simulator.my_math;
using static VVVF_Simulator.Generation.Generate_Sound.Harmonic_Data;
using VVVF_Simulator.Yaml_VVVF_Sound;

namespace VVVF_Simulator.Generation
{
    public class Generate_Sound
    {
        public static void generate_sound(String output_path, Yaml_Sound_Data sound_name)
        {
            VVVF_Control_Values control = new();
            control.reset_control_variables();
            control.reset_all_variables();

            Int32 sound_block_count = 0;

            BinaryWriter writer = new BinaryWriter(new FileStream(output_path, FileMode.Create));

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
                control.add_Sine_Time(1.00 / div_freq);
                control.add_Saw_Time(1.00 / div_freq);

                Control_Values cv_U = new Control_Values
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 0,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(control, cv_U, sound_name);

                Control_Values cv_V = new Control_Values
                {
                    brake = control.is_Braking(),
                    mascon_on = !control.is_Mascon_Off(),
                    free_run = control.is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = control.get_Control_Frequency()
                };
                Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(control, cv_V, sound_name);

                for (int i = 0; i < 1; i++)
                {
                    double pwm_value = wv_U.pwm_value - wv_V.pwm_value;
                    byte sound_byte = 0x80;
                    if (pwm_value == 2) sound_byte += 0x40;
                    else if (pwm_value == 1) sound_byte += 0x20;
                    else if (pwm_value == -1) sound_byte -= 0x20;
                    else if (pwm_value == -2) sound_byte -= 0x40;
                    writer.Write(sound_byte);
                }
                sound_block_count++;

                loop = Check_For_Freq_Change(control);

            }



            writer.Seek(4, SeekOrigin.Begin);
            writer.Write(sound_block_count + 36);

            writer.Seek(40, SeekOrigin.Begin);
            writer.Write(sound_block_count);

            writer.Close();
        }

        public class Harmonic_Data {
            public double harmonic { get; set; } = 0;
            public Harmonic_Data_Amplitude amplitude { get; set; } = new Harmonic_Data_Amplitude();
            public double disappear { get; set; } = 0;

            public class Harmonic_Data_Amplitude {
                public double start { get; set; } = 0;
                public double start_val { get; set; } = 0;
                public double end { get; set; } = 0;
                public double end_val { get; set; } = 0;
                public double min_val { get; set; } = 0;
                public double max_val { get; set; } = 0;
            }

        }

        public static void generate_env_sound(String output_path)
        {
            VVVF_Control_Values control = new();
            control.reset_control_variables();
            control.reset_all_variables();

            Int32 sound_block_count = 0;

            BinaryWriter writer = new (new FileStream(output_path, FileMode.Create));

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
                control.add_Sine_Time(1.00 / div_freq);
                control.add_Saw_Time(1.00 / div_freq);

                double sound_byte_int = 0;
                int total_sound_count = 0;

                Harmonic_Data[] harmonics = new Harmonic_Data[] {
                    new Harmonic_Data{harmonic = 0.8, amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x30,end=60,end_val=0x30,min_val=0,max_val=0x30},disappear = 880},
                    new Harmonic_Data{harmonic = 0.333, amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x30,end=60,end_val=0x30,min_val=0,max_val=0x30},disappear = 880},
                    new Harmonic_Data{harmonic = 1, amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x30,end=60,end_val=0x30,min_val=0,max_val=0x30},disappear = 880},
                    new Harmonic_Data{harmonic = 10, amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x30,end=60,end_val=0x30,min_val=0,max_val=0x30},disappear = 880},
                    new Harmonic_Data{harmonic = 6, amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x10,end=60,end_val=0x40,min_val=0,max_val=0x30},disappear = 880},
                    new Harmonic_Data{harmonic = 50, amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x30,end=60,end_val=0x30,min_val=0,max_val=0x30},disappear = 880},

                    new Harmonic_Data{harmonic = 18.34, amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x20,end=60,end_val=0x40,min_val=0,max_val=0x30},disappear = 880},
                    new Harmonic_Data{harmonic = 23.1,amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x20,end=60,end_val=0x40,min_val=0,max_val=0x30},disappear = 880},
                    new Harmonic_Data{harmonic = 70,amplitude = new Harmonic_Data_Amplitude{start=0,start_val=0x20,end=15,end_val=0x40,min_val=0,max_val=0x30},disappear = 880}
                };
                for(int harmonic = 0; harmonic < harmonics.Length; harmonic++)
                {
                    Harmonic_Data harmonic_data = harmonics[harmonic];

                    double harmonic_freq = harmonic_data.harmonic * control.get_Sine_Freq();

                    if (harmonic_freq > harmonic_data.disappear) continue;
                    double sine_val = sin(control.get_Sine_Time() * control.get_Sine_Angle_Freq() * harmonic_data.harmonic);

                    double amplitude = harmonic_data.amplitude.start_val + (harmonic_data.amplitude.end_val - harmonic_data.amplitude.start_val) / (harmonic_data.amplitude.end - harmonic_data.amplitude.start) * (control.get_Sine_Freq() - harmonic_data.amplitude.start);
                    if (amplitude > harmonic_data.amplitude.max_val) amplitude = harmonic_data.amplitude.max_val;
                    if (amplitude < harmonic_data.amplitude.min_val) amplitude = harmonic_data.amplitude.min_val;

                    double amplitude_disappear = (harmonic_freq + 100.0 > harmonic_data.disappear) ?
                        ((harmonic_data.disappear - harmonic_freq) / 100.0) : 1;

                    sine_val *= amplitude * amplitude_disappear;
                    sound_byte_int += Math.Round(sine_val);
                    total_sound_count++;
                }

                double[] saw_harmonics = new double[] { 3 };
                for(int harmonic = 0; harmonic < saw_harmonics.Length; harmonic++)
                {
                    double saw_val = sin(control.get_Saw_Time() * control.get_Saw_Angle_Freq() * harmonic);
                    saw_val *= 0x20;
                    sound_byte_int += Math.Round(saw_val);
                    total_sound_count++;
                }
                byte sound_byte = (byte)(sound_byte_int / total_sound_count + 0xFF/2);
                //sound_byte = (byte)((pwm_byte / 4 + sound_byte) / 2);

                writer.Write(sound_byte);
                sound_block_count++;
                loop = Check_For_Freq_Change(control);

            }



            writer.Seek(4, SeekOrigin.Begin);
            writer.Write(sound_block_count + 36);

            writer.Seek(40, SeekOrigin.Begin);
            writer.Write(sound_block_count);

            writer.Close();
        }
    }

}
