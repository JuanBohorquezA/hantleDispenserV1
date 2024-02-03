using hantleDispenser.Domain;
using hantleDispenser.Domain.Models;
using hantleDispenser.Modals;
using hantleDispenser.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static hantleDispenser.UserControls.DispendQuantitiesViewModel;

namespace hantleDispenser.UserControls
{
    /// <summary>
    /// Lógica de interacción para Dispende.xaml
    /// </summary>
    public partial class DispendQuantitiesUC : AppUserControl
    {
        public DispendQuantitiesViewModel _viewModel;
        public List<DenominationQuantity> combinedList = new ();
        private ModalWindow? _loadModal;
        public DispendQuantitiesUC()
        {
            InitializeComponent();
            _viewModel = new();
            this.DataContext = _viewModel;

            this.Loaded += Onloaded;
        
        }
        public void Onloaded(object sender, EventArgs e)
        {
            var HantleProps = SessionManager.CurrentProps;
            for (int i = 0; i < HantleProps.Denomination.Count; i++)
            {
                combinedList.Add(new DenominationQuantity { Denomination = HantleProps.Denomination[i], Quantity = 0 });
            }
            DataView.ItemsSource = combinedList;

        }
        public void BtnSum(object sender, EventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext != null)
            {
                var fila = element.DataContext as DenominationQuantity;
                if (fila != null) fila.Quantity += 1;
            }
            DataView.Items.Refresh();
        }
        public void BtnSubtract(object sender, EventArgs e)
        {
            if(sender is FrameworkElement element && element.DataContext != null)
            {
                var fila = element.DataContext as DenominationQuantity;
                if (fila != null && fila.Quantity > 0) fila.Quantity -= 1;
            }
            DataView.Items.Refresh();
        }


        public async void BtnContinue(object sender, MouseButtonEventArgs e)
        {
            txtMessage.Visibility = Visibility.Collapsed;

            var quantities = combinedList.Select(Element => Element.Quantity);
            bool IsValid = quantities.Where(quantity => quantity > 0).Any();
            if (!IsValid)
            {
                txtMessage.Visibility = Visibility.Visible;
                return;
            } 

            bool IsConfirmed = _nav.ShowModal("¿Estas seguro de realizar esta acción? es completamente irreversible...", new ConfirmationModal());
            if(!IsConfirmed)
            {
                Dispatcher.Invoke(() => { Goto(new MenuUC()); });
                return;
            }
            _loadModal =_nav.ShowLoadModal("Realizando la dispensación...");

            var result = await OperationManager.GoDispendQuantities(quantities.ToArray());

            _nav.CloseLoadModal(ref _loadModal);
            
            Dispatcher.Invoke(() => { Goto(new FinishUC()); });
            
        }
      
        public void BtnCancelar(object sender, MouseButtonEventArgs e)
        {
            
            Dispatcher.Invoke(() => { Goto(new MenuUC()); });
        }
        
    }
    public class DispendQuantitiesViewModel: INotifyPropertyChanged
    {
        private List<string> _denominations = new List<string>();
        public List<string> Denominations
        {
            get
            {
                return _denominations;
            }
            set
            {
                _denominations = value;
                OnPropertyRaised(nameof(Denominations));
            }
        }

        private List<int> _quantities = new List<int>();
        public List<int> Quantities
        {
            get
            {
                return _quantities;
            }
            set
            {
                _quantities = value;
                OnPropertyRaised(nameof(Quantities));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        }
        public class DenominationQuantity
        {
            public string Denomination { get; set; } = string.Empty;
            public int Quantity { get; set; } = 0;
        }

    }

}
