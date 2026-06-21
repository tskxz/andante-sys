using AndanteSys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Helpers
{
    public class ZonaHelper
    {
        public void Insert(Zona zona)
        {
            if (zona != null && !App.lstZonas.Any(z => z.CodigoZona == zona.CodigoZona))
            {
                App.lstZonas.Add(zona);
            }
        }

        public void Apagar(Zona zona)
        {
            var zonaExistente = App.lstZonas.FirstOrDefault(z => z.CodigoZona == zona.CodigoZona);
            if (zonaExistente != null)
            {
                App.lstZonas.Remove(zonaExistente);
            }
        }

        public List<Zona> ListarTodas()
        {
            return App.lstZonas;
        }
    }
}
