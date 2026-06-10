using System;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using TriangulationToolApp.Business;
using TriangulationToolApp.ViewModels;
using TriangulationToolApp.Views;

namespace TriangulationToolApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainViewModel _viewModel;

        public MainViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                DataContext = _viewModel;
            }
        }
        private const string EyeshotSerialNumber = "PROWPF-97X1-2T1TY-F9HJV-EP5VT";

        public MainWindow()
        {
            InitializeComponent();
            ViewportLayout.Unlock(EyeshotSerialNumber);
            ViewModel = new MainViewModel(ViewportLayout, Snackbar.MessageQueue);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
#if EVALUATION_VERSION
           CheckProtection();
#endif
        }

        private void CheckProtection()
        {
            try
            {
                var remainingUsages = EvaluationProtector.UseApplication();
                Title += string.Format(" - Evaluation Version ({0} usages left)", remainingUsages);
            }
            catch (InvalidOperationException)
            {
                DialogHost.ShowDialog(new EvaluationExpirationDialog(), OnDialogClosing);
            }
        }

        private void OnDialogClosing(object sender, DialogClosingEventArgs eventargs)
        {
            Environment.Exit(0);
        }
    }
}
