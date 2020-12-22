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
using System.Windows.Shapes;
using System.Net;
using System.IO;

namespace Bukva
{
    /// <summary>
    /// Interaction logic for PackageManager.xaml
    /// </summary>
    public partial class PackageManager : Window
    {
        private List<string> Online;
        private List<string> Local;

        public PackageManager()
        {
            InitializeComponent();

            RefreshOnline();
            RefreshLocal();
        }

        void RefreshOnline()
        {
            Online = GetManifest();
            OnlineFiles.ItemsSource = Online;
        }

        void RefreshLocal()
        {
            Local = LetterTable.GetCandidateFiles();
            LocalFiles.ItemsSource = Local;
        }

        List<string> GetManifest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/vzyrianov/Bukva/master/Languages/manifest.txt");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string result = streamReader.ReadToEnd();

                var resultingList = new List<string>(result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
                resultingList.RemoveAt(resultingList.Count - 1);
                return resultingList;
            }
        }

        void DownloadFile(string filename)
        {
            string fileContent;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://raw.githubusercontent.com/vzyrianov/Bukva/master/Languages/{filename}.buk");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(stream))
            {
                fileContent = streamReader.ReadToEnd();
            }

            System.IO.File.WriteAllText($@".\{filename}.buk", fileContent);
        }

        void DownloadButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (string)OnlineFiles.SelectedItem;

            if (selectedItem != null)
            {
                DownloadFile(selectedItem);
            }

            RefreshLocal();
        }

        string GetCurrentPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        }

        void OpenDirectorButton(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", GetCurrentPath());
        }

        void DeleteButton(object sender, RoutedEventArgs e)
        {
            string filename = (string)LocalFiles.SelectedItem;


            if (filename != null)
            {
                System.IO.File.Delete($@"{filename}");
            }
            RefreshLocal();
        }
    }
}
