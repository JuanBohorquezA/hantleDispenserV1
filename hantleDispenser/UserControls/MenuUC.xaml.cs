using hantleDispenser.Domain;
using hantleDispenser.Modals;
using MahApps.Metro.Controls;
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
    /// Lógica de interacción para MenuUC.xaml
    /// </summary>
    public partial class MenuUC : AppUserControl
    {
        private ModalWindow? _loadModal;
        public MenuUC()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }
        public void OnLoaded(Object sender, RoutedEventArgs e)
        {
            txtWelcome.Text = $"Hola {SessionManager.CurrentUser?.Name}, ¿que deseas realizar hoy?";
            GetAccions();
        }
        public async void BtnInicialize(object sender, MouseButtonEventArgs e)
        {
            _loadModal = _nav.ShowLoadModal("Realizando la Inicializacion...");
            SessionManager.HantleResponse = await OperationManager.GoInitialize();
            _nav.CloseLoadModal(ref _loadModal);
            Dispatcher.Invoke(() => { Goto(new FinishUC()); });
        }
        public void BtnDispend(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { Goto(new DispendUC()); });
        }
        public void BtnDispendQuantites(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { Goto(new DispendQuantitiesUC()); });
        }
        
        public async void BtnGetSensor(object sender, MouseButtonEventArgs e)
        {
            _loadModal = _nav.ShowLoadModal("Obteniendo la respuesta del sensor...");
            await Task.Delay(1000);
            SessionManager.HantleResponse = await OperationManager.GoGetSensor();
            _nav.CloseLoadModal(ref _loadModal);
            Dispatcher.Invoke(() => { Goto(new FinishUC()); });

        }
        public async void BtntryEject(object sender, MouseButtonEventArgs e)
        {
            _loadModal =  _nav.ShowLoadModal("Realizando la Ejección...");
            SessionManager.HantleResponse = await OperationManager.GoEject();
            _nav.CloseLoadModal(ref _loadModal);
            Dispatcher.Invoke(() => { Goto(new FinishUC()); });
        }
        public void BtnExit(object sender, MouseButtonEventArgs e)
        {
            if (!_nav.ShowModal("¿Estas seguro de que deseas salir?", new ConfirmationModal())) return;
            Dispatcher.Invoke(() => { Goto(new HomeUC()); });

        }
        private void GetAccions()
        {
            var role = SessionManager.CurrentUser?.role;
            if (string.IsNullOrEmpty(role)) return;
            if(role == "root")
            {
                ButtonDispend.Visibility = Visibility.Visible;
                ButtonDispendQuantities.Visibility = Visibility.Visible;
                return;
            }

        }
       
    }
}
