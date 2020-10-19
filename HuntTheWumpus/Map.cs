using System;
using System.Collections.Generic;

namespace HuntTheWumpus
{
    public class Map

    {
        static string PlayerLocation = "";
        static string PlayerLocationPrevious = "";
        public static string PlayerCurrentLoc() => PlayerLocation = $"[{PlayerCurrentCol},{PlayerCurrentRow}]";
        public static string PlayerPreviousLoc() => PlayerLocationPrevious = $"[{PlayerPreviousCol},{PlayerPreviousRow}]";
        
        
        static int PlayerCurrentCol = 0;
        static int PlayerCurrentRow = 0;

        static int PlayerPreviousCol;
        static int PlayerPreviousRow;

        //**Methods and statements in below section help setup the game.** 

        //A static two demensional array of cavern objects is declared to represent the map.  
        static Cavern[,] newMap;
        //Random object allow us to randomly generate the coordinates of occupied caverns.
        static Random Loc = new Random();

        //InitializeCavern is where new instances of object Caverns are instantiated.
        //This mthod also assign values to the column and row property.
        //For example [3,4] element of the array newMap Cavern array will have 3 assign to its Column property,
        //and 4 assign to its row property; hence the coordinate of the cavern will also be [3,4].  
        public static void InitializeCavern(string size)
        {
            newMap = new Cavern[MapSize[size], MapSize[size]];
            for (int i = 0; i < MapSize[size]; i++)
            {
                for (int j = 0; j < MapSize[size]; j++)
                    newMap[j,i] = new Cavern(j, i);
            }            
        }

        //Methods start with "Populate" substring in their name assign the value of true to a specific property of a random cavern.
        //This is how the program know whether a cavern is empty, have Wumpus in it or a pit...etc
        public static void PopulateWumpus(string size)
        {
            int col = Loc.Next(MapSize[size]);
            int row = Loc.Next(MapSize[size]);

            newMap[col, row].Wumpus = true;
            newMap[col, row].Occupied = true;
            PopulateBlood(col, row, size);
        }
        public static void PopulatePlayer(string size)
        {
            int col = Loc.Next(MapSize[size]);
            int row = Loc.Next(MapSize[size]);

            if (newMap[col, row].Occupied == false)
            {
                newMap[col, row].Player = true;
                newMap[col, row].Occupied = true;
                PlayerCurrentCol = col;
                PlayerCurrentRow = row;
                newMap[col, row].VisitedTrace = true;
                VisitedTrace.Enqueue($"[{newMap[col, row].Column},{newMap[col, row].Row}]");
            }
            else
                PopulatePlayer(size);
        }

        //The constructor passes the value of number of pits and bats this map will have to a static field that can be used in below methods. 
        public static Dictionary<string, int> MapSize = new Dictionary<string, int>()
        {
            {"small", 6},
            {"medium", 7},
            {"large", 8}
        };
        public Map(string size)
        {
            switch (MapSize[size])
            {
                case 6:
                    numberOfPits = 2;
                    numberOfBats = 2;
                    break;
                case 7:
                    numberOfPits = 3;
                    numberOfBats = 3;
                    break;
                case 8:
                    numberOfPits = 4;
                    numberOfBats = 4;
                    break;
            }
        }
        public static int numberOfPits = 0;
        public static int numberOfBats = 0;
        public static void PopulatePits(string size)
        {
            int col = Loc.Next(MapSize[size]);
            int row = Loc.Next(MapSize[size]);

            if (newMap[col, row].Occupied == false)
            {
                newMap[col, row].Pit = true;
                newMap[col, row].Occupied = true;
                PopulateDraft(col, row, size);
            }
            else
                PopulatePits(size);
        }
        public static void PopulateBats(string size)
        {
            int col = Loc.Next(MapSize[size]);
            int row = Loc.Next(MapSize[size]);

            if (newMap[col, row].Occupied == false)
            {
                newMap[col, row].Bats = true;
                newMap[col, row].Occupied = true;
            }
            else
                PopulateBats(size);
        }
        public static void PopulateBlood(int col, int row, string size )
        {
            int newCol = col + 1;
            if (newCol > MapSize[size] - 1)
                newCol = 0;
            newMap[newCol, row].Blood = true;
            newCol = col - 1;
            if (newCol < 0)
                newCol = MapSize[size] - 1;
            newMap[newCol, row].Blood = true;
            int newRow = row + 1;
            if (newRow > MapSize[size] - 1)
                newRow = 0;
            newMap[col, newRow].Blood = true;
            newRow = row - 1;
            if (newRow < 0)
                newRow = MapSize[size] - 1;
            newMap[col, newRow].Blood = true;
        }
        public static void PopulateDraft(int col, int row, string size)
        {

            int newCol = col + 1;
            if (newCol > MapSize[size] - 1)
                newCol = 0;
            newMap[newCol, row].Draft = true;
            newCol = col - 1;
            if (newCol < 0)
                newCol = MapSize[size] - 1;
            newMap[newCol, row].Draft = true;
            int newRow = row + 1;
            if (newRow > MapSize[size] - 1)
                newRow = 0;
            newMap[col, newRow].Draft = true;
            newRow = row - 1;
            if (newRow < 0)
                newRow = MapSize[size] - 1;
            newMap[col, newRow].Draft = true;
        }

