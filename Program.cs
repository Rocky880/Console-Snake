using System.CodeDom.Compiler;

Random rnd = new Random();

int[,] FullMap = {         //creates the base size of the map, the playable map is reset to this each loop so is necessary
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
};

int[,] PrintableMap = {          //this map is changed every game loop but reset after each one allowing changes without worrying about the integrity of the code
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0},
};

int[] SnakeX = { 6, 6, 6 };  // declares snakes starting x values
int[] SnakeY = { 4, 5, 6 };  // declares snakes starting y values

bool CherryAlive = false;
(int, int) cherryPos = (0, 0);
int points = 0;

void PrintTile(int Type) //is run for each tile when printed, changes colour based on what is in that position and prints the correct tile into the console
{
    switch (Type) //executes different lines of code based on what tile is in a pos
    {
        case 0: //floor
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" ");
            break;
        case 1: //snake
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(" ");
            break;
        case 2:  //cherry
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
            break;
        default: //visualises unaccepted values making debugging easier
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write(" ");
            break;
    }
}
(int[], int[]) Movement(string direction, List<(int, int)> Pos)   // returns new SnakeX and SnakeY lists after changing all the values inside
{
    int[] x = new int[SnakeX.Length]; //new lists that always match the size of the snake
    int[] y = new int[SnakeY.Length];
    (int, int) temp3 = (SnakeX[0], SnakeY[0]);
    Array.Copy(SnakeX, x, SnakeX.Length);  //makes the new list the exact same as SnakeX however its not referranced so they can be changed independant of eachother
    Array.Copy(SnakeY, y, SnakeY.Length);


    switch (direction) //different operations are needed for each seperate direction, switch case statements are more efficient than if ladders 
    {
        case "W": // if w is pressed
            try     // if you somehow glitch out of map catches the error
            {
                if (y[0] > 0)
                    temp3.Item2--;
                    if(!Pos.Contains(temp3)){
                        {  // ensuring player stays within the map
                            y[0]--;  // the y axis increases as you go down so decreasing the y value makes the snake go up
                            for (int i = 1; i < y.GetLength(0); i++)  //starts at 1 because index 0 is changed in the previous line
                            {
                                y[i] = SnakeY[i - 1]; // makes index 1 in y the same as index 0 of SnakeY so the second head body part is now where the head used to be
                                x[i] = SnakeX[i - 1];
                            }
                        }
                }
            }
            catch
            {
                Console.WriteLine("You die");
            }
            break;
        case "S":
            try
            {
                if (y[0] < FullMap.GetLength(0) - 1)
                {
                    temp3.Item2++;
                    if (!Pos.Contains(temp3))
                    {
                        y[0]++;
                        for (int i = 1; i < y.GetLength(0); i++)
                        {
                            y[i] = SnakeY[i - 1];
                            x[i] = SnakeX[i - 1];
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("You die");
            }
            break;
        case "A":
            try
            {

                if (x[0] > 0)
                {
                    temp3.Item1--;
                    if (!Pos.Contains(temp3))
                    {
                        x[0]--;
                        for (int i = 1; i < x.GetLength(0); i++)
                        {
                            y[i] = SnakeY[i - 1];
                            x[i] = SnakeX[i - 1];
                        }
                    }
                }

            }
            catch
            {
                Console.WriteLine("You die");
            }
            break;
        case "D":
            try
            {
                if (x[0] < 12)
                {
                    temp3.Item1++;
                    if (!Pos.Contains(temp3))
                    {
                        x[0]++;
                        for (int i = 1; i < x.GetLength(0); i++)
                        {
                            y[i] = SnakeY[i - 1];
                            x[i] = SnakeX[i - 1];
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("You die");
            }
            break;
    }
    return (x, y);
}
(int, int) temp = (SnakeX[SnakeX.GetLength(0) - 1], SnakeY[SnakeX.GetLength(0) - 1]);

do
{
    Console.BackgroundColor = ConsoleColor.Black;
    Console.SetCursorPosition(0, 0); //very top left, fitting place for title
    Console.Write("Snake"); //prints title
    Console.SetCursorPosition(0, 1);  //curser position changed so it doesnt affect game title


    List<(int, int)> snakePos = new List<(int, int)>(); //creates a list of all pairs of coordinates for the snake

    for (int i = 0; i < SnakeY.GetLength(0); i++)
    {
        PrintableMap[SnakeY[i], SnakeX[i]] = 1;  // the current snake positions are passed into the temp map
        (int, int) temp1 = (SnakeX[i], SnakeY[i]);
        snakePos.Add(temp1);
        if (CherryAlive == false)
        {
            do
            {
                cherryPos = (rnd.Next(0, 13), rnd.Next(0, 9));
            } while (snakePos.Contains(cherryPos) == true);


            CherryAlive = true;
        }
        PrintableMap[cherryPos.Item2, cherryPos.Item1] = 2;
    }

    for (int i = 0; i < 9; i++)
    {
        for (int j = 0; j < 13; j++)
        {

            PrintTile(PrintableMap[i, j]);    // the type of tile (0,1 or 2) is passed into a function. each type represents a different element of the game
        }                                     // i used a new function instead of an immediate switch case statement as it feels more closely related to object based programming so im trying to develop those skills
        Console.WriteLine();
    }
    // Array.Copy(FullMap,0, PrintableMap,0,FullMap.GetLength(0));    
    PrintableMap = FullMap.Clone() as int[,];          // i used clone to reset the temp map as copy isnt suitable for 2d arrays

    Console.SetCursorPosition(0, 13);  // so it doesnt print ontop of the game map
    switch (Console.ReadKey().Key)
    {   // reads the inut from the user
        case ConsoleKey.W: // if w is pressed
            temp = (SnakeX[SnakeX.GetLength(0) - 1], SnakeY[SnakeX.GetLength(0) - 1]);
            (SnakeX, SnakeY) = Movement("W", snakePos);
            snakePos[0] = (SnakeX[0], SnakeY[0]);  //updates snakepos so it can be checked against cherrypos
            break;
        case ConsoleKey.S:
            temp = (SnakeX[SnakeX.GetLength(0) - 1], SnakeY[SnakeX.GetLength(0) - 1]);
            (SnakeX, SnakeY) = Movement("S", snakePos);
            snakePos[0] = (SnakeX[0], SnakeY[0]);
            break;
        case ConsoleKey.A:
            temp = (SnakeX[SnakeX.GetLength(0) - 1], SnakeY[SnakeX.GetLength(0) - 1]);
            (SnakeX, SnakeY) = Movement("A", snakePos);
            snakePos[0] = (SnakeX[0], SnakeY[0]);
            break;
        case ConsoleKey.D:
            temp = (SnakeX[SnakeX.GetLength(0) - 1], SnakeY[SnakeX.GetLength(0) - 1]);
            (SnakeX, SnakeY) = Movement("D", snakePos);
            snakePos[0] = (SnakeX[0], SnakeY[0]);
            break;
    }

    if (snakePos[0] == cherryPos)
    {
        SnakeX = SnakeX.Concat(new int[] { temp.Item1 }).ToArray();
        SnakeY = SnakeY.Concat(new int[] { temp.Item2 }).ToArray();
        CherryAlive = false;
        points++;
    }
    Console.BackgroundColor = ConsoleColor.Black;
    Console.SetCursorPosition(6, 0);
    Console.WriteLine("Score: " + points);

} while (true);
