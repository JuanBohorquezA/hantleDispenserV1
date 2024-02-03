using hantleDispenser.Domain;
using hantleDispenser.Modals;
using hantleDispenser.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace hantleDispenser.UserControls
{
    /// <summary>
    /// Lógica de interacción para Dispende.xaml
    /// </summary>
    public partial class DispendUC : AppUserControl
    {
        private ModalWindow? _loadModal;
        public DispendUC()
        {
            InitializeComponent();
        }
        public void BtnWrite(object sender, EventArgs e)
        {
            var value = ((Border)sender).Tag;
            if (value == null) return;
            if (inputDispendValue.Text.Length < 10) inputDispendValue.Text += value;

        }
        public void OnTextChange(object sender, EventArgs e)
        {
            if (inputDispendValue.Text.Length > 0) txtInputValue.Visibility = Visibility.Collapsed;
            else txtInputValue.Visibility = Visibility.Visible;
        }
        public void BtnErasing(object sender, EventArgs e)
        {
            inputDispendValue.Text = inputDispendValue.Text.Length > 0 ? inputDispendValue.Text.Substring(0, inputDispendValue.Text.Length - 1) : inputDispendValue.Text;
        }
        public void BtnClear(object sender, EventArgs e)
        {
            inputDispendValue.Text = inputDispendValue.Text.Length > 0 ? inputDispendValue.Text = string.Empty : inputDispendValue.Text;
        }
        public async void BtnContinue(object sender, MouseButtonEventArgs e)
        {
            txtMessage.Visibility = Visibility.Collapsed;

            if (inputDispendValue.Text.Length <= 0)
            {
                txtMessage.Visibility = Visibility.Visible;
                return;
            }

            if (!_nav.ShowModal("¿Estas seguro de realizar esta acción? es completamente irreversible...", new ConfirmationModal()))
            {
                Dispatcher.Invoke(() => { Goto(new MenuUC()); }); ;
                return;
            }
            _loadModal = _nav.ShowLoadModal("Realizando la dispensación...");
            SessionManager.HantleResponse = await OperationManager.GoDispend(int.Parse(inputDispendValue.Text));
            _nav.CloseLoadModal(ref _loadModal);
            Dispatcher.Invoke(() => { Goto(new FinishUC()); });

            
        }
        public void BtnCancelar(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { Goto(new MenuUC()); });
        }
       
    }
}
