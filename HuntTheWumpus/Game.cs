using System;
using System.Collections.Generic;

namespace HuntTheWumpus
{
    class Game
    {
        public static Dictionary<int, string> MapSize = new Dictionary<int, string>()
        {
            {1, "small"},
            {2, "medium"},
            {3, "large"}
        };
        public static string size = "";
        //Game state is evaluated in Play method. 
        static int GameState = 0;
        public static int move = 0;

        public static string PlayerPreviousLocation = "";

        public static void Intro()
        {
            Console.Clear();
            Console.WriteLine("===========================\nWelcome to Hunt the Wumpus!\n===========================" +
                "\nThe original version of Hunt the Wumpus was created by Gregory Yob in 1972. The original version was quite a bit different than this version: it was text based, and was based on the vertices of a collapsed dodecahedron (rather than a grid). Each room (vertex) connected to 3 others (rather than four). " +
                "In this Version of Hunt the Wumpus, the player will traverse through a grid of caverns to locate, close with and hunt down the Wumpus. ");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        public static bool ProLougue()
        {
            move = 0;
            Console.Clear();
            Console.WriteLine(
                        "Main Menu" +
                        "\n===========================" +
                        "\n1. How to play" +
                        "\n2. Control" +
                        "\n3. Start" +
                        "\n4. Exit");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine(
                        "* There are three types of hazard in the map." +
                        "\n    1. Wumpus. If you walk into the cavern where Wumpus is located, you will be eaten by the Wumpus." +
                        "\n    2. Pits. If you walk into one of the cavern where a pit is located, you will fall into a bottomless pit where gravity shows no mercy." +
                        "\n    3. Bats. If you walk into one of the cavern where a colony of bats reside, there is a 50% chance that you will be carried to a random unoccupied cavern.\n" +
                        "\n* You will detect blood in caverns adjacent to the Wumpus; you will detect draft in caverns adjacent to the pit.\n" +
                        "\n* There will only be 1 Wumpus in the map.\n" +
                        "\n* There are multiple bats and pits in the map. The amount of these hazards depend on the map size.\n" +
                        "\n* You have one shot to kill the Wumpus and the reach of your bullet is one cavern away.\n" +
                        "\n* Beware the map wraps around.\n" +
                        "\npress any key to go back to the main menu");
                    Console.ReadKey();
                    Console.Clear();
                    ProLougue();
                    return true;
                case "2":
                    Console.Clear();
                    Console.WriteLine("* MOVE: Your currently location is indicated by a square bracket on the grid. Example as such [0,0]." +
                        "\nYou can move to adjacent cavern by following the menu option in game. To cancel move action just input anything other than 1 - 4. \n" +
                        "\n* SHOOT: You can fire at an adjacent cavern by following the menu option in game. To cancel shoot action just input anything other than 1 - 4.\n" +
                        "\n* NOTE: As you traverse through caverns that has blood, draft or bats in it, their coordinates will be recorded into your note.\n" +
                        "\n* TRACE: This function records all your previous moves.\n" +
                        "\n* CHEAT: This function will reveal all hazards in the map.\n" +
                        "\npress any key to go baack to the main menu");
                    Console.ReadKey();
                    Console.Clear();
                    ProLougue();
                    return true;
                case "3":
                    Console.Clear();
                    Console.WriteLine("You're a legendary hunter hired by a gorup of villagers to rid of a monstrous beast called \"Wumpus\"." +
                        "\nAccording to your intel, the Wumpus is currently sleeping in one of the caverns located at the valley 5 miles south of the village." +
                        "\nYou grabed your beloved rifle and start marching southward in to the valley.\n" +
                        "\nPress any key to continue...");
                    Console.ReadKey();
                    return true;
                    //Menu();
                    //Map.InitializeMap(Game.size);
                    //Play();
                    
                case "4":
                    Environment.Exit(0);
                    return false;
                    
                default:
                    Console.Clear();
                    Console.WriteLine("Please pick a valid option" +
                        "\n----------------------");
                    ProLougue();
                    return true;
            }



        }
        //Menu method provides the player 3 options of map size to choose from. 

