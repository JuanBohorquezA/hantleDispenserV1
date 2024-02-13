using hantleDispenser.Domain;
using hantleDispenser.Domain.Models;
using hantleDispenser.Modals;
using HantleDispenserAPI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using handlerDispenser.Domain;
using System;
using System.ComponentModel;

namespace hantleDispenser.UserControls
{
    /// <summary>
    /// Lógica de interacción para ConfigUC.xaml
    /// </summary>
    public partial class ConfigUC : AppUserControl
    {
        private ConfigViewModel _viewModel;
        private ModalWindow? _loadModal;
        public ConfigUC()
        {
            InitializeComponent();
            _viewModel = new();
            this.DataContext = _viewModel;
            this.Loaded += Onloaded;

        }
        private void Onloaded(object sender, EventArgs e)
        {
            GetElements();
            COMS.ItemsSource = _viewModel.COMLIST;
            Denominations.ItemsSource = _viewModel.DENOMINATIONLIST;
        }
        private bool ItemSelected(ListBox listBox)
        {
            var item = listBox.SelectedItem;
            if(item == null)
                return false;

            return true;
        }
        private void GetElements()
        {
            _viewModel.COMLIST = Persistence.COM_LIST;
            _viewModel.DENOMINATIONLIST = Persistence.DENOMINATIONS_LIST;
        }

        private void BtnRefresh(object sender, MouseButtonEventArgs e)
        {
            GetElements();
            COMS.ItemsSource = _viewModel.COMLIST;
            Denominations.ItemsSource = _viewModel.DENOMINATIONLIST;
        }
        private void  SetDenominations()
        {

                var COMSelected = COMS.SelectedItem.ToString() ?? string.Empty;

                var DenominationSelected = Denominations.SelectedItem.ToString()?.Trim().Replace("$", "").Replace(",", "").Replace(" ", "") ?? string.Empty;

                SessionManager.InicializeHantleSeccion(new DenominationsQuantities { COM = COMSelected, Denomination = DenominationSelected.Split("-").ToList() });

                CDMS_Handler.Portname = COMSelected;
                CDMS_Handler.Denominations = DenominationSelected;

        }
        private  bool IsValid(List<CDMS_Response> response) 
        {
            if (response.Count <= 0) return false;
            if (response[0].isSuccess) return true;
            if (response[0].UIResponse.Contains(HandlerResponse.ErrorInitialize.Split(":")[0])) return true;

            txtMessage.Text = response[0].UIResponse;
            return false;
        }

        private async void BtnContinue(object sender, MouseButtonEventArgs e)
        {
            
            txtMessage.Text = string.Empty;
            if (!ItemSelected(COMS))
            {
                txtMessage.Text = "SELECCIONE UN PUERTO COM PARA CONTINUAR";
                return;
            }
            if (!ItemSelected(Denominations))
            {
                txtMessage.Text = "SELECCIONE UNA DENOMINACIÓN PARA CONTINUAR";
                return;
            }
            
            SetDenominations();

            _loadModal = _nav.ShowLoadModal("Realizando la configuración...");

            var inicializeResponse = await OperationManager.GoInitialize();
            _nav.CloseLoadModal(ref _loadModal);
            if (!IsValid(inicializeResponse)) return;
            

            Dispatcher.Invoke(() => { Goto(new MenuUC()); });

        }

    }
    public class ConfigViewModel : INotifyPropertyChanged
    {
        private List<string>? _COMLIST;
        public List<string>? COMLIST
        {
            get
            {
                return _COMLIST;
            }
            set
            {
                _COMLIST = value;
                OnPropertyRaised(nameof(COMLIST));
            }
        }

        private List<string>? _DENOMINATIONLIST;
        public List<string>? DENOMINATIONLIST
        {
            get
            {
                return _DENOMINATIONLIST;
            }
            set
            {
                _DENOMINATIONLIST = value;
                OnPropertyRaised(nameof(DENOMINATIONLIST));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        }
    }
}
