using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        Button winkelButton = new Button();
        Label labelPrijs = new Label();
        Label labelAantKlik = new Label();


        public MainWindow()
        {
            InitializeComponent();
            //BtnStore5.Visibility = Visibility.Collapsed;
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
            //VisibleButton();
            ShopButtonEnable();

        }

        /// <summary>
        /// Zorgt voor de update van de score boven het koekje. 
        /// </summary>
        /// <returns></returns>
        private int UpdateScore()
        {
            LblScore.Content = Math.Round(teller).ToString();
            int score = Convert.ToInt32(LblScore.Content);
            

            return score;
        }
        /// <summary>
        /// Update de score in titelbalk en zorgt voor de zichtbaarheid van de aankoopknoppen.
        /// </summary>
        private void UpdateTitle()
        {
            string titleScore = Convert.ToString(UpdateScore());
            Title = $"{titleScore} cookies  -  Cookie Clicker";

            //ShopButtonVisible();
        }

        //private void VisibleButton()
        //{
        //    if (UpdateScore() >= 60000)
        //    {
        //        BtnStore5.Visibility = Visibility.Visible;
        //    }

        //}     
       
        /// <summary>
        /// Per aankoop wordt gekeken hoeveel maal het al aangekocht is geweest.
        /// Indien aankopen 0 zijn, wordt eerst de basisprijs gehanteerd die hard coded staat.
        /// Zijn er al aankopen gebeurd, wordt de basisprijs (hardcoded) vermedigvuldigd met 1.15 tot de macht van aantal aankopen.
        /// de aankoopprijs wordt afgerond weergegeven in het spel.
        /// </summary>
        /// <returns></returns>
        private double AankoopStore()
        {            
            double basisPrijs = Convert.ToDouble(labelPrijs.Content);
            double aankoopprijs = (Convert.ToDouble(labelPrijs.Content));
            if (Convert.ToDouble(LblAantalKlik1.Content) == 0)
            {
                aankoopprijs = basisPrijs;
                teller -= aankoopprijs;
                UpdateScore();
                UpdateTitle();
                aankoopprijs = basisPrijs * 1.15;
            }
            else if (UpdateScore() >= (Convert.ToDouble(labelPrijs.Content)))
            {               
                teller -= aankoopprijs;
                UpdateScore();
                UpdateTitle();
                aankoopprijs = basisPrijs * Math.Pow(1.15, Convert.ToDouble(labelAantKlik.Content));
            }
            
            aankoopprijs = Math.Round(aankoopprijs);

            labelPrijs.Content = aankoopprijs.ToString();

            return aankoopprijs;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            string aankoop = buttonName.Substring(buttonName.Length - 1, 1);

            clicker = Convert.ToDouble(labelAantKlik.Content);
            StoreButton(aankoop);
            AankoopStore();
            //clicker.Equals(labelAantKlik);
            clicker++;
            labelAantKlik.Content = clicker.ToString();
            ShopButtonEnable();
            
        }
       

        private void StoreButton(string button)
        {
            switch(button)
            {
                case "1":
                    winkelButton = BtnStore1;
                    labelPrijs = LblPrijs1;
                    labelAantKlik = LblAantalKlik1;
                break;
                case "2":
                    winkelButton = BtnStore2;
                    labelPrijs = LblPrijs2;
                    labelAantKlik = LblAantalKlik2;
                    break;
                case "3":
                    winkelButton = BtnStore3;
                    labelPrijs = LblPrijs3;
                    labelAantKlik = LblAantalKlik3;
                    break;
                case "4":
                    winkelButton = BtnStore4;
                    labelPrijs = LblPrijs4;
                    labelAantKlik = LblAantalKlik4;
                    break;
                case "5":
                    winkelButton = BtnStore5;
                    labelPrijs = LblPrijs5;
                    labelAantKlik = LblAantalKlik5;
                    break;
                
            }
        }
        
        private void ShopButtonEnable()
        {
            if (UpdateScore() >= Convert.ToDouble(LblPrijs1.Content))
            {
                BtnStore1.IsEnabled = true; 
            }
            else
            {
                BtnStore1.IsEnabled = false;
            }
            if (UpdateScore() >= Convert.ToDouble(LblPrijs2.Content))
            {
                BtnStore2.IsEnabled = true;
            }
            else
            {
                BtnStore2.IsEnabled = false;
            }
            if (UpdateScore() >= Convert.ToDouble(LblPrijs3.Content))
            {
                BtnStore3.IsEnabled = true;
            }
            else
            {
                BtnStore3.IsEnabled = false;
            }
            if (UpdateScore() >= Convert.ToDouble(LblPrijs4.Content))
            {
                BtnStore4.IsEnabled = true;
            }
            else
            {
                BtnStore4.IsEnabled = false;
            }
            if (UpdateScore() >= Convert.ToDouble(LblPrijs5.Content))
            {
                BtnStore5.IsEnabled = true;
            }
            else
            {
                BtnStore5.IsEnabled = false;
            }

        }
    }
}