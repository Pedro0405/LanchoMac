using LanchoMac.Data;
using LanchoMac.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchoMac.Areas.Admin.Controllers
{
    [Authorize("Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly LanchesContexto _lanchesContexto;

        public AdminController(LanchesContexto lanchesContexto)
        {
            _lanchesContexto = lanchesContexto;
        }

        public IActionResult Index(Status.StoreStatus? status)
        {
            if (status.HasValue)
            {
                ViewData["Status"] = status;
            }
            return View();
        }
            [HttpPost]
            public IActionResult AbrirLoja()
            {
                // Verifique se já existe um registro com Id igual a 1
                var status = _lanchesContexto.Status.FirstOrDefault(s => s.Id == 1);

                if (status == null)
                {
                    // Se não existir, crie um novo registro
                    status = new Status
                    {
                        Id = 1,
                        StatusLoja = Status.StoreStatus.Aberto
                    };

                    _lanchesContexto.Status.Add(status); // Adicione o novo registro ao contexto
                }
                else
                {
                    // Se já existe, simplesmente atualize o status para "Aberto"
                    status.StatusLoja = Status.StoreStatus.Aberto;
                }

                _lanchesContexto.SaveChanges(); // Salve as alterações no banco de dados

            var novoStatus = _lanchesContexto.Status.FirstOrDefault(s => s.Id == 1);

            if (novoStatus != null)
            {
                return RedirectToAction("Index", new { status = novoStatus.StatusLoja });
            }

            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult FecharLoja()
        {
            // Verifique se já existe um registro com Id igual a 1
            var status = _lanchesContexto.Status.FirstOrDefault(s => s.Id == 1);

            if (status == null)
            {
                // Se não existir, crie um novo registro
                status = new Status
                {
                    Id = 1,
                    StatusLoja = Status.StoreStatus.Fechado
                };

                _lanchesContexto.Status.Add(status); // Adicione o novo registro ao contexto
            }
            else
            {
                // Se já existe, atualize o status para "Fechado"
                status.StatusLoja = Status.StoreStatus.Fechado;
            }

            _lanchesContexto.SaveChanges(); // Salve as alterações no banco de dados

            return RedirectToAction("Index"); // Redirecione para a página desejada após a alteração.
        }
        [HttpPost]
        public IActionResult ToggleStatus(string statusAction)
        {
            if (statusAction == "AbrirLoja")
            {
                return RedirectToAction("AbrirLoja");
            }
            else if (statusAction == "FecharLoja")
            {
                return RedirectToAction("FecharLoja");
            }
            return View();
        }


    }
}
