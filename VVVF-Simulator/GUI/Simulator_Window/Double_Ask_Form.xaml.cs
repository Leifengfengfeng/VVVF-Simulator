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

namespace VVVF_Simulator.GUI.UtilForm
{
    /// <summary>
    /// Double_Ask_Form.xaml の相互作用ロジック
    /// </summary>
    public partial class Double_Ask_Form : Window
    {
        ViewData vd = new ViewData();
        private class ViewData
        {
            public String Str_Title { get; set; } = "SUS";
            public double Value { get; set; } = 0;
        }
        public Double_Ask_Form(String title)
        {
            vd.Str_Title = title;

            DataContext = vd;

            InitializeComponent();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = Num_Box;
            double d = parse_d(tb);
            if (d < 0) return;
            vd.Value = d;
            MainWindow.Generation_Params.Double_Values = new List<double>() { d };

            Close();
        }
    }
}