        //The InitializeMap runs above methods in correct sequence and correct amount of iteration to finalize the setup of a new game map.
        //It also adds all the occupied caverns coordinates and what they contain to a List by using the addINFO method.  
        static List<string> MapInfo = new List<string>();
        public static void InitializeMap(string size)
        {
            Map map = new Map(size);
            InitializeCavern(size);
            PopulateWumpus(size);
            PopulatePlayer(size);
            for (int i = 0; i < numberOfPits; i++)
                PopulatePits(size);
            for (int i = 0; i < numberOfBats; i++)
                PopulateBats(size);
            addINFO(size);
        }
        public static void addINFO(string size)
        {
            for (int i = 0; i < MapSize[size]; i++)
            {
                for (int j = 0; j < MapSize[size]; j++)
                {
                    if (newMap[j, i].Wumpus == true)
                    {
                        MapInfo.Add($"Wumpus: [{newMap[j, i].Column},{newMap[j, i].Row}]");
                        EndMapInfo.Add($"Wumpus: [{newMap[j, i].Column},{newMap[j, i].Row}]");
                    }
                    if (newMap[j, i].Pit == true)
                    {
                        MapInfo.Add($"Pit: [{newMap[j, i].Column},{newMap[j, i].Row}]");
                        EndMapInfo.Add($"Pit: [{newMap[j, i].Column},{newMap[j, i].Row}]");
                    }
                    if (newMap[j, i].Bats == true)
                        MapInfo.Add($"Bats: [{newMap[j, i].Column},{newMap[j, i].Row}]");
                    if (newMap[j, i].Player == true)
                        MapInfo.Add($"Player Start Cavern: [{newMap[j, i].Column},{newMap[j, i].Row}]");
                    if (newMap[j, i].Blood == true)
                        MapInfo.Add($"Blood: {newMap[j, i].Column},{newMap[j, i].Row}");
                    if (newMap[j, i].Draft == true)
                        MapInfo.Add($"Draft: {newMap[j, i].Column},{newMap[j, i].Row}");
                }
            }
        }

        //**Methods and statements in below section allow players to Move, Shoot, get Notes, trace Player's previous moves and cheat.**

