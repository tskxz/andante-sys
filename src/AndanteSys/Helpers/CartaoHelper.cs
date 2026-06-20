using AndanteSys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndanteSys.Helpers
{
    public class CartaoHelper
    {
        public void Insert(CartaoAndante cartao)
        {
            if (cartao != null)
            {
                App.lstCartoes.Add(cartao);
            }
        }

        public void Apagar(CartaoAndante cartao)
        {
            var cartaoExistente = App.lstCartoes.FirstOrDefault(c => c.IdCartao == cartao.IdCartao);
            if (cartaoExistente != null)
            {
                App.lstCartoes.Remove(cartaoExistente);
            }
        }

        public List<CartaoAndante> ListarTodos()
        {
            return App.lstCartoes;
        }
    }
}
