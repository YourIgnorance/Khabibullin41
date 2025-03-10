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

namespace Khabibullin41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private string _captcha;
        private string _ValidLitters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwyz1234567890";
        private bool _isCaptched = true;
        public AuthPage()
        {
            InitializeComponent();

            TBoxLogin.Text = "DEftn2018";
            TBoxPassword.Text = "gPq+a}";

            TBCaptcha.Visibility = Visibility.Hidden;
            TBlockCaptcha.Visibility = Visibility.Hidden;
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage());

            TBoxLogin.Text = "";
            TBoxPassword.Text = "";
            _isCaptched = true;
            TBCaptcha.Text = "";
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = TBoxLogin.Text;
            string password = TBoxPassword.Text;
           
            if(TBCaptcha.Text == _captcha)
                _isCaptched = true;

            if (!_isCaptched)
            {
                MessageBox.Show("Каптча введена неверно!");
                CaptchaEnable();
                LoginButton.IsEnabled = false;
                await Task.Delay(10000);
                LoginButton.IsEnabled = true;
                return;
            }
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                CaptchaEnable();
                return;
            }

            User user = Khabibullin41Entities.getInstance().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                TBoxLogin.Text = "";
                TBoxPassword.Text = "";
                CaptchaDisable();
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                if (TBCaptcha.IsVisible)
                {
                    LoginButton.IsEnabled = false;
                    await Task.Delay(10000);
                    LoginButton.IsEnabled = true;
                }
                CaptchaEnable();
            }
        }
        private void CaptchaEnable()
        {
            _isCaptched = false;
            TBCaptcha.Visibility = Visibility.Visible;
            TBlockCaptcha.Visibility = Visibility.Visible;

            
            Random random = new Random();

            capthaOneWord.Text = Convert.ToString(_ValidLitters[random.Next(_ValidLitters.Length)]);
            capthaTwoWord.Text = Convert.ToString(_ValidLitters[random.Next(_ValidLitters.Length)]);
            capthaThreeWord.Text = Convert.ToString(_ValidLitters[random.Next(_ValidLitters.Length)]);
            capthaFourWord.Text = Convert.ToString(_ValidLitters[random.Next(_ValidLitters.Length)]);


            capthaOneWord.TextDecorations = TextDecorations.Strikethrough;


            _captcha = capthaOneWord.Text + capthaTwoWord.Text + capthaThreeWord.Text + capthaFourWord.Text;

        }
        private void CaptchaDisable()
        {
            _isCaptched = true;
            TBCaptcha.Visibility = Visibility.Hidden;
            TBlockCaptcha.Visibility = Visibility.Hidden;
            capthaOneWord.Text = "";
            capthaTwoWord.Text = "";
            capthaThreeWord.Text = "";
            capthaFourWord.Text = "";
            TBCaptcha.Text = "";
        }
    }
}
