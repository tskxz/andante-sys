using AndanteSys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Helpers
{
    public class RegistoValidacaoHelper
    {
        public void Insert(RegistoValidacao registo)
        {
            if (registo != null)
            {
                App.lstRegistos.Add(registo);
            }
        }

        public List<RegistoValidacao> ListarHistorico()
        {
            return App.lstRegistos;
        }
    }
}
}
