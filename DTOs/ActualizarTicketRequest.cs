using System.ComponentModel.DataAnnotations;

namespace TicketsApi.DTOs;


public class ActualizarTicketRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public bool Resuelto { get; set; }
}