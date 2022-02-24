using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using VVVF_Yaml_Generator.Pages.Control_Settings;
using VVVF_Yaml_Generator.Pages.Control_Settings.Level_3;
using YamlDotNet.Serialization;
using static VVVF_Data_Generator.Yaml_Sound_Data;

namespace VVVF_Data_Generator
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
                setting_window.Navigate(new Uri("Pages/Settings/level_setting.xaml", UriKind.Relative));
            else if(name.Equals("settings_minimum"))
                setting_window.Navigate(new Uri("Pages/Settings/minimum_freq_setting.xaml", UriKind.Relative));
            else if(name.Equals("settings_mascon"))
                setting_window.Navigate(new Uri("Pages/Settings/mascon_off_setting.xaml", UriKind.Relative));
        }

        private void file_button_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
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
                if (selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async || selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async_THI)
                    setting_window.Navigate(new Level_2_Page_Control_Common_Async(selected_data, this));
                else
                    setting_window.Navigate(new Level_2_Page_Control_Common_Sync(selected_data, this));
            }
            else
            {
                if (selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async || selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async_THI)
                    setting_window.Navigate(new Level_3_Page_Control_Common_Async(selected_data, this));
                else
                    setting_window.Navigate(new Level_3_Page_Control_Common_Sync(selected_data, this));
            }

        }

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
                if (selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async || selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async_THI)
                    setting_window.Navigate(new Level_2_Page_Control_Common_Async(selected_data, this));
                else
                    setting_window.Navigate(new Level_2_Page_Control_Common_Sync(selected_data, this));
            }
            else
            {
                if (selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async || selected_data.pulse_Mode == Yaml_Control_Data.Pulse_Mode.Async_THI)
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
    }
}
