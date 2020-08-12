using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;

namespace Bukva
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IKeyTranslator keyTranslator;
        LetterTable letterTable;
        bool Enabled;

        public MainWindow()
        {
            InitializeComponent();

            Enabled = false;

            letterTable = new LetterTable();
            keyTranslator = new LowLevelKeyTranslator(letterTable);
            StatusLabel.Content = "Language: \n" + letterTable.Filename;
            
        }

        private void ToggleButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Enabled)
            {
                keyTranslator.Start();
                ToggleButton.Background = Brushes.DeepSkyBlue;
                ToggleButton.Content = "On";
                Enabled = true;
            }
            else
            {
                keyTranslator.Stop();
                ToggleButton.Background = Brushes.SteelBlue;
                ToggleButton.Content = "Off";
                Enabled = false;
            }
        }
    }
}
