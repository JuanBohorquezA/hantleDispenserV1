using hantleDispenser.Domain;
using hantleDispenser.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace hantleDispenser.Modals
{
    /// <summary>
    /// Lógica de interacción para ModalWindow.xaml
    /// </summary>
    public partial class ModalResponse : Window
    {
        private ModalResponseModel _viewModel;
        private double _minDimension { set; get; }
        private double _maxDimension { set; get; }
        public ModalResponse()
        {
            InitializeComponent();

            this.Height = SystemParameters.WorkArea.Height;
            this.Width = SystemParameters.WorkArea.Width;
            _viewModel = new (); 
            this.Loaded += Onloaded;
            this.DataContext = _viewModel;


        }
        public void Onloaded(object sender, RoutedEventArgs e)
        {
            SetDimensions();
            SetDynamicResource();
            ConfigureModal();
        }

        public void SetDimensions()
        { 
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
            this.Resources["ButtonHeight"] = (double)Math.Round(_minDimension * 0.15);
            this.Resources["CommonFontSize"] = (double)Math.Round(_minDimension * 0.034);
            this.Resources["RowDefinitionHeight"] = new GridLength((double)Math.Round(_minDimension * 0.24), GridUnitType.Pixel);
            this.Resources["MarginGrid"] = new Thickness(Math.Round(_minDimension * 0.02));


            this.Resources["MinButtonWidth"] = (double)Math.Round(_minDimension * 0.24);
            this.Resources["MidButtonWidth"] = (double)Math.Round(_minDimension * 0.44);

            this.Resources["MinButtonHeight"] = (double)Math.Round(_minDimension * 0.05);
            this.Resources["GifDimensions"] = (double)Math.Round(_minDimension * 0.19);

        }
        private void ConfigureModal()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var response in SessionManager.HantleResponse)
            {
                string jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
                stringBuilder.AppendLine(jsonResponse);
            }
            _viewModel.Response = stringBuilder.ToString();
        }

        private void BtnAcept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }

    }
    public class ModalResponseModel : INotifyPropertyChanged
    {

        private string _response = string.Empty;

        public string Response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;
                OnPropertyRaised(nameof(Response));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        }
    }



    
}