        public static void Menu()
        {
            Console.Clear();
            int exceptionCounter;
            do
            {
                exceptionCounter = 0;
                try
                {
                    Console.WriteLine("Please select the size of the map: " +
                    "\n----------------------------------" +
                    "\n1: small" +
                    "\n2: medium" +
                    "\n3: large");
                    string input = Console.ReadLine();
                    if (int.Parse(input) >= 1 & int.Parse(input) <= 3)
                    {
                        Console.Clear();
                        size = MapSize[int.Parse(input)];
                        Console.WriteLine($"In a {size} size map:\n");
                        switch (size)
                        {
                            case "small":
                                Console.WriteLine("There is (1) Wumpus in the map." +
                                    $"\nThere are (2) Pits in the map." +
                                    $"\nThere are (2) Bats in the map.\n" +
                                    $"\nPress any key to continue...");
                                Console.ReadKey();
                                break;
                            case "medium":
                                Console.WriteLine("There is (1) Wumpus in the map." +
                                    $"\nThere are (3) Pits in the map." +
                                    $"\nThere are (3) Bats in the map.\n" +
                                    $"\nPress any key to continue...");
                                Console.ReadKey();
                                break;
                            case "large":
                                Console.WriteLine("There is (1) Wumpus in the map." +
                                    $"\nThere are (4) Pits in the map." +
                                    $"\nThere are (4) Bats in the map.\n" +
                                    $"\nPress any key to continue...");
                                Console.ReadKey();
                                break;
                            default:
                                break;
                        }

                    }
                    else
                        throw new OperationCanceledException("Enter a number between 1 to 3");
                }
                catch (OperationCanceledException oCe)
                {
                    Console.Clear();
                    Console.WriteLine(oCe.Message);
                    Console.WriteLine("=============================");
                    exceptionCounter++;
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Enter a number between 1 to 3");
                    Console.WriteLine("=============================");
                    exceptionCounter++;
                }
            } while (exceptionCounter != 0);
        }

        //This is where the game runs, if GameState is 0, the while loop of play will continue. 
        public static void Play()
        {
            do
            {
                Update();
                Action();
            } while (GameState == 0);
            GameState = 0;
        }

        //Update method clears the screen, and prints the map with player's location in square bracket. 
        //It also prints player's previous location
        public static void Update()
        {
            Console.Clear();
            Map.PrintCaverns(size);
            if(move != 0)
                Console.WriteLine($"==========================\nYou were in cavern: {Map.PlayerPreviousLoc()}");
            Map.CheckCavern(size);
            Console.WriteLine($"You are in Cavern:  {Map.PlayerCurrentLoc()}\n==========================");
            //PlayerPreviousLocation = Map.PlayerCurrentLoc();
        }
        //Action takes user input to perform an action. 
        public static void Action()
        {
            
            Console.WriteLine("Please choose your action: \n--------------------------");
            Console.WriteLine("1: Move " +
                "\n2: Shoot" +
                "\n3: Note" +
                "\n4: Trace" +
                "\n5: Cheat" +
                "\n--------------------------");
            string ActionInput = Console.ReadLine();
            Console.WriteLine();
            switch (ActionInput)
            {
                case "1":
                    MoveMenu();
                    GameState = Map.Move(size);
                    break;
                case "2":
                    ShootMenu();
                    GameState = Map.Shoot(size);
                    break;
                case "3":
                    Map.Note(size);
                    Console.WriteLine("enter any charater to get back in action");
                    Console.ReadLine();
                    Update();
                    break;
                case "4":
                    Map.Trace(size);
                    Console.WriteLine("enter any charater to get back in action");
                    Console.ReadLine();
                    Update();
                    break;
                case "5":
                    Map.Cheat(size);
                    Console.WriteLine("enter any charater to get back in action");
                    Console.ReadLine();
                    Update();
                    break;
                default:
                    Console.Clear();
                    Update();
                    break;
            }
        }
        //When you choose to move you invoke MoveMenu method first to show you the direction options.
        public static void MoveMenu()
        {
            Console.WriteLine("Please Choose your direction: \n-------------------------" +
                "\n1: Move up" +
                "\n2: Move down" +
                "\n3: Move right" +
                "\n4: Move left" +
                "\n-------------------------");
        }
        //When you choose to shoot you invoke ShootMenu method first to show you the direction options.
        public static void ShootMenu()
        {
            Console.WriteLine("Please CHoose your direction: \n-------------------------" +
                "\n1: Shoot above" +
                "\n2: Shoot below" +
                "\n3: shoot right" +
                "\n4: shoot left" +
                "\n-------------------------");
        }
    }
}
