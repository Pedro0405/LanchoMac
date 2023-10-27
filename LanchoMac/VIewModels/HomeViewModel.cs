using LanchoMac.Models;

namespace LanchoMac.VIewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Lanche> LanchesPreferidos { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }

    }
}
