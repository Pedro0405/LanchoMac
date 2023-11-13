using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.VIewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Permissions;

namespace LanchoMac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminPedidosController : Controller
    {

        private readonly LanchesContexto _context;

        public AdminPedidosController(LanchesContexto context)
        {
            _context = context;
        }

        //Exibir items de pedido
        public IActionResult PedidoLanches(int id)
        {
            var pedido = _context.Pedidos.Include(pd => pd.PedidoItens).ThenInclude(l => l.Lanche).FirstOrDefault(I => I.PedidoId == id);
            if (pedido == null)
            {
                Response.StatusCode = 404;
                return View("PedidoNotFound", id);
            }
            PedidoLancheViewModel pedidoLanches = new PedidoLancheViewModel()
            {
                Pedido = pedido,
                PedidoDetalhes = pedido.PedidoItens
            };
            return View(pedidoLanches);
        }



        // GET: Admin/AdminPedidos
      public async Task<IActionResult> Index(string filter, string dateFilter, int pageindex = 1,  string sort = "PedidoEnviado")
        {
            // Obtem os valores da tabela e coloca em uma variável de consulta
            var resultado = _context.Pedidos.AsNoTracking().AsQueryable();

            // Verifica se algo foi pesquisado na caixa de pesquisa e adiciona à consulta, se houver
            if (!string.IsNullOrEmpty(filter))
            {
                resultado = resultado.Where(p => p.Nome.Contains(filter));
            }
            // Adicione a verificação para filtrar por data
            if (!string.IsNullOrEmpty(dateFilter) && DateTime.TryParse(dateFilter, out var selectedDate))
            {
                resultado = resultado.Where(p => p.PedidoEnviado.Date == selectedDate.Date);
            }


            // Cria um modelo de PagingList com a consulta já ordenada
            var model = await PagingList.CreateAsync(resultado, 8, pageindex, sort, "PedidoEnviado");

            // Cria a rota da pesquisa que será usada na view
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            // Passa o modelo para a view
            return View(model);
        }






            // GET: Admin/AdminPedidos/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Admin/AdminPedidos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PedidoId,Nome,Sobrenome,Endereco1,Endereco2,Cep,Estado,Cidade,Telefone,Email,PedidoEnviado,PedidoEntregueEm")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedido);
        }

        // GET: Admin/AdminPedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        // POST: Admin/AdminPedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PedidoId,Nome,Sobrenome,Endereco1,Endereco2,Cep,Estado,Cidade,Telefone,Email,PedidoEnviado,PedidoEntregueEm")] Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.PedidoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pedido);
        }

        // GET: Admin/AdminPedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Admin/AdminPedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.PedidoId == id);
        }
        public async Task<IActionResult> MarcarPedidoComoEntregue(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            // Define o atributo "PedidoEntregueEm" como a data atual
            pedido.PedidoEntregueEm = DateTime.Now;

            // Atualiza o pedido no banco de dados
            _context.Update(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
