using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Control_Data_Amplitude_Control;
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Control_Data_Amplitude_Control.Yaml_Control_Data_Amplitude;

namespace VVVF_Yaml_Generator.Pages.Control_Settings
{
    /// <summary>
    /// Control_Amplitude.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Amplitude : UserControl
    {
        private Yaml_Control_Data_Amplitude target;
        private MainWindow mainWindow;
        private Control_Amplitude_Content content;

        private bool no_update = true;
        private Visible_Class visible_Class;

        public class Visible_Class : ViewModelBase
        {
            private bool _start_freq_visible = true;
            public bool start_freq_visible { get { return _start_freq_visible; } set { _start_freq_visible = value; RaisePropertyChanged(nameof(start_freq_visible)); } }
            
            private bool _start_amp_visible = true;
            public bool start_amp_visible { get { return _start_amp_visible; } set { _start_amp_visible = value; RaisePropertyChanged(nameof(start_amp_visible)); } }

            private bool _end_freq_visible = true;
            public bool end_freq_visible { get { return _end_freq_visible; } set { _end_freq_visible = value; RaisePropertyChanged(nameof(end_freq_visible)); } }

            private bool _end_amp_visible = true;
            public bool end_amp_visible { get { return _end_amp_visible; } set { _end_amp_visible = value; RaisePropertyChanged(nameof(end_amp_visible)); } }

            private bool _cut_off_amp_visible = true;
            public bool cut_off_amp_visible { get { return _cut_off_amp_visible; } set { _cut_off_amp_visible = value; RaisePropertyChanged(nameof(cut_off_amp_visible)); } }

            private bool _max_amp_visible = true;
            public bool max_amp_visible { get { return _max_amp_visible; } set { _max_amp_visible = value; RaisePropertyChanged(nameof(max_amp_visible)); } }

            private bool _polynomial_visible = true;
            public bool polynomial_visible { get { return _polynomial_visible; } set { _polynomial_visible = value; RaisePropertyChanged(nameof(polynomial_visible)); } }

            private bool _curve_rate_visible = true;
            public bool curve_rate_visible { get { return _curve_rate_visible; } set { _curve_rate_visible = value; RaisePropertyChanged(nameof(curve_rate_visible)); } }

            private bool _disable_range_visible = true;
            public bool disable_range_visible { get { return _disable_range_visible; } set { _disable_range_visible = value; RaisePropertyChanged(nameof(disable_range_visible)); } }
        };
        public class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void RaisePropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Control_Amplitude(Yaml_Control_Data_Amplitude ycd, Control_Amplitude_Content cac, MainWindow mainWindow)
        {
            target = ycd;
            this.mainWindow = mainWindow;
            content = cac;

            InitializeComponent();

            if (cac == Control_Amplitude_Content.Default)
                title.Content = "Default Amplitude Setting";
                
            else if (cac == Control_Amplitude_Content.Free_Run_On)
                title.Content = "Mascon On Free Run Amplitude Setting";

            else
                title.Content = "Mascon Off Free Run Amplitude Setting";

            visible_Class = new Visible_Class();
            DataContext = visible_Class;
            apply_view();

            no_update = false;
        }

        private void apply_view()
        {
            Amplitude_Mode[] modes = (Amplitude_Mode[])Enum.GetValues(typeof(Amplitude_Mode));
            amplitude_mode_selector.ItemsSource = modes;
            amplitude_mode_selector.SelectedItem = target.mode;

            start_freq_box.Text = target.parameter.start_freq.ToString();
            start_amp_box.Text = target.parameter.start_amp.ToString();
            end_freq_box.Text = target.parameter.end_freq.ToString();
            end_amp_box.Text = target.parameter.end_amp.ToString();
            cutoff_amp_box.Text = target.parameter.cut_off_amp.ToString();
            max_amp_box.Text = target.parameter.max_amp.ToString();
            polynomial_box.Text = target.parameter.polynomial.ToString();
            curve_rate_box.Text = target.parameter.curve_change_rate.ToString();
            disable_range_limit_check.IsChecked = target.parameter.disable_range_limit;

            grid_hider(target.mode, content);
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

        private void textbox_change(object sender, TextChangedEventArgs e)
        {
            if (no_update) return;

            TextBox tb = (TextBox)sender;
            Object? tag = tb.Tag;
            if (tag == null) return;

            if (tag.Equals("start_freq"))
                target.parameter.start_freq = parse_d(tb);
            else if (tag.Equals("start_amp"))
                target.parameter.start_amp = parse_d(tb);
            else if (tag.Equals("end_freq"))
                target.parameter.end_freq = parse_d(tb);
            else if (tag.Equals("end_amp"))
                target.parameter.end_amp = parse_d(tb);
            else if (tag.Equals("cutoff_amp"))
                target.parameter.cut_off_amp = parse_d(tb);
            else if (tag.Equals("max_amp"))
                target.parameter.max_amp = parse_d(tb);
            else if(tag.Equals("curve_rate"))
                target.parameter.curve_change_rate = parse_d(tb);
            else if (tag.Equals("polynomial"))
                target.parameter.polynomial = parse_i(tb);

            mainWindow.update_Control_List_View();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (no_update) return;

            CheckBox cb = (CheckBox)sender;
            target.parameter.disable_range_limit = (cb.IsChecked == false) ? false : true;
            mainWindow.update_Control_List_View();
        }

        private void amplitude_mode_selector_Selected(object sender, RoutedEventArgs e)
        {
            if (no_update) return;

            Amplitude_Mode selected = (Amplitude_Mode)amplitude_mode_selector.SelectedItem;
            target.mode = selected;
            grid_hider(target.mode, content);

            mainWindow.update_Control_List_View();

            
        }

        private Grid get_Grid(int i)
        {
            switch (i) {
                case 0: return start_freq_grid;
                case 1: return start_amp_grid;
                case 2: return end_freq_grid;
                case 3: return end_amp_grid;
                case 4: return cut_off_amp_grid;
                case 5: return max_amp_grid;
                case 6: return polynomial_grid;
                case 7: return curve_change_grid;
                default: return disable_range_grid;
            
            }
        }

        private void set_Visible_Bool(int i, bool b)
        {
            if (i == 0) visible_Class.start_freq_visible = b;
            else if (i == 1) visible_Class.start_amp_visible = b;
            else if (i == 2) visible_Class.end_freq_visible = b;
            else if (i == 3) visible_Class.end_amp_visible = b;
            else if (i == 4) visible_Class.cut_off_amp_visible = b;
            else if (i == 5) visible_Class.max_amp_visible = b;
            else if (i == 6) visible_Class.polynomial_visible = b;
            else if (i == 7) visible_Class.curve_rate_visible = b;
            else visible_Class.disable_range_visible = b;
        }

        private void grid_hider(Amplitude_Mode mode , Control_Amplitude_Content cac)
        {
            Boolean[] condition_1, condition_2;

            if (mode == Amplitude_Mode.Linear)
                condition_1 = new Boolean[9] { true, true, true, true, true, true, false, false, true };
            else if(mode == Amplitude_Mode.Wide_3_Pulse)
                condition_1 = new Boolean[9] { true, true, true, true, true, true, false, false, true };
            else if(mode == Amplitude_Mode.Inv_Proportional)
                condition_1 = new Boolean[9] { true, true, true, true, true, true, false, true, true };
            else if(mode == Amplitude_Mode.Exponential)
                condition_1 = new Boolean[9] { false, false, true, true, true, true, false, false, true };
            else
                condition_1 = new Boolean[9] { false, false, true, true, true, true, true, false, true };

            if (cac == Control_Amplitude_Content.Default)
                condition_2 = new Boolean[9] { true, true, true, true, true, true, true, true, true };
            else if(cac == Control_Amplitude_Content.Free_Run_On)
                condition_2 = new Boolean[9] { true, true, true, true, true, true, true, true, true };
            else
                condition_2 = new Boolean[9] { true, true, true, true, true, true, true, true, true };

            for(int i =0; i < 9; i++)
            {
                set_Visible_Bool(i, (condition_1[i] && condition_2[i]));

            }
        }
    }



    public enum Control_Amplitude_Content
    {
        Default,Free_Run_On,Free_Run_Off
    }
}
