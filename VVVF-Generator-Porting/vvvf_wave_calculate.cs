using static VVVF_Generator_Porting.Program;

namespace VVVF_Generator_Porting
{
	public class vvvf_wave_calculate
	{
		public static double M_2PI = 6.283185307179586476925286766559;
		public static double M_PI = 3.1415926535897932384626433832795;
		public static double M_PI_2 = 1.5707963267948966192313216916398;
		public static double M_2_PI = 0.63661977236758134307553505349006;
		public static double M_1_PI = 0.31830988618379067153776752674503;
		public static double M_1_2PI = 0.15915494309189533576888376337251;
		public static double M_PI_180 = 0.01745329251994329576923690768489;
		public static double M_PI_4 = 0.78539816339744830961566084581988;

		public struct Wave_Values
		{
			public double sin_value;
			public double saw_value;
			public int pwm_value;
		};

		public static Wave_Values get_Wave_Values_None()
		{
			Wave_Values wv;
			wv.sin_value = 0;
			wv.saw_value = 0;
			wv.pwm_value = 0;
			return wv;
		}
		public struct Control_Values
		{
			public bool brake;
			public bool mascon_on;
			public bool free_run;
			public double initial_phase;
			public double wave_stat;
		};
		public enum Pulse_Mode
		{
			Not_In_Sync, P_1, P_Wide_3, P_10, P_12, P_18,
			P_3, P_5, P_7, P_9, P_11, P_13, P_15, P_17, P_19,
			P_21, P_23, P_25, P_27, P_29, P_31, P_33, P_35, P_37, P_39, P_41
			, P_43, P_45, P_47, P_49, P_51, P_53, P_55, P_57, P_59, P_61,

			SP_1, SP_Wide_3, SP_10, SP_12, SP_18,
			SP_3, SP_5, SP_7, SP_9, SP_11, SP_13, SP_15, SP_17, SP_19,
			SP_21, SP_23, SP_25, SP_27, SP_29, SP_31, SP_33, SP_35, SP_37, SP_39, SP_41
			, SP_43, SP_45, SP_47, SP_49, SP_51, SP_53, SP_55, SP_57, SP_59, SP_61

			, Asyn_THI, CHMP_3, CHMP_Wide_3, CHMP_5, CHMP_Wide_5, CHMP_7, CHMP_Wide_7, CHMP_9, CHMP_Wide_9, CHMP_11, CHMP_13, CHMP_15

			, SHEP_3, SHEP_5, SHEP_7, SHEP_9, SHEP_11, SHEP_13, SHEP_15
		};

		public enum VVVF_Sound_Names
		{
			SOUND_JRE_209_MITSUBISHI_GTO_3_LEVEL,
			SOUND_JRE_E231_MITSUBISHI_IGBT_3_LEVEL,
			SOUND_JRE_E231_1000_HITACHI_IGBT_2_LEVEL,
			SOUND_JRE_E233_MITSUBISHI_IGBT_2_LEVEL,
			SOUND_JRE_E233_3000_HITACHI_IGBT_2_LEVEL,
			SOUND_JRE_E235_TOSHIBA_SIC_2_LEVEL,
			SOUND_JRE_E235_MITSUBISHI_SIC_2_LEVEL,

			SOUND_JRW_207_TOSHIBA_GTO_2_LEVEL,
			SOUND_JRW_207_UPDATE_TOSHIBA_IGBT_2_LEVEL,
			SOUND_JRW_223_2000_HITACHI_IGBT_3_LEVEL,
			SOUND_JRW_321_HITACHI_IGBT_2_LEVEL,
			SOUND_JRW_225_5100_MITSUBISHI_IGBT_2_LEVEL,

			SOUND_TOKYUU_9000_HITACHI_GTO_2_LEVEL,
			SOUND_TOKYUU_5000_HITACHI_IGBT_2_LEVEL,
			SOUND_TOKYUU_1000_1500_UPDATE_TOSHIBA_IGBT_2_LEVEL,

			SOUND_KINTETSU_5800_MITSUBISHI_GTO_2_LEVEL,
			SOUND_KINTETSU_9820_MITSUBISHI_IGBT_2_LEVEL,
			SOUND_KINTETSU_9820_HITACHI_IGBT_2_LEVEL,

			SOUND_KEIO_8000_HITACHI_GTO_2_LEVEL,

			SOUND_KEIKYU_N1000_SIEMENS_GTO_2_LEVEL,
			SOUND_KEIKYU_N1000_SIEMENS_IGBT_2_LEVEL,

