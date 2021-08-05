using System;

namespace WebApplication1
{
    public class AllPeople
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public People[] results { get; set; }
    }
}
