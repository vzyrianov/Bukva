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
        LetterTable letterTable;

        public MainWindow()
        {
            InitializeComponent();

            letterTable = new LetterTable();
            keyTranslator = new KeyTranslator(letterTable);
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
            keyTranslator.Start();
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
            keyTranslator.Stop();
            //this.Text = "Bukva: OFF";
        }
    }
}
