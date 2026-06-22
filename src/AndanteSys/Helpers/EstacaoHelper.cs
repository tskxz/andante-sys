using AndanteSys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Helpers
{
    public class EstacaoHelper
    {
        public void Insert(Estacao estacao, Linha linhaAlvo, Zona zonaAlvo)
        {
            if (estacao == null || linhaAlvo == null || zonaAlvo == null) return;

            // vincular a estacao a sua zona
            estacao.Zona = zonaAlvo;
            zonaAlvo.AddEstacao(estacao);

            // adicionar a estacao a rota da linha selecionada
            linhaAlvo.AddEstacao(estacao);

            // instalar automaticamente o validador físico na estação
            estacao.Validador = new Validador { Estacao = estacao };
        }

        public List<Estacao> ListarTodasDoSistema()
        {
            // vai buscar todas as estações registadas em todas as linhas da Rede
            return App.Rede.LstLinha.SelectMany(l => l.LstEstacao).Distinct().ToList();
        }
    }
}
