using System.ComponentModel.DataAnnotations;

namespace TicketsApi.DTOs;

public class CrearTicketRequest
{
    [Required]
    [MinLength(3)]
    public string Titulo { get; set; } = string.Empty;
}