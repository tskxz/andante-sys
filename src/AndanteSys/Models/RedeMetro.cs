using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public class RedeMetro
    {
        private string _nomeRede;
        private List<Linha> _lstLinha;

        public string NomeRede
        {
            get { return _nomeRede; }
            set { _nomeRede = value.Trim(); }
        }

        public List<Linha> LstLinha
        {
            get { return _lstLinha; }
        }

        public RedeMetro()
        {
            _nomeRede = "Rede Desconhecida";
            _lstLinha = new List<Linha>();
        }

        public void AddLinha(Linha l)
        {
            if (l != null && !_lstLinha.Contains(l))
            {
                _lstLinha.Add(l);
            }
        }

        public override string ToString()
        {
            return $"Rede: {_nomeRede} | Operando com {_lstLinha.Count} linha(s).";
        }
    }
}
