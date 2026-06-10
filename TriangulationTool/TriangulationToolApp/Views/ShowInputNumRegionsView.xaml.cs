using System.Windows;

namespace TriangulationToolApp.Views
{
    /// <summary>
    /// Lógica de interacción para ShowInputTextView.xaml
    /// </summary>
    public partial class ShowInputNumRegionsView 
    {
        public ShowInputNumRegionsView()
        {
            InitializeComponent();
        }

        public void CloseWindow()
        {
            Close();
        }

        public void ShowView()
        {
            ShowDialog();
        }

        private void GetFocus()
        {
            TextBoxConfigurationName.Focus();
        }

        private void ShowInputNumRegionsViewOnLoaded(object sender, RoutedEventArgs e)
        {
            GetFocus();
        }
    }
}
