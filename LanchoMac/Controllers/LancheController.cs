using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories;
using LanchoMac.Repositories.Interfaces;
using LanchoMac.VIewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchoMac.Controllers
{
    public class LancheController : Controller
    {
        private readonly ILancheRepository _lanchesRepository;
        private readonly ICategotiaRepository _categotiaRepository;
        private readonly LanchesContexto _lanchesContexto;
        public LancheController(ILancheRepository lanchesRepository, ICategotiaRepository categotiaRepository, LanchesContexto lanchesContexto)
        {
            _lanchesRepository = lanchesRepository;
            _categotiaRepository = categotiaRepository;
            _lanchesContexto = lanchesContexto;
        }
        public IActionResult List(string categoria)
        {
            var stauts = _lanchesContexto.Status.FirstOrDefault(i => i.Id == 1);
            ViewBag.StatusLoja = stauts.StatusLoja;
            var categorias = _categotiaRepository.categorias;
            IEnumerable<Lanche> lanches;
            string CategoriaAtual = string.Empty;
            if (string.IsNullOrEmpty(categoria))
            {
                lanches = _lanchesRepository.lanches.OrderBy(l => l.lancheId);
                CategoriaAtual = "Todos os laches";
            }
            else
            {
                // if(string.Equals("Normal", categoria,StringComparison.OrdinalIgnoreCase))
                //{
                //   lanches = _lanchesRepository.lanches.Where(l => l.Categoria.CategoriaNome.Equals("Normal")).OrderBy(l => l.Nome);
                //}
                //else
                //{
                //   lanches = _lanchesRepository.lanches.Where(l => l.Categoria.CategoriaNome.Equals("Natural")).OrderBy(l => l.Nome);
                //}
                lanches = _lanchesRepository.lanches.Where(l => l.Categoria.CategoriaNome.Equals(categoria)).OrderBy(l => l.Nome);
                CategoriaAtual = categoria;
            }
            var lanchelistViewNodel = new LancheListViewModel
            {
                lanches = lanches,
                categoriaAtual = CategoriaAtual,
                Categorias = categorias
            };
           
            return View(lanchelistViewNodel);
        }
        public IActionResult Details(int lancheid)
        {
            var lanche = _lanchesRepository.lanches.FirstOrDefault(l => l.lancheId == lancheid);
            return View(lanche);
        }
        public ViewResult Search(string searchString)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(searchString))
            {
                lanches = _lanchesRepository.lanches.OrderBy(p => p.lancheId);
                categoriaAtual = "Todos os Lanches";
            }
            else
            {
                lanches = _lanchesRepository.lanches.Where(p => p.Nome.ToLower().Contains(searchString.ToLower()));

                if (lanches.Any())
                    categoriaAtual = "Lanches";
                else
                    categoriaAtual = "Nenhum lanche foi encontrado";
            }

            return View("~/Views/Lanche/List.cshtml", new LancheListViewModel
            {
                lanches = lanches,
                categoriaAtual = categoriaAtual
            });
        }
    }
} 
