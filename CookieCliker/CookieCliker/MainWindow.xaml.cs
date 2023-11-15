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

namespace CookieCliker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int teller = 0;
        float totaleTeller = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImgCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {

            ImgCookie.Width = ImgCookie.ActualWidth - 10;

        }

        private void ImgCookie_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImgCookie.Width = ImgCookie.ActualWidth + 10;

            teller++;
            totaleTeller++;
            UpdateScore();
            TotaleScore();

        }

        private void UpdateScore()
        {
            LblScore.Content = teller.ToString();
        }

        private void TotaleScore()
        {
            LblTotaleScore.Content = totaleTeller.ToString();
        }
              
    }
}
