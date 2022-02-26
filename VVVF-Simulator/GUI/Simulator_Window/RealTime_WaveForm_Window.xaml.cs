using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VVVF_Simulator.Yaml_VVVF_Sound;
using static VVVF_Simulator.Generation.Generate_RealTime;
using static VVVF_Simulator.vvvf_wave_calculate;

namespace VVVF_Simulator.GUI.Simulator_Window
{
    /// <summary>
    /// RealTime_WaveForm_Window.xaml の相互作用ロジック
    /// </summary>
    public partial class RealTime_WaveForm_Window : Window
    {
        private ViewModel view_model = new ViewModel();
        public class ViewModel : ViewModelBase
        {
            private BitmapFrame? _waveform;
            public BitmapFrame? waveform { get { return _waveform; } set { _waveform = value; RaisePropertyChanged(nameof(waveform)); } }
        };
        public class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void RaisePropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RealTime_WaveForm_Window()
        {
            DataContext = view_model;
            InitializeComponent();
            interval_update();
        }

        private void interval_update()
        {

            Task.Run(() => {
                while (!RealTime_Parameter.quit)
                {
                    System.Threading.Thread.Sleep(20);
                    generate_taroimo_like_wave_U_V();
                }
            });
        }

        private void generate_taroimo_like_wave_U_V()
        {
            Yaml_Sound_Data sound_data = RealTime_Parameter.sound_data;
            VVVF_Control_Values control = RealTime_Parameter.control_values.Clone();

            control.set_Saw_Time(0);
            control.set_Sine_Time(0);

            control.set_Allowed_Random_Freq_Move(false);

            int image_width = 1200;
            int image_height = 450;
            int calculate_div = 1;
            int wave_height = 100;

            Bitmap image = new(image_width, image_height);
            Graphics g = Graphics.FromImage(image);
            g.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, image_width, image_height);

            for (int i = 0; i < (image_width - 10) * calculate_div; i++)
            {
                Wave_Values[] values = new Wave_Values[4];

                for (int j = 0; j < 2; j++)
                {
                    Control_Values cv_U = new Control_Values
                    {
                        brake = control.is_Braking(),
                        mascon_on = !control.is_Mascon_Off(),
                        free_run = control.is_Free_Running(),
                        initial_phase = Math.PI / 6.0,
                        wave_stat = control.get_Control_Frequency()
                    };
                    Wave_Values wv_U = Yaml_VVVF_Wave.calculate_Yaml(control, cv_U, sound_data);
                    Control_Values cv_V = new Control_Values
                    {
                        brake = control.is_Braking(),
                        mascon_on = !control.is_Mascon_Off(),
                        free_run = control.is_Free_Running(),
                        initial_phase = Math.PI / 6.0 + Math.PI * 2.0 / 3.0 * 1,
                        wave_stat = control.get_Control_Frequency()
                    };
                    Wave_Values wv_V = Yaml_VVVF_Wave.calculate_Yaml(control, cv_V, sound_data);

                    if (j == 0)
                    {
                        control.add_Saw_Time(2 / (60.0 * calculate_div * (image_width - 100)));
                        control.add_Sine_Time(2 / (60.0 * calculate_div * (image_width - 100)));
                    }

                    values[j * 2] = wv_U;
                    values[j * 2 + 1] = wv_V;
                }


                int curr_val = (int)(-(values[0].pwm_value - values[1].pwm_value) * wave_height + image_height / 2.0);
                int next_val = (int)(-(values[2].pwm_value - values[3].pwm_value) * wave_height + image_height / 2.0);
                g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black, 2), (int)(i / (double)calculate_div) + 5, curr_val, (int)(((curr_val != next_val) ? i : i + 1) / (double)calculate_div) + 5, next_val);
            }

            using (Stream st = new MemoryStream())
            {
                image.Save(st, ImageFormat.Bmp);
                st.Seek(0, SeekOrigin.Begin);
                var data = BitmapFrame.Create(st, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                view_model.waveform = data;
            }
        }
    }
}
