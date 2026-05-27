using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Pedido : BaseEntity
    {
        public string ClienteNome { get; private set; } = string.Empty;
        public PedidoStatusEnum Status { get; private set; }
        public decimal ValorTotal { get; private set; }
        private readonly List<ItemPedido> _itens = [];
        public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

        private Pedido() : base() { }

        public Pedido(string clienteNome, List<ItemPedido> itens)
        {

            if(string.IsNullOrEmpty(clienteNome) || clienteNome.Length < 5 || clienteNome.Length > 50)
                throw new ArgumentException("O nome do cliente deve conter entre 5 e 50 caracteres.", nameof(clienteNome));

            if(!itens.Any())
                throw new ArgumentException("O pedido deve conter pelo menos um item.", nameof(itens));

            ClienteNome = clienteNome;
            Status = PedidoStatusEnum.Novo;
            _itens.AddRange(itens);
            ValorTotal = itens.Sum(i => i.PrecoUnitario * i.Quantidade);
        }

        public void AdicionarItem(ItemPedido item)
        {
            ValidarPedidoAberto();

            _itens.Add(item);
            RecalcularValorTotal();
            AtualizadoEm = DateTime.UtcNow;
        }
        public void RemoverItem(Guid itemId)
        {
            ValidarPedidoAberto();

            var item = _itens.FirstOrDefault(i => i.Id == itemId)
                ?? throw new ArgumentException("Item não encontrado no pedido.", nameof(itemId));

            _itens.Remove(item);
            RecalcularValorTotal();
            AtualizadoEm = DateTime.UtcNow;
        }
        public void AtualizarItem(Guid itemId, string produtoNome, int quantidade, decimal precoUnitario)
        {
            ValidarPedidoAberto();

            var itemExistente = _itens.FirstOrDefault(i => i.Id == itemId)
                ?? throw new ArgumentException("Item não encontrado no pedido.", nameof(itemId));

            itemExistente.AtualizarItemPedido(produtoNome, quantidade, precoUnitario);

            RecalcularValorTotal();
            AtualizadoEm = DateTime.UtcNow;
        }
        public void AtualizarStatus(PedidoStatusEnum novoStatus)
        {
            if (Status == PedidoStatusEnum.Pago)
                throw new InvalidOperationException("Não é possível alterar o status de um pedido pago.");

            Status = novoStatus;
            AtualizadoEm = DateTime.UtcNow;
        }
        private void RecalcularValorTotal()
        {
            ValorTotal = _itens.Sum(i => i.PrecoUnitario * i.Quantidade);
        }
        private void ValidarPedidoAberto()
        {
            if (Status == PedidoStatusEnum.Cancelado || Status == PedidoStatusEnum.Pago)
                throw new InvalidOperationException($"Não é possível alterar um pedido {Status}.");
        }
    }
}
