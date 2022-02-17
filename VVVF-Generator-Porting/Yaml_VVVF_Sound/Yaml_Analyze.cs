using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Control_Data_Amplitude_Control;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Control_Data_Amplitude_Control.Yaml_Control_Data_Amplitude;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Free_Run_Condition;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Mascon_Data;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Mascon_Data.Yaml_Mascon_Data_On_Off;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.Generation.Generate_Common;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_VVVF_Wave;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Table;

namespace VVVF_Generator_Porting.Yaml_VVVF_Sound
{

    public class Yaml_Sound_Data {
        private static String get_Value(Object o)
        {
            if (o == null) return "null";
            return o.ToString();
        }
        public int level { get; set; }
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
                    public int div { get; set; }
                    public double control_freq_go_to { get; set; }

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
            public double from { get; set; }
            public bool enable_on_free_run { get; set; }
            public bool enable_on_not_free_run { get; set; }
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
                public Moving_Value_Type type { get; set; }
                public double start { get; set; }
                public double start_value { get; set; }
                public double end { get; set; }
                public double end_value { get; set; }

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
                    public bool skip { get; set; }
                    public bool stuck_at_here { get; set; }
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
                public int random_range { get; set; }
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
                            publicbool free_run_stuck_here { get; set; }

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
                        public double start_freq { get; set; }
                        public double start_amp { get; set; }
                        public double end_freq { get; set; }
                        public double end_amp { get; set; }
                        public double curve_change_rate { get; set; }
                        public double cut_off_amp { get; set; }
                        public bool disable_range_limit { get; set; }
                        public int polynomial { get; set; }

