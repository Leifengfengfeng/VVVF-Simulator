using static VVVF_Simulator.Generation.Generate_Common;
using static VVVF_Simulator.VVVF_Control_Values;
using static VVVF_Simulator.my_math;
using System;
using static VVVF_Simulator.vvvf_wave_calculate.Amplitude_Argument;

namespace VVVF_Simulator
{
	public class vvvf_wave_calculate
	{
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
			Async, 
			Async_THI,
			P_Wide_3,

			P_1, P_2, P_3, P_4,P_5, P_6, P_7,P_8, P_9, P_10, 
			P_11, P_12, P_13, P_14, P_15, P_16, P_17, P_18, P_19, P_20,
			P_21, P_22, P_23, P_24, P_25, P_26, P_27, P_28, P_29, P_30,
			P_31, P_32, P_33, P_34, P_35, P_36, P_37, P_38, P_39, P_40,
			P_41, P_42, P_43, P_44, P_45, P_46, P_47, P_48, P_49, P_50,
			P_51, P_52, P_53, P_54, P_55, P_56, P_57, P_58, P_59, P_60,
			P_61,

			SP_2, SP_3, SP_4, SP_5, SP_6, SP_8, SP_7, SP_9, SP_10, SP_11, SP_13, SP_15, SP_17, SP_19,
			SP_21, SP_23, SP_25, SP_27, SP_29, SP_31, SP_33, SP_35, SP_37, SP_39, SP_41
			, SP_43, SP_45, SP_47, SP_49, SP_51, SP_53, SP_55, SP_57, SP_59, SP_61
			
			,// Current harmonic minimum Pulse width modulation
			CHMP_3, CHMP_Wide_3, CHMP_5, CHMP_Wide_5, CHMP_7, CHMP_Wide_7,
			CHMP_9, CHMP_Wide_9, CHMP_11, CHMP_Old_11, CHMP_Wide_11, CHMP_13,
			CHMP_Old_13, CHMP_Wide_13, CHMP_15, CHMP_Old_15, CHMP_Wide_15,

			// Selective harmonic elimination Pulse width modulation
			SHEP_3, SHEP_5, SHEP_7, SHEP_9, SHEP_11, SHEP_13, SHEP_15
		};

		//function calculation
		public static double get_saw_value(double x)
		{
			double val;
			double fixed_x = x - (double)((int)(x * M_1_2PI) * M_2PI);
			if (0 <= fixed_x && fixed_x < M_PI_2)
				val = M_2_PI * fixed_x;
			else if (M_PI_2 <= fixed_x && fixed_x < 3.0 * M_PI_2)
				val = -M_2_PI * fixed_x + 2;
			else
				val = M_2_PI * fixed_x - 4;

			return -val;
		}

