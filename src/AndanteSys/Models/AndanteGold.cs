using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AndanteSys.Models
{
    public class AndanteGold : CartaoAndante
    {

        private int _mesPago;
        private List<Zona> _zonasAutorizadas;

        public int MesPago
        {
            get { return _mesPago; }
            set {
                _mesPago = value;
                if (_mesPago < 1 || _mesPago > 12)
                {
                    _mesPago = 0;
                }
            }
        }

        public List<Zona> ZonasAutorizadas
        {
            get { return _zonasAutorizadas; }
            set { _zonasAutorizadas = value; }
        }
       

        public override bool ValidarViagem(Zona zonaEstacaoAtual)
        {
            int mesAtual = DateTime.Now.Month;
            // o mes do passe tem de estar atualizado e a zona da estação tem de estar autorizada
            if (_mesPago == mesAtual)
            {
                if (zonaEstacaoAtual != null)
                {
                    if (_zonasAutorizadas.Any(z => z.CodigoZona == zonaEstacaoAtual.CodigoZona))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void AddZonaAutorizada(Zona zona)
        {
            if (zona == null)
                return;

            string codigo = zona.CodigoZona?.Trim().ToUpper();
            if (string.IsNullOrEmpty(codigo))
                return;

            if (!_zonasAutorizadas.Any(z => z.CodigoZona == codigo))
            {
                Zona nova = new Zona { CodigoZona = codigo };
                _zonasAutorizadas.Add(nova);
            }
        }

        public void RenovarAssinatura(int mes)
        {
            _mesPago = mes;
        }

        public AndanteGold()
        {
            _mesPago = 0;
            _zonasAutorizadas = new List<Zona> { new Zona { CodigoZona = "PRT1" }, new Zona { CodigoZona = "PRT2" } };
            TipoCartao = "GOLD";
        }

        public override string ToString()
        {
            return $"{base.ToString()} | Mês Pago: {_mesPago}, Zonas Autorizadas: {string.Join(", ", _zonasAutorizadas.Select(z => z.CodigoZona))}";
        }
    }
}
