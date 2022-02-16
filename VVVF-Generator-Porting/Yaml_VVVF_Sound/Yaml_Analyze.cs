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
                    public int control_freq_go_to { get; set; }

                    public override string ToString()
                    {
                        String re = "[div : " + get_Value(div) + " , " + "control_freq_go_to : " + get_Value(control_freq_go_to) + "]";
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
            public int from { get; set; }
            public Pulse_Mode pulse_Mode { get; set; }
            public Yaml_Free_Run_Condition when_freerun { get; set; }
            public Yaml_Control_Data_Amplitude_Control amplitude_control { get; set; }
            public Yaml_Async_Parameter async_data { get; set; }

            public override string ToString()
            {
                String change_line = "\r\n";
                String final = "From : " + get_Value(from) + change_line +
                    "PulseMode : " + get_Value(pulse_Mode) + change_line +
                    "when_freerun : " + get_Value(when_freerun) + change_line +
                    "amplitude_control : " + get_Value(amplitude_control) + change_line +
                    "async_data : " + get_Value(async_data);
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

                    public override string ToString()
                    {
                        String final = "[\r\n";
                        final += "carrier_mode : " + carrier_mode.ToString() + "\r\n";
                        final += "const_value : " + String.Format("{0:f3}", const_value) + "\r\n";
                        final += "moving_value : " + get_Value(moving_value) + "\r\n";
                        final += "vibrato_value : " + get_Value(vibrato_value) + "\r\n";
                        final += "]";
                        return final;
                    }

                    public enum Yaml_Async_Carrier_Mode
                    {
                        Const,Moving,Vibrato
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
            obj.level = 3;

            Yaml_Min_Sine_Freq ymsf = new Yaml_Min_Sine_Freq
            {
                accelerate = 2,
                braking = -1
            };
            obj.min_freq = ymsf;

            Yaml_Mascon_Data ymd = new Yaml_Mascon_Data
            {
                accelerating = new Yaml_Mascon_Data_On_Off
                {
                    on = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 53,
                        div = 19100
                    },
                    off = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 53,
                        div = 40000
                    }
                },
                braking = new Yaml_Mascon_Data_On_Off
                {
                    on = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 60,
                        div = 19100
                    },
                    off = new Yaml_Mascon_Data_Single
                    {
                        control_freq_go_to = 60,
                        div = 40000
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
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 60,
                    pulse_Mode = Pulse_Mode.P_1,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Inv_Proportional,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_freq = 60,
                                start_amp = 0.6,
                                end_freq = 72,
                                end_amp = 3,
                                curve_change_rate = 0,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.8, false
                                mode = Amplitude_Mode.Inv_Proportional,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    start_freq = 0,
                                    start_amp = 0.0001,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = 0.8,
                                    cut_off_amp = 0.001,
                                    disable_range_limit = false
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Inv_Proportional,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    start_freq = 0,
                                    start_amp = 0.0001,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = 0.06,
                                    cut_off_amp = 0.1,
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
                    from = 49,
                    pulse_Mode = Pulse_Mode.P_3,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 68.5, 1, cv.wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 68.5,
                                end_amp = 1,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    from = 40,
                    pulse_Mode = Pulse_Mode.P_9,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //40, 0.725, 49, 0.79, cv.wave_stat, false
                                start_freq = 40,
                                start_amp = 0.725,
                                end_freq = 59,
                                end_amp = 0.79,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    from = 19,
                    pulse_Mode = Pulse_Mode.P_21,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 58, 1.02, cv.wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 58,
                                end_amp = 1.02,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    from = 7,
                    pulse_Mode = Pulse_Mode.P_33,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 19,
                                end_amp = 0.34,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    from = 0,
                    pulse_Mode = Pulse_Mode.P_33,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 19, 0.34, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 0,
                                end_amp = 100,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            }
                        }
                    },
                }

            };
            obj.braking_pattern = braking_data;

            List<Yaml_Control_Data> accelerate_data = new List<Yaml_Control_Data>()
            {
                new Yaml_Control_Data
                {
                    when_freerun = new Yaml_Free_Run_Condition
                    {
                        on = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Free_Run_Condition_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 53,
                    pulse_Mode = Pulse_Mode.P_1,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            //53, 0.49, 66, 3, cv.wave_stat, 0, false
                            mode = Amplitude_Mode.Inv_Proportional,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                start_freq = 53,
                                start_amp = 0.49,
                                end_freq = 66,
                                end_amp = 3,
                                curve_change_rate = 0,
                                cut_off_amp = -1,
                                disable_range_limit = false
                            }
                        },
                        free_run_data = new Yaml_Control_Data_Amplitude_Free_Run
                        {
                            mascon_on = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.8, false
                                mode = Amplitude_Mode.Inv_Proportional,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    start_freq = 0,
                                    start_amp = 0.0001,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = 0.8,
                                    cut_off_amp = 0.001,
                                    disable_range_limit = false
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Inv_Proportional,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    start_freq = 0,
                                    start_amp = 0.0001,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = 0.06,
                                    cut_off_amp = 0.1,
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
                    from = 45,
                    pulse_Mode = Pulse_Mode.P_3,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //45, 0.7, 53, 0.84, cv.wave_stat, false
                                start_freq = 45,
                                start_amp = 0.7,
                                end_freq = 53,
                                end_amp = 0.84,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    from = 29,
                    pulse_Mode = Pulse_Mode.P_9,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 59, 1, cv.wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 59,
                                end_amp = 1,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    from = 19,
                    pulse_Mode = Pulse_Mode.P_21,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 59, 1, cv.wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 59,
                                end_amp = 1,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    from = 9,
                    pulse_Mode = Pulse_Mode.P_33,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0, 59, 1, cv.wave_stat, false
                                start_freq = 0,
                                start_amp = 0,
                                end_freq = 59,
                                end_amp = 1,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
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
                    pulse_Mode = Pulse_Mode.P_57,
                    async_data= null,
                    amplitude_control = new Yaml_Control_Data_Amplitude_Control
                    {
                        default_data = new Yaml_Control_Data_Amplitude
                        {
                            mode = Amplitude_Mode.Linear,
                            parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                            {
                                //0, 0.02, 59, 1, original_wave_stat, false
                                start_freq = 0,
                                start_amp = 0.02,
                                end_freq = 59,
                                end_amp = 1,
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
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            },
                            mascon_off = new Yaml_Control_Data_Amplitude
                            {
                                //0, 0.0001, target_freq, target_amplitude, cv.wave_stat, 0.06, false
                                mode = Amplitude_Mode.Linear,
                                parameter = new Yaml_Control_Data_Amplitude_Single_Parameter
                                {
                                    //45, 0.7, 53, 0.84, cv.wave_stat, false
                                    start_freq = 0,
                                    start_amp = 0,
                                    end_freq = -1,
                                    end_amp = -1,
                                    curve_change_rate = -1,
                                    cut_off_amp = -1,
                                    disable_range_limit = false,
                                    polynomial = -1
                                }
                            }
                        }
                    },
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
