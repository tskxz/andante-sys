using AndanteSys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Helpers
{
    public class LinhaHelper
    {
        public void Insert(Linha linha)
        {
            if (linha == null) return;

            // ensure not already present in the Rede
            if (!App.Rede.LstLinha.Any(l => l.LetraLinha == linha.LetraLinha))
            {
                App.Rede.AddLinha(linha);
            }
        }

        public void Apagar(Linha linha)
        {
            if (linha == null) return;

            var linhaExistente = App.Rede.LstLinha.FirstOrDefault(l => l.LetraLinha == linha.LetraLinha);
            if (linhaExistente != null)
            {
                App.Rede.LstLinha.Remove(linhaExistente);
            }
        }

        public List<Linha> ListarTodas()
        {
            return App.Rede.LstLinha;
        }
    }
}
