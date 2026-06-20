using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public class RegistoValidacao
    {
        private Guid _idValidacao;
        private DateTime _dataValidacao;
        private bool _sucesso;
        private string _zonaValidada;
        private CartaoAndante _cartao;

        public Guid IdValidacao
        {
            get { return _idValidacao; }
        }

        public DateTime DataValidacao
        {
            get { return _dataValidacao; }
        }

        public bool Sucesso
        {
            get { return _sucesso; }
            set { _sucesso = value; }
        }

        public string ZonaValidada
        {
            get { return _zonaValidada; }
            set { _zonaValidada = value.Trim().ToUpper(); }
        }

        public CartaoAndante Cartao
        {
            get { return _cartao; }
            set { _cartao = value; }
        }

        public RegistoValidacao()
        {
            _idValidacao = Guid.NewGuid();
            _dataValidacao = DateTime.Now;
            _sucesso = false;
            _zonaValidada = "???";
            _cartao = null;
        }

        // Um ToString poderoso que extrai TODA a informação navegando pelos objetos
        public override string ToString()
        {
            string status = _sucesso ? "SUCESSO" : "RECUSADO";

            // Descobre o dono do cartão se ele existir (Gold vs Azul)
            string nomePassageiro = (_cartao != null && _cartao.Titular != null)
                                    ? _cartao.Titular.Nome
                                    : "Anónimo";

            string tipoCartao = (_cartao != null) ? _cartao.TipoCartao : "Desconhecido";
            string idCartaoCurto = (_cartao != null) ? _cartao.IdCartao.ToString().Substring(0, 8).ToUpper() : "N/A";

            return $"[{_dataValidacao:dd/MM/yyyy HH:mm:ss}] Registo: {_idValidacao.ToString().Substring(0, 5)} | " +
                   $"Resultado: {status} | Zona: {_zonaValidada} | " +
                   $"Passageiro: {nomePassageiro} ({tipoCartao} - {idCartaoCurto})";
        }
    }
}
