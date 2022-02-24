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
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Vibrato;

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Async.Vibrato
{
    /// <summary>
    /// Control_Async_Vibrato_Const.xaml の相互作用ロジック
    /// </summary>
    /// 
    
    public partial class Control_Async_Vibrato_Const : UserControl
    {
        Yaml_Async_Parameter_Vibrato_Value target;
        MainWindow main;

        bool no_update = true;

        public Control_Async_Vibrato_Const(Yaml_Async_Parameter_Vibrato_Value data, MainWindow mainWindow)
        {
            target = data;
            main = mainWindow;

            InitializeComponent();

            apply_data();

            no_update = false;
        }

        private void apply_data()
        {
            const_box.Text = target.const_value.ToString();
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

        private void const_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;

            int v = parse_i((TextBox)sender);
            target.const_value = v;
        }
    }
}
