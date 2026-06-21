using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AndanteSys.Models
{
    public class AndanteGold : CartaoAndante
    {
        private int _mesPago;
        private Zona _zonaAutorizada;

        public int MesPago
        {
            get { return _mesPago; }
            set
            {
                _mesPago = value;
                if (_mesPago < 1 || _mesPago > 12)
                {
                    _mesPago = 0;
                }
            }
        }

        public Zona ZonaAutorizada
        {
            get { return _zonaAutorizada; }
            set { _zonaAutorizada = value; }
        }

        public override bool ValidarViagem(Zona zonaEstacaoAtual)
        {
            int mesAtual = DateTime.Now.Month;
            // o mes do passe tem de estar atualizado e a zona da estação tem de estar autorizada
            if (_mesPago == mesAtual)
            {
                if (zonaEstacaoAtual != null)
                {
                    if (_zonaAutorizada != null && _zonaAutorizada.CodigoZona == zonaEstacaoAtual.CodigoZona)
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

            _zonaAutorizada = new Zona { CodigoZona = codigo };
        }

        public void RenovarAssinatura(int mes)
        {
            _mesPago = mes;
        }

        public AndanteGold()
        {
            _mesPago = 0;
            _zonaAutorizada = new Zona { CodigoZona = "PRT1" };
            TipoCartao = "GOLD";
        }

        public override string ToString()
        {
            string zona = _zonaAutorizada != null ? _zonaAutorizada.CodigoZona : "Nenhuma";
            return $"{base.ToString()} | Mês Pago: {_mesPago}, Zona Autorizada: {zona}";
        }
    }
}
