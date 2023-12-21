using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
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

        private double cursorMultiplier = 0.001;
        private double grandmaMultiplier = 0.01;
        private double farmMultiplier = 0.08;
        private double mineMultiplier = 0.47;
        private double factoryMultiplier = 2.60;
        private double bankMultiplier = 14;
        private double templeMultiplier = 78;

        private double passiveCursor = 0.1;
        private double passiveGrandma = 1;
        private double passiveFarm = 8;
        private double passiveMine = 47;
        private double passiveFactory = 260;
        private double passiveBank = 1400;
        private double passiveTemple = 7800;

        private const double basePriceCursor = 15;
        private const double basePriceGrandma = 100;
        private const double basePriceFarm = 1100;
        private const double basePriceMine = 12000;
        private const double basePriceFactory = 130000;
        private const double basePriceBank = 1400000;
        private const double basePriceTemple = 20000000;

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

        public MainWindow()
        {
            InitializeComponent();
            MotivationSound();

            //timer die iedere 10 milliseconden de passieve inkomen berekent
            DispatcherTimer millisecondsTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            millisecondsTimer.Tick += new EventHandler(PassiveIncome);
            millisecondsTimer.Start();

            ButtonVisibility();
            PerkVisibility();
            LabelBakery();
            PriceLabelWord();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //timer die iedere minuut de kans voor een gouden koekje berekent
            DispatcherTimer goldenCookieTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            goldenCookieTimer.Tick += new EventHandler(GoldenCookie);
            goldenCookieTimer.Start();
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
            PerkVisibility();

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
                PerkVisibility();
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

        /// <summary>
        /// Methode die het aankoop bedrag omzet van numerieke waarde naar woord.
        /// Maakt gebruik van InvestmentWordAmount methode.
        /// </summary>
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
            PerkVisibility();
            Quests();
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
                    passiveCounter += passiveCursor;
                    break;

                case "Btn_Grandma":
                    labelPrice = LblPriceGrandma;
                    labelClick = LblClickGrandma;
                    basePrice = basePriceGrandma;
                    GrandmaSound();
                    passiveCounter += passiveGrandma;
                    AddGrandmaInvestment();
                    break;

                case "Btn_Farm":
                    labelPrice = LblPriceFarm;
                    labelClick = LblClickFarm;
                    basePrice = basePriceFarm;
                    FarmSound();
                    passiveCounter += passiveFarm;
                    AddFarmInvestment();
                    break;

                case "Btn_Mine":
                    labelPrice = LblPriceMine;
                    labelClick = LblClickMine;
                    basePrice = basePriceMine;
                    MineSound();
                    passiveCounter += passiveMine;
                    AddMineInvestment();
                    break;

                case "Btn_Factory":
                    labelPrice = LblPriceFactory;
                    labelClick = LblClickFactory;
                    basePrice = basePriceFactory;
                    FactorySound();
                    passiveCounter += passiveFactory;
                    AddFactoryInvestment();
                    break;

                case "Btn_Bank":
                    labelPrice = LblPriceBank;
                    labelClick = LblClickBank;
                    basePrice = basePriceBank;
                    BankSound();
                    passiveCounter += passiveBank;
                    AddBankInvestment();
                    break;

                case "Btn_Temple":
                    labelPrice = LblPriceTemple;
                    labelClick = LblClickTemple;
                    basePrice = basePriceTemple;
                    TempleSound();
                    passiveCounter += passiveTemple;
                    AddTempleInvestment();
                    break;
            }
        }

        private void AddGrandmaInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"../../Media/Grandma.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckGrandma.Visibility = Visibility.Visible;

            StckGrandma.Children.Add(rectangle);
        }

        private void AddFarmInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"../../Media/Farm.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckFarm.Visibility = Visibility.Visible;

            StckFarm.Children.Add(rectangle);
        }

        private void AddMineInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"../../Media/Mine.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckMine.Visibility = Visibility.Visible;

            StckMine.Children.Add(rectangle);
        }

        private void AddFactoryInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"../../Media/Factory.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckFactory.Visibility = Visibility.Visible;

            StckFactory.Children.Add(rectangle);
        }

        private void AddBankInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"../../Media/bank.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckBank.Visibility = Visibility.Visible;

            StckBank.Children.Add(rectangle);
        }

        private void AddTempleInvestment()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"../../Media/temple.png", UriKind.RelativeOrAbsolute));
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            StckTemple.Visibility = Visibility.Visible;

            StckTemple.Children.Add(rectangle);
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
                cookieCounter += Convert.ToDouble(LblClickCursor.Content) * cursorMultiplier;
                cookieTotal += Convert.ToDouble(LblClickCursor.Content) * cursorMultiplier;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickGrandma.Content) * grandmaMultiplier;
                cookieTotal += Convert.ToDouble(LblClickGrandma.Content) * grandmaMultiplier;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickFarm.Content) * farmMultiplier;
                cookieTotal += Convert.ToDouble(LblClickFarm.Content) * farmMultiplier;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickMine.Content) * mineMultiplier;
                cookieTotal += Convert.ToDouble(LblClickMine.Content) * mineMultiplier;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickFactory.Content) * factoryMultiplier;
                cookieTotal += Convert.ToDouble(LblClickFactory.Content) * factoryMultiplier;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickBank.Content) * bankMultiplier;
                cookieTotal += Convert.ToDouble(LblClickBank.Content) * bankMultiplier;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 1)
            {
                cookieCounter += Convert.ToDouble(LblClickTemple.Content) * templeMultiplier;
                cookieTotal += Convert.ToDouble(LblClickTemple.Content) * templeMultiplier;
            }

            UpdateScore();
            ShopButtonEnable();
            ButtonVisibility();
            PerkVisibility();
        }

        /// <summary>
        /// Een label dat het passieve inkomen weergeeft. De passiveCounter wordt opgehaald uit StoreButton (Switch Case).
        /// </summary>
        private void PassiveCounter()
        {
            LblPassive.Visibility = Visibility.Visible;
            LblPassive.Content = $"+{Math.Round(passiveCounter, 2)}";
        }

        // Players die specifieke geluiden afspelen bij bepaalde acties

        private readonly MediaPlayer backgroundPlayer = new MediaPlayer();
        private readonly MediaPlayer soundPlayer = new MediaPlayer();
        private readonly MediaPlayer popPlayer = new MediaPlayer();

        /// <summary>
        /// Geluid dat afgespeeld wordt als er op het koekje geklikt wordt.
        /// </summary>
        private void PopSound()
        {
            popPlayer.Open(pop);
            popPlayer.Play();
        }

        /// <summary>
        /// Achtergrond muziek dat wordt afgespeeld, het moment dat de applicatie wordt opgestart.
        /// Als het liedje is afgelopen wordt het geloopt door de methode OnMediaEnded
        /// </summary>
        private void MotivationSound()
        {
            backgroundPlayer.Open(motivation);
            backgroundPlayer.Volume = 0.2;
            backgroundPlayer.Play();
            backgroundPlayer.MediaEnded += OnMediaEnded;
        }

        /// <summary>
        /// een methode die de backgroundPlayer's positie terug naar nul (begin) zet en opnieuw de achtergrond muziek afspeelt.
        /// Deze methode wordt gebruikt binnen de methode Motivation Sound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMediaEnded(object sender, EventArgs e)
        {
            backgroundPlayer.Position = TimeSpan.Zero;
            backgroundPlayer.Play();
        }

        // Soundeffects voor cookie, buttons, powerupsen quests.

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

        /// <summary>
        /// Afbeelding Music OFF
        /// </summary>
        /// <param name="newImagePath"></param>
        private void MusicOffImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgMusic.Source = bitmapImage;
        }

        /// <summary>
        /// Afbeelding Music ON
        /// </summary>
        /// <param name="newImagePath"></param>
        private void MusicOnImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgMusic.Source = bitmapImage;
        }

        /// <summary>
        /// Afbeelding Sound OFF
        /// </summary>
        /// <param name="newImagePath"></param>
        private void SoundOffImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgSound.Source = bitmapImage;
        }

        /// <summary>
        /// Afbeelding Sound ON
        /// </summary>
        /// <param name="newImagePath"></param>
        private void SoundOnImage(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgSound.Source = bitmapImage;
        }

        /// <summary>
        /// Een Mouse Up event om het icoontje Music ON te wisselen naar Music OFF, en om het achtergrond geluid in en uit te schakelen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Een Mouse Up event om het icoontje Sound ON te wisselen naar Sound OFF, en om de soundeffects in en uit te schakelen.
        /// Zowel de button geluiden als het geluidje op de cookie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Label om bakkerij naam weer te geven.
        /// </summary>
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

        /// <summary>
        /// Een mouseUp event dat nakijkt of de ingegeven waarde niet leeg is, of enkel uit spaties bestaat. Als deze correct zijn wordt de nieuwe ingegeven naam weergegeven.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Zorgt voor het verwoorden van de numerieke waarde van het aantal cookies.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Zorgt voor de verwoording van de numerieke waarde van de aankoopprijs.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Een methode om alle aankoop knoppen niet zichtbaar te maken, en slechts zichtbaar te maken na dat het totaal verzamelde cookies een bepaald aantal heeft behaald.
        /// cookieTotal is de variabele die steeds blijft optellen, waar nooit aankoopbedragen van afgetrokken worden.
        /// </summary>
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

        // Quests en PowerUps die de speler kan aankopen

        private void Quests()
        {
            if (LblClickGrandma.Content.ToString() == "20")
            {
                MessageBox.Show("20 grandma's die voor je werken, opa's hebben tijd voor een lemon party ;)");
            }
            if (LblClickFarm.Content.ToString() == "20")
            {
                MessageBox.Show("20 farms, de koeien zijn blij met hun nieuwe stal");
            }
        }

        /// <summary>
        /// Kansberekning voor een gouden koekje, 30% kans dat er een gouden koekje verschijnt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoldenCookie(object sender, EventArgs e)
        {
            double random = new Random().NextDouble();
            double chance = 0.3;

            if (random < chance)
            {
                ImgGoldenCookieRandomPlace();
                BonusSound();
            }
        }

        /// <summary>
        /// Actie die uitgevoerd wordt als er op het gouden koekje geklikt wordt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgGoldenCookie_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImgGoldenCookie.Visibility = Visibility.Collapsed;
            cookieCounter += (passiveCounter * 900);
            cookieTotal += (passiveCounter * 900);
            UpdateScore();
            SuccesSound();
        }

        /// <summary>
        /// ImgGoldenCookie random op het scherm laten verschijnen
        /// </summary>
        private void ImgGoldenCookieRandomPlace()
        {
            Random random = new Random();
            double x = random.Next(0, (int)(ActualWidth - ImgGoldenCookie.Width));
            double y = random.Next(0, (int)(ActualHeight - ImgGoldenCookie.Height));

            ImgGoldenCookie.Margin = new Thickness(x, y, 0, 0);

            ImgGoldenCookie.Visibility = Visibility.Visible;
        }

        //Perk Upgrades die de speler kan aankopen om de passieve inkomen te verhogen
        private void BtnPerk_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            
            BuyPerk(buttonName);
        }

        private void BuyPerk(string button)
        {
            switch (button)
            {
                case "BtnPerkCursor1":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    basePrice = basePriceCursor;
                    break;

                case "BtnPerkCursor2":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    basePrice = basePriceCursor;
                    break;

                case "BtnPerkCursor3":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    basePrice = basePriceCursor;
                    break;

                case "BtnPerkCursor4":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    basePrice = basePriceCursor;
                    break;

                case "BtnPerkCursor5":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    basePrice = basePriceCursor;
                    break;
                case "BtnPerkGrandma1":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    basePrice = basePriceGrandma;
                    break;
                case "BtnPerkGrandma2":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    basePrice = basePriceGrandma;
                    break;
                case "BtnPerkGrandma3":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    basePrice = basePriceGrandma;
                    break;
                case "BtnPerkGrandma4":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    basePrice = basePriceGrandma;
                    break;
                case "BtnPerkGrandma5":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    basePrice = basePriceGrandma;
                    break;
                case "BtnPerkFarm1":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    basePrice = basePriceFarm;
                    break;
                case "BtnPerkFarm2":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    basePrice = basePriceFarm;
                    break;
                case "BtnPerkFarm3":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    basePrice = basePriceFarm;
                    break;
                case "BtnPerkFarm4":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    basePrice = basePriceFarm;
                    break;
                case "BtnPerkFarm5":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    basePrice = basePriceFarm;
                    break;
                case "BtnPerkMine1":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    basePrice = basePriceMine;
                    break;
                case "BtnPerkMine2":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    basePrice = basePriceMine;
                    break;
                case "BtnPerkMine3":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    basePrice = basePriceMine;
                    break;
                case "BtnPerkMine4":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    basePrice = basePriceMine;
                    break;
                case "BtnPerkMine5":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    basePrice = basePriceMine;
                    break;
                case "BtnPerkFactory1":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    basePrice = basePriceFactory;
                    break;
                case "BtnPerkFactory2":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    basePrice = basePriceFactory;
                    break;
                case "BtnPerkFactory3":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    basePrice = basePriceFactory;
                    break;
                case "BtnPerkFactory4":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    basePrice = basePriceFactory;
                    break;
                case "BtnPerkFactory5":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    basePrice = basePriceFactory;
                    break;
                case "BtnPerkBank1":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    basePrice = basePriceBank;
                    break;
                case "BtnPerkBank2":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    basePrice = basePriceBank;
                    break;
                case "BtnPerkBank3":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    basePrice = basePriceBank;
                    break;
                case "BtnPerkBank4":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    basePrice = basePriceBank;
                    break;
                case "BtnPerkBank5":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    basePrice = basePriceBank;
                    break;
                case "BtnPerkTemple1":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    basePrice = basePriceTemple;
                    break;
                case "BtnPerkTemple2":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    basePrice = basePriceTemple;
                    break;
                case "BtnPerkTemple3":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    basePrice = basePriceTemple;
                    break;
                case "BtnPerkTemple4":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    basePrice = basePriceTemple;
                    break;
                case "BtnPerkTemple5":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    basePrice = basePriceTemple;
                    break;
            }
        }
        private bool btnPerkCursor1Clicked = false;
        private bool btnPerkCursor2Clicked = false;
        private bool btnPerkCursor3Clicked = false;
        private bool btnPerkCursor4Clicked = false;
        private bool btnPerkCursor5Clicked = false;
        private bool btnPerkGrandma1Clicked = false;
        private bool btnPerkGrandma2Clicked = false;
        private bool btnPerkGrandma3Clicked = false;
        private bool btnPerkGrandma4Clicked = false;
        private bool btnPerkGrandma5Clicked = false;
        private bool btnPerkFarm1Clicked = false;
        private bool btnPerkFarm2Clicked = false;
        private bool btnPerkFarm3Clicked = false;
        private bool btnPerkFarm4Clicked = false;
        private bool btnPerkFarm5Clicked = false;
        private bool btnPerkMine1Clicked = false;
        private bool btnPerkMine2Clicked = false;
        private bool btnPerkMine3Clicked = false;
        private bool btnPerkMine4Clicked = false;
        private bool btnPerkMine5Clicked = false;
        private bool btnPerkFactory1Clicked = false;
        private bool btnPerkFactory2Clicked = false;
        private bool btnPerkFactory3Clicked = false;
        private bool btnPerkFactory4Clicked = false;
        private bool btnPerkFactory5Clicked = false;
        private bool btnPerkBank1Clicked = false;
        private bool btnPerkBank2Clicked = false;
        private bool btnPerkBank3Clicked = false;
        private bool btnPerkBank4Clicked = false;
        private bool btnPerkBank5Clicked = false;
        private bool btnPerkTemple1Clicked = false;
        private bool btnPerkTemple2Clicked = false;
        private bool btnPerkTemple3Clicked = false;
        private bool btnPerkTemple4Clicked = false;
        private bool btnPerkTemple5Clicked = false;

        private void BuyPerk(ref double multiplier, ref double passive)
        {
            if (Convert.ToDouble(LblClickCursor.Content) >= 1 && !btnPerkCursor1Clicked)
            {
                if (BtnPerkCursor1.IsEnabled = cookieCounter >= basePrice * 100)
                {
                    cookieCounter -= basePrice * 100;
                    UpdateScore();

                    passiveCounter += passive;
                    PassiveCounter();

                    multiplier *= 2;
                    passive *= 2;

                    BtnPerkCursor1.Visibility = Visibility.Collapsed;
                    BonusSound();

                    btnPerkCursor1Clicked = true;
                }
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 5 && !btnPerkCursor2Clicked)
            {
                if (BtnPerkCursor2.IsEnabled = cookieCounter >= basePrice * 500)
                {
                    cookieCounter -= basePrice * 500;
                    UpdateScore();

                    PassiveCalculate(ref multiplier, ref passive);

                    BtnPerkCursor2.Visibility = Visibility.Collapsed;
                    BonusSound();

                    btnPerkCursor2Clicked = true;
                }
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 25 && !btnPerkCursor3Clicked)
            {
                if (BtnPerkCursor3.IsEnabled = cookieCounter >= basePrice * 5000)
                {
                    cookieCounter -= basePrice * 5000;
                    UpdateScore();

                    PassiveCalculate(ref multiplier, ref passive);

                    BtnPerkCursor3.Visibility = Visibility.Collapsed;
                    BonusSound();

                    btnPerkCursor3Clicked = true;
                }               
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 50 && !btnPerkCursor4Clicked)
            {
                if (BtnPerkCursor4.IsEnabled = cookieCounter >= basePrice * 50000)
                {
                    cookieCounter -= basePrice * 50000;
                    UpdateScore();

                    PassiveCalculate(ref multiplier, ref passive);

                    BtnPerkCursor3.Visibility = Visibility.Collapsed;
                    BonusSound();

                    btnPerkCursor4Clicked = true;
                }                
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 75 && !btnPerkCursor5Clicked)
            {
                if (BtnPerkCursor5.IsEnabled = cookieCounter >= basePrice * 500000)
                {
                    cookieCounter -= basePrice * 500000;
                    UpdateScore();

                    PassiveCalculate(ref multiplier, ref passive);

                    BtnPerkCursor3.Visibility = Visibility.Collapsed;
                    BonusSound();

                    btnPerkCursor5Clicked = true;
                }
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 1 && !btnPerkGrandma1Clicked)
            {
                cookieCounter -= basePrice * 100;
                UpdateScore();

                passiveCounter += passive;
                PassiveCounter();

                multiplier *= 2;
                passive *= 2;

                BtnPerkGrandma1.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkGrandma1Clicked = true;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 5 && !btnPerkGrandma2Clicked)
            {
                cookieCounter -= basePrice * 500;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkGrandma2.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkGrandma2Clicked = true;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 25 && !btnPerkGrandma3Clicked)
            {
                cookieCounter -= basePrice * 5000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkGrandma3.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkGrandma3Clicked = true;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 50 && !btnPerkGrandma4Clicked)
            {
                cookieCounter -= basePrice * 50000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkGrandma4.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkGrandma4Clicked = true;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 75 && !btnPerkGrandma5Clicked)
            {
                cookieCounter -= basePrice * 500000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkGrandma5.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkGrandma5Clicked = true;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 1 && !btnPerkFarm1Clicked)
            {
                cookieCounter -= basePrice * 100;
                UpdateScore();

                passiveCounter += passive;
                PassiveCounter();

                multiplier *= 2;
                passive *= 2;

                BtnPerkFarm1.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFarm1Clicked = true;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 5 && !btnPerkFarm2Clicked)
            {
                cookieCounter -= basePrice * 500;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFarm2.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFarm2Clicked = true;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 25 && !btnPerkFarm3Clicked)
            {
                cookieCounter -= basePrice * 5000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFarm3.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFarm3Clicked = true;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 50 && !btnPerkFarm4Clicked)
            {
                cookieCounter -= basePrice * 50000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFarm4.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFarm4Clicked = true;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 75 && !btnPerkFarm5Clicked)
            {
                cookieCounter -= basePrice * 500000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFarm5.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFarm5Clicked = true;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 1 && !btnPerkMine1Clicked)
            {
                cookieCounter -= basePrice * 100;
                UpdateScore();

                passiveCounter += passive;
                PassiveCounter();

                multiplier *= 2;
                passive *= 2;

                BtnPerkMine1.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkMine1Clicked = true;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 5 && !btnPerkMine2Clicked)
            {
                cookieCounter -= basePrice * 500;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkMine2.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkMine2Clicked = true;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 25 && !btnPerkMine3Clicked)
            {
                cookieCounter -= basePrice * 5000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkMine3.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkMine3Clicked = true;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 50 && !btnPerkMine4Clicked)
            {
                cookieCounter -= basePrice * 50000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkMine4.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkMine4Clicked = true;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 75 && !btnPerkMine5Clicked)
            {
                cookieCounter -= basePrice * 500000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkMine5.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkMine5Clicked = true;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 1 && !btnPerkFactory1Clicked)
            {
                cookieCounter -= basePrice * 100;
                UpdateScore();

                passiveCounter += passive;
                PassiveCounter();

                multiplier *= 2;
                passive *= 2;

                BtnPerkFactory1.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFactory1Clicked = true;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 5 && !btnPerkFactory2Clicked)
            {
                cookieCounter -= basePrice * 500;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFactory2.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFactory2Clicked = true;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 25 && !btnPerkFactory3Clicked)
            {
                cookieCounter -= basePrice * 5000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFactory3.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFactory3Clicked = true;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 50 && !btnPerkFactory4Clicked)
            {
                cookieCounter -= basePrice * 50000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFactory4.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFactory4Clicked = true;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 75 && !btnPerkFactory5Clicked)
            {
                cookieCounter -= basePrice * 500000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkFactory5.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkFactory5Clicked = true;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 1 && !btnPerkBank1Clicked)
            {
                cookieCounter -= basePrice * 100;
                UpdateScore();

                passiveCounter += passive;
                PassiveCounter();

                multiplier *= 2;
                passive *= 2;

                BtnPerkBank1.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkBank1Clicked = true;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 5 && !btnPerkBank2Clicked)
            {
                cookieCounter -= basePrice * 500;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkBank2.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkBank2Clicked = true;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 25 && !btnPerkBank3Clicked)
            {
                cookieCounter -= basePrice * 5000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkBank3.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkBank3Clicked = true;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 50 && !btnPerkBank4Clicked)
            {
                cookieCounter -= basePrice * 50000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkBank4.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkBank4Clicked = true;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 75 && !btnPerkBank5Clicked)
            {
                cookieCounter -= basePrice * 500000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkBank5.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkBank5Clicked = true;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 1 && !btnPerkTemple1Clicked)
            {
                cookieCounter -= basePrice * 100;
                UpdateScore();

                passiveCounter += passive;
                PassiveCounter();

                multiplier *= 2;
                passive *= 2;

                BtnPerkTemple1.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkTemple1Clicked = true;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 5 && !btnPerkTemple2Clicked)
            {
                cookieCounter -= basePrice * 500;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkTemple2.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkTemple2Clicked = true;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 25 && !btnPerkTemple3Clicked)
            {
                cookieCounter -= basePrice * 5000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkTemple3.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkTemple3Clicked = true;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 50 && !btnPerkTemple4Clicked)
            {
                cookieCounter -= basePrice * 50000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkTemple4.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkTemple4Clicked = true;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 75 && !btnPerkTemple5Clicked)
            {
                cookieCounter -= basePrice * 500000;
                UpdateScore();

                PassiveCalculate(ref multiplier, ref passive);

                BtnPerkTemple5.Visibility = Visibility.Collapsed;
                BonusSound();

                btnPerkTemple5Clicked = true;
            }

        }

        private void BtnPerk_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;

            BuyPerk(buttonName);
        }

        private void PassiveCalculate(ref double multiplier, ref double passive)
        {
            passiveCounter += passive * 2;
            PassiveCounter();

            multiplier *= 2;
            passive *= 2;
        }

        private void PerkVisibility()
        {
            BtnPerkCursor1.Visibility = Visibility.Collapsed;
            BtnPerkCursor2.Visibility = Visibility.Collapsed;
            BtnPerkCursor3.Visibility = Visibility.Collapsed;
            BtnPerkCursor4.Visibility = Visibility.Collapsed;
            BtnPerkCursor5.Visibility = Visibility.Collapsed;
            BtnPerkGrandma1.Visibility = Visibility.Collapsed;
            BtnPerkGrandma2.Visibility = Visibility.Collapsed;
            BtnPerkGrandma3.Visibility = Visibility.Collapsed;
            BtnPerkGrandma4.Visibility = Visibility.Collapsed;
            BtnPerkGrandma5.Visibility = Visibility.Collapsed;
            BtnPerkFarm1.Visibility = Visibility.Collapsed;
            BtnPerkFarm2.Visibility = Visibility.Collapsed;
            BtnPerkFarm3.Visibility = Visibility.Collapsed;
            BtnPerkFarm4.Visibility = Visibility.Collapsed;
            BtnPerkFarm5.Visibility = Visibility.Collapsed;
            BtnPerkMine1.Visibility = Visibility.Collapsed;
            BtnPerkMine2.Visibility = Visibility.Collapsed;
            BtnPerkMine3.Visibility = Visibility.Collapsed;
            BtnPerkMine4.Visibility = Visibility.Collapsed;
            BtnPerkMine5.Visibility = Visibility.Collapsed;
            BtnPerkFactory1.Visibility = Visibility.Collapsed;
            BtnPerkFactory2.Visibility = Visibility.Collapsed;
            BtnPerkFactory3.Visibility = Visibility.Collapsed;
            BtnPerkFactory4.Visibility = Visibility.Collapsed;
            BtnPerkFactory5.Visibility = Visibility.Collapsed;
            BtnPerkBank1.Visibility = Visibility.Collapsed;
            BtnPerkBank2.Visibility = Visibility.Collapsed;
            BtnPerkBank3.Visibility = Visibility.Collapsed;
            BtnPerkBank4.Visibility = Visibility.Collapsed;
            BtnPerkBank5.Visibility = Visibility.Collapsed;
            BtnPerkTemple1.Visibility = Visibility.Collapsed;
            BtnPerkTemple2.Visibility = Visibility.Collapsed;
            BtnPerkTemple3.Visibility = Visibility.Collapsed;
            BtnPerkTemple4.Visibility = Visibility.Collapsed;
            BtnPerkTemple5.Visibility = Visibility.Collapsed;

            if (Convert.ToDouble(LblClickCursor.Content) >= 1 && !btnPerkCursor1Clicked)
            {
                BtnPerkCursor1.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 5 && !btnPerkCursor2Clicked)
            {
                BtnPerkCursor2.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 25 && !btnPerkCursor3Clicked)
            {
                BtnPerkCursor3.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 50 && !btnPerkCursor4Clicked)
            {
                BtnPerkCursor4.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickCursor.Content) >= 75 && !btnPerkCursor5Clicked)
            {
                BtnPerkCursor5.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 1 && !btnPerkGrandma1Clicked)
            {
                BtnPerkGrandma1.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 5 && !btnPerkGrandma2Clicked)
            {
                BtnPerkGrandma2.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 25 && !btnPerkGrandma3Clicked)
            {
                BtnPerkGrandma3.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 50 && !btnPerkGrandma4Clicked)
            {
                BtnPerkGrandma4.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickGrandma.Content) >= 75 && !btnPerkGrandma5Clicked)
            {
                BtnPerkGrandma5.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 1 && !btnPerkFarm1Clicked)
            {
                BtnPerkFarm1.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 5 && !btnPerkFarm2Clicked)
            {
                BtnPerkFarm2.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 25 && !btnPerkFarm3Clicked)
            {
                BtnPerkFarm3.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 50 && !btnPerkFarm4Clicked)
            {
                BtnPerkFarm4.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFarm.Content) >= 75 && !btnPerkFarm5Clicked)
            {
                BtnPerkFarm5.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 1 && !btnPerkMine1Clicked)
            {
                BtnPerkMine1.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 5 && !btnPerkMine2Clicked)
            {
                BtnPerkMine2.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 25 && !btnPerkMine3Clicked)
            {
                BtnPerkMine3.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 50 && !btnPerkMine4Clicked)
            {
                BtnPerkMine4.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickMine.Content) >= 75 && !btnPerkMine5Clicked)
            {
                BtnPerkMine5.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 1 && !btnPerkFactory1Clicked)
            {
                BtnPerkFactory1.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 5 && !btnPerkFactory2Clicked)
            {
                BtnPerkFactory2.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 25 && !btnPerkFactory3Clicked)
            {
                BtnPerkFactory3.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 50 && !btnPerkFactory4Clicked)
            {
                BtnPerkFactory4.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickFactory.Content) >= 75 && !btnPerkFactory5Clicked)
            {
                BtnPerkFactory5.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 1 && !btnPerkBank1Clicked)
            {
                BtnPerkBank1.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 5 && !btnPerkBank2Clicked)
            {
                BtnPerkBank2.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 25 && !btnPerkBank3Clicked)
            {
                BtnPerkBank3.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 50 && !btnPerkBank4Clicked)
            {
                BtnPerkBank4.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickBank.Content) >= 75 && !btnPerkBank5Clicked)
            {
                BtnPerkBank5.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 1 && !btnPerkTemple1Clicked)
            {
                BtnPerkTemple1.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 5 && !btnPerkTemple2Clicked)
            {
                BtnPerkTemple2.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 25 && !btnPerkTemple3Clicked)
            {
                BtnPerkTemple3.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 50 && !btnPerkTemple4Clicked)
            {
                BtnPerkTemple4.Visibility = Visibility.Visible;
            }
            if (Convert.ToDouble(LblClickTemple.Content) >= 75 && !btnPerkTemple5Clicked)
            {
                BtnPerkTemple5.Visibility = Visibility.Visible;
            }
        }
    }
}