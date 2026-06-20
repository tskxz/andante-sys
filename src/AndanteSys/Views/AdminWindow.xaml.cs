using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AndanteSys.Views
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        // fechar a janela e reabrir o menu principal
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {

            // janela principal main window
            MainWindow mainWin = new MainWindow();
            mainWin.Show();

            this.Close();
        }

        private void BtnEmitirCartao_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCriarZona_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCriarLinha_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCriarEstacao_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCarregarAzul_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRenovarGold_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
