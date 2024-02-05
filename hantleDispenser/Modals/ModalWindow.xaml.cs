using hantleDispenser.UserControls;
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
    public partial class ModalWindow : Window
    {
        private ModalViewModel _viewModel;
        private double _minDimension { set; get; }
        private double _maxDimension { set; get; }
        public ModalWindow(ModalViewModel viewModel)
        {
            InitializeComponent();

            this.Height = SystemParameters.WorkArea.Height;
            this.Width = SystemParameters.WorkArea.Width;

            this.Loaded += Onloaded;

            _viewModel = viewModel;
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

             //definiciones para el ancho de los botones 
            this.Resources["ButtonWidthMenu"] = (double)Math.Round(_maxDimension * 0.2);
            this.Resources["ButtonHeightMenu"] = (double)Math.Round(_maxDimension * 0.05);

            this.Resources["GifDimensions"] = (double)Math.Round(_minDimension * 0.19);
        }
        private void ConfigureModal()
        {
            this.BtnAcept.Visibility = _viewModel.TypeModal.BtnOkVisibility;
            this.BtnConfirm.Visibility = _viewModel.TypeModal.BtnYesVisibility;
            this.BtnCancel.Visibility = _viewModel.TypeModal.BtnNoVisibility;
            this.LoadGif.Visibility = _viewModel.TypeModal.LoadGifVisibility;
        }

        private void BtnAcept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnConfirm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close ();
        }

        private void BtnCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
    public class ModalViewModel : INotifyPropertyChanged
    {
        private string _message = string.Empty;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyRaised(nameof(Message));
            }
        }



        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyRaised(nameof(Title));
            }
        }

        private ModalType _typeModal;

        public ModalType TypeModal
        {
            get
            {
                return _typeModal;
            }
            set
            {
                _typeModal = value;
                OnPropertyRaised(nameof(TypeModal));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        }
    }

    public class ModalType
    {

        public Visibility BtnOkVisibility { get; set; }
        public Visibility BtnYesVisibility { get; set; }
        public Visibility BtnNoVisibility { get; set; }
        public Visibility LoadGifVisibility { get; set; }
    }

    public class InfoModal : ModalType
    {
        public InfoModal()
        {
            BtnOkVisibility = Visibility.Visible;
            BtnYesVisibility = Visibility.Collapsed;
            BtnNoVisibility = Visibility.Collapsed;
            LoadGifVisibility = Visibility.Collapsed;

        }

    }

    public class LoadModal : ModalType
    {
        public LoadModal()
        {
            BtnOkVisibility = Visibility.Collapsed;
            BtnYesVisibility = Visibility.Collapsed;
            BtnNoVisibility = Visibility.Collapsed;
            LoadGifVisibility = Visibility.Visible;

        }
    }

    public class ConfirmationModal : ModalType
    {
        public ConfirmationModal()
        {
            BtnOkVisibility = Visibility.Collapsed;
            BtnYesVisibility = Visibility.Visible;
            BtnNoVisibility = Visibility.Visible;
            LoadGifVisibility = Visibility.Collapsed;

        }
    }
}
