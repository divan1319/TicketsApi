namespace TicketsApi.DTOs;

public record TicketResponse(
    int Id, 
    string Titulo, 
    bool Resuelto
);