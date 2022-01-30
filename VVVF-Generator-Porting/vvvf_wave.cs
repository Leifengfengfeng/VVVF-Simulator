using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.Program;

namespace VVVF_Generator_Porting
{
    public class vvvf_wave
    {
		//JR East
		public static Wave_Values calculate_jre_209_mitsubishi_gto_3_level(Control_Values cv)
		{

			double amplitude = 0;
			Pulse_Mode pulse_mode = Pulse_Mode.Not_In_Sync;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);
			set_Mascon_Off_Div(24000);

			if (!cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 59);
				if (53 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 53 * M_2PI))
				{
					pulse_mode = Pulse_Mode.P_1;
					amplitude = get_overmodulation_amplitude(53, 66, 3, cv.wave_stat);
				}
				else if (45 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 45 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (29 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 29 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (19 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 19 * M_2PI)) pulse_mode = Pulse_Mode.P_21;
				else if (9 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 9 * M_2PI)) pulse_mode = Pulse_Mode.P_33;
				else if (2 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 2 * M_2PI)) pulse_mode = Pulse_Mode.P_57;
				else
                {
					carrier_freq = new Carrier_Freq(114, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
			}

			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 66);
				if (60 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 60 * M_2PI))
				{
					pulse_mode = Pulse_Mode.P_1;
					amplitude = get_overmodulation_amplitude(60, 72, 3, cv.wave_stat);
				}
				else if (49 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 49 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (40 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 40 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (19 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 19 * M_2PI)) pulse_mode = Pulse_Mode.P_21;
				else if (7 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 7 * M_2PI)) pulse_mode = Pulse_Mode.P_33;
				else get_Wave_Values_None();
			}
			return calculate_three_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude, -1);
		}

		public static Wave_Values calculate_jre_e231_mitsubishi_igbt_3_level(Control_Values cv)
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
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				amplitude = get_Amplitude(cv.wave_stat, 68);
				if (59 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 59 * M_2PI))
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
				else if (50 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 50 * M_2PI))
					pulse_Mode = Pulse_Mode.P_3;
				else if (40 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 40 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(1000, 100);
				}
				else if (4 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 4 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					double expect_freq = get_changing_carrier_freq(4, 169, 40, 1000, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
				}
				else
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(169, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 58);
				set_Mascon_Off_Div(20000);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 61)
				{
					cv.wave_stat = 61;
					set_Control_Frequency(61);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 61)
				{
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (51 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 51 * M_2PI))
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

				else if (39 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 39 * M_2PI))
					pulse_Mode = Pulse_Mode.P_3;
				else if (35 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 35 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(880, 100);
				}
				else if (14 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 14 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					double expect_freq = get_changing_carrier_freq(14, 460, 35, 880, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
				}
				else if (2 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 2 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					double expect_freq = get_changing_carrier_freq(2, 198, 14, 460, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
					dipolar = 2;
				}
				else
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(198, 100);
					dipolar = 2;
				}
			}
			return calculate_three_level(pulse_Mode, carrier_freq, cv.initial_phase, amplitude, dipolar);
		}
		public static Wave_Values calculate_jre_e231_1000_hitachi_igbt_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (cv.brake)
            {
				set_Mascon_Off_Div(24000);

				amplitude = get_Amplitude(cv.wave_stat, 73);
				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 73)
				{
					cv.wave_stat = 73;
					set_Control_Frequency(73);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 73)
				{
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

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
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else if (cv.wave_stat >= 29)
				{
					double expect_saw_freq = 1045 + (700 - 1045) / (55.9-29) * (cv.wave_stat - 29);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
			}
            else
            {
				set_Mascon_Off_Div(12000);

				amplitude = get_Amplitude(cv.wave_stat, 65);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 67)
				{
					cv.wave_stat = 67;
					set_Control_Frequency(67);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 67)
				{
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

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
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else if (cv.wave_stat >= 23)
				{
					double expect_saw_freq = 1045 + (710 - 1045) / (48.9-23) * (cv.wave_stat - 23);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_jre_e233_mitsubishi_igbt_2_level(Control_Values cv)
		{
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);
			double amplitude = get_Amplitude(cv.wave_stat, 50);

			if (cv.free_run && cv.mascon_on == false && amplitude < 0.85)
			{
				amplitude = 0.0;
			}

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 73.5);
				if (73.5 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (62.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 62.5 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(750, 100);
				}
			}
			else
			{
				if (50 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (45 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 45 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(750, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
				amplitude = get_Amplitude(cv.wave_stat, 69.5);
				if (69.5 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (64.8 <= cv.wave_stat && cv.mascon_on)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 6.0 * (cv.wave_stat - 64.8);
				}
				else if (51.3 <= cv.wave_stat && cv.mascon_on)
				{
					amplitude = 1.3 + (get_Amplitude(51.3, 69.5) - 1.3) / (64.8 - 51.3) * (64.8 - cv.wave_stat);
					pulse_mode = Pulse_Mode.SP_9;
				}
				else if (cv.wave_stat <= 4)
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(200, 0);
				}
				else
				{
					double base_freq = 200 + (900 - 200) / 47.3 * (cv.wave_stat - 4);
					if (base_freq > 900) base_freq = 900;
					pulse_mode = Pulse_Mode.Not_In_Sync;
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
				amplitude = get_Amplitude(cv.wave_stat, 51);
				if (51 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (46.8 <= cv.wave_stat && cv.mascon_on)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 6.0 * (cv.wave_stat - 46.8);
				}
				else if (42 <= cv.wave_stat && cv.mascon_on)
				{
					amplitude = 1.3 + (get_Amplitude(42, 51) - 1.3) / (46.8 - 42) * (46.8 - cv.wave_stat);
					pulse_mode = Pulse_Mode.SP_9;
				}
				else if (19.7 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = 525 + (910 - 510) / (41.6 - 19.7) * (cv.wave_stat - 19.7);
					if (base_freq > 910) base_freq = 910;
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(525, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_jre_e235_toshiba_sic_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 54);

			double sin_value = get_sin_value(sin_time, sin_angle_freq, cv.initial_phase, amplitude);
			double saw_value;
			if (cv.wave_stat > 54)
			{

				saw_angle_freq = sin_angle_freq * 15;
				saw_time = sin_time;

				saw_value = get_saw_value(saw_time, saw_angle_freq, 0);
			}
			else
			{

				if (random_freq_move_count == 0)
				{
					//saw_freq = 740;
					int random_v = my_math.my_random();
					int diff_freq = my_math.mod_i(random_v, 100);
					if ((random_v & 1) == 1)
						diff_freq = -diff_freq;

					double base_freq = (double)550 + 3.148148148148148 * (cv.wave_stat); //170.0/54.0*(wave_stat);

					double silent_random_freq = base_freq + diff_freq;

					double expect_saw_angle_freq = 2 * M_PI * silent_random_freq;
					saw_time = saw_angle_freq / expect_saw_angle_freq * saw_time;
					saw_angle_freq = expect_saw_angle_freq;
				}
				saw_value = get_saw_value(saw_time, saw_angle_freq, 0);

				random_freq_move_count++;
				if (random_freq_move_count == 100)
					random_freq_move_count = 0;
			}

			int pwm_value = get_pwm_value(sin_value, saw_value);

			Wave_Values wv;
			wv.sin_value = sin_value;
			wv.saw_value = saw_value;
			wv.pwm_value = pwm_value;

			return wv;
		}

		public static Wave_Values calculate_jre_e235_mitsubishi_sic_2_level(Control_Values cv)
		{
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);
			double amplitude = get_Amplitude(cv.wave_stat, 50);

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 95);
				if (92 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = get_Amplitude(cv.wave_stat, 66) + 0.6;
				}
				else if (86 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 71 * M_2PI))
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = get_Amplitude(cv.wave_stat, 66) + 0.3;
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(1200, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 66);
				if (77 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = get_Amplitude(cv.wave_stat, 66) + 0.6;
				}
				else if (71 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 71 * M_2PI))
				{
					pulse_mode = Pulse_Mode.P_27;
					amplitude = get_Amplitude(cv.wave_stat, 66) + 0.3;
				}
				else if (66 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 66 * M_2PI))
					pulse_mode = Pulse_Mode.P_27;
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(1200, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
				amplitude = get_Amplitude(cv.wave_stat, 60);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 60)
				{
					cv.wave_stat = 60;
					set_Control_Frequency(60);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 60)
				{
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (60 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (53 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 53 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(cv.wave_stat, 50);
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (44 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 44 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (31 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 31 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 14 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else
				{
					carrier_freq = new Carrier_Freq(365, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
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
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				amplitude = get_Amplitude(cv.wave_stat, 80);
				if (80 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (65 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 65 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(cv.wave_stat, 50);
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (50 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 50 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (30 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 30 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 14 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else if (8 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 8 * M_2PI))
					pulse_mode = Pulse_Mode.P_27;
				else
				{
					return get_Wave_Values_None();
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}
		public static Wave_Values calculate_jrw_207_update_toshiba_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (!cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 60);
				if (60 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (59 <= cv.wave_stat)
				{
					amplitude = 0.9 + 0.1 / 2.0 * (cv.wave_stat - 59);
					pulse_mode = Pulse_Mode.P_Wide_3;
				}
				else if (55 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 55 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (47 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 47 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (36 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 36 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (23 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 23 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = 550 + 3.272727272727273 * cv.wave_stat;
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 80);
				if (80 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (72 <= cv.wave_stat)
				{
					amplitude = 0.8 + 0.2 / 8.0 * (cv.wave_stat - 72);
					pulse_mode = Pulse_Mode.P_Wide_3;
				}
				else if (57 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 57 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (44 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 44 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (29 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 29 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 14 * M_2PI))
					pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = 550 + 3.272727272727273 * cv.wave_stat;
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				amplitude = get_Amplitude(cv.wave_stat, 68);
				if (59 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 59 * M_2PI))
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
				else if (50 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 50 * M_2PI))
					pulse_Mode = Pulse_Mode.P_3;
				else if (40 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 40 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(1000, 100);
				}
				else if (4 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 4 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					double expect_freq = get_changing_carrier_freq(4, 169, 40, 1000, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 100);
				}
				else
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(169, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 58);
				set_Mascon_Off_Div(20000);

				if (cv.free_run && !cv.mascon_on && cv.wave_stat > 61)
				{
					cv.wave_stat = 61;
					set_Control_Frequency(61);
				}

				else if (cv.free_run && cv.mascon_on && cv.wave_stat > 61)
				{
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
					cv.wave_stat = rolling_freq;
					set_Control_Frequency(rolling_freq);
				}

				if (51 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 51 * M_2PI))
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

				else if (42 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 42 * M_2PI))
					pulse_Mode = Pulse_Mode.SP_9;
				else if (28 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 28 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(1000, 0);
					dipolar = 1;
				}
				else if (19 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 19 * M_2PI))
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					double expect_freq = get_changing_carrier_freq(19, 500, 28, 1000, cv.wave_stat);
					carrier_freq = new Carrier_Freq(expect_freq, 0);
					dipolar = 1;
				}
				else
				{
					amplitude = get_Amplitude(cv.wave_stat, 50);
					amplitude *= 2;
					pulse_Mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(500, 0);
					dipolar = 1;
				}
			}
			return calculate_three_level(pulse_Mode, carrier_freq, cv.initial_phase, amplitude, dipolar);
		}
		public static Wave_Values calculate_jrw_321_hitachi_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 72);
				if (72 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (56 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_9;
					amplitude = 2 + (get_Amplitude(56, 72) - 2) / 16.0 * (72 - cv.wave_stat);
				}
				else
				{

					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = 1050;
					if (4 >= cv.wave_stat)
						base_freq = 510 + ((cv.wave_stat > 1) ? ((623 - 510) / 3.0 * (cv.wave_stat - 1)) : 0);
					carrier_freq = new Carrier_Freq(base_freq, 60);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 55);
				if (55 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (40 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_9;
					amplitude = 2 + (get_Amplitude(40, 55) - 2) / 15.0 * (55 - cv.wave_stat);
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(1050, 60);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
					amplitude = 1.1 + (2 - 1.1) / 8.0 * (cv.wave_stat - 48);
				}
				else
				{
					amplitude = get_Amplitude(cv.wave_stat, 48);
					pulse_mode = Pulse_Mode.Not_In_Sync;
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
					amplitude = 0.8 + (2 - 0.8) / 18.0 * (cv.wave_stat - 59);
				}
				else
				{
					amplitude = get_Amplitude(cv.wave_stat, 74);
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(1050, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
				amplitude = get_Amplitude(cv.wave_stat, 45);
				if (45 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (43 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = get_wide_3_pulse_amplitude(43, 0.5, 45, 0.8, cv.wave_stat);
				}
				else if (37 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 37 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (32 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 32 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 25 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (14 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 14 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (7 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 7 * M_2PI)) pulse_mode = Pulse_Mode.P_27;
				else if (5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 5 * M_2PI)) pulse_mode = Pulse_Mode.P_45;
				else
				{
					carrier_freq = new Carrier_Freq(200, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 60);
				if (60 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (54 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = get_wide_3_pulse_amplitude(54, 0.5, 60, 0.8, cv.wave_stat);
				}
				else if (50 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 50 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (41 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 41 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (27 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 27 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (14.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 14.5 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (8 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 8 * M_2PI)) pulse_mode = Pulse_Mode.P_27;
				else if (7 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 7 * M_2PI)) pulse_mode = Pulse_Mode.P_45;
				else if (5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 5 * M_2PI))
				{
					carrier_freq = new Carrier_Freq(200, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else
				{
					return get_Wave_Values_None();
				}
			}
			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_tokyuu_5000_hitachi_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 65);
				if (65 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (61 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 5.0 * (cv.wave_stat - 61);
				}
				else if (50 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = (double)700 + 1100 / 11.0 * (cv.wave_stat - 50); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else if (23 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = (double)740 - 40.0 / (50 - 23) * (cv.wave_stat - 23); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else if (cv.brake && cv.wave_stat <= 4)
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(200, 0);
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(740, 0);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 58);
				if (58 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (55 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 4.0 * (cv.wave_stat - 55);
				}
				else if (42 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = (double)700 + 1100 / 13.0 * (cv.wave_stat - 42); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else if (23 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = (double)740 - 40.0 / (46 - 23) * (cv.wave_stat - 23); //170.0/54.0*(cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(740, 0);
				}
			}


			if (!cv.mascon_on && cv.free_run && cv.wave_stat < 23) amplitude = 0;

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
				amplitude = get_Amplitude(cv.wave_stat, 51);
				if (51 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (50.8 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 0.2 * (cv.wave_stat - 50.8);
				}
				else if (38.3 <= cv.wave_stat && !(cv.free_run && sin_angle_freq > 50 * M_2PI))
				{
					pulse_mode = Pulse_Mode.SP_15;
					amplitude = 2 + (get_Amplitude(38.3, 50) - 2) / (50 - 38.3) * (50 - cv.wave_stat);
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = get_pattern_random((int)(400 + 180 / 38.3 * cv.wave_stat), 600, 20000);
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 47.3);
				if (47.3 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (44.4 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 4.0 * (cv.wave_stat - 44.4);
				}
				else if (35.8 <= cv.wave_stat && !(cv.free_run && sin_angle_freq > 44.4 * M_2PI))
				{
					pulse_mode = Pulse_Mode.SP_15;
					amplitude = 1.3 + (get_Amplitude(35.8, 50) - 1.3) / (44.4 - 35.8) * (44.4 - cv.wave_stat);
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = get_pattern_random((int)(400 + 180 / 34.0 * cv.wave_stat), 600, 20000); ;
					carrier_freq = new Carrier_Freq(base_freq, 0);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
				amplitude = get_Amplitude(cv.wave_stat, 50);
				if (50 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (40 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 40 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(cv.wave_stat, 50);
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (32 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 32 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 25 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else
				{
					carrier_freq = new Carrier_Freq(260, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
			}
			else
			{
				set_Mascon_Off_Div(15000);
				amplitude = get_Amplitude(cv.wave_stat, 50);
				if (50 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (40 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 40 * M_2PI))
				{
					if (cv.free_run) amplitude = get_Amplitude(cv.wave_stat, 50);
					if (amplitude > 0.97) amplitude = 0.97;
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (32 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 32 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 25 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 5 * M_2PI))
				{
					carrier_freq = new Carrier_Freq(260, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else
				{
					return get_Wave_Values_None();
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}
		public static Wave_Values calculate_kintetsu_9820_mitsubishi_igbt_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 55); ;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(0, 0);

			if (55 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (50 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_3;
			else if (13 <= cv.wave_stat)
			{
				carrier_freq = new Carrier_Freq(700, 0);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else if (cv.brake && cv.wave_stat < 8.5)
			{
				return get_Wave_Values_None();
			}
			else if (cv.wave_stat > 2)
			{
				double expect_saw_freq = 250 + (700 - 250) / 11 * (cv.wave_stat - 2);
				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else
			{
				carrier_freq = new Carrier_Freq(250, 0);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_kintetsu_9820_hitachi_igbt_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 65); ;
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
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else
			{
				carrier_freq = new Carrier_Freq(780, 0);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

















		//Keio
		public static Wave_Values calculate_keio_8000_hitachi_gto_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 68.2);
				if (68.2 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (63.5 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 6.0 * (cv.wave_stat - 63.5);
				}
				else if ((cv.free_run && sin_angle_freq > 63.5 * M_2PI) && cv.wave_stat >= 30)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 34.0 * (cv.wave_stat - 30);
				}
				else if (54.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 54.5 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (41.2 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 41.2 * M_2PI)) pulse_mode = Pulse_Mode.P_7;
				else if (32.3 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 32.3 * M_2PI)) pulse_mode = Pulse_Mode.P_11;
				else if (21.0 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 21.0 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (7.8 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 7.8 * M_2PI)) pulse_mode = Pulse_Mode.P_21;
				else
				{
					return get_Wave_Values_None();
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 50);
				if (50 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (48.7 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 2.0 * (cv.wave_stat - 48.7);
				}
				else if ((cv.free_run && sin_angle_freq > 48.7 * M_2PI) && cv.wave_stat >= 30)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 20.0 * (cv.wave_stat - 30);
				}
				else if (41.2 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 41.2 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (32.4 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 38.4 * M_2PI)) pulse_mode = Pulse_Mode.P_7;
				else if (29.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 29.5 * M_2PI)) pulse_mode = Pulse_Mode.P_11;
				else if (25.8 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 25.8 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(400, 0);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
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
				else if (cv.free_run && sin_angle_freq >= 57 * M_2PI)
				{
					pulse_mode = Pulse_Mode.P_3;
				}
				else if (57 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 57.0 * M_2PI))
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_5;
				}
				else if (53.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 53.5 * M_2PI))
				{
					a = 1; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_7;
				}
				else if (43.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 43.5 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_7;
				}
				else if (36.7 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 36.7 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_9;
				}
				else if (30 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 30 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_11;
				}
				else if (27 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 27 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_13;
				}
				else if (24 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 24 * M_2PI))
				{
					a = 1; b = 1;
					pulse_mode = Pulse_Mode.CHMP_15;
				}
				else
				{
					a = 1; b = 1;
					double expect_saw_freq = 400;
					pulse_mode = Pulse_Mode.Asyn_THI;
					if (5.6 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 5.6 * M_2PI))
						expect_saw_freq = 400;
					else if (5 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 5.0 * M_2PI))
						expect_saw_freq = 350;
					else if (4.3 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 4.3 * M_2PI))
						expect_saw_freq = 311;
					else if (3.4 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 3.4 * M_2PI))
						expect_saw_freq = 294;
					else if (2.7 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 2.7 * M_2PI))
						expect_saw_freq = 262;
					else if (2.0 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 2.0 * M_2PI))
						expect_saw_freq = 233;
					else if (1.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 1.5 * M_2PI))
						expect_saw_freq = 223;
					else if (0.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 0.5 * M_2PI))
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
					double rolling_freq = get_Rolling_Angle_Frequency() * M_1_2PI;
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
				else if(cv.free_run && sin_angle_freq >= 63.35 * M_2PI)
                {
					pulse_mode = Pulse_Mode.P_3;
                }
				else if (63.35 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 63.35 * M_2PI))
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_5;
				}
				else if (56.84 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 56.84 * M_2PI))
				{
					a = 2; b = 2;
					pulse_mode = Pulse_Mode.CHMP_Wide_7;
				}
				else if (53.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 53.5 * M_2PI))
				{
					a = 2; b = 1;
					pulse_mode = Pulse_Mode.CHMP_7;
				}
				else if (41 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 41 * M_2PI))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_7;
				}
				else if (34.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 34.5 * M_2PI))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_9;
				}
				else if (28.9 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 28.9 * M_2PI))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_11;
				}
				else if (24.9 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 24.9 * M_2PI))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_13;
				}
				else if (22.4 <= cv.wave_stat || (cv.free_run && sin_angle_freq >= 22.4 * M_2PI))
				{
					a = 2; b = 3;
					pulse_mode = Pulse_Mode.CHMP_15;
				}
				else if (cv.wave_stat > 4 || (cv.free_run && sin_angle_freq > 4 * M_2PI))
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

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_not_real_keikyu_n1000_siemens_gto_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 80);
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
					pulse_mode = Pulse_Mode.Not_In_Sync;
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
						pulse_mode = Pulse_Mode.Not_In_Sync;
					}
					else
					{
						return get_Wave_Values_None();
					}
				}
			}


			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}



















		//Toubu
		public static Wave_Values calculate_toubu_50050_hitachi_igbt_2_level(Control_Values cv)
		{

			double amplitude = get_Amplitude(cv.wave_stat, 61);
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (61 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (58 <= cv.wave_stat)
			{
				pulse_mode = Pulse_Mode.P_Wide_3;
				amplitude = 0.8 + 0.2 / 4.0 * (cv.wave_stat - 58);
			}
			else if (49 <= cv.wave_stat)
			{
				pulse_mode = Pulse_Mode.Not_In_Sync;
				double base_freq = (double)680 + 1140 / 9.0 * (cv.wave_stat - 49); //170.0/54.0*(cv.wave_stat);
				carrier_freq = new Carrier_Freq(base_freq, 0);
			}
			else if (46 <= cv.wave_stat)
			{
				pulse_mode = Pulse_Mode.Not_In_Sync;
				double base_freq = (double)730 - 50.0 / 49.0 * (cv.wave_stat); //170.0/54.0*(cv.wave_stat);
				carrier_freq = new Carrier_Freq(base_freq, 0);
			}
			else if (cv.brake && cv.wave_stat <= 4)
			{
				pulse_mode = Pulse_Mode.Not_In_Sync;
				carrier_freq = new Carrier_Freq(200, 0);
			}
			else
			{
				pulse_mode = Pulse_Mode.Not_In_Sync;
				double base_freq = (double)730 - 50.0 / 49.0 * (cv.wave_stat); //170.0/54.0*(cv.wave_stat);
				carrier_freq = new Carrier_Freq(base_freq, 100);
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}















		//Kyoto Subway
		public static Wave_Values calculate_kyoto_subway_50_mitsubishi_gto_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 63);
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
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else if (cv.brake && cv.wave_stat < 7.4)
			{
				return get_Wave_Values_None();
			}
			else if (cv.wave_stat >= 2)
			{
				double expect_saw_freq = 216 + (400 - 216) / 14 * (cv.wave_stat - 2);
				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else
			{
				carrier_freq = new Carrier_Freq(216, 0);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}







		//Nagoya Subway
		public static Wave_Values calculate_nagoya_subway_2000_update_hitachi_gto_2_level(Control_Values cv)
		{

			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 57);
				if (57 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (52 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 5 * (cv.wave_stat - 52);
				}
				else if (47 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 47 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (36 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 36 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (28 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 28 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (16.6 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 16.6 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else if (5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 5 * M_2PI))
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(250, 0);
				}
				else
				{
					return get_Wave_Values_None();
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 32);
				if (32 <= cv.wave_stat) pulse_mode = Pulse_Mode.P_1;
				else if (31 <= cv.wave_stat && !cv.free_run)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 1 * (cv.wave_stat - 31);
				}
				else if ((cv.free_run && sin_angle_freq > 31 * M_2PI) && cv.wave_stat >= 31)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 1 * (cv.wave_stat - 30);
				}
				else if (29 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 29 * M_2PI)) pulse_mode = Pulse_Mode.P_3;
				else if (25 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 25 * M_2PI)) pulse_mode = Pulse_Mode.P_5;
				else if (20 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 20 * M_2PI)) pulse_mode = Pulse_Mode.P_9;
				else if (16.6 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 16.6 * M_2PI)) pulse_mode = Pulse_Mode.P_15;
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					carrier_freq = new Carrier_Freq(250, 0);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}








		//Keihan
		public static Wave_Values calculate_keihan_13000_toyo_igbt_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 55);

			if (53 <= cv.wave_stat && cv.wave_stat <= 55)
			{
				amplitude = 5 + (get_Amplitude(53, 55) - 5) / 2.0 * (55 - cv.wave_stat);
			}

			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (55 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (34 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_9;
			else
			{
				carrier_freq = new Carrier_Freq(525, 0);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}



















		//Toei Subway
		public static Wave_Values calculate_toei_6300_mitsubishi_igbt_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 60);
			if (cv.mascon_on == false && amplitude < 0.65)
				amplitude = 0.0;

			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (60 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_1;
			else if (52.5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 52.5 * M_2PI))
				pulse_mode = Pulse_Mode.P_3;
			else if (15 <= cv.wave_stat)
			{
				carrier_freq = new Carrier_Freq(1000, 100);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else if (5 <= cv.wave_stat)
			{
				double expect_saw_freq = 220 + (1000 - 220) / 10 * (cv.wave_stat - 5);
				carrier_freq = new Carrier_Freq(expect_saw_freq, 100);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else if (cv.wave_stat <= 3 && cv.brake)
			{
				double expect_saw_freq = 205 + (220 - 205) / 3.0 * (cv.wave_stat);
				carrier_freq = new Carrier_Freq(expect_saw_freq, 100);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}
			else
			{
				carrier_freq = new Carrier_Freq(220, 100);
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
				amplitude = get_Amplitude(cv.wave_stat, 85);
				pulse_mode = Pulse_Mode.Not_In_Sync;

				double expect_saw_freq = 1190;

				if (82 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 82 * M_2PI)) expect_saw_freq = 1190;
				else if (50 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 50 * M_2PI)) expect_saw_freq = 1230;
				else if (47 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 47 * M_2PI)) expect_saw_freq = 1210;
				else if (40 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 40 * M_2PI)) expect_saw_freq = 1460;
				else if (30 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 30 * M_2PI)) expect_saw_freq = 1235;
				else if (25 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 25 * M_2PI)) expect_saw_freq = 1210;
				else if (6 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 6 * M_2PI)) expect_saw_freq = 1190;
				else if (5 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 5 * M_2PI)) expect_saw_freq = 1235;
				else return get_Wave_Values_None();

				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 85);
				pulse_mode = Pulse_Mode.Not_In_Sync;

				double expect_saw_freq = 1190;

				if (85 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 85 * M_2PI)) expect_saw_freq = 1190;
				else if (57 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 57 * M_2PI)) expect_saw_freq = 1230;
				else if (50 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 50 * M_2PI)) expect_saw_freq = 1210;
				else if (44 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 44 * M_2PI)) expect_saw_freq = 1460;
				else if (35 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 35 * M_2PI)) expect_saw_freq = 1235;
				else if (27 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 27 * M_2PI)) expect_saw_freq = 1210;
				else if (11 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 11 * M_2PI)) expect_saw_freq = 1190;
				else expect_saw_freq = 1235;

				carrier_freq = new Carrier_Freq(expect_saw_freq, 0);

			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
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
				amplitude = get_Amplitude(cv.wave_stat, 90);
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
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = (560 + 145 / 70 * cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 150);
				}

				else return get_Wave_Values_None();
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 85);
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
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = (560 + 145 / 64 * cv.wave_stat);
					carrier_freq = new Carrier_Freq(base_freq, 150);
				}

			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}


















		//Custom
		public static Wave_Values calculate_toyo_gto_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 77);

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

				else if (60 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 60 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (43 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 43 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (25 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 25 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (6 <= cv.wave_stat && cv.wave_stat <= 9)
				{
					double expect_saw_freq = (260 + (365 - 260) / 25.0 * (cv.wave_stat - 3));
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else
				{
					if (cv.wave_stat < 5)
					{
						return get_Wave_Values_None();
					}
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = 260;
					if (cv.wave_stat > 3)
						base_freq = (260 + (365 - 260) / 25.0 * (cv.wave_stat - 3));
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 60);

				if (60 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (56 <= cv.wave_stat)
				{
					pulse_mode = Pulse_Mode.P_Wide_3;
					amplitude = 0.8 + 0.2 / 5.0 * (cv.wave_stat - 56);
				}

				else if (51 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 51 * M_2PI))
					pulse_mode = Pulse_Mode.P_3;
				else if (43 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 43 * M_2PI))
					pulse_mode = Pulse_Mode.P_5;
				else if (27 <= cv.wave_stat || (cv.free_run && sin_angle_freq > 27 * M_2PI))
					pulse_mode = Pulse_Mode.P_9;
				else if (6 <= cv.wave_stat && cv.wave_stat <= 9)
				{
					double expect_saw_freq = 260 + (365 - 260) / 23.0 * (cv.wave_stat - 3);
					carrier_freq = new Carrier_Freq(expect_saw_freq, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else
				{
					pulse_mode = Pulse_Mode.Not_In_Sync;
					double base_freq = 260;
					if (cv.wave_stat > 3)
						base_freq = (260 + (365 - 260) / 23.0 * (cv.wave_stat - 3));
					carrier_freq = new Carrier_Freq(base_freq, 100);
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_toyo_igbt_2_level(Control_Values cv)
		{
			double amplitude;
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (cv.brake)
			{
				amplitude = get_Amplitude(cv.wave_stat, 98);

				if (96 <= cv.wave_stat && cv.wave_stat <= 98)
				{
					amplitude = 5 + (get_Amplitude(96, 98) - 5) / 2.0 * (98 - cv.wave_stat);
				}

				pulse_mode = Pulse_Mode.P_1;
				if (98 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (33 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_9;
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
			}
			else
			{
				amplitude = get_Amplitude(cv.wave_stat, 55);

				if (53 <= cv.wave_stat && cv.wave_stat <= 55)
				{
					amplitude = 5 + (get_Amplitude(53, 55) - 5) / 2.0 * (55 - cv.wave_stat);
				}

				pulse_mode = Pulse_Mode.P_1;
				if (55 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_1;
				else if (34 <= cv.wave_stat)
					pulse_mode = Pulse_Mode.P_9;
				else
				{
					carrier_freq = new Carrier_Freq(1045, 0);
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
			}
			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_silent_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 60);
			Pulse_Mode pulse_mode;
			Carrier_Freq carrier_freq = new Carrier_Freq(700, 0);

			if (50 <= cv.wave_stat)
				pulse_mode = Pulse_Mode.P_27;
			else
			{
				pulse_mode = Pulse_Mode.Not_In_Sync;
				carrier_freq = new Carrier_Freq(550, 100);
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}
		public static Wave_Values calculate_jre_209_mitsubishi_gto_2_level(Control_Values cv)
		{
			double amplitude = get_Amplitude(cv.wave_stat, 53);
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
				pulse_mode = Pulse_Mode.Not_In_Sync;
				carrier_freq = new Carrier_Freq(114, 0);
			}
			else if (8 < cv.wave_stat && cv.wave_stat < 18 && cv.brake)
				pulse_mode = Pulse_Mode.P_33;
			else
			{
				return get_Wave_Values_None();
			}
			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_famina_2_level(Control_Values cv)
		{

			double amplitude = get_Amplitude(cv.wave_stat, 60);
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
				pulse_mode = Pulse_Mode.Not_In_Sync;
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}

		public static Wave_Values calculate_real_doremi_2_level(Control_Values cv)
		{

			double amplitude = get_Amplitude(cv.wave_stat, 80);
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
					pulse_mode = Pulse_Mode.Not_In_Sync;
				}
				else
				{
					if (cv.wave_stat > 4)
					{
						carrier_freq = new Carrier_Freq(400, 0);
						pulse_mode = Pulse_Mode.Not_In_Sync;
					}
					else
					{
						return get_Wave_Values_None();
					}
				}
			}

			return calculate_two_level(pulse_mode, carrier_freq, cv.initial_phase, amplitude);
		}
	}
}