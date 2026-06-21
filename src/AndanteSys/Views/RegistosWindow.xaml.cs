using AndanteSys.Helpers;
using AndanteSys.Models;
using System.Windows;

namespace AndanteSys.Views
{
    /// <summary>
    /// Interaction logic for RegistosWindow.xaml
    /// </summary>
    public partial class RegistosWindow : Window
    {
        public RegistosWindow()
        {
            InitializeComponent();
            LoadRegistos();
        }

        private void LoadRegistos()
        {
            var rh = new RegistoValidacaoHelper();
            dgRegistos.ItemsSource = null;
            dgRegistos.ItemsSource = rh.ListarHistorico();
        }
    }
}
