using LanchoMac.Models;
using LanchoMac.VIewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchoMac.Components
{
    public class CarrinhoCompreResumo : ViewComponent
    {
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompreResumo(CarrinhoCompra carrinhoCompra)
        {
            _carrinhoCompra = carrinhoCompra;
        }

        public IViewComponentResult Invoke() 
        {
             var itens = _carrinhoCompra.GetCarrinhoCompraItems();
         //   var itens = new List<CarrinhoCompraItem>()
          //{
            //  new CarrinhoCompraItem(),
              //new CarrinhoCompraItem()
          //};
            _carrinhoCompra.carrinhoCompraItems = itens;
            var carrinhocompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.getCarrinhoCompraTotal()
            };
            return View("Default", carrinhocompraVM);

        }
    }
}
