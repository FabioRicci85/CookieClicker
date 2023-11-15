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

        private bool isMousePressed = false;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImgCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMousePressed = true;            
        }

        private void ImgCookie_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMousePressed = false;
        }

      
              
    }
}
