using hantleDispenser.Domain;
using hantleDispenser.UserControls;
using System;
using System.Windows;

namespace hantleDispenser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _minDimension { set; get; }
        private double _maxDimension { set; get; }
        private Navigator _navigatorSingleton { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _navigatorSingleton = Navigator.Instance;
            _navigatorSingleton.Init(this);

            this.Height = SystemParameters.WorkArea.Height;
            this.Width = SystemParameters.WorkArea.Width;

            this.Loaded += Onloaded;

        }
        public void Onloaded(object sender, RoutedEventArgs e)
        {
            SetDimensions();
            SetDynamicResource();
            _navigatorSingleton.NavigateTo(new HomeUC());
        }

        public void SetDimensions()
        {

            MainContainer.Width = this.Width;
            MainContainer.Height = this.Height;


            if (this.Height > this.Width)
            {
                _maxDimension = this.Height;
                _minDimension = this.Width;
            }
            else
            {
                _maxDimension = this.Width;
                _minDimension = this.Height;
            }

        }
        public void SetDynamicResource()
        {
            //Creamos los Recursos dinamicos que usaremos en toda la aplicacion
            this.Resources["ButtonWidth"] = (double)Math.Round(_maxDimension * 0.4);
            this.Resources["MButtonWidth"] = (double)Math.Round(_maxDimension * 0.2);
            this.Resources["ButtonHeight"] = (double)Math.Round(_minDimension * 0.15);
            this.Resources["MButtonHeight"] = (double)Math.Round(_minDimension * 0.075);
            this.Resources["CommonFontSize"] = (double)Math.Round(_minDimension * 0.034);
            this.Resources["MininFontSize"] = (double)Math.Round(_minDimension * 0.025);
            this.Resources["RowDefinitionHeight"] = new GridLength((double)Math.Round(_minDimension * 0.24), GridUnitType.Pixel);
            this.Resources["MarginGrid"] = new Thickness(Math.Round(_minDimension * 0.02));
            this.Resources["MinMarginGrid"] = new Thickness(Math.Round(_minDimension * 0.01));
            this.Resources["IconSize"] = (double)Math.Round(_minDimension * 0.0497);
            this.Resources["ButtonCornerRadius"] = new CornerRadius((double)Math.Round(_minDimension * 0.0497));
            this.Resources["MidMarginGrid"] = new Thickness(Math.Round(_minDimension * 0.298), Math.Round(_minDimension * 0.010), Math.Round(_minDimension * 0.298), Math.Round(_minDimension * 0.010));
            this.Resources["MinCornersRadius"] = new CornerRadius (Math.Round(_minDimension * 0.005));



            //definiciones para el ancho de los botones de Menu
            this.Resources["ButtonWidthMenu"] = (double)Math.Round(_maxDimension * 0.2);
            this.Resources["ButtonHeightMenu"] = (double)Math.Round(_maxDimension * 0.05);

            this.Resources["MinButtonWidth"] = (double)Math.Round(_minDimension * 0.24);
            this.Resources["MidButtonWidth"] = (double)Math.Round(_minDimension * 0.44);
            this.Resources["MinButtonHeight"] = (double)Math.Round(_minDimension * 0.05);
            this.Resources["ButtonDimentions"] = (double)Math.Round(_minDimension * 0.08);

        }

    }
}
