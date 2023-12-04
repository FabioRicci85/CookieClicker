using Microsoft.VisualBasic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CookieCliker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double cookieCounter = 1000000000;
        private double cookieTotal = 1000000000;
        private double clicker = 0;
        private double passiveCounter = 0;
        private Label labelPrijs = new Label();
        private Label labelAantKlik = new Label();
        private double basePrice = new double();

        private readonly Label labelBakeryName = new Label();                                                               //nieuwe labels en buttons voor UI

        private bool isMouseDown = false;

        private readonly Uri pop = new Uri(@"../../Sound/pop1.mp3", UriKind.RelativeOrAbsolute);                             //Audio om af te spelen
        private readonly Uri ping = new Uri(@"../../Sound/ping1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri succes = new Uri(@"../../Sound/succes1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri motivation = new Uri(@"../../Sound/motivation1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri mooFarm = new Uri(@"../../Sound/moo1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri clickCursor = new Uri(@"../../Sound/click1.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri grandma = new Uri(@"../../Sound/grandma1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri factory = new Uri(@"../../Sound/factory1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri mine = new Uri(@"../../Sound/mine1.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri bank = new Uri(@"../../Sound/bank.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri temple = new Uri(@"../../Sound/temple.MP3", UriKind.RelativeOrAbsolute);

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

            ButtonVisibility();
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
            cookieTotal++;
            UpdateScore();
            ShopButtonEnable();
            ButtonVisibility();

            //roep sound effect pop op
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
                cookieTotal++;
                UpdateScore();
                ButtonVisibility();
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
            LblScore.Content = DoubleToWordAmount(cookieCounter);

            Title = $"{DoubleToWordAmount(cookieCounter)} cookies  -  Cookie Clicker";
        }

        /// <summary>
        /// Per aankoop wordt gekeken hoeveel maal het al aangekocht is geweest.
        /// Indien aankopen 0 zijn, wordt eerst de basisprijs gehanteerd die hard coded staat.
        /// Zijn er al aankopen gebeurd, wordt de basisprijs (hardcoded) vermedigvuldigd met 1.15 tot de macht van aantal aankopen.
        /// de aankoopprijs wordt afgerond weergegeven in het spel.
        /// </summary>
        /// <returns></returns>
        private void BuyStore()
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
            else if (cookieCounter >= (Convert.ToDouble(labelPrijs.Content)))
            {
                cookieCounter -= price;
                UpdateScore();

                price = basePrice * Math.Pow(1.15, (Convert.ToDouble(labelAantKlik.Content) + 1));
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
            BuyStore();
            clicker++;
            labelAantKlik.Content = clicker.ToString();
            PassiveCounter();
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
                    passiveCounter += 0.1;
                    break;

                case "2":
                    labelPrijs = LblPrijs2;
                    labelAantKlik = LblAantalKlik2;
                    basePrice = basisPrijs2;
                    GrandmaSound();
                    passiveCounter += 1;
                    break;

                case "3":
                    labelPrijs = LblPrijs3;
                    labelAantKlik = LblAantalKlik3;
                    basePrice = basisPrijs3;
                    FarmSound();
                    passiveCounter += 8;
                    break;

                case "4":
                    labelPrijs = LblPrijs4;
                    labelAantKlik = LblAantalKlik4;
                    basePrice = basisPrijs4;
                    MineSound();
                    passiveCounter += 47;
                    break;

                case "5":
                    labelPrijs = LblPrijs5;
                    labelAantKlik = LblAantalKlik5;
                    basePrice = basisPrijs5;
                    FactorySound();
                    passiveCounter += 260;
                    break;

                case "6":
                    labelPrijs = LblPrijs6;
                    labelAantKlik = LblAantalKlik6;
                    basePrice = basisPrijs6;
                    BankSound();
                    passiveCounter += 1400;
                    break;

                case "7":
                    labelPrijs = LblPrijs7;
                    labelAantKlik = LblAantalKlik7;
                    basePrice = basisPrijs7;
                    TempleSound();
                    passiveCounter += 7800;
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
                cookieTotal += Convert.ToDouble(LblAantalKlik1.Content) * 0.001;
            }
            if (Convert.ToDouble(LblAantalKlik2.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik2.Content) * 0.01;
                cookieTotal += Convert.ToDouble(LblAantalKlik2.Content) * 0.01;
            }
            if (Convert.ToDouble(LblAantalKlik3.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik3.Content) * 0.08;
                cookieTotal += Convert.ToDouble(LblAantalKlik3.Content) * 0.08;
            }
            if (Convert.ToDouble(LblAantalKlik4.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik4.Content) * 0.47;
                cookieTotal += Convert.ToDouble(LblAantalKlik4.Content) * 0.47;
            }
            if (Convert.ToDouble(LblAantalKlik5.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik5.Content) * 2.60;
                cookieTotal += Convert.ToDouble(LblAantalKlik5.Content) * 2.60;
            }
            if (Convert.ToDouble(LblAantalKlik6.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik6.Content) * 14;
                cookieTotal += Convert.ToDouble(LblAantalKlik6.Content) * 14;
            }
            if (Convert.ToDouble(LblAantalKlik7.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblAantalKlik7.Content) * 78;
                cookieTotal += Convert.ToDouble(LblAantalKlik7.Content) * 78;
            }

            UpdateScore();
            ShopButtonEnable();
            ButtonVisibility();
        }

        private void PassiveCounter()
        {
            LblPassive.Visibility = Visibility.Visible;
            LblPassive.Content = $"+{passiveCounter}";
        }

        // Players die specifieke geluiden afspelen bij bepaalde acties

        private readonly MediaPlayer backgroundPlayer = new MediaPlayer();
        private readonly MediaPlayer soundPlayer = new MediaPlayer();
        private readonly MediaPlayer popPlayer = new MediaPlayer();

        private void PopSound()
        {
            popPlayer.Open(pop);
            //popPlayer.Volume = 0.2;
            popPlayer.Play();
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

        private void MusicOffImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgMusic.Source = bitmapImage;
        }

        private void MusicOnImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgMusic.Source = bitmapImage;
        }

        private void SoundOffImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgSound.Source = bitmapImage;
        }

        private void SoundOnImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgSound.Source = bitmapImage;
        }

        private void ImgMusic_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string musicOnMediaPath = "./Media/MusicOn1.png";
            string musicOffMediaPath = "./Media/MusicOff1.png";

            if (ImgMusic.Tag != null)
            {
                if (ImgMusic.Tag.ToString() == "On")
                {
                    backgroundPlayer.Volume = 0;
                    MusicOffImage(musicOffMediaPath);
                    ImgMusic.Tag = "Off";
                }
                else if (ImgMusic.Tag.ToString() == "Off")
                {
                    backgroundPlayer.Volume = 0.2;
                    MusicOnImage(musicOnMediaPath);
                    ImgMusic.Tag = "On";
                }
            }
        }

        private void ImgSound_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string soundOnMediaPath = "./Media/SoundOn1.png";
            string soundOffMediaPath = "./Media/SoundOff1.png";

            if (ImgSound.Tag != null && ImgCookie.Tag != null)
            {
                if (ImgSound.Tag.ToString() == "On")
                {
                    soundPlayer.Volume = 0;
                    SoundOffImage(soundOffMediaPath);
                    ImgSound.Tag = "Off";
                    popPlayer.Volume = 0;
                    ImgCookie.Tag = "Off";
                }
                else if (ImgSound.Tag.ToString() == "Off")
                {
                    soundPlayer.Volume = 1;
                    SoundOnImage(soundOnMediaPath);
                    ImgSound.Tag = "On";
                    popPlayer.Volume = 0.2;
                    ImgCookie.Tag = "On";
                }
            }
        }

        private void LabelBakery()
        {
            labelBakeryName.Content = "PXL's Bakery";
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
            string newBakeryName = Interaction.InputBox("Geef je eigen naam voor je bakerij in :)", "Bakerij Naam", "PXL");

            bool hasAllWhitespace = newBakeryName.Trim().Length == 0;

            while (string.IsNullOrEmpty(newBakeryName) || hasAllWhitespace)
            {
                MessageBox.Show("Gelieve een naam in te geven");
                newBakeryName = Interaction.InputBox("Geef je eigen naam voor je bakerij in :)", "Bakerij Naam", "PXL");
                hasAllWhitespace = newBakeryName.Trim().Length == 0;
            }

            labelBakeryName.Content = $"{newBakeryName}'s Bakery";
        }

        private string DoubleToWordAmount(double value)
        {
            double miljoen = 1000000;
            double miljard = 1000000000;
            double biljoen = 1000000000000;
            double biljard = 1000000000000000;
            double triljoen = 1000000000000000000;

            if (value >= triljoen)
            {
                return $"{Math.Round(value / triljoen, 3)} triljoen";
            }
            if (value >= biljard)
            {
                return $"{Math.Round(value / biljard, 3)} biljard";
            }
            if (value >= biljoen)
            {
                return $"{Math.Round(value / biljoen, 3)} biljoen";
            }
            if (value >= miljard)
            {
                return $"{Math.Round(value / miljard, 3)} miljard";
            }
            if (value >= miljoen)
            {
                return $"{Math.Round(value / miljoen, 3)} miljoen";
            }

            if (value >= 1000)
            {
                return $"{value:#,0}".Replace(".", " ");
            }

            return $"{Math.Floor(value)}";
        }

        private string InvestmentWordAmount(double value)
        {
            double duizend = 1000;
            double miljoen = 1000000;
            double miljard = 1000000000;
            double biljoen = 1000000000000;
            double biljard = 1000000000000000;
            double triljoen = 1000000000000000000;

            if (value >= triljoen)
            {
                return $"{Math.Round(value / triljoen, 3)} triljoen";
            }
            if (value >= biljard)
            {
                return $"{Math.Round(value / biljard, 3)} biljard";
            }
            if (value >= biljoen)
            {
                return $"{Math.Round(value / biljoen, 3)} biljoen";
            }
            if (value >= miljard)
            {
                return $"{Math.Round(value / miljard, 3)} miljard";
            }
            if (value >= miljoen)
            {
                return $"{Math.Round(value / miljoen, 3)} miljoen";
            }
            if (value >= duizend)
            {
                return $"{Math.Round(value / duizend, 1)} duizend";
            }

            return $"{Math.Ceiling(value)}";
        }

        private void ButtonVisibility()
        {
            BtnStore1.Visibility = Visibility.Collapsed;
            BtnStore2.Visibility = Visibility.Collapsed;
            BtnStore3.Visibility = Visibility.Collapsed;
            BtnStore4.Visibility = Visibility.Collapsed;
            BtnStore5.Visibility = Visibility.Collapsed;
            BtnStore6.Visibility = Visibility.Collapsed;
            BtnStore7.Visibility = Visibility.Collapsed;

            if (cookieTotal >= 15)
            {
                BtnStore1.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 100)
            {
                BtnStore2.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 1100)
            {
                BtnStore3.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 12000)
            {
                BtnStore4.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 130000)
            {
                BtnStore5.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 1400000)
            {
                BtnStore6.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 20000000)
            {
                BtnStore7.Visibility = Visibility.Visible;
            }
        }
    }
}