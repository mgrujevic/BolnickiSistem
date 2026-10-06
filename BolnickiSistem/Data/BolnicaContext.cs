using BolnickiSistem.Models;
using Microsoft.EntityFrameworkCore;

namespace BolnickiSistem.Data;

public class BolnicaContext : DbContext
{
    public BolnicaContext(DbContextOptions<BolnicaContext> options)
        : base(options)
    {
    }

    public DbSet<Uloga> Uloge { get; set; }

    public DbSet<Korisnik> Korisnici { get; set; }

    public DbSet<Pacijent> Pacijenti { get; set; }

    public DbSet<Pregled> Pregledi { get; set; }

    public DbSet<Dijagnoza> Dijagnoze { get; set; }

    public DbSet<Terapija> Terapije { get; set; }

    public DbSet<LaboratorijskiNalaz> LaboratorijskiNalazi { get; set; }

    public DbSet<Dokument> Dokumenti { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Korisnik>()
            .HasOne(k => k.Uloga)
            .WithMany(u => u.Korisnici)
            .HasForeignKey(k => k.UlogaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pregled>()
            .HasOne(p => p.Pacijent)
            .WithMany(p => p.Pregledi)
            .HasForeignKey(p => p.PacijentId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Pregled>()
            .HasOne(p => p.Lekar)
            .WithMany(k => k.Pregledi)
            .HasForeignKey(p => p.LekarId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Dijagnoza>()
            .HasOne(d => d.Pregled)
            .WithMany(p => p.Dijagnoze)
            .HasForeignKey(d => d.PregledId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Terapija>()
            .HasOne(t => t.Pregled)
            .WithMany(p => p.Terapije)
            .HasForeignKey(t => t.PregledId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<LaboratorijskiNalaz>()
            .HasOne(l => l.Pacijent)
            .WithMany(p => p.LaboratorijskiNalazi)
            .HasForeignKey(l => l.PacijentId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<LaboratorijskiNalaz>()
            .HasOne(l => l.Pregled)
            .WithMany(p => p.LaboratorijskiNalazi)
            .HasForeignKey(l => l.PregledId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Dokument>()
            .HasOne(d => d.Pacijent)
            .WithMany(p => p.Dokumenti)
            .HasForeignKey(d => d.PacijentId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Dokument>()
            .HasOne(d => d.Pregled)
            .WithMany(p => p.Dokumenti)
            .HasForeignKey(d => d.PregledId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Dokument>()
            .HasOne(d => d.DodaoKorisnik)
            .WithMany(k => k.Dokumenti)
            .HasForeignKey(d => d.DodaoKorisnikId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Pacijent>()
            .HasIndex(p => p.BrojZdravstveneKartice)
            .IsUnique();


        modelBuilder.Entity<Pacijent>()
            .HasIndex(p => p.JMBG)
            .IsUnique();


        modelBuilder.Entity<Korisnik>()
            .HasIndex(k => k.KorisnickoIme)
            .IsUnique();


        modelBuilder.Entity<Pacijent>()
            .HasOne(p => p.Lekar)
            .WithMany(k => k.Pacijenti)
            .HasForeignKey(p => p.LekarId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Uloga>().HasData(
            new Uloga
            {
                UlogaId = 1,
                Naziv = "Administrator"
            },
            new Uloga
            {
                UlogaId = 2,
                Naziv = "Lekar"
            }
        );
    }
}