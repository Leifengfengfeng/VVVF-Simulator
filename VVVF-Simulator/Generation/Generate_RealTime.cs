using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using static VVVF_Simulator.vvvf_wave_calculate;
using static VVVF_Simulator.VVVF_Control_Values;
using static VVVF_Simulator.my_math;
using VVVF_Simulator.Yaml_VVVF_Sound;

namespace VVVF_Simulator.Generation
{
    public class Generate_RealTime
    {
        public static class RealTime_Parameter {
            public static double change_amount { get; set; } = 0;
            public static Boolean braking { get; set; } = false;
            public static Boolean quit { get; set; } = false;
            public static Boolean reselect { get; set; } = false;
            public static Boolean free_run { get; set; } = false;

            public static int buff_size { get; set; } = 20000;

            public static VVVF_Control_Values control_values { get; set; } = new();
            public static Yaml_Sound_Data sound_data { get; set; } = new();
        }
        private static int realtime_sound_calculate(BufferedWaveProvider provider, Yaml_Sound_Data sound_data, VVVF_Control_Values control)
        {
            while (true)
            {
                control.set_Braking(RealTime_Parameter.braking);
                control.set_Mascon_Off(RealTime_Parameter.free_run);

                double change_amo = RealTime_Parameter.change_amount;

                double sin_new_angle_freq = control.get_Sine_Angle_Freq();
                sin_new_angle_freq += change_amo;
                if (sin_new_angle_freq < 0) sin_new_angle_freq = 0;

                if (!control.is_Free_Running())
                {
                    if (control.is_Allowed_Sine_Time_Change())
                    {
                        if (sin_new_angle_freq != 0)
                        {
                            double amp = control.get_Sine_Angle_Freq() / sin_new_angle_freq;
                            control.multi_Sine_Time(amp);
                        }
                        else
                            control.set_Sine_Time(0);
                    }

                    control.set_Control_Frequency(control.get_Sine_Angle_Freq() / (M_2PI));
                    control.set_Sine_Angle_Freq(sin_new_angle_freq);
                }


                if (RealTime_Parameter.quit) return 0;
                else if (RealTime_Parameter.reselect) return 1;

                if (!control.is_Mascon_Off())
                {
                    if (!control.is_Free_Running())
                        control.set_Control_Frequency(control.get_Sine_Angle_Freq() / M_2PI);
                    else
                    {
                        control.add_Control_Frequency(M_2PI / (control.get_Mascon_Off_Div() / 24));

                        if (control.get_Sine_Angle_Freq() < control.get_Control_Frequency() * M_2PI)
                        {
                            control.set_Free_Running(false);
                            control.set_Control_Frequency(control.get_Sine_Angle_Freq() * M_1_2PI);
                        }
                        else
                        {
                            control.set_Free_Running(true);
                        }
                    }
                }
                else
                {
                    control.add_Control_Frequency(- M_2PI / (control.get_Mascon_Off_Div() / 24));
                    if (control.get_Control_Frequency() < 0)
                    {
                        control.set_Control_Frequency(0);
                    }
                    control.set_Free_Running(true);
                }

                byte[] add = new byte[20];

                for (int i = 0; i < 20; i++)
                {
                    control.add_Sine_Time(1.0 / 192000.0);
                    control.add_Saw_Time(1.0 / 192000.0);

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

                    double pwm_value = wv_U.pwm_value - wv_V.pwm_value;

                    byte sound_byte = 0xFF/2;
                    if (pwm_value == 2) sound_byte += 0x40;
                    else if (pwm_value == 1) sound_byte += 0x20;
                    else if (pwm_value == -1) sound_byte -= 0x20;
                    else if (pwm_value == -2) sound_byte -= 0x40;

                    add[i] = sound_byte;

                    RealTime_Parameter.control_values = control;
                }


                int bufsize = 20;

                provider.AddSamples(add, 0, bufsize);
                while (provider.BufferedBytes > RealTime_Parameter.buff_size) ;
            }
        }
        public static void realtime_sound(Yaml_Sound_Data ysd)
        {
            RealTime_Parameter.quit = false;
            RealTime_Parameter.sound_data = ysd;
            while (true)
            {

                var bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(192000, 8, 1));
                var mmDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                IWavePlayer wavPlayer = new WasapiOut(mmDevice, AudioClientShareMode.Shared, false, 50);

                wavPlayer.Init(bufferedWaveProvider);
                wavPlayer.Play();

                VVVF_Control_Values control = new();
                control.reset_all_variables();
                control.reset_control_variables();

                int stat;
                try
                {
                    stat = realtime_sound_calculate(bufferedWaveProvider, ysd, control);
                }
                catch
                {
                    wavPlayer.Stop();
                    wavPlayer.Dispose();

                    mmDevice.Dispose();
                    bufferedWaveProvider.ClearBuffer();

                    throw;
                }

                wavPlayer.Stop();
                wavPlayer.Dispose();

                mmDevice.Dispose();
                bufferedWaveProvider.ClearBuffer();

                if (stat == 0) break;
            }


        }
    }
}
