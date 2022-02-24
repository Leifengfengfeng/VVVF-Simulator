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
    /// Control_Dipolar_Moving.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Dipolar_Moving : UserControl
    {
        Yaml_Control_Data target;
        MainWindow main;

        bool no_update = true;  
        public Control_Dipolar_Moving(Yaml_Control_Data target, MainWindow main)
        {
            this.target = target;
            this.main = main;

            InitializeComponent();

            DataContext = target.async_data.dipoar_data.moving_value;

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

        private void text_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Object? tag = tb.Tag;
            if (tag == null) return;

            double d = parse_d(tb);
            if (tag.Equals("start"))
                target.async_data.dipoar_data.moving_value.start = d;
            else if (tag.Equals("start_val"))
                target.async_data.dipoar_data.moving_value.start_value = d;
            else if (tag.Equals("end"))
                target.async_data.dipoar_data.moving_value.end = d;
            else
                target.async_data.dipoar_data.moving_value.end_value = d;
        }
    }
}
