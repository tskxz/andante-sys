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

        public override string ToString()
        {
            string status;
            if (_sucesso)
            {
                status = "SUCESSO";
            }
            else
            {
                status = "RECUSADO";
            }

            string nomePassageiro;
            if (_cartao != null && _cartao.Titular != null)
            {
                nomePassageiro = _cartao.Titular.Nome;
            }
            else
            {
                nomePassageiro = "Anónimo";
            }

            string tipoCartao;
            if (_cartao != null)
            {
                tipoCartao = _cartao.TipoCartao;
            }
            else
            {
                tipoCartao = "Desconhecido";
            }

            string idCartaoCurto;
            if (_cartao != null)
            {
                idCartaoCurto = _cartao.IdCartao.ToString().Substring(0, 8).ToUpper();
            }
            else
            {
                idCartaoCurto = "N/A";
            }

            return $"[{_dataValidacao:dd/MM/yyyy HH:mm:ss}] Registo: {_idValidacao.ToString().Substring(0, 5)} | " +
                   $"Resultado: {status} | Zona: {_zonaValidada} | " +
                   $"Passageiro: {nomePassageiro} ({tipoCartao} - {idCartaoCurto})";
        }
    }
}
