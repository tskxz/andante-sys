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
            LoadInitialData();

            // wire tree event after component initialization
            var tree = this.FindName("tvZonas") as TreeView;
            if (tree != null)
            {
                tree.SelectedItemChanged += TvZonas_SelectedItemChanged;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string input = txtLoginInput.Text.Trim();
            if (input.Length == 0)
            {
                MessageBox.Show("Por favor, introduza um NIF ou ID válido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // try to find by card id (guid short) or by NIF
            AndanteSys.Models.CartaoAndante cartaoEncontrado = null;
            // search by full GUID
            cartaoEncontrado = App.lstCartoes.FirstOrDefault(c => c.IdCartao.ToString().Equals(input, StringComparison.OrdinalIgnoreCase));
            if (cartaoEncontrado == null)
            {
                // try by short prefix (first 8 chars)
                cartaoEncontrado = App.lstCartoes.FirstOrDefault(c => c.IdCartao.ToString().StartsWith(input, StringComparison.OrdinalIgnoreCase));
            }

            if (cartaoEncontrado == null)
            {
                // try lookup by NIF
                var pessoa = App.lstPessoas.FirstOrDefault(p => p.NIF == input);
                if (pessoa != null)
                {
                    cartaoEncontrado = App.lstCartoes.FirstOrDefault(c => c.Titular != null && c.Titular.IdPessoa == pessoa.IdPessoa);
                }
            }

            if (cartaoEncontrado == null)
            {
                MessageBox.Show("Nenhum cartão ou titular encontrado com essa identificação.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // show session info
            string tipo = cartaoEncontrado.TipoCartao ?? "Desconhecido";
            string nomeTit;
            if (cartaoEncontrado.Titular != null)
            {
                nomeTit = cartaoEncontrado.Titular.Nome;
            }
            else
            {
                nomeTit = "Anónimo";
            }

            lblStatusSessao.Text = $"Sessão Ativa: {nomeTit} ({tipo})";

            if (cartaoEncontrado is AndanteSys.Models.AndanteAzul azul)
            {
                lblSaldoSessao.Text = $"Saldo: {azul.SaldoViagens} viagem(ns) | Zona: {azul.ZonaContratada}";
            }
            else if (cartaoEncontrado is AndanteSys.Models.AndanteGold gold)
            {
                string zonas;
                if (gold.ZonaAutorizada != null)
                {
                    zonas = gold.ZonaAutorizada.CodigoZona;
                }
                else
                {
                    zonas = "Nenhuma";
                }

                lblSaldoSessao.Text = $"Assinatura mês: {gold.MesPago} | Zona: {zonas}";
            }
            else
            {
                lblSaldoSessao.Text = "Cartão sem informação adicional.";
            }

            // store selected card id in Tag for validation step
            panelViagem.Tag = cartaoEncontrado;

            // esconder o login e mostrar viagem
            panelLogin.Visibility = Visibility.Collapsed;
            panelViagem.Visibility = Visibility.Visible;

            // Reseta o validador para o estado inicial
            ellipseLuz.Fill = Brushes.LightGray;
            lblDisplayValidador.Text = "AGUARDANDO VALIDAÇÃO...";
        }

        private void BtnValidar_Click(object sender, RoutedEventArgs e)
        {
            var cartao = panelViagem.Tag as AndanteSys.Models.CartaoAndante;
            if (cartao == null)
            {
                MessageBox.Show("Nenhum cartão válido na sessão. Faça login novamente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var estacao = cbEstacoes.SelectedItem as AndanteSys.Models.Estacao;
            if (estacao == null)
            {
                MessageBox.Show("Selecione uma estação de embarque.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // use the station's validador to process read (this will create and persist RegistoValidacao)
            bool resultado = false;
            if (estacao.Validador != null)
            {
                resultado = estacao.Validador.ProcessarLeitura(cartao);
            }
            else
            {
                // fallback: validate directly against station zone
                resultado = cartao.ValidarViagem(estacao);

                // create and persist registo manually
                var novo = new AndanteSys.Models.RegistoValidacao();
                novo.Sucesso = resultado;
                novo.ZonaValidada = estacao.Zona?.CodigoZona ?? "???";
                novo.Cartao = cartao;
                var rh = new AndanteSys.Helpers.RegistoValidacaoHelper();
                rh.Insert(novo);
            }

            if (resultado)
            {
                ellipseLuz.Fill = Brushes.Green;
                lblDisplayValidador.Text = ">>> VALIDADO COM SUCESSO. <<<";
            }
            else
            {
                ellipseLuz.Fill = Brushes.Red;
                lblDisplayValidador.Text = ">>> RECUSADO - ZONA NÃO AUTORIZADA! <<<";
            }

            // if card is azul, update displayed saldo
            if (cartao is AndanteSys.Models.AndanteAzul az)
            {
                lblSaldoSessao.Text = $"Saldo: {az.SaldoViagens} viagem(ns) | Zona: {az.ZonaContratada}";
            }
        }


        // esconde a viagem e mostra o login outra vez
        private void BtnLogoff_Click(object sender, RoutedEventArgs e)
        {
            txtLoginInput.Clear();
            panelViagem.Tag = null;
            panelViagem.Visibility = Visibility.Collapsed;
            panelLogin.Visibility = Visibility.Visible;
        }

        private void LoadInitialData()
        {
            // populate stations
            cbEstacoes.ItemsSource = null;
            var estHelper = new AndanteSys.Helpers.EstacaoHelper();
            var todas = estHelper.ListarTodasDoSistema();
            cbEstacoes.ItemsSource = todas;
            if (todas != null && todas.Count > 0)
                cbEstacoes.SelectedIndex = 0;

            // populate zones and their stations in the tree view
            LoadZonesTree();
        }

        private void LoadZonesTree()
        {
            var tree = this.FindName("tvZonas") as TreeView;
            if (tree == null) return;
            tree.Items.Clear();
            if (App.lstZonas == null) return;

            foreach (var zona in App.lstZonas)
            {
                var header = $"{zona.CodigoZona} - {zona.NomeRegiao} ({zona.LstEstacao.Count} estações)";
                var tzi = new System.Windows.Controls.TreeViewItem() { Header = header, Tag = zona };

                foreach (var est in zona.LstEstacao)
                {
                    var child = new System.Windows.Controls.TreeViewItem() { Header = est.NomeEstacao, Tag = est };
                    tzi.Items.Add(child);
                }

                tree.Items.Add(tzi);
            }
        }

        private void TvZonas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selected = e.NewValue as System.Windows.Controls.TreeViewItem;
            if (selected == null) return;

            if (selected.Tag is AndanteSys.Models.Estacao est)
            {
                cbEstacoes.SelectedItem = est;
            }
            else if (selected.Tag is AndanteSys.Models.Zona)
            {
                selected.IsExpanded = true;
            }
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
