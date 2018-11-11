using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdCollectieRichert
{
    public class CdCollectieContext : DbContext
    {
        public DbSet<Artiest> Artiesten { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(System.Configuration.ConfigurationManager
            .ConnectionStrings["cdCollectie"].ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<Artiest>().HasData(
                new Artiest { Id = 1, Naam = "The beatles"},
                new Artiest { Id = 2, Naam = "Johann Sebastian Bach" },
                new Artiest { Id = 3, Naam = "Johan Verminnen" },
                new Artiest { Id = 4, Naam = "Air" }
           );

            modelBuilder.Entity<Album>().HasData(
                new Album { Id = 1, Titel = "Bach Greatetst Hits", Genre = Genre.Klassiek, ArtiestId = 2, ReleaseJaar = 1985 },
                new Album { Id = 2, Titel = "Johan in Sint-Martens-Latem, 20 jaar later", Genre = Genre.Kleinkunst, ArtiestId = 3, ReleaseJaar = 2011 },
                new Album { Id = 3, Titel = "The beatles greatest hits", Genre = Genre.Popmuziek, ArtiestId = 1, ReleaseJaar = 1990 },
                new Album { Id = 4, Titel = "The beatles eternal collection", Genre = Genre.Popmuziek, ArtiestId = 1, ReleaseJaar = 1990 }
           );

            modelBuilder.Entity<Song>().HasData(
                new Song { Id = 1, Titel = "Laat me nu toch niet alleen", ArtiestId = 3, Duur="3:35", AlbumId = 2 },
                new Song { Id = 2, Titel = "In de rue des bouchers", ArtiestId = 3, Duur = "2:45", AlbumId = 2 },
                new Song { Id = 3, Titel = "Toccata & Fuga", ArtiestId = 2, Duur = "10:33", AlbumId = 1 },
                new Song { Id = 4, Titel = "Air uit suite in g", ArtiestId = 2, Duur = "4:12", AlbumId = 1 },
                new Song { Id = 5, Titel = "Jesu bleibet meine freude", ArtiestId = 2, Duur = "2:45",AlbumId = 1 },
                new Song { Id = 6, Titel = "Let it be", ArtiestId = 1, Duur = "2:45", AlbumId = 3 },
                new Song { Id = 7, Titel = "Yesterday", ArtiestId = 1, Duur = "2:55", AlbumId = 3 },
                new Song { Id = 8, Titel = "Yellow submarine", ArtiestId = 1, Duur = "1:45", AlbumId = 4 }
           );
            
        }
    }
}
