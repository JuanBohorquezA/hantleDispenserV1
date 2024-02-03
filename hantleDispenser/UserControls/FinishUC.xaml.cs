using hantleDispenser.Domain;
using hantleDispenser.Modals;
using hantleDispenser.UserControls;
using HantleDispenserAPI;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static hantleDispenser.UserControls.FinishViewModel;

namespace hantleDispenser.UserControls
{
    /// <summary>
    /// Lógica de interacción para FinishUC.xaml
    /// </summary>
    public partial class FinishUC : AppUserControl
    {
        private FinishViewModel _viewModel;
        private List<ResponseHantle> List = new(); 
        public FinishUC()
        {
            InitializeComponent();

            _viewModel = new();
            this.DataContext = _viewModel;
            this.Loaded += Onloaded;

        }
        private void Onloaded(object sender, EventArgs e)
        {

            var Response = SessionManager.HantleResponse.Last();
            
            List.Add(new ResponseHantle {CodeError = (int)Response.ErrorCode, Description = Response.ErrorDescription });

            if (Response.isSuccess)
            {
                _viewModel.TitleStatus = "Respuesta exitosa";
                this.Resources["ColorStatus"] = (Brush)Application.Current.Resources["SUCCESSCOLOR"];
                this.Resources["IconState"] = PackIconKind.Check;
            }
            else
            {
                _viewModel.TitleStatus = "Respuesta no fue exitosa";
                this.Resources["ColorStatus"] = (Brush)Application.Current.Resources["ERRORCOLOR"];
                this.Resources["IconState"] = PackIconKind.Error;
            }
                
            DataView.ItemsSource = List;
        }
        private void BtnSeeResponse(object sender, MouseButtonEventArgs e)
        {
            _nav.ShowModalOf(new ModalResponse());
        }
        private void BtnContinue(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => { Goto(new MenuUC()); });
        }

    }
    public class FinishViewModel: INotifyPropertyChanged
    {
        private string _titleStatus = string.Empty;
        public string TitleStatus
        {
            get
            {
                return _titleStatus;
            }
            set
            {
                _titleStatus = value;
                OnPropertyRaised(nameof(TitleStatus));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        }
        public class ResponseHantle
        {
            public bool IsSuccess { get; set; } = false;
            public int CodeError { get; set; }
            public string? Description { get; set; } = string.Empty;
        }
    }
}
