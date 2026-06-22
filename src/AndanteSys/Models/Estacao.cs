using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AndanteSys.Models
{
    public class Estacao
    {
        private string _nomeEstacao;
        private Zona _zona;
        private Validador _validador;

        public string NomeEstacao
        {
            get { return _nomeEstacao; }
            set { _nomeEstacao = value.Trim(); }
        }

        public Zona Zona
        {
            get { return _zona; }
            set { _zona = value; }
        }

        public Validador Validador
        {
            get { return _validador; }
            set { _validador = value; }
        }

        public Estacao()
        {
            _nomeEstacao = "Estação Genérica";
            _zona = null;
            _validador = null;
        }


        public override string ToString()
        {
            string codZona = (_zona != null) ? _zona.CodigoZona : "Nenhuma";
            string infoValidador = (_validador != null) ? "1 Validador" : "0 Validadores";
            return $"Estação: {_nomeEstacao} [Zona: {codZona}] - {infoValidador}";
        }
    }
}
