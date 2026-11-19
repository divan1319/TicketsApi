namespace TicketsApi.DTOs;

public record TicketDetalleResponse(
    int Id,
    string Titulo,
    bool Resuelto,
    string? Usuario,
    IReadOnlyList<ComentarioResponse> Comentarios
    );