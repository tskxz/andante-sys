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
            if (linha != null && !App.lstLinhas.Any(l => l.LetraLinha == linha.LetraLinha))
            {
                App.lstLinhas.Add(linha);
            }
        }

        public void Apagar(Linha linha)
        {
            var linhaExistente = App.lstLinhas.FirstOrDefault(l => l.LetraLinha == linha.LetraLinha);
            if (linhaExistente != null)
            {
                App.lstLinhas.Remove(linhaExistente);
            }
        }

        public List<Linha> ListarTodas()
        {
            return App.lstLinhas;
        }
    }
}
