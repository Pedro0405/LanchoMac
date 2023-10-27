using LanchoMac.Models;

namespace LanchoMac.VIewModels
{
    public class LancheListViewModel
    {
        public IEnumerable<Lanche> lanches { get; set; }
        public string categoriaAtual { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }
    }
}
