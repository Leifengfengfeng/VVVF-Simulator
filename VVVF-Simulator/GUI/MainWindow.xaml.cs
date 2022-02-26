using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using VVVF_Simulator.Yaml_VVVF_Sound;
using VVVF_Simulator.Pages.Control_Settings;
using VVVF_Simulator.Pages.Control_Settings.Level_3;
using YamlDotNet.Serialization;
using static VVVF_Simulator.vvvf_wave_calculate;
using static VVVF_Simulator.Yaml_VVVF_Sound.Yaml_Sound_Data;
using VVVF_Simulator.GUI.UtilForm;
using System.ComponentModel;
using System.Media;
using System.Threading.Tasks;
using VVVF_Simulator.GUI.Simulator_Window;
using VVVF_Simulator.GUI.Simulator_Window.RealTime_Generation;

namespace VVVF_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewData view_data = new ViewData();
        public class ViewData : ViewModelBase
        {
            private bool _blocking = false;
            public bool blocking
            {
                get
                {
                    return _blocking;
                }
                set
                {
                    _blocking = value;
                    RaisePropertyChanged(nameof(blocking));
                }
            }
        };
        public class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void RaisePropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainWindow()
        {
            DataContext = view_data;
            InitializeComponent();
        }

        

        private void setting_button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string name = button.Name;
            if (name.Equals("settings_level"))
                setting_window.Navigate(new Uri("GUI/Pages/Settings/level_setting.xaml", UriKind.Relative));
            else if(name.Equals("settings_minimum"))
                setting_window.Navigate(new Uri("GUI/Pages/Settings/minimum_freq_setting.xaml", UriKind.Relative));
            else if(name.Equals("settings_mascon"))
                setting_window.Navigate(new Uri("GUI/Pages/Settings/mascon_off_setting.xaml", UriKind.Relative));
        }

        private void settings_edit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            object? tag = btn.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;
            String[] command = tag_str.Split("_");

            var list_view = command[0].Equals("accelerate") ? accelerate_settings : accelerate_settings;
            var settings = command[0].Equals("accelerate") ? Yaml_Generation.current_data.accelerate_pattern : Yaml_Generation.current_data.braking_pattern;

            if (command[1].Equals("remove"))
                settings.RemoveAt(list_view.SelectedIndex);
            else if (command[1].Equals("add"))
                settings.Add(new Yaml_Control_Data());
            else if (command[1].Equals("reset"))
                settings.Clear();

            list_view.Items.Refresh();
        }
        private void settings_load(object sender, RoutedEventArgs e)
        {
            ListView btn = (ListView)sender;
            object? tag = btn.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;

            if (tag.Equals("accelerate"))
            {
                accelerate_settings.ItemsSource = Yaml_Generation.current_data.accelerate_pattern;
                accelerate_selected_show();
            }
            else
            {
                brake_settings.ItemsSource = Yaml_Generation.current_data.braking_pattern;
                brake_selected_show();
            }
        }
        private void settings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView btn = (ListView)sender;
            object? tag = btn.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;


            if(tag.Equals("accelerate"))
                accelerate_selected_show();
            else
                brake_selected_show();
        }
       
        private void accelerate_selected_show()
        {
            int selected = accelerate_settings.SelectedIndex;
            if (selected < 0) return;

            Yaml_Sound_Data ysd = Yaml_Generation.current_data;
            var selected_data = ysd.accelerate_pattern[selected];
            
            if(ysd.level == 2)
            {
                if (selected_data.pulse_Mode == Pulse_Mode.Async || selected_data.pulse_Mode == Pulse_Mode.Async_THI)
                    setting_window.Navigate(new Level_2_Page_Control_Common_Async(selected_data, this));
                else
                    setting_window.Navigate(new Level_2_Page_Control_Common_Sync(selected_data, this));
            }
            else
            {
                if (selected_data.pulse_Mode == Pulse_Mode.Async || selected_data.pulse_Mode == Pulse_Mode.Async_THI)
                    setting_window.Navigate(new Level_3_Page_Control_Common_Async(selected_data, this));
                else
                    setting_window.Navigate(new Level_3_Page_Control_Common_Sync(selected_data, this));
            }

        }
        private void brake_selected_show()
        {
            int selected = brake_settings.SelectedIndex;
            if (selected < 0) return;

            Yaml_Sound_Data ysd = Yaml_Generation.current_data;
            var selected_data = ysd.braking_pattern[selected];
           
            if(ysd.level == 2)
            {
                if (selected_data.pulse_Mode == Pulse_Mode.Async || selected_data.pulse_Mode == Pulse_Mode.Async_THI)
                    setting_window.Navigate(new Level_2_Page_Control_Common_Async(selected_data, this));
                else
                    setting_window.Navigate(new Level_2_Page_Control_Common_Sync(selected_data, this));
            }
            else
            {
                if (selected_data.pulse_Mode == Pulse_Mode.Async || selected_data.pulse_Mode == Pulse_Mode.Async_THI)
                    setting_window.Navigate(new Level_3_Page_Control_Common_Async(selected_data, this));
                else
                    setting_window.Navigate(new Level_3_Page_Control_Common_Sync(selected_data, this));
            }
        }

        public void update_Control_List_View()
        {
            accelerate_settings.Items.Refresh();
            brake_settings.Items.Refresh();
        }
        public void update_Control_Showing()
        {
            if (setting_tabs.SelectedIndex == 2)
            {
                accelerate_selected_show();
            }
            else if (setting_tabs.SelectedIndex == 3)
            {
                brake_selected_show();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            Object? tag = mi.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;
            String[] command = tag_str.Split(".");
            if (command[0].Equals("brake"))
            {
                if (command[1].Equals("sort"))
                {
                    Yaml_Generation.current_data.braking_pattern.Sort((a, b) => Math.Sign(b.from - a.from));
                    update_Control_List_View();
                    brake_selected_show();
                }
            }else if (command[0].Equals("accelerate"))
            {
                if (command[1].Equals("sort"))
                {
                    Yaml_Generation.current_data.accelerate_pattern.Sort((a, b) => Math.Sign(b.from - a.from));
                    update_Control_List_View();
                    accelerate_selected_show();
                }
            }
        }
        private void File_Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem button = (MenuItem)sender;
            Object? tag = button.Tag;
            if (tag == null) return;
            if (tag.Equals("Load"))
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Yaml (*.yaml)|*.yaml|All (*.*)|*.*"
                };
                if (dialog.ShowDialog() == false) return;

                if (Yaml_Generation.load_Yaml(dialog.FileName))
                    MessageBox.Show("Load OK.", "Great", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Invalid yaml or path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else if (tag.Equals("Save"))
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Yaml (*.yaml)|*.yaml"
                };

                // ダイアログを表示する
                if (dialog.ShowDialog() == false) return;

                if (Yaml_Generation.save_Yaml(dialog.FileName))
                    MessageBox.Show("Save OK.", "Great", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Error occurred on saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static class Generation_Params
        {
            public static List<double> Double_Values = new();
            public static List<Generation.Generate_Control_Info.Taroimo_Status_Language_Mode> Video_Language = new();
        }
        private void Generation_Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem button = (MenuItem)sender;
            Object? tag = button.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;
            String[] command = tag_str.Split("_");


            view_data.blocking = true;

            Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
            
            bool unblock = solve_Command(command, clone);

            if (!unblock) return;
            view_data.blocking = false;
            SystemSounds.Beep.Play();

        }
        private Boolean solve_Command(String[] command, Yaml_Sound_Data data)
        {

            if (command[0].Equals("Audio"))
            {
                var dialog = new SaveFileDialog { Filter = "wav (*.wav)|*.wav" };
                if (dialog.ShowDialog() == false) return true;
                Task task = Task.Run(() => {
                    Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                    if (command[1].Equals("VVVF"))
                        Generation.Generate_Sound.generate_sound(dialog.FileName, clone);
                    else if (command[1].Equals("Environment"))
                        Generation.Generate_Sound.generate_env_sound(dialog.FileName);
                    SystemSounds.Beep.Play();
                });
                
            }
            else if (command[0].Equals("Control"))
            {
                var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                if (dialog.ShowDialog() == false) return true;
                if (command[1].Equals("Original"))
                {
                    Task task = Task.Run(() => {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Control_Info.generate_status_video(dialog.FileName, clone);
                        SystemSounds.Beep.Play();
                    });
                }
                    
                else if (command[1].Equals("Taroimo"))
                {
                    Generate_Language_Select lang_select = new();
                    lang_select.ShowDialog();

                    Task task = Task.Run(() => {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Control_Info.generate_status_taroimo_like_video(
                            dialog.FileName,
                            clone,
                            Generation.Generate_Control_Info.Taroimo_Status_Language_Mode.Japanese,
                            Generation_Params.Video_Language[1]
                        );
                        SystemSounds.Beep.Play();
                    });
                }
                    
            }
            else if (command[0].Equals("WaveForm"))
            {
                var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                if (dialog.ShowDialog() == false) return true;
                Task task = Task.Run(() => {
                    if (command[1].Equals("Original"))
                    {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Wave_Form.generate_wave_U_V(dialog.FileName, clone);
                        SystemSounds.Beep.Play();
                    }
                    else if (command[1].Equals("Taroimo"))
                    {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Wave_Form.generate_taroimo_like_wave_U_V(dialog.FileName, clone);
                        SystemSounds.Beep.Play();
                    }
                    else if (command[1].Equals("UVW"))
                    {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Wave_Form.generate_taroimo_like_wave_U_V(dialog.FileName, clone);
                        SystemSounds.Beep.Play();
                    }
                       
                });
            }
            else if (command[0].Equals("Hexagon"))
            {
                MessageBoxResult result = MessageBox.Show("Enable zero vector circle?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question);
                bool circle = result == MessageBoxResult.Yes;

                if (command[1].Equals("Original"))
                {
                    var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                    if (dialog.ShowDialog() == false) return true;

                    Task task = Task.Run(() => {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Hexagon.generate_wave_hexagon(dialog.FileName, clone, circle);
                        SystemSounds.Beep.Play();
                    });
                }
                else if (command[1].Equals("Taroimo"))
                {
                    var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                    if (dialog.ShowDialog() == false) return true;

                    Task task = Task.Run(() => {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Hexagon.generate_wave_hexagon_taroimo_like(dialog.FileName, clone, circle);
                        SystemSounds.Beep.Play();
                    });
                }
                else if (command[1].Equals("Explain"))
                {
                    var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                    if (dialog.ShowDialog() == false) return true;

                    Double_Ask_Form double_Ask_Dialog = new Double_Ask_Form("Enter the frequency.");
                    double_Ask_Dialog.ShowDialog();

                    Task task = Task.Run(() => {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Hexagon.generate_wave_hexagon_explain(dialog.FileName, clone, circle, Generation_Params.Double_Values[0]);
                        SystemSounds.Beep.Play();
                    });
                }
                else if (command[1].Equals("Image"))
                {
                    var dialog = new SaveFileDialog { Filter = "png (*.png)|*.png" };
                    if (dialog.ShowDialog() == false) return true;

                    Double_Ask_Form double_Ask_Dialog = new Double_Ask_Form("Enter the frequency.");
                    double_Ask_Dialog.ShowDialog();

                    Task task = Task.Run(() => {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_Hexagon.generate_wave_hexagon_picture(dialog.FileName, clone, circle, Generation_Params.Double_Values[0]);
                        SystemSounds.Beep.Play();
                    });
                }
            }else if (command[0].Equals("RealTime"))
            {
                if (command[1].Equals("RealTime"))
                {
                    Generation.Generate_RealTime.RealTime_Parameter.quit = false;
                    Generation.Generate_RealTime.RealTime_Parameter.buff_size = Properties.Settings.Default.G_RealTime_Buff;

                    RealTime_Mascon_Window mascon = new();
                    mascon.Show();

                    if (Properties.Settings.Default.G_RealTime_WaveForm)
                    {
                        RealTime_WaveForm_Window wave_form = new();
                        wave_form.Show();
                    }
                    



                    view_data.blocking = true;
                    Task task = Task.Run(() => {
                        Yaml_Sound_Data clone = Yaml_Generation.DeepClone(Yaml_Generation.current_data);
                        Generation.Generate_RealTime.realtime_sound(clone);
                        view_data.blocking = false;
                        SystemSounds.Beep.Play();
                    });
                    return false;

                }else if (command[1].Equals("Setting"))
                {
                    RealTime_Settings setting = new();
                    setting.ShowDialog();
                }

            }

            return true;
        }
    }
}
