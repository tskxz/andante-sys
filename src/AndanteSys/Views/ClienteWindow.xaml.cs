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
    /// Interaction logic for ClienteWindow.xaml
    /// </summary>
    public partial class ClienteWindow : Window
    {
        public ClienteWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string input = txtLoginInput.Text.Trim();
            if(input.Length == 0)
            {
                MessageBox.Show("Por favor, introduza um NIF ou ID válido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: isto e uma solucao temporaria, dps mudo isto, tenho que criar as classes do Model
            if(input == "123456789")
            {
                lblStatusSessao.Text = "Sessão Ativa: Carlos Silva (Passe Gold)";
                lblSaldoSessao.Text = "Assinatura Ativa: Zona PRT1";
            } else if (input.ToUpper() == "AZUL-01")
            {
                lblStatusSessao.Text = "Sessão Ativa: Cartão Ocasional Anónimo (ID: AZUL-01)";
                lblSaldoSessao.Text = "Saldo Atual: 2 Viagens";
            } else 
            {
                // para ja aceita tudo so pa testar o layout
                lblStatusSessao.Text = $"Sessão Ativa: Cartão de Teste ({input})";
                lblSaldoSessao.Text = "Modo de simulação geral.";
            }

            // esconder o login e mostrar viagem
            panelLogin.Visibility = Visibility.Collapsed;
            panelViagem.Visibility = Visibility.Visible;

            // Reseta o validador para o estado inicial
            ellipseLuz.Fill = Brushes.LightGray;
            lblDisplayValidador.Text = "AGUARDANDO VALIDAÇÃO...";
        }

        private void BtnValidar_Click(object sender, RoutedEventArgs e)
        {
            string input = txtLoginInput.Text.Trim();
            int estacaoSelecionada = cbEstacoes.SelectedIndex; // 0 = Trindade, 1 = Matosinhos
            if (input == "123456789")
            {
                if (estacaoSelecionada == 0)
                {
                    ellipseLuz.Fill = Brushes.Green;
                    lblDisplayValidador.Text = ">>> LUZ VERDE: VALIDADO COM SUCESSO. BOA VIAGEM! <<<";
                }
                else
                {
                    ellipseLuz.Fill = Brushes.Red;
                    lblDisplayValidador.Text = ">>> LUZ VERMELHA: RECUSADO - ZONA NÃO AUTORIZADA! <<<";
                }
            }
            else
            {
                // temporario, simula sucesso direto apenas para ver o comportamento do ecrã
                ellipseLuz.Fill = Brushes.Green;
                lblDisplayValidador.Text = ">>> LUZ VERDE: VIAGEM DESCONTADA. BOA VIAGEM! <<<";
            }
        }


        // esconde a viagem e mostra o login outra vez
        private void BtnLogoff_Click(object sender, RoutedEventArgs e)
        {

            
            txtLoginInput.Clear();
            panelViagem.Visibility = Visibility.Collapsed;
            panelLogin.Visibility = Visibility.Visible;
        }

        // voltar ao menu principal
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWin = new MainWindow();
            mainWin.Show();
            this.Close();
        }
    }
}
