using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public class Linha
    {
        private char _letraLinha;
        private string _cor;
        private List<Estacao> _lstEstacao;

        public char LetraLinha
        {
            get { return _letraLinha; }
            set { _letraLinha = value; }
        }

        public string Cor
        {
            get { return _cor; }
            set { _cor = value.Trim(); }
        }

        public List<Estacao> LstEstacao
        {
            get { return _lstEstacao; }
        }

        public Linha()
        {
            _letraLinha = '?';
            _cor = "Transparente";
            _lstEstacao = new List<Estacao>();
        }

        public void AddEstacao(Estacao e)
        {
            if (e != null && !_lstEstacao.Contains(e))
            {
                _lstEstacao.Add(e);
            }
        }

        public override string ToString()
        {
            return $"Linha {_letraLinha} ({_cor}) - {_lstEstacao.Count} estações na rota.";
        }
    }
}
