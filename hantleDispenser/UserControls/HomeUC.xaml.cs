using hantleDispenser.Domain;
using hantleDispenser.Modals;
using hantleDispenser.UserControls;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

namespace hantleDispenser.UserControls
{
    /// <summary>
    /// Lógica de interacción para HomeUC.xaml
    /// </summary>
    public partial class HomeUC : AppUserControl
    {
        public HomeUC()
        {
            InitializeComponent();
            this.Loaded += Onloaded;

        }
        private void Onloaded(object sender, RoutedEventArgs e)
        {
            CopyRight.Text = Persistence.CopyRight;
        }
        public void BtnContinue(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { Goto(new LoginUC()); });
        }
    }
}
