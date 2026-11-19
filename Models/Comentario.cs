namespace TicketsApi.Models;

public class Comentario
{
    public int Id { get; set; }
    
    public string Texto  { get; set; } = string.Empty;
    
    public int TicketId { get; set; }

    public Ticket Ticket { get; set; } = null!;
}