		public static double get_sin_value(double x, double amplitude)
		{
			return my_math.sin(x) * amplitude;
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
			double sin = get_sin_value(time * angle_frequency + initial_phase, 1);
			double saw = get_saw_value(time * angle_frequency + initial_phase);
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
		public static Wave_Values get_P_with_saw(double time, double sin_angle_frequency, double initial_phase, double carrier_initial_phase, double voltage, double carrier_mul, bool saw_oppose)
		{
			double carrier_saw = -get_saw_value(carrier_mul * (time * sin_angle_frequency + initial_phase) + carrier_initial_phase);
			double saw = -get_saw_value(time * sin_angle_frequency + initial_phase);
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
		
		public enum Amplitude_Mode
        {
			Linear, Wide_3_Pulse, Inv_Proportional , Exponential,
			Linear_Polynomial,
		}

		public class Amplitude_Argument
        {
			public class General_Amplitude_Argument
			{
				public double min_freq = 0;
				public double min_amp = 0;
				public double max_freq = 0;
				public double max_amp = 0;
				public bool disable_range_limit = true;

				public double current = 0;

				public General_Amplitude_Argument(double Minimum_Freq, double Minimum_Amplitude, double Maximum_Freq, double Maximum_Amplitude, double Current, bool Disable_Range_Limit)
				{
					min_freq = Minimum_Freq;
					max_freq = Maximum_Freq;

					min_amp = Minimum_Amplitude;
					max_amp = Maximum_Amplitude;

					disable_range_limit = Disable_Range_Limit;

					current = Current;
				}

			}

			public class Inv_Proportional_Amplitude_Argument
			{
				public double min_freq = 0;
				public double min_amp = 0;
				public double max_freq = 0;
				public double max_amp = 0;
				public bool disable_range_limit = true;

				public double change_const = 0.43;

				public double current = 0;

				public Inv_Proportional_Amplitude_Argument(double Minimum_Freq, double Minimum_Amplitude, double Maximum_Freq, double Maximum_Amplitude, double Current, double Change_Const, bool Disable_Range_Limit)
				{
					min_freq = Minimum_Freq;
					max_freq = Maximum_Freq;

					min_amp = Minimum_Amplitude;
					max_amp = Maximum_Amplitude;

					disable_range_limit = Disable_Range_Limit;

					change_const = Change_Const;

					current = Current;
				}

			}

			public class Linear_Polynomial_Amplitude_Argument
			{
				public int polynomial = 2;

				public double max_freq = 0;
				public double max_amp = 0;
				public bool disable_range_limit = true;

				public double current = 0;

				public Linear_Polynomial_Amplitude_Argument(double Maximum_Freq, double Maximum_Amplitude, int Polynomial, double Current, bool Disable_Range_Limit)
				{
					max_freq = Maximum_Freq;

					max_amp = Maximum_Amplitude;

					polynomial = Polynomial;

					disable_range_limit = Disable_Range_Limit;

					current = Current;
				}

			}

			public class Exponential_Amplitude_Argument
			{

				public double max_freq = 0;
				public double max_amp = 0;
				public bool disable_range_limit = true;

				public double current = 0;

				public Exponential_Amplitude_Argument(double Maximum_Freq, double Maximum_Amplitude, double Current, bool Disable_Range_Limit)
				{
					max_freq = Maximum_Freq;

					max_amp = Maximum_Amplitude;

					disable_range_limit = Disable_Range_Limit;

					current = Current;
				}

			}
		}

		public static double get_Amplitude(Amplitude_Mode mode , Object arg_o)
        {
			double val = 0;
			if (mode == Amplitude_Mode.Linear)
            {
				General_Amplitude_Argument arg = (General_Amplitude_Argument)arg_o;

				if (!arg.disable_range_limit)
				{
					if (arg.current < arg.min_freq) arg.current = arg.min_freq;
					if (arg.current > arg.max_freq) arg.current = arg.max_freq;
				}
				val = (arg.max_amp - arg.min_amp) / (arg.max_freq - arg.min_freq) * (arg.current - arg.min_freq) + arg.min_amp;
			}
				
			else if(mode == Amplitude_Mode.Wide_3_Pulse)
            {
				General_Amplitude_Argument arg = (General_Amplitude_Argument)arg_o;

				if (!arg.disable_range_limit)
				{
					if (arg.current < arg.min_freq) arg.current = arg.min_freq;
					if (arg.current > arg.max_freq) arg.current = arg.max_freq;
				}
				val = (0.2 * ((arg.current - arg.min_freq) * ((arg.max_amp - arg.min_amp) / (arg.max_freq - arg.min_freq)) + arg.min_amp)) + 0.8;
			}
				

			else if(mode == Amplitude_Mode.Inv_Proportional)
            {
				Inv_Proportional_Amplitude_Argument arg = (Inv_Proportional_Amplitude_Argument)arg_o;

				if (!arg.disable_range_limit)
				{
					if (arg.current < arg.min_freq) arg.current = arg.min_freq;
					if (arg.current > arg.max_freq) arg.current = arg.max_freq;
				}

				double x = get_Amplitude(Amplitude_Mode.Linear, new General_Amplitude_Argument(arg.min_freq, 1 / arg.min_amp, arg.max_freq, 1 / arg.max_amp, arg.current, arg.disable_range_limit));

				double c = -arg.change_const;
				double k = arg.max_amp;
				double l = arg.min_amp;
				double a = 1 / ((1 / l) - (1 / k)) * (1 / (l - c) - 1 / (k - c));
				double b = 1 / (1 - (1 / l) * k) * (1 / (l - c) - (1 / l) * k / (k - c));

				//val = 1 / (6.25*x - 2.5) + 0.4;
				val = 1 / (a * x + b) + c;
			}
			else if(mode == Amplitude_Mode.Exponential)
            {
				Exponential_Amplitude_Argument arg = (Exponential_Amplitude_Argument)arg_o;

				if (!arg.disable_range_limit)
				{
					if (arg.current > arg.max_freq) arg.current = arg.max_freq;
				}

				double t = 1 / arg.max_freq *  Math.Log(arg.max_amp + 1);

				val = Math.Pow(Math.E, t * arg.current) - 1;
			}
			else if(mode == Amplitude_Mode.Linear_Polynomial)
            {
				Linear_Polynomial_Amplitude_Argument arg = (Linear_Polynomial_Amplitude_Argument)arg_o;

				if (!arg.disable_range_limit)
				{
					if (arg.current > arg.max_freq) arg.current = arg.max_freq;
				}

				val = Math.Pow(arg.current, arg.polynomial) / Math.Pow(arg.max_freq, arg.polynomial) * arg.max_amp;

			}

            
			return val;
		}

		public static int get_Pulse_Num(Pulse_Mode mode)
		{
			if (mode == Pulse_Mode.Async || mode == Pulse_Mode.Async_THI)
				return 0;
			if (mode == Pulse_Mode.P_1)
				return 1;
			if (mode == Pulse_Mode.P_Wide_3 || mode == Pulse_Mode.CHMP_Wide_3 || mode == Pulse_Mode.CHMP_3)
				return 0;
			if (mode == Pulse_Mode.P_5 || mode == Pulse_Mode.SP_5 || mode == Pulse_Mode.CHMP_5 || mode == Pulse_Mode.CHMP_Wide_5)
				return 6;
			if (mode == Pulse_Mode.P_7 || mode == Pulse_Mode.SP_7 || mode == Pulse_Mode.CHMP_7 || mode == Pulse_Mode.CHMP_Wide_7)
				return 9;
			if (mode == Pulse_Mode.CHMP_9)
				return 10;
			if (mode == Pulse_Mode.P_11 || mode == Pulse_Mode.SP_11 || mode == Pulse_Mode.CHMP_11)
				return 15;
			if ( mode == Pulse_Mode.CHMP_13)
				return 12;
			if ( mode == Pulse_Mode.CHMP_15)
				return 18;

			String pulse_name = mode.ToString();
			String[] split = pulse_name.Split('_');
			if (split.Length < 2)
				return 0;

			String pulse = split[split.Length - 1];
			int pulse_num = Int32.Parse(pulse);
			
			if( pulse_num % 2 == 0)
				return (int)(pulse_num * 1.5);
			return pulse_num;
		}
		public static double get_Pulse_Initial(Pulse_Mode mode)
        {
			String pulse_name = mode.ToString();
			String[] split = pulse_name.Split('_');
			String pulse = split[split.Length - 1];

			int pulse_num = Int32.Parse(pulse);
			if (split.Length > 1 && pulse_num % 2 == 0)
				return M_PI_2;

			return 0;
        }

		// random range => -range ~ range
		public class Carrier_Freq
        {
			public Carrier_Freq Clone()
			{
				return (Carrier_Freq)MemberwiseClone();
			}
			public Carrier_Freq( double base_freq_a , double range_b)
            {
				base_freq = base_freq_a;
				range = range_b;
			}

			public double base_freq;
			public double range;
        }

		private static double get_Random_freq(Carrier_Freq data, VVVF_Control_Values control)
		{
            if (control.is_Allowed_Random_Freq_Move())
            {
				double random_freq = 0;
				if (control.get_Random_Freq_Move_Count() == 0 || control.get_Pre_Saw_Random_Freq() == 0)
				{
					int random_v = my_math.my_random();
					double diff_freq = my_math.mod_d(random_v, data.range);
					if ((random_v & 0x01) == 1)
						diff_freq = -diff_freq;
					double silent_random_freq = data.base_freq + diff_freq;
					random_freq = silent_random_freq;
					control.set_Pre_Saw_Random_Freq(silent_random_freq);
				}
				else
				{
					random_freq = control.get_Pre_Saw_Random_Freq();
				}

				control.add_Random_Freq_Move_Count(1);
				if (control.get_Random_Freq_Move_Count() == 100)
					control.set_Random_Freq_Move_Count(0);

				return random_freq;
            }
            else
            {
				return data.base_freq;
            }
		}

		public static double get_Changing_Freq(double starting_freq, double starting_carrier_freq, double ending_freq, double ending_carrier_freq, double current_frequency)
		{
			return starting_carrier_freq + (ending_carrier_freq - starting_carrier_freq) / (ending_freq - starting_freq) * (current_frequency - starting_freq);
		}

		public static double get_Vibrato_Freq(double lowest, double highest, double interval_count , VVVF_Control_Values control)
		{
			double random_freq = 0;
			int vib_coun = control.get_Vibrato_Freq_Move_Count();
			if (vib_coun < interval_count / 2.0)
				random_freq = lowest + (highest - lowest) / (interval_count / 2.0) * vib_coun;
			else
				random_freq = highest + (lowest - highest) / (interval_count / 2.0) * (vib_coun - interval_count / 2.0);

			control.add_Vibrato_Freq_Move_Count(1);
			if (control.get_Vibrato_Freq_Move_Count() > interval_count)
				control.set_Vibrato_Freq_Move_Count(0);
			return random_freq;
        }

        public class Sine_Control_Data {
			public double initial_phase = 0;
			public double amplitude = 0;
			public double min_sine_freq = 0;

			public Sine_Control_Data(double Initial_Phase , double Amplitude , double Minimum_Sine_Frequency)
            {
				initial_phase = Initial_Phase;
				amplitude = Amplitude;
				min_sine_freq = Minimum_Sine_Frequency;
            }
		}

		public static double check_for_mascon_off(Control_Values cv, VVVF_Control_Values control, double max_voltage_freq)
        {
			if (cv.free_run && !cv.mascon_on && cv.wave_stat > max_voltage_freq)
			{
				control.set_Control_Frequency(max_voltage_freq);
				return max_voltage_freq;
				
			}
			else if (cv.free_run && cv.mascon_on && cv.wave_stat > max_voltage_freq)
			{
				double rolling_freq = control.get_Sine_Angle_Freq() * M_1_2PI;
				control.set_Control_Frequency(rolling_freq);
				return rolling_freq;
			}
			return -1;
		}

        public static Wave_Values calculate_three_level(VVVF_Control_Values control ,Pulse_Mode pulse_mode, Carrier_Freq data, Sine_Control_Data sine_control, double dipolar)
		{
			control.set_Video_Pulse_Mode(pulse_mode);
			control.set_Video_Sine_Amplitude(sine_control.amplitude);
			control.set_Video_Carrier_Freq_Data(data.Clone());
			control.set_Video_Dipolar(dipolar);

			double sine_angle_freq = control.get_Sine_Angle_Freq();
			double sine_time = control.get_Sine_Time();
			double min_sine_angle_freq = sine_control.min_sine_freq * M_2PI;
			if (sine_angle_freq < min_sine_angle_freq)
            {
				control.set_Allowed_Sine_Time_Change(false);
				sine_angle_freq = min_sine_angle_freq;
            }
            else
				control.set_Allowed_Sine_Time_Change(true);

			if (pulse_mode == Pulse_Mode.Async)
            {
				double desire_saw_angle_freq = (data.range == 0) ? data.base_freq * M_2PI : get_Random_freq(data, control) * M_2PI;

				control.set_Saw_Time(control.get_Saw_Angle_Freq() / desire_saw_angle_freq * control.get_Saw_Time());
				control.set_Saw_Angle_Freq(desire_saw_angle_freq);

				double sin_value = get_sin_value(sine_time * sine_angle_freq + sine_control.initial_phase, sine_control.amplitude);
				double saw_value = get_saw_value(control.get_Saw_Time() * control.get_Saw_Angle_Freq());

				double changed_saw = ((dipolar != -1) ? dipolar : 0.5) * saw_value;
				int pwm_value = get_pwm_value(sin_value, changed_saw + 0.5) + get_pwm_value(sin_value, changed_saw - 0.5);

				Wave_Values wv;
				wv.sin_value = sin_value;
				wv.saw_value = saw_value;
				wv.pwm_value = pwm_value;
				return wv;

			}
			else
			{
				

				double sin_value = get_sin_value(sine_time * sine_angle_freq + sine_control.initial_phase, sine_control.amplitude);
				double saw_value = get_saw_value(get_Pulse_Num(pulse_mode) * (sine_angle_freq * sine_time + sine_control.initial_phase));

				double changed_saw = ((dipolar != -1) ? dipolar : 0.5) * saw_value;
				int pwm_value = get_pwm_value(sin_value, changed_saw + 0.5) + get_pwm_value(sin_value, changed_saw - 0.5);

				control.set_Saw_Angle_Freq(sine_angle_freq * get_Pulse_Num(pulse_mode));
				control.set_Saw_Time(sine_time);

				Wave_Values wv;
				wv.sin_value = sin_value;
				wv.saw_value = saw_value;
				wv.pwm_value = pwm_value;
				return wv;
			}

			
		}

		public static Wave_Values calculate_two_level (VVVF_Control_Values control , Pulse_Mode pulse_mode, Carrier_Freq carrier_freq_data, Sine_Control_Data sine_control)
		{
			control.set_Video_Pulse_Mode(pulse_mode);
			control.set_Video_Sine_Amplitude(sine_control.amplitude);
			control.set_Video_Carrier_Freq_Data(carrier_freq_data.Clone());
			control.set_Video_Dipolar(-1);

			double sin_angle_freq = control.get_Sine_Angle_Freq();
			double sin_time = control.get_Sine_Time();
			double min_sine_angle_freq = sine_control.min_sine_freq * M_2PI;
			if (sin_angle_freq < min_sine_angle_freq)
			{
				control.set_Allowed_Sine_Time_Change(false);
				sin_angle_freq = min_sine_angle_freq;
			}
			else
				control.set_Allowed_Sine_Time_Change(true);

			double saw_time = control.get_Saw_Time();
			double saw_angle_freq = control.get_Saw_Angle_Freq();

			double initial_phase = sine_control.initial_phase;
			double amplitude = sine_control.amplitude;

			//if mode is wide 3
			if (pulse_mode == Pulse_Mode.P_Wide_3)
				return get_Wide_P_3(sin_time, sin_angle_freq, initial_phase, amplitude, false);

			//if async
			if (pulse_mode == Pulse_Mode.Async || pulse_mode == Pulse_Mode.Async_THI)
			{
				double desire_saw_angle_freq = (carrier_freq_data.range == 0) ? carrier_freq_data.base_freq * M_2PI : get_Random_freq(carrier_freq_data, control) * M_2PI;

				if (desire_saw_angle_freq == 0)
					saw_time = 0;
				else 
					saw_time = saw_angle_freq / desire_saw_angle_freq * saw_time;
				saw_angle_freq = desire_saw_angle_freq;

				double sin_value;

				if (pulse_mode == Pulse_Mode.Async_THI)
					sin_value = get_sin_value(sin_time * sin_angle_freq + initial_phase, amplitude) + 0.2 * get_sin_value(sin_time * 3 * sin_angle_freq + 3 * initial_phase, amplitude);
				else
					sin_value = get_sin_value(sin_time * sin_angle_freq + initial_phase, amplitude);


				double saw_value = get_saw_value(saw_time * saw_angle_freq);
				int pwm_value = get_pwm_value(sin_value, saw_value) * 2;

				control.set_Saw_Angle_Freq(saw_angle_freq);
				control.set_Saw_Time(saw_time);

				Wave_Values wv;
				wv.sin_value = sin_value;
				wv.saw_value = saw_value;
				wv.pwm_value = pwm_value;
				return wv;

			}

			if (pulse_mode == Pulse_Mode.CHMP_15)
				return get_P_with_switchingangle(
				   My_Switchingangles._7Alpha[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._7Alpha[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._7Alpha[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   My_Switchingangles._7Alpha[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
				   My_Switchingangles._7Alpha[(int)(1000 * amplitude) + 1, 4] * M_PI_180,
				   My_Switchingangles._7Alpha[(int)(1000 * amplitude) + 1, 5] * M_PI_180,
				   My_Switchingangles._7Alpha[(int)(1000 * amplitude) + 1, 6] * M_PI_180,
				   My_Switchingangles._7Alpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Old_15)
				return get_P_with_switchingangle(
				   My_Switchingangles._7Alpha_Old[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._7Alpha_Old[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._7Alpha_Old[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   My_Switchingangles._7Alpha_Old[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
				   My_Switchingangles._7Alpha_Old[(int)(1000 * amplitude) + 1, 4] * M_PI_180,
				   My_Switchingangles._7Alpha_Old[(int)(1000 * amplitude) + 1, 5] * M_PI_180,
				   My_Switchingangles._7Alpha_Old[(int)(1000 * amplitude) + 1, 6] * M_PI_180,
				   My_Switchingangles._7OldAlpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_15)
				return get_P_with_switchingangle(
				   My_Switchingangles._7WideAlpha[(int)(1000 * amplitude) - 999, 0] * M_PI_180,
				   My_Switchingangles._7WideAlpha[(int)(1000 * amplitude) - 999, 1] * M_PI_180,
				   My_Switchingangles._7WideAlpha[(int)(1000 * amplitude) - 999, 2] * M_PI_180,
				   My_Switchingangles._7WideAlpha[(int)(1000 * amplitude) - 999, 3] * M_PI_180,
				   My_Switchingangles._7WideAlpha[(int)(1000 * amplitude) - 999, 4] * M_PI_180,
				   My_Switchingangles._7WideAlpha[(int)(1000 * amplitude) - 999, 5] * M_PI_180,
				   My_Switchingangles._7WideAlpha[(int)(1000 * amplitude) - 999, 6] * M_PI_180,
				   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_13)
				return get_P_with_switchingangle(
				   My_Switchingangles._6Alpha[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._6Alpha[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._6Alpha[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   My_Switchingangles._6Alpha[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
				   My_Switchingangles._6Alpha[(int)(1000 * amplitude) + 1, 4] * M_PI_180,
				   My_Switchingangles._6Alpha[(int)(1000 * amplitude) + 1, 5] * M_PI_180,
				   M_PI_2,
				   My_Switchingangles._6Alpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Old_13)
				return get_P_with_switchingangle(
				   My_Switchingangles._6Alpha_Old[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._6Alpha_Old[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._6Alpha_Old[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   My_Switchingangles._6Alpha_Old[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
				   My_Switchingangles._6Alpha_Old[(int)(1000 * amplitude) + 1, 4] * M_PI_180,
				   My_Switchingangles._6Alpha_Old[(int)(1000 * amplitude) + 1, 5] * M_PI_180,
				   M_PI_2,
				   My_Switchingangles._6OldAlpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_13)
				return get_P_with_switchingangle(
				   My_Switchingangles._6WideAlpha[(int)(1000 * amplitude) - 999, 0] * M_PI_180,
				   My_Switchingangles._6WideAlpha[(int)(1000 * amplitude) - 999, 1] * M_PI_180,
				   My_Switchingangles._6WideAlpha[(int)(1000 * amplitude) - 999, 2] * M_PI_180,
				   My_Switchingangles._6WideAlpha[(int)(1000 * amplitude) - 999, 3] * M_PI_180,
				   My_Switchingangles._6WideAlpha[(int)(1000 * amplitude) - 999, 4] * M_PI_180,
				   My_Switchingangles._6WideAlpha[(int)(1000 * amplitude) - 999, 5] * M_PI_180,
				   M_PI_2,
				   'A', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_11)
				return get_P_with_switchingangle(
					My_Switchingangles._5Alpha[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
					My_Switchingangles._5Alpha[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
					My_Switchingangles._5Alpha[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
					My_Switchingangles._5Alpha[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
					My_Switchingangles._5Alpha[(int)(1000 * amplitude) + 1, 4] * M_PI_180,
					M_PI_2,
					M_PI_2,
					My_Switchingangles._5Alpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Old_11)
				return get_P_with_switchingangle(
					My_Switchingangles._5Alpha_Old[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
					My_Switchingangles._5Alpha_Old[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
					My_Switchingangles._5Alpha_Old[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
					My_Switchingangles._5Alpha_Old[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
					My_Switchingangles._5Alpha_Old[(int)(1000 * amplitude) + 1, 4] * M_PI_180,
					M_PI_2,
					M_PI_2,
					My_Switchingangles._5OldAlpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_11)
				return get_P_with_switchingangle(
					My_Switchingangles._5WideAlpha[(int)(1000 * amplitude) - 999, 0] * M_PI_180,
					My_Switchingangles._5WideAlpha[(int)(1000 * amplitude) - 999, 1] * M_PI_180,
					My_Switchingangles._5WideAlpha[(int)(1000 * amplitude) - 999, 2] * M_PI_180,
					My_Switchingangles._5WideAlpha[(int)(1000 * amplitude) - 999, 3] * M_PI_180,
					My_Switchingangles._5WideAlpha[(int)(1000 * amplitude) - 999, 4] * M_PI_180,
					M_PI_2,
					M_PI_2,
					'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_9)
				return get_P_with_switchingangle(
				   My_Switchingangles._4Alpha[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._4Alpha[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._4Alpha[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   My_Switchingangles._4Alpha[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   My_Switchingangles._4Alpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_9)
				return get_P_with_switchingangle(
				   My_Switchingangles._4WideAlpha[(int)(1000 * amplitude) - 799, 0] * M_PI_180,
				   My_Switchingangles._4WideAlpha[(int)(1000 * amplitude) - 799, 1] * M_PI_180,
				   My_Switchingangles._4WideAlpha[(int)(1000 * amplitude) - 799, 2] * M_PI_180,
				   My_Switchingangles._4WideAlpha[(int)(1000 * amplitude) - 799, 3] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'A', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_7)
				return get_P_with_switchingangle(
				   My_Switchingangles._3Alpha[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._3Alpha[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._3Alpha[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   My_Switchingangles._3Alpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_7)
				return get_P_with_switchingangle(
				   My_Switchingangles._3WideAlpha[(int)(1000 * amplitude) - 799, 0] * M_PI_180,
				   My_Switchingangles._3WideAlpha[(int)(1000 * amplitude) - 799, 1] * M_PI_180,
				   My_Switchingangles._3WideAlpha[(int)(1000 * amplitude) - 799, 2] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_5)
				return get_P_with_switchingangle(
				   My_Switchingangles._2Alpha[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._2Alpha[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   My_Switchingangles._2Alpha_Polary[(int)(1000 * amplitude) + 1], sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_5)
				return get_P_with_switchingangle(
				   My_Switchingangles._2WideAlpha[(int)(1000 * amplitude) - 799, 0] * M_PI_180,
				   My_Switchingangles._2WideAlpha[(int)(1000 * amplitude) - 799, 1] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'A', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.CHMP_Wide_3)
				return get_P_with_switchingangle(
				   My_Switchingangles._WideAlpha[(int)(500 * amplitude) + 1] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.SHEP_3)
				return get_P_with_switchingangle(
				   My_Switchingangles._1Alpha_SHE[(int)(1000 * amplitude) + 1] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.SHEP_5)
				return get_P_with_switchingangle(
				   My_Switchingangles._2Alpha_SHE[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._2Alpha_SHE[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'A', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.SHEP_7)
				return get_P_with_switchingangle(
				   My_Switchingangles._3Alpha_SHE[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._3Alpha_SHE[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._3Alpha_SHE[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   M_PI_2,
				   'B', sin_time, sin_angle_freq, initial_phase);
			if (pulse_mode == Pulse_Mode.SHEP_11)
				return get_P_with_switchingangle(
				  My_Switchingangles._5Alpha_SHE[(int)(1000 * amplitude) + 1, 0] * M_PI_180,
				   My_Switchingangles._5Alpha_SHE[(int)(1000 * amplitude) + 1, 1] * M_PI_180,
				   My_Switchingangles._5Alpha_SHE[(int)(1000 * amplitude) + 1, 2] * M_PI_180,
				   My_Switchingangles._5Alpha_SHE[(int)(1000 * amplitude) + 1, 3] * M_PI_180,
				   My_Switchingangles._5Alpha_SHE[(int)(1000 * amplitude) + 1, 4] * M_PI_180,
				   M_PI_2,
				   M_PI_2,
				   'A', sin_time, sin_angle_freq, initial_phase);


			String[] pulse_name_split = pulse_mode.ToString().Split('_');
			bool saw_go = false;
			if (Int32.Parse(pulse_name_split[pulse_name_split.Length - 1]) % 2 == 0)
				saw_go = true;
			if (

				pulse_mode == Pulse_Mode.P_5 ||
				pulse_mode == Pulse_Mode.SP_5 ||

				pulse_mode == Pulse_Mode.P_7 ||
				pulse_mode == Pulse_Mode.SP_7 ||

				pulse_mode == Pulse_Mode.P_11 ||
				pulse_mode == Pulse_Mode.SP_11 ||
				pulse_mode == Pulse_Mode.CHMP_3 ||

				saw_go
			)
			{
				bool is_shift = pulse_mode.ToString().StartsWith("SP");
				int pulse_num = get_Pulse_Num(pulse_mode);
				double pulse_initial_phase = get_Pulse_Initial(pulse_mode);
				return get_P_with_saw(sin_time, sin_angle_freq, initial_phase, pulse_initial_phase, amplitude, pulse_num, is_shift);
			}


			//sync mode but no the above.
            {
				int pulse_num = get_Pulse_Num(pulse_mode);

				double sin_value = get_sin_value(sin_time * sin_angle_freq + initial_phase, amplitude);
				double saw_value = get_saw_value(pulse_num * (sin_time * sin_angle_freq + initial_phase));

				if (pulse_mode.ToString().StartsWith("SP"))
					saw_value = -saw_value;

				int pwm_value = get_pwm_value(sin_value, saw_value) * 2;
				//Console.WriteLine(pwm_value);

				control.set_Saw_Angle_Freq(sin_angle_freq * pulse_num);
				control.set_Saw_Time(sin_time);

				Wave_Values wv;
				wv.sin_value = sin_value;
				wv.saw_value = saw_value;
				wv.pwm_value = pwm_value;
				return wv;
			}


		}
	}
}
