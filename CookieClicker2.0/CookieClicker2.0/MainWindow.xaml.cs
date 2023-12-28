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
using System.Windows.Threading;
using Microsoft.VisualBasic;


namespace CookieClicker2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double cookieCounter = 100000000;
        private double cookieTotal = 100000000;

        private double passiveCounter = 0;
        private Label labelPrice = new Label();
        private Label labelClick = new Label();
        private double basePrice = new double();

        private Button btnPerk = new Button();

        private double investmentAmount = new double();
        private double investmentCursorAmount = 0;                          // aantal aankopen van investeringen, dit veranderd uiteindelijk het LblClick.Content
        private double investmentGrandmaAmount = 0;                         // zodat we dat niet van voor moeten opvragen
        private double investmentFarmAmount = 0;
        private double investmentMineAmount = 0;
        private double investmentFactoryAmount = 0;
        private double investmentBankAmount = 0;
        private double investmentTempleAmount = 0;

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

        private readonly Label labelBakeryName = new Label();                                                               //nieuwe label voor UI

        private readonly Uri pop = new Uri(@"../../Sound/pop2.mp3", UriKind.RelativeOrAbsolute);                             //Audio om af te spelen
        private readonly Uri ping = new Uri(@"../../Sound/ping1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri succes = new Uri(@"../../Sound/succes1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri bonus = new Uri(@"../../Sound/bonus1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri motivation = new Uri(@"../../Sound/motivation1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri farm = new Uri(@"../../Sound/moo1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri clickCursor = new Uri(@"../../Sound/click1.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri grandma = new Uri(@"../../Sound/grandma1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri factory = new Uri(@"../../Sound/factory1.mp3", UriKind.RelativeOrAbsolute);
        private readonly Uri mine = new Uri(@"../../Sound/mine1.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri bank = new Uri(@"../../Sound/bank.MP3", UriKind.RelativeOrAbsolute);
        private readonly Uri temple = new Uri(@"../../Sound/temple.MP3", UriKind.RelativeOrAbsolute);

        private readonly BitmapImage grandmaImage = new BitmapImage(new Uri(@"../../Media/Grandma.png", UriKind.RelativeOrAbsolute));    //Afbleedingen voor UI Investeringen                                                //nieuwe imagebrush voor UI
        private readonly BitmapImage farmImage = new BitmapImage(new Uri(@"../../Media/Farm.png", UriKind.RelativeOrAbsolute));
        private readonly BitmapImage mineImage = new BitmapImage(new Uri(@"../../Media/Mine.png", UriKind.RelativeOrAbsolute));
        private readonly BitmapImage factoryImage = new BitmapImage(new Uri(@"../../Media/Factory.png", UriKind.RelativeOrAbsolute));
        private readonly BitmapImage bankImage = new BitmapImage(new Uri(@"../../Media/bank.png", UriKind.RelativeOrAbsolute));
        private readonly BitmapImage templeImage = new BitmapImage(new Uri(@"../../Media/temple.png", UriKind.RelativeOrAbsolute));

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

        /// <summary>
        /// Als het spel geladen is, wordt er een timer gestart die iedere minuut de kans berekent voor een gouden koekje.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //timer die iedere minuut de kans voor een gouden koekje berekent
            DispatcherTimer goldenCookieTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            goldenCookieTimer.Tick += new EventHandler(GoldenCookie);
            goldenCookieTimer.Start();

            AddQuests();
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
        /// Maakt het koekje weer naar de normale grootte, verhoogt de teller met 1 (per klik 1 koekje), update de score,
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

            PerkButtonEnabled();

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

                PerkButtonEnabled();
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
        private void BuyStore(ref double price, ref double investmentAmount)
        {
            if (investmentAmount == 0)
            {
                cookieCounter -= price;
                UpdateScore();

                price = basePrice * 1.15;
            }
            else if (investmentAmount == 1)
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

                price = basePrice * Math.Pow(1.15, (investmentAmount + 1));
            }

            investmentAmount++;
            labelClick.Content = investmentAmount.ToString();
            labelPrice.Content = InvestmentWordAmount(price);
        }

        /// <summary>
        /// Methode die het aankoopbedrag omzet van numerieke waarde naar woord.
        /// Maakt gebruik van InvestmentWordAmount methode.
        /// </summary>
        private void PriceLabelWord()
        {
            LblPriceCursor.Content = InvestmentWordAmount(_cursorPrice);
            LblPriceGrandma.Content = InvestmentWordAmount(_grandmaPrice);
            LblPriceFarm.Content = InvestmentWordAmount(_farmPrice);
            LblPriceMine.Content = InvestmentWordAmount(_minePrice);
            LblPriceFactory.Content = InvestmentWordAmount(_factoryPrice);
            LblPriceBank.Content = InvestmentWordAmount(_bankPrice);
            LblPriceTemple.Content = InvestmentWordAmount(_templePrice);
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
            BuyStore(buttonName);

            PassiveCounter();
            ShopButtonEnable();
            UpdateScore();
            PerkVisibility();
            PerkButtonEnabled();
            Quests();
        }

        private void BuyStore(string button)
        {
            switch (button)
            {
                case "Btn_Cursor":
                    BuyStore(ref _cursorPrice, ref investmentCursorAmount);
                    break;

                case "Btn_Grandma":
                    BuyStore(ref _grandmaPrice, ref investmentGrandmaAmount);
                    break;

                case "Btn_Farm":
                    BuyStore(ref _farmPrice, ref investmentFarmAmount);
                    break;

                case "Btn_Mine":
                    BuyStore(ref _minePrice, ref investmentMineAmount);
                    break;

                case "Btn_Factory":
                    BuyStore(ref _factoryPrice, ref investmentFactoryAmount);
                    break;

                case "Btn_Bank":
                    BuyStore(ref _bankPrice, ref investmentBankAmount);
                    break;

                case "Btn_Temple":
                    BuyStore(ref _templePrice, ref investmentTempleAmount);
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
                    PlaySound(clickCursor);
                    passiveCounter += passiveCursor;

                    break;

                case "Btn_Grandma":
                    labelPrice = LblPriceGrandma;
                    labelClick = LblClickGrandma;
                    basePrice = basePriceGrandma;
                    PlaySound(grandma);
                    passiveCounter += passiveGrandma;
                    AddInvestment(grandmaImage, StckGrandma);
                    break;

                case "Btn_Farm":
                    labelPrice = LblPriceFarm;
                    labelClick = LblClickFarm;
                    basePrice = basePriceFarm;
                    PlaySound(farm);
                    passiveCounter += passiveFarm;
                    AddInvestment(farmImage, StckFarm);
                    break;

                case "Btn_Mine":
                    labelPrice = LblPriceMine;
                    labelClick = LblClickMine;
                    basePrice = basePriceMine;
                    PlaySound(mine);
                    passiveCounter += passiveMine;
                    AddInvestment(mineImage, StckMine);
                    break;

                case "Btn_Factory":
                    labelPrice = LblPriceFactory;
                    labelClick = LblClickFactory;
                    basePrice = basePriceFactory;
                    PlaySound(factory);
                    passiveCounter += passiveFactory;
                    AddInvestment(factoryImage, StckFactory);
                    break;

                case "Btn_Bank":
                    labelPrice = LblPriceBank;
                    labelClick = LblClickBank;
                    basePrice = basePriceBank;
                    PlaySound(bank);
                    passiveCounter += passiveBank;
                    AddInvestment(bankImage, StckBank);
                    break;

                case "Btn_Temple":
                    labelPrice = LblPriceTemple;
                    labelClick = LblClickTemple;
                    basePrice = basePriceTemple;
                    PlaySound(temple);
                    passiveCounter += passiveTemple;
                    AddInvestment(templeImage, StckTemple);
                    break;
            }
        }

        private void AddInvestment(BitmapImage image, StackPanel stackPanel)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = image;
            imageBrush.TileMode = TileMode.Tile;

            Rectangle rectangle = new Rectangle();
            rectangle.Fill = imageBrush;
            rectangle.Width = 50;
            rectangle.Height = 50;

            stackPanel.Visibility = Visibility.Visible;

            stackPanel.Children.Add(rectangle);
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
            if (investmentCursorAmount >= 1)
            {
                cookieCounter += investmentCursorAmount * cursorMultiplier;
                cookieTotal += investmentCursorAmount * cursorMultiplier;
            }
            if (investmentGrandmaAmount >= 1)
            {
                cookieCounter += investmentGrandmaAmount * grandmaMultiplier;
                cookieTotal += investmentGrandmaAmount * grandmaMultiplier;
            }
            if (investmentFarmAmount >= 1)
            {
                cookieCounter += investmentFarmAmount * farmMultiplier;
                cookieTotal += investmentFarmAmount * farmMultiplier;
            }
            if (investmentMineAmount >= 1)
            {
                cookieCounter += investmentMineAmount * mineMultiplier;
                cookieTotal += investmentMineAmount * mineMultiplier;
            }
            if (investmentFactoryAmount >= 1)
            {
                cookieCounter += investmentFactoryAmount * factoryMultiplier;
                cookieTotal += investmentFactoryAmount * factoryMultiplier;
            }
            if (investmentBankAmount >= 1)
            {
                cookieCounter += investmentBankAmount * bankMultiplier;
                cookieTotal += investmentBankAmount * bankMultiplier;
            }
            if (investmentTempleAmount >= 1)
            {
                cookieCounter += investmentTempleAmount * templeMultiplier;
                cookieTotal += investmentTempleAmount * templeMultiplier;
            }

            UpdateScore();
            ShopButtonEnable();
            ButtonVisibility();
            PerkButtonEnabled();
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

        // Soundeffects voor cookie, buttons en dergelijke.
        private void PlaySound(Uri sound)
        {
            soundPlayer.Open(sound);
            soundPlayer.Play();
        }

        /// <summary>
        /// Geluid dat afgespeeld wordt als er op het koekje geklikt wordt.
        /// </summary>
        private void PopSound()
        {
            popPlayer.Open(pop);
            popPlayer.Play();
        }

        /// <summary>
        /// Afbeelding Music On/Off
        /// </summary>
        /// <param name="newImagePath"></param>
        private void MusicImageOnOff(string newImagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            ImgMusic.Source = bitmapImage;
        }

        /// <summary>
        /// Afbeelding Sound On/Off
        /// </summary>
        /// <param name="newImagePath"></param>
        private void SoundImageOnOff(string newImagePath)
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
                    MusicImageOnOff(musicOffMediaPath);
                    ImgMusic.Tag = "Off";
                }
                else if (ImgMusic.Tag.ToString() == "Off")
                {
                    backgroundPlayer.Volume = 0.2;
                    MusicImageOnOff(musicOnMediaPath);
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
                    SoundImageOnOff(soundOffMediaPath);
                    ImgSound.Tag = "Off";
                    popPlayer.Volume = 0;
                    ImgCookie.Tag = "Off";
                }
                else if (ImgSound.Tag.ToString() == "Off")
                {
                    soundPlayer.Volume = 1;
                    SoundImageOnOff(soundOnMediaPath);
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

            if (cookieTotal >= basePriceCursor)
            {
                Btn_Cursor.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= basePriceGrandma)
            {
                Btn_Grandma.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= basePriceFarm)
            {
                Btn_Farm.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= basePriceMine)
            {
                Btn_Mine.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= basePriceFactory)
            {
                Btn_Factory.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= basePriceBank)
            {
                Btn_Bank.Visibility = Visibility.Visible;
            }
            if (cookieTotal >= basePriceTemple)
            {
                Btn_Temple.Visibility = Visibility.Visible;
            }
        }

        // Quests en PowerUps die de speler kan aankopen
        private Dictionary<string, string> quests = new Dictionary<string, string>();

        private readonly string behaaldeQuest1 = "1 cursor";
        private readonly string behaaldeQuest2 = "5 farms";
        private readonly string behaaldeQuest3 = "5 temples";
        private readonly string behaaldeQuest4 = "10 cursors";
        private readonly string behaaldeQuest5 = "10 mines";
        private readonly string behaaldeQuest6 = "10 banks";
        private readonly string behaaldeQuest7 = "20 grandmas";
        private readonly string behaaldeQuest8 = "20 farms";
        private readonly string behaaldeQuest9 = "25 factories";
        private readonly string behaaldeQuest10 = "30 tempels";
        private readonly string behaaldeQuest11 = "50 farms";
        private readonly string behaaldeQuest12 = "100 cursors";
        private readonly string behaaldeQuest13 = "100 grandmas";
        private readonly string behaaldeQuest14 = "100 banks";
        private readonly string behaaldeQuest15 = "1000 cookies";
        private readonly string behaaldeQuest16 = "1.000.000 cookies";
        private readonly string behaaldeQuest17 = "10.000.000 cookies";
        private readonly string behaaldeQuest18 = "Golden Cookie";
        private readonly string behaaldeQuest19 = "1 Perk";
        private readonly string behaaldeQuest20 = "10 Perks";
        private readonly string behaaldeQuest21 = "Quest Collector";

        private void Quests()
        {
            if (investmentCursorAmount == 1 && !LstBoxQuests.Items.Contains(behaaldeQuest1))
            {
                MessageBox.Show(quests["1 cursor"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest1);
            }
            if (investmentFarmAmount == 5 && !LstBoxQuests.Items.Contains(behaaldeQuest2))
            {
                MessageBox.Show(quests["5 farms"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest2);
            }
            if (investmentTempleAmount == 5 && !LstBoxQuests.Items.Contains(behaaldeQuest3))
            {
                MessageBox.Show(quests["5 temples"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest3);
            }
            if (investmentCursorAmount == 10 && !LstBoxQuests.Items.Contains(behaaldeQuest4))
            {
                MessageBox.Show(quests["10 cursors"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest4);
            }
            if (investmentMineAmount == 10 && !LstBoxQuests.Items.Contains(behaaldeQuest5))
            {
                MessageBox.Show(quests["10 mines"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest5);
            }
            if (investmentBankAmount == 10 && !LstBoxQuests.Items.Contains(behaaldeQuest6))
            {
                MessageBox.Show(quests["10 banks"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest6);
            }
            if (investmentGrandmaAmount == 20 && !LstBoxQuests.Items.Contains(behaaldeQuest7))
            {
                MessageBox.Show(quests["20 grandmas"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest7);
            }
            if (investmentFarmAmount == 20 && !LstBoxQuests.Items.Contains(behaaldeQuest8))
            {
                MessageBox.Show(quests["20 farms"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest8);
            }
            if (investmentFactoryAmount == 25 && !LstBoxQuests.Items.Contains(behaaldeQuest9))
            {
                MessageBox.Show(quests["25 factories"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest9);
            }
            if (investmentTempleAmount == 30 && !LstBoxQuests.Items.Contains(behaaldeQuest10))
            {
                MessageBox.Show(quests["30 tempels"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest10);
            }
            if (investmentFarmAmount == 50 && !LstBoxQuests.Items.Contains(behaaldeQuest11))
            {
                MessageBox.Show(quests["50 farms"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest11);
            }
            if (investmentCursorAmount == 100 && !LstBoxQuests.Items.Contains(behaaldeQuest12))
            {
                MessageBox.Show(quests["100 cursors"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest12);
            }
            if (investmentGrandmaAmount == 100 && !LstBoxQuests.Items.Contains(behaaldeQuest13))
            {
                MessageBox.Show(quests["100 grandmas"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest13);
            }
            if (investmentBankAmount == 100 && !LstBoxQuests.Items.Contains(behaaldeQuest14))
            {
                MessageBox.Show(quests["100 banks"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest14);
            }
            if (cookieCounter >= 1000 && !LstBoxQuests.Items.Contains(behaaldeQuest15))
            {
                MessageBox.Show(quests["1000 cookies"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest15);
            }
            if (cookieCounter >= 1000000 && !LstBoxQuests.Items.Contains(behaaldeQuest16))
            {
                MessageBox.Show(quests["1.000.000 cookies"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest16);
            }
            if (cookieCounter >= 10000000 && !LstBoxQuests.Items.Contains(behaaldeQuest17))
            {
                MessageBox.Show(quests["10.000.000 cookies"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest17);
            }
            if (goldenCookieClicked == true && !LstBoxQuests.Items.Contains(behaaldeQuest18))
            {
                MessageBox.Show(quests["Golden Cookie"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest18);
            }
            if (perkAmount == 1 && !LstBoxQuests.Items.Contains(behaaldeQuest19))
            {
                MessageBox.Show(quests["1 Perk"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest19);
            }
            if (perkAmount == 10 && !LstBoxQuests.Items.Contains(behaaldeQuest20))
            {
                MessageBox.Show(quests["10 Perks"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest20);
            }
            if (LstBoxQuests.Items.Count == 20 && !LstBoxQuests.Items.Contains(behaaldeQuest21))
            {
                MessageBox.Show(quests["Quest Collector"], "Quest Completed");
                LstBoxQuests.Items.Add(behaaldeQuest21);
            }
        }

        private void AddQuests()
        {
            quests.Add("1 cursor", "WOOOOO je eerste investering!");
            quests.Add("5 farms", "Al 5 boerderijen! Je koopt deze toch niet voor de boerendochter hé ;) ");
            quests.Add("5 temples", "5 temples, de goden krijgen niet genoeg van je speciale recept!");
            quests.Add("10 cursors", "Klik er maar op los cursors! KLIK ER MAAR OP LOS!");
            quests.Add("10 mines", "10 mines, tijd om sneeuwitje te bellen en de dwergen een part time job aan te bieden");
            quests.Add("10 banks", "10 banks, bringing in that dough!");
            quests.Add("20 grandmas", "20 grandma's die voor je werken, opa's hebben tijd voor een lemon party ;) ");
            quests.Add("20 farms", "20 farms, de koeien zijn blij met hun nieuwe stal");
            quests.Add("25 factories", "Wat een aankopen! 25 factories, daar is Wonka niks tegen!");
            quests.Add("30 tempels", "That's the Teen Spirit!");
            quests.Add("50 farms", "Old McDonald had A farm, niet 50!!");
            quests.Add("100 cursors", "LAN PARTY");
            quests.Add("100 grandmas", "Het rusthuis heeft gebeld, ze willen hun oma's terug!");
            quests.Add("100 banks", "The real Wolf of Sesamestreet");
            quests.Add("1000 cookies", "je eerste 1000 cookies zijn binnen, tijd om te investeren en je cookie emporium op te zetten!");
            quests.Add("1.000.000 cookies", "1.000.000 cookies, je bent een echte cookie monster!");
            quests.Add("10.000.000 cookies", "10.000.000 cookies :o keep going! Maar let wel op voor diabetes");
            quests.Add("Golden Cookie", "Een Golden Cookie, laat het smaken!");
            quests.Add("1 Perk", "Time to get Perkaholic!");
            quests.Add("10 Perks", "PERKAHOLIC BABY!!");
            quests.Add("Quest Collector", "Alle Quests behaald!");
        }

        private void LstBoxQuests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItems = LstBoxQuests.SelectedItem.ToString();

            TxtBoxQuests.Text = quests[selectedItems];
        }

        private void ImgScrollAndQuill_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgScrollAndQuill.Visibility = Visibility.Collapsed;
            LstBoxQuests.Visibility = Visibility.Visible;
            TxtBoxQuests.Visibility = Visibility.Visible;
        }

        private void LstBoxQuests_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgScrollAndQuill.Visibility = Visibility.Visible;
            LstBoxQuests.Visibility = Visibility.Collapsed;
            TxtBoxQuests.Visibility = Visibility.Collapsed;
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
                PlaySound(bonus);
            }
        }

        private bool goldenCookieClicked = false;

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
            ButtonVisibility();
            PerkButtonEnabled();
            ShopButtonEnable();
            PlaySound(succes);

            goldenCookieClicked = true;
            Quests();
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

        private double perkAmount = 0;                            // Variabele die aangeeft hoeveel perks er zijn aangekocht

        private double tierValue = new double();                // Variabele die aangeeft welke tier het is en de waarde van de tier
        private const double tierValue1 = 100;
        private const double tierValue2 = 500;
        private const double tierValue3 = 5000;
        private const double tierValue4 = 50000;
        private const double tierValue5 = 500000;

        private bool clickedOnCursorPerk1 = false;  // Boolean die aangeeft of er op de knop geklikt is, deze wordt gebruikt om de knoppen te enabelen of visibility te geven of niet
        private bool clickedOnCursorPerk2 = false;
        private bool clickedOnCursorPerk3 = false;
        private bool clickedOnCursorPerk4 = false;
        private bool clickedOnCursorPerk5 = false;
        private bool clickedOnGrandmaPerk1 = false;
        private bool clickedOnGrandmaPerk2 = false;
        private bool clickedOnGrandmaPerk3 = false;
        private bool clickedOnGrandmaPerk4 = false;
        private bool clickedOnGrandmaPerk5 = false;
        private bool clickedOnFarmPerk1 = false;
        private bool clickedOnFarmPerk2 = false;
        private bool clickedOnFarmPerk3 = false;
        private bool clickedOnFarmPerk4 = false;
        private bool clickedOnFarmPerk5 = false;
        private bool clickedOnMinePerk1 = false;
        private bool clickedOnMinePerk2 = false;
        private bool clickedOnMinePerk3 = false;
        private bool clickedOnMinePerk4 = false;
        private bool clickedOnMinePerk5 = false;
        private bool clickedOnFactoryPerk1 = false;
        private bool clickedOnFactoryPerk2 = false;
        private bool clickedOnFactoryPerk3 = false;
        private bool clickedOnFactoryPerk4 = false;
        private bool clickedOnFactoryPerk5 = false;
        private bool clickedOnBankPerk1 = false;
        private bool clickedOnBankPerk2 = false;
        private bool clickedOnBankPerk3 = false;
        private bool clickedOnBankPerk4 = false;
        private bool clickedOnBankPerk5 = false;
        private bool clickedOnTemplePerk1 = false;
        private bool clickedOnTemplePerk2 = false;
        private bool clickedOnTemplePerk3 = false;
        private bool clickedOnTemplePerk4 = false;
        private bool clickedOnTemplePerk5 = false;

        private void BuyPerk(ref double multiplier, ref double passive)
        {
            btnPerk.Visibility = Visibility.Collapsed;

            cookieCounter -= basePrice * tierValue;
            UpdateScore();

            passiveCounter += passive * investmentAmount;
            PassiveCounter();

            multiplier *= 2;
            passive *= 2;

            perkAmount++;
            Quests();

            PlaySound(ping);
        }

        private void BuyPerk(string button)
        {
            switch (button)
            {
                case "BtnPerkCursor1":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    break;

                case "BtnPerkCursor2":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    break;

                case "BtnPerkCursor3":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    break;

                case "BtnPerkCursor4":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    break;

                case "BtnPerkCursor5":
                    BuyPerk(ref cursorMultiplier, ref passiveCursor);
                    break;

                case "BtnPerkGrandma1":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    break;

                case "BtnPerkGrandma2":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    break;

                case "BtnPerkGrandma3":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    break;

                case "BtnPerkGrandma4":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    break;

                case "BtnPerkGrandma5":
                    BuyPerk(ref grandmaMultiplier, ref passiveGrandma);
                    break;

                case "BtnPerkFarm1":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    break;

                case "BtnPerkFarm2":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    break;

                case "BtnPerkFarm3":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    break;

                case "BtnPerkFarm4":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    break;

                case "BtnPerkFarm5":
                    BuyPerk(ref farmMultiplier, ref passiveFarm);
                    break;

                case "BtnPerkMine1":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    break;

                case "BtnPerkMine2":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    break;

                case "BtnPerkMine3":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    break;

                case "BtnPerkMine4":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    break;

                case "BtnPerkMine5":
                    BuyPerk(ref mineMultiplier, ref passiveMine);
                    break;

                case "BtnPerkFactory1":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    break;

                case "BtnPerkFactory2":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    break;

                case "BtnPerkFactory3":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    break;

                case "BtnPerkFactory4":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    break;

                case "BtnPerkFactory5":
                    BuyPerk(ref factoryMultiplier, ref passiveFactory);
                    break;

                case "BtnPerkBank1":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    break;

                case "BtnPerkBank2":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    break;

                case "BtnPerkBank3":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    break;

                case "BtnPerkBank4":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    break;

                case "BtnPerkBank5":
                    BuyPerk(ref bankMultiplier, ref passiveBank);
                    break;

                case "BtnPerkTemple1":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    break;

                case "BtnPerkTemple2":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    break;

                case "BtnPerkTemple3":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    break;

                case "BtnPerkTemple4":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    break;

                case "BtnPerkTemple5":
                    BuyPerk(ref templeMultiplier, ref passiveTemple);
                    break;
            }
        }

        private void PerkStore(string button)
        {
            switch (button)
            {
                case "BtnPerkCursor1":
                    basePrice = basePriceCursor;
                    btnPerk = BtnPerkCursor1;
                    tierValue = tierValue1;
                    investmentAmount = investmentCursorAmount;
                    clickedOnCursorPerk1 = true;
                    break;

                case "BtnPerkCursor2":
                    basePrice = basePriceCursor;
                    btnPerk = BtnPerkCursor2;
                    tierValue = tierValue2;
                    investmentAmount = investmentCursorAmount;
                    clickedOnCursorPerk2 = true;
                    break;

                case "BtnPerkCursor3":
                    basePrice = basePriceCursor;
                    btnPerk = BtnPerkCursor3;
                    tierValue = tierValue3;
                    investmentAmount = investmentCursorAmount;
                    clickedOnCursorPerk3 = true;
                    break;

                case "BtnPerkCursor4":
                    basePrice = basePriceCursor;
                    btnPerk = BtnPerkCursor4;
                    tierValue = tierValue4;
                    investmentAmount = investmentCursorAmount;
                    clickedOnCursorPerk4 = true;
                    break;

                case "BtnPerkCursor5":
                    basePrice = basePriceCursor;
                    btnPerk = BtnPerkCursor5;
                    tierValue = tierValue5;
                    investmentAmount = investmentCursorAmount;
                    clickedOnCursorPerk5 = true;
                    break;

                case "BtnPerkGrandma1":
                    basePrice = basePriceGrandma;
                    btnPerk = BtnPerkGrandma1;
                    tierValue = tierValue1;
                    investmentAmount = investmentGrandmaAmount;
                    clickedOnGrandmaPerk1 = true;
                    break;

                case "BtnPerkGrandma2":
                    basePrice = basePriceGrandma;
                    btnPerk = BtnPerkGrandma2;
                    tierValue = tierValue2;
                    investmentAmount = investmentGrandmaAmount;
                    clickedOnGrandmaPerk2 = true;
                    break;

                case "BtnPerkGrandma3":
                    basePrice = basePriceGrandma;
                    btnPerk = BtnPerkGrandma3;
                    tierValue = tierValue3;
                    investmentAmount = investmentGrandmaAmount;
                    clickedOnGrandmaPerk3 = true;
                    break;

                case "BtnPerkGrandma4":
                    basePrice = basePriceGrandma;
                    btnPerk = BtnPerkGrandma4;
                    tierValue = tierValue4;
                    investmentAmount = investmentGrandmaAmount;
                    clickedOnGrandmaPerk4 = true;
                    break;

                case "BtnPerkGrandma5":
                    basePrice = basePriceGrandma;
                    btnPerk = BtnPerkGrandma5;
                    tierValue = tierValue5;
                    investmentAmount = investmentGrandmaAmount;
                    clickedOnGrandmaPerk5 = true;
                    break;

                case "BtnPerkFarm1":
                    basePrice = basePriceFarm;
                    btnPerk = BtnPerkFarm1;
                    tierValue = tierValue1;
                    investmentAmount = investmentFarmAmount;
                    clickedOnFarmPerk1 = true;
                    break;

                case "BtnPerkFarm2":
                    basePrice = basePriceFarm;
                    btnPerk = BtnPerkFarm2;
                    tierValue = tierValue2;
                    investmentAmount = investmentFarmAmount;
                    clickedOnFarmPerk2 = true;
                    break;

                case "BtnPerkFarm3":
                    basePrice = basePriceFarm;
                    btnPerk = BtnPerkFarm3;
                    tierValue = tierValue3;
                    investmentAmount = investmentFarmAmount;
                    clickedOnFarmPerk3 = true;
                    break;

                case "BtnPerkFarm4":
                    basePrice = basePriceFarm;
                    btnPerk = BtnPerkFarm4;
                    tierValue = tierValue4;
                    investmentAmount = investmentFarmAmount;
                    clickedOnFarmPerk4 = true;
                    break;

                case "BtnPerkFarm5":
                    basePrice = basePriceFarm;
                    btnPerk = BtnPerkFarm5;
                    tierValue = tierValue5;
                    investmentAmount = investmentFarmAmount;
                    clickedOnFarmPerk5 = true;
                    break;

                case "BtnPerkMine1":
                    basePrice = basePriceMine;
                    btnPerk = BtnPerkMine1;
                    tierValue = tierValue1;
                    investmentAmount = investmentMineAmount;
                    clickedOnMinePerk1 = true;
                    break;

                case "BtnPerkMine2":
                    basePrice = basePriceMine;
                    btnPerk = BtnPerkMine2;
                    tierValue = tierValue2;
                    investmentAmount = investmentMineAmount;
                    clickedOnMinePerk2 = true;
                    break;

                case "BtnPerkMine3":
                    basePrice = basePriceMine;
                    btnPerk = BtnPerkMine3;
                    tierValue = tierValue3;
                    investmentAmount = investmentMineAmount;
                    clickedOnMinePerk3 = true;
                    break;

                case "BtnPerkMine4":
                    basePrice = basePriceMine;
                    btnPerk = BtnPerkMine4;
                    tierValue = tierValue4;
                    investmentAmount = investmentMineAmount;
                    clickedOnMinePerk4 = true;
                    break;

                case "BtnPerkMine5":
                    basePrice = basePriceMine;
                    btnPerk = BtnPerkMine5;
                    tierValue = tierValue5;
                    investmentAmount = investmentMineAmount;
                    clickedOnMinePerk5 = true;
                    break;

                case "BtnPerkFactory1":
                    basePrice = basePriceFactory;
                    btnPerk = BtnPerkFactory1;
                    tierValue = tierValue1;
                    investmentAmount = investmentFactoryAmount;
                    clickedOnFactoryPerk1 = true;
                    break;

                case "BtnPerkFactory2":
                    basePrice = basePriceFactory;
                    btnPerk = BtnPerkFactory2;
                    tierValue = tierValue2;
                    investmentAmount = investmentFactoryAmount;
                    clickedOnFactoryPerk2 = true;
                    break;

                case "BtnPerkFactory3":
                    basePrice = basePriceFactory;
                    btnPerk = BtnPerkFactory3;
                    tierValue = tierValue3;
                    investmentAmount = investmentFactoryAmount;
                    clickedOnFactoryPerk3 = true;
                    break;

                case "BtnPerkFactory4":
                    basePrice = basePriceFactory;
                    btnPerk = BtnPerkFactory4;
                    tierValue = tierValue4;
                    investmentAmount = investmentFactoryAmount;
                    clickedOnFactoryPerk4 = true;
                    break;

                case "BtnPerkFactory5":
                    basePrice = basePriceFactory;
                    btnPerk = BtnPerkFactory5;
                    tierValue = tierValue5;
                    investmentAmount = investmentFactoryAmount;
                    clickedOnFactoryPerk5 = true;
                    break;

                case "BtnPerkBank1":
                    basePrice = basePriceBank;
                    btnPerk = BtnPerkBank1;
                    tierValue = tierValue1;
                    investmentAmount = investmentBankAmount;
                    clickedOnBankPerk1 = true;
                    break;

                case "BtnPerkBank2":
                    basePrice = basePriceBank;
                    btnPerk = BtnPerkBank2;
                    tierValue = tierValue2;
                    investmentAmount = investmentBankAmount;
                    clickedOnBankPerk2 = true;
                    break;

                case "BtnPerkBank3":
                    basePrice = basePriceBank;
                    btnPerk = BtnPerkBank3;
                    tierValue = tierValue3;
                    investmentAmount = investmentBankAmount;
                    clickedOnBankPerk3 = true;
                    break;

                case "BtnPerkBank4":
                    basePrice = basePriceBank;
                    btnPerk = BtnPerkBank4;
                    tierValue = tierValue4;
                    investmentAmount = investmentBankAmount;
                    clickedOnBankPerk4 = true;
                    break;

                case "BtnPerkBank5":
                    basePrice = basePriceBank;
                    btnPerk = BtnPerkBank5;
                    tierValue = tierValue5;
                    investmentAmount = investmentBankAmount;
                    clickedOnBankPerk5 = true;
                    break;

                case "BtnPerkTemple1":
                    basePrice = basePriceTemple;
                    btnPerk = BtnPerkTemple1;
                    tierValue = tierValue1;
                    investmentAmount = investmentTempleAmount;
                    clickedOnTemplePerk1 = true;
                    break;

                case "BtnPerkTemple2":
                    basePrice = basePriceTemple;
                    btnPerk = BtnPerkTemple2;
                    tierValue = tierValue2;
                    investmentAmount = investmentTempleAmount;
                    clickedOnTemplePerk2 = true;
                    break;

                case "BtnPerkTemple3":
                    basePrice = basePriceTemple;
                    btnPerk = BtnPerkTemple3;
                    tierValue = tierValue3;
                    investmentAmount = investmentTempleAmount;
                    clickedOnTemplePerk3 = true;
                    break;

                case "BtnPerkTemple4":
                    basePrice = basePriceTemple;
                    btnPerk = BtnPerkTemple4;
                    tierValue = tierValue4;
                    investmentAmount = investmentTempleAmount;
                    clickedOnTemplePerk4 = true;
                    break;

                case "BtnPerkTemple5":
                    basePrice = basePriceTemple;
                    btnPerk = BtnPerkTemple5;
                    tierValue = tierValue5;
                    investmentAmount = investmentTempleAmount;
                    clickedOnTemplePerk5 = true;
                    break;
            }
        }

        /// <summary>
        /// Click handler om Perk Upgrades aan te kopen door speler om de passieve inkomen te verhogen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPerk_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;

            PerkStore(buttonName);
            BuyPerk(buttonName);
        }

        private readonly int tier1 = 1;
        private readonly int tier2 = 5;
        private readonly int tier3 = 25;
        private readonly int tier4 = 50;
        private readonly int tier5 = 100;

        private void PerkVisibility()
        {
            if (investmentCursorAmount >= tier1 && !clickedOnCursorPerk1)
            {
                BtnPerkCursor1.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentCursorAmount >= tier2 && !clickedOnCursorPerk2)
            {
                BtnPerkCursor2.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentCursorAmount >= tier3 && !clickedOnCursorPerk3)
            {
                BtnPerkCursor3.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentCursorAmount >= tier4 && !clickedOnCursorPerk4)
            {
                BtnPerkCursor4.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentCursorAmount >= tier5 && !clickedOnCursorPerk5)
            {
                BtnPerkCursor5.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentGrandmaAmount >= tier1 && !clickedOnGrandmaPerk1)
            {
                BtnPerkGrandma1.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentGrandmaAmount >= tier2 && !clickedOnGrandmaPerk2)
            {
                BtnPerkGrandma2.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentGrandmaAmount >= tier3 && !clickedOnGrandmaPerk3)
            {
                BtnPerkGrandma3.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentGrandmaAmount >= tier4 && !clickedOnGrandmaPerk4)
            {
                BtnPerkGrandma4.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentGrandmaAmount >= tier5 && !clickedOnGrandmaPerk5)
            {
                BtnPerkGrandma5.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFarmAmount >= tier1 && !clickedOnFarmPerk1)
            {
                BtnPerkFarm1.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFarmAmount >= tier2 && !clickedOnFarmPerk2)
            {
                BtnPerkFarm2.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFarmAmount >= tier3 && !clickedOnFarmPerk3)
            {
                BtnPerkFarm3.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFarmAmount >= tier4 && !clickedOnFarmPerk4)
            {
                BtnPerkFarm4.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFarmAmount >= tier5 && !clickedOnFarmPerk5)
            {
                BtnPerkFarm5.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentMineAmount >= tier1 && !clickedOnMinePerk1)
            {
                BtnPerkMine1.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentMineAmount >= tier2 && !clickedOnMinePerk2)
            {
                BtnPerkMine2.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentMineAmount >= tier3 && !clickedOnMinePerk3)
            {
                BtnPerkMine3.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentMineAmount >= tier4 && !clickedOnMinePerk4)
            {
                BtnPerkMine4.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentMineAmount >= tier5 && !clickedOnMinePerk5)
            {
                BtnPerkMine5.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFactoryAmount >= tier1 && !clickedOnFactoryPerk1)
            {
                BtnPerkFactory1.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFactoryAmount >= tier2 && !clickedOnFactoryPerk2)
            {
                BtnPerkFactory2.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFactoryAmount >= tier3 && !clickedOnFactoryPerk3)
            {
                BtnPerkFactory3.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFactoryAmount >= tier4 && !clickedOnFactoryPerk4)
            {
                BtnPerkFactory4.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentFactoryAmount >= tier5 && !clickedOnFactoryPerk5)
            {
                BtnPerkFactory5.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentBankAmount >= tier1 && !clickedOnBankPerk1)
            {
                BtnPerkBank1.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentBankAmount >= tier2 && !clickedOnBankPerk2)
            {
                BtnPerkBank2.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentBankAmount >= tier3 && !clickedOnBankPerk3)
            {
                BtnPerkBank3.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentBankAmount >= tier4 && !clickedOnBankPerk4)
            {
                BtnPerkBank4.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentBankAmount >= tier5 && !clickedOnBankPerk5)
            {
                BtnPerkBank5.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentTempleAmount >= tier1 && !clickedOnTemplePerk1)
            {
                BtnPerkTemple1.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentTempleAmount >= tier2 && !clickedOnTemplePerk2)
            {
                BtnPerkTemple2.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentTempleAmount >= tier3 && !clickedOnTemplePerk3)
            {
                BtnPerkTemple3.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentTempleAmount >= tier4 && !clickedOnTemplePerk4)
            {
                BtnPerkTemple4.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
            if (investmentTempleAmount >= tier5 && !clickedOnTemplePerk5)
            {
                BtnPerkTemple5.Visibility = Visibility.Visible;
                LblPerks.Visibility = Visibility.Visible;
            }
        }

        private void PerkButtonEnabled()
        {
            BtnPerkCursor1.IsEnabled = (cookieCounter >= basePriceCursor * tierValue1);
            BtnPerkCursor2.IsEnabled = (cookieCounter >= basePriceCursor * tierValue2);
            BtnPerkCursor3.IsEnabled = (cookieCounter >= basePriceCursor * tierValue3);
            BtnPerkCursor4.IsEnabled = (cookieCounter >= basePriceCursor * tierValue4);
            BtnPerkCursor5.IsEnabled = (cookieCounter >= basePriceCursor * tierValue5);
            BtnPerkGrandma1.IsEnabled = (cookieCounter >= basePriceGrandma * tierValue1);
            BtnPerkGrandma2.IsEnabled = (cookieCounter >= basePriceGrandma * tierValue2);
            BtnPerkGrandma3.IsEnabled = (cookieCounter >= basePriceGrandma * tierValue3);
            BtnPerkGrandma4.IsEnabled = (cookieCounter >= basePriceGrandma * tierValue4);
            BtnPerkGrandma5.IsEnabled = (cookieCounter >= basePriceGrandma * tierValue5);
            BtnPerkFarm1.IsEnabled = (cookieCounter >= basePriceFarm * tierValue1);
            BtnPerkFarm2.IsEnabled = (cookieCounter >= basePriceFarm * tierValue2);
            BtnPerkFarm3.IsEnabled = (cookieCounter >= basePriceFarm * tierValue3);
            BtnPerkFarm4.IsEnabled = (cookieCounter >= basePriceFarm * tierValue4);
            BtnPerkFarm5.IsEnabled = (cookieCounter >= basePriceFarm * tierValue5);
            BtnPerkMine1.IsEnabled = (cookieCounter >= basePriceMine * tierValue1);
            BtnPerkMine2.IsEnabled = (cookieCounter >= basePriceMine * tierValue2);
            BtnPerkMine3.IsEnabled = (cookieCounter >= basePriceMine * tierValue3);
            BtnPerkMine4.IsEnabled = (cookieCounter >= basePriceMine * tierValue4);
            BtnPerkMine5.IsEnabled = (cookieCounter >= basePriceMine * tierValue5);
            BtnPerkFactory1.IsEnabled = (cookieCounter >= basePriceFactory * tierValue1);
            BtnPerkFactory2.IsEnabled = (cookieCounter >= basePriceFactory * tierValue2);
            BtnPerkFactory3.IsEnabled = (cookieCounter >= basePriceFactory * tierValue3);
            BtnPerkFactory4.IsEnabled = (cookieCounter >= basePriceFactory * tierValue4);
            BtnPerkFactory5.IsEnabled = (cookieCounter >= basePriceFactory * tierValue5);
            BtnPerkBank1.IsEnabled = (cookieCounter >= basePriceBank * tierValue1);
            BtnPerkBank2.IsEnabled = (cookieCounter >= basePriceBank * tierValue2);
            BtnPerkBank3.IsEnabled = (cookieCounter >= basePriceBank * tierValue3);
            BtnPerkBank4.IsEnabled = (cookieCounter >= basePriceBank * tierValue4);
            BtnPerkBank5.IsEnabled = (cookieCounter >= basePriceBank * tierValue5);
            BtnPerkTemple1.IsEnabled = (cookieCounter >= basePriceTemple * tierValue1);
            BtnPerkTemple2.IsEnabled = (cookieCounter >= basePriceTemple * tierValue2);
            BtnPerkTemple3.IsEnabled = (cookieCounter >= basePriceTemple * tierValue3);
            BtnPerkTemple4.IsEnabled = (cookieCounter >= basePriceTemple * tierValue4);
            BtnPerkTemple5.IsEnabled = (cookieCounter >= basePriceTemple * tierValue5);
        }
    }
}

