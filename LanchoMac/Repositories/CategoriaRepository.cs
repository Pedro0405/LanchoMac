using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using System.Data.Common;

namespace LanchoMac.Repositories
{
    public class CategoriaRepository : ICategotiaRepository
    {
        private readonly LanchesContexto _contexto;
        public CategoriaRepository(LanchesContexto contexto) 
        {
            _contexto = contexto;
        }
        public IEnumerable<Categoria> categorias => _contexto.categorias;
    }
}
