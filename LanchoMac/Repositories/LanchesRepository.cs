using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LanchoMac.Repositories
{
    public class LanchesRepository : ILancheRepository
    {
        private readonly LanchesContexto _contexto;
        public LanchesRepository(LanchesContexto contexto) 
        {
            _contexto = contexto;
        }

        public IEnumerable<Lanche> lanches => _contexto.lanches.Include(c => c.Categoria);

        public IEnumerable<Lanche> LanchesPreferidos => _contexto.lanches.Where(l => l.IsLacnhePreferido).Include(c => c.Categoria);

        public Lanche GetlancheByID(int lancheId) 
        {
            return _contexto.lanches.FirstOrDefault(i => i.lancheId == lancheId); 
        }
    }
}
