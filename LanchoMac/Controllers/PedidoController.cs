using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using LanchoMac.Services;
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

        public PedidoController(CarrinhoCompra carrinhoCompra, IPedidoRepository pedidoRepository, UserManager<IdentityUser> userManager, LanchesContexto lanchesContexto)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
            _userManager = userManager;
            _lanchesContexto = lanchesContexto;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var pedidoJson = Request.Cookies["PedidoCookie"];

            if (!string.IsNullOrEmpty(pedidoJson))
            {
                // Desserializa o JSON para obter o PedidoCookieViewModel
                PedidoCokieViewModel pedidoCokieViewModel = JsonConvert.DeserializeObject<PedidoCokieViewModel>(pedidoJson);

                // Agora você tem o objeto PedidoCookieViewModel para passar para a view
                return View(pedidoCokieViewModel);
            }

            // Se não houver cookie, crie uma nova instância de PedidoCookieViewModel ou lide com isso de acordo com a sua lógica
            PedidoCokieViewModel pedidoCookieViewModel = new PedidoCokieViewModel();
            return View(pedidoCookieViewModel);
        }

        [HttpPost]

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
                PedidoCokieViewModel pedidoCookieViewModel = new PedidoCokieViewModel
                {
                    PedidoId = pedido.PedidoId,
                    IdUser = pedido.IdUser,
                    Nome = pedido.Nome,
                    Sobrenome = pedido.Sobrenome,
                    Endereco1 = pedido.Endereco1,
                    Numero = pedido.Numero,
                    Endereco2 = pedido.Endereco2,
                    Cep = pedido.Cep,
                    Estado = pedido.Estado,
                    Cidade = pedido.Cidade,
                    Telefone = pedido.Telefone,
                    Email = pedido.Email,
                    FormaPagamento = pedido.FormaPagamento,
                    PedidoTotal = pedido.PedidoTotal,
                    TotalItensPedido = pedido.TotalItensPedido,
                    PedidoEnviado = pedido.PedidoEnviado,
                    PedidoEntregueEm = pedido.PedidoEntregueEm
                };

                // Serializa o PedidoCookieViewModel em JSON
                var pedidoJson = JsonConvert.SerializeObject(pedidoCookieViewModel);
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(17520) 
                };

                Response.Cookies.Append("MeuCookie", "MeusDados", cookieOptions);

                // Armazena o JSON em um cookie
                Response.Cookies.Append("PedidoCookie", pedidoJson);
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