                        public override string ToString()
                        {
                            String re = "[ " +  
                                 "start_freq : " + String.Format("{0:f3}", start_freq) + " , " +
                                 "start_amp : " + String.Format("{0:f3}", start_amp) + " , " +
                                 "end_freq : " + String.Format("{0:f3}", end_freq) + " , " +
                                 "end_amp : " + String.Format("{0:f3}", end_amp) + " , " +
                                 "curve_change_rate : " + String.Format("{0:f3}", curve_change_rate) + " , " +
                                 "cut_off_amp : " + String.Format("{0:f3}", cut_off_amp) + " , " +
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
        public static void output_yaml(String path)
        {
            Yaml_Sound_Data obj = new Yaml_Sound_Data();
            obj.level = 2;

            Yaml_Min_Sine_Freq ymsf = new Yaml_Min_Sine_Freq
            {
                accelerate = 0,
                braking = 0
            };
            obj.min_freq = ymsf;

            Yaml_Mascon_Data ymd = new Yaml_Mascon_Data
            {
                accelerating = new Yaml_Mascon_Data_On_Off
                {
                    on = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 80,
                        div = 12000
                    },
                    off = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 80,
                        div = 12000
                    }
                },
                braking = new Yaml_Mascon_Data_On_Off
                {
                    on = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 79.5,
                        div = 20000
                    },
                    off = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 79.5,
                        div = 20000
                    }
                }
            };
            obj.mascon_data = ymd;

            List<Yaml_Control_Data> braking_data = new List<Yaml_Control_Data>()
            {
                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = false },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = false }
                    },
                    from = 79.5,
                    pulse_Mode = Pulse_Mode.P_1,
                    async_data= null,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_freq = 60,
                                start_amp = 1,
                                end_freq = 72,
                                end_amp = 1,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = false },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = false }
                    },
                    from = 70.7,
                    pulse_Mode = Pulse_Mode.CHMP_Wide_3,
                    async_data= null,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 68.5, 1, cv.wave_stat, false
                                start_freq = 70.7,
                                start_amp = 1.2102388,
                                end_freq = 79.5,
                                end_amp = 0.018464 * 68.5 -0.095166,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 63.35,
                    pulse_Mode = Pulse_Mode.P_3,
                    async_data= null,
                    enable_on_free_run = true,
                    enable_on_not_free_run = false,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 79.5,
                                end_amp = 1,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 63.35,
                    pulse_Mode = Pulse_Mode.CHMP_Wide_5,
                    enable_on_not_free_run = true,
                    enable_on_free_run = false,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 58, 1.02, cv.wave_stat, false
                                start_freq = 63.35,
                                start_amp = 0.018464 * 63.35 -0.095166,
                                end_freq = 70.7,
                                end_amp = 0.018464 * 70.7 -0.095166,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 58, 1.02, cv.wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 70.7,
                                end_amp = 0.018464 * 70.7 -0.095166,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 58, 1.02, cv.wave_stat, false
                                start_freq =0,
                                start_amp = 06,
                                end_freq = 70.7,
                                end_amp = 0.018464 * 70.7 -0.095166,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 56.84,
                    pulse_Mode = Pulse_Mode.CHMP_Wide_7,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 56.84,
                                start_amp = 0.018464 * 56.84 -0.095166,
                                end_freq = 63.35,
                                end_amp = 0.018464 * 63.35 -0.095166,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 63.35,
                                end_amp = 0.018464 * 63.35 -0.095166,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 63.35,
                                end_amp = 0.018464 * 63.35 -0.095166,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 53.5,
                    pulse_Mode = Pulse_Mode.CHMP_7,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 53.5,
                                start_amp = 0.014763975813 * 53.5 + 0.10000000000,
                                end_freq = 56.84,
                                end_amp = 0.014763975813 * 56.84 + 0.10000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 56.84,
                                end_amp = 0.014763975813 * 56.84 + 0.10000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 56.84,
                                end_amp = 0.014763975813 * 56.84 + 0.10000000000,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 41,
                    pulse_Mode = Pulse_Mode.CHMP_7,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 41,
                                start_amp = 0.013504901961 * 41 + 0.100000000000,
                                end_freq = 53.5,
                                end_amp = 0.013504901961 * 53.5 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 53.5,
                                end_amp = 0.013504901961 * 53.5 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 53.5,
                                end_amp = 0.013504901961 * 53.5 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 34.5,
                    pulse_Mode = Pulse_Mode.CHMP_9,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 34.5,
                                start_amp = 0.013504901961 * 34.5 + 0.100000000000,
                                end_freq = 41,
                                end_amp = 0.013504901961 * 41 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 41,
                                end_amp = 0.013504901961 * 41 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 41,
                                end_amp = 0.013504901961 * 41 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 28.9,
                    pulse_Mode = Pulse_Mode.CHMP_11,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 28.9,
                                start_amp = 0.013504901961 * 28.9 + 0.100000000000,
                                end_freq = 34.5,
                                end_amp = 0.013504901961 * 34.5 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 34.5,
                                end_amp = 0.013504901961 * 34.5 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 34.5,
                                end_amp = 0.013504901961 * 34.5 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },
                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 24.9,
                    pulse_Mode = Pulse_Mode.CHMP_13,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 24.9,
                                start_amp = 0.013504901961 * 24.9 + 0.100000000000,
                                end_freq = 28.9,
                                end_amp = 0.013504901961 * 28.9 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 28.9,
                                end_amp = 0.013504901961 * 28.9 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 28.9,
                                end_amp = 0.013504901961 * 28.9 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 22.4,
                    pulse_Mode = Pulse_Mode.CHMP_15,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 22.4,
                                start_amp = 0.013504901961 * 22.4 + 0.100000000000,
                                end_freq = 24.9,
                                end_amp = 0.013504901961 * 24.9 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 24.9,
                                end_amp = 0.013504901961 * 24.9 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            },
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 24.9,
                                end_amp = 0.013504901961 * 24.9 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },

                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 4,
                    pulse_Mode = Pulse_Mode.Async_THI,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= new Yaml_Async_Parameter
                    {
                        dipoar_data = null,
                        random_range = 0,
                        carrier_wave_data = new Yaml_Async_Parameter_Carrier_Freq{
                            carrier_mode = Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Carrier_Mode.Const,
                            const_value = 400,
                            moving_value = null,
                            vibrato_value = null,
                        }
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 4,
                                start_amp = 0.013504901961 * 4 + 0.100000000000,
                                end_freq = 22.4,
                                end_amp = 0.013504901961 * 22.4 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 22.4,
                                end_amp = 0.013504901961 * 22.4 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 22.4,
                                end_amp = 0.013504901961 * 22.4 + 0.100000000000,
                                curve_change_rate = -1,
                                cut_off_amp = 0.498,
                                disable_range_limit = false
                            }
                            }
                        }
                    },
                },


                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = -1,
                    pulse_Mode = Pulse_Mode.Async_THI,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    async_data= new Yaml_Async_Parameter
                    {
                        dipoar_data = null,
                        random_range = 0,
                        carrier_wave_data = new Yaml_Async_Parameter_Carrier_Freq{
                            carrier_mode = Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Carrier_Mode.Const,
                            const_value = 400,
                            moving_value = null,
                            vibrato_value = null,
                        }
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = -1,
                                start_amp = 0,
                                end_freq = 20,
                                end_amp = 0,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                    },
                },

            };
            obj.braking_pattern = braking_data;

            List<Yaml_Control_Data> accelerate_data = new List<Yaml_Control_Data>()
            {
                new Yaml_Control_Data
                {
                    from = 80,
                    pulse_Mode = Pulse_Mode.P_1,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = false },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = false },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 1,
                                start_freq = 0,
                                end_amp = 1,
                                end_freq = 10,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },


                new Yaml_Control_Data
                {
                    from = 59,
                    pulse_Mode = Pulse_Mode.CHMP_Wide_3,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = false },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = false },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0222656250000 * 59 -0.07467187500,
                                start_freq = 59,
                                end_amp = 0.0222656250000 * 80 -0.07467187500,
                                end_freq = 80,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 57,
                    pulse_Mode = Pulse_Mode.P_3,
                    enable_on_free_run = true,
                    enable_on_not_free_run = false,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0,
                                start_freq = 0,
                                end_amp = 1,
                                end_freq = 80,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 57,
                    pulse_Mode = Pulse_Mode.CHMP_Wide_5,
                    enable_on_free_run = false,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0222656250000 * 57 -0.07467187500,
                                start_freq = 57,
                                end_amp = 0.0222656250000 * 59 -0.07467187500,
                                end_freq = 59,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 53.5,
                    pulse_Mode = Pulse_Mode.CHMP_Wide_7,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0222656250000 * 53.5 -0.07467187500,
                                start_freq = 53.5,
                                end_amp = 0.0222656250000 * 57 -0.07467187500,
                                end_freq = 57,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 43.5,
                    pulse_Mode = Pulse_Mode.CHMP_7,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0193294460641 * 43.5 + 0.10000000000,
                                start_freq = 43.5,
                                end_amp = 0.0193294460641 * 53.5 + 0.10000000000,
                                end_freq = 53.5,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 36.7,
                    pulse_Mode = Pulse_Mode.CHMP_9,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0193294460641 * 36.7 + 0.10000000000,
                                start_freq = 36.7,
                                end_amp = 0.0193294460641 * 43.5 + 0.10000000000,
                                end_freq = 43.5,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 30,
                    pulse_Mode = Pulse_Mode.CHMP_11,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0193294460641 * 30 + 0.10000000000,
                                start_freq = 30,
                                end_amp = 0.0193294460641 * 36.7 + 0.10000000000,
                                end_freq = 36.7,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 27,
                    pulse_Mode = Pulse_Mode.CHMP_13,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0193294460641 * 27 + 0.10000000000,
                                start_freq = 27,
                                end_amp = 0.0193294460641 * 30 + 0.10000000000,
                                end_freq = 30,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 24,
                    pulse_Mode = Pulse_Mode.CHMP_15,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0193294460641 * 24 + 0.10000000000,
                                start_freq = 24,
                                end_amp = 0.0193294460641 * 27 + 0.10000000000,
                                end_freq = 27,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = null
                },

                new Yaml_Control_Data
                {
                    from = 0,
                    pulse_Mode = Pulse_Mode.Async_THI,
                    enable_on_free_run = true,
                    enable_on_not_free_run = true,
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false , stuck_at_here = true },
                    },
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_amp = 0.0193294460641 * 0 + 0.10000000000,
                                start_freq = 0,
                                end_amp = 0.0193294460641 * 24 + 0.10000000000,
                                end_freq = 24,
                                curve_change_rate = -1,
                                cut_off_amp = -1,
                                disable_range_limit = false,
                                polynomial = -1
                            }
                        }
                    },
                    async_data = new Yaml_Async_Parameter
                    {
                        dipoar_data = null,
                        random_range = 0,
                        carrier_wave_data = new Yaml_Async_Parameter_Carrier_Freq
                        {
                            carrier_mode = Yaml_Async_Carrier_Mode.Table,
                            const_value = -1,
                            moving_value = null,
                            vibrato_value = null,
                            carrier_table_value = new Yaml_Async_Parameter_Carrier_Freq_Table
                            {
                                carrier_freq_table = new List<Yaml_Async_Parameter_Carrier_Freq_Table_Single>()
                                {
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 400, from = 5.6 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 350, from = 5 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 311, from = 4.3 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 294, from = 3.4 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 262, from = 2.7 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 233, from = 2.0 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 223, from = 1.5 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 196, from = 0.5 },
                                    new Yaml_Async_Parameter_Carrier_Freq_Table_Single{ carrier_freq = 175, from = 0 },
                                }
                            }
                        }
                    }
                }

            };
            obj.accelerate_pattern = accelerate_data;

            // シリアライズ
            using TextWriter writer = File.CreateText(path);
            var serializer = new Serializer();
            serializer.Serialize(writer, obj);

            writer.Close();

            yaml_generate_sound(System.IO.Path.GetDirectoryName(path),path);
        }

        public static void yaml_generate_sound(String output_path,String path)
        {
            
            Yaml_Sound_Data deserializeObject;
            try
            {
                var input = new StreamReader(path, Encoding.UTF8);
                var deserializer = new Deserializer();
                deserializeObject = deserializer.Deserialize<Yaml_Sound_Data>(input);
            }catch(Exception)
            {
                Console.WriteLine("Invalid yaml or Invalid path");
                return;
            }

            reset_control_variables();
            reset_all_variables();

            Int32 sound_block_count = 0;
            DateTime dt = DateTime.Now;
            String gen_time = dt.ToString("yyyy-MM-dd_HH-mm-ss");
             String fileName = output_path + "\\" + gen_time + ".wav";

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
                Wave_Values wv_U = calculate_yaml_go(cv_U, deserializeObject);

                Control_Values cv_V = new Control_Values
                {
                    brake = is_Braking(),
                    mascon_on = !is_Mascon_Off(),
                    free_run = is_Free_Running(),
                    initial_phase = Math.PI * 2.0 / 3.0 * 1,
                    wave_stat = get_Control_Frequency()
                };
                Wave_Values wv_V = calculate_yaml_go(cv_V, deserializeObject);

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
