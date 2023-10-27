using LanchoMac.Models;

namespace LanchoMac.Repositories.Interfaces
{
    public interface ICategotiaRepository
    {
        IEnumerable<Categoria> categorias { get; }

    }
}
