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

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Async
{
    /// <summary>
    /// Control_Async_Moving.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Async_Carrier_Moving : UserControl
    {
        Yaml_Control_Data target;
        MainWindow main;

        bool no_update = true;
        public Control_Async_Carrier_Moving(Yaml_Control_Data data, MainWindow mainWindow)
        {
            target = data;
            main = mainWindow;

            InitializeComponent();

            apply_data();
            no_update = false;
        }

        private void apply_data()
        {
            var moving_val = target.async_data.carrier_wave_data.moving_value;

            DataContext = moving_val;
        }

        private double parse_d(TextBox tb)
        {
            try
            {
                tb.Background = new BrushConverter().ConvertFrom("#FFFFFFFF") as Brush;
                return Double.Parse(tb.Text);
            }
            catch
            {
                tb.Background = new BrushConverter().ConvertFrom("#FFfed0d0") as Brush;
                return -1;
            }
        }
        private void text_changed(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;
            TextBox tb = (TextBox)sender;
            Object? tag = tb.Tag;
            if (tag == null) return;

            if (tag.Equals("start"))
                target.async_data.carrier_wave_data.moving_value.start = parse_d(tb);
            else if (tag.Equals("start_hz"))
                target.async_data.carrier_wave_data.moving_value.start_value = parse_d(tb);
            else if (tag.Equals("end"))
                target.async_data.carrier_wave_data.moving_value.end = parse_d(tb);
            else
                target.async_data.carrier_wave_data.moving_value.end_value = parse_d(tb);

            main.update_Control_List_View();
        }
    }
}
