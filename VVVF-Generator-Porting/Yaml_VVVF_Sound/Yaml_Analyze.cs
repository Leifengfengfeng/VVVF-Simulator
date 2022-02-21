using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.Generation.Generate_Common;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_VVVF_Wave;

namespace VVVF_Generator_Porting.Yaml_VVVF_Sound
{

    public class Yaml_Sound_Data {
        private static String get_Value(Object o)
        {
            if (o == null) return "null";
            return o.ToString();
        }
        public int level { get; set; } = 2;
        public Yaml_Mascon_Data mascon_data { get; set; }
        public Yaml_Min_Sine_Freq min_freq { get; set; }
        public List<Yaml_Control_Data> accelerate_pattern { get; set; }
        public List<Yaml_Control_Data> braking_pattern { get; set; }

        public override string ToString()
        {
            String final = "[\r\n";
            final += "level : " + get_Value(level) + "\r\n";
            final += "mascon_data : " + get_Value(mascon_data) + "\r\n";
            final += "min_freq : " + get_Value(min_freq) + "\r\n";
            final += "accelerate_pattern : [";

            for(int i = 0; i < accelerate_pattern.Count; i++)
            {
                final += get_Value(accelerate_pattern[i]) + "\r\n";
            }
            final += "]";

            final += "braking_pattern : [";
            for (int i = 0; i < braking_pattern.Count; i++)
            {
                final += get_Value(braking_pattern[i]) + "\r\n";
            }
            final += "]\r\n";
            final += "]";
            return final;
        }

        public class Yaml_Mascon_Data
        {
            public Yaml_Mascon_Data_On_Off braking { get; set; }
            public Yaml_Mascon_Data_On_Off accelerating { get; set; }

            public override string ToString()
            {
                String final;
                final = "[\r\n";
                final += "braking : " + get_Value(braking) + "\r\n";
                final += "accelerate : " + get_Value(accelerating) + "\r\n";
                final += "]";

                return final;
            }

            public class Yaml_Mascon_Data_On_Off
            {
                public Yaml_Mascon_Data_Single on { get; set; }
                public Yaml_Mascon_Data_Single off { get; set; }

                public override string ToString()
                {
                    String final;
                    final = "[\r\n";
                    final += "on : " + get_Value(on) + "\r\n";
                    final += "off : " + get_Value(off) + "\r\n";
                    final += "]";
  
                    return final;
                }

                public class Yaml_Mascon_Data_Single
                {
                    public int div { get; set; } = 20000;
                    public double control_freq_go_to { get; set; } = 60;

                    public override string ToString()
                    {
                        String re = "[div : " + get_Value(div) + " , " + "control_freq_go_to : " + String.Format("{0:f3}", control_freq_go_to) + "]";
                        return re;
                    }
                }
            }

            
        }

        public class Yaml_Min_Sine_Freq {
            public double accelerate { get; set; }
            public double braking { get; set; }

            public override string ToString()
            {
                String final = "[";
                final += "accelerate : " + String.Format("{0:f3}", accelerate) + ",";
                final += "braking : " + String.Format("{0:f3}", braking);
                final += "]";
                return final;
            }
        }

        public class Yaml_Control_Data {
            public double from { get; set; } = -1;
            public bool enable_on_free_run { get; set; } = true;
            public bool enable_on_not_free_run { get; set; } = true;
            public Pulse_Mode pulse_Mode { get; set; }
            public Yaml_Free_Run_Condition when_freerun { get; set; }
            public Yaml_Control_Data_Amplitude_Control amplitude_control { get; set; }
            public Yaml_Async_Parameter async_data { get; set; }

            public override string ToString()
            {
                String change_line = "\r\n";
                String final = "From : " + String.Format("{0:f3}", from) + change_line;
                final += "enable_on_free_run : " + get_Value(enable_on_free_run) + change_line;
                final += "enable_on_not_free_run : " + get_Value(enable_on_not_free_run) + change_line;
                final += "PulseMode : " + get_Value(pulse_Mode) + change_line;
                final += "when_freerun : " + get_Value(when_freerun) + change_line;
                final += "amplitude_control : " + get_Value(amplitude_control) + change_line;
                final += "async_data : " + get_Value(async_data);
                return final;
            }

            public class Yaml_Moving_Value
            {
                public Moving_Value_Type type { get; set; } = Moving_Value_Type.Proportional;
                public double start { get; set; } = -1;
                public double start_value { get; set; } = -1;
                public double end { get; set; } = -1;
                public double end_value { get; set; } = -1;

                public enum Moving_Value_Type
                {
                    Proportional,Quadratic
                }

