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
        private List<Validador> _lstValidador;

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

        public List<Validador> LstValidador
        {
            get { return _lstValidador; }
        }

        public Estacao()
        {
            _nomeEstacao = "Estação Genérica";
            _zona = null;
            _lstValidador = new List<Validador>();
        }

        public void AddValidador(Validador v)
        {
            if (v != null && !_lstValidador.Contains(v))
            {
                _lstValidador.Add(v);
            }
        }

        public override string ToString()
        {
            string codZona = (_zona != null) ? _zona.CodigoZona : "Nenhuma";
            return $"Estação: {_nomeEstacao} [Zona: {codZona}] - {_lstValidador.Count} Validadores";
        }
    }
}