			SOUND_TOUBU_50050_HITACHI_IGBT_2_LEVEL,

			SOUND_KYOTO_SUBWAY_50_MITSUBISHI_GTO_2_LEVEL,

			SOUND_NAGOYA_SUBWAY_2000_UPDATE_HITACHI_GTO_2_LEVEL,

			SOUND_KEIHAN_13000_TOYO_IGBT_2_LEVEL,

			SOUND_TOEI_6300_MITSUBISHI_IGBT_2_LEVEL,

			SOUND_WMATA_6000_ALSTOM_IGBT_2_LEVEL,
			SOUND_WMATA_7000_TOSHIBA_IGBT_2_LEVEL,

			SOUND_X_TOYO_GTO_2_LEVEL,
			SOUND_X_TOYO_IGBT_2_LEVEL,
			SOUND_X_SILENT_2_LEVEL,
			SOUND_JRE_209_MITSUBISHI_GTO_2_LEVEL,
			SOUND_X_FAMINA_2_LEVEL,
			SOUND_X_REAL_DOREMI_2_LEVEL,
			SOUND_KEIKYU_NOT_REAL_N1000_SIEMENS_GTO_2_LEVEL,
		}


		//function calculation
		public static double get_saw_value_simple(double x)
		{
			double fixed_x = x - (double)((int)(x * M_1_2PI) * M_2PI);
			if (0 <= fixed_x && fixed_x < M_PI_2)
				return M_2_PI * fixed_x;
			else if (M_PI_2 <= fixed_x && fixed_x < 3.0 * M_PI_2)
				return -M_2_PI * fixed_x + 2;
			else
				return M_2_PI * fixed_x - 4;
		}

		public static double get_saw_value(double time, double angle_frequency, double initial_phase)
		{
			return -get_saw_value_simple(time * angle_frequency + initial_phase);
		}

		public static double get_sin_value(double time, double angle_frequency, double initial_phase, double amplitude)
		{
			return my_math.sin(time * angle_frequency + initial_phase) * amplitude;
		}

		public static int get_pwm_value(double sin_value, double saw_value)
		{
			if (sin_value - saw_value > 0)
				return 1;
			else
				return 0;
		}

