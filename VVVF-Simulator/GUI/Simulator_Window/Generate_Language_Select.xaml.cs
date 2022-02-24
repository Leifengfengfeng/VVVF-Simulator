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
using System.Windows.Shapes;
using static VVVF_Simulator.Generation.Generate_Control_Info;

namespace VVVF_Simulator.GUI.UtilForm
{
    /// <summary>
    /// Generate_Language_Select.xaml の相互作用ロジック
    /// </summary>
    public partial class Generate_Language_Select : Window
    {
        MainWindow main;
        public Generate_Language_Select(MainWindow main)
        {
            this.main = main;

            InitializeComponent();

            Taroimo_Status_Language_Mode[] modes = (Taroimo_Status_Language_Mode[])Enum.GetValues(typeof(Taroimo_Status_Language_Mode));

            string_lang_selector.ItemsSource = modes;
            string_lang_selector.SelectedItem = modes[0];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Taroimo_Status_Language_Mode selected = (Taroimo_Status_Language_Mode)string_lang_selector.SelectedItem;
            main.gen_param.Video_Language = new() { selected, selected };
            Close();
        }
    }
}
