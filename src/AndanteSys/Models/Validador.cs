using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public class Validador
    {
        private Guid _idValidador;
        private bool _statusAtivo;
        private Estacao _estacao;

        public static List<RegistoValidacao> HistoricoGlobal = new List<RegistoValidacao>();
        public Guid IdValidador
        {
            get { return _idValidador; }
        }

        public bool StatusAtivo
        {
            get { return _statusAtivo; }
            set { _statusAtivo = value; }
        }

        public Estacao Estacao
        {
            get { return _estacao; }
            set { _estacao = value; }
        }

        public Validador()
        {
            _idValidador = Guid.NewGuid();
            _statusAtivo = true;
            _estacao = null;
        }

        public bool ProcessarLeitura(CartaoAndante c)
        {
            if (!_statusAtivo || c == null || _estacao == null || _estacao.Zona == null)
                return false;

            bool sucessoViagem = c.ValidarViagem(_estacao.Zona);

            RegistoValidacao novoRegisto = new RegistoValidacao();
            novoRegisto.Sucesso = sucessoViagem;
            novoRegisto.ZonaValidada = _estacao.Zona.CodigoZona;
            novoRegisto.Cartao = c;
            HistoricoGlobal.Add(novoRegisto);
            return sucessoViagem;
        }

        public override string ToString()
        {
            string nomeEst;
            if (_estacao != null)
            {
                nomeEst = _estacao.NomeEstacao;
            }
            else
            {
                nomeEst = "Armazém";
            }

            return $"Validador ID: {_idValidador}, Ativo: {_statusAtivo}, Local: {nomeEst}";
        }
    }
}
