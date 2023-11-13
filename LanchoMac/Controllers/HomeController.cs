using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using LanchoMac.VIewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using OpenQA.Selenium.DevTools.V117.CSS;
using System.Diagnostics;

namespace LanchoMac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILancheRepository _Lancherepository;
        private readonly ICategotiaRepository _categotiaRepository;
        private readonly LanchesContexto _lanchesContexto;
        public HomeController(ILancheRepository lancherepository, ICategotiaRepository categotiaRepository, LanchesContexto lanchesContexto)
        {
            _Lancherepository = lancherepository;
            _categotiaRepository = categotiaRepository;
            _lanchesContexto = lanchesContexto;
        }

        public IActionResult Index()
        {
            var categorias = _categotiaRepository.categorias;
            var stauts = _lanchesContexto.Status.FirstOrDefault(i => i.Id == 1);
            if (stauts != null)
            {
                ViewBag.StatusLoja = stauts.StatusLoja;
            }
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