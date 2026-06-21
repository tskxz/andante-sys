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
        public static List<Pessoa> lstPessoas = new List<Pessoa>();
        public static List<CartaoAndante> lstCartoes = new List<CartaoAndante>();
        public static List<Zona> lstZonas = new List<Zona>();
        public static List<Linha> lstLinhas = new List<Linha>();
        public static List<RegistoValidacao> lstRegistos = new List<RegistoValidacao>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InicializarCenarioMetro();
        }
        private void InicializarCenarioMetro()
        {
            // Criar duas zonas base
            Zona z1 = new Zona { CodigoZona = "PRT1", NomeRegiao = "Porto Centro" };
            Zona z2 = new Zona { CodigoZona = "MTS1", NomeRegiao = "Matosinhos" };
            lstZonas.Add(z1);
            lstZonas.Add(z2);

            // criar estacoes e validadores
            Estacao trindade = new Estacao { NomeEstacao = "Trindade", Zona = z1 };
            trindade.AddValidador(new Validador { Estacao = trindade });
            z1.AddEstacao(trindade);

            Estacao matosinhos = new Estacao { NomeEstacao = "Matosinhos Sul", Zona = z2 };
            matosinhos.AddValidador(new Validador { Estacao = matosinhos });
            z2.AddEstacao(trindade);
            z2.AddEstacao(matosinhos);

            // criar uma linha
            Linha linhaAzul = new Linha { LetraLinha = 'A', NomeLinha = "Linha de Matosinhos", Cor = "Azul" };
            linhaAzul.AddEstacao(matosinhos);
            linhaAzul.AddEstacao(trindade);
            lstLinhas.Add(linhaAzul);
        }
    }

}
