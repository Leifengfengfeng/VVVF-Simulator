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
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Dipolar;

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Dipolar
{
    /// <summary>
    /// Control_Dipolar.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Dipolar : UserControl
    {
        Yaml_Control_Data target;
        MainWindow main;

        bool no_update = true;

        public Control_Dipolar(Yaml_Control_Data ycd, MainWindow mainWindow)
        {
            target = ycd;
            main = mainWindow;

            InitializeComponent();

            apply_data();

            no_update = false;
        }

        private void apply_data()
        {
            Yaml_Async_Parameter_Dipolar_Mode[] modes = (Yaml_Async_Parameter_Dipolar_Mode[])Enum.GetValues(typeof(Yaml_Async_Parameter_Dipolar_Mode));
            dipolar_mode.ItemsSource = modes;
            dipolar_mode.SelectedItem = target.async_data.dipoar_data.value_mode;

            set_Selected(target.async_data.dipoar_data.value_mode);
        }

        private void dipolar_mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no_update) return;

            Yaml_Async_Parameter_Dipolar_Mode selected = (Yaml_Async_Parameter_Dipolar_Mode)dipolar_mode.SelectedItem;

            target.async_data.dipoar_data.value_mode = selected;

            set_Selected(selected);
        }

        private void set_Selected(Yaml_Async_Parameter_Dipolar_Mode selected)
        {
            if (selected == Yaml_Async_Parameter_Dipolar_Mode.Const)
                dipolar_param.Navigate(new Control_Dipolar_Const(target, main));
            else
                dipolar_param.Navigate(new Control_Dipolar_Moving(target, main));
        }
    }
}
