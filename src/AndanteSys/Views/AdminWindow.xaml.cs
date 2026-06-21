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
            LoadInitialData();
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
            string nome = txtNomeTitular.Text.Trim();
            string nif = txtNifTitular.Text.Trim();

            var cartaoHelper = new Helpers.CartaoHelper();

            if (cbTipoCartao.SelectedIndex == 1) // Gold
            {
                if (nome.Length == 0 || nif.Length == 0)
                {
                    MessageBox.Show("Para emitir um Andante Gold é obrigatório informar Nome e NIF.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var pessoa = new AndanteSys.Models.Pessoa { Nome = nome, NIF = nif };
                var ph = new Helpers.PessoaHelper();
                ph.Insert(pessoa);

                var gold = new AndanteSys.Models.AndanteGold();
                gold.Titular = pessoa;
                gold.RenovarAssinatura(DateTime.Now.Month);
                cartaoHelper.Insert(gold);
            }
            else // azul pode ser anonimo
            {
                var azul = new AndanteSys.Models.AndanteAzul();

                if (!string.IsNullOrEmpty(nome) || !string.IsNullOrEmpty(nif))
                {
                    var pessoa = new AndanteSys.Models.Pessoa { Nome = nome, NIF = nif };
                    var ph = new Helpers.PessoaHelper();
                    ph.Insert(pessoa);
                    azul.Titular = pessoa;
                }

                cartaoHelper.Insert(azul);
            }

            // clear input fields
            txtNomeTitular.Text = string.Empty;
            txtNifTitular.Text = string.Empty;

            RefreshCartoesGrid();
            MessageBox.Show("Cartão emitido com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnCriarZona_Click(object sender, RoutedEventArgs e)
        {
            string codigo = txtCodigoZona.Text.Trim().ToUpper();
            string nome = txtNomeRegiao.Text.Trim();

            if (codigo.Length == 0)
            {
                MessageBox.Show("Informe o código da zona.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zona = new AndanteSys.Models.Zona { CodigoZona = codigo, NomeRegiao = nome };
            var zh = new Helpers.ZonaHelper();
            zh.Insert(zona);

            LoadZonesIntoCombos();
            MessageBox.Show("Zona criada.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnCriarLinha_Click(object sender, RoutedEventArgs e)
        {
            string letra = txtLetraLinha.Text.Trim().ToUpper();
            string nome = txtNomeLinha.Text.Trim();
            string cor = txtCorLinha.Text.Trim();

            if (letra.Length == 0)
            {
                MessageBox.Show("Informe a letra identificadora da linha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var linha = new AndanteSys.Models.Linha { LetraLinha = letra[0], NomeLinha = nome, Cor = cor };
            var lh = new Helpers.LinhaHelper();
            lh.Insert(linha);

            LoadLinesIntoCombo();
            MessageBox.Show("Linha registada.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnCriarEstacao_Click(object sender, RoutedEventArgs e)
        {
            string nomeEst = txtNomeEstacao.Text.Trim();
            var linha = cbLinhasDisponiveis.SelectedItem as AndanteSys.Models.Linha;
            var zona = cbZonasDisponiveis.SelectedItem as AndanteSys.Models.Zona;

            if (string.IsNullOrEmpty(nomeEst) || linha == null || zona == null)
            {
                MessageBox.Show("Preencha o nome da estação e selecione linha e zona.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var est = new AndanteSys.Models.Estacao { NomeEstacao = nomeEst };
            var eh = new Helpers.EstacaoHelper();
            eh.Insert(est, linha, zona);

            MessageBox.Show("Estação instalada.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnCarregarAzul_Click(object sender, RoutedEventArgs e)
        {
            if (dgCartoes.SelectedItem == null)
            {
                MessageBox.Show("Selecione um cartão na grelha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cartao = dgCartoes.SelectedItem as AndanteSys.Models.CartaoAndante;
            if (cartao is AndanteSys.Models.AndanteAzul azul)
            {
                if (!int.TryParse(txtQtdViagens.Text.Trim(), out int qtd) || qtd <= 0)
                {
                    MessageBox.Show("Quantidade inválida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var zona = cbZonasCarregamento.SelectedItem as AndanteSys.Models.Zona;
                string codigoZona;
                if (zona != null)
                {
                    codigoZona = zona.CodigoZona;
                }
                else
                {
                    codigoZona = "PRT1";
                }

                azul.CarregarViagens(qtd, codigoZona);
                
                cbZonasCarregamento.SelectedValue = codigoZona;
                RefreshCartoesGrid();
                MessageBox.Show("Carregamento efetuado.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("O cartão seleccionado não é Andante Azul.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRenovarGold_Click(object sender, RoutedEventArgs e)
        {
            if (dgCartoes.SelectedItem == null)
            {
                MessageBox.Show("Selecione um cartão na grelha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cartao = dgCartoes.SelectedItem as AndanteSys.Models.CartaoAndante;
            if (cartao is AndanteSys.Models.AndanteGold gold)
            {
                // renovar mês pago
                gold.RenovarAssinatura(DateTime.Now.Month);

                
                // atualizamos a zona autorizada do passe gold para a selecionada
                var zonaSelecionada = cbZonasCarregamento.SelectedItem as AndanteSys.Models.Zona;
                if (zonaSelecionada != null)
                {
                    // obtain current authorized zone code (if any)
                    string codigoAtual;
                    if (gold.ZonaAutorizada != null && gold.ZonaAutorizada.CodigoZona != null)
                    {
                        codigoAtual = gold.ZonaAutorizada.CodigoZona;
                    }
                    else
                    {
                        codigoAtual = string.Empty;
                    }

                    // normalize both values
                    string codigoAtualNorm;
                    if (codigoAtual != null)
                    {
                        codigoAtualNorm = codigoAtual.Trim().ToUpperInvariant();
                    }
                    else
                    {
                        codigoAtualNorm = string.Empty;
                    }

                    string zonaSelNorm;
                    if (zonaSelecionada != null && zonaSelecionada.CodigoZona != null)
                    {
                        zonaSelNorm = zonaSelecionada.CodigoZona.Trim().ToUpperInvariant();
                    }
                    else
                    {
                        zonaSelNorm = string.Empty;
                    }

                    // update ZonaAutorizada only if different
                    if (codigoAtualNorm.Length != zonaSelNorm.Length)
                    {
                        gold.ZonaAutorizada = zonaSelecionada;
                    }
                    else
                    {
                        if (codigoAtualNorm == zonaSelNorm)
                        {
                            // same zone -> do nothing
                        }
                        else
                        {
                            gold.ZonaAutorizada = zonaSelecionada;
                        }
                    }
                }

                RefreshCartoesGrid();
                MessageBox.Show("Assinatura renovada e zonas atualizadas (se aplicável).", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("O cartão seleccionado não é Andante Gold.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnVerRegistos_Click(object sender, RoutedEventArgs e)
        {
            var win = new RegistosWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void LoadInitialData()
        {
            LoadLinesIntoCombo();
            LoadZonesIntoCombos();
            RefreshCartoesGrid();
        }

        private void LoadLinesIntoCombo()
        {
            cbLinhasDisponiveis.ItemsSource = null;
            cbLinhasDisponiveis.ItemsSource = App.lstLinhas;
        }

        private void LoadZonesIntoCombos()
        {
            cbZonasDisponiveis.ItemsSource = null;
            cbZonasCarregamento.ItemsSource = null;

            cbZonasDisponiveis.ItemsSource = App.lstZonas;
            cbZonasCarregamento.ItemsSource = App.lstZonas;
        }

        private void RefreshCartoesGrid()
        {
            dgCartoes.ItemsSource = null;
            dgCartoes.ItemsSource = App.lstCartoes;
        }

        private void DgCartoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgCartoes.SelectedItem as AndanteSys.Models.CartaoAndante;
            if (selected == null)
            {
                txtQtdViagens.Text = "";
                cbZonasCarregamento.SelectedItem = null;
                return;
            }

                if (selected is AndanteSys.Models.AndanteAzul azul)
            {
                // show saldo and contracted zone
                txtQtdViagens.Text = azul.SaldoViagens.ToString();
                if (!string.IsNullOrEmpty(azul.ZonaContratada))
                {
                    cbZonasCarregamento.SelectedValue = azul.ZonaContratada;
                }
                else
                {
                    cbZonasCarregamento.SelectedItem = null;
                }
            }
            else if (selected is AndanteSys.Models.AndanteGold gold)
            {
                txtQtdViagens.Text = "0";
                if (gold.ZonaAutorizada != null)
                {
                    var codigo = gold.ZonaAutorizada.CodigoZona;
                    var zonaObj = App.lstZonas.FirstOrDefault(z => z.CodigoZona == codigo);
                    cbZonasCarregamento.SelectedItem = zonaObj;
                }
                else
                {
                    cbZonasCarregamento.SelectedItem = null;
                }
            }
            else
            {
                txtQtdViagens.Text = "";
                cbZonasCarregamento.SelectedItem = null;
            }
        }
    }
}
