using static VVVF_Simulator.vvvf_wave_calculate;
using static VVVF_Simulator.VVVF_Control_Values;
using static VVVF_Simulator.my_math;
using static VVVF_Simulator.vvvf_wave_calculate.Amplitude_Argument;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data;
using System;
using System.Collections.Generic;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Free_Run_Condition;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Mascon_Data;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Control_Data_Amplitude_Control;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Table;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Random_Range;

namespace VVVF_Simulator.Yaml_VVVF_Sound
{
    public class Yaml_VVVF_Wave
    {
		private static double yaml_amplitude_calculate(Yaml_Control_Data_Amplitude amp_data, double x)
		{
			var amp_param = amp_data.parameter;
			object default_amplitude_parameter;
			if (amp_data.mode == Amplitude_Mode.Linear)
				default_amplitude_parameter = new General_Amplitude_Argument(amp_param.start_freq, amp_param.start_amp, amp_param.end_freq, amp_param.end_amp, x, amp_param.disable_range_limit);
			else if (amp_data.mode == Amplitude_Mode.Wide_3_Pulse)
				default_amplitude_parameter = new General_Amplitude_Argument(amp_param.start_freq, amp_param.start_amp, amp_param.end_freq, amp_param.end_amp, x, amp_param.disable_range_limit);
			else if (amp_data.mode == Amplitude_Mode.Inv_Proportional)
				default_amplitude_parameter = new Inv_Proportional_Amplitude_Argument(amp_param.start_freq, amp_param.start_amp, amp_param.end_freq, amp_param.end_amp, x, amp_param.curve_change_rate, amp_param.disable_range_limit);
			else if (amp_data.mode == Amplitude_Mode.Linear_Polynomial)
				default_amplitude_parameter = new Linear_Polynomial_Amplitude_Argument(amp_param.end_freq, amp_param.end_amp, amp_param.polynomial, x, amp_param.disable_range_limit);
			else
				default_amplitude_parameter = new Exponential_Amplitude_Argument(amp_param.end_freq, amp_param.end_amp, x, amp_param.disable_range_limit);
			double amp = get_Amplitude(amp_data.mode, default_amplitude_parameter);
			if (amp_param.cut_off_amp > amp) amp = 0;
			if (amp_param.max_amp != -1 && amp_param.max_amp < amp) amp = amp_param.max_amp;
			return amp;
		}
		public static Wave_Values calculate_Yaml(VVVF_Control_Values control , Control_Values cv, Yaml_Sound_Data yvs)
		{
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new(0, 0);
			double amplitude = 0;
			double dipolar = -1;

			//
			// min sine freq solve
			//
			double minimum_sine_freq, original_wave_stat = cv.wave_stat;
			if (cv.brake) minimum_sine_freq = yvs.min_freq.braking;
			else minimum_sine_freq = yvs.min_freq.accelerate;
			if (cv.wave_stat < minimum_sine_freq) cv.wave_stat = minimum_sine_freq;



			//
			// mascon off solve
			//
			double mascon_off_check;
			Yaml_Mascon_Data_On_Off mascon_on_off_check_data;
			if (cv.brake) mascon_on_off_check_data = yvs.mascon_data.braking;
			else mascon_on_off_check_data = yvs.mascon_data.accelerating;
			if (cv.mascon_on)
			{
				mascon_off_check = check_for_mascon_off(cv, control, mascon_on_off_check_data.on.control_freq_go_to);
				control.set_Mascon_Off_Div(mascon_on_off_check_data.on.div);
			}
			else
			{
				mascon_off_check = check_for_mascon_off(cv, control, mascon_on_off_check_data.off.control_freq_go_to);
				control.set_Mascon_Off_Div(mascon_on_off_check_data.off.div);
			}
			if (mascon_off_check != -1) cv.wave_stat = mascon_off_check;

			//
			// control stat solve
			//
			List<Yaml_Control_Data> control_list = new(cv.brake ? yvs.braking_pattern : yvs.accelerate_pattern);
			control_list.Sort((a, b) => (int)(b.from - a.from));

			//determine what control data to solve
			int solve = -1;
			for (int x = 0; x < control_list.Count; x++)
			{
				Yaml_Control_Data ysd = control_list[x];
				Yaml_Free_Run_Condition_Single free_run_data;
				if (cv.mascon_on) free_run_data = ysd.when_freerun.on;
				else free_run_data = ysd.when_freerun.off;

				bool enable_on_free_run_condition = ysd.enable_on_free_run && cv.free_run;
				bool enable_on_not_free_run_condition = ysd.enable_on_not_free_run && !cv.free_run;
				if (!enable_on_free_run_condition && !enable_on_not_free_run_condition) continue;//alow

				bool condition_1 = ysd.from <= cv.wave_stat;
				if (condition_1 && !cv.free_run)
				{
					solve = x;
					break;
				}


				if (!cv.free_run) continue;
				if (free_run_data.skip) continue;
				if (cv.free_run && control.get_Sine_Angle_Freq() < ysd.from * M_2PI) continue;

				if (!free_run_data.stuck_at_here && !condition_1) continue;
				solve = x;
				break;

			}
			if (solve == -1)
			{
				return get_Wave_Values_None();
			}

			Yaml_Control_Data solve_data = control_list[solve];
			pulse_mode = solve_data.pulse_Mode;

			if (pulse_mode == Pulse_Mode.Async || pulse_mode == Pulse_Mode.Async_THI)
			{
				var async_data = solve_data.async_data;

				var carrier_data = async_data.carrier_wave_data;
				var carrier_freq_mode = carrier_data.carrier_mode;

				double carrier_freq_val = 100;
				if (carrier_freq_mode == Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Carrier_Mode.Const)
					carrier_freq_val = carrier_data.const_value;
				else if (carrier_freq_mode == Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Carrier_Mode.Moving)
				{
					var moving_val = carrier_data.moving_value;
					//TODO implement moving type.
					carrier_freq_val = get_Changing_Value(
						moving_val.start,
						moving_val.start_value,
						moving_val.end,
						moving_val.end_value,
						original_wave_stat
					);
				}
				else if (carrier_freq_mode == Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Carrier_Mode.Table)
				{
					var table_data = carrier_data.carrier_table_value;

					//Solve from high.
					List<Yaml_Async_Parameter_Carrier_Freq_Table_Single> async_carrier_freq_table = new(table_data.carrier_freq_table);
					async_carrier_freq_table.Sort((a, b) => Math.Sign(b.from - a.from));
	
					for(int i = 0; i < async_carrier_freq_table.Count; i++)
                    {
						var carrier = async_carrier_freq_table[i];
						bool condition_1 = carrier.free_run_stuck_here && (control.get_Sine_Freq() < carrier.from) && cv.free_run;
						bool condition_2 = cv.wave_stat > carrier.from;
						if (!condition_1 && !condition_2) continue;

						carrier_freq_val = carrier.carrier_freq;
						break;

					}
					
				}
				else //Vibrato
				{
					var vibrato_data = carrier_data.vibrato_value;

					double highest, lowest;
					if (vibrato_data.highest.mode == Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Vibrato.Yaml_Async_Parameter_Vibrato_Value.Yaml_Async_Parameter_Vibrato_Mode.Const)
						highest = vibrato_data.highest.const_value;
					else
					{
						var moving_val = vibrato_data.highest.moving_value;
						highest = get_Changing_Value(moving_val.start, moving_val.start_value, moving_val.end, moving_val.end_value, cv.wave_stat);
					}

					if (vibrato_data.lowest.mode == Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Vibrato.Yaml_Async_Parameter_Vibrato_Value.Yaml_Async_Parameter_Vibrato_Mode.Const)
						lowest = vibrato_data.lowest.const_value;
					else
					{
						var moving_val = vibrato_data.lowest.moving_value;
						lowest = get_Changing_Value(moving_val.start, moving_val.start_value, moving_val.end, moving_val.end_value, cv.wave_stat);
					}
					carrier_freq_val = get_Vibrato_Freq(lowest, highest, vibrato_data.interval , control);
				}

				double random_range = 0;
				if (async_data.random_range.value_mode == Yaml_Async_Parameter_Random_Range_Mode.Const)
					random_range = async_data.random_range.const_value;
                else
                {
					var moving_val = async_data.random_range.moving_value;
					random_range = get_Changing_Value(moving_val.start, moving_val.start_value, moving_val.end, moving_val.end_value, cv.wave_stat);
				}
				carrier_freq = new Carrier_Freq(carrier_freq_val, random_range);

				if (async_data.dipoar_data != null)
				{
					var dipolar_data = async_data.dipoar_data;
					if (dipolar_data.value_mode == Yaml_Async_Parameter.Yaml_Async_Parameter_Dipolar.Yaml_Async_Parameter_Dipolar_Mode.Const)
						dipolar = dipolar_data.const_value;
					else
					{
						var moving_val = dipolar_data.moving_value;
						//TODO implement moving type.
						dipolar = get_Changing_Value(
							moving_val.start,
							moving_val.start_value,
							moving_val.end,
							moving_val.end_value,
							original_wave_stat
						);
					}

				}



			}

			amplitude = yaml_amplitude_calculate(solve_data.amplitude_control.default_data, cv.wave_stat);

			if (cv.free_run && solve_data.amplitude_control.free_run_data != null)
			{
				var free_run_data = solve_data.amplitude_control.free_run_data;
				var free_run_amp_data = (cv.mascon_on) ? free_run_data.mascon_on : free_run_data.mascon_off;
				var free_run_amp_param = free_run_amp_data.parameter;

				double max_control_freq = cv.mascon_on ? mascon_on_off_check_data.on.control_freq_go_to : mascon_on_off_check_data.off.control_freq_go_to;
				double target_freq , target_amp;

				if (free_run_amp_param.end_freq == -1)
					target_freq = (control.get_Sine_Freq() > max_control_freq) ? max_control_freq : control.get_Sine_Freq();
				else
					target_freq = free_run_amp_param.end_freq;

				if (free_run_amp_param.end_amp == -1)
					target_amp = yaml_amplitude_calculate(solve_data.amplitude_control.default_data, control.get_Sine_Freq());
				else
					target_amp = free_run_amp_param.end_amp;


				object free_run_amplitude_parameter;
				if (free_run_amp_data.mode == Amplitude_Mode.Linear)
					free_run_amplitude_parameter = new General_Amplitude_Argument(free_run_amp_param.start_freq, free_run_amp_param.start_amp, target_freq, target_amp, cv.wave_stat, free_run_amp_param.disable_range_limit);
				else if (free_run_amp_data.mode == Amplitude_Mode.Wide_3_Pulse)
					free_run_amplitude_parameter = new General_Amplitude_Argument(free_run_amp_param.start_freq, free_run_amp_param.start_amp, target_freq, target_amp, cv.wave_stat, free_run_amp_param.disable_range_limit);
				else if (free_run_amp_data.mode == Amplitude_Mode.Inv_Proportional)
					free_run_amplitude_parameter = new Inv_Proportional_Amplitude_Argument(free_run_amp_param.start_freq, free_run_amp_param.start_amp, target_freq, target_amp, cv.wave_stat, free_run_amp_param.curve_change_rate, free_run_amp_param.disable_range_limit);
				else if (free_run_amp_data.mode == Amplitude_Mode.Exponential)
					free_run_amplitude_parameter = new Exponential_Amplitude_Argument(target_freq, target_amp, cv.wave_stat, free_run_amp_param.disable_range_limit);
				else
					free_run_amplitude_parameter = new Linear_Polynomial_Amplitude_Argument(target_freq, target_amp, free_run_amp_param.polynomial, cv.wave_stat, free_run_amp_param.disable_range_limit);
				amplitude = get_Amplitude(free_run_amp_data.mode, free_run_amplitude_parameter);

				if (free_run_amp_param.cut_off_amp > amplitude) amplitude = 0;
				if (free_run_amp_param.max_amp != -1 && amplitude > free_run_amp_param.max_amp) amplitude = free_run_amp_param.max_amp;
				if (!cv.mascon_on && amplitude == 0) control.set_Control_Frequency(0);
			}

			if (cv.wave_stat == 0) amplitude = 0;

			if (yvs.level == 3) return calculate_three_level(control , pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, minimum_sine_freq), dipolar);
			else return calculate_two_level(control, pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, minimum_sine_freq));
		}
	}
}
