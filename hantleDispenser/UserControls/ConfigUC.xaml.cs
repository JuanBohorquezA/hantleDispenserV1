using hantleDispenser.Domain;
using hantleDispenser.Domain.Models;
using hantleDispenser.Modals;
using HantleDispenserAPI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace hantleDispenser.UserControls
{
    /// <summary>
    /// Lógica de interacción para ConfigUC.xaml
    /// </summary>
    public partial class ConfigUC : AppUserControl
    {
        private ModalWindow? _loadModal;
        public List<string> COMLIST { get; }
        public List<string> DENOMINATIONSLIST { get; }


        public ConfigUC()
        {
            InitializeComponent();
            COMLIST = Persistence.COM_LIST;
            DENOMINATIONSLIST = Persistence.DENOMINATIONS_LIST;
            DataContext = this;

        }
        private bool ItemSelected(ListBox listBox)
        {
            var item = listBox.SelectedItem;
            if(item == null)
                return false;

            return true;
        }
        private void  SetDenominations()
        {

                var COMSelected = COMS.SelectedItem.ToString() ?? string.Empty;

                var DenominationSelected = Denominations.SelectedItem.ToString()?.Trim().Replace("$", "").Replace(",", "").Replace(" ", "") ?? string.Empty;

                SessionManager.InicializeHantleSeccion(new DenominationsQuantities { COM = COMSelected, Denomination = DenominationSelected.Split("-").ToList() });

                CDMS_Handler.Portname = COMSelected;
                CDMS_Handler.Denominations = DenominationSelected;

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
            if (!inicializeResponse[0].isSuccess)
            {
                txtMessage.Text = inicializeResponse[0].UIResponse;
                return;
            }
            Dispatcher.Invoke(() => { Goto(new MenuUC()); });

        }

    }
}
