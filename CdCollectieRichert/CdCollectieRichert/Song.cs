using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdCollectieRichert
{
    public class Song
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public int ArtiestId { get; set; }
        public Artiest Artiest { get; set; }
        public string Duur { get; set; }
        public int? AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
