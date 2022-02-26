using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace VVVF_Simulator.GUI.Simulator_Window.RealTime_Generation
{
    /// <summary>
    /// RealTime_Settings.xaml の相互作用ロジック
    /// </summary>
    public partial class RealTime_Settings : Window
    {
        bool no_update = true;
        public RealTime_Settings()
        {
            InitializeComponent();
            apply_data();

            no_update = false;
        }

        private void apply_data()
        {
            audio_buff_box.Text = Properties.Settings.Default.G_RealTime_Buff.ToString();
            show_waveform_box.IsChecked = Properties.Settings.Default.G_RealTime_WaveForm;
            realtime_edit_box.IsChecked = Properties.Settings.Default.G_RealTime_Edit;
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

        private void show_waveform_box_Checked(object sender, RoutedEventArgs e)
        {
            if (no_update) return;
            CheckBox cb = (CheckBox)sender;
            Object tag = cb.Tag;

            Boolean is_checked = cb.IsChecked == true;

            if (tag.Equals("WaveForm"))
                Properties.Settings.Default.G_RealTime_WaveForm = is_checked;
            else if(tag.Equals("Edit"))
                Properties.Settings.Default.G_RealTime_Edit = is_checked;
        }

        private void audio_buff_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;


            int i = parse_i(audio_buff_box);
            Properties.Settings.Default.G_RealTime_Buff = i;
        }
    }
}
