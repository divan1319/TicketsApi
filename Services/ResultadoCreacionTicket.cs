namespace TicketsApi.Services;

public enum EstadoCreacionTicket
{
    Creado,
    UsuarioNoEncontrado,
    DatosInvalidos
}

public record ResultadoCreacionTicket(
    EstadoCreacionTicket Estado,
    int? TicketId = null
);