using LanchoMac.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace LanchoMac.Models
{
    public class CarrinhoCompra
    {
        private readonly LanchesContexto _contexto;

        public CarrinhoCompra(LanchesContexto contexto)
        {
            _contexto = contexto;
        }

        public string CarrinhoCompraId { get; set; }
      public List<CarrinhoCompraItem> carrinhoCompraItems { get; set; }

       public static CarrinhoCompra GetCarrinho(IServiceProvider services)
        {
            //defina uma sessão
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            //obtem um seriço do tipo do nosso contexto
            var context = services.GetService<LanchesContexto>();

            //Obtem ou gera o Id do carrinho
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

            //Atribui o id do carrinho na sessão
            session.SetString("CarrinhoId", carrinhoId);

            //Retorna o carrinho com o contexto e o Id
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };


        }
        public void AdcionarAoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _contexto.CarrinhoCompraItens.SingleOrDefault(x => x.Lanche.lancheId == lanche.lancheId && x.carrinhoCompraId == CarrinhoCompraId);
            if (carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    carrinhoCompraId = CarrinhoCompraId,
                    Lanche = lanche,
                    quantidade = 1

                };
                _contexto.CarrinhoCompraItens.Add(carrinhoCompraItem);

            }
            else
            {
                carrinhoCompraItem.quantidade++;
            }
            _contexto.SaveChanges();
        }

        public int RemoverDoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _contexto.CarrinhoCompraItens.SingleOrDefault(
                   s => s.Lanche.lancheId == lanche.lancheId &&
                   s.carrinhoCompraId == CarrinhoCompraId);

            var quantidadeLocal = 0;

            if (carrinhoCompraItem != null)
            {
                if (carrinhoCompraItem.quantidade > 1)
                {
                    carrinhoCompraItem.quantidade--;
                    quantidadeLocal = carrinhoCompraItem.quantidade;
                }
                else
                {
                    _contexto.CarrinhoCompraItens.Remove(carrinhoCompraItem);
                }
            }
            _contexto.SaveChanges();
            return quantidadeLocal;
        }

        public List<CarrinhoCompraItem> GetCarrinhoCompraItems()
        {
            return carrinhoCompraItems ?? (carrinhoCompraItems = _contexto.CarrinhoCompraItens.Where(c => c.carrinhoCompraId == CarrinhoCompraId).Include(s => s.Lanche).ToList());
        }

        public void LimparCarrinho()
        {
            var carrihoItens = _contexto.CarrinhoCompraItens.Where(s => s.carrinhoCompraId == CarrinhoCompraId);
            _contexto.CarrinhoCompraItens.RemoveRange(carrihoItens);
            _contexto.SaveChanges();
        }
        public decimal getCarrinhoCompraTotal()
        {
            var total = _contexto.CarrinhoCompraItens.Where(s => s.carrinhoCompraId == CarrinhoCompraId).Select(s => s.Lanche.Preco * s.quantidade).Sum();
            return total;
        }

    }
}
