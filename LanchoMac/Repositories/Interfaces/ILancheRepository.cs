using LanchoMac.Models;

namespace LanchoMac.Repositories.Interfaces
{
    public interface ILancheRepository
    {
        IEnumerable<Lanche> lanches { get; }
        IEnumerable<Lanche> LanchesPreferidos { get; }
        Lanche GetlancheByID (int lancheId);

    }
}
