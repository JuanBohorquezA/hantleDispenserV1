using hantleDispenser.Domain;
using hantleDispenser.Domain.Models;
using hantleDispenser.UserControls;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace hantleDispenser.UserControls
{
    /// <summary>
    /// Lógica de interacción para LoginUC.xaml
    /// </summary>
    public partial class LoginUC : AppUserControl
    {
        private string _pwd = string.Empty;
        public LoginUC()
        {
            InitializeComponent();
            this.Loaded += Onloaded;
        }
        public void Onloaded(object sender, RoutedEventArgs e)
        {
            this.Resources["IconEye"] = PackIconKind.None;
        }
        private bool Login(string userName, string pwd)
        {
            if (UsernameBox.Text.Length == 0) return false;
            if (PasswordBox.Password.Length == 0) return false;

            var dataEncrypted = Encrypt(userName, pwd);
            var Users = JsonConvert.DeserializeObject<List<User>>(Persistence.USERS);

            if (Users == null)
                return false;

            foreach (var User in Users)
            {
                if (User.UserName == dataEncrypted[0] && User.pwd == dataEncrypted[1])
                {
                    User.Name = userName;
                    SessionManager.InicializeSession(User);
                    return true;
                }

            }
            return false;
        }
        private List<string> Encrypt(string userName, string pwd)
        {
            byte[] userNameBytes = Encoding.UTF8.GetBytes(userName);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);
            using (SHA256 sha256Hash = SHA256.Create())
                return new List<string> { Convert.ToBase64String(sha256Hash.ComputeHash(userNameBytes)), Convert.ToBase64String(sha256Hash.ComputeHash(pwdBytes)) };
        }
        private void ButtonShowPWD(object sender, MouseButtonEventArgs e)
        {
            if (this.Resources["IconEye"].Equals(PackIconKind.Eye))
            {
                this.Resources["IconEye"] = PackIconKind.EyeOff;
                TextPasswordBox.Text = PasswordBox.Password;
                TextPasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                _pwd = TextPasswordBox.Text;
            }
            else
            {
                this.Resources["IconEye"] = PackIconKind.Eye;
                PasswordBox.Password = TextPasswordBox.Text;
                TextPasswordBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                _pwd = PasswordBox.Password;
            }
            

        }
        private void ShowMessageError(string message)
        {
            txtMessage.Text = message;
            BorderUsername.BorderBrush = (Brush)Application.Current.Resources["ERRORCOLOR"];
            BorderPassword.BorderBrush = (Brush)Application.Current.Resources["ERRORCOLOR"];
        }
        private void InitValues()
        {
            txtMessage.Text = string.Empty;
            PasswordBox.Visibility = Visibility.Visible;
            TextPasswordBox.Visibility = Visibility.Collapsed;
            BorderUsername.BorderBrush = (Brush)Application.Current.Resources["SECONDARYCOLOR"];
            BorderPassword.BorderBrush = (Brush)Application.Current.Resources["SECONDARYCOLOR"];
        }
        private void ButtonLogin(object sender, MouseButtonEventArgs e)
        {
            InitValues();

            if (string.IsNullOrEmpty(UsernameBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            { 
                ShowMessageError("COMPLETE TODOS LOS CAMPOS");
                return;
            }
            _pwd = this.Resources["IconEye"].Equals(PackIconKind.Eye) ? PasswordBox.Password : TextPasswordBox.Text;

            if (!Login(UsernameBox.Text, _pwd))
            {
                ShowMessageError("USUARIO Y/O CONTRASEÑA INCORRECTOS");
                UsernameBox.Text = string.Empty;
                PasswordBox.Password = string.Empty;
                return;
            }
            Dispatcher.Invoke(() => { Goto(new ConfigUC()); });
        }
        
        #region Inputs
        private void OnGotFocus(object sender, EventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null || !(Element.Parent is Grid parentGrid)) return;
            var TextBlock = parentGrid?.Children.OfType<TextBlock>().FirstOrDefault() ?? new();
            TextBlock.Visibility = Visibility.Collapsed;

        }
        private void OnLostFocus(object sender, EventArgs e)
        {

            var Element = sender as FrameworkElement;
            if (Element == null || !(Element.Parent is Grid parentGrid)) return;
            var TextBlock = parentGrid?.Children.OfType<TextBlock>().FirstOrDefault() ?? new();
            if (Element is TextBox textBox && !string.IsNullOrEmpty(textBox.Text) || Element is PasswordBox pwdBox && !string.IsNullOrEmpty(pwdBox.Password)) return;  
            TextBlock.Visibility = Visibility.Visible;

        }
        private void Input_changed(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var placeHolder = textBox.Tag as TextBlock;
            if (placeHolder == null) return;

            placeHolder.Visibility = string.IsNullOrEmpty(textBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void Password_changed(object sender, EventArgs e)
        {
            var PasswordBox = sender as PasswordBox;
            if (PasswordBox == null) return;

            var placeHolder = PasswordBox.Tag as TextBlock;
            if (placeHolder == null) return;

          
            if (!string.IsNullOrEmpty(PasswordBox.Password))
            {
                placeHolder.Visibility = Visibility.Collapsed;
                this.Resources["IconEye"] = PackIconKind.Eye;
                return;
            }
            this.Resources["IconEye"] = PackIconKind.None;
            placeHolder.Visibility = Visibility.Visible;


        }
        #endregion

    }
}
