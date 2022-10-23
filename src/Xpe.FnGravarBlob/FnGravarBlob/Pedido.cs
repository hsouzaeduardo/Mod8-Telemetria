using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FnGravarBlob
{
    public class Pedido
    {
        public Guid IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public string Item { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public decimal TotalPedido { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
    }
}