		public static Wave_Values get_Wide_P_3(double time, double angle_frequency, double initial_phase, double voltage, bool saw_oppose)
		{
			double sin = get_sin_value(time, angle_frequency, initial_phase, 1);
			double saw = get_saw_value(time, angle_frequency, initial_phase);
			if (saw_oppose)
				saw = -saw;
			double pwm = ((sin - saw > 0) ? 1 : -1) * voltage;
			double nega_saw = (saw > 0) ? saw - 1 : saw + 1;
			int gate = get_pwm_value(pwm, nega_saw) * 2;
			Wave_Values wv = new Wave_Values();
			wv.sin_value = pwm;
			wv.saw_value = nega_saw;
			wv.pwm_value = gate;
			return wv;
		}
		public static Wave_Values get_P_with_saw(double time, double sin_angle_frequency, double initial_phase, double voltage, double carrier_mul, bool saw_oppose)
		{
			double carrier_saw = -get_saw_value(time, carrier_mul * sin_angle_frequency, carrier_mul * initial_phase);
			double saw = -get_saw_value(time, sin_angle_frequency, initial_phase);
			if (saw_oppose)
				saw = -saw;
			double pwm = (saw > 0) ? voltage : -voltage;
			int gate = get_pwm_value(pwm, carrier_saw) * 2;
			Wave_Values wv;
			wv.sin_value = saw;
			wv.saw_value = carrier_saw;
			wv.pwm_value = gate;
			return wv;
		}
		public static Wave_Values get_P_with_switchingangle(
			double alpha1,
			double alpha2,
			double alpha3,
			double alpha4,
			double alpha5,
			double alpha6,
			double alpha7,
			int flag,
			double time, double sin_angle_frequency, double initial_phase)
		{
			double theta = (initial_phase + time * sin_angle_frequency) - (double)((int)((initial_phase + time * sin_angle_frequency) * M_1_2PI) * M_2PI);

			int PWM_OUT = (((((theta <= alpha2) && (theta >= alpha1)) || ((theta <= alpha4) && (theta >= alpha3)) || ((theta <= alpha6) && (theta >= alpha5)) || ((theta <= M_PI - alpha1) && (theta >= M_PI - alpha2)) || ((theta <= M_PI - alpha3) && (theta >= M_PI - alpha4)) || ((theta <= M_PI - alpha5) && (theta >= M_PI - alpha6))) && ((theta <= M_PI) && (theta >= 0))) || (((theta <= M_PI - alpha7) && (theta >= alpha7)) && ((theta <= M_PI) && (theta >= 0)))) || ((!(((theta <= alpha2 + M_PI) && (theta >= alpha1 + M_PI)) || ((theta <= alpha4 + M_PI) && (theta >= alpha3 + M_PI)) || ((theta <= alpha6 + M_PI) && (theta >= alpha5 + M_PI)) || ((theta <= M_2PI - alpha1) && (theta >= M_2PI - alpha2)) || ((theta <= M_2PI - alpha3) && (theta >= M_2PI - alpha4)) || ((theta <= M_2PI - alpha5) && (theta >= M_2PI - alpha6))) && ((theta <= M_2PI) && (theta >= M_PI))) && !((theta <= M_2PI - alpha7) && (theta >= M_PI + alpha7)) && (theta <= M_2PI) && (theta >= M_PI)) ? 1 : -1;

			int gate = flag == 'A' ? -PWM_OUT + 1 : PWM_OUT + 1;
			Wave_Values wv;
			wv.sin_value = 0;
			wv.saw_value = 0;
			wv.pwm_value = gate;
			return wv;

		}
		public static double get_Amplitude(double freq, double max_freq)
		{
			double rate = 0.99, init = 0.01;
			if (freq > max_freq) return 1.0;
			if (freq <= 0.1) return 0.0;
			return rate / max_freq * freq + init;
		}
		public static double get_overmodulation_amplitude(double min_freq, double max_freq, double max_amplitude, double freq)
		{
			if (freq > max_freq) return max_amplitude;
			if (freq <= 0.1) return 0.0;
			else return my_math.exponential(max_amplitude , (freq - min_freq) * ((max_amplitude - 1) / (max_freq - min_freq)) / (max_amplitude - 1));
		}
		public static double get_wide_3_pulse_amplitude(double min_freq, double min_amplitude, double max_freq, double max_amplitude, double freq)
		{
			return (0.2 * ((freq - min_freq) * ((max_amplitude - min_amplitude) / (max_freq - min_freq)) + min_amplitude)) + 0.8;
		}

		public static int get_Pulse_Num(Pulse_Mode mode)
		{
			if (mode == Pulse_Mode.Not_In_Sync || mode == Pulse_Mode.Asyn_THI)
				return -1;
			if (mode == Pulse_Mode.P_1)
				return 0;
			if (mode == Pulse_Mode.P_Wide_3 || mode == Pulse_Mode.CHMP_Wide_3 || mode == Pulse_Mode.CHMP_3)
				return 0;
			if (mode == Pulse_Mode.P_5 || mode == Pulse_Mode.CHMP_5 || mode == Pulse_Mode.CHMP_Wide_5)
				return 6;
			if (mode == Pulse_Mode.P_7 || mode == Pulse_Mode.CHMP_7 || mode == Pulse_Mode.CHMP_Wide_7)
				return 9;
			if (mode == Pulse_Mode.P_10 || mode == Pulse_Mode.CHMP_9)
				return 10;
			if (mode == Pulse_Mode.P_11 || mode == Pulse_Mode.CHMP_11)
				return 15;
			if (mode == Pulse_Mode.P_12 || mode == Pulse_Mode.CHMP_13)
				return 12;
			if (mode == Pulse_Mode.P_18 || mode == Pulse_Mode.CHMP_15)
				return 18;
			if ((int)mode <= (int)Pulse_Mode.P_61)
				return 3 + (2 * ((int)mode - 6));

			return get_Pulse_Num((Pulse_Mode)((int)mode - 35));
		}

		//sin value definitions
		public static double sin_angle_freq = 0;
		public static double sin_time = 0;

		//saw value definitions
		public static double saw_angle_freq = 1050;
		public static double saw_time = 0;
		public static double pre_saw_random_freq = 0;


		public static int random_freq_move_count = 0;


		public static void reset_all_variables()
		{
			sin_angle_freq = 0;
			sin_time = 0;

			//saw value definitions
			saw_angle_freq = 1050;
			saw_time = 0;

			random_freq_move_count = 0;
		}

		public static double get_Rolling_Angle_Frequency()
        {
			return sin_angle_freq;
		}

		// random range => -range ~ range
		public class Carrier_Freq
        {
			public Carrier_Freq( double base_freq_a , double range_b)
            {
				base_freq = base_freq_a;
				range = range_b;
			}

