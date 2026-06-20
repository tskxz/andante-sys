using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Models
{
    public class Pessoa
    {
        private Guid _idPessoa;
        private string _nif;
        private string _nome;


        public Guid IdPessoa
        {
            get {  return _idPessoa; }
        }

        public string NIF
        {
            get { return _nif; }
            set
            {
                _nif = value.Trim();
                if(_nif.Length == 0)
                {
                    _nif = "#########";
                }
            }
        }

        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value.Trim();
                if (_nome.Length == 0)
                {
                    _nome = "Anónimo";
                }
            }
        }

        public Pessoa()
        {
            _idPessoa = Guid.NewGuid();
            _nif = "";
            _nome = "";
        }

        public override string ToString()
        {
            string saida = $"ID: {_idPessoa}, NIF: {_nif}, Nome: {_nome}";
            return saida;
        }

        
    }
}
