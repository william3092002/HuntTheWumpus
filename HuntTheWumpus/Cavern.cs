using System;


namespace HuntTheWumpus
{
    public class Cavern
    {
        public bool Pit { get; set; }
        public bool Bats { get; set; }
        public bool Wumpus { get; set; }
        public bool Player { get; set; }
        public bool Blood { get; set; }
        public bool Draft { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public bool Occupied { get; set; }
        public bool VisitedHint { get; set; }
        public bool VisitedTrace { get; set; }
        
        public Cavern(int clmn, int r)
        {
            Pit = false;
            Bats = false;
            Wumpus = false;
            Player = false;
            Blood = false;
            Draft = false;
            Column = clmn;
            Row = r;
            Occupied = false;
            VisitedHint = false;
            VisitedTrace = false;
        }
        


        
        
    }
}
