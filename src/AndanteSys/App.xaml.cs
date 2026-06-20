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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Inicia o cenário de testes assim que o programa abre
            ExecutarSimulacaoAndante();
        }

        private void ExecutarSimulacaoAndante()
        {
            Debug.WriteLine("======================================================================");
            Debug.WriteLine("          INICIANDO SIMULAÇÃO DE SISTEMA METRO                        ");
            Debug.WriteLine("======================================================================");

            // -------------------------------------------------------------------------
            // [PARTE ADMINISTRADOR] - Configuração de infraestrutura de rede, zonas e linhas
            // -------------------------------------------------------------------------
            Debug.WriteLine("\n[ADMINISTRADOR] -> Criando Zonas Tarifárias...");
            Zona zonaPrt1 = new Zona { CodigoZona = "PRT1", NomeRegiao = "Porto Centro" };
            Zona zonaPrt2 = new Zona { CodigoZona = "PRT2", NomeRegiao = "Campanhã / Asprela" };
            Zona zonaMts1 = new Zona { CodigoZona = "MTS1", NomeRegiao = "Matosinhos Sul" };

            Debug.WriteLine($"Criada: {zonaPrt1}");
            Debug.WriteLine($"Criada: {zonaPrt2}");
            Debug.WriteLine($"Criada: {zonaMts1}");

            Debug.WriteLine("\n[ADMINISTRADOR] -> Criando Estações e instalando Validadores...");

            // Estação 1: Trindade (Zona PRT1)
            Estacao estacaoTrindade = new Estacao { NomeEstacao = "Trindade", Zona = zonaPrt1 };
            Validador validadorTrindade1 = new Validador { Estacao = estacaoTrindade };
            Validador validadorTrindade2 = new Validador { Estacao = estacaoTrindade };
            estacaoTrindade.AddValidador(validadorTrindade1);
            estacaoTrindade.AddValidador(validadorTrindade2);
            zonaPrt1.AddEstacao(estacaoTrindade);

            // Estação 2: Dragão (Zona PRT2)
            Estacao estacaoDragao = new Estacao { NomeEstacao = "Estádio do Dragão", Zona = zonaPrt2 };
            Validador validadorDragao = new Validador { Estacao = estacaoDragao };
            estacaoDragao.AddValidador(validadorDragao);
            zonaPrt2.AddEstacao(estacaoDragao);

            // Estação 3: Senhor de Matosinhos (Zona MTS1)
            Estacao estacaoMatosinhos = new Estacao { NomeEstacao = "Senhor de Matosinhos", Zona = zonaMts1 };
            Validador validadorMts = new Validador { Estacao = estacaoMatosinhos };
            estacaoMatosinhos.AddValidador(validadorMts);
            zonaMts1.AddEstacao(estacaoMatosinhos);

            Debug.WriteLine(estacaoTrindade);
            Debug.WriteLine(estacaoDragao);
            Debug.WriteLine(estacaoMatosinhos);

            Debug.WriteLine("\n[ADMINISTRADOR] -> Organizando as Linhas e a Rede Central...");
            RedeMetro metroDoPorto = new RedeMetro { NomeRede = "Metro do Porto" };

            // Criar Linha A (Azul) - Vai de Matosinhos até à Trindade
            Linha linhaAzul = new Linha { LetraLinha = 'A', NomeLinha = "Linha de Matosinhos", Cor = "Azul" };
            linhaAzul.AddEstacao(estacaoMatosinhos);
            linhaAzul.AddEstacao(estacaoTrindade);

            // Criar Linha F (Laranja) - Passa na Trindade e vai até ao Dragão
            Linha linhaLaranja = new Linha { LetraLinha = 'F', NomeLinha = "Linha de Gondomar", Cor = "Laranja" };
            linhaLaranja.AddEstacao(estacaoTrindade);
            linhaLaranja.AddEstacao(estacaoDragao);

            // Adicionar Linhas à Rede
            metroDoPorto.AddLinha(linhaAzul);
            metroDoPorto.AddLinha(linhaLaranja);

            Debug.WriteLine(metroDoPorto);
            Debug.WriteLine(linhaAzul);
            Debug.WriteLine(linhaLaranja);

            // -------------------------------------------------------------------------
            // [PARTE CLIENTE] - Criação de Clientes, Emissão de Cartões e Carregamentos
            // -------------------------------------------------------------------------
            Debug.WriteLine("\n----------------------------------------------------------------------");
            Debug.WriteLine("[CLIENTE] -> Registando Passageiros e Emitindo Títulos Andante...");

            Pessoa passageiro1 = new Pessoa { Nome = "Carlos Silva", NIF = "251432999" };
            Pessoa passageiro2 = new Pessoa { Nome = "Ana Rocha", NIF = "264875111" };

            // 1. Cliente Carlos compra um Cartão AZUL (Vem por defeito configurado para PRT1 com 0 viagens)
            AndanteAzul cartaoCarlos = new AndanteAzul { Titular = passageiro1 };

            // Carlos carrega o seu cartão azul com 2 viagens para a zona central PRT1
            cartaoCarlos.CarregarViagens(2, "PRT1");

            // 2. Cliente Ana compra um Cartão GOLD (Assinatura mensal ativa para o mês corrente)
            AndanteGold cartaoAna = new AndanteGold { Titular = passageiro2 };
            cartaoAna.RenovarAssinatura(DateTime.Now.Month); // Paga o mês atual

            // O construtor do Gold já adiciona PRT1 e PRT2 por defeito na tua classe.
            // Vamos adicionar também a zona de Matosinhos (MTS1) às autorizações da Ana
            cartaoAna.AddZonaAutorizada(zonaMts1);

            Debug.WriteLine($"Cliente 1: {passageiro1.Nome} -> {cartaoCarlos}");
            Debug.WriteLine($"Cliente 2: {passageiro2.Nome} -> {cartaoAna}");

            // -------------------------------------------------------------------------
            // [SIMULAÇÃO DE MOVIMENTAÇÃO / VIAGENS]
            // -------------------------------------------------------------------------
            Debug.WriteLine("\n----------------------------------------------------------------------");
            Debug.WriteLine("                     SIMULAÇÃO DE VIAGENS EM TEMPO REAL               ");
            Debug.WriteLine("----------------------------------------------------------------------");

            // VIAGEM 1: Carlos entra na Trindade (Zona PRT1) e valida o seu cartão Azul
            Debug.WriteLine($"\n[Viagem] {cartaoCarlos.Titular.Nome} passa o cartão Azul na Trindade (Máquina Zona PRT1)...");
            bool okCarlos1 = validadorTrindade1.ProcessarLeitura(cartaoCarlos);
            Debug.WriteLine($"Result no Validador: {(okCarlos1 ? "VERDE (Acesso Autorizado)" : "VERMELHO (Recusado)")}");
            Debug.WriteLine($"Saldo atual do Carlos: {cartaoCarlos.SaldoViagens} viagem(ns)");

            // VIAGEM 2: Carlos vai até Matosinhos Sul (Zona MTS1) e tenta validar o mesmo cartão Azul
            // Deve falhar, porque o cartão dele está bloqueado eletronicamente para a zona PRT1!
            Debug.WriteLine($"\n[Viagem] {cartaoCarlos.Titular.Nome} tenta validar o cartão Azul em Matosinhos (Máquina Zona MTS1)...");
            bool okCarlos2 = validadorMts.ProcessarLeitura(cartaoCarlos);
            Debug.WriteLine($"Result no Validador: {(okCarlos2 ? "VERDE (Acesso Autorizado)" : "VERMELHO (Recusado - Zona Incompatível)")}");

            // VIAGEM 3: Ana Rocha viaja muito. Valida o seu passe Gold em Matosinhos (Zona MTS1)
            Debug.WriteLine($"\n[Viagem] {cartaoAna.Titular.Nome} valida o passe Gold em Matosinhos (Máquina Zona MTS1)...");
            bool okAna1 = validadorMts.ProcessarLeitura(cartaoAna);
            Debug.WriteLine($"Result no Validador: {(okAna1 ? "VERDE (Acesso Autorizado)" : "VERMELHO (Recusado)")}");

            // VIAGEM 4: Ana apanha o metro até à Trindade (Zona PRT1) e faz nova validação
            Debug.WriteLine($"\n[Viagem] {cartaoAna.Titular.Nome} valida o passe Gold na Trindade (Máquina Zona PRT1)...");
            bool okAna2 = validadorTrindade2.ProcessarLeitura(cartaoAna);
            Debug.WriteLine($"Result no Validador: {(okAna2 ? "VERDE (Acesso Autorizado)" : "VERMELHO (Recusado)")}");

            // -------------------------------------------------------------------------
            // [AUDITORIA / FISCALIZAÇÃO CENTRAL]
            // -------------------------------------------------------------------------
            Debug.WriteLine("\n======================================================================");
            Debug.WriteLine("          RELATÓRIO DE FISCALIZAÇÃO CENTRAL (REGISTOS DE VALIDAÇÃO)    ");
            Debug.WriteLine("======================================================================");
            Debug.WriteLine($"Total de leituras registadas no sistema: {Validador.HistoricoGlobal.Count}\n");

            foreach (RegistoValidacao registo in Validador.HistoricoGlobal)
            {
                // Aqui é impresso o ToString() detalhado que criaste na classe RegistoValidacao!
                Debug.WriteLine(registo.ToString());
            }

            Debug.WriteLine("======================================================================\n");
        }
    }

}
