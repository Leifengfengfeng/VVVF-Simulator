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
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Free_Run_Condition;

namespace VVVF_Yaml_Generator.Pages.Control_Settings
{
    /// <summary>
    /// Control_When_FreeRun.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_When_FreeRun : UserControl
    {
        Yaml_Control_Data target;
        MainWindow MainWindow;

        private bool no_update = true;

        public Control_When_FreeRun(Yaml_Control_Data ycd, MainWindow mainWindow)
        {
            InitializeComponent();
            target = ycd;
            MainWindow = mainWindow;

            apply_view();

            no_update = false;
        }

        private void apply_view()
        {
            on_skip.IsChecked = target.when_freerun.on.skip;
            off_skip.IsChecked = target.when_freerun.off.skip;

            on_stuck.IsChecked = target.when_freerun.on.stuck_at_here;
            off_stuck.IsChecked = target.when_freerun.off.stuck_at_here;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (no_update) return;

            CheckBox cb = (CheckBox)sender;
            Object? tag = cb.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;

            String[] mode = tag_str.Split("_");

            Yaml_Free_Run_Condition_Single condition;
            if (mode[0].Equals("ON")) condition = target.when_freerun.on;
            else condition = target.when_freerun.off;

            bool is_cheked = (cb.IsChecked == false) ? false : true;

            if (mode[1].Equals("Stuck"))
            {
                condition.stuck_at_here = is_cheked;
            }
            else
            {
                condition.skip = is_cheked;
            }

            MainWindow.update_Control_List_View();

        }
    }
}
