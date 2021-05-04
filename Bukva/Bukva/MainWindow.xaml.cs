using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Bukva
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IKeyTranslator keyTranslator;
        bool Enabled;
        LowLevelKeyboardHook scrollLockListener;


        List<LetterTable> translationTables;
        int index;

        PackageManager packageManager;

        public MainWindow()
        {


            translationTables = new List<LetterTable>();
            index = 0;

            InitializeComponent();

            Enabled = false;
            keyTranslator = new LowLevelKeyTranslator(new LetterTable());

            LoadFiles();
            UpdateLetterTableToCurrentIndex();

            scrollLockListener = new LowLevelKeyboardHook();
            scrollLockListener.Listen = true;
            scrollLockListener.OnKeyPressed += OnKeyPressed;

        }

        private void LoadFiles()
        {
            translationTables = new List<LetterTable>();

            List<string> foundFiles = LetterTable.GetCandidateFiles();

            foreach (string filename in foundFiles)
            {
                translationTables.Add(new LetterTable(filename));
            }


            index = index % translationTables.Count;
        }

        private void UpdateLetterTableToCurrentIndex()
        {
            LanguageLabel.Content = translationTables[index].Filename;
            keyTranslator.SetLetterTable(translationTables[index]);
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



        private void MoveRightClick(object sender, RoutedEventArgs e)
        {
            index = (index + 1) % translationTables.Count;

            UpdateLetterTableToCurrentIndex();
        }

        private void MoveLeftClick(object sender, RoutedEventArgs e)
        {
            index = index - 1;
            
            if (index == -1)
            {
                index = translationTables.Count - 1;
            }

            UpdateLetterTableToCurrentIndex();
        }

        private void PackageManagerClosed(object sender, System.EventArgs e)
        {
            LoadFiles();
            UpdateLetterTableToCurrentIndex();
        }

        private void ManageLanguagesClicked(object sender, RoutedEventArgs e)
        {
            PackageManager pm = new PackageManager();
            pm.Show();
            pm.Closed += PackageManagerClosed;
        }
    }
}
