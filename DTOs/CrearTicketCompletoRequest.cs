using System.ComponentModel.DataAnnotations;

namespace TicketsApi.DTOs;

public class CrearTicketCompletoRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;
    
    public int? UsuarioId { get; init; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(1000)]
    public string ComentarioInicial { get; init; } = string.Empty;
}