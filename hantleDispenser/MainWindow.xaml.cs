using hantleDispenser.Domain;
using hantleDispenser.Domain.UIDictionaries.DinamycResources;
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
            this.Resources.AddDinamycResources(_maxDimension, _minDimension);
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

    }
}
