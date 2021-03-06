using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using static VVVF_Simulator.Generation.Generate_RealTime;
using static VVVF_Simulator.vvvf_wave_calculate;

namespace VVVF_Simulator.GUI.UtilForm
{
    /// <summary>
    /// Mascon.xaml の相互作用ロジック
    /// </summary>
    public partial class RealTime_Mascon_Window : Window
    {
        public RealTime_Mascon_Window()
        {
            InitializeComponent();
            set_Stat(0);
            interval_update();
            DataContext = view_model;
        }

        private void interval_update()
        {

            Task.Run(() => {
                while (!RealTime_Parameter.quit)
                {
                    System.Threading.Thread.Sleep(20);
                    view_model.sine_freq = RealTime_Parameter.control_values.get_Sine_Freq();
                    view_model.pulse_state = get_Pulse_Name();
                }
            });
            Task.Run(() => {
                double pre_voltage = 0.0;
                while (!RealTime_Parameter.quit)
                {
                    VVVF_Control_Values control = RealTime_Parameter.control_values.Clone();
                    control.set_Saw_Time(0);
                    control.set_Sine_Time(0);
                    double voltage = Generation.Generate_Control_Info.get_voltage_rate_with_radius_from_surface(RealTime_Parameter.sound_data, control);
                    double avg_voltage = Math.Round((pre_voltage + voltage) / 2.0, 2);
                    view_model.voltage = avg_voltage;
                    pre_voltage = voltage;
                }
            });
        }

        private ViewModel view_model = new ViewModel();
        public class ViewModel : ViewModelBase
        {
            private Brush _B3 = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0));
            public Brush B3 { get { return _B3; } set { _B3 = value; RaisePropertyChanged(nameof(B3)); } }

            private Brush _B2 = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0));
            public Brush B2 { get { return _B2; } set { _B2 = value; RaisePropertyChanged(nameof(B2)); } }

            private Brush _B1 = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0));
            public Brush B1 { get { return _B1; } set { _B1 = value; RaisePropertyChanged(nameof(B1)); } }

            private Brush _N = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0));
            public Brush N { get { return _N; } set { _N = value; RaisePropertyChanged(nameof(N)); } }

            private Brush _P3 = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0));
            public Brush P3 { get { return _P3; } set { _P3 = value; RaisePropertyChanged(nameof(P3)); } }

            private Brush _P2 = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0));
            public Brush P2 { get { return _P2; } set { _P2 = value; RaisePropertyChanged(nameof(P2)); } }

            private Brush _P1 = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0));
            public Brush P1 { get { return _P1; }  set { _P1 = value; RaisePropertyChanged(nameof(P1)); } }

            private double _sine_freq = RealTime_Parameter.control_values.get_Sine_Freq();
            public double sine_freq { get { return _sine_freq; } set { _sine_freq = value; RaisePropertyChanged(nameof(sine_freq)); } }

            private double _voltage = RealTime_Parameter.control_values.get_Sine_Freq();
            public double voltage { get { return _voltage; } set { _voltage = value; RaisePropertyChanged(nameof(voltage)); } }

            private String _pulse_state = Pulse_Mode.Async.ToString();
            public String pulse_state { get { return _pulse_state; } set { _pulse_state = value; RaisePropertyChanged(nameof(pulse_state)); } }
        };
        public class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void RaisePropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private String get_Pulse_Name()
        {
            Pulse_Mode mode = RealTime_Parameter.control_values.get_Video_Pulse_Mode();
            //Not in sync
            if (mode == Pulse_Mode.Async || mode == Pulse_Mode.Async_THI)
            {
                Carrier_Freq carrier_freq_data = RealTime_Parameter.control_values.get_Video_Carrier_Freq_Data();
                String default_s = String.Format(carrier_freq_data.base_freq.ToString("F2"));
                return default_s;
            }

            //Abs
            if (mode == Pulse_Mode.P_Wide_3)
                return "W 3";

            if (mode.ToString().StartsWith("CHM"))
            {
                String mode_name = mode.ToString();
                bool contain_wide = mode_name.Contains("Wide");
                mode_name = mode_name.Replace("_Wide", "");

                String[] mode_name_type = mode_name.Split("_");

                String final_mode_name = ((contain_wide) ? "W " : "") + mode_name_type[1];

                return "CHM " + final_mode_name;
            }
            if (mode.ToString().StartsWith("SHE"))
            {
                String mode_name = mode.ToString();
                bool contain_wide = mode_name.Contains("Wide");
                mode_name = mode_name.Replace("_Wide", "");

                String[] mode_name_type = mode_name.Split("_");

                String final_mode_name = (contain_wide) ? "W " : "" + mode_name_type[1];

                return "SHE " + final_mode_name;
            }
            else
            {
                String[] mode_name_type = mode.ToString().Split("_");
                return mode_name_type[1];
            }
        }

        private void set_Color(int c,Color col)
        {
            SolidColorBrush brush = new SolidColorBrush(col);
            if (c == 0) view_model.N = brush;

            if (c == -1) view_model.B1 = brush;
            if (c == -2) view_model.B2 = brush;
            if (c == -3) view_model.B3 = brush;

            if (c == 1) view_model.P1 = brush;
            if (c == 2) view_model.P2 = brush;
            if (c == 3) view_model.P3 = brush;
        }

        private void set_Stat(int at)
        {
            for (int i = -3; i < 4; i++)
            {
                Color c = Color.FromRgb(0xA0, 0xA0, 0xA0);
                if (i == at)
                {
                    if (i > 0) c = Color.FromRgb(0x34, 0xb7, 0xeb);
                    else if (i < 0) c = Color.FromRgb(0xf0, 0xa6, 0x30);
                    else c = Color.FromRgb(0x4c, 0xeb, 0x34);
                }
                set_Color(i, c);
            }

            RealTime_Parameter.change_amount = at * Math.PI * 0.0003;
            RealTime_Parameter.free_run = at == 0;

            bool pre_braking = RealTime_Parameter.braking;
            RealTime_Parameter.braking = (at == 0) ? pre_braking : at < 0;
        }

        private int current_stat = 0;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;

            if (key.Equals(Key.S))
                current_stat++;
            else if (key.Equals(Key.W))
                current_stat--;

            if (current_stat > 3) current_stat = 3;
            if (current_stat < -3) current_stat = -3;

            set_Stat(current_stat);

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            RealTime_Parameter.quit = true;
        }
    }
}
