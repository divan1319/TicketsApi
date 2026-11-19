using Microsoft.EntityFrameworkCore;
using TicketsApi.Data;
using TicketsApi.DTOs;
using TicketsApi.Models;

namespace TicketsApi.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _dbContext;

    public TicketService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<TicketResponse>> ObtenerTodosAsync(
        TicketQuery filtros,
        CancellationToken ct = default
    )
    {
        //1. Construir la consulta base
        IQueryable<Ticket> consulta = _dbContext.Tickets.AsNoTracking();

        //2. Filtrar por estado
        if(filtros.Resuelto is bool resuelto){
            consulta = consulta.Where(
                t=> t.Resuelto == resuelto
            );
        }

        //3. Buscar por titulo
        var busqueda = filtros.Buscar?.Trim();

        if(!string.IsNullOrWhiteSpace(busqueda)){
            var texto = busqueda
                .Replace("\\","\\\\")
                .Replace("%","\\%")
                .Replace("_","\\_");

            var patron = $"%{texto}%";

            consulta = consulta.Where(t => EF.Functions.ILike(t.Titulo, patron, "\\"));

        }

        //4. Contar registros filtrados

        var total = await consulta.CountAsync(ct);

        //5. Ordenamieno dinamico
        consulta = filtros.Orden switch{
            "id_asc" => consulta.OrderBy(t => t.Id),
            "titulo_asc" => consulta.OrderBy(t => t.Titulo).ThenBy(t => t.Id),
            "titulo_desc" => consulta.OrderByDescending(t => t.Titulo).ThenBy(t => t.Id),

            _ => consulta.OrderByDescending(t => t.Id),
        };

        //6. Calcular paginacion
        var saltar = (filtros.Pagina -1 ) * filtros.PorPagina;

        // 7. Proyectar y ejecutar
        var tickets = await consulta
            .Skip(saltar)
            .Take(filtros.PorPagina)
            .Select(t=> new TicketResponse(
                t.Id,
                t.Titulo,
                t.Resuelto
            ))
            .ToListAsync(ct);

        //8. Construir respuesta
        return new PagedResult<TicketResponse>(
            tickets,
            filtros.Pagina,
            filtros.PorPagina,
            total
        );
    }

    public async Task<Ticket?> ObtenerPorIdAsync(
        int id,
        CancellationToken ct= default
    )
    {
        return await _dbContext.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);
    } 

    public async Task<Ticket> CrearAsync(
        string titulo,
        CancellationToken ct= default
    )
    {
        var ticket = new Ticket
        {
            Titulo = titulo,
            Resuelto = false
        };

        _dbContext.Tickets.Add(ticket);

        await _dbContext.SaveChangesAsync(ct);

        return ticket;
    }

    public async Task<Ticket?> ActualizarAsync(
        int id,
        string titulo,
        bool resuelto,
        CancellationToken ct = default
    )
    {
        var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);

        if(ticket is null) return null;

        ticket.Titulo = titulo;
        ticket.Resuelto = resuelto;

        await _dbContext.SaveChangesAsync(ct);

        return ticket;
    }

    public async Task<bool> EliminarAsync(
        int id,
        CancellationToken ct = default
    )
    {
        var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);

        if(ticket is null) return false;

        _dbContext.Tickets.Remove(ticket);

        await _dbContext.SaveChangesAsync(ct);

        return true;
    }

    public async Task<TicketDetalleResponse?> ObtenerDetalleAsync(
        int id,
        CancellationToken ct = default)
    {
        var ticket = await _dbContext.Tickets
            .AsNoTracking()
            .Include(t => t.Usuario)
            .Include(t => t.Comentarios)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
        
        if(ticket is null) return null;

        return new TicketDetalleResponse(
            ticket.Id,
            ticket.Titulo,
            ticket.Resuelto,
            ticket.Usuario?.Nombre,
            ticket.Comentarios
                .OrderBy(c => c.Id)
                .Select(c => new ComentarioResponse(
                    c.Id, c.Texto
                    )
                ).ToList()
            );
    }
}