                public override string ToString()
                {
                    String final = "[";
                    final += "Type : " + type.ToString() + ",";
                    final += "Start : " + String.Format("{0:f3}", start) + ",";
                    final += "Start_Val : " + String.Format("{0:f3}", start_value) + ",";
                    final += "End : " + String.Format("{0:f3}", end) + ",";
                    final += "End_Val : " + String.Format("{0:f3}", end_value) + "]";
                    return final;
                }
            }
            public class Yaml_Free_Run_Condition
            {
                public Yaml_Free_Run_Condition_Single on { get; set; }
                public Yaml_Free_Run_Condition_Single off { get; set; }

                public override string ToString()
                {
                    String re = "[ " +
                             "on : " + get_Value(on) + " , " +
                             "off : " + get_Value(off) + " , " +
                        "]";
                    return re;
                }

                public class Yaml_Free_Run_Condition_Single {
                    public bool skip { get; set; } = false;
                    public bool stuck_at_here { get; set; } = false;
                    public override string ToString()
                    {
                        String re = "[ " +
                             "skip : " + get_Value(skip) + " , " +
                             "stuck_at_here : " + get_Value(stuck_at_here) + " , " +
                        "]";
                        return re;
                    }
                }

            }

            public class Yaml_Async_Parameter
            {
                public int random_range { get; set; } = 0;
                public Yaml_Async_Parameter_Carrier_Freq carrier_wave_data { get; set; }
                public Yaml_Async_Parameter_Dipolar dipoar_data { get; set; }

                public override string ToString()
                {
                    String re = "[ " +
                             "random_range : " + get_Value(random_range) + " , " +
                             "dipoar_data : " + get_Value(dipoar_data) + " , " +
                             "carrier_wave_data : " + get_Value(carrier_wave_data) +
                        "]";
                    return re;
                }

                public class Yaml_Async_Parameter_Carrier_Freq
                {
                    public Yaml_Async_Carrier_Mode carrier_mode { get; set; }
                    public double const_value { get; set; }
                    public Yaml_Moving_Value moving_value { get; set; }
                    public Yaml_Async_Parameter_Carrier_Freq_Vibrato vibrato_value { get; set; }
                    public Yaml_Async_Parameter_Carrier_Freq_Table carrier_table_value { get; set; }

                    public override string ToString()
                    {
                        String final = "[\r\n";
                        final += "carrier_mode : " + carrier_mode.ToString() + "\r\n";
                        final += "const_value : " + String.Format("{0:f3}", const_value) + "\r\n";
                        final += "moving_value : " + get_Value(moving_value) + "\r\n";
                        final += "vibrato_value : " + get_Value(vibrato_value) + "\r\n";
                        final += "carrier_table_value : " + get_Value(carrier_table_value) + "\r\n";
                        final += "]";
                        return final;
                    }

                    public enum Yaml_Async_Carrier_Mode
                    {
                        Const,Moving,Vibrato,Table
                    }

                    public class Yaml_Async_Parameter_Carrier_Freq_Vibrato
                    {
                        public Yaml_Async_Parameter_Vibrato_Value highest { get; set; }
                        public Yaml_Async_Parameter_Vibrato_Value lowest { get; set; }
                        public double interval { get; set; }

                        public override string ToString()
                        {
                            String final = "[\r\n";
                            final += "highest : " + get_Value(highest) + "\r\n";
                            final += "lowest : " + get_Value(lowest) + "\r\n";
                            final += "]";
                            return final;
                        }

                        public class Yaml_Async_Parameter_Vibrato_Value
                        {
                            public Yaml_Async_Parameter_Vibrato_Mode mode { get; set; }
                            public double const_value { get; set; }
                            public Yaml_Moving_Value moving_value { get; set; }
                            public override string ToString()
                            {
                                String final = "[\r\n";
                                final += "mode : " + mode.ToString() + "\r\n";
                                final += "const_value : " + String.Format("{0:f3}", const_value) + "\r\n";
                                final += "moving_value : " + get_Value(moving_value) + "\r\n";
                                final += "]";
                                return final;
                            }
                            public enum Yaml_Async_Parameter_Vibrato_Mode
                            {
                                Const, Moving
                            }
                        }
                    }
               
                    public class Yaml_Async_Parameter_Carrier_Freq_Table
                    {
                        public List<Yaml_Async_Parameter_Carrier_Freq_Table_Single> carrier_freq_table { get; set; }
                        public class Yaml_Async_Parameter_Carrier_Freq_Table_Single
                        {
                            public double from { get; set; }
                            public double carrier_freq { get; set; }
                            public bool free_run_stuck_here { get; set; }

