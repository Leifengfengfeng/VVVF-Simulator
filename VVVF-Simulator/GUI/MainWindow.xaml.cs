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

namespace VVVF_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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

        // ACCELERATING SETTING EVENTS
        private void accelerate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            object? tag = btn.Tag;
            if (tag == null) return;
            if (tag.Equals("remove"))
            {
                Yaml_Generation.current_data.accelerate_pattern.RemoveAt(accelerate_settings.SelectedIndex);
            }
            else if (tag.Equals("add"))
            {
                Yaml_Generation.current_data.accelerate_pattern.Add(new Yaml_Control_Data());
            }
            else if (tag.Equals("reset"))
            {
                Yaml_Generation.current_data.accelerate_pattern.Clear();
            }

            accelerate_settings.Items.Refresh();
        }
        private void accelerate_setting_load(object sender, RoutedEventArgs e)
        {
            accelerate_settings.ItemsSource = Yaml_Generation.current_data.accelerate_pattern;
            accelerate_selected_show();
        }
        private void accelerate_settings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            accelerate_selected_show();
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


        // BRAKING SETTING EVENTS
        private void brake_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            object? tag = btn.Tag;
            if (tag == null) return;
            if (tag.Equals("remove"))
            {
                Yaml_Generation.current_data.braking_pattern.RemoveAt(brake_settings.SelectedIndex);
            }else if (tag.Equals("add"))
            {
                Yaml_Generation.current_data.braking_pattern.Add(new Yaml_Control_Data());
            }else if (tag.Equals("reset"))
            {
                Yaml_Generation.current_data.braking_pattern.Clear();
            }

            brake_settings.Items.Refresh();
        }
        private void brake_settings_load(object sender, RoutedEventArgs e)
        {
            brake_settings.ItemsSource = Yaml_Generation.current_data.braking_pattern;
            brake_selected_show();
        }
        private void brake_settings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brake_selected_show();
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

        public class Generation_Param_Vari
        {
            public List<double> Double_Values = new();
        }
        public Generation_Param_Vari gen_param = new();
        private void Generation_Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem button = (MenuItem)sender;
            Object? tag = button.Tag;
            if (tag == null) return;
            String? tag_str = tag.ToString();
            if (tag_str == null) return;
            String[] command = tag_str.Split("_");

            if (command[0].Equals("Audio"))
            {
                var dialog = new SaveFileDialog { Filter = "wav (*.wav)|*.wav" };
                if (dialog.ShowDialog() == false) return;
                if (command[1].Equals("VVVF"))
                    Generation.Generate_Sound.generate_sound(dialog.FileName, Yaml_Generation.current_data);
                else if (command[1].Equals("Environment"))
                    Generation.Generate_Sound.generate_env_sound(dialog.FileName);
            }
            else if (command[0].Equals("Control"))
            {
                var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                if (dialog.ShowDialog() == false) return;
                if (command[1].Equals("Original"))
                    Generation.Generate_Control_Info.generate_status_video(dialog.FileName, Yaml_Generation.current_data);
                else if (command[1].Equals("Taroimo"))
                    Generation.Generate_Control_Info.generate_status_taroimo_like_video(
                        dialog.FileName, 
                        Yaml_Generation.current_data, 
                        Generation.Generate_Control_Info.Language_Mode.Japanese, 
                        Generation.Generate_Control_Info.Language_Mode.Japanese
                    );
            }
            else if (command[0].Equals("WaveForm"))
            {
                var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                if (dialog.ShowDialog() == false) return;
                if (command[1].Equals("Original"))
                    Generation.Generate_Wave_Form.generate_wave_U_V(dialog.FileName, Yaml_Generation.current_data);
                else if (command[1].Equals("Taroimo"))
                    Generation.Generate_Wave_Form.generate_taroimo_like_wave_U_V(dialog.FileName, Yaml_Generation.current_data);
                else if (command[1].Equals("UVW"))
                    Generation.Generate_Wave_Form.generate_taroimo_like_wave_U_V(dialog.FileName, Yaml_Generation.current_data);
            }
            else if (command[0].Equals("Hexagon"))
            {
                if (command[1].Equals("Original"))
                {
                    var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                    if (dialog.ShowDialog() == false) return;

                    MessageBoxResult result = MessageBox.Show("Enable zero vector circle?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    bool circle = result == MessageBoxResult.Yes;

                    Generation.Generate_Hexagon.generate_wave_hexagon(dialog.FileName, Yaml_Generation.current_data, circle);
                }
                else if (command[1].Equals("Taroimo"))
                {
                    var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                    if (dialog.ShowDialog() == false) return;

                    MessageBoxResult result = MessageBox.Show("Enable zero vector circle?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    bool circle = result == MessageBoxResult.Yes;

                    Generation.Generate_Hexagon.generate_wave_hexagon_taroimo_like(dialog.FileName, Yaml_Generation.current_data, circle);
                }
                else if (command[1].Equals("Explain"))
                {
                    var dialog = new SaveFileDialog { Filter = "mp4 (*.mp4)|*.mp4" };
                    if (dialog.ShowDialog() == false) return;

                    MessageBoxResult result = MessageBox.Show("Enable zero vector circle?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    bool circle = result == MessageBoxResult.Yes;

                    Double_Ask_Form double_Ask_Dialog = new Double_Ask_Form(this,"Enter the frequency.");
                    double_Ask_Dialog.ShowDialog();

                    bool t = Generation.Generate_Hexagon.generate_wave_hexagon_explain(dialog.FileName, Yaml_Generation.current_data, circle , gen_param.Double_Values[0]);
                    Debug.Print(t.ToString());
                }
                else if (command[1].Equals("Image"))
                {
                    var dialog = new SaveFileDialog { Filter = "png (*.png)|*.png" };
                    if (dialog.ShowDialog() == false) return;

                    MessageBoxResult result = MessageBox.Show("Enable zero vector circle?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    bool circle = result == MessageBoxResult.Yes;

                    Double_Ask_Form double_Ask_Dialog = new Double_Ask_Form(this, "Enter the frequency.");
                    double_Ask_Dialog.ShowDialog();

                    Generation.Generate_Hexagon.generate_wave_hexagon_picture(dialog.FileName, Yaml_Generation.current_data, circle, gen_param.Double_Values[0]);
                }
            }
            else if (command[0].Equals("RealTime"))
            {
                if (command[1].Equals("RealTime"))
                    Generation.Generate_RealTime.realtime_sound(Yaml_Generation.current_data);
            }
        }
    }
}
