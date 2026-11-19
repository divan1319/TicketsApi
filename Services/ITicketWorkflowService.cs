using TicketsApi.DTOs;

namespace TicketsApi.Services;

public interface ITicketWorkflowService
{
    Task<ResultadoCreacionTicket> CrearConComentarioAsync(
        CrearTicketCompletoRequest request,
        CancellationToken ct = default
        );
}