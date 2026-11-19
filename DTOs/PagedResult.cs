namespace TicketsApi.DTOs;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Pagina,
    int PorPagina,
    int Total
){
    public int TotalPaginas => (int)Math.Ceiling((double)Total / PorPagina);
}