using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanchoMac.Migrations
{
    /// <inheritdoc />
    public partial class PopularLanches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Lanches(CategoriaId, Nome, DescricaoCurta, DescricaoLonga, Preco, ImagemUrl, ImagemTumbnailUrl, IsLacnhePreferido, EmEstoque) VALUES ('2','Sanduiche Natural', 'Pão integral, Queijo branco Peito de peru', 'Um delicioso sanduiche natural e saudavel', '11.50','C:\\Users\\usuário\\Documents\\C#\\LanchoMac\\LanchoMac\\wwwroot\\imgLanches\\SanduicheNatural.jpg','C:\\Users\\usuário\\Documents\\C#\\LanchoMac\\LanchoMac\\wwwroot\\imgLanches\\SanduicheNatural.jpg','1','1')");
            migrationBuilder.Sql("INSERT INTO Lanches(CategoriaId, Nome, DescricaoCurta, DescricaoLonga, Preco, ImagemUrl, ImagemTumbnailUrl, IsLacnhePreferido, EmEstoque) VALUES ('1','XTudo','Pão,Carne de Hamburguer, Molho, queijo, OVo, Presunto, Salada, Bacon','Carro chefe da casa, Excelente lanhce para encher a barriga','12.00','C:\\Users\\usuário\\Documents\\C#\\LanchoMac\\LanchoMac\\wwwroot\\imgLanches\\xtudo.jpg','C:\\Users\\usuário\\Documents\\C#\\LanchoMac\\LanchoMac\\wwwroot\\imgLanches\\xtudo.jpg','1','1')");
            migrationBuilder.Sql("INSERT INTO Lanches(CategoriaId, Nome, DescricaoCurta, DescricaoLonga, Preco, ImagemUrl, ImagemTumbnailUrl, IsLacnhePreferido, EmEstoque) VALUES ('1','XSalada', 'Pão, Carne de Hamburguer, Saladaa completa, molho, queijo','Muito delicioso, barato e ainda mais saudavel','10.50','C:\\Users\\usuário\\Documents\\C#\\LanchoMac\\LanchoMac\\wwwroot\\imgLanches\\Xsalada.jpg','C:\\Users\\usuário\\Documents\\C#\\LanchoMac\\LanchoMac\\wwwroot\\imgLanches\\Xsalada.jpg','0','1')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Lanches");
        }
    }
}
