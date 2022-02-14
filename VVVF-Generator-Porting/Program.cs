﻿using System;
using static VVVF_Generator_Porting.vvvf_sound_definition;

using static VVVF_Generator_Porting.Generation.Generate_Control_Info;
using static VVVF_Generator_Porting.Generation.Generate_Hexagon;
using static VVVF_Generator_Porting.Generation.Generate_RealTime;
using static VVVF_Generator_Porting.Generation.Generate_Sound;
using static VVVF_Generator_Porting.Generation.Generate_Wave_Form;

namespace VVVF_Generator_Porting
{
    internal class Program
    {
        public static String get_Path()
        {
            String output_path = "";
            while (output_path.Length == 0)
            {
                Console.Write("Enter the export path : ");
                output_path = Console.ReadLine();
                if (output_path.Length == 0)
                {
                    Console.WriteLine("Error. Reenter a path.");
                }
            }
            return output_path;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Select the function to do. (Use \",\" to use multiple function)");

            String[] index = new string[]
            {
                "-SOUND RELATE",
                "Generate Sound",
                "Generate Environment Sound",

                "-WAVE FORM RELATE",
                "Generate Wave form Video",
                "Generate U V W Wave form Video",
                "Generate Taroimo like Wave form Video",

                "-HEXAGON RELATE",
                "Generate Hexagon",
                "Generate Taroimo like Hexagon",
                "Generate Hexagon Explain",
                "Generate Hexagon Image",

                "-MASCON RELATE",
                "Generate Mascon Video",
                "Generate Taroimo like Mascon Video",

                "-OTHERS",
                "Realtime VVVF Sound generation",
            };

            int count = 1;
            for(int i = 0; i < index.Length; i++)
            {
                String index_x = index[i];
                if (index_x.StartsWith("-"))
                {
                    if (count != 1) Console.WriteLine();
                    Console.WriteLine(index_x);
                }
                else
                {
                    Console.WriteLine(count.ToString() + " : " + index_x);
                    count++;
                }

                
                    
            }

            String line = Console.ReadLine();

            String[] split = line.Split(",");

            bool gen_audio = false, gen_env_audio = false;
            bool gen_U_V = false, gen_UVW = false, gen_U_V_taroimo = false;
            bool gen_hexagon = false , gen_hexagon_taroimo = false, gen_hexagon_explain = false, gen_hexagon_image = false;
            bool gen_mascon_video = false , gen_mascon_taroimo_video = false;
            bool realtime = false;

            int c = 1;
            for(int i = 0; i < split.Length; i++)
            {
                if (split[i] == c++.ToString()) gen_audio = true;
                if (split[i] == c++.ToString()) gen_env_audio = true;

                if (split[i] == c++.ToString()) gen_U_V = true;
                if (split[i] == c++.ToString()) gen_UVW = true;
                if (split[i] == c++.ToString()) gen_U_V_taroimo = true;

                if (split[i] == c++.ToString()) gen_hexagon = true;
                if (split[i] == c++.ToString()) gen_hexagon_taroimo = true;
                if (split[i] == c++.ToString()) gen_hexagon_explain = true;
                if (split[i] == c++.ToString()) gen_hexagon_image = true;

                if (split[i] == c++.ToString()) gen_mascon_video = true;
                if (split[i] == c++.ToString()) gen_mascon_taroimo_video = true;

                if (split[i] == c++.ToString()) realtime = true;
            }

            
            if(gen_audio || gen_U_V || gen_mascon_video || gen_UVW || gen_hexagon || gen_hexagon_explain || gen_hexagon_taroimo || gen_mascon_taroimo_video
                || gen_U_V_taroimo || gen_hexagon_image || gen_env_audio)
            {
                VVVF_Sound_Names sound_name = get_Choosed_Sound();
                String output_path = get_Path();

                if (gen_audio) generate_sound(output_path, sound_name);
                if (gen_env_audio) generate_env_sound(output_path, sound_name);

                if (gen_U_V) generate_wave_U_V(output_path, sound_name);
                if (gen_UVW) generate_wave_UVW(output_path, sound_name);
                if (gen_U_V_taroimo) generate_taroimo_like_wave_U_V(output_path, sound_name);


                if (gen_hexagon) generate_wave_hexagon_taroimo_like(output_path, sound_name);
                if (gen_hexagon_taroimo) generate_wave_hexagon_taroimo_like(output_path, sound_name);
                if (gen_hexagon_explain) generate_wave_hexagon_explain(output_path, sound_name);
                if (gen_hexagon_image) generate_wave_hexagon_picture(output_path, sound_name);


                if (gen_mascon_video) generate_status_video(output_path, sound_name);
                if (gen_mascon_taroimo_video) generate_status_taroimo_like_video(output_path, sound_name);

            }

            if (realtime) realtime_sound();
        }
    }
}
