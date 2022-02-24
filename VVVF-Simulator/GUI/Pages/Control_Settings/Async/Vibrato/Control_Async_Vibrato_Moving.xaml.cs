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
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Vibrato;

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Async.Vibrato
{
    /// <summary>
    /// Control_Async_Vibrato_Moving.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Async_Vibrato_Moving : UserControl
    {
        Yaml_Async_Parameter_Vibrato_Value target;
        MainWindow main;

        bool no_update = true;

        public Control_Async_Vibrato_Moving(Yaml_Async_Parameter_Vibrato_Value data, MainWindow mainWindow)
        {
            target = data;
            main = mainWindow;

            InitializeComponent();

            apply_data();

            no_update = false;
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

        private void apply_data()
        {
            DataContext = target.moving_value;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Object? tag = tb.Tag;
            if (tag == null) return;

            double d = parse_d(tb);

            if (tag.Equals("start_point"))
                target.moving_value.start = d;
            else if (tag.Equals("start_freq"))
                target.moving_value.start_value = d;
            else if (tag.Equals("end_point"))
                target.moving_value.end = d;
            else
                target.moving_value.end_value = d;
        }
    }
}
