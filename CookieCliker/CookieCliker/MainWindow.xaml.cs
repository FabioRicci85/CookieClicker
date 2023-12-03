using Microsoft.VisualBasic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CookieCliker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double cookieCounter = 100000000;
        private double clicker = 0;
        private Label labelPrijs = new Label();
        private Label labelAantKlik = new Label();
        private double basePrice = new double();

        private Label labelBakeryName = new Label();                                                               //nieuwe labels en buttons voor UI

        private bool isMouseDown = false;

        private Uri pop = new Uri(@"../../Sound/pop1.mp3", UriKind.RelativeOrAbsolute);                             //Audio om af te spelen
        private Uri ping = new Uri(@"../../Sound/ping1.mp3", UriKind.RelativeOrAbsolute);
        private Uri succes = new Uri(@"../../Sound/succes1.mp3", UriKind.RelativeOrAbsolute);
        private Uri motivation = new Uri(@"../../Sound/motivation1.mp3", UriKind.RelativeOrAbsolute);
        private Uri mooFarm = new Uri(@"../../Sound/moo1.mp3", UriKind.RelativeOrAbsolute);
        private Uri clickCursor = new Uri(@"../../Sound/click1.MP3", UriKind.RelativeOrAbsolute);
        private Uri grandma = new Uri(@"../../Sound/grandma1.mp3", UriKind.RelativeOrAbsolute);
        private Uri factory = new Uri(@"../../Sound/factory1.mp3", UriKind.RelativeOrAbsolute);
        private Uri mine = new Uri(@"../../Sound/mine1.MP3", UriKind.RelativeOrAbsolute);
        private Uri bank = new Uri(@"../../Sound/bank.MP3", UriKind.RelativeOrAbsolute);
        private Uri temple = new Uri(@"../../Sound/temple.MP3", UriKind.RelativeOrAbsolute);

        private const double basisPrijs1 = 15;
        private const double basisPrijs2 = 100;
        private const double basisPrijs3 = 1100;
        private const double basisPrijs4 = 12000;
        private const double basisPrijs5 = 130000;
        private const double basisPrijs6 = 1400000;
        private const double basisPrijs7 = 20000000;

        public MainWindow()
        {
            InitializeComponent();
            MotivationSound();

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            timer.Tick += new EventHandler(PassiveIncome);
            timer.Start();

            LabelBakery();
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
            ImgCookie.Width = double.NaN;

            cookieCounter++;
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
                cookieCounter++;
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
            ImgCookie.Width = double.NaN;

            isMouseDown = false;
        }

        /// <summary>
        /// Zorgt voor de update van de score boven het koekje en in de titelbalk.
        /// </summary>
        /// <returns></returns>
        private void UpdateScore()
        {
            LblScore.Content = Math.Floor(cookieCounter).ToString();

            Title = $"{Math.Floor(cookieCounter)} cookies  -  Cookie Clicker";
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
            double price = Convert.ToDouble(labelPrijs.Content);
            if (Convert.ToDouble(labelAantKlik.Content) == 0)
            {
                cookieCounter -= price;
                UpdateScore();

                price = basePrice * 1.15;
            }
            else if (Convert.ToDouble(labelAantKlik.Content) == 1)
            {
                {
                    cookieCounter -= price;
                    UpdateScore();

                    price = basePrice * Math.Pow(1.15, 2);
                }
            }
            else if(cookieCounter >= (Convert.ToDouble(labelPrijs.Content)))
            {
                cookieCounter -= price;
                UpdateScore();

                price = basePrice * Math.Pow(1.15, (Convert.ToDouble(labelAantKlik.Content)+1));
            }

            price = Math.Ceiling(price);

            labelPrijs.Content = price.ToString();
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
            //hier achtergrond Method plaatsen
            labelAantKlik.Content = clicker.ToString();
            ShopButtonEnable();
            UpdateScore();
        }

        private void StoreButton(string button)
        {
            switch (button)
            {
                case "1":
                    labelPrijs = LblPrijs1;
                    labelAantKlik = LblAantalKlik1;
                    basePrice = basisPrijs1;
                    ClickSound();
                    break;

                case "2":
                    labelPrijs = LblPrijs2;
                    labelAantKlik = LblAantalKlik2;
                    basePrice = basisPrijs2;
                    GrandmaSound();
                    break;

                case "3":
                    labelPrijs = LblPrijs3;
                    labelAantKlik = LblAantalKlik3;
                    basePrice = basisPrijs3;
                    FarmSound();
                    break;

                case "4":
                    labelPrijs = LblPrijs4;
                    labelAantKlik = LblAantalKlik4;
                    basePrice = basisPrijs4;
                    MineSound();
                    break;

                case "5":
                    labelPrijs = LblPrijs5;
                    labelAantKlik = LblAantalKlik5;
                    basePrice = basisPrijs5;
                    FactorySound();
                    break;
                case "6":
                    labelPrijs = LblPrijs6;
                    labelAantKlik = LblAantalKlik6;
                    basePrice = basisPrijs6;
                    BankSound();
                    break;
                case "7":
                    labelPrijs = LblPrijs7;
                    labelAantKlik = LblAantalKlik7;
                    basePrice = basisPrijs7;
                    TempleSound();
                    break;
            }
        }

        /// <summary>
        /// Kijkt na als er genoeg cookies verdiend/ beschikbaar zijn en maakt dan pas de aankoopknoppen zichtbaar.
        /// </summary>
        private void ShopButtonEnable()
        {
            BtnStore1.IsEnabled = (cookieCounter >= Convert.ToDouble(LblPrijs1.Content));
            BtnStore2.IsEnabled = (cookieCounter >= Convert.ToDouble(LblPrijs2.Content));
            BtnStore3.IsEnabled = (cookieCounter >= Convert.ToDouble(LblPrijs3.Content));
            BtnStore4.IsEnabled = (cookieCounter >= Convert.ToDouble(LblPrijs4.Content));
            BtnStore5.IsEnabled = (cookieCounter >= Convert.ToDouble(LblPrijs5.Content));
            BtnStore6.IsEnabled = (cookieCounter >= Convert.ToDouble(LblPrijs6.Content));
            BtnStore7.IsEnabled = (cookieCounter >= Convert.ToDouble(LblPrijs7.Content));
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
                cookieCounter += Convert.ToDouble(LblAantalKlik1.Content) * 0.001;
            }
            if (Convert.ToDouble(LblAantalKlik2.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik2.Content) * 0.01;
            }
            if (Convert.ToDouble(LblAantalKlik3.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik3.Content) * 0.08;
            }
            if (Convert.ToDouble(LblAantalKlik4.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik4.Content) * 0.47;
            }
            if (Convert.ToDouble(LblAantalKlik5.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik5.Content) * 2.60;
            }
            if (Convert.ToDouble(LblAantalKlik6.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik6.Content) * 14;
            }
            if (Convert.ToDouble(LblAantalKlik7.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik7.Content) * 78;
            }

            LblTijd.Content = DateTime.Now.ToString("HH:mm:ss.fff");

            UpdateScore();
            ShopButtonEnable();
        }

                                                                                        // Players die specifieke geluiden afspelen bij bepaalde acties

        private readonly MediaPlayer backgroundPlayer = new MediaPlayer();
        private readonly MediaPlayer soundPlayer = new MediaPlayer();

        private void PopSound()
        {
            soundPlayer.Open(pop);
            soundPlayer.Volume = 0.2;
            soundPlayer.Play();
        }

        private void PingSound()
        {
            soundPlayer.Open(ping);
            soundPlayer.Play();
        }

        private void MotivationSound()
        {
            backgroundPlayer.Open(motivation);
            backgroundPlayer.Volume = 0.2;
            backgroundPlayer.Play();
            backgroundPlayer.MediaEnded += OnMediaEnded;
        }

        private void OnMediaEnded(object sender, EventArgs e)
        {
            backgroundPlayer.Position = TimeSpan.Zero;
            backgroundPlayer.Play();
        }

        private void ClickSound()
        {
            soundPlayer.Open(clickCursor);
            soundPlayer.Play();
        }
        private void GrandmaSound()
        {
            soundPlayer.Open(grandma);
            soundPlayer.Volume = 0.2;
            soundPlayer.Play();
        }

        private void FarmSound()
        {
            soundPlayer.Open(mooFarm);
            soundPlayer.Play();
        }
        private void MineSound()
        {
            soundPlayer.Open(mine);
            soundPlayer.Play();
        }


        private void FactorySound()
        {
            soundPlayer.Open(factory);
            soundPlayer.Play();
        }

        private void BankSound()
        {
            soundPlayer.Open(bank);
            soundPlayer.Play();
        }
        private void TempleSound()
        {
            soundPlayer.Open(temple);
            soundPlayer.Play();
        }

        private void LabelBakery()
        {
            labelBakeryName.Content = "PXL-Bakery";
            labelBakeryName.Width = 250;
            labelBakeryName.FontSize = 35;
            labelBakeryName.Foreground = Brushes.LightCyan;
            labelBakeryName.HorizontalContentAlignment = HorizontalAlignment.Center;
            labelBakeryName.VerticalContentAlignment = VerticalAlignment.Top;
            labelBakeryName.HorizontalAlignment = HorizontalAlignment.Center;
            labelBakeryName.VerticalAlignment = VerticalAlignment.Top;
            GrdWindow.Children.Add(labelBakeryName);
            Grid.SetColumn(labelBakeryName, 1);
            Grid.SetRow(labelBakeryName, 0);

            labelBakeryName.MouseUp += LabelBakeryName_MouseUp;
        }

        private void LabelBakeryName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string newBakeryName = Interaction.InputBox("Geef je eigen naam voor je bakerij in :)", "Bakerij Naam", "PXL-Bakery");

            bool hasAllWhitespace = newBakeryName.Trim().Length == 0;

            while (string.IsNullOrEmpty(newBakeryName) || hasAllWhitespace)
            {
                MessageBox.Show("Gelieve een naam in te geven");
                newBakeryName = Interaction.InputBox("Geef je eigen naam voor je bakerij in :)", "Bakerij Naam", "PXL-Bakery");
                hasAllWhitespace = newBakeryName.Trim().Length == 0;
            }

            labelBakeryName.Content = newBakeryName;
        }
    }
}