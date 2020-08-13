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
        LowLevelKeyboardHook scrollLockListener;

        public MainWindow()
        {
            InitializeComponent();

            Enabled = false;

            letterTable = new LetterTable();
            keyTranslator = new LowLevelKeyTranslator(letterTable);
            StatusLabel.Content = "Language: \n" + letterTable.Filename;

            scrollLockListener = new LowLevelKeyboardHook();
            scrollLockListener.Listen = true;
            scrollLockListener.OnKeyPressed += OnKeyPressed;

        }

        private void ToggleButtonClick(object sender, RoutedEventArgs e)
        {
            Toggle();
        }

        private void Toggle()
        {
            if (!Enabled)
            {
                Enable();
            }
            else
            {
                Disable();
            }

        }

        private void Enable()
        {
            keyTranslator.Start();
            ToggleButton.Background = Brushes.DeepSkyBlue;
            ToggleButton.Content = "On";
            Enabled = true;
        }

        private void Disable()
        {
            keyTranslator.Stop();
            ToggleButton.Background = Brushes.SteelBlue;
            ToggleButton.Content = "Off";
            Enabled = false;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.ScrollLockPressed)
                Toggle();
        }
    }
}
