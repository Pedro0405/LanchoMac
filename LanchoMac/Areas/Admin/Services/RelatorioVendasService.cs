﻿using LanchoMac.Data;
using LanchoMac.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LanchoMac.Areas.Admin.Services
{
    public class RelatorioVendasService
    {
        private readonly LanchesContexto context;
        public RelatorioVendasService(LanchesContexto _context)
        {
            context = _context;
        }

        public async Task<List<Pedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in context.Pedidos select obj;

            if (minDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado <= maxDate.Value);
            }

            return await resultado.Include(l => l.PedidoItens)
                         .ThenInclude(l => l.Lanche)
                         .OrderByDescending(x => x.PedidoEnviado)
                         .ToListAsync();
        }
    }
}   