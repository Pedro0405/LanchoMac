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

        public IActionResult Index()
        {

            var status = _lanchesContexto.Status.FirstOrDefault(i => i.Id == 1);
            if (status != null)
            {
                ViewBag.Status = status.StatusLoja;
            }
            return View();
        }
        [HttpPost]
        public IActionResult AbrirLoja()
        {
            AtualizarStatus(Status.StoreStatus.Aberto);
            return RedirectToAction("Index", new { status = Status.StoreStatus.Aberto });
        }

        [HttpPost]
        public IActionResult FecharLoja()
        {
            AtualizarStatus(Status.StoreStatus.Fechado);
            return RedirectToAction("Index", new { status = Status.StoreStatus.Fechado });
        }

        private void AtualizarStatus(Status.StoreStatus novoStatus)
        {
            var status = _lanchesContexto.Status.FirstOrDefault(s => s.Id == 1);

            if (status == null)
            {
                status = new Status
                {
                    Id = 1,
                    StatusLoja = novoStatus
                };

                _lanchesContexto.Status.Add(status);
            }
            else
            {
                status.StatusLoja = novoStatus;
            }

            _lanchesContexto.SaveChanges();
        }



    }
}
