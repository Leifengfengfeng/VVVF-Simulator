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

namespace VVVF_Yaml_Generator.Pages.Control_Settings
{
    /// <summary>
    /// Page_Control_Common_Async.xaml の相互作用ロジック
    /// </summary>
    public partial class Level_2_Page_Control_Common_Async : Page
    {
        private Yaml_Control_Data target;
        private MainWindow MainWindow;

        public Level_2_Page_Control_Common_Async(Yaml_Control_Data ycd, MainWindow mainWindow)
        {
            InitializeComponent();

            target = ycd;
            MainWindow = mainWindow;

            Control_Basic.Navigate(new Control_Basic(ycd, mainWindow));
            Control_When_FreeRun.Navigate(new Control_When_FreeRun(ycd, mainWindow));

            Control_Amplitude_Default.Navigate(new Control_Amplitude(ycd.amplitude_control.default_data, Control_Amplitude_Content.Default, mainWindow));

            Control_Amplitude_FreeRun_On.Navigate(new Control_Amplitude(ycd.amplitude_control.free_run_data.mascon_on, Control_Amplitude_Content.Free_Run_On, mainWindow));
            Control_Amplitude_FreeRun_Off.Navigate(new Control_Amplitude(ycd.amplitude_control.free_run_data.mascon_off, Control_Amplitude_Content.Free_Run_Off, mainWindow));

            Control_Async.Navigate(new Control_Async(ycd, mainWindow));
        }
    }
}