                            public override string ToString()
                            {
                                String final = "[\r\n";
                                final += "from : " + String.Format("{0:f3}", from) + ",";
                                final += "carrier_freq : " + String.Format("{0:f3}", carrier_freq) + ",";
                                final += "free_run_stuck_here : " + get_Value(free_run_stuck_here) + ",";
                                final += "]";
                                return final;
                            }
                        }

                        public override string ToString()
                        {
                            String final = "[\r\n";
                            for(int i = 0; i < carrier_freq_table.Count; i++)
                            {
                                final += carrier_freq_table[i].ToString() + "\r\n";
                            }
                            final += "]";
                            return final;
                        }
                    }
                }
                public class Yaml_Async_Parameter_Dipolar
                {
                    public Yaml_Async_Parameter_Dipolar_Mode value_mode { get; set; }
                    public double const_value { get; set; }
                    public Yaml_Moving_Value moving_value { get; set; }
                    public override string ToString()
                    {
                        String final = "[\r\n";
                        final += "value_mode : " + value_mode.ToString() + "\r\n";
                        final += "const_value : " + String.Format("{0:f3}", const_value) + "\r\n";
                        final += "moving_value : " + get_Value(moving_value) + "\r\n";
                        final += "]";
                        return final;
                    }

                    public enum Yaml_Async_Parameter_Dipolar_Mode { 
                        Const,Moving
                    }


                }

            }

            public class Yaml_Control_Data_Amplitude_Control {
                public Yaml_Control_Data_Amplitude default_data { get; set; }
                public Yaml_Control_Data_Amplitude_Free_Run free_run_data { get; set; }

                public override string ToString()
                {
                    String re = "[ " +
                             "default_data : " + get_Value(default_data) + " , " +
                             "free_run_data : " + get_Value(free_run_data) +
                        "]";
                    return re;
                }

                public class Yaml_Control_Data_Amplitude_Free_Run
                {
                    public Yaml_Control_Data_Amplitude mascon_on { get; set; }
                    public Yaml_Control_Data_Amplitude mascon_off { get; set; }

                    public override string ToString()
                    {
                        String re = "[ " +
                                 "mascon_on : " + get_Value(mascon_on) + " , " +
                                 "mascon_off : " + get_Value(mascon_off) +
                            "]";
                        return re;
                    }
                }
                public class Yaml_Control_Data_Amplitude
                {
                    public Amplitude_Mode mode { get; set; }
                    public Yaml_Control_Data_Amplitude_Single_Parameter parameter { get; set; }

                    public override string ToString()
                    {
                        String re = "[ " +
                                 "Amplitude_Mode : " + get_Value(mode) + " , " +
                                 "parameter : " + get_Value(parameter) +
                            "]";
                        return re;
                    }

                    public class Yaml_Control_Data_Amplitude_Single_Parameter
                    {
                        public double start_freq { get; set; } = -1;
                        public double start_amp { get; set; } = -1;
                        public double end_freq { get; set; } = -1;
                        public double end_amp { get; set; } = -1;
                        public double curve_change_rate { get; set; } = 0;
                        public double cut_off_amp { get; set; } = -1;
                        public double max_amp { get; set; } = -1;
                        public bool disable_range_limit { get; set; } = false;
                        public int polynomial { get; set; } = 0;


                        public override string ToString()
                        {
                            String re = "[ " +  
                                 "start_freq : " + String.Format("{0:f3}", start_freq) + " , " +
                                 "start_amp : " + String.Format("{0:f3}", start_amp) + " , " +
                                 "end_freq : " + String.Format("{0:f3}", end_freq) + " , " +
                                 "end_amp : " + String.Format("{0:f3}", end_amp) + " , " +
                                 "curve_change_rate : " + String.Format("{0:f3}", curve_change_rate) + " , " +
                                 "cut_off_amp : " + String.Format("{0:f3}", cut_off_amp) + " , " +
                                 "max_amp : " + String.Format("{0:f3}", max_amp) + " , " +
                                 "disable_range_limit : " + get_Value(disable_range_limit) + " , " +
                                 "polynomial : " + polynomial.ToString()  + 

                            "]";
                            return re;
                        }
                    }
                }
            }
        }
    }

    public class Yaml_Analyze
    {
        public static Yaml_Sound_Data get_Deserialized(String path)
        {
            Yaml_Sound_Data deserializeObject;
            try
            {
                var input = new StreamReader(path, Encoding.UTF8);
                var deserializer = new Deserializer();
                deserializeObject = deserializer.Deserialize<Yaml_Sound_Data>(input);
                return deserializeObject;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid yaml or Invalid path");
                return null;
            }
        }
    }
}