        //The Move method update the player's location by assigning proper value to Cavern.Player property.
        public static int Move(string size)
        {
            PlayerPreviousCol = PlayerCurrentCol;
            PlayerPreviousRow = PlayerCurrentRow;
            string direction = Console.ReadLine();
            switch (direction)
            {
                case "1":
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Player = false;
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Occupied = false;
                    int updatedRow = PlayerCurrentRow - 1;
                    if (updatedRow < 0)
                        updatedRow = MapSize[size] - 1;
                    newMap[PlayerCurrentCol, updatedRow].Player = true;
                    PlayerCurrentRow = updatedRow;
                    VisitedTrace.Enqueue($"[{newMap[PlayerCurrentCol, updatedRow].Column},{newMap[PlayerCurrentCol, updatedRow].Row}]");
                    newMap[PlayerCurrentCol, updatedRow].VisitedTrace = true;
                    Game.move++;
                    return CheckIfCavernIsOccupied(size);
                case "2":
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Player = false;
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Occupied = false;
                    updatedRow = PlayerCurrentRow + 1;
                    if (updatedRow > MapSize[size] - 1)
                        updatedRow = 0;
                    newMap[PlayerCurrentCol, updatedRow].Player = true;
                    PlayerCurrentRow = updatedRow;
                    VisitedTrace.Enqueue($"[{newMap[PlayerCurrentCol, updatedRow].Column},{newMap[PlayerCurrentCol, updatedRow].Row}]");
                    newMap[PlayerCurrentCol, updatedRow].VisitedTrace = true;
                    Game.move++;
                    return CheckIfCavernIsOccupied(size);
                case "4":
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Player = false;
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Occupied = false;
                    int updatedCol = PlayerCurrentCol - 1;
                    if (updatedCol < 0)
                        updatedCol = MapSize[size] - 1;
                    newMap[updatedCol, PlayerCurrentRow].Player = true;
                    PlayerCurrentCol = updatedCol;
                    VisitedTrace.Enqueue($"[{newMap[updatedCol, PlayerCurrentRow].Column},{newMap[updatedCol, PlayerCurrentRow].Row}]");
                    newMap[updatedCol, PlayerCurrentRow].VisitedTrace = true;
                    Game.move++;
                    return CheckIfCavernIsOccupied(size);
                case "3":
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Player = false;
                    newMap[PlayerCurrentCol, PlayerCurrentRow].Occupied = false;
                    updatedCol = PlayerCurrentCol + 1;
                    if (updatedCol > MapSize[size] - 1)
                        updatedCol = 0;
                    newMap[updatedCol, PlayerCurrentRow].Player = true;
                    PlayerCurrentCol = updatedCol;
                    VisitedTrace.Enqueue($"[{newMap[updatedCol, PlayerCurrentRow].Column},{newMap[updatedCol, PlayerCurrentRow].Row}]");
                    newMap[updatedCol, PlayerCurrentRow].VisitedTrace = true;
                    Game.move++;
                    return CheckIfCavernIsOccupied(size);
                default:
                    Game.Update();
                    return 0;
            }
        }
        //PrintCaverns method will update the map output to reflect player's location in map.
        public static void PrintCaverns(string size)
        {
            for (int i = 0; i < MapSize[size]; i++)
            {
                for (int j = 0; j < MapSize[size]; j++)
                {
                    if (newMap[j, i].Player == true)
                    {
                        Console.Write($"[{newMap[j, i].Column},{newMap[j, i].Row}]  ");
                    }
                    else
                        Console.Write($" {newMap[j, i].Column},{newMap[j, i].Row}   ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //CheckIfCavernIsOccupied checks if the cavern player move to has the Wumpus or pit in it,
        //if so it invokes other method and return a value that will be passed to a variable call GameState.
        //Game state will determin whether the game should continue or finish. 
        public static int CheckIfCavernIsOccupied(string size)
        {
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].Wumpus == true)
                return WumpusEatsYou();
            else if (newMap[PlayerCurrentCol, PlayerCurrentRow].Pit == true)
                return FallInToPit();
            else
                return 0;
        }
        public static int WumpusEatsYou()
        {
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].Wumpus == true)
            {
                Console.Clear();

                Console.WriteLine("==========================");
                Console.WriteLine("You got eaten by Wumpus!");
                Console.WriteLine("==========================");
                PrintEndINFO();
                Console.WriteLine("--------\nGameOver\n--------");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                VisitedPointOfInterest.Clear();
                VisitedTrace.Clear();
                EndMapInfo.Clear();
                MapInfo.Clear();

                return 99;
            }
            else
            {
                Console.Clear();

                Console.WriteLine("==========================");
                Console.WriteLine("You missed Wumpus...");
                Console.WriteLine("You got eaten by Wumpus!");
                Console.WriteLine("==========================");
                PrintEndINFO();
                Console.WriteLine("--------\nGameOver\n--------");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                VisitedPointOfInterest.Clear();
                VisitedTrace.Clear();
                EndMapInfo.Clear();
                MapInfo.Clear();


                return 99;
            }
        }
        public static int FallInToPit()
        {
            Console.Clear();
            Console.WriteLine("============================================");
            Console.WriteLine("You fall into a buttomless pit...");
            Console.WriteLine("============================================");
            PrintEndINFO();
            Console.WriteLine("--------\nGameOver\n--------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            VisitedPointOfInterest.Clear();
            VisitedTrace.Clear();
            EndMapInfo.Clear();
            MapInfo.Clear();

            return 99;
        }
        //CheckCavern method checks if the cavern player move to has blood, draft or bats. 
        public static void CheckCavern(string size)
        {
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].Blood == true)
                PrintBlood();
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].Draft == true)
                PrintDraft();
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].Bats == true & PlayerCurrentLoc() != Game.PlayerPreviousLocation)
            {
                PrintBats();
                newMap[PlayerCurrentCol, PlayerCurrentRow].VisitedHint = true;
                BatsCarry(size);
            }
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].Blood == true | newMap[PlayerCurrentCol, PlayerCurrentRow].Draft == true | newMap[PlayerCurrentCol, PlayerCurrentRow].Bats == true)
                newMap[PlayerCurrentCol, PlayerCurrentRow].VisitedHint = true;
        }

        //Below three Print methods print the hint to alert player the current cavern they're in contain hints of thier surrounding caverns.
        public static void PrintBlood()
        {
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].VisitedHint != true)
                VisitedPointOfInterest.Add($"[{newMap[PlayerCurrentCol, PlayerCurrentRow].Column},{newMap[PlayerCurrentCol, PlayerCurrentRow].Row}]: Blood");
            Console.WriteLine("You see blood in this cavern");

        }
        public static void PrintDraft()
        {
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].VisitedHint != true)
                VisitedPointOfInterest.Add($"[{newMap[PlayerCurrentCol, PlayerCurrentRow].Column},{newMap[PlayerCurrentCol, PlayerCurrentRow].Row}]: Draft");
            Console.WriteLine("You detected draft in this cavern");
        }
        public static void PrintBats()
        {
            if (newMap[PlayerCurrentCol, PlayerCurrentRow].VisitedHint != true)
                VisitedPointOfInterest.Add($"[{newMap[PlayerCurrentCol, PlayerCurrentRow].Column},{newMap[PlayerCurrentCol, PlayerCurrentRow].Row}]: Bats");
            Console.WriteLine("You walked in to a group of bats");
        }
        //If player move into a cavern with bats in it, there's a 50% chance the player will be teleport to a random cavern that is unoccupied with hazard. 
        public static int BatsCarry(string size)
        {
            int batCarryChance = Loc.Next(2);
            if (batCarryChance == 1)
            {
                newMap[PlayerCurrentCol, PlayerCurrentRow].Player = false;
                int updatedCol;
                int updatedRow;
                do
                {
                    updatedCol = Loc.Next(MapSize[size]);
                    updatedRow = Loc.Next(MapSize[size]);
                } while (newMap[updatedCol, updatedRow].Occupied == true);

                newMap[updatedCol, updatedRow].Player = true;
                
                PlayerCurrentCol = updatedCol;
                PlayerCurrentRow = updatedRow;
                Console.Clear();
                PrintCaverns(size);
                Console.WriteLine("You walked in to a group of bats");
                Console.WriteLine($"you were in Cavern: {Game.PlayerPreviousLocation}");
                Console.WriteLine($"The bats carried you to cavern: {PlayerCurrentLoc()}\n");
                return CheckIfCavernIsOccupied(size);
            }
            else
                return 0;
        }
        

        //Shoot method allow the players to shoot at a direction.
        public static int Shoot(string size)
        {
            Console.WriteLine();
            string direction = Console.ReadLine();
            switch (direction)
            {
                case "1":
                    int updatedRow = PlayerCurrentRow - 1;
                    if (updatedRow < 0)
                        updatedRow = MapSize[size] - 1;
                    return CheckIfWumpusDead(PlayerCurrentCol, updatedRow);
                case "2":
                    updatedRow = PlayerCurrentRow + 1;
                    if (updatedRow > MapSize[size] - 1)
                        updatedRow = 0;
                    return CheckIfWumpusDead(PlayerCurrentCol, updatedRow);
                case "4":
                    int updatedCol = PlayerCurrentCol - 1;
                    if (updatedCol < 0)
                        updatedCol = MapSize[size] - 1;
                    return CheckIfWumpusDead(updatedCol, PlayerCurrentRow);
                case "3":
                    updatedCol = PlayerCurrentCol + 1;
                    if (updatedCol > MapSize[size] - 1)
                        updatedCol = 0;
                    return CheckIfWumpusDead(updatedCol, PlayerCurrentRow);
                default:
                    Game.Update();
                    return 0;
            }
        }
        public static int CheckIfWumpusDead(int updateCol, int updatedRow)
        {
            if (newMap[updateCol, updatedRow].Wumpus == true)
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("You shot Wumpus dead in the head, you win!!");
                Console.WriteLine("=============================================");
                PrintEndINFO();
                Console.WriteLine("--------\nGameOver\n--------");
                Console.ReadKey();
                Console.Clear();
                VisitedPointOfInterest.Clear();
                VisitedTrace.Clear();
                EndMapInfo.Clear();
                MapInfo.Clear();
                return 100;
            }
            else
                return WumpusEatsYou();
        }

        //Note method will output information (List and a map) of all occupied caverns the player has visited.  
        static List<string> VisitedPointOfInterest = new List<string>();
        public static void Note(string size)
        {
            if (VisitedPointOfInterest.Count == 0)
                Console.WriteLine("No hints available");
            else
            {
                Console.Clear();
                PrintNoteCavern(size);
                VisitedPointOfInterest.Sort();
                foreach (var item in VisitedPointOfInterest)
                    Console.WriteLine(item);
                Console.WriteLine();
            }
        }
        public static void PrintNoteCavern(string size)
        {
            for (int i = 0; i < MapSize[size]; i++)
            {
                for (int j = 0; j < MapSize[size]; j++)
                {
                    if (newMap[j, i].VisitedHint == true)
                    {
                        if (newMap[j, i].Blood == true & newMap[j, i].Draft == true & newMap[j, i].Bats == true)
                            Console.Write($"^{newMap[j, i].Column},{newMap[j, i].Row}^  ");
                        if (newMap[j, i].Blood == true & newMap[j, i].Draft == true)
                            Console.Write($"^{newMap[j, i].Column},{newMap[j, i].Row}^  ");
                        if (newMap[j, i].Blood == true & newMap[j, i].Bats == true)
                            Console.Write($"^{newMap[j, i].Column},{newMap[j, i].Row}^  ");
                        if (newMap[j, i].Draft == true & newMap[j, i].Bats == true)
                            Console.Write($"^{newMap[j, i].Column},{newMap[j, i].Row}^  ");
                        else if (newMap[j, i].Bats == true)
                            Console.Write($"#{newMap[j, i].Column},{newMap[j, i].Row}#  ");
                        else if (newMap[j, i].Blood == true)
                            Console.Write($"+{newMap[j, i].Column},{newMap[j, i].Row}+  ");
                        else if (newMap[j, i].Draft == true)
                            Console.Write($"-{newMap[j, i].Column},{newMap[j, i].Row}-  ");
                    }
                    else
                        Console.Write($" {newMap[j, i].Column},{newMap[j, i].Row}   ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //Trace method will output information (List and a map) of all caverns the player has visited. 
        static Queue<string> VisitedTrace = new Queue<string>();
        public static void Trace(string size)
        {
            Console.Clear();
            PrintTraceCaverns(size);
            foreach (var item in VisitedTrace)
                Console.Write(item);
            Console.WriteLine();
            Console.WriteLine();
        }
        public static void PrintTraceCaverns(string size)
        {
            for (int i = 0; i < MapSize[size]; i++)
            {
                for (int j = 0; j < MapSize[size]; j++)
                {
                    if (newMap[j, i].VisitedTrace == true)
                    {
                        Console.Write($"[{newMap[j, i].Column},{newMap[j, i].Row}]  ");
                    }
                    else
                        Console.Write($" {newMap[j, i].Column},{newMap[j, i].Row}   ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //Cheat method will output inforamtion (List and a map) of all caverns that have hints or characters.
        public static void Cheat(string size)
        {
            Console.Clear();
            PrintCheatedCavern(size);
            MapInfo.Sort();
            Console.WriteLine($"Player Current Location: {PlayerCurrentLoc()}");
            foreach (var item in MapInfo)
                Console.WriteLine(item);
            Console.WriteLine();
        }
        public static void PrintCheatedCavern(string size)
        {
            for (int i = 0; i < MapSize[size]; i++)
            {
                for (int j = 0; j < MapSize[size]; j++)
                {
                    if (newMap[j, i].Player == true)
                        Console.Write($"[{newMap[j, i].Column},{newMap[j, i].Row}]  ");
                    else if (newMap[j, i].Wumpus == true)
                        Console.Write($"[{newMap[j, i].Column},{newMap[j, i].Row}]  ");
                    else if (newMap[j, i].Pit == true)
                        Console.Write($"[{newMap[j, i].Column},{newMap[j, i].Row}]  ");
                    else if (newMap[j, i].Bats == true)
                        Console.Write($"[{newMap[j, i].Column},{newMap[j, i].Row}]  ");
                    else
                        Console.Write($" {newMap[j, i].Column},{newMap[j, i].Row}   ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //Wumpus and pits coordinates will be added to EndMapInfo List, it will populate when a game ends.
        static List<string> EndMapInfo = new List<string>();
        public static void PrintEndINFO()
        {
            Console.WriteLine("Wumpus and pit's location: ");
            foreach (var item in EndMapInfo)
                Console.WriteLine(item);
        }

    }

}
