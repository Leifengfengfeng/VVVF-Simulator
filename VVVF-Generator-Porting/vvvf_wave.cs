using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_wave_control;
using static VVVF_Generator_Porting.my_math;

namespace VVVF_Generator_Porting
{
    public class vvvf_wave
    {
		//JR East
		public static Wave_Values calculate_jre_209_mitsubishi_gto_3_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode = Pulse_Mode.Async;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.brake)
			{
				set_Mascon_Off_Div(18000);
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 59, 1, cv.wave_stat, false));

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 66)
				{
					cv.wave_stat = 66;
					set_Control_Frequency(66);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 66)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (53 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 53 * M_2PI))
				{
					pulse_mode = Pulse_Mode.P_1;

					//if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Exponential, new Amplitude_Argument(0, -1, 66, 3, cv.wave_stat, false));
					if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_2, new Amplitude_Argument(0, 0.5, 66, 3, cv.wave_stat, false));
					else amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_1, new Amplitude_Argument(53, 0.83, 66, 3, cv.wave_stat, false));
				}
				else if (45 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 45 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (29 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 29 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (19 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 19 * M_2PI)) pulse_mode = Pulse_Mode.P_21;
				else if (9 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 9 * M_2PI)) pulse_mode = Pulse_Mode.P_33;
				else if (2 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 2 * M_2PI)) pulse_mode = Pulse_Mode.P_57;
				else
                {
					carrier_freq = new Carrier_Freq(114, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}

			else
			{
				set_Mascon_Off_Div(24000);
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 66, 1, cv.wave_stat, false));
				if (60 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 60 * M_2PI))
				{
					pulse_mode = Pulse_Mode.P_1;

					//if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Exponential, new Amplitude_Argument(0, -1, 72, 3, cv.wave_stat, false));
					if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_2, new Amplitude_Argument(0, 0.5, 72, 3, cv.wave_stat, false));
					else amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_1, new Amplitude_Argument(60, 0.83, 72, 3, cv.wave_stat, false));
					//get_overmodulation_amplitude(60, 72, 3, cv.wave_stat);
				}
				else if (49 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 49 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (40 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 40 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (19 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 19 * M_2PI)) pulse_mode = Pulse_Mode.P_21;
				else if (7 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 7 * M_2PI)) pulse_mode = Pulse_Mode.P_33;
				else get_Wave_Values_None();
			}
			return calculate_three_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0), -1);
		}

		public static Wave_Values calculate_jre_e231_mitsubishi_igbt_3_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_Mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			double dipolar = -1;

			if (cv.wave_stat > 0 && cv.wave_stat < 2 && !cv.free_run && !cv.brake) cv.wave_stat = 2;

			if (cv.brake)
			{
				set_Mascon_Off_Div(24000);
				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 71)
				{
					cv.wave_stat = 71;
					set_Control_Frequency(71);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 71)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 68, 1, cv.wave_stat, false));
				if (59 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 59 * M_2PI))
                {
					if (cv.free_run)
					{
						if (cv.mascon_on) amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_1, new Amplitude_Argument(0, 0.5, 71, 3.8, cv.wave_stat, false));
						else amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_2, new Amplitude_Argument(0, 0.5, 71, 3.8, cv.wave_stat, false));
					}
					else amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_1, new Amplitude_Argument(59, 0.83, 71, 3.8, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.P_1;
				}
				else if (50 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 50 * M_2PI))
					pulse_Mode = Pulse_Mode.P_3;
				else if (40 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 40 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(1000, 100);
				}
				else if (4 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 4 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					double expect_freq = get_Changing_freq(4, 169, 40, 1000, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(169, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 58, 1, cv.wave_stat, false));
				set_Mascon_Off_Div(20000);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 61)
				{
					cv.wave_stat = 61;
					set_Control_Frequency(61);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 61)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (51 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 51 * M_2PI))
				{
					if (cv.free_run)
                    {
						if(cv.mascon_on) amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_1, new Amplitude_Argument(0, 0.5, 61, 3.8, cv.wave_stat, false));
						else amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_2, new Amplitude_Argument(0, 0.5, 61, 3.8, cv.wave_stat, false));
					}
					else amplitude = get_Amplitude(Amplitude_Mode.Level_3_1P_1, new Amplitude_Argument(51, 0.83, 61, 3.8, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.P_1;
				}

				else if (39 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 39 * M_2PI))
					pulse_Mode = Pulse_Mode.P_3;
				else if (35 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 35 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(880, 100);
				}
				else if (14 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 14 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					double expect_freq = get_Changing_freq(14, 460, 35, 880, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
				}
				else if (2 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 2 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Async;
					double expect_freq = get_Changing_freq(2, 198, 14, 460, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
					dipolar = 2;
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(198, 100);
					dipolar = 2;
				}
			}
			return calculate_three_level(pulse_Mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, (!cv.free_run && !cv.brake) ? 2 : -1), dipolar);
		}
		public static Wave_Values calculate_jre_e231_1000_hitachi_igbt_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (cv.brake)
            {
				set_Mascon_Off_Div(24000);

				double mascon_off_check = check_for_mascon_off(cv, 73);
				if (mascon_off_check != -1) cv.wave_stat = mascon_off_check;

				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 73, 1, cv.wave_stat, false));

				if (cv.wave_stat > 73)
					pulse_mode = Pulse_Mode.P_1;
				else if (cv.wave_stat > 67)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.1 / 8.0 * (cv.wave_stat - 67);
				}
				else if (cv.wave_stat >= 56)
				{
					double expect_saw_freq = 700 + (1600 - 700) / 11 * (cv.wave_stat - 56);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else if (cv.wave_stat >= 29)
				{
					double expect_saw_freq = 1045 + (700 - 1045) / (55.9-29) * (cv.wave_stat - 29);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}
            else
            {
				set_Mascon_Off_Div(12000);

				double mascon_off_check = check_for_mascon_off(cv, 67);
				if (mascon_off_check != -1) cv.wave_stat = mascon_off_check;

				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 65, 1, cv.wave_stat, false));
				if (cv.wave_stat > 67)
					pulse_mode = Pulse_Mode.P_1;
				else if (cv.wave_stat > 60)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 8.0 * (cv.wave_stat - 60);
				}
				else if (cv.wave_stat >= 49)
				{
					double expect_saw_freq = 710 + (1750 - 710) / 11 * (cv.wave_stat - 49);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else if (cv.wave_stat >= 23)
				{
					double expect_saw_freq = 1045 + (710 - 1045) / (48.9-23) * (cv.wave_stat - 23);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}

		public static Wave_Values calculate_jre_e233_mitsubishi_igbt_2_level(Control_Values cv)
		{
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));

			if (cv.free_run && cv.mascon_on == false && amplitude < 0.85)
			{
				amplitude = 0.0;
			}

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 73.5, 1, cv.wave_stat, false));
				if (73.5 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (62.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 62.5 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(750, 100);
				}
			}
			else
			{
				if (50 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (45 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 45 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(750, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}

		public static Wave_Values calculate_jre_e233_3000_hitachi_igbt_2_level(Control_Values cv)
		{
			set_Mascon_Off_Div(10000);

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.mascon_on && cv.free_run && cv.wave_stat < 18)
			{
				return get_Wave_Values_None();
			}

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 69.5, 1, cv.wave_stat, false));
				if (69.5 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (64.8 <= cv.wave_stat && cv.mascon_on)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 6.0 * (cv.wave_stat - 64.8);
				}
				else if (51.3 <= cv.wave_stat && cv.mascon_on)
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(51.3, 51.3 / 69.5 , 64.8, 1.3, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.SP_9;
				}
				else if (cv.wave_stat <= 4)
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(200, 0);
				}
				else
				{
					double base_freq = 200 + (900 - 200) / 47.3 * (cv.wave_stat - 4);
					if (base_freq > 900) base_freq = 900;
					pulse_mode = Pulse_Mode.Async;
					if (cv.wave_stat <= 9)
					{
						double random_range = 99.0 / 5.0 * (cv.wave_stat - 4) + 1;
						carrier_freq = new Carrier_Freq(base_freq, random_range);
					}
					else
					{
						carrier_freq = new Carrier_Freq(base_freq, 100);
					}
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 51, 1, cv.wave_stat, false));
				if (51 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (46.8 <= cv.wave_stat && cv.mascon_on)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 6.0 * (cv.wave_stat - 46.8);
				}
				else if (42 <= cv.wave_stat && cv.mascon_on)
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(42, 42.0 / 51.0, 46.8, 1.3, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.SP_9;
				}
				else if (19.7 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = 525 + (910 - 510) / (41.6 - 19.7) * (cv.wave_stat - 19.7);
					if (base_freq > 910) base_freq = 910;
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
				else
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(525, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}

		public static Wave_Values calculate_jre_e235_toshiba_sic_2_level(Control_Values cv)
		{
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 54, 1, cv.wave_stat, false));

			if (cv.wave_stat > 54)
				pulse_mode = Pulse_Mode.P_15;
			else
			{
				pulse_mode = Pulse_Mode.Async;
				double base_freq = (double)550 + 3.148148148148148 * (cv.wave_stat); //170.0/54.0*(wave_stat);
				carrier_freq = new Carrier_Freq(base_freq, 100);
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}

		public static Wave_Values calculate_jre_e235_mitsubishi_sic_2_level(Control_Values cv)
		{
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);
			double amplitude;

			if (cv.brake)
			{
				double mascon_off_check = check_for_mascon_off(cv,92);
				if (mascon_off_check != -1) cv.wave_stat = mascon_off_check;

				set_Mascon_Off_Div(12000);
				if (92 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = 1.6;
				}
				else if (86 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = 1.3;
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(1200, 200);
				}
			}
			else
			{
				double mascon_off_check = check_for_mascon_off(cv, 77);
				if (mascon_off_check != -1) cv.wave_stat = mascon_off_check;

				set_Mascon_Off_Div(12000);
				if (77 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = 1.6;
				}
				else if (71 <= cv.wave_stat )
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = 1.3;
				}
				else if (66 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 66 * M_2PI))
                {
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.P_27;
                }
                else
                {
                    amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
                    pulse_mode = Pulse_Mode.Async;
                    carrier_freq = new Carrier_Freq(1200, 200);
                }
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}












		//JR West
		public static Wave_Values calculate_jrw_207_toshiba_gto_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.brake)
			{
				set_Mascon_Off_Div(15000);
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 60)
				{
					cv.wave_stat = 60;
					set_Control_Frequency(60);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 60)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (60 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (53 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 53 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (44 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 44 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (31 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 31 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 14 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else
				{
					carrier_freq = new Carrier_Freq(365, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}
			else
			{
				set_Mascon_Off_Div(15000);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 80)
				{
					cv.wave_stat = 80;
					set_Control_Frequency(80);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 80)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 80, 1, cv.wave_stat, false));
				if (80 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (65 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 65 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (50 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 50 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (30 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 30 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 14 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else if (8 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 8 * M_2PI))
					pulse_mode = Pulse_Mode.P_27;
				else
				{
					return get_Wave_Values_None();
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}
		public static Wave_Values calculate_jrw_207_update_toshiba_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
				if (60 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (59 <= cv.wave_stat)
				{
					amplitude = 0.9 + 0.1 / 2.0 * (cv.wave_stat - 59);
					pulse_mode = Pulse_Mode.P_Wide_3;
				}
				else if (55 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 55 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (47 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 47 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (36 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 36 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (23 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 23 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = 550 + 3.272727272727273 * cv.wave_stat;
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 80, 1, cv.wave_stat, false));
				if (80 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (72 <= cv.wave_stat)
				{
					amplitude = 0.8 + 0.2 / 8.0 * (cv.wave_stat - 72);
					pulse_mode = Pulse_Mode.P_Wide_3;
				}
				else if (57 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 57 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (44 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 44 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (29 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 29 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 14 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = 550 + 3.272727272727273 * cv.wave_stat;
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}

		public static Wave_Values calculate_jrw_223_2000_hitachi_igbt_3_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_Mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			double dipolar = -1;

			if (cv.brake)
			{
				set_Mascon_Off_Div(24000);
				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 71)
				{
					cv.wave_stat = 71;
					set_Control_Frequency(71);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 71)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (59 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 59 * M_2PI))
				{
					if (cv.wave_stat > 71) amplitude = 1 / 0.2;
					else
					{
						if (cv.free_run)
						{
							double temp = 0.2 + (-0.2 + 2) / 71 * (71 - cv.wave_stat);
							amplitude = 1 / temp;
						}
						else
						{
							double temp = 0.2 + (-0.2 + 1.2) / 12 * (71 - cv.wave_stat);
							amplitude = 1 / temp;
						}
					}
					pulse_Mode = Pulse_Mode.P_1;
				}
				else if (50 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 50 * M_2PI))
                {
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 68, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.P_3;
				}
				else if (40 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 40 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(1000, 100);
				}
				else if (4 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 4 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					double expect_freq = get_Changing_freq(4, 169, 40, 1000, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(169, 100);
				}
			}
			else
			{
				set_Mascon_Off_Div(20000);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 61)
				{
					cv.wave_stat = 61;
					set_Control_Frequency(61);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 61)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (51 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 51 * M_2PI))
				{
					if (cv.wave_stat > 61) amplitude = 1 / 0.2;
					else
					{
						if (cv.free_run)
						{
							double temp = 0.2 + (-0.2 + 2) / 61 * (61 - cv.wave_stat);
							amplitude = 1 / temp;

						}
						else
						{
							double temp = 0.2 + (-0.2 + 1.2) / 10 * (61 - cv.wave_stat);
							amplitude = 1 / temp;
						}
					}
					pulse_Mode = Pulse_Mode.P_1;
				}

				else if (42 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 42 * M_2PI))
                {
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 58, 1, cv.wave_stat, false));
					pulse_Mode = Pulse_Mode.SP_9;
				}
				else if (28 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 28 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(1000, 0);
					dipolar = 1;
				}
				else if (19 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 19 * M_2PI))
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Async;
					double expect_freq = get_Changing_freq(19, 500, 28, 1000, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 0);
					dipolar = 1;
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(500, 0);
					dipolar = 1;
				}
			}
			return calculate_three_level(pulse_Mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0), dipolar);
		}
		public static Wave_Values calculate_jrw_321_hitachi_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (cv.brake)
			{
				if (72 <= cv.wave_stat)
                {
					amplitude = 1.0;
					pulse_mode = Pulse_Mode.P_1;
                }
				else if (56 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_9;
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(56, 56.0 / 72.0, 72, 2, cv.wave_stat, false));
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 72, 1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					double base_freq = 1050;
					if (4 >= cv.wave_stat)
						base_freq = 510 + ((cv.wave_stat > 1) ? ((623 - 510) / 3.0 * (cv.wave_stat - 1)) : 0);
					carrier_freq = new Carrier_Freq(base_freq, 60);
				}
			}
			else
			{
				if (55 <= cv.wave_stat)
                {
					amplitude = 1.0;
					pulse_mode = Pulse_Mode.P_1;
                }
				else if (40 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_9;
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(40, 40.0 / 55.0, 55, 2, cv.wave_stat, false));
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 55, 1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(1050, 60);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, 0));
		}

		public static Wave_Values calculate_jrw_225_5100_mitsubishi_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.brake)
			{
				if (56 <= cv.wave_stat)
                {
					pulse_mode = Pulse_Mode.P_1;
					amplitude = 1;
				}
				else if (48 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_9;
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(48, 1.1, 56, 2, cv.wave_stat, false));
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 48, 1.1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(1050, 100);
				}
			}
			else
			{
				if (77 <= cv.wave_stat)
                {
					pulse_mode = Pulse_Mode.P_1;
					amplitude = 1.0;
				}
				else if (59 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_9;
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(59, 0.8, 78, 2, cv.wave_stat, false));
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 59, 0.8, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(1050, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}













		

		//Tokyuu
		public static Wave_Values calculate_tokyuu_9000_hitachi_gto_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);
			set_Mascon_Off_Div(10000);

			if (!cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 45, 1, cv.wave_stat, false));
				if (45 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (43 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = get_Amplitude(Amplitude_Mode.Wide_3_Pulse, new Amplitude_Argument(43, 0.5, 45, 0.8, cv.wave_stat, false));
				}
				else if (37 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 37 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (32 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 32 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 25 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 14 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (7 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 7 * M_2PI)) pulse_mode = Pulse_Mode.P_27;
				else if (5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 5 * M_2PI)) pulse_mode = Pulse_Mode.P_45;
				else
				{
					carrier_freq = new Carrier_Freq(200, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
				if (60 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (54 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = get_Amplitude(Amplitude_Mode.Wide_3_Pulse, new Amplitude_Argument(54, 0.5, 60, 0.8, cv.wave_stat, false));
				}
				else if (50 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 50 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (41 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 41 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (27 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 27 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (14.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 14.5 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (8 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 8 * M_2PI)) pulse_mode = Pulse_Mode.P_27;
				else if (7 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 7 * M_2PI)) pulse_mode = Pulse_Mode.P_45;
				else if (5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 5 * M_2PI))
				{
					carrier_freq = new Carrier_Freq(200, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else
				{
					return get_Wave_Values_None();
				}
			}
			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_tokyuu_5000_hitachi_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 65, 1, cv.wave_stat, false));
				if (65 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (61 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 5.0 * (cv.wave_stat - 61);
				}
				else if (50 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = (double)700 + 1100 / 11.0 * (cv.wave_stat - 50); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else if (23 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = (double)740 - 40.0 / (50 - 23) * (cv.wave_stat - 23); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else if (cv.brake && cv.wave_stat <= 4)
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(200, 0);
				}
				else
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(740, 0);
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 58, 1, cv.wave_stat, false));
				if (58 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (55 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 4.0 * (cv.wave_stat - 55);
				}
				else if (42 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = (double)700 + 1100 / 13.0 * (cv.wave_stat - 42); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else if (23 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = (double)740 - 40.0 / (46 - 23) * (cv.wave_stat - 23); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(740, 0);
				}
			}


			if (!cv.mascon_on && cv.free_run && cv.wave_stat < 23) amplitude = 0;

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}
		public static Wave_Values calculate_tokyuu_1000_1500_update_toshiba_igbt_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (cv.free_run && !cv.mascon_on)
			{
				return get_Wave_Values_None();
			}

			if (cv.brake)
			{
				if (51 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_1;
					amplitude = 1.0;
				}
				else if (50.8 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 0.2 * (cv.wave_stat - 50.8);
				}
				else if (38.3 <= cv.wave_stat && !(cv.free_run && get_Sine_Angle_Freq() > 50 * M_2PI))
				{
					pulse_mode = Pulse_Mode.SP_15;
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(35.8, 35.8 / 50.0 , 50, 2, cv.wave_stat, false));
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 51, 1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					double base_freq = get_pattern_random_freq((int)(400 + 180 / 38.3 * cv.wave_stat), 600, 20000);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
			}
			else
			{
				if (47.3 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_1;
					amplitude = 1.0;
				}
				else if (44.4 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = get_Amplitude(Amplitude_Mode.Wide_3_Pulse, new Amplitude_Argument(44.4, 0, 47.3, 0.8, cv.wave_stat, false));//0.8 + 0.2 / 4.0 * (cv.wave_stat - 44.4);
				}
				else if (35.8 <= cv.wave_stat && !(cv.free_run && get_Sine_Angle_Freq() > 44.4 * M_2PI))
				{
					pulse_mode = Pulse_Mode.SP_15;
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(35.8, 35.8 / 50, 44.4, 2, cv.wave_stat, false));
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 47.3, 1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					double base_freq = get_pattern_random_freq((int)(400 + 180 / 34.0 * cv.wave_stat), 600, 20000); ;
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}















		//Kintetsu

		public static Wave_Values calculate_kintetsu_5800_mitsubishi_gto_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.brake)
			{
				set_Mascon_Off_Div(15000);
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
				if (50 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (40 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 40 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (32 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 32 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 25 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else
				{
					carrier_freq = new Carrier_Freq(260, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}
			else
			{
				set_Mascon_Off_Div(15000);
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
				if (50 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (40 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 40 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (32 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 32 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 25 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 5 * M_2PI))
				{
					carrier_freq = new Carrier_Freq(260, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else
				{
					return get_Wave_Values_None();
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}
		public static Wave_Values calculate_kintetsu_9820_mitsubishi_igbt_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 55, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (55 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (50 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_3;
			else if (13 <= cv.wave_stat)
			{
				carrier_freq = new Carrier_Freq(700, 0);
				pulse_mode = Pulse_Mode.Async;
			}
			else if (cv.brake && cv.wave_stat < 8.5)
			{
				return get_Wave_Values_None();
			}
			else if (cv.wave_stat > 2)
			{
				double expect_saw_freq = 250 + (700 - 250) / 11 * (cv.wave_stat - 2);
				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				pulse_mode = Pulse_Mode.Async;
			}
			else
			{
				carrier_freq = new Carrier_Freq(250, 0);
				pulse_mode = Pulse_Mode.Async;
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_kintetsu_9820_hitachi_igbt_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 65, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (67 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (60 <= cv.wave_stat)
			{
				pulse_mode = Pulse_Mode.P_Wide_3;
				amplitude = 0.8 + 0.2 / 8.0 * (cv.wave_stat - 60);
			}
			else if (49 <= cv.wave_stat)
			{
				double expect_saw_freq = 780 + (1820 - 780) / 11 * (cv.wave_stat - 49);
				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				pulse_mode = Pulse_Mode.Async;
			}
			else
			{
				carrier_freq = new Carrier_Freq(780, 0);
				pulse_mode = Pulse_Mode.Async;
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

















		//Keio
		public static Wave_Values calculate_keio_8000_hitachi_gto_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 68.2, 1, cv.wave_stat, false));
				if (68.2 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (63.5 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 6.0 * (cv.wave_stat - 63.5);
				}
				else if ((cv.free_run && get_Sine_Angle_Freq() > 63.5 * M_2PI) && cv.wave_stat >= 30)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 34.0 * (cv.wave_stat - 30);
				}
				else if (54.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 54.5 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (41.2 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 41.2 * M_2PI)) pulse_mode = Pulse_Mode.P_7;
				else if (32.3 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 32.3 * M_2PI)) pulse_mode = Pulse_Mode.P_11;
				else if (21.0 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 21.0 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (7.8 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 7.8 * M_2PI)) pulse_mode = Pulse_Mode.P_21;
				else
				{
					return get_Wave_Values_None();
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 50, 1, cv.wave_stat, false));
				if (50 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (48.7 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 2.0 * (cv.wave_stat - 48.7);
				}
				else if ((cv.free_run && get_Sine_Angle_Freq() > 48.7 * M_2PI) && cv.wave_stat >= 30)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 20.0 * (cv.wave_stat - 30);
				}
				else if (41.2 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 41.2 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (32.4 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 38.4 * M_2PI)) pulse_mode = Pulse_Mode.P_7;
				else if (29.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 29.5 * M_2PI)) pulse_mode = Pulse_Mode.P_11;
				else if (25.8 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 25.8 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(400, 0);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}




















		//Keikyu
		public static Wave_Values calculate_keikyu_n1000_siemens_gto_2_level(Control_Values cv)
		{
			int a = 2, b = 3;
			double[,] k = new double[2, 3]
			{
				{ 0.0193294460641,0.0222656250000,0},
				{ 0.014763975813,0.018464,0.013504901961},
			};
			double[,] B = new double[2, 3]
			{
				{ 0.10000000000,-0.07467187500,0},
				{ 0.10000000000,-0.095166,0.100000000000},
			};

			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (!cv.brake)
			{
				set_Mascon_Off_Div(12000);
				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 80)
				{
					cv.wave_stat = 80;
					set_Control_Frequency(80);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 80)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}


				if (80 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_1;
				}
				else if (59 <= cv.wave_stat)
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_3;
				}
				else if (cv.free_run && get_Sine_Angle_Freq() >= 57 * M_2PI)
				{
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (57 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 57.0 * M_2PI))
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_5;
				}
				else if (53.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 53.5 * M_2PI))
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_7;
				}
				else if (43.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 43.5 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_7;
				}
				else if (36.7 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 36.7 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_9;
				}
				else if (30 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 30 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_11;
				}
				else if (27 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 27 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_13;
				}
				else if (24 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 24 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_15;
				}
				else
				{
					a = 1; b = 1;
					double expect_saw_freq = 400;
					pulse_mode = Pulse_Mode.Asyn_THI;
					if (5.6 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 5.6 * M_2PI))
						expect_saw_freq = 400;
					else if (5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 5.0 * M_2PI))
						expect_saw_freq = 350;
					else if (4.3 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 4.3 * M_2PI))
						expect_saw_freq = 311;
					else if (3.4 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 3.4 * M_2PI))
						expect_saw_freq = 294;
					else if (2.7 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 2.7 * M_2PI))
						expect_saw_freq = 262;
					else if (2.0 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 2.0 * M_2PI))
						expect_saw_freq = 233;
					else if (1.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 1.5 * M_2PI))
						expect_saw_freq = 223;
					else if (0.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 0.5 * M_2PI))
						expect_saw_freq = 196;
					else
						expect_saw_freq = 175;

					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				}
			}
			else
			{
				set_Mascon_Off_Div(20000);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 79.5)
				{
					cv.wave_stat = 79.5;
					set_Control_Frequency(79.5);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 79.5)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (79.5 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_1;
				}
				else if (70.7 <= cv.wave_stat)
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_3;
				}
				else if(cv.free_run && get_Sine_Angle_Freq() >= 63.35 * M_2PI)
                {
					pulse_mode = Pulse_Mode.P_3;
                }
				else if (63.35 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 63.35 * M_2PI))
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_5;
				}
				else if (56.84 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 56.84 * M_2PI))
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_7;
				}
				else if (53.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 53.5 * M_2PI))
				{
					a = 2; b = 1;
					pulse_mode = Pulse_Mode.CHMP_7;
				}
				else if (41 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 41 * M_2PI))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_7;
				}
				else if (34.5 <= cv.wave_stat || check_for_mascon_off_wave_stat(cv, 34.5))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_9;
				}
				else if (28.9 <= cv.wave_stat || check_for_mascon_off_wave_stat(cv, 28.9))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_11;
				}
				else if (24.9 <= cv.wave_stat || check_for_mascon_off_wave_stat(cv,24.9))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_13;
				}
				else if (22.4 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() >= 22.4 * M_2PI))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_15;
				}
				else if (cv.wave_stat > 4 || (cv.free_run && get_Sine_Angle_Freq() > 4 * M_2PI))
				{
					a = 2; b = 3;
					carrier_freq = new Carrier_Freq(400, 0);
					pulse_mode = Pulse_Mode.Asyn_THI;
				}
				else
				{
					return get_Wave_Values_None();
				}
			}
			double amplitude = ((k[a - 1, b - 1] * cv.wave_stat) + B[a - 1, b - 1]) >= 1.25 ? 1.25 : ((k[a - 1, b - 1] * cv.wave_stat) + B[a - 1, b - 1]);

			if (cv.free_run && amplitude < 0.498) amplitude = 0;
			if (cv.wave_stat == 0) amplitude = 0;
			if (pulse_mode == Pulse_Mode.P_3) amplitude /= 1.25;

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}


		public static Wave_Values calculate_keikyu_n1000_siemens_igbt_2_level(Control_Values cv)
		{
			int a = 2, b = 3;
			double[,] k = new double[2, 3]
			{
				{0.0169963174645,0.0140366562360,0},
				{0.0147535292756,0.0550408163265,0},
			};
			double[,] B = new double[2, 3]
			{
				{0.10000000000,0.31137934734,0},
				{0.10000000000,-2.84023265306,0},
			};

			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (!cv.brake)
			{
				if (80 <= cv.wave_stat)
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.P_1;
				}
				else if (64.58 <= cv.wave_stat)
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_5;
				}
				else if (62.597 <= cv.wave_stat)
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_7;
				}
				else if (44.519 <= cv.wave_stat)
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_9;
				}
				else if (38.936 <= cv.wave_stat)
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_11;
				}
				else
				{
					a = 1; b = 1;
					double expect_saw_freq = 600;
					pulse_mode = Pulse_Mode.Asyn_THI;
					if (10 <= cv.wave_stat)
						expect_saw_freq = 14.7297098247165 * cv.wave_stat + 452.70290175284;
					else if (37.156 <= cv.wave_stat)
						expect_saw_freq = 1000;
					else
						expect_saw_freq = 600;

					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				}
			}
			else
			{
				if (80 <= cv.wave_stat)
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.P_1;
				}
				else if (74.355 <= cv.wave_stat)
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_3;
				}
				else if (73 <= cv.wave_stat)
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_5;
				}
				else if (64.453 <= cv.wave_stat)
				{
					a = 2; b = 1;
					pulse_mode = Pulse_Mode.CHMP_7;
				}
				else if (44.468 <= cv.wave_stat)
				{
					a = 2; b = 1;
					pulse_mode = Pulse_Mode.CHMP_9;
				}
				else if (39.961 <= cv.wave_stat)
				{
					a = 2; b = 1;
					pulse_mode = Pulse_Mode.CHMP_11;
				}
				else if (cv.wave_stat > 1)
				{
					a = 2; b = 1;
					double expect_saw_freq = 600;
					pulse_mode = Pulse_Mode.Asyn_THI;
					if (10 <= cv.wave_stat)
						expect_saw_freq = 14.7297098247165 * cv.wave_stat + 452.70290175284;
					else if (37.156 <= cv.wave_stat)
						expect_saw_freq = 1000;
					else
						expect_saw_freq = 600;

					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				}
				else
				{
					return get_Wave_Values_None();
				}
			}
			double amplitude = ((k[a - 1, b - 1] * cv.wave_stat) + B[a - 1, b - 1]) >= 1.25 ? 1.25 : ((k[a - 1, b - 1] * cv.wave_stat) + B[a - 1, b - 1]);//¼ÆËãµ÷ÖÆ¶È
			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_not_real_keikyu_n1000_siemens_gto_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 80, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (80 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (57 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_Wide_3;
			else if (50 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_3;
			else if (43 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_5;
			else if (35 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_7;
			else if (30 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_12;
			else if (27 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_15;
			else if (24.5 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_18;
			else
			{
				if (!cv.brake)
				{
					double expect_saw_freq = 400;
					pulse_mode = Pulse_Mode.Async;
					if (5.6 <= cv.wave_stat)
						expect_saw_freq = 400;
					else if (5 <= cv.wave_stat)
						expect_saw_freq = 350;
					else if (4.3 <= cv.wave_stat)
						expect_saw_freq = 311;
					else if (3.4 <= cv.wave_stat)
						expect_saw_freq = 294;
					else if (2.7 <= cv.wave_stat)
						expect_saw_freq = 262;
					else if (2.0 <= cv.wave_stat)
						expect_saw_freq = 233;
					else if (1.5 <= cv.wave_stat)
						expect_saw_freq = 223;
					else if (0.5 <= cv.wave_stat)
						expect_saw_freq = 196;
					else
						expect_saw_freq = 175;

					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				}
				else
				{
					if (cv.wave_stat > 4)
					{
						carrier_freq = new Carrier_Freq(400, 0);
						pulse_mode = Pulse_Mode.Async;
					}
					else
					{
						return get_Wave_Values_None();
					}
				}
			}


			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}



















		//Toubu
		public static Wave_Values calculate_toubu_50050_hitachi_igbt_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

            if (cv.brake)
            {
				set_Mascon_Off_Div(24000);
				double change = check_for_mascon_off(cv, 69.5);
				if (change != -1) cv.wave_stat = change;

				if (69.5 <= cv.wave_stat)
				{
					amplitude = 1;
					pulse_mode = Pulse_Mode.P_1;
				}
				else if (65 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = get_Amplitude(Amplitude_Mode.Wide_3_Pulse, new Amplitude_Argument(65, 0.0, 69.5, 0.5, cv.wave_stat, false));
				}
				else if (56 <= cv.wave_stat)
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 65, 1.2, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(get_Changing_freq(56, 700, 65, 1620, cv.wave_stat), 0);
				}
				else if (53 <= cv.wave_stat)
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 65, 1.2, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(get_Changing_freq(0, 730, 53, 700, cv.wave_stat), 0);
				}
				else if(4 >= cv.wave_stat && !cv.free_run)
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 65, 1.2, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(200, 0);
                }
                else
                {
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 65, 1.2, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(get_Changing_freq(0, 730, 53, 700, cv.wave_stat), 150);
				}
			}
            else
            {
				set_Mascon_Off_Div(24000);
				double change = check_for_mascon_off(cv, 61);
				if (change != -1) cv.wave_stat = change;

				if (61 <= cv.wave_stat)
                {
					amplitude = 1;
					pulse_mode = Pulse_Mode.P_1;
				}
				else if (58 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = get_Amplitude(Amplitude_Mode.Wide_3_Pulse, new Amplitude_Argument(58, 0.0, 61, 0.8, cv.wave_stat, false));
				}
				else if (48 <= cv.wave_stat)
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 61, 1.2, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(get_Changing_freq(48 , 700, 58 , 1820, cv.wave_stat), 0);
				}
				else if (46 <= cv.wave_stat)
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 61, 1.2, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(get_Changing_freq(0, 730, 49, 700, cv.wave_stat), 0);
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 61, 1.2, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(get_Changing_freq(0,730,49, 700, cv.wave_stat), 150);
				}

				if (cv.wave_stat <= 30 && !cv.mascon_on && cv.free_run)
					amplitude = 0;
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}















		//Kyoto Subway
		public static Wave_Values calculate_kyoto_subway_50_mitsubishi_gto_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 63, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (63 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (60 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_Wide_3;
			else if (57 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_3;
			else if (44 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_5;
			else if (36 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_7;
			else if (16 <= cv.wave_stat)
			{
				carrier_freq = new Carrier_Freq(400, 0);
				pulse_mode = Pulse_Mode.Async;
			}
			else if (cv.brake && cv.wave_stat < 7.4)
			{
				return get_Wave_Values_None();
			}
			else if (cv.wave_stat >= 2)
			{
				double expect_saw_freq = 216 + (400 - 216) / 14 * (cv.wave_stat - 2);
				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				pulse_mode = Pulse_Mode.Async;
			}
			else
			{
				carrier_freq = new Carrier_Freq(216, 0);
				pulse_mode = Pulse_Mode.Async;
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}







		//Nagoya Subway
		public static Wave_Values calculate_nagoya_subway_2000_update_hitachi_gto_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 57, 1, cv.wave_stat, false));
				if (57 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (52 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 5 * (cv.wave_stat - 52);
				}
				else if (47 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 47 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (36 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 36 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (28 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 28 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (16.6 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 16.6 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 5 * M_2PI))
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(250, 0);
				}
				else
				{
					return get_Wave_Values_None();
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 32, 1, cv.wave_stat, false));
				if (32 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (31 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 1 * (cv.wave_stat - 31);
				}
				else if ((cv.free_run && get_Sine_Angle_Freq() > 31 * M_2PI) && cv.wave_stat >= 31)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 1 * (cv.wave_stat - 30);
				}
				else if (29 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 29 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (25 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 25 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (20 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 20 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (16.6 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 16.6 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Async;
					carrier_freq = new Carrier_Freq(250, 0);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}








		//Keihan
		public static Wave_Values calculate_keihan_13000_toyo_igbt_2_level(Control_Values cv)
		{
			double amplitude;

			if (53 <= cv.wave_stat && cv.wave_stat <= 55)
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(53, 0.963, 55, 5, cv.wave_stat, false));
			else
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 55, 1, cv.wave_stat, false));

			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (55 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (34 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_9;
			else
			{
				carrier_freq = new Carrier_Freq(525, 0);
				pulse_mode = Pulse_Mode.Async;
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}



















		//Toei Subway
		public static Wave_Values calculate_toei_6300_mitsubishi_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.brake)
			{
				set_Mascon_Off_Div(18000);
				

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 60)
				{
					cv.wave_stat = 60;
					set_Control_Frequency(60);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 60)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				

				if (60 <= cv.wave_stat)
                {
					amplitude = 1;
					pulse_mode = Pulse_Mode.P_1;
				}
					
				else if (52.5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 52.5 * M_2PI))
                {
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 61, 1, cv.wave_stat, false));
					pulse_mode = Pulse_Mode.P_3;
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 52.5, 1.1, cv.wave_stat, false));
					if (cv.wave_stat < 5)
						carrier_freq = new Carrier_Freq(220, 110);
					else if(cv.wave_stat < 15)
						carrier_freq = new Carrier_Freq(get_Changing_freq(5,220,15,1000,cv.wave_stat), 110);
					else
						carrier_freq = new Carrier_Freq(1000, 110);
					pulse_mode = Pulse_Mode.Async;
				}
			}
			else
			{
				set_Mascon_Off_Div(16000);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 62.5)
				{
					cv.wave_stat = 62.5;
					set_Control_Frequency(62.5);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 62.5)
				{
					double rolling_freq = get_Sine_Angle_Freq() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (62.5 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_1;
					amplitude = 1;
				}
				else if (52 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 52 * M_2PI))
                {
					pulse_mode = Pulse_Mode.P_3;
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 63.5, 1, cv.wave_stat, false));
				}
				else
				{
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 52, 1.05, cv.wave_stat, false));
					if (cv.wave_stat < 1)
						carrier_freq = new Carrier_Freq(205, 110);
					else if (cv.wave_stat < 3)
						carrier_freq = new Carrier_Freq(get_Changing_freq(1, 205, 5, 220, cv.wave_stat), 110);
					else if (cv.wave_stat < 5)
						carrier_freq = new Carrier_Freq(220, 110);
					else if (cv.wave_stat < 15)
						carrier_freq = new Carrier_Freq(get_Changing_freq(5, 220, 15, 1000, cv.wave_stat), 110);
					else
						carrier_freq = new Carrier_Freq(1000, 110);
					pulse_mode = Pulse_Mode.Async;
				}
			}

			if (cv.free_run && amplitude < 0.65 && !cv.mascon_on)
				amplitude = 0;

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude, (cv.brake) ? -1 : 1.2));
		}

















		//WMATA
		public static Wave_Values calculate_wmata_6000_alstom_igbt_2_level(Control_Values cv)
		{
			set_Mascon_Off_Div(5000);

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.free_run && !cv.mascon_on)
			{
				return get_Wave_Values_None();
			}

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 85, 1, cv.wave_stat, false));
				pulse_mode = Pulse_Mode.Async;

				double expect_saw_freq = 1190;

				if (82 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 82 * M_2PI)) expect_saw_freq = 1190;
				else if (50 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 50 * M_2PI)) expect_saw_freq = 1230;
				else if (47 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 47 * M_2PI)) expect_saw_freq = 1210;
				else if (40 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 40 * M_2PI)) expect_saw_freq = 1460;
				else if (30 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 30 * M_2PI)) expect_saw_freq = 1235;
				else if (25 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 25 * M_2PI)) expect_saw_freq = 1210;
				else if (6 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 6 * M_2PI)) expect_saw_freq = 1190;
				else if (5 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 5 * M_2PI)) expect_saw_freq = 1235;
				else return get_Wave_Values_None();

				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 85, 1, cv.wave_stat, false));
				pulse_mode = Pulse_Mode.Async;

				double expect_saw_freq = 1190;

				if (85 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 85 * M_2PI)) expect_saw_freq = 1190;
				else if (57 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 57 * M_2PI)) expect_saw_freq = 1230;
				else if (50 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 50 * M_2PI)) expect_saw_freq = 1210;
				else if (44 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 44 * M_2PI)) expect_saw_freq = 1460;
				else if (35 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 35 * M_2PI)) expect_saw_freq = 1235;
				else if (27 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 27 * M_2PI)) expect_saw_freq = 1210;
				else if (11 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 11 * M_2PI)) expect_saw_freq = 1190;
				else expect_saw_freq = 1235;

				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);

			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_wmata_7000_toshiba_igbt_2_level(Control_Values cv)
		{
			set_Mascon_Off_Div(10000);

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.free_run && !cv.mascon_on)
			{
				return get_Wave_Values_None();
			}

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 90, 1, cv.wave_stat, false));
				if (98 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (92 <= cv.wave_stat && cv.mascon_on)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 10 * (cv.wave_stat - 90);
				}
				else if (70 <= cv.wave_stat && cv.mascon_on)
					pulse_mode = Pulse_Mode.SP_9;
				else if (8 <= cv.wave_stat && cv.mascon_on)
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = (560 + 145 / 70 * cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 150);
				}

				else return get_Wave_Values_None();
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 85, 1, cv.wave_stat, false));
				if (88 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (82 <= cv.wave_stat && cv.mascon_on)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 10 * (cv.wave_stat - 80);
				}
				else if (60 <= cv.wave_stat && cv.mascon_on)
					pulse_mode = Pulse_Mode.SP_9;
				else
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = (560 + 145 / 64 * cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 150);
				}

			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}


















		//Custom
		public static Wave_Values calculate_toyo_gto_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 77, 1, cv.wave_stat, false));

				if (77 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (74 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 4.0 * (cv.wave_stat - 74);
				}
				else if (69 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8;
				}

				else if (60 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 60 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (43 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 43 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 25 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (6 <= cv.wave_stat && cv.wave_stat <= 9)
				{
					double expect_saw_freq = (260 + (365 - 260) / 25.0 * (cv.wave_stat - 3));
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else
				{
					if (cv.wave_stat < 5)
					{
						return get_Wave_Values_None();
					}
					pulse_mode = Pulse_Mode.Async;
					double base_freq = 260;
					if (cv.wave_stat > 3)
						base_freq = (260 + (365 - 260) / 25.0 * (cv.wave_stat - 3));
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));

				if (60 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (56 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 5.0 * (cv.wave_stat - 56);
				}

				else if (51 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 51 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (43 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 43 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (27 <= cv.wave_stat || (cv.free_run && get_Sine_Angle_Freq() > 27 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (6 <= cv.wave_stat && cv.wave_stat <= 9)
				{
					double expect_saw_freq = 260 + (365 - 260) / 23.0 * (cv.wave_stat - 3);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else
				{
					pulse_mode = Pulse_Mode.Async;
					double base_freq = 260;
					if (cv.wave_stat > 3)
						base_freq = (260 + (365 - 260) / 23.0 * (cv.wave_stat - 3));
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_toyo_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				if (96 <= cv.wave_stat && cv.wave_stat <= 98)
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(96, 0.979, 98, 5, cv.wave_stat, false));
				else
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 98, 1, cv.wave_stat, false));

				pulse_mode = Pulse_Mode.P_1;
				if (98 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (33 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_9;
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}
			else
			{
				if (53 <= cv.wave_stat && cv.wave_stat <= 55)
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(53, 0.979, 55, 5, cv.wave_stat, false));
				else
					amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 55, 1, cv.wave_stat, false));

				pulse_mode = Pulse_Mode.P_1;
				if (55 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (34 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_9;
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Async;
				}
			}
			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_silent_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (50 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_27;
			else
			{
				pulse_mode = Pulse_Mode.Async;
				carrier_freq = new Carrier_Freq(550, 100);
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}
		public static Wave_Values calculate_jre_209_mitsubishi_gto_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 53, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (53 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (46 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_3;
			else if (30 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_9;
			else if (19 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_21;
			else if (9 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_33;
			else if (2 <= cv.wave_stat && !cv.brake)
				pulse_mode = Pulse_Mode.P_57;
			else if (cv.wave_stat < 2 && !cv.brake)
			{
				pulse_mode = Pulse_Mode.Async;
				carrier_freq = new Carrier_Freq(114, 0);
			}
			else if (8 < cv.wave_stat && cv.wave_stat < 18 && cv.brake)
				pulse_mode = Pulse_Mode.P_33;
			else
			{
				return get_Wave_Values_None();
			}
			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_famina_2_level(Control_Values cv)
		{

			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 60, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (60 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (56 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_3;
			else
			{
				double expect_saw_freq = 0;
				if (48 <= cv.wave_stat)
					expect_saw_freq = 622;
				else if (44 <= cv.wave_stat)
					expect_saw_freq = 466;
				else if (40 <= cv.wave_stat)
					expect_saw_freq = 698;
				else if (36 <= cv.wave_stat)
					expect_saw_freq = 783;
				else if (32 <= cv.wave_stat)
					expect_saw_freq = 698;
				else if (28 <= cv.wave_stat)
					expect_saw_freq = 466;
				else if (20 <= cv.wave_stat)
					expect_saw_freq = 932;
				else if (16 <= cv.wave_stat)
					expect_saw_freq = 587;
				else if (12 <= cv.wave_stat)
					expect_saw_freq = 622;
				else if (8 <= cv.wave_stat)
					expect_saw_freq = 466;
				else if (4 <= cv.wave_stat)
					expect_saw_freq = 622;
				else
					expect_saw_freq = 783;

				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				pulse_mode = Pulse_Mode.Async;
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}

		public static Wave_Values calculate_real_doremi_2_level(Control_Values cv)
		{

			double amplitude = get_Amplitude(Amplitude_Mode.Linear, new Amplitude_Argument(0, 0, 80, 1, cv.wave_stat, false));
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (80 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (57 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_Wide_3;
			else if (50 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_3;
			else if (43 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_5;
			else if (35 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_7;
			else if (30 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_12;
			else if (27 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_15;
			else if (24.5 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_18;
			else
			{
				if (!cv.brake)
				{
					double expect_saw_freq = 0;
					if (5.6 <= cv.wave_stat)
						expect_saw_freq = 587;
					else if (5 <= cv.wave_stat)
						expect_saw_freq = 523;
					else if (4.3 <= cv.wave_stat)
						expect_saw_freq = 493;
					else if (3.4 <= cv.wave_stat)
						expect_saw_freq = 440;
					else if (2.7 <= cv.wave_stat)
						expect_saw_freq = 391;
					else if (2.0 <= cv.wave_stat)
						expect_saw_freq = 349;
					else if (1.5 <= cv.wave_stat)
						expect_saw_freq = 329;
					else if (0.5 <= cv.wave_stat)
						expect_saw_freq = 293;
					else
						expect_saw_freq = 261;
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Async;
				}
				else
				{
					if (cv.wave_stat > 4)
					{
						carrier_freq = new Carrier_Freq(400, 0);
						pulse_mode = Pulse_Mode.Async;
					}
					else
					{
						return get_Wave_Values_None();
					}
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, new Sine_Control_Data(cv.initial_phase, amplitude,0));
		}
	}
}