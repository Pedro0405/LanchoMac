using LanchoMac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LanchoMac.Components
{
    public class CategoriaMenu : ViewComponent
    {
        private readonly ICategotiaRepository _categotiaRepository;

        public CategoriaMenu(ICategotiaRepository categotiaRepository)
        {
            _categotiaRepository = categotiaRepository;
        }
        public IViewComponentResult Invoke()
        {
            var categorias = _categotiaRepository.categorias.OrderBy(l => l.CategoriaNome);
            return View(categorias);
        }
    }
}
