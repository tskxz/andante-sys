using AndanteSys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Helpers
{
    public class PessoaHelper
    {
        public void Insert(Pessoa pessoa)
        {
            if (pessoa != null)
            {
                App.lstPessoas.Add(pessoa);
            }
        }

        public void Atualizar(Pessoa pessoa)
        {
            var pessoaExistente = App.lstPessoas.FirstOrDefault(p => p.IdPessoa == pessoa.IdPessoa);
            if (pessoaExistente != null)
            {
                pessoaExistente.Nome = pessoa.Nome;
                pessoaExistente.NIF = pessoa.NIF;
            }
        }

        public void Apagar(Pessoa pessoa)
        {
            var pessoaExistente = App.lstPessoas.FirstOrDefault(p => p.IdPessoa == pessoa.IdPessoa);
            if (pessoaExistente != null)
            {
                App.lstPessoas.Remove(pessoaExistente);
            }
        }

        public List<Pessoa> ListarTodas()
        {
            return App.lstPessoas;
        }
    }
}
