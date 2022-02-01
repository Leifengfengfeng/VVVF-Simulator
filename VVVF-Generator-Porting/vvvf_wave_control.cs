namespace VVVF_Generator_Porting
{
    public class vvvf_wave_control
    {
        // variables for controlling parameters
        private static int mascon_off_div = 18000;
        private static bool do_frequency_change = true;
        private static bool brake = false;
        private static bool free_run = false;
        private static double wave_stat = 0;
        private static bool mascon_off = false;
        private static int temp_count = 0;

        public static void reset_control_variables()
        {
            do_frequency_change = true;
            brake = false;
            free_run = false;
            wave_stat = 0;
            mascon_off = false;
            temp_count = 0;
        }

        public static double get_Control_Frequency() { return wave_stat; }
        public static void set_Control_Frequency(double b) { wave_stat = b; }
        public static void add_Control_Frequency(double b) { wave_stat += b; }

        public static bool is_Mascon_Off() { return mascon_off; }
        public static void set_Mascon_Off(bool b) { mascon_off = b; }
        public static void toggle_Mascon_Off() { mascon_off = !mascon_off; }

        public static bool is_Free_Running() { return free_run; }
        public static void set_Free_Running(bool b) { free_run = b; }
        public static void toggle_Free_Running() { free_run = !free_run; }

        public static bool is_Braking() { return brake; }
        public static void set_Braking(bool b) { brake = b; }
        public static void toggle_Braking() { brake = !brake; }

        public static void set_Mascon_Off_Div(int div) { mascon_off_div = div; }
        public static int get_Mascon_Off_Div() { return mascon_off_div; }

        public static bool is_Do_Freq_Change() { return do_frequency_change; }
        public static void set_Do_Freq_Change(bool b) { do_frequency_change = b; }
        public static void toggle_Do_Freq_Change() { do_frequency_change = !do_frequency_change; }

        public static int get_Temp_Count() { return temp_count; }
        public static void set_Temp_Count(int v) { temp_count = v; }


        //--- from vvvf wave calculate
        //sin value definitions
        private static double sin_angle_freq = 0;
        private static double sin_time = 0;
        //saw value definitions
        private static double saw_angle_freq = 1050;
        private static double saw_time = 0;
        private static double pre_saw_random_freq = 0;
        private static int random_freq_move_count = 0;
        
        public static void set_Sine_Angle_Freq(double b) { sin_angle_freq = b; }
        public static double get_Sine_Angle_Freq() { return sin_angle_freq; }
        public static void add_Sine_Angle_Freq(double b) { sin_angle_freq += b; }

        public static void set_Sine_Time(double t) { sin_time = t; }
        public static double get_Sine_Time() { return sin_time; }
        public static void add_Sine_Time(double t) { sin_time += t; }
        public static void multi_Sine_Time(double x) { sin_time *= x; }

        
        public static void set_Saw_Angle_Freq(double f) { saw_angle_freq = f; }
        public static double get_Saw_Angle_Freq() { return saw_angle_freq; }
        public static void add_Saw_Angle_Freq(double f) { saw_angle_freq += f; }

        public static void set_Saw_Time(double t) { saw_time = t; }
        public static double get_Saw_Time() { return saw_time; }
        public static void add_Saw_Time(double t) { saw_time += t; }
        public static void multi_Saw_Time(double x) { saw_time *= x; }

        public static void set_Pre_Saw_Random_Freq(double f) { pre_saw_random_freq = f; }
        public static double get_Pre_Saw_Random_Freq() { return pre_saw_random_freq; }
        

        public static void set_Random_Freq_Move_Count(int i) { random_freq_move_count = i; }
        public static int get_Random_Freq_Move_Count() { return random_freq_move_count; }
        public static void add_Random_Freq_Move_Count(int x) { random_freq_move_count += x; }

        public static void reset_all_variables()
        {
            sin_angle_freq = 0;
            sin_time = 0;

            saw_angle_freq = 1050;
            saw_time = 0;

            random_freq_move_count = 0;
        }
    }
}
