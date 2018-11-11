using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdCollectieRichert
{
    class Program
    {
        static CdCollectieContext db = new CdCollectieContext();
        static void Main(string[] args)
        {

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            StartProgramma();

            //Console.ReadKey();

        }

        static void StartProgramma()
        {
            while (true)
            {
                Console.Clear();
                ConsoleExt.WriteTitle("Mijn CD collectie");
                ConsoleExt.WriteLineInfo("Kies een optie uit het menu");
                Console.WriteLine("1 - Nieuw album invoeren");
                Console.WriteLine("2 - Song aan een bestaand album toevoegen");
                Console.WriteLine("3 - Song uit een album verwijderen");
                Console.WriteLine("4 - Overzichtslijsten");
                Console.WriteLine("5 - Zoekfunctie");
                Console.WriteLine("0 - Afsluiten");

                switch (Console.ReadLine())
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        NieuwAlbumInvoeren();
                        break;
                    case "2":
                        LinkSongAanAlbum();
                        break;
                    case "3":
                        SongVerwijderenUitAlbum();
                        break;
                    case "4":
                        SubMenuOverzichtenWeergeven();
                        break;
                    case "5":
                        Zoekfunctie();
                        break;
                    default:
                        ConsoleExt.WriteLineError("Uw keuze is niet beschikbaar in het menu. Gelieve een andere keuze te maken");
                        break;
                }
            }
        }

        private static void SongVerwijderenUitAlbum()
        {
            ConsoleExt.WriteLineInfo("Song verwijderen uit album");
            Console.WriteLine("Welke song wil u verwijderen. Typ een stuk van de titel:");
            var geselecteerdSong = KiesSong();
            db.Songs.Remove(geselecteerdSong);
            try
            {
                db.SaveChanges();
                ConsoleExt.WriteLineSucces("De song werd uit het album verwijderd");
            }
            catch (Exception ex)
            {
                ConsoleExt.WriteLineError("De song kon niet verwijderd worden.");
                ConsoleExt.WriteLineError(ex.InnerException.Message);
            }
        }

        private static void Zoekfunctie()
        {
            ConsoleExt.WriteLineInfo("Geef een stuk van de Artiest, Album of Titel in:");
            string zoekterm = Console.ReadLine().ToLower();

            List<Song> collectie = db.Songs
                .Include(song => song.Album)
                .Include(song => song.Artiest)
                .Where(song => song.Titel.ToLower().Contains(zoekterm)
                            || song.Artiest.Naam.ToLower().Contains(zoekterm)
                            || song.Album.Titel.ToLower().Contains(zoekterm)).ToList();

            foreach (var song in collectie)
            {
                Console.WriteLine($"Titel: {song.Titel}\nAlbum: {song.Album.Titel}\nArtiest: {song.Artiest.Naam}\n\n");
            }
            Console.ReadKey();
        }

       



        private static void LinkSongAanAlbum()
        {
            NieuweSongToevoegen();
           
            /*Voorlopig vagt deze method enkel het linken van nieuwe song aan bestaand album op
             to do: mogelijkheid voorzien om bestaande song aan bestaand album te linken*/
        }

        private static void NieuweSongToevoegen()
        {
            var nieuweSong = new Song();
            Console.WriteLine("Titel:");
            nieuweSong.Titel = Console.ReadLine();
            nieuweSong.Artiest = KiesArtiest();
            if (nieuweSong.Artiest != null)
            {
                ConsoleExt.WriteLineInfo($"U heeft gekozen voor {nieuweSong.Artiest.Naam}");
            }
            Console.WriteLine("Duur:");
            nieuweSong.Duur = Console.ReadLine();
            nieuweSong.Album = KiesAlbum();
            if (nieuweSong.Album != null)
            {
                ConsoleExt.WriteLineInfo($"U heeft gekozen voor {nieuweSong.Album.Titel}");
            }
            db.Songs.Add(nieuweSong);
            try
            {
                db.SaveChanges();
                ConsoleExt.WriteLineSucces("De song werd aan de database toegevoegd");
            }
            catch(Exception ex)
            {
                ConsoleExt.WriteLineError("De song werd niet toegevoegd.");
                ConsoleExt.WriteLineError(ex.InnerException.Message);
            }
            
        }

        

        private static void NieuwAlbumInvoeren()
        {
            Console.Clear();
            ConsoleExt.WriteLineInfo("Nieuw Album Invoeren");

            var nieuwAlbum = new Album();

            Console.WriteLine("Titel:");
            nieuwAlbum.Titel = Console.ReadLine();
            nieuwAlbum.Artiest = KiesArtiest();
            if (nieuwAlbum.Artiest != null)
            {
                ConsoleExt.WriteLineInfo($"U heeft gekozen voor {nieuwAlbum.Artiest.Naam}");
            }
            nieuwAlbum.Genre = KiesGenre();

            Console.WriteLine("Releasejaar:");
            nieuwAlbum.ReleaseJaar = Convert.ToInt32(Console.ReadLine());

            db.Albums.Add(nieuwAlbum);
            try
            {
                db.SaveChanges();
                ConsoleExt.WriteLineSucces("Het album werd met succes toegeoegd");
            }
            catch (Exception ex)
            {
                ConsoleExt.WriteLineError("Het boek werd niet opgeslagen");
                ConsoleExt.WriteLineError(ex.InnerException.Message);
            }

        }

        private static Genre KiesGenre()
        {
            ConsoleExt.WriteLineInfo("Kies het genre: ");
            Console.WriteLine("1 - Klassiek");
            Console.WriteLine("2 - Popmuziek");
            Console.WriteLine("3 - Jazz");
            Console.WriteLine("4 - Kleinkunst");

            Console.WriteLine("Geef een waarde tussen 1 en 4 in.");
            int val = Convert.ToInt32(Console.ReadLine());
            switch (val)
            {
                case 1: return Genre.Klassiek;
                case 2: return Genre.Popmuziek;
                case 3: return Genre.Jazz;
                case 4: return Genre.Kleinkunst;
                default: return Genre.Geen;
            }
        }

        private static Artiest KiesArtiest()
        {
            while (true)
            {
                Console.WriteLine("Artiest: (geef een deel van de naam in, lege invoer om over te slaan");

                string zoekterm = Console.ReadLine();

                if (zoekterm == "")
                {
                    return null;
                }

                var zoekresultaten = ZoekArtiesten(zoekterm);

                for (int i = 0; i < zoekresultaten.Count; i++)
                {
                    Console.WriteLine(i + 1 + $" - {zoekresultaten[i].Naam}");
                }

                Console.WriteLine("0 - overslaan");

                while (true)
                {
                    Console.Write("Geef het nummer van de artiest in: ");
                    string invoer = Console.ReadLine();

                    if (invoer == "0")
                    {
                        return null;
                    }

                    if (!int.TryParse(invoer, out int gekozennummer))
                    {
                        ConsoleExt.WriteLineError("ongelding getal");
                        continue;
                    }

                    try
                    {
                        return zoekresultaten[gekozennummer - 1];
                    }
                    catch
                    {
                        ConsoleExt.WriteLineError("Getal komt niet voor in de lijst");
                    }
                }

            }
        }

        static List<Artiest> ZoekArtiesten(string zoekterm)
        {

                zoekterm = zoekterm.ToLower();
                return db.Artiesten
                    .Where(artiest => artiest.Naam.ToLower().Contains(zoekterm)).ToList();

        }

        private static Album KiesAlbum()
        {
            while (true)
            {
                Console.WriteLine("Album: (geef een deel van de naam in, lege invoer om over te slaan");

                string zoekterm = Console.ReadLine();

                if (zoekterm == "")
                {
                    return null;
                }

                var zoekresultaten = ZoekAlbums(zoekterm);

                for (int i = 0; i < zoekresultaten.Count; i++)
                {
                    Console.WriteLine(i + 1 + $" - {zoekresultaten[i].Titel}");
                }

                Console.WriteLine("0 - overslaan");

                while (true)
                {
                    Console.Write("Geef het nummer van het album in: ");
                    string invoer = Console.ReadLine();

                    if (invoer == "0")
                    {
                        return null;
                    }

                    if (!int.TryParse(invoer, out int gekozennummer))
                    {
                        ConsoleExt.WriteLineError("ongelding getal");
                        continue;
                    }

                    try
                    {
                        return zoekresultaten[gekozennummer - 1];
                    }
                    catch
                    {
                        ConsoleExt.WriteLineError("Getal komt niet voor in de lijst");
                    }
                }

            }
        }

        static List<Album> ZoekAlbums(string zoekterm)
        {

            zoekterm = zoekterm.ToLower();
            return db.Albums
                .Where(album => album.Titel.ToLower().Contains(zoekterm)).ToList();

        }



        private static Song KiesSong()
        {
            while (true)
            {
                Console.WriteLine("Song: (geef een deel van de naam in, lege invoer om over te slaan");

                string zoekterm = Console.ReadLine();

                if (zoekterm == "")
                {
                    return null;
                }

                var zoekresultaten = ZoekSongs(zoekterm);

                for (int i = 0; i < zoekresultaten.Count; i++)
                {
                    Console.WriteLine(i + 1 + $" - {zoekresultaten[i].Titel}");
                }

                Console.WriteLine("0 - overslaan");

                while (true)
                {
                    Console.Write("Geef het nummer van de song in: ");
                    string invoer = Console.ReadLine();

                    if (invoer == "0")
                    {
                        return null;
                    }

                    if (!int.TryParse(invoer, out int gekozennummer))
                    {
                        ConsoleExt.WriteLineError("ongelding getal");
                        continue;
                    }

                    try
                    {
                        return zoekresultaten[gekozennummer - 1];
                    }
                    catch
                    {
                        ConsoleExt.WriteLineError("Getal komt niet voor in de lijst");
                    }
                }

            }
        }


        static List<Song> ZoekSongs(string zoekterm)
        {

            zoekterm = zoekterm.ToLower();
            return db.Songs
                .Where(song => song.Titel.ToLower().Contains(zoekterm)).ToList();

        }

/****************************************************************
 * 
 * PROGRAMMATUUR OVERZICHTSLIJSTEN + apart submenu
 * 
 * **************************************************************/

        static void SubMenuOverzichtenWeergeven()
        {
            Console.Clear();
            while (true)
            {
                ConsoleExt.WriteTitle("Mijn CD collectie - Overzichten");
                ConsoleExt.WriteLineInfo("Kies een overzicht");
                Console.WriteLine("41 - Albums per artiest");
                Console.WriteLine("42 - Albums per releasejaar");
                Console.WriteLine("43 - Albums per genre");
                Console.WriteLine("44 - Alfabetisch overzicht van de songs");
                Console.WriteLine("45 - Alfabetisch overzicht van de songs - per artiest");
                Console.WriteLine("00 - Terug naar hoofdmenu");

                switch (Console.ReadLine())
                {
                    case "00":
                        return;
                    case "41":
                        ToonAlbumsPerArtiest();
                        break;
                    case "42":
                        ToonAlbumsPerReleaseJaar();
                        break;
                    case "43":
                        ToonAlbumsPerGenre();
                        break;
                    case "44":
                        ToonSongsAlfabetisch();
                        break;
                    case "45":
                        ToonSongsPerArtiest();
                        break;
                    default:
                        ConsoleExt.WriteLineError("Uw keuze is niet beschikbaar in het menu. Gelieve een andere keuze te maken");
                        break;
                }
            }

        }

        public static void ToonAlbumsPerArtiest()
        {
            Console.Clear();
            ConsoleExt.WriteLineInfo("Albums per artiest");

            foreach (var album in db.Albums
                .Include(album => album.Songs)
                .Include(album => album.Artiest)
                .OrderBy(album => album.Artiest.Naam))
            {
                Console.WriteLine($"{album.Artiest.Naam} : {album.Titel} ({album.Genre}) uitgebracht in {album.ReleaseJaar}");

                foreach (var song in album.Songs)
                {
                    Console.WriteLine($"\t{song.Titel} ({song.Duur})");
                }

            }

            Console.WriteLine("\n");

        }

        public static void ToonAlbumsPerReleaseJaar()
        {
            Console.Clear();
            ConsoleExt.WriteLineInfo("Albums per Releasejaar");

            foreach (var album in db.Albums
                .Include(album => album.Songs)
                .Include(album => album.Artiest)
                .OrderBy(album => album.ReleaseJaar))
            {
                Console.WriteLine($"{album.ReleaseJaar} - {album.Titel} van {album.Artiest.Naam} ({album.Genre})");

                foreach (var song in album.Songs)
                {
                    Console.WriteLine($"\t{song.Titel} ({song.Duur})");
                }
            }

            Console.WriteLine("\n");
        }

        public static void ToonAlbumsPerGenre()
        {
            Console.Clear();
            ConsoleExt.WriteLineInfo("Albums per Genre");

            foreach (var album in db.Albums
                .Include(album => album.Songs)
                .Include(album => album.Artiest)
                .OrderBy(album => album.Genre))
            {
                Console.WriteLine($"{album.Genre} - {album.Titel} van {album.Artiest.Naam} ({album.ReleaseJaar})");

                foreach (var song in album.Songs)
                {
                    Console.WriteLine($"\t{song.Titel} ({song.Duur})");
                }
            }

            Console.WriteLine("\n");
        }

        private static void ToonSongsAlfabetisch()
        {
            Console.Clear();
            ConsoleExt.WriteLineInfo("Alfabetische songlijst");

            foreach (var song in db.Songs
                .Include(song => song.Artiest)
                .OrderBy(song => song.Titel))

            {
                Console.WriteLine($"{song.Titel} ({song.Artiest.Naam})");
            }

        }

        private static void ToonSongsPerArtiest()
        {
            Console.Clear();
            ConsoleExt.WriteLineInfo("Alfabetische songlijst");
            /*OPTIMALISATIE NAKIJKEN*/

            foreach (var song in db.Songs
                .Include(song => song.Artiest)
                .OrderBy(song => song.Artiest.Naam))

            {
                Console.WriteLine($"{song.Artiest.Naam} - {song.Titel}");
            }

        }

    }
}
