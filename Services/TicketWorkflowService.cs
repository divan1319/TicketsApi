using Microsoft.EntityFrameworkCore;
using TicketsApi.Data;
using TicketsApi.DTOs;
using TicketsApi.Models;

namespace TicketsApi.Services;

public class TicketWorkflowService(AppDbContext db) : ITicketWorkflowService
{
    public async Task<ResultadoCreacionTicket> CrearConComentarioAsync(CrearTicketCompletoRequest request, CancellationToken ct = default)
    {
        //1. Normalizar y validar
        var titulo = request.Titulo?.Trim() ?? "";
        var comentario = request.ComentarioInicial?.Trim() ?? "";

        if (titulo.Length is < 3 or > 200 || comentario.Length is < 3 or > 1000)
        {
            return new(
                EstadoCreacionTicket.DatosInvalidos
            );
        }
        
        // 2.Validar usuario si fue proporcionado
        if (request.UsuarioId is int usuarioId)
        {
            var existe = await db.Usuarios.AnyAsync(
                u => u.Id == usuarioId,
                ct
            );
            if (!existe)
            {
                return new(
                    EstadoCreacionTicket.UsuarioNoEncontrado
                );
            }
        }
        
        // 3. Iniciar transaccion
        await using var tx = await db.Database.BeginTransactionAsync(ct);
        
        // 4. Crear ticket
        var ticket = new Ticket
        {
            Titulo = titulo,
            UsuarioId = request.UsuarioId,
            Resuelto = false
        };

        ticket.Comentarios.Add(new Comentario
        {
            Texto = comentario
        });
        
        db.Tickets.Add(ticket);
        
        await db.SaveChangesAsync(ct);

        await tx.CommitAsync(ct);

        return new(
            EstadoCreacionTicket.Creado,
            ticket.Id
            );
    }
}