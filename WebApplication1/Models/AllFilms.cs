using System;

namespace WebApplication1
{
    public class AllFilms
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public Films[] results { get; set; }
    }
}
