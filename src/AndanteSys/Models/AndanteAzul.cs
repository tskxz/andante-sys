using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public class AndanteAzul : CartaoAndante
    {
        private int _saldoViagens;
        private string _zonaContratada;

        public int SaldoViagens
        {
            get { return _saldoViagens; }
            set {
                _saldoViagens = value;
                if(_saldoViagens < 0)
                {
                    _saldoViagens = 0;
                }
            }
        }

        public string ZonaContratada
        {
            get { return _zonaContratada; }
            set { _zonaContratada = value?.Trim().ToUpper(); }
        }

        public override bool ValidarViagem(Estacao estacaoAtual)
        {
            if (_saldoViagens > 0 && estacaoAtual != null)
            {
                // Se a estação é da zona contratada
                if (estacaoAtual.Zona.CodigoZona == _zonaContratada)
                {
                    _saldoViagens--;
                    return true;
                }

                // Se for zona de sobreposição, vai buscar a zona do contrato e vê se ela inclui esta estação
                var zonaDoContrato = App.lstZonas.FirstOrDefault(z => z.CodigoZona == _zonaContratada);
                if (zonaDoContrato != null && zonaDoContrato.TemEstacao(estacaoAtual))
                {
                    _saldoViagens--;
                    return true;
                }
            }
            return false;
        }

        public void CarregarViagens(int quantidade, string zona)
        {
            if (quantidade > 0)
            {
                _saldoViagens += quantidade;
                // use public property to normalize value
                ZonaContratada = zona;
            }
        }

        public AndanteAzul() : base()
        {
            _saldoViagens = 0;
            _zonaContratada = "PRT1";
            TipoCartao = "AZUL";
        }

        public override string ToString()
        {
            return $"{base.ToString()} | Saldo: {_saldoViagens} viagem(ns), Zona Fixa: {_zonaContratada}";
        }
    }
}
