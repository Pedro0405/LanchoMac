using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;

namespace LanchoMac.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly LanchesContexto _contexto;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoRepository(CarrinhoCompra carrinhoCompra, LanchesContexto contexto)
        {
            _carrinhoCompra = carrinhoCompra;
            _contexto = contexto;
        }

        public void CriarPedido(Pedido pedido)
        {
            pedido.PedidoEnviado = DateTime.Now;
            _contexto.Pedidos.Add(pedido);
            _contexto.SaveChanges();

            var carrinhoCompraItens = _carrinhoCompra.carrinhoCompraItems;
            foreach(var itemCarrinho in carrinhoCompraItens)
            {
                var pedidoDetail = new PedidoDetalhe
                {
                    Quantidade = itemCarrinho.quantidade,
                    LancheId = itemCarrinho.Lanche.lancheId,
                    PedidoId = pedido.PedidoId,
                    Preco = itemCarrinho.Lanche.Preco
                };
                _contexto.PedidoDetalhes.Add(pedidoDetail);
            }
            _contexto.SaveChanges();
        }
    }
}
