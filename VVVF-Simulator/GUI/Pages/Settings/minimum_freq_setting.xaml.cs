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

namespace VVVF_Data_Generator.Pages.Settings
{
    /// <summary>
    /// minimum_freq_setting.xaml の相互作用ロジック
    /// </summary>
    public partial class minimum_freq_setting : Page
    {
        public minimum_freq_setting()
        {
            InitializeComponent();

            accelerate_min_freq_box.Text = Yaml_Generation.current_data.min_freq.accelerate.ToString();
            braking_min_freq_box.Text = Yaml_Generation.current_data.min_freq.braking.ToString();
        }

        private void textbox_value_change(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox) sender;
            Object? tag = tb.Tag;
            if (tag == null) return;

            if (tag.Equals("Accelerate"))
            {
                try
                {
                    double d = Double.Parse(accelerate_min_freq_box.Text);
                    accelerate_min_freq_box.Background = new BrushConverter().ConvertFrom("#FFFFFFFF") as Brush;

                    Yaml_Generation.current_data.min_freq.accelerate = d;
                }
                catch
                {
                    accelerate_min_freq_box.Background = new BrushConverter().ConvertFrom("#FFfed0d0") as Brush;
                }
            }else if (tag.Equals("Brake"))
            {
                try
                {
                    double d = Double.Parse(braking_min_freq_box.Text);
                    accelerate_min_freq_box.Background = new BrushConverter().ConvertFrom("#FFFFFFFF") as Brush;

                    Yaml_Generation.current_data.min_freq.braking = d;
                }
                catch
                {
                    accelerate_min_freq_box.Background = new BrushConverter().ConvertFrom("#FFfed0d0") as Brush;
                }
            }

        }
    }
}
