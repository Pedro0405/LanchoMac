using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

namespace LanchoMac.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly LanchesContexto _lanchesContexto;
        private readonly CarrinhoCompra _carrinhoCompra;
        private readonly UserManager<IdentityUser> _userManager;

        public PedidoController(CarrinhoCompra carrinhoCompra, IPedidoRepository pedidoRepository, UserManager<IdentityUser> userManager, LanchesContexto lanchesContexto)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
            _userManager = userManager;
            _lanchesContexto = lanchesContexto;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var useriD = _userManager.GetUserId(User);
            var pedidosSalvos = _lanchesContexto.Pedidos.FirstOrDefault(i => i.IdUser == useriD);
            Pedido pedido = pedidosSalvos;
            return View(pedido);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Pedido pedido)
        {
            var userId = _userManager.GetUserId(User); // Certifique-se de injetar o UserManager em seu controlador
  
            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;

            //obtem os itens do carrinho de compra do cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.carrinhoCompraItems = items;

            //verifica se existem itens de pedido
            if (_carrinhoCompra.carrinhoCompraItems.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho esta vazio, que tal incluir um lanche...");
            }

            //calcula o total de itens e o total do pedido
            foreach (var item in items)
            {
                totalItensPedido += item.quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.quantidade);
            }

            //atribui os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;
            pedido.IdUser = userId;
            //valida os dados do pedido
            if (ModelState.IsValid)
            {
                //cria o pedido e os detalhes
                _pedidoRepository.CriarPedido(pedido);

                //define mensagens ao cliente
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :)";
                ViewBag.TotalPedido = _carrinhoCompra.getCarrinhoCompraTotal();

                //limpa o carrinho do cliente
                _carrinhoCompra.LimparCarrinho();

                //exibe a view com dados do cliente e do pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);
        }
    }
}
