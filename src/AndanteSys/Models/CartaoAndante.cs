using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public abstract class CartaoAndante
    {
        private Guid _idCartao;
        private DateTime _dataEmissao;
        private string _tipoCartao;
        private Pessoa _titular;

        public Guid IdCartao
        {
            get { return _idCartao; }
        }

        public DateTime DataEmissao
        {
            get { return _dataEmissao; }
        }

        public string TipoCartao
        {
            get { return _tipoCartao; }
            set
            {
                _tipoCartao = value.Trim();
                if(_tipoCartao.Length == 0)
                {
                    _tipoCartao = "AZUL";
                }
            }
        }

        public Pessoa Titular
        {
            get { return _titular; }
            set
            {
                _titular = value;
            }
        }

        protected CartaoAndante()
        {
            _idCartao = Guid.NewGuid();
            _dataEmissao = DateTime.Now;
            _tipoCartao = "";
            _titular = new Pessoa();
        }

        public abstract bool ValidarViagem(Zona zonaEstacaoAtual);

        public override string ToString()
        {
            string infoTitular;
            if (_titular != null)
            {
                infoTitular = _titular.Nome;
            }
            else
            {
                infoTitular = "Anónimo";
            }

            return $"Cartão ID: {_idCartao}, Tipo: {_tipoCartao}, Emitido: {_dataEmissao:dd/MM/yyyy}, Titular: {infoTitular}";
        }
    }
}
