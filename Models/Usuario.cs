namespace TicketsApi.Models;

public class Usuario
{
    public int Id { get; set; }
    
    public string Nombre { get; set; } = string.Empty;
    
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}