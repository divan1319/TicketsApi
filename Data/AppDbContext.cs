using Microsoft.EntityFrameworkCore;
using TicketsApi.Models;

namespace TicketsApi.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options
) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
        )
    {
        modelBuilder.Entity<Usuario>(u =>
        {
            u.ToTable("usuarios");
            u.HasKey(x => x.Id);

            u.Property(x => x.Id)
                .HasColumnName("id");

            u.Property(x => x.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(120)
                .IsRequired();
        });

        modelBuilder.Entity<Ticket>(t =>
        {
            t.ToTable("tickets");
            t.HasKey(x => x.Id);
            
            t.Property(x => x.Id)
                .HasColumnName("id");

            t.Property(x => x.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(200)
                .IsRequired();

            t.Property(x => x.Resuelto)
                .HasColumnName("resuelto");
            
            t.Property(x => x.UsuarioId)
                .HasColumnName("usuario_id");

            t.HasOne(x => x.Usuario)
                .WithMany(u => u.Tickets)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Comentario>(c =>
        {
            c.ToTable("comentarios");
            c.HasKey(x => x.Id);
            
            c.Property(x => x.Id)
                .HasColumnName("id");
            
            c.Property(x => x.Texto)
                .HasColumnName("texto")
                .HasMaxLength(1000)
                .IsRequired();
            
            c.Property(x=> x.TicketId)
                .HasColumnName("ticket_id");
            
            c.HasOne(x => x.Ticket)
                .WithMany(t => t.Comentarios)
                .HasForeignKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}