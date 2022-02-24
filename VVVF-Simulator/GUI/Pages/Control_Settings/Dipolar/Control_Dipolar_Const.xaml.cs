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

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Dipolar
{
    /// <summary>
    /// Control_Dipolar_Const.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Dipolar_Const : UserControl
    {
        Yaml_Control_Data target;
        MainWindow main;

        bool no_update = true;
        public Control_Dipolar_Const(Yaml_Control_Data target, MainWindow main)
        {
            this.target = target;
            this.main = main;

            InitializeComponent();

            const_box.Text = target.async_data.dipoar_data.const_value.ToString();

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

        private void const_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;

            TextBox tb = (TextBox)sender;
            double v = parse_d(tb);
            target.async_data.dipoar_data.const_value = v;
        }
    }
}
