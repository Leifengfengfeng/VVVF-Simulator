using System;
using static VVVF_Generator_Porting.vvvf_wave_calculate;
using static VVVF_Generator_Porting.vvvf_wave;

namespace VVVF_Generator_Porting
{
    public class vvvf_sound_definition
    {

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

            SOUND_TOEI_5300_MITSUBISHI_GTO_2_LEVEL,
            SOUND_TOEI_6300_MITSUBISHI_GTO_2_LEVEL,
            SOUND_TOEI_6300_MITSUBISHI_IGBT_2_LEVEL,

            SOUND_WMATA_6000_ALSTOM_IGBT_2_LEVEL,
            SOUND_WMATA_7000_TOSHIBA_IGBT_2_LEVEL,

            SOUND_X_TOYO_GTO_2_LEVEL,
            SOUND_X_TOYO_IGBT_2_LEVEL,
            SOUND_X_SILENT_2_LEVEL,
            SOUND_JRE_209_MITSUBISHI_GTO_2_LEVEL,
            SOUND_X_FAMINA_2_LEVEL,
            SOUND_X_PULSE_TEST_2_LEVEL,
            SOUND_DATA_FILE_2_LEVEL,
            SOUND_DATA_FILE_3_LEVEL,
        }

        public static Wave_Values get_Calculated_Value(VVVF_Sound_Names name, Control_Values cv)
        {
            switch (name) {

                //JR EAST
                case VVVF_Sound_Names.SOUND_JRE_209_MITSUBISHI_GTO_3_LEVEL : return calculate_jre_209_mitsubishi_gto_3_level(cv);
                case VVVF_Sound_Names.SOUND_JRE_E231_MITSUBISHI_IGBT_3_LEVEL: return calculate_jre_e231_mitsubishi_igbt_3_level(cv);
                case VVVF_Sound_Names.SOUND_JRE_E231_1000_HITACHI_IGBT_2_LEVEL : return calculate_jre_e231_1000_hitachi_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRE_E233_MITSUBISHI_IGBT_2_LEVEL : return calculate_jre_e233_mitsubishi_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRE_E233_3000_HITACHI_IGBT_2_LEVEL : return calculate_jre_e233_3000_hitachi_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRE_E235_TOSHIBA_SIC_2_LEVEL : return calculate_jre_e235_toshiba_sic_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRE_E235_MITSUBISHI_SIC_2_LEVEL : return calculate_jre_e235_mitsubishi_sic_2_level(cv);

                //JR WEST
                case VVVF_Sound_Names.SOUND_JRW_207_TOSHIBA_GTO_2_LEVEL : return calculate_jrw_207_toshiba_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRW_207_UPDATE_TOSHIBA_IGBT_2_LEVEL : return calculate_jrw_207_update_toshiba_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRW_223_2000_HITACHI_IGBT_3_LEVEL : return calculate_jrw_223_2000_hitachi_igbt_3_level(cv);
                case VVVF_Sound_Names.SOUND_JRW_321_HITACHI_IGBT_2_LEVEL : return calculate_jrw_321_hitachi_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRW_225_5100_MITSUBISHI_IGBT_2_LEVEL : return calculate_jrw_225_5100_mitsubishi_igbt_2_level(cv);

                //TOKYUU
                case VVVF_Sound_Names.SOUND_TOKYUU_9000_HITACHI_GTO_2_LEVEL : return calculate_tokyuu_9000_hitachi_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_TOKYUU_5000_HITACHI_IGBT_2_LEVEL : return calculate_tokyuu_5000_hitachi_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_TOKYUU_1000_1500_UPDATE_TOSHIBA_IGBT_2_LEVEL:  return calculate_tokyuu_1000_1500_update_toshiba_igbt_2_level(cv);

                //KINTETSU
                case VVVF_Sound_Names.SOUND_KINTETSU_5800_MITSUBISHI_GTO_2_LEVEL : return calculate_kintetsu_5800_mitsubishi_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_KINTETSU_9820_MITSUBISHI_IGBT_2_LEVEL : return calculate_kintetsu_9820_mitsubishi_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_KINTETSU_9820_HITACHI_IGBT_2_LEVEL : return calculate_kintetsu_9820_hitachi_igbt_2_level(cv);

                //KEIO
                case VVVF_Sound_Names.SOUND_KEIO_8000_HITACHI_GTO_2_LEVEL : return calculate_keio_8000_hitachi_gto_2_level(cv);
                    
                //KEIKYUU
                case VVVF_Sound_Names.SOUND_KEIKYU_N1000_SIEMENS_GTO_2_LEVEL : return calculate_keikyu_n1000_siemens_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_KEIKYU_N1000_SIEMENS_IGBT_2_LEVEL : return calculate_keikyu_n1000_siemens_igbt_2_level(cv);

                //TOBU
                case VVVF_Sound_Names.SOUND_TOUBU_50050_HITACHI_IGBT_2_LEVEL : return calculate_toubu_50050_hitachi_igbt_2_level(cv);

                //KYOTO SUBWAY
                case VVVF_Sound_Names.SOUND_KYOTO_SUBWAY_50_MITSUBISHI_GTO_2_LEVEL : return calculate_kyoto_subway_50_mitsubishi_gto_2_level(cv);

                //NAGOYA SUBWAY
                case VVVF_Sound_Names.SOUND_NAGOYA_SUBWAY_2000_UPDATE_HITACHI_GTO_2_LEVEL : return calculate_nagoya_subway_2000_update_hitachi_gto_2_level(cv);

                //KEIHAN
                case VVVF_Sound_Names.SOUND_KEIHAN_13000_TOYO_IGBT_2_LEVEL : return calculate_keihan_13000_toyo_igbt_2_level(cv);

                //TOEI SUBWAY
                case VVVF_Sound_Names.SOUND_TOEI_5300_MITSUBISHI_GTO_2_LEVEL : return calculate_toei_5300_mitsubishi_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_TOEI_6300_MITSUBISHI_GTO_2_LEVEL : return calculate_toei_6300_mitsubishi_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_TOEI_6300_MITSUBISHI_IGBT_2_LEVEL : return calculate_toei_6300_mitsubishi_igbt_2_level(cv);

                //WMATA
                case VVVF_Sound_Names.SOUND_WMATA_6000_ALSTOM_IGBT_2_LEVEL : return calculate_wmata_6000_alstom_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_WMATA_7000_TOSHIBA_IGBT_2_LEVEL : return calculate_wmata_7000_toshiba_igbt_2_level(cv);

                //custom sounds
                case VVVF_Sound_Names.SOUND_X_TOYO_GTO_2_LEVEL : return calculate_toyo_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_X_TOYO_IGBT_2_LEVEL : return calculate_toyo_igbt_2_level(cv);
                case VVVF_Sound_Names.SOUND_X_SILENT_2_LEVEL : return calculate_silent_2_level(cv);
                case VVVF_Sound_Names.SOUND_JRE_209_MITSUBISHI_GTO_2_LEVEL : return calculate_jre_209_mitsubishi_gto_2_level(cv);
                case VVVF_Sound_Names.SOUND_X_FAMINA_2_LEVEL : return calculate_famina_2_level(cv);
                case VVVF_Sound_Names.SOUND_X_PULSE_TEST_2_LEVEL : return calculate_pulse_test_2_level(cv);
                case VVVF_Sound_Names.SOUND_DATA_FILE_2_LEVEL: return calculate_data_file_2_level(cv);
                case VVVF_Sound_Names.SOUND_DATA_FILE_3_LEVEL: return calculate_data_file_3_level(cv);
                default : return calculate_silent_2_level(cv);
            }

        }

