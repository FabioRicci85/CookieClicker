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

        double teller = 0;
        

        public MainWindow()
        {
            InitializeComponent();
            UpdateTitle();
        }

        private void ImgCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {

            ImgCookie.Width = ImgCookie.ActualWidth - 10;

        }

        private void ImgCookie_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImgCookie.Width = ImgCookie.ActualWidth + 10;

            teller++;

            UpdateScore();
            UpdateTitle();
           

        }

        private int UpdateScore()
        {
            LblScore.Content = Math.Round(teller).ToString();
            int score = Convert.ToInt32(LblScore.Content);

            return score;
        }

        private void UpdateTitle()
        {
            string titleScore = Convert.ToString(UpdateScore());
            Title = $"{titleScore} cookies";
        }
              
    }
}
