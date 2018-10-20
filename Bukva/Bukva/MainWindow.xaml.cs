using System;
using System.IO;
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

using Newtonsoft.Json;

namespace Bukva
{
    public partial class MainWindow : Window
    {
        Configuration currentConfig;

        public MainWindow()
        {
            InitializeComponent();

            StreamReader streamReader = new StreamReader(@"config.xml");
            string json = streamReader.ReadToEnd();

            Configuration config = JsonConvert.DeserializeObject<Configuration>(json);
            int i = 12;
        }

        private void Enable()
        {
            this.Title = "Bukva: On";
        }

        private void Disable()
        {
            this.Title = "Bukva: Off";
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Enable();
        }


        private void OffButtonClick(object sender, RoutedEventArgs e)
        {
            Disable();
        }
    }
}
