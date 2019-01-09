using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Bukva
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KeyTranslator keyTranslator;

        public MainWindow()
        {
            InitializeComponent();

            keyTranslator = new KeyTranslator();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {/*
            button2.BackColor = Color.DeepSkyBlue;
            button2.Font = new Font(button2.Font, FontStyle.Regular);
            button2.ForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            button1.BackColor = Color.SteelBlue;
            button1.Font = new Font(button1.Font, FontStyle.Bold);
            button1.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            */
            keyTranslator.Translate = true;
            //this.Text = "Bukva: ON";
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
            keyTranslator.Translate = false;
            //this.Text = "Bukva: OFF";
        }
    }
}
