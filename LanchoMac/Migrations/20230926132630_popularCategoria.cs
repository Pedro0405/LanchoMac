﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanchoMac.Migrations
{
    /// <inheritdoc />
    public partial class popularCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorias(CategoriaNome, Descricao) VALUES ('Normal', 'Lanche com ingredientes normais')");
            migrationBuilder.Sql("INSERT INTO Categorias(CategoriaNome, Descricao) VALUES ('Natural','Lanche com ingredientes integrais e naturais')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorias");
        }
    }
}