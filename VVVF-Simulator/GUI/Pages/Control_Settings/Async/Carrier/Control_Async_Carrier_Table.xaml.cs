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
using static VVVF_Data_Generator.Yaml_Sound_Data.Yaml_Control_Data.Yaml_Async_Parameter.Yaml_Async_Parameter_Carrier_Freq.Yaml_Async_Parameter_Carrier_Freq_Table;

namespace VVVF_Yaml_Generator.Pages.Control_Settings.Async
{
    /// <summary>
    /// Control_Async_Table.xaml の相互作用ロジック
    /// </summary>
    public partial class Control_Async_Carrier_Table : UserControl
    {
        View_Data vd = new View_Data();
        public class View_Data {
            public List<Yaml_Async_Parameter_Carrier_Freq_Table_Single> Async_Table_Data { get; set; } = new List<Yaml_Async_Parameter_Carrier_Freq_Table_Single>();
        }

        Yaml_Control_Data target;
        public Control_Async_Carrier_Table(Yaml_Control_Data data, MainWindow mainWindow)
        {
            InitializeComponent();

            vd.Async_Table_Data = data.async_data.carrier_wave_data.carrier_table_value.carrier_freq_table;
            DataContext = vd;
            target = data;

        }

        private void DataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            target.async_data.carrier_wave_data.carrier_table_value.carrier_freq_table = vd.Async_Table_Data;
        }
    }
}