			public double base_freq;
			public double range;
        }

		private static double get_random_freq(Carrier_Freq data)
		{
			double random_freq = 0;
			if (random_freq_move_count == 0 || pre_saw_random_freq == 0)
			{
				int random_v = my_math.my_random();
				double diff_freq = my_math.mod_d(random_v, data.range);
				if ((random_v & 0x01) == 1)
					diff_freq = -diff_freq;
				double silent_random_freq = data.base_freq + diff_freq;
				random_freq = silent_random_freq;
				pre_saw_random_freq = silent_random_freq;
			}
			else
			{
				random_freq = pre_saw_random_freq;
			}
			random_freq_move_count++;
			if (random_freq_move_count == 100)
				random_freq_move_count = 0;
			return random_freq;
		}

		public static double get_changing_carrier_freq(double starting_freq, double starting_carrier_freq, double ending_freq, double ending_carrier_freq, double current_frequency)
		{
			return starting_carrier_freq + (ending_carrier_freq - starting_carrier_freq) / (ending_freq - starting_freq) * (current_frequency - starting_freq);
		}

		public static double get_pattern_random(int lowest, int highest, int interval_count)
		{
			double random_freq = 0;
			if (random_freq_move_count < interval_count / 2.0)
				random_freq = lowest + (highest - lowest) / (interval_count / 2.0) * random_freq_move_count;
			else
				random_freq = highest + (lowest - highest) / (interval_count / 2.0) * (random_freq_move_count - interval_count / 2.0);
			if (++random_freq_move_count > interval_count)
				random_freq_move_count = 0;
			return random_freq;
		}

		public static Wave_Values calculate_three_level(Pulse_Mode pulse_mode, Carrier_Freq data, double initial_phase, double amplitude, double dipolar)
		{
			//variable change for video
			//no need in RPI zero vvvf
			Video_Generate_Values.pulse_mode = pulse_mode;
			Video_Generate_Values.sine_amplitude = amplitude;
			Video_Generate_Values.carrier_freq_data = data;
			Video_Generate_Values.dipolar = dipolar;

			if (pulse_mode == Pulse_Mode.Not_In_Sync)
            {
				double desire_saw_angle_freq = (data.range == 0) ? data.base_freq * M_2PI : get_random_freq(data) * M_2PI;
				saw_time = saw_angle_freq / desire_saw_angle_freq * saw_time;
				saw_angle_freq = desire_saw_angle_freq;

			}
			else
			{
				saw_angle_freq = sin_angle_freq * get_Pulse_Num(pulse_mode);
				saw_time = sin_time;
			}

			double sin_value = get_sin_value(sin_time, sin_angle_freq, initial_phase, amplitude);

			double saw_value = get_saw_value(saw_time, saw_angle_freq, 0);

			double changed_saw = ((dipolar != -1) ? dipolar : 0.5) * saw_value;
			int pwm_value = get_pwm_value(sin_value, changed_saw + 0.5) + get_pwm_value(sin_value, changed_saw - 0.5);

			Wave_Values wv;
			wv.sin_value = sin_value;
			wv.saw_value = saw_value;
			wv.pwm_value = pwm_value;
			return wv;
		}

