using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using LanchoMac.VIewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LanchoMac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILancheRepository _Lancherepository;
        private readonly ICategotiaRepository _categotiaRepository;
        public HomeController(ILancheRepository lancherepository, ICategotiaRepository categotiaRepository)
        {
            _Lancherepository = lancherepository;
            _categotiaRepository = categotiaRepository;
        }

        public IActionResult Index()
        {
            var categorias = _categotiaRepository.categorias;
            var HomeVM = new HomeViewModel
            {
                LanchesPreferidos = _Lancherepository.LanchesPreferidos,
                Categorias = categorias
            };

            return View(HomeVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}