        public static String get_Sound_Name(VVVF_Sound_Names name)
        {
            String name_str = name.ToString();
            String[] pattern = name_str.Split("_");

            String final_name = "";
            for (int i = 1; i < pattern.Length - 2; i++)
            {
                final_name += pattern[i];
                if (i + 1 < pattern.Length - 2)
                {
                    final_name += "_";
                }
            }

            final_name += "(" + pattern[pattern.Length - 2] + "-" + pattern[pattern.Length - 1] + ")";

            return final_name;
        }

        public static VVVF_Sound_Names get_Choosed_Sound()
        {
            VVVF_Sound_Names sound_name = VVVF_Sound_Names.SOUND_JRE_E231_1000_HITACHI_IGBT_2_LEVEL;
            Console.WriteLine("Select sound");
            int enum_len = Enum.GetNames(typeof(VVVF_Sound_Names)).Length;
            for (int i = 0; i < enum_len; i++)
            {
                Console.WriteLine(i.ToString() + " : " + get_Sound_Name((VVVF_Sound_Names)i));
            }

            while (true)
            {
                String val = Console.ReadLine();
                int val_i = 0;
                try
                {
                    val_i = Int32.Parse(val);
                }
                catch
                {
                    Console.WriteLine("Invalid value.");
                    continue;
                }
                if (enum_len <= val_i)
                {
                    Console.WriteLine("Invalid value.");
                    continue;
                }
                else
                {
                    sound_name = (VVVF_Sound_Names)val_i;
                    break;
                }
            }
            Console.WriteLine(sound_name.ToString() + " was selected");

            return sound_name;
        }

    }
}
