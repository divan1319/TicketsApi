using Microsoft.AspNetCore.Mvc;
using TicketsApi.DTOs;
using TicketsApi.Services;

namespace TicketsApi.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ITicketWorkflowService _workflowService;

    public TicketsController(ITicketService ticketService,  ITicketWorkflowService workflowService)
    {
        _ticketService = ticketService;
        _workflowService = workflowService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos(
        [FromQuery] TicketQuery filtros,
        CancellationToken ct)
    {
        var tickets = await _ticketService.ObtenerTodosAsync(filtros,ct);

        return Ok(tickets);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var ticket = await _ticketService.ObtenerPorIdAsync(id,ct);
        
        if (ticket is null)
        {
            return NotFound();
        }

        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearTicketRequest request, CancellationToken ct)
    {
        var ticket = await _ticketService.CrearAsync(request.Titulo, ct);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = ticket.Id}, ticket);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarTicketRequest request,
        CancellationToken ct
    )
    {
        var ticket = await _ticketService.ActualizarAsync(
            id,
            request.Titulo,
            request.Resuelto,
            ct
        );

        if (ticket is null) return NotFound();

        return Ok(ticket);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(
        int id,
        CancellationToken ct
    )
    {
        var eliminado = await _ticketService.EliminarAsync(id, ct);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{id:int}/detalle")]
    public async Task<IActionResult> ObtenerDetalle(
        int id,
        CancellationToken ct
    )
    {
        var detalle = await _ticketService.ObtenerDetalleAsync(id, ct);
        if (detalle is null) return NotFound();
        return Ok(detalle);
    }

    [HttpPost("completos")]
    public async Task<IActionResult> CrearCompletos(
        [FromBody] CrearTicketCompletoRequest request,
        CancellationToken ct
    )
    {
        var res = await _workflowService.CrearConComentarioAsync(request, ct);

        if (res.Estado == EstadoCreacionTicket.UsuarioNoEncontrado)
        {
            return NotFound(
                new
                {
                    mensaje = "El usuario no existe"
                });
        }

        if (res.Estado == EstadoCreacionTicket.DatosInvalidos)
        {
            return BadRequest(new
            {
                mensaje = "Datos invalidos" 
            });
        }

        var id = res.TicketId!.Value;

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new {id},
            new { id });
    }
}