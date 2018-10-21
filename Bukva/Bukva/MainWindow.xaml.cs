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
        KeyTranslator keyTranslator;
        CustomLanguageConfiguration languageConfiguration;

        public MainWindow()
        {
            InitializeComponent();

            string filePath = System.IO.Path.GetFullPath("config.json");
            StreamReader streamReader = new StreamReader(filePath);
            string json = streamReader.ReadToEnd();

            Configuration config = JsonConvert.DeserializeObject<Configuration>(json);
            
            languageConfiguration = new CustomLanguageConfiguration();
            languageConfiguration.LoadConfiguration(config.keyTranslations);

            keyTranslator = new KeyTranslator(languageConfiguration);
        }

        private void Enable()
        {
            this.Title = "Bukva: On";
            keyTranslator.Enabled = true;
        }

        private void Disable()
        {
            this.Title = "Bukva: Off";
            keyTranslator.Enabled = false;
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
