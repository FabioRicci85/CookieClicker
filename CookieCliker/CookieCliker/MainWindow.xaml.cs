using Microsoft.VisualBasic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CookieCliker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double cookieCounter = 10000000;
        private double cookieTotal = 10000000;
        private double clicker = 0;
        private double passiveCounter = 0;
        private Label labelPrice = new Label();
        private Label labelClick = new Label();
        private double basePrice = new double();

        private double _cursorPrice = 15;
        private double _grandmaPrice = 100;
        private double _farmPrice = 1100;
        private double _minePrice = 12000;
        private double _factoryPrice = 130000;
        private double _bankPrice = 1400000;
        private double _templePrice = 20000000;

        private bool isMouseDown = false;

        private readonly Label labelBakeryName = new Label();                                                               //nieuwe labels voor UI

        private readonly Uri pop = new Uri(@"../../Sound/pop2.mp3", UriKind.RelativeOrAbsolute);                             //Audio om af te spelen
        private readonly Uri ping = new Uri(@"../../Sound/ping1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri succes = new Uri(@"../../Sound/succes1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri bonus = new Uri(@"../../Sound/bonus1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri motivation = new Uri(@"../../Sound/motivation1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri mooFarm = new Uri(@"../../Sound/moo1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri clickCursor = new Uri(@"../../Sound/click1.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri grandma = new Uri(@"../../Sound/grandma1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri factory = new Uri(@"../../Sound/factory1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri mine = new Uri(@"../../Sound/mine1.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri bank = new Uri(@"../../Sound/bank.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri temple = new Uri(@"../../Sound/temple.MP3", UriKind.RelativeOrAbsolute);

        private const double basePriceCursor = 15;
        private const double basePriceGrandma = 100;
        private const double basePriceFarm = 1100;
        private const double basePriceMine = 12000;
        private const double basePriceFactory = 130000;
        private const double basePriceBank = 1400000;
        private const double basePriceTemple = 20000000;

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
            PriceLabelWord();
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
        private void BuyStore(ref double price)
        {
            if (Convert.ToDouble(labelClick.Content) == 0)
            {
                cookieCounter -= price;
                UpdateScore();

                price = basePrice * 1.15;
            }
            else if (Convert.ToDouble(labelClick.Content) == 1)
            {
                {
                    cookieCounter -= price;
                    UpdateScore();

                    price = basePrice * Math.Pow(1.15, 2);
                }
            }
            else if (cookieCounter >= price)
            {
                cookieCounter -= price;
                UpdateScore();

                price = basePrice * Math.Pow(1.15, (Convert.ToDouble(labelClick.Content) + 1));
            }

            labelPrice.Content = InvestmentWordAmount(price);
        }

        private void PriceLabelWord()
        {
            LblPriceCursor.Content = InvestmentWordAmount(Convert.ToDouble(LblPriceCursor.Content));
            LblPriceGrandma.Content = InvestmentWordAmount(Convert.ToDouble(LblPriceGrandma.Content));
            LblPriceFarm.Content = InvestmentWordAmount(Convert.ToDouble(LblPriceFarm.Content));
            LblPriceMine.Content = InvestmentWordAmount(Convert.ToDouble(LblPriceMine.Content));
            LblPriceFactory.Content = InvestmentWordAmount(Convert.ToDouble(LblPriceFactory.Content));
            LblPriceBank.Content = InvestmentWordAmount(Convert.ToDouble(LblPriceBank.Content));
            LblPriceTemple.Content = InvestmentWordAmount(Convert.ToDouble(LblPriceTemple.Content));
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

            StoreButton(buttonName);

            clicker = Convert.ToDouble(labelClick.Content);
            BuyStore(buttonName);
            clicker++;
            labelClick.Content = clicker.ToString();
            PassiveCounter();
            ShopButtonEnable();
            UpdateScore();
        }

        private void BuyStore(string button)
        {
            switch (button)
            {
                case "Btn_Cursor":
                    BuyStore(ref _cursorPrice);
                    break;

                case "Btn_Grandma":
                    BuyStore(ref _grandmaPrice);
                    break;

                case "Btn_Farm":
                    BuyStore(ref _farmPrice);
                    break;

                case "Btn_Mine":
                    BuyStore(ref _minePrice);
                    break;

                case "Btn_Factory":
                    BuyStore(ref _factoryPrice);
                    break;

                case "Btn_Bank":
                    BuyStore(ref _bankPrice);
                    break;

                case "Btn_Temple":
                    BuyStore(ref _templePrice);
                    break;
            }
        }

        private void StoreButton(string button)
        {
            switch (button)
            {
                case "Btn_Cursor":
                    labelPrice = LblPriceCursor;
                    labelClick = LblClickCursor;
                    basePrice = basePriceCursor;
                    ClickSound();
                    passiveCounter += 0.1;
                    break;

                case "Btn_Grandma":
                    labelPrice = LblPriceGrandma;
                    labelClick = LblClickGrandma;
                    basePrice = basePriceGrandma;
                    GrandmaSound();
                    passiveCounter += 1;
                    AddGrandmaInvestment();
                    break;

                case "Btn_Farm":
                    labelPrice = LblPriceFarm;
                    labelClick = LblClickFarm;
                    basePrice = basePriceFarm;
                    FarmSound();
                    passiveCounter += 8;
                    AddFarmInvestment();
                    break;

                case "Btn_Mine":
                    labelPrice = LblPriceMine;
                    labelClick = LblClickMine;
                    basePrice = basePriceMine;
                    MineSound();
                    passiveCounter += 47;
                    break;

                case "Btn_Factory":
                    labelPrice = LblPriceFactory;
                    labelClick = LblClickFactory;
                    basePrice = basePriceFactory;
                    FactorySound();
                    passiveCounter += 260;
                    break;

                case "Btn_Bank":
                    labelPrice = LblPriceBank;
                    labelClick = LblClickBank;
                    basePrice = basePriceBank;
                    BankSound();
                    passiveCounter += 1400;
                    break;

                case "Btn_Temple":
                    labelPrice = LblPriceTemple;
                    labelClick = LblClickTemple;
                    basePrice = basePriceTemple;
                    TempleSound();
                    passiveCounter += 7800;
                    break;
            }
        }

        private void AddGrandmaInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"../../Media/Grandma.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckGrandma.Visibility = Visibility.Visible; 
            // Voeg de Rectangle toe aan de bestaande StackPanel
            StckGrandma.Children.Add(rectangle);
        }
        private void AddFarmInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"../../Media/Farm.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckFarm.Visibility = Visibility.Visible;
            // Voeg de Rectangle toe aan de bestaande StackPanel
            StckFarm.Children.Add(rectangle);
        }

        /// <summary>
        /// Kijkt na als er genoeg cookies verdiend/ beschikbaar zijn en maakt dan pas de aankoopknoppen zichtbaar.
        /// </summary>
        private void ShopButtonEnable()
        {
            Btn_Cursor.IsEnabled = (cookieCounter >= _cursorPrice);
            Btn_Grandma.IsEnabled = (cookieCounter >= _grandmaPrice);
            Btn_Farm.IsEnabled = (cookieCounter >= _farmPrice);
            Btn_Mine.IsEnabled = (cookieCounter >= _minePrice);
            Btn_Factory.IsEnabled = (cookieCounter >= _factoryPrice);
            Btn_Bank.IsEnabled = (cookieCounter >= _bankPrice);
            Btn_Temple.IsEnabled = (cookieCounter >= _templePrice);
        }

        /// <summary>
        /// Passieve inkomen die worden berekend via DispatchTimer na aankoop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassiveIncome(object sender, EventArgs e)
        {
            if (Convert.ToDouble(LblClickCursor.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickCursor.Content) * 0.001;
                cookieTotal += Convert.ToDouble(LblClickCursor.Content) * 0.001;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickGrandma.Content) * 0.01;
                cookieTotal += Convert.ToDouble(LblClickGrandma.Content) * 0.01;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickFarm.Content) * 0.08;
                cookieTotal += Convert.ToDouble(LblClickFarm.Content) * 0.08;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickMine.Content) * 0.47;
                cookieTotal += Convert.ToDouble(LblClickMine.Content) * 0.47;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickFactory.Content) * 2.60;
                cookieTotal += Convert.ToDouble(LblClickFactory.Content) * 2.60;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickBank.Content) * 14;
                cookieTotal += Convert.ToDouble(LblClickBank.Content) * 14;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickTemple.Content) * 78;
                cookieTotal += Convert.ToDouble(LblClickTemple.Content) * 78;
            }

            UpdateScore();
            ShopButtonEnable();
            ButtonVisibility();
        }

        private void PassiveCounter()
        {
            LblPassive.Visibility = Visibility.Visible;
            LblPassive.Content = $"+{Math.Round(passiveCounter, 2)}";
        }

        // Players die specifieke geluiden afspelen bij bepaalde acties

        private readonly MediaPlayer backgroundPlayer = new MediaPlayer();
        private readonly MediaPlayer soundPlayer = new MediaPlayer();
        private readonly MediaPlayer popPlayer = new MediaPlayer();

        private void PopSound()
        {
            popPlayer.Open(pop);
            popPlayer.Play();
        }

        private void PingSound()
        {
            soundPlayer.Open(ping);
            soundPlayer.Play();
        }

        private void BonusSound()
        {
            soundPlayer.Open(bonus);
            soundPlayer.Play();
        }

        private void SuccesSound()
        {
            soundPlayer.Open(succes);
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
                    popPlayer.Volume = 1;
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
            Btn_Cursor.Visibility = Visibility.Collapsed;
            Btn_Grandma.Visibility = Visibility.Collapsed;
            Btn_Farm.Visibility = Visibility.Collapsed;
            Btn_Mine.Visibility = Visibility.Collapsed;
            Btn_Factory.Visibility = Visibility.Collapsed;
            Btn_Bank.Visibility = Visibility.Collapsed;
            Btn_Temple.Visibility = Visibility.Collapsed;

            if (cookieTotal >= 15)
            {
                Btn_Cursor.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 100)
            {
                Btn_Grandma.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 1100)
            {
                Btn_Farm.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 12000)
            {
                Btn_Mine.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 130000)
            {
                Btn_Factory.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 1400000)
            {
                Btn_Bank.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= 20000000)
            {
                Btn_Temple.Visibility = Visibility.Visible;
            }
        }
    }
}