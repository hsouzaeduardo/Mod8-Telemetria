namespace UIPedidos.Models
{
    public class Pedidos
    {
        public Pedidos()
        {
            IdPedido = Guid.NewGuid();
        }
        public Guid IdPedido { get; private set; }
        public DateTime DataPedido { get; set; }
        public string Item { get; set; }
        public int Quantidade { get; set; }
        public float Preco { get; set; }
        public float TotalPedido { get { return Preco * Quantidade; } }
        public string Cep { get; set; }
        public string Complemento { get; set; }


    }
}
