using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdCollectieRichert
{
    public enum Genre
    {
        Klassiek = 1,
        Popmuziek = 2,
        Jazz = 3,
        Kleinkunst = 4,
        Geen = 99
    }
    public class Album
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        // er bestaan albums waarin songs opgenomen worden van verschillende artisten
        // daarom voorzie ik de keuze om een album NIET aan een artiest te linken
        public int? ArtiestId { get; set; }
        public Artiest Artiest { get; set; }
        public List<Song> Songs { get; set; }
        public Genre Genre { get; set; }
        public int ReleaseJaar { get; set; } = DateTime.Now.Year;

    }
}
