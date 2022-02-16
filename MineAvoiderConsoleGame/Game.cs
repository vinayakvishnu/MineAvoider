using System;

  class Game
  {
      public static int parseCheck(string input)
      {
          Boolean success = false;
          Boolean parsed = false;
          int result = -1;

          while (!success)
          {
              parsed = Int32.TryParse(input, out result);
              if (parsed)
              {
                  success = true;
              }
              else
              {
                  Console.Write("Invalid entry. Please enter an integer: ");
                  input = Console.ReadLine();
              }
          }
          return result;
      }

      public static void Main(string[] args)
      {
          Boolean debug;
          string debugchoice;
          int cols;
          int rows;
          Boolean turn;
          string playerchoice;
          int row;
          int col;
          Boolean validdirection;
          string directionchoice;
          int direction;
          Boolean validscan;
          int scanchoice;
          string quitchoice;
          string again;
          Boolean repeat = true;

          while (repeat)
          {
              Console.WriteLine("MINEAVOIDER - Written by Vinayak Rajesh");
              Console.WriteLine("\nGameplay:\n    type 'sweep' to uncover a square\n    type 'rat' to count mines in a row or column\n    type 'flag' to place or remove a flag\n    type 'quit' to end the game\n");
              Console.WriteLine("- Selecting an invalid target will skip a turn as a penalty.\n- Try to finish in as few turns as possible.\n- The game will end when a mine explodes or only/no mines remain on the board.\n");
              
              debug = false;
              Console.Write("Would you like to enable Debug Mode? (y/n): ");
              debugchoice = Console.ReadLine();
              if (debugchoice == "y" || debugchoice == "Y")
              {
                  Console.WriteLine("Debug Mode Enabled.");
                  debug = true;
              }
              else
              {
                  Console.WriteLine($"Regular Gameplay Mode Selected.");
              }

              Console.WriteLine(); // Debug Selection -> Gameboard Setup

              Console.WriteLine("Gameboard Setup:");
              Console.Write("Enter a desired number of columns, between 3 and 10: ");
              cols = parseCheck(Console.ReadLine());
              while ((cols < 3) || (cols > 10))
              {
                  Console.Write("Invalid number of columns specified. Please enter an integer between 3 and 10: ");
                  cols = parseCheck(Console.ReadLine());
              }
              Console.Write("Enter a desired number of rows, between 3 and 10: ");
              rows = parseCheck(Console.ReadLine());
              while ((rows < 3) || (rows > 10))
              {
                  Console.Write("Invalid number of rows specified. Please enter an integer between 3 and 10: ");
                  rows = parseCheck(Console.ReadLine());
              }
              Board gameplay = new Board(cols, rows);
              gameplay.placeMines();

              Console.WriteLine(); // Gameboard Setup -> Gameplay            

              if (debug)
              {
                  Console.Write("Specify number of rats: ");
                  gameplay.setRats(parseCheck(Console.ReadLine()));
              }

              while ((gameplay.getFreeSquares() != 0) && (gameplay.getNumMines() != 0))
              {
                  Console.WriteLine();
                  turn = true;
                  gameplay.addTurn();
                  Console.WriteLine($"TURN {gameplay.getTurns()}");
                  if (debug)
                  {
                      gameplay.print();
                  }
                  else
                  {
                      gameplay.display();
                  }
                  Console.WriteLine($"Live Mines: {gameplay.getNumMines()}");
                  Console.WriteLine($"Rats Available: {gameplay.getRats()}");
                  Console.WriteLine($"Actions Taken: {gameplay.getGuesses()}");
                  Console.WriteLine();

                  while (turn)
                  {
                      validdirection = false;
                      validscan = false;
                      Console.Write("What would you like to do? (sweep/rat/flag/quit): ");
                      playerchoice = Console.ReadLine();
                      
                      if ((playerchoice == "rat") || (playerchoice == "RAT") || (playerchoice == "r") || (playerchoice == "R"))
                      {
                          Console.WriteLine("Rat Selected.");
                          if (gameplay.getRats() > 0)
                          {
                              direction = -1;
                              while (!validdirection)
                              {
                                  Console.Write("Enter rat instruction to sniff a column or row (c/r): ");
                                  directionchoice = Console.ReadLine();
                                  if ((directionchoice == "r") || (directionchoice == "R"))
                                  {
                                      direction = 0;
                                      validdirection = true;
                                  }
                                  else if ((directionchoice == "c") || (directionchoice == "C"))
                                  {
                                      direction = 1;
                                      validdirection = true;
                                  }
                                  else
                                  {
                                      Console.WriteLine("Invalid scan direction specified.");
                                  }
                              }
                              while (!validscan)
                              {
                                  Console.Write("Enter column/row number to be sniffed: ");
                                  scanchoice = parseCheck(Console.ReadLine());
                                  if ((direction == 0) && (scanchoice >= 0) && (scanchoice < gameplay.getRowLength()) || ((direction == 1) && (scanchoice >= 0) && (scanchoice < gameplay.getRowLength())))
                                  {
                                      gameplay.rat(direction, scanchoice);
                                      validscan = true;
                                  }
                                  else if (direction == 0)
                                  {
                                      Console.WriteLine($"Invalid input. Please type a number between 0 and {gameplay.getColumnLength() - 1}.");
                                  }
                                  else if (direction == 1)
                                  {
                                      Console.WriteLine($"Invalid input. Please type a number between 0 and {gameplay.getRowLength() - 1}.");
                                  }
                                  else
                                  {
                                      Console.WriteLine("Invalid input. Please type a number within the boundaries of the board.");
                                  }
                              }
                              turn = false;
                          }
                          else
                          {
                              Console.WriteLine("No rats remaining! Please select another option.");
                          }
                      }
                      else if ((playerchoice == "flag") || (playerchoice == "FLAG") || (playerchoice == "f") || (playerchoice == "F"))
                      {
                          Console.WriteLine("Flag Selected.");
                          Console.Write("Enter desired column number: ");
                          col = parseCheck(Console.ReadLine());
                          Console.Write("Enter desired row number: ");
                          row = parseCheck(Console.ReadLine());
                          gameplay.toggleFlag(col, row);

                          if (debug)
                          {
                              gameplay.print();
                          }
                          else
                          {
                              gameplay.display();
                          }
                      }
                      else if ((playerchoice == "quit") || (playerchoice == "QUIT") || (playerchoice == "q") || (playerchoice == "Q"))
                      {
                          Console.WriteLine("Are you sure you would like to quit? (y/n)");
                          quitchoice = Console.ReadLine();
                          if (quitchoice == "y" || quitchoice == "Y")
                          {
                              System.Environment.Exit(1);
                          }
                          else
                          {
                              Console.WriteLine("Quit Cancelled");
                              Console.WriteLine();
                          }
                      }
                      else
                      {
                          Console.WriteLine("Basic Sweep Selected.");
                          Console.Write("Enter a target column: ");
                          col = parseCheck(Console.ReadLine());
                          Console.Write("Enter a target row: ");
                          row = parseCheck(Console.ReadLine());
                          gameplay.guess(col, row);
                          turn = false;
                      }
                      if (gameplay.wasExplosion() && gameplay.checkAutoEnd())
                      {
                          if (debug)
                          {
                              gameplay.print();
                          }
                          else
                          {
                              gameplay.display();
                          }
                          Console.WriteLine("Aww... A mine has exploded. Game over!");

                          Console.WriteLine("Would you like to continue? (y/n)");
                          quitchoice = Console.ReadLine();
                          if (quitchoice == "y" || quitchoice == "Y")
                          {
                              gameplay.setAutoEnd(false);
                          }
                          else
                          {
                              gameplay.setNumMines(0);
                          }
                      }
                  }
              }

              Console.WriteLine(); // Gameplay -> Stats

              if (!gameplay.wasExplosion())
              {
                  Console.WriteLine($"Congratulations! You have uncovered all free squares.");
              }

              gameplay.print();
              Console.WriteLine($"Total Turns: {gameplay.getTurns()}");
              Console.WriteLine($"Free Squares Uncovered: {(gameplay.getColumnLength() * gameplay.getRowLength()) - gameplay.getFreeSquares()}");

              Console.WriteLine(); // Stats -> Again?

              Console.Write("Enter 'y' to play again: ");
              again = Console.ReadLine();
              if ((again != "y") && (again != "Y"))
              {
                  repeat = false;
              }
              Console.WriteLine();
              Console.WriteLine("------------------------------------------------------------");
              Console.WriteLine();
          }
      }
  }
