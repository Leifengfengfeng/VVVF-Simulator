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

namespace VVVF_Data_Generator.Pages.Settings
{
    /// <summary>
    /// level_setting.xaml の相互作用ロジック
    /// </summary>
    public partial class level_setting : Page
    {
        public level_setting()
        {
            InitializeComponent();

            if (Yaml_Generation.current_data.level == 2)
                level_image.Source = new BitmapImage(new Uri("../../Images/Settings/2-level.png", UriKind.Relative));
            else
                level_image.Source = new BitmapImage(new Uri("../../Images/Settings/3-level.png", UriKind.Relative));

        }

        private void level_button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            String? tag = btn.Tag.ToString();
            if (tag == null) return;

            if (tag.Equals("2"))
                level_image.Source = new BitmapImage(new Uri("../../Images/Settings/2-level.png", UriKind.Relative));
            else
                level_image.Source = new BitmapImage(new Uri("../../Images/Settings/3-level.png", UriKind.Relative));

            Yaml_Generation.current_data.level = Int32.Parse(tag);
        }
    }
}
