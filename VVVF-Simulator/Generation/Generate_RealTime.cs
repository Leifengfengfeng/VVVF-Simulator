using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using static VVVF_Simulator.vvvf_wave_calculate;
using static VVVF_Simulator.vvvf_wave_control;
using static VVVF_Simulator.my_math;
using VVVF_Simulator.Yaml_VVVF_Sound;

namespace VVVF_Simulator.Generation
{
    public class Generate_RealTime
    {

        private static int realtime_sound_calculate(BufferedWaveProvider provider, Yaml_Sound_Data sound_data)
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey();
                    ConsoleKey key = keyinfo.Key;

                    if (key.Equals(ConsoleKey.B))
                    {
                        set_Braking(!is_Braking()); ;
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
                        if (is_Allowed_Sine_Time_Change())
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
                        add_Control_Frequency(M_2PI / (get_Mascon_Off_Div() / 24));

                        if (get_Sine_Angle_Freq() < get_Control_Frequency() * M_2PI)
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
                    add_Control_Frequency(- M_2PI / (get_Mascon_Off_Div() / 24));
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

                int stat = realtime_sound_calculate(bufferedWaveProvider, Yaml_Generation.current_data);

                wavPlayer.Stop();

                if (stat == 0) break;
            }


        }
    }
}
