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
        double clicker = 0;

        public MainWindow()
        {
            InitializeComponent();
            BtnStore5.Visibility = Visibility.Collapsed;
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
            VisibleButton();
           
            
        }

        private int UpdateScore()
        {
            LblScore.Content = Math.Round(teller).ToString();
            int score = Convert.ToInt32(LblScore.Content);
            if (score >= Convert.ToDouble(LblPrijs1.Content))
            {
                BtnStore1.IsEnabled = true;
            }
            else
                BtnStore1.IsEnabled = false;

            return score;
        }

        private void UpdateTitle()
        {
            string titleScore = Convert.ToString(UpdateScore());
            Title = $"{titleScore} cookies  -  Cookie Clicker";
        }

        private void VisibleButton()
        {
            if (UpdateScore() == 60000)
            {
                BtnStore5.Visibility = Visibility.Visible;
            }

        }

        //private void ButtonEnable()
        //{
        //    if (UpdateScore() >= Convert.ToDouble(LblPrijs1.Content))
        //    {
        //        BtnStore1.IsEnabled = true;
        //    }
        //    else
        //        BtnStore1.IsEnabled = false;
               
        //    //do
        //    //{
        //    //    BtnStore1.IsEnabled = true;
        //    //} while (UpdateScore() >= Convert.ToDouble(LblPrijs1.Content));
        //}

        private void BtnStore1_Click(object sender, RoutedEventArgs e)
        {
            double aankoopprijs = AankoopCursor();
            AftrekAankoop(aankoopprijs);
            clicker++;
            LblAantalKlik1.Content = clicker.ToString();           
            
        }

        private double AankoopCursor()
        {            
            double basisCursor = Convert.ToDouble(LblPrijs1.Content);
            double aankoopprijs = basisCursor * Math.Pow(1.15, Convert.ToDouble(LblAantalKlik1.Content));
            aankoopprijs = Math.Round(aankoopprijs);

            LblPrijs1.Content = aankoopprijs.ToString();

            return aankoopprijs;
        }

        private void AftrekAankoop(double aankoopprijs)
        {
            teller -= aankoopprijs;
            UpdateScore();
            UpdateTitle();
        }
    }
}