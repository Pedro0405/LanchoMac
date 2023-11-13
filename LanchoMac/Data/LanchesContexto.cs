using LanchoMac.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LanchoMac.Data
{
    public class LanchesContexto : IdentityDbContext<IdentityUser>
    {
public LanchesContexto(DbContextOptions<LanchesContexto> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Lanche> lanches { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }
        public DbSet<Status> Status { get; set; }


    }
}
