using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VVVF_Data_Generator;
using static VVVF_Data_Generator.Yaml_Sound_Data;
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Vibrato.Yaml_Async_Parameter_Vibrato_Value;

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Async.Vibrato
{
    /// <summary>
    /// Control_Async_Vibrato.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Async_Vibrato : UserControl
    {
        Yaml_Control_Data target;
        MainWindow main;

        bool no_update = true;
        public Control_Async_Vibrato(Yaml_Control_Data data, MainWindow mainWindow)
        {
            target = data;
            main = mainWindow;

            InitializeComponent();

            apply_data();

            no_update = false;
        }

        private void apply_data()
        {
            Yaml_Async_Parameter_Vibrato_Mode[] modes = (Yaml_Async_Parameter_Vibrato_Mode[])Enum.GetValues(typeof(Yaml_Async_Parameter_Vibrato_Mode));
            highest_mode.ItemsSource = modes;
            lowest_mode.ItemsSource = modes;

            highest_mode.SelectedItem = target.async_data.carrier_wave_data.vibrato_value.highest.mode;
            set_Selected(true, target.async_data.carrier_wave_data.vibrato_value.highest.mode);

            lowest_mode.SelectedItem = target.async_data.carrier_wave_data.vibrato_value.lowest.mode;
            set_Selected(false, target.async_data.carrier_wave_data.vibrato_value.lowest.mode);

            interval.Text = target.async_data.carrier_wave_data.vibrato_value.interval.ToString();
        }

        private int parse_i(TextBox tb)
        {
            try
            {
                tb.Background = new BrushConverter().ConvertFrom("#FFFFFFFF") as Brush;
                return Int32.Parse(tb.Text);
            }
            catch
            {
                tb.Background = new BrushConverter().ConvertFrom("#FFfed0d0") as Brush;
                return -1;
            }
        }

        private void interval_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;

            int v = parse_i((TextBox)sender);

            target.async_data.carrier_wave_data.vibrato_value.interval = v;
        }

        private void selection_change(object sender, SelectionChangedEventArgs e)
        {
            if (no_update) return;

            ComboBox cb = (ComboBox)sender;
            Object? tag = cb.Tag;

            Yaml_Async_Parameter_Vibrato_Mode mode = (Yaml_Async_Parameter_Vibrato_Mode)cb.SelectedItem;
            if (tag.Equals("Highest"))
            {
                target.async_data.carrier_wave_data.vibrato_value.highest.mode = mode;
                set_Selected(true, mode);
            }
            else
            {
                target.async_data.carrier_wave_data.vibrato_value.lowest.mode = mode;
                set_Selected(false, mode);
            }
                
        }

        private void set_Selected(bool high, Yaml_Async_Parameter_Vibrato_Mode mode)
        {
            if (high)
            {
                if(mode == Yaml_Async_Parameter_Vibrato_Mode.Const)
                    highest_param_frame.Navigate(new Control_Async_Vibrato_Const(target.async_data.carrier_wave_data.vibrato_value.highest, main));
                else
                    highest_param_frame.Navigate(new Control_Async_Vibrato_Moving(target.async_data.carrier_wave_data.vibrato_value.highest, main));
            }
            else
            {
                if (mode == Yaml_Async_Parameter_Vibrato_Mode.Const)
                    lowest_param_frame.Navigate(new Control_Async_Vibrato_Const(target.async_data.carrier_wave_data.vibrato_value.lowest, main));
                else
                    lowest_param_frame.Navigate(new Control_Async_Vibrato_Moving(target.async_data.carrier_wave_data.vibrato_value.lowest, main));
            }
        }
    }
}
