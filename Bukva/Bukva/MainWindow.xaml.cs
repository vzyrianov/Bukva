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
        KeyTranslator keyTranslator;
        LetterTable letterTable;
        bool Enabled;

        public MainWindow()
        {
            InitializeComponent();

            Enabled = false;

            letterTable = new LetterTable();
            keyTranslator = new KeyTranslator(letterTable);
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

        private void OffButtonClick(object sender, RoutedEventArgs e)
        {
            /*
            button1.BackColor = Color.DeepSkyBlue;
            button1.Font = new Font(button1.Font, FontStyle.Regular);
            button1.ForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            button2.BackColor = Color.SteelBlue;
            button2.Font = new Font(button2.Font, FontStyle.Bold);
            button2.ForeColor = Color.FromKnownColor(KnownColor.ControlText);*/
            keyTranslator.Stop();
            //this.Text = "Bukva: OFF";
        }
    }
}
