using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using LanchoMac.VIewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchoMac.Controllers
{
   
    public class CarrinhoCompraController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(CarrinhoCompra carrinhoCompra, ILancheRepository lancheRepository)
        {
            _carrinhoCompra = carrinhoCompra;
            _lancheRepository = lancheRepository;
        }

     
        public IActionResult Index()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.carrinhoCompraItems = itens;
            var carrinhocompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.getCarrinhoCompraTotal()
            };
            return View(carrinhocompraVM);
        }
  
        public IActionResult AdcionarItemNoCarrinho(int lancheid)
        {
            var lancheselecionado = _lancheRepository.lanches.FirstOrDefault(l => l.lancheId == lancheid);
            if (lancheselecionado != null)
            {
                _carrinhoCompra.AdcionarAoCarrinho(lancheselecionado);

            }

           return RedirectToAction("Index");
        }
        
        public IActionResult RemoverItemDoCarrinho(int lancheid)
        {
            var lancheselecionado = _lancheRepository.lanches.FirstOrDefault(l => l.lancheId == lancheid);
            if (lancheselecionado != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(lancheselecionado);

            }

            return RedirectToAction("List", "Lanche");
        }
    }
}