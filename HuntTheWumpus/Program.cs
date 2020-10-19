using System;


namespace HuntTheWumpus
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ContinueGame;
            Game.Intro();
            do
            {
                ContinueGame = Game.ProLougue();
                Game.Menu();
                Map.InitializeMap(Game.size);
                Game.Play();
            } while (ContinueGame == true);
            
        }
    }
}
