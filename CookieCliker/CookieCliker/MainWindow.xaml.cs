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
using System.Windows.Threading;

namespace CookieCliker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double teller = 0;
        private double totaleTeller = 0;
        private double clicker = 0;
        Label labelPrijs = new Label();
        Label labelAantKlik = new Label();        
        bool isMouseDown = false;
        readonly DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(PassiveIncome);            
            timer.Start();
        }

        private void ImgCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            ImgCookie.Width = ImgCookie.ActualWidth - 10;
        }

        private void ImgCookie_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImgCookie.Width = ImgCookie.ActualWidth + 10;

            teller++;            
            totaleTeller++;

            UpdateScore();          
            ShopButtonEnable();
        }

        /// <summary>
        /// Als de linkermuisknop ingedrukt is en de cursor op het koekje komt, registreert het een mouse down event en update het de score naar boven.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgCookie_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true)
            {
                teller++;
                totaleTeller++;
                UpdateScore();
            }
        }

        /// <summary>
        /// Als de linker muis knop is ingedrukt, dan registreert het niet als een mouse up event wordt uitgevoerd en wordt de score niet verhoogd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgCookie_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgCookie.Width = ImgCookie.ActualWidth + 10;
            isMouseDown = false;
        }

        /// <summary>
        /// Zorgt voor de update van de score boven het koekje. 
        /// </summary>
        /// <returns></returns>
        private void UpdateScore()
        {
            LblScore.Content = Math.Floor(teller).ToString();
            
            Title = $"{Math.Floor(teller)} cookies  -  Cookie Clicker";       
        }   
       
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
            double aankoopprijs = Convert.ToDouble(labelPrijs.Content);
            if (Convert.ToDouble(labelAantKlik.Content) == 0)
            {
                aankoopprijs = basisPrijs;
                teller -= aankoopprijs;
                UpdateScore();
                
                aankoopprijs = basisPrijs * 1.15;
            }
            else if (teller >= (Convert.ToDouble(labelPrijs.Content)))
            {               
                teller -= aankoopprijs;
                UpdateScore();
                
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

            StoreButton(aankoop);
            clicker = Convert.ToDouble(labelAantKlik.Content);
            AankoopStore();
            clicker++;
            labelAantKlik.Content = clicker.ToString();
            ShopButtonEnable();
            UpdateScore();
        }
       

        private void StoreButton(string button)
        {
            switch(button)
            {
                case "1":
                    labelPrijs = LblPrijs1;
                    labelAantKlik = LblAantalKlik1;
                break;
                case "2":
                    labelPrijs = LblPrijs2;
                    labelAantKlik = LblAantalKlik2;
                    break;
                case "3":
                    labelPrijs = LblPrijs3;
                    labelAantKlik = LblAantalKlik3;
                    break;
                case "4":
                    labelPrijs = LblPrijs4;
                    labelAantKlik = LblAantalKlik4;
                    break;
                case "5":
                    labelPrijs = LblPrijs5;
                    labelAantKlik = LblAantalKlik5;
                    break;
                    
            }
        }
        
        private void ShopButtonEnable()
        {
            BtnStore1.IsEnabled = (teller >= Convert.ToDouble(LblPrijs1.Content));
            BtnStore2.IsEnabled = (teller >= Convert.ToDouble(LblPrijs2.Content));
            BtnStore3.IsEnabled = (teller >= Convert.ToDouble(LblPrijs3.Content));
            BtnStore4.IsEnabled = (teller >= Convert.ToDouble(LblPrijs4.Content));
            BtnStore5.IsEnabled = (teller >= Convert.ToDouble(LblPrijs5.Content));
            BtnStore5.IsEnabled = (teller >= Convert.ToDouble(LblPrijs5.Content));
            BtnStore5.IsEnabled = (teller >= Convert.ToDouble(LblPrijs5.Content));

        }



        private void PassiveIncome(object sender, EventArgs e)
        {
            if (Convert.ToDouble(LblAantalKlik1.Content) > 0)
            {
                teller += (Convert.ToDouble(LblAantalKlik1.Content)) * 0.001;
                totaleTeller += (Convert.ToDouble(LblAantalKlik1.Content)) * 0.001;
            }
            if (Convert.ToDouble(LblAantalKlik2.Content) > 0)
            {
                teller += (Convert.ToDouble(LblAantalKlik2.Content)) * 0.01;
                totaleTeller += (Convert.ToDouble(LblAantalKlik2.Content)) * 0.01;
            }
            if (Convert.ToDouble(LblAantalKlik3.Content) > 0)
            {
                teller += (Convert.ToDouble(LblAantalKlik3.Content)) * 0.08;
                totaleTeller += (Convert.ToDouble(LblAantalKlik3.Content)) * 0.08;
            }
            if (Convert.ToDouble(LblAantalKlik4.Content) > 0)
            {
                teller += (Convert.ToDouble(LblAantalKlik4.Content)) * 0.47;
                totaleTeller += (Convert.ToDouble(LblAantalKlik4.Content)) * 0.47;
            }
            if (Convert.ToDouble(LblAantalKlik5.Content) > 0)
            {
                teller += (Convert.ToDouble(LblAantalKlik5.Content)) * 2.60;
                totaleTeller += (Convert.ToDouble(LblAantalKlik5.Content)) * 2.60;
            }

            UpdateScore();
            ShopButtonEnable();
        }
    }
}