using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LanchoMac.Data;
using LanchoMac.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using ReflectionIT.Mvc.Paging;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminLanchesController : Controller
{

        private readonly LanchesContexto _context;
        private readonly IWebHostEnvironment _hostEnvironment;
      private readonly ConfigurationImagens _myConfig;

    public AdminLanchesController(LanchesContexto context, IWebHostEnvironment hostEnvironment, IOptions<ConfigurationImagens> myConfig)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
        _myConfig = myConfig.Value;
    }

    // GET: Admin/AdminLanches
    public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
    {
        //obtem os valores da tabela e por numa variavel de consulta
        var resultado = _context.lanches.AsNoTracking().AsQueryable();

        //verifica se algo foi pesquisa  na caixa de pesquisa e coloca na consulta caso tenha
        if (!string.IsNullOrEmpty(filter))
        {
            resultado = resultado.Where(p => p.Nome.Contains(filter));
        }
        //criacao de um modelo pagelist que vauter o resultado da cpnsilta, e quantos valores por paginas ecibe, alem de qual valor sera ordenado
        var model = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Nome");
        //cria a arota da pesquisa que sera usada na view
        model.RouteValue = new RouteValueDictionary { { "filter", filter } };
        //passa a model pra view
        return View(model);
    }

    // GET: Admin/AdminLanches/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lanche = await _context.lanches
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.lancheId == id);
            if (lanche == null)
            {
                return NotFound();
            }

            return View(lanche);
        }

        // GET: Admin/AdminLanches/Create
        public IActionResult Create()
        {
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string imagePath = Path.Combine(wwwRootPath, _myConfig.NomePastaImagensProdutos);
        string[] imageFiles = Directory.GetFiles(imagePath).Select(Path.GetFileName).ToArray();

        // Passe a lista de nomes de arquivos para a View
        ViewBag.ImageFilesList = new SelectList(imageFiles);
        ViewData["CategoriaId"] = new SelectList(_context.categorias, "CategoriaId", "CategoriaNome");
            return View();
        }

        // POST: Admin/AdminLanches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("lancheId,Nome,DescricaoCurta,DescricaoLonga,Preco,ImagemUrl,ImagemTumbnailUrl,IsLacnhePreferido,EmEstoque,CategoriaID")] Lanche lanche)
        {
        
        if (ModelState.IsValid)
            {
            if (!string.IsNullOrEmpty(lanche.ImagemUrl))
            {
                // Adicione o prefixo "/imgLanches/" ao nome da imagem
                lanche.ImagemUrl = "/imgLanches/" + lanche.ImagemUrl;
                lanche.ImagemTumbnailUrl = "/imgLanches/" + lanche.ImagemUrl;
            }

            _context.Add(lanche);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "CategoriaId", "CategoriaNome", lanche.CategoriaID);
            return View(lanche);
        }

        // GET: Admin/AdminLanches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string imagePath = Path.Combine(wwwRootPath, _myConfig.NomePastaImagensProdutos);
        string[] imageFiles = Directory.GetFiles(imagePath).Select(Path.GetFileName).ToArray();

        // Passe a lista de nomes de arquivos para a View
        ViewBag.ImageFilesList = new SelectList(imageFiles);
        var lanche = await _context.lanches.FindAsync(id);
            if (lanche == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "CategoriaId", "CategoriaNome", lanche.CategoriaID);
            return View(lanche);
        }

        // POST: Admin/AdminLanches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("lancheId,Nome,DescricaoCurta,DescricaoLonga,Preco,ImagemUrl,ImagemTumbnailUrl,IsLacnhePreferido,EmEstoque,CategoriaID")] Lanche lanche)
        {
            if (id != lanche.lancheId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                // Adicione o prefixo "/imgLanches/" ao nome da imagem
                lanche.ImagemUrl = "/imgLanches/" + lanche.ImagemUrl;
                lanche.ImagemTumbnailUrl = "/imgLanches/" + lanche.ImagemUrl;
                _context.Update(lanche);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LancheExists(lanche.lancheId))
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
            
            return View(lanche);
        }

        // GET: Admin/AdminLanches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lanche = await _context.lanches
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.lancheId == id);
            if (lanche == null)
            {
                return NotFound();
            }

            return View(lanche);
        }

        // POST: Admin/AdminLanches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lanche = await _context.lanches.FindAsync(id);
            _context.lanches.Remove(lanche);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LancheExists(int id)
        {
            return _context.lanches.Any(e => e.lancheId == id);
        }
    
}
