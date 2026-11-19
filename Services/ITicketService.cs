using TicketsApi.DTOs;
using TicketsApi.Models;

namespace TicketsApi.Services;

public interface ITicketService
{
    Task< PagedResult<TicketResponse>> ObtenerTodosAsync(
        TicketQuery query,
        CancellationToken ct = default
    );
    Task<Ticket?> ObtenerPorIdAsync(int id,CancellationToken ct = default);
    Task<Ticket> CrearAsync(string titulo,CancellationToken ct = default);

    Task<Ticket?> ActualizarAsync(int id, string titulo, bool resuelto, CancellationToken ct = default);

    Task<bool> EliminarAsync(int id, CancellationToken ct = default);

    Task<TicketDetalleResponse?> ObtenerDetalleAsync(
        int id,
        CancellationToken ct = default
        );
}