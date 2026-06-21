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

        public override bool ValidarViagem(Zona zonaEstacaoAtual)
        {

            // tem de ter saldo e a estação atual tem de ser da zona contratada
            if (_saldoViagens > 0 && zonaEstacaoAtual != null && zonaEstacaoAtual.CodigoZona == _zonaContratada)
            {
                // descontar 1 viagem
                _saldoViagens--;
                return true;
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
