using AndanteSys.Models;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace AndanteSys
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static RedeMetro Rede = new RedeMetro();

        public static List<Pessoa> lstPessoas = new List<Pessoa>();
        public static List<CartaoAndante> lstCartoes = new List<CartaoAndante>();
        public static List<Zona> lstZonas = new List<Zona>();
        // legacy collection kept for compatibility; prefer App.Rede.LstLinha
        public static List<Linha> lstLinhas = new List<Linha>();
        public static List<RegistoValidacao> lstRegistos = new List<RegistoValidacao>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InicializarCenarioMetro();
        }
        private void InicializarCenarioMetro()
        {
            // Criar zonas base
            Zona z1 = new Zona { CodigoZona = "PRT1", NomeRegiao = "Porto Centro" };
            Zona z2 = new Zona { CodigoZona = "MTS1", NomeRegiao = "Matosinhos" };
            Zona z3 = new Zona { CodigoZona = "MAI1", NomeRegiao = "Maia" };
            Zona z4 = new Zona { CodigoZona = "GDL1", NomeRegiao = "Gaia / Gondomar" };

            lstZonas.Add(z1);
            lstZonas.Add(z2);
            lstZonas.Add(z3);
            lstZonas.Add(z4);

            // Criar estacoes e instalar validadores
            Estacao trindade = new Estacao { NomeEstacao = "Trindade", Zona = z1 };
            trindade.Validador = new Validador { Estacao = trindade };
            z1.AddEstacao(trindade);

            Estacao matosinhos = new Estacao { NomeEstacao = "Matosinhos Sul", Zona = z2 };
            matosinhos.Validador = new Validador { Estacao = matosinhos };
            z2.AddEstacao(matosinhos);

            Estacao aliados = new Estacao { NomeEstacao = "Aliados", Zona = z1 };
            aliados.Validador = new Validador { Estacao = aliados };
            z1.AddEstacao(aliados);

            Estacao senhoraHora = new Estacao { NomeEstacao = "Senhora da Hora", Zona = z1 };
            senhoraHora.Validador = new Validador { Estacao = senhoraHora };
            z1.AddEstacao(senhoraHora);

            Estacao maiaCentro = new Estacao { NomeEstacao = "Maia Centro", Zona = z3 };
            maiaCentro.Validador = new Validador { Estacao = maiaCentro };
            z3.AddEstacao(maiaCentro);

            Estacao gondomar = new Estacao { NomeEstacao = "Gondomar Centro", Zona = z4 };
            gondomar.Validador = new Validador { Estacao = gondomar };
            z4.AddEstacao(gondomar);

            // Criar linhas e associar estacoes
            Linha linhaAzul = new Linha { LetraLinha = 'A', NomeLinha = "Linha Matosinhos - Centro", Cor = "Azul" };
            linhaAzul.AddEstacao(matosinhos);
            linhaAzul.AddEstacao(trindade);
            linhaAzul.AddEstacao(aliados);

            Linha linhaVermelha = new Linha { LetraLinha = 'B', NomeLinha = "Linha Maia", Cor = "Vermelho" };
            linhaVermelha.AddEstacao(maiaCentro);
            linhaVermelha.AddEstacao(trindade);

            Linha linhaVerde = new Linha { LetraLinha = 'C', NomeLinha = "Linha Gondomar", Cor = "Verde" };
            linhaVerde.AddEstacao(gondomar);
            linhaVerde.AddEstacao(senhoraHora);

            // legacy compatibility: keep lstLinhas populated for older helpers/views
            lstLinhas.Add(linhaAzul);
            lstLinhas.Add(linhaVermelha);
            lstLinhas.Add(linhaVerde);

            
            Rede.NomeRede = "Rede Andante Demo";
            Rede.AddLinha(linhaAzul);
            Rede.AddLinha(linhaVermelha);
            Rede.AddLinha(linhaVerde);

            // Criar algumas pessoas
            Pessoa pessoaCarlos = new Pessoa { Nome = "Carlos Silva", NIF = "123456789" };
            Pessoa pessoaAna = new Pessoa { Nome = "Ana Pereira", NIF = "987654321" };
            Pessoa pessoaJoao = new Pessoa { Nome = "João Sousa", NIF = "555444333" };

            lstPessoas.Add(pessoaCarlos);
            lstPessoas.Add(pessoaAna);
            lstPessoas.Add(pessoaJoao);

            // Emitir alguns cartoes
            var cartaoGoldCarlos = new AndanteGold();
            cartaoGoldCarlos.Titular = pessoaCarlos;
            cartaoGoldCarlos.RenovarAssinatura(DateTime.Now.Month);
            cartaoGoldCarlos.ZonaAutorizada = z1; // Porto Centro

            var cartaoAzulAna = new AndanteAzul();
            cartaoAzulAna.Titular = pessoaAna;
            cartaoAzulAna.CarregarViagens(5, z2.CodigoZona); // Matosinhos

            var cartaoAzulAnonimo = new AndanteAzul();
            cartaoAzulAnonimo.CarregarViagens(3, z1.CodigoZona); // ocasional anónimo

            var cartaoGoldJoao = new AndanteGold();
            cartaoGoldJoao.Titular = pessoaJoao;
            cartaoGoldJoao.RenovarAssinatura(DateTime.Now.Month);
            cartaoGoldJoao.ZonaAutorizada = z3; // Maia

            lstCartoes.Add(cartaoGoldCarlos);
            lstCartoes.Add(cartaoAzulAna);
            lstCartoes.Add(cartaoAzulAnonimo);
            lstCartoes.Add(cartaoGoldJoao);
        }
    }

}
