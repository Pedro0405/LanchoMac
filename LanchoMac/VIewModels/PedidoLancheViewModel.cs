using LanchoMac.Models;

namespace LanchoMac.VIewModels
{
        public class PedidoLancheViewModel
        {
            public Pedido Pedido { get; set; }
            public IEnumerable<PedidoDetalhe> PedidoDetalhes { get; set; }
        }
}
