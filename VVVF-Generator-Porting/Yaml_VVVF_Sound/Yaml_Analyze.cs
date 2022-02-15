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
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Control_Data_Mascon_Control;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Mascon_Data;
using static VVVF_Generator_Porting.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Mascon_Data.Yaml_Mascon_Data_On_Off;

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
            String change_line = "\r\n";
            String re = "[ " + change_line +
                     "level : " + get_Value(level) + change_line +
                     "mascon_data : " + get_Value(mascon_data) + change_line +
                     "min_freq : " + get_Value(min_freq) + change_line +
                     "accelerate_pattern : [[";

            for(int i = 0; i < accelerate_pattern.Count; i++)
            {
                re += get_Value(accelerate_pattern[i]) + " , " + change_line;
            }
            re += "]" + change_line + "[";
            for (int i = 0; i < braking_pattern.Count; i++)
            {
                re += get_Value(braking_pattern[i]) + " , " + change_line;
            }
            re += "]" + change_line;
            re += "]";
            return re;
        }

        public class Yaml_Mascon_Data
        {
            public Yaml_Mascon_Data_On_Off braking { get; set; }
            public Yaml_Mascon_Data_On_Off accelerating { get; set; }

            public override string ToString()
            {
                String re = "[ " +
                         "braking : " + get_Value(braking) + " , " +
                         "accelerating : " + get_Value(accelerating) +
                    "]";
                return re;
            }

            public class Yaml_Mascon_Data_On_Off
            {
                public Yaml_Mascon_Data_Single on { get; set; }
                public Yaml_Mascon_Data_Single off { get; set; }

                public override string ToString()
                {
                    String re = "[ " +
                             "on : " + get_Value(on) + " , " +
                             "off : " + get_Value(off) +
                        "]";
                    return re;
                }

                public class Yaml_Mascon_Data_Single
                {
                    public int div { get; set; }
                    public int control_freq_go_to { get; set; }

                    public override string ToString()
                    {
                        String re = "[ " +
                                 "div : " + get_Value(div) + " , " +
                                 "control_freq_go_to : " + get_Value(control_freq_go_to) +
                            "]";
                        return re;
                    }
                }
            }

            
        }

        public class Yaml_Min_Sine_Freq {
            public int accelerate { get; set; }
            public int braking { get; set; }

            public override string ToString()
            {
                String re = "[ " +
                         "accelerate : " + get_Value(accelerate) + " , " +
                         "braking : " + get_Value(braking) +
                    "]";
                return re;
            }
        }

        public class Yaml_Control_Data {
            public int from { get; set; }
            public Pulse_Mode pulse_Mode { get; set; }
            public Yaml_Control_Data_Mascon_Control when_mascon_switched { get; set; }
            public Yaml_Control_Data_Amplitude_Control amplitude_control { get; set; }
            public Yaml_Async_Parameter async_data { get; set; }

            public override string ToString()
            {
                String change_line = "\r\n";
                String final = "From : " + get_Value(from) + change_line +
                    "PulseMode : " + get_Value(pulse_Mode) + change_line +
                    "when_mascon_switched : " + get_Value(when_mascon_switched) + change_line +
                    "amplitude_control : " + get_Value(amplitude_control) + change_line +
                    "async_data : " + get_Value(async_data);
                return final;
            }
            public class Yaml_Control_Data_Mascon_Control
            {
                public Yaml_Control_Data_Mascon_Control_Single on { get; set; }
                public Yaml_Control_Data_Mascon_Control_Single off { get; set; }

                public override string ToString()
                {
                    String re = "[ " +
                             "on : " + get_Value(on) + " , " +
                             "off : " + get_Value(off) + " , " +
                        "]";
                    return re;
                }

                public class Yaml_Control_Data_Mascon_Control_Single {
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
                    public double const_value { get; set; }
                    public Yaml_Async_Parameter_Carrier_Freq_Move moving_value { get; set; }
                    public Yaml_Async_Parameter_Carrier_Freq_Vibrato vibrato_value { get; set; }

                    public override string ToString()
                    {
                        String re = "[ " +
                                 "const_value : " + String.Format("{0:f3}", const_value) + " , " +
                                 "moving_value : " + get_Value(moving_value) + " , " +
                                 "vibrato_value : " + get_Value(vibrato_value) + " , " +
                            "]";
                        return re;
                    }

                    public class Yaml_Async_Parameter_Carrier_Freq_Move
                    {
                        double pos1 { get; set; }
                        double pos1_freq { get; set; }
                        double pos2 { get; set; }
                        double pos2_freq { get; set; }

                        public override string ToString()
                        {
                            String re = "[ " +
                                 "pos1 : " + String.Format("{0:f3}", pos1) + " , " +
                                 "pos1_freq : " + String.Format("{0:f3}", pos1_freq) + " , " +
                                 "pos2 : " + String.Format("{0:f3}", pos2) + " , " +
                                 "pos2_freq : " + String.Format("{0:f3}", pos2_freq) + " , " +

                            "]";
                            return re;
                        }
                    }

                    public class Yaml_Async_Parameter_Carrier_Freq_Vibrato
                    {
                        public Yaml_Async_Parameter_Carrier_Freq_Vibrato_Value highest { get; set; }
                        public Yaml_Async_Parameter_Carrier_Freq_Vibrato_Value lowest { get; set; }
                        public double interval { get; set; }

                        public override string ToString()
                        {
                            String re = "[ " +
                                         "interval : " + String.Format("{0:f3}", interval) + " , " +
                                         "lowest : " + get_Value(lowest) + " , " +
                                         "highest : " + get_Value(highest) + " , " +

                                    "]";
                            return re;
                        }

                        public class Yaml_Async_Parameter_Carrier_Freq_Vibrato_Value
                        {
                            public double const_value { get; set; }
                            public Yaml_Async_Parameter_Carrier_Freq_Vibrato_Value_Move moving_value { get; set; }
                            public override string ToString()
                            {
                                String re = "[ " +
                                         "const_value : " + String.Format("{0:f3}", const_value) + " , " +
                                         "moving_value : " + get_Value(moving_value) +

                                    "]";
                                return re;
                            }
                            public class Yaml_Async_Parameter_Carrier_Freq_Vibrato_Value_Move
                            {
                                double start_freq { get; set; }
                                double start_value { get; set; }
                                double end_freq { get; set; }
                                double end_value { get; set; }

                                public override string ToString()
                                {
                                    String re = "[ " +
                                         "start_freq : " + String.Format("{0:f3}", start_freq) + " , " +
                                         "start_value : " + String.Format("{0:f3}", start_value) + " , " +
                                         "end_freq : " + String.Format("{0:f3}", end_freq) + " , " +
                                         "end_value : " + String.Format("{0:f3}", end_value) + " , " +

                                    "]";
                                    return re;
                                }

                            }
                        }
                    }
                }
                public class Yaml_Async_Parameter_Dipolar
                {
                    public double const_value { get; set; }
                    public Yaml_Async_Parameter_Dipoar_Move move_value { get; set; }
                    public override string ToString()
                    {
                        String re = "[ " +
                                 "const_value : " + String.Format("{0:f3}", const_value) + " , " +
                                 "move_value : " + get_Value(move_value) +
                            "]";
                        return re;
                    }

                    public class Yaml_Async_Parameter_Dipoar_Move
                    {
                        public double start_freq { get; set; }
                        public double start_value { get; set; }
                        public double end_freq { get; set; }
                        public double end_value { get; set; }

                        public override string ToString()
                        {
                            String re = "[ " +
                                 "start_freq : " + String.Format("{0:f3}", start_freq) + " , " +
                                 "start_value : " + String.Format("{0:f3}", start_value) + " , " +
                                 "end_freq : " + String.Format("{0:f3}", end_freq) + " , " +
                                 "end_value : " + String.Format("{0:f3}", end_value) + " , " +

                            "]";
                            return re;
                        }
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
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },

                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },

                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },

                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },
                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                }

            };
            obj.braking_pattern = braking_data;

            List<Yaml_Control_Data> accelerate_data = new List<Yaml_Control_Data>()
            {
                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },

                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },

                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },

                new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
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
                        free_run_data = null
                    },
                },

                 new Yaml_Control_Data
                {
                    when_mascon_switched = new Yaml_Control_Data_Mascon_Control
                    {
                        on = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true },
                        off = new Yaml_Control_Data_Mascon_Control_Single{ skip = false, stuck_at_here = true }
                    },
                    from = 9,
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
                        free_run_data = null
                    },
                }

            };
            obj.accelerate_pattern = accelerate_data;

            // シリアライズ
            using TextWriter writer = File.CreateText(path);
            var serializer = new Serializer();
            serializer.Serialize(writer, obj);

            writer.Close();

            read_yaml(path);
        }

        public static void read_yaml(String path)
        {
            var input = new StreamReader(path, Encoding.UTF8);

            // デシリアライザインスタンス作成
            var deserializer = new Deserializer();

            // yamlデータのオブジェクトを作成
            Yaml_Sound_Data deserializeObject = deserializer.Deserialize<Yaml_Sound_Data>(input);

            Console.WriteLine(deserializeObject.ToString());

        }
    }
}
