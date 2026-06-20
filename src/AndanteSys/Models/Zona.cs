using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public class Zona
    {
        private string _codigoZona;
        private string _nomeRegiao;
        private List<Estacao> _lstEstacao;

        public string CodigoZona
        {
            get { return _codigoZona; }
            set { _codigoZona = value.Trim().ToUpper(); }
        }

        public string NomeRegiao
        {
            get { return _nomeRegiao; }
            set { _nomeRegiao = value.Trim(); }
        }

        public List<Estacao> LstEstacao
        {
            get { return _lstEstacao; }
        }

        public Zona()
        {
            _codigoZona = "???";
            _nomeRegiao = "Sem Região";
            _lstEstacao = new List<Estacao>();
        }

        public void AddEstacao(Estacao e)
        {
            if (e != null && !_lstEstacao.Contains(e))
            {
                _lstEstacao.Add(e);
            }
        }

        public bool TemEstacao(Estacao e)
        {
            return _lstEstacao.Contains(e);
        }

        public override string ToString()
        {
            return $"Zona: {_codigoZona} ({_nomeRegiao}) - {_lstEstacao.Count} estações vinculadas";
        }
    }
}
