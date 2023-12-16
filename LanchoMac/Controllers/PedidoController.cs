using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using LanchoMac.Services;
using LanchoMac.Services.AutomaticMessages;
using LanchoMac.VIewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReflectionIT.Mvc.Paging;

namespace LanchoMac.Controllers
{
  
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly LanchesContexto _lanchesContexto;
        private readonly CarrinhoCompra _carrinhoCompra;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISendMessage _sendMessage;

        public PedidoController(CarrinhoCompra carrinhoCompra, IPedidoRepository pedidoRepository, UserManager<IdentityUser> userManager, LanchesContexto lanchesContexto, ISendMessage sendMessage)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
            _userManager = userManager;
            _lanchesContexto = lanchesContexto;
            _sendMessage = sendMessage;

        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Checkout(Pedido pedido)
        {
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
                int IdDopedido = pedido.PedidoId;
                _sendMessage.sendMessage(IdDopedido);
                

                //exibe a view com dados do cliente e do pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);
        }

    }
}
