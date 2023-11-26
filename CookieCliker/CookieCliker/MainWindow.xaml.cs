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
        private double clicker = 0;
        Label labelPrijs = new Label();
        Label labelAantKlik = new Label();
        double basisPrijs = new double();

        bool isMouseDown = false;

        Uri pop = new Uri(@"../../Sound/pop1.mp3", UriKind.RelativeOrAbsolute);                             //Audio om af te spelen
        Uri ping = new Uri(@"../../Sound/ping1.mp3", UriKind.RelativeOrAbsolute);
        Uri motivation = new Uri(@"../../Sound/motivation1.mp3", UriKind.RelativeOrAbsolute);
        Uri mooFarm = new Uri(@"../../Sound/moo1.mp3", UriKind.RelativeOrAbsolute);
        Uri clickCursor = new Uri(@"../../Sound/click1.mp3", UriKind.RelativeOrAbsolute);
        Uri grandma = new Uri(@"../../Sound/grandma1.mp3", UriKind.RelativeOrAbsolute);
        Uri factory = new Uri(@"../../Sound/factory1.mp3", UriKind.RelativeOrAbsolute);
        Uri mine = new Uri(@"../../Sound/mine1.mp3", UriKind.RelativeOrAbsolute);

        //double[] basisprijzen = new double[] { 15, 100, 1100, 12000, 130000 };                            //Probeersel

        double basisPrijs1 = 15;
        double basisPrijs2 = 100;
        double basisPrijs3 = 1100;
        double basisPrijs4 = 12000;
        double basisPrijs5 = 130000;


        public MainWindow()
        {
            InitializeComponent();
            MotivationSound();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(PassiveIncome);
            timer.Start();
        }


        /// <summary>
        /// Verkleint het koekje een beetje zodat er een dynamische beweging ontstaat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            ImgCookie.Width = ImgCookie.ActualWidth - 10;
        }

        /// <summary>
        /// Maakt het koekje weer naar de normale grootte, verhoogt de teller met 1 (per klik 1 koekje), uopdate de score,
        /// kijkt als er genoeg koekjes zijn om de aankoop knoppen te enabelen en speelt een geluidje af iedere keer er geklikt
        /// wordt op het koekje.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgCookie_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImgCookie.Width = ImgCookie.ActualWidth + 10;

            teller++;
            UpdateScore();
            ShopButtonEnable();

            //roep sound effect op

            PopSound();
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
        /// Zorgt voor de update van de score boven het koekje en in de titelbalk.
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
        private void AankoopStore()
        {
            double aankoopprijs = Convert.ToDouble(labelPrijs.Content);
            if (Convert.ToDouble(labelAantKlik.Content) == 0)
            {
                
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
        }


        /// <summary>
        /// Click eventhandler om te bepalen op welke knop er in de winkel geklikt is geweest en voert dan een aankoopactie uit.
        /// Verhoogt het label met de aantal kliks, kijkt na als er voldoende cookies zijn om te knoppen te enabelen en update de score.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    basisPrijs = basisPrijs1;
                    ClickSound();
                    break;
                case "2":
                    labelPrijs = LblPrijs2;
                    labelAantKlik = LblAantalKlik2;
                    basisPrijs = basisPrijs2;
                    GrandmaSound();
                    break;
                case "3":
                    labelPrijs = LblPrijs3;
                    labelAantKlik = LblAantalKlik3;
                    basisPrijs = basisPrijs3;
                    FarmSound();
                    break;
                case "4":
                    labelPrijs = LblPrijs4;
                    labelAantKlik = LblAantalKlik4;
                    basisPrijs = basisPrijs4;
                    MineSound();
                    break;
                case "5":
                    labelPrijs = LblPrijs5;
                    labelAantKlik = LblAantalKlik5;
                    basisPrijs = basisPrijs5;
                    FactorySound();
                    break;
                    
            }
        }
        

        /// <summary>
        /// Kijkt na als er genoeg cookies verdiend/ beschikbaar zijn en maakt dan pas de aankoopknoppen zichtbaar.
        /// </summary>
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


        /// <summary>
        /// Passieve inkomen die worden berekend via DispatchTimer na aankoop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassiveIncome(object sender, EventArgs e)
        {
            if (Convert.ToDouble(LblAantalKlik1.Content) >= 1)
            {
                teller += Convert.ToDouble(LblAantalKlik1.Content) * 0.001;
            }
            if (Convert.ToDouble(LblAantalKlik2.Content) >= 1)
            {
                teller += Convert.ToDouble(LblAantalKlik2.Content) * 0.01;
            }
            if (Convert.ToDouble(LblAantalKlik3.Content) >= 1)
            {
                teller += Convert.ToDouble(LblAantalKlik3.Content) * 0.08;
            }
            if (Convert.ToDouble(LblAantalKlik4.Content) >= 1)
            {
                teller += Convert.ToDouble(LblAantalKlik4.Content) * 0.47;
            }
            if (Convert.ToDouble(LblAantalKlik5.Content) >= 1)
            {
                teller += Convert.ToDouble(LblAantalKlik5.Content) * 2.60;
            }

            LblTijd.Content = DateTime.Now.ToString("HH:mm:ss.fff");

            UpdateScore();
            ShopButtonEnable();
        }             
        
        // Players die specifieke geluiden afspelen bij bepaalde acties

        private void PopSound()
        {            
            var player = new MediaPlayer();
            player.Open(pop);
            player.Volume = 0.2;
            player.Play();
        }

        private void PingSound()
        {
            var player = new MediaPlayer();
            player.Open(ping);
            player.Play();
        }

        private void MotivationSound()
        {
            var player = new MediaPlayer();
            player.Open(motivation);
            player.Volume = 0.2;
            player.Play();
        }

        private void ClickSound()
        {
            var player = new MediaPlayer();
            player.Open(clickCursor);
            player.Play();
        }

        private void FarmSound()
        {
            var player = new MediaPlayer();
            player.Open(mooFarm);
            player.Play();
        }

        private void GrandmaSound()
        {
            var player = new MediaPlayer();
            player.Open(grandma);
            player.Volume = 0.3;
            player.Play();
        }

        private void FactorySound()
        {
            var player = new MediaPlayer();
            player.Open(factory);
            player.Play();
        }

        private void MineSound()
        {
            var player = new MediaPlayer();
            player.Open(mine);
            player.Play();
        }
    }
}