		public static Wave_Values calculate_two_level(Pulse_Mode pulse_mode, Carrier_Freq carrier_freq_data, double initial_phase, double amplitude)
		{
			Video_Generate_Values.pulse_mode = pulse_mode;
			Video_Generate_Values.sine_amplitude = amplitude;
			Video_Generate_Values.carrier_freq_data = carrier_freq_data;

			if (pulse_mode == Pulse_Mode.P_Wide_3)
				return get_Wide_P_3(sin_time, sin_angle_freq, initial_phase, amplitude, false);
			if (pulse_mode == Pulse_Mode.SP_Wide_3)
				return get_Wide_P_3(sin_time, sin_angle_freq, initial_phase, amplitude, true);
			if (pulse_mode == Pulse_Mode.P_5)
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, amplitude, get_Pulse_Num(pulse_mode), false);
			if (pulse_mode == Pulse_Mode.SP_5)
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, amplitude, get_Pulse_Num(pulse_mode), true);
			if (pulse_mode == Pulse_Mode.P_7)
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, amplitude, get_Pulse_Num(pulse_mode), false);
			if (pulse_mode == Pulse_Mode.SP_7)
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, amplitude, get_Pulse_Num(pulse_mode), true);
			if (pulse_mode == Pulse_Mode.P_11)
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, amplitude, get_Pulse_Num(pulse_mode), false);
			if (pulse_mode == Pulse_Mode.SP_11)
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, amplitude, get_Pulse_Num(pulse_mode), true);
			if (pulse_mode == Pulse_Mode.CHMP_15)
				return get_P_with_switchingangle(
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 0] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 1] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 2] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 3] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 4] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 5] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 6] * M_PI_180,
				   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_13)
				return get_P_with_switchingangle(
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 7] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 8] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 9] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 10] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 11] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 12] * M_PI_180,
				   M_PI_2,
					   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_11)
				return get_P_with_switchingangle(
					my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 13] * M_PI_180,
					my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 14] * M_PI_180,
					my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 15] * M_PI_180,
					my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 16] * M_PI_180,
					my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 17] * M_PI_180,
					M_PI_2,
					M_PI_2,
					'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_9)
				return get_P_with_switchingangle(
				   my_switchingangles._4Alpha[(int)(100 * amplitude) + 1, 0] * M_PI_180,
				   my_switchingangles._4Alpha[(int)(100 * amplitude) + 1, 1] * M_PI_180,
				   my_switchingangles._4Alpha[(int)(100 * amplitude) + 1, 2] * M_PI_180,
				   my_switchingangles._4Alpha[(int)(100 * amplitude) + 1, 3] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   my_switchingangles._4Alpha_Polary[(int)(100 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_7)
				return get_P_with_switchingangle(
				   my_switchingangles._3Alpha[(int)(100 * amplitude) + 1, 0] * M_PI_180,
				   my_switchingangles._3Alpha[(int)(100 * amplitude) + 1, 1] * M_PI_180,
				   my_switchingangles._3Alpha[(int)(100 * amplitude) + 1, 2] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   my_switchingangles._3Alpha_Polary[(int)(100 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_7)
				return get_P_with_switchingangle(
				   my_switchingangles._3WideAlpha[(int)(1000 * amplitude) - 799, 0] * M_PI_180,
				   my_switchingangles._3WideAlpha[(int)(1000 * amplitude) - 799, 1] * M_PI_180,
				   my_switchingangles._3WideAlpha[(int)(1000 * amplitude) - 799, 2] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_5)
				return get_P_with_switchingangle(
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 13] * M_PI_180,
				   my_switchingangles.Alpha[(int)(100 * amplitude) + 1, 14] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'A', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_5)
				return get_P_with_switchingangle(
				   my_switchingangles._2WideAlpha[(int)(1000 * amplitude) - 799, 0] * M_PI_180,
				   my_switchingangles._2WideAlpha[(int)(1000 * amplitude) - 799, 1] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'A', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_3)
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, amplitude, get_Pulse_Num(pulse_mode), false);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_3)
				return get_P_with_switchingangle(
				   my_switchingangles._WideAlpha[(int)(500 * amplitude) + 1] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'B', sin_time, sin_angle_freq, initial_phase);

			if (pulse_mode == Pulse_Mode.Not_In_Sync || pulse_mode == Pulse_Mode.Asyn_THI)
			{
				double desire_saw_angle_freq = (carrier_freq_data.range == 0) ? carrier_freq_data.base_freq * M_2PI : get_random_freq(carrier_freq_data) * M_2PI;
				saw_time = saw_angle_freq / desire_saw_angle_freq * saw_time;
				saw_angle_freq = desire_saw_angle_freq;
			}
			else
			{
				saw_angle_freq = sin_angle_freq * get_Pulse_Num(pulse_mode);
				saw_time = sin_time;
			}


			double sin_value = pulse_mode == Pulse_Mode.Asyn_THI ?
			get_sin_value(sin_time, sin_angle_freq, initial_phase, amplitude) + 0.2 * get_sin_value(sin_time, 3 * sin_angle_freq, 3 * initial_phase, amplitude) :
			get_sin_value(sin_time, sin_angle_freq, initial_phase, amplitude);

			double saw_value = get_saw_value(saw_time, saw_angle_freq, 0);
			if ((int)pulse_mode > (int)Pulse_Mode.P_61)
				saw_value = -saw_value;

			int pwm_value = get_pwm_value(sin_value, saw_value) * 2;
			//Console.WriteLine(pwm_value);

			Wave_Values wv;
			wv.sin_value = sin_value;
			wv.saw_value = saw_value;
			wv.pwm_value = pwm_value;
			return wv;
		}
	}
}
