using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TriangulationToolApp.Views;

namespace TriangulationToolApp.ViewModels
{
    public class ShowInputNumRegionsViewModel: ViewModelBase
    {
        private string _informationMessage;

        public ShowInputNumRegionsViewModel(string informationMessage)
        {
            _informationMessage = informationMessage;
            Title = InformationMessage;
            Value = 500;
        }

        /// <summary>
        /// Sets and gets the InformationMessage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InformationMessage
        {
            get { return _informationMessage; }
            set { Set(() => InformationMessage, ref _informationMessage, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }




        private int _value;

        /// <summary>
        /// Sets and gets the Value property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                Set(() => Value, ref _value, value);
                AcceptCommand.RaiseCanExecuteChanged();
            }
        }


        private RelayCommand _acceptCommand;

        public RelayCommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                {
                    _acceptCommand = new RelayCommand(ExecuteAccept, CanExecuteAccept);
                }
                return _acceptCommand;
            }
        }

        private void ExecuteAccept()
        {
            if (CanNotExecuteAccept()) return;
            View.CloseWindow();
            
        }

        private bool CanExecuteAccept()
        {
            
            return true;
        }

        private bool CanNotExecuteAccept()
        {
            return !AcceptCommand.CanExecute(null);
        }

        private RelayCommand _cancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(ExecuteCancel, CanExecuteCancel);
                }
                return _cancelCommand;
            }
        }

        private void ExecuteCancel()
        {
            if (CanNotExecuteCancel()) return;
            IsViewClosed = true;
            Value = -1;
            View.CloseWindow();

        }

        private bool CanExecuteCancel()
        {
            
            return true;
        }

        private bool CanNotExecuteCancel()
        {
            return !CancelCommand.CanExecute(null);
        }

        public ShowInputNumRegionsView View { get; set; }

        public void CloseWindow()
        {
            View.CloseWindow();
        }

        public bool IsViewClosed { get; set; }

        public int GetValue()
        {
            return Value;

        }
    }
}
