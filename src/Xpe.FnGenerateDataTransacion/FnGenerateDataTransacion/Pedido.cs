using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FnGenerateDataTransacion
{
    public class Pedido
    {

        public string IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public string Item { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public decimal TotalPedido { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public Endereco Endereco { get; set; }
    }


    public class Endereco
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string ddd { get; set; }
        public string siafi { get; set; }
    }


}
