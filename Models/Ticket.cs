namespace TicketsApi.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Resuelto { get; set; }
    
    public int? UsuarioId { get; set; }
    
    public Usuario? Usuario { get; set; }
    
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    
}