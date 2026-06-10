using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;
using TriangulationToolApp.Business;

namespace TriangulationToolApp.Views
{
    /// <summary>
    /// Interaction logic for EvaluationExpirationContactDialog.xaml
    /// </summary>
    public partial class EvaluationExpirationDialog : UserControl
    {
        public EvaluationExpirationDialog()
        {
            InitializeComponent();
            ContactHyperlink.NavigateUri = new Uri(string.Format("mailto:{0}", EvaluationProtector.ContactEmail));
        }

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
