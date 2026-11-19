using System.ComponentModel.DataAnnotations;

namespace TicketsApi.DTOs;

public class TicketQuery{

    public bool? Resuelto { get; set; }

    [MaxLength(100)]
    public string? Buscar { get; set; }

    [Range(1, int.MaxValue)]
    public int Pagina { get; set; } = 1;

    [Range(1, 100)]
    public int PorPagina { get; set; } = 10;

    public string Orden { get; set; } = "id_desc";
}