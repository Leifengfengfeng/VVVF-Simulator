using static VVVF_Simulator.my_math;

namespace VVVF_Simulator
{
    public class VVVF_Control_Values
    {
        public VVVF_Control_Values Clone()
        {
            return (VVVF_Control_Values)MemberwiseClone();
        }



        // variables for controlling parameters
        private int mascon_off_div = 18000;
        private bool do_frequency_change = true;
        private bool brake = false;
        private bool free_run = false;
        private double wave_stat = 0;
        private bool mascon_off = false;
        private int temp_count = 0;

        private bool allow_sine_time_change = true;
        private bool allow_random_freq_move = true;

        public void reset_control_variables()
        {
            do_frequency_change = true;
            brake = false;
            free_run = false;
            wave_stat = 0;
            mascon_off = false;
            temp_count = 0;
            allow_sine_time_change = true;
            allow_random_freq_move = true;
        }

        public double get_Control_Frequency() { return wave_stat; }
        public void set_Control_Frequency(double b) { wave_stat = b; }
        public void add_Control_Frequency(double b) { wave_stat += b; }

        public bool is_Mascon_Off() { return mascon_off; }
        public void set_Mascon_Off(bool b) { mascon_off = b; }
        public void toggle_Mascon_Off() { mascon_off = !mascon_off; }

        public bool is_Free_Running() { return free_run; }
        public void set_Free_Running(bool b) { free_run = b; }
        public void toggle_Free_Running() { free_run = !free_run; }

        public bool is_Braking() { return brake; }
        public void set_Braking(bool b) { brake = b; }
        public void toggle_Braking() { brake = !brake; }

        public void set_Mascon_Off_Div(int div) { mascon_off_div = div; }
        public int get_Mascon_Off_Div() { return mascon_off_div; }

        public bool is_Do_Freq_Change() { return do_frequency_change; }
        public void set_Do_Freq_Change(bool b) { do_frequency_change = b; }
        public void toggle_Do_Freq_Change() { do_frequency_change = !do_frequency_change; }

        public int get_Temp_Count() { return temp_count; }
        public void set_Temp_Count(int v) { temp_count = v; }

        public bool is_Allowed_Sine_Time_Change() { return allow_sine_time_change; }
        public void set_Allowed_Sine_Time_Change(bool b) { allow_sine_time_change = b; }

        public bool is_Allowed_Random_Freq_Move() { return allow_random_freq_move; }
        public void set_Allowed_Random_Freq_Move(bool b) { allow_random_freq_move = b; }


        //--- from vvvf wave calculate
        //sin value definitions
        private double sin_angle_freq = 0;
        private double sin_time = 0;
        //saw value definitions
        private double saw_angle_freq = 1050;
        private double saw_time = 0;
        private double pre_saw_random_freq = 0;
        private int random_freq_move_count = 0;
        private int vibrato_freq_move_count = 0;

        public void set_Sine_Angle_Freq(double b) { sin_angle_freq = b; }
        public double get_Sine_Angle_Freq() { return sin_angle_freq; }
        public void add_Sine_Angle_Freq(double b) { sin_angle_freq += b; }

        public double get_Sine_Freq() { return sin_angle_freq * M_1_2PI; }

        public void set_Sine_Time(double t) { sin_time = t; }
        public double get_Sine_Time() { return sin_time; }
        public void add_Sine_Time(double t) { sin_time += t; }
        public void multi_Sine_Time(double x) { sin_time *= x; }

        
        public void set_Saw_Angle_Freq(double f) { saw_angle_freq = f; }
        public double get_Saw_Angle_Freq() { return saw_angle_freq; }
        public void add_Saw_Angle_Freq(double f) { saw_angle_freq += f; }

        public void set_Saw_Time(double t) { saw_time = t; }
        public double get_Saw_Time() { return saw_time; }
        public void add_Saw_Time(double t) { saw_time += t; }
        public void multi_Saw_Time(double x) { saw_time *= x; }

        public void set_Pre_Saw_Random_Freq(double f) { pre_saw_random_freq = f; }
        public double get_Pre_Saw_Random_Freq() { return pre_saw_random_freq; }
        

        public void set_Random_Freq_Move_Count(int i) { random_freq_move_count = i; }
        public int get_Random_Freq_Move_Count() { return random_freq_move_count; }
        public void add_Random_Freq_Move_Count(int x) { random_freq_move_count += x; }

        public void set_Vibrato_Freq_Move_Count(int i) { vibrato_freq_move_count = i; }
        public int get_Vibrato_Freq_Move_Count() { return vibrato_freq_move_count; }
        public void add_Vibrato_Freq_Move_Count(int x) { vibrato_freq_move_count += x; }

        public void reset_all_variables()
        {
            sin_angle_freq = 0;
            sin_time = 0;

            saw_angle_freq = 1050;
            saw_time = 0;

            random_freq_move_count = 0;
        }
    }
}
