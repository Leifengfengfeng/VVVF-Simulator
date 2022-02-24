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

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Async.Random_Range
{
    /// <summary>
    /// Control_Async_Random_Moving.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Async_Random_Moving : UserControl
    {
        Yaml_Control_Data target;
        MainWindow main;

        bool no_update = true;

        public Control_Async_Random_Moving(Yaml_Control_Data data, MainWindow mainWindow)
        {
            target = data;
            main = mainWindow;

            DataContext = data.async_data.random_range.moving_value;

            InitializeComponent();
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
        private void text_changed(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;
            TextBox tb = (TextBox)sender;
            Object? tag = tb.Tag;
            if (tag == null) return;

            var moving_set = target.async_data.random_range.moving_value;
            if (tag.Equals("start"))
                moving_set.start = parse_d(tb);
            else if (tag.Equals("start_val"))
                moving_set.start_value = parse_i(tb);
            else if (tag.Equals("end"))
                moving_set.end = parse_d(tb);
            else
                moving_set.end_value = parse_i(tb);

            main.update_Control_List_View();
        }
    }
}
