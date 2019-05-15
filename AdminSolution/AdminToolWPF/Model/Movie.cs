using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminToolWPF.Model
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public List<Genre> GenreList { get; set; } = new List<Genre>() { new Genre() { GenreId = 1, GenreText = "Action" } };
    }
}
