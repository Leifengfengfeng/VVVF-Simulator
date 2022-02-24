using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VVVF_Data_Generator;
using static VVVF_Data_Generator.Yaml_Sound_Data;
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data;

namespace VVVF_Yaml_Generator.Pages.Control_Settings
{
    /// <summary>
    /// Control_Basic.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Basic : UserControl
    {
        private Yaml_Control_Data target;
        private MainWindow MainWindow;

        private bool no_update = true;
        public Control_Basic(Yaml_Control_Data ycd, MainWindow mainWindow)
        {
            InitializeComponent();

            target = ycd;
            MainWindow = mainWindow;

            apply_view();

            no_update = false;
        }

        private double parse(TextBox tb)
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;

            TextBox tb = (TextBox)sender;
            Object? tag = tb.Tag;
            if (tag == null) return;

            if (tag.Equals("From"))
            {
                double parsed = parse(tb);
                target.from = parsed;
                MainWindow.update_Control_List_View();
            }
        }

        private void apply_view()
        {
            from_text_box.Text = target.from.ToString();

            Pulse_Mode[] result = (Pulse_Mode[])Enum.GetValues(typeof(Pulse_Mode));
            pulse_name_selector.ItemsSource = result;
            pulse_name_selector.SelectedItem = target.pulse_Mode;

            enable_on_free_check.IsChecked = target.enable_on_free_run;
            enable_on_normal_check.IsChecked = target.enable_on_not_free_run;
        }

        private void pulse_name_selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no_update) return;

            Pulse_Mode selected = (Pulse_Mode)pulse_name_selector.SelectedItem;
            target.pulse_Mode = selected;

            MainWindow.update_Control_List_View();
            MainWindow.update_Control_Showing();


        }

        private void enable_checked(object sender, RoutedEventArgs e)
        {
            if (no_update) return;

            CheckBox tb = (CheckBox)sender;
            Object? tag = tb.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;

            bool check = (tb.IsChecked == false) ? false : true;

            if (tag_str.Equals("Normal"))
            {
                target.enable_on_not_free_run = check;
            }
            else
            {
                target.enable_on_free_run = check;
            }

            MainWindow.update_Control_List_View();
        }
    }
}
