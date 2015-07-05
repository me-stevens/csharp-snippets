/********************************
 * REVERSI / OTHELLO BOARD GAME
 * ******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello {

	class Program {

		static void Main(string[] args) {

			Console.WriteLine("\n------------------------------------");
			Console.WriteLine("         WELCOME TO REVERSI");
			Console.WriteLine("------------------------------------");

			Board board = new Board();
			board.opening(0);
			board.printBoard();

			Console.WriteLine("\n --- PRESS ENTER TO START ---");
			Console.ReadLine();

			// 32 fixed turns to fill the board (64 / 2 players)
			int good = 0;
			int turn = 0;
			do {
				turn++;
				Console.WriteLine(" --- Turn " + turn + " ---------------");
				good = board.newTurn();
				Console.Write("Press ENTER when ready: ");
				Console.ReadLine();
				Console.WriteLine();
			} while(turn < 32 && good == 0);
			
			board.andTheWinnerIs();
			Console.ReadLine();
		}

		/* *******************************
		 *  CLASS BOARD
		 * *******************************/
		public class Board {
			// To change the board size on runtime, make these non const
			private const int ROWS = 8;
			private const int COLS = 8;
			private int[,] board   = new int[ROWS, COLS];

			/* ******************************************************
			 *  CONSTRUCTOR
			 * ******************************************************/
			public Board() {
				// Fill the matrix with empty cells
				for (int i=0; i<board.GetLength(0); i++)
					for (int j=0; j<board.GetLength(1); j++)
						board[i, j] = -1;
			}

			/* ******************************************************
			 *  I'LL JUST LEAVE THIS HERE IN ORDER TO ADD 
			 *  OTHER OPENINGS IN THE FUTURE
			 * ******************************************************/
			public void opening(int i) {
				// Typical opening:
				if (i == 0) {
					board[3, 3] = 1;
					board[3, 4] = 0;
					board[4, 3] = 0;
					board[4, 4] = 1;
				}
				// Other openings go here
			}

			/* ******************************************************
			 *  PRINT BOARD (WITH AND WITHOUT AVAILABLE SQUARES)
			 * ******************************************************/
			public void printBoard() {
				// Dummy matrix. Not elegant =(
				// Also, me thinks it's filled with zeros (good)
				int[,] x = new int[ROWS, COLS];
				printBoard(x);
			}

			public void printBoard(int[,] moves) {
				int cols = 0;
				int rows = 1;

				Console.Write("\n\n\t  A B C D E F G H\n\t" + rows);
				for (int i=0; i<board.GetLength(0); i++) {
					for (int j=0; j<board.GetLength(1); j++) {

						// Empty or available to move
						if (board[i, j] == -1) {
							if (moves[i, j] == 1)
								Console.Write(" ?");
							else
								Console.Write("  ");
						}
						// Blacks		
						if (board[i, j] ==  0)
							Console.Write(" X");

						// Whites
						if (board[i, j] ==  1)
							Console.Write(" O");

						cols++;
						if (cols % COLS == 0) {
							rows++;
							if (rows <= ROWS)
								Console.Write("\n\t" + rows);
						}
					}
				}
				Console.WriteLine("\n");
			}

			/* ******************************************************
			 *  FROM INDEXES (I,J) TO LETTER-NUMBER
			 * ******************************************************/
			public string index2letter(int i, int j) {
				if (i >= 0 && i < ROWS && j >= 0 && j < COLS) {
					i++;
					switch (j) {
						case 0:
							return "A" + i;
						case 1:
							return "B" + i;
						case 2:
							return "C" + i;
						case 3:
							return "D" + i;
						case 4:
							return "E" + i;
						case 5:
							return "F" + i;
						case 6:
							return "G" + i;
						case 7:
							return "H" + i;
						default:
							return "Wrong input";
					}
				} else
					return "Wrong input";
			}

			/* ******************************************************
			 *  FROM LETTER-NUMBER TO INDEXES (I, J)
			 * ******************************************************/
			public int letter2index(string position, out int i, out int j) {
				i = 0;
				j = 0;

				if (position.Length != 2) {
					Console.WriteLine("Type the letter of a column and the number of a row, for example: A1");
					return -1;
				}

				string letter   = position.Substring(0, 1);
				string number   = position.Substring(1, 1);
				Boolean success = true;
				
				// Check letter
				switch (letter.ToUpper()) {
					case "A":
						j = 0;
						break;
					case "B":
						j = 1;
						break;
					case "C":
						j = 2;
						break;
					case "D":
						j = 3;
						break;
					case "E":
						j = 4;
						break;
					case "F":
						j = 5;
						break;
					case "G":
						j = 6;
						break;
					case "H":
						j = 7;
						break;
					default:
						success = false;
						break;
				}
				if (!success) {
					Console.WriteLine("Type a valid column letter (from A to H)");
					return -1;
				}
				
				// Check number
				try {
					i = Convert.ToInt32(number) - 1;
				} catch (FormatException e) {
					Console.WriteLine("Your second digit is not a row number.");
					return -1;
				} catch (OverflowException e) {
					Console.WriteLine("That row number is too big!.");
					return -1;
				}

				if (i < 0 || i >= ROWS) {
					Console.WriteLine("Introduce a valid row number (between 1 and 8)");
					return -1;
				}

				return 0;
			}

			/* ******************************************************
			 *  CALCULATE AVAILABLE SQUARES
			 *  BEWARE: NON-ELEGANT CODE. WILL MAKE YOUR EYES BLEED
			 * ******************************************************/
			public int[,] calculateMoves(Boolean thisPlayer, out int found) {

				// Opposite color
				int oppColor = 1;
				if (thisPlayer)
					oppColor = 0;
						
				int[,] possibilities = new int[ROWS,COLS];
				found = 0;

				for (int i=0; i<board.GetLength(0); i++) {
					for (int j=0; j<board.GetLength(1); j++) {

						// Search for a chip of opposite color
						if (board[i, j] == oppColor) {

							// Start search for surrounding empty cells to place chip
							for (int m=i-1; m<=i+1; m++) {
								for (int n=j-1; n<=j+1; n++) {

									// Check that we don't leave the board limits and the square is empty:
									if (m >= 0 && n >= 0 && m <ROWS && n<COLS && board[m, n] == -1) {

										// Set increments to advance in the right direction
										// Gives -1, 0 or 1 automatically
										int deltam   = i-m;
										int deltan   = j-n;
										Boolean exit = false;

										// Start search for a chip of the same color as condition to place chip
										int k = i + deltam;
										int l = j + deltan;

										// check we keep inside limits and squares are not empty or sameColor
										while (k >= 0 && k < ROWS && l>=0 && l < COLS && !exit) { 

											int sameColor = 0;
											if (thisPlayer)
												sameColor = 1;

											// If square is empty, there's no reason to continue searching
											if (board[k, l] == -1)
												exit = true;

											// If same color as player, square is a possible placement for the chip
											else if (board[k, l] == sameColor) {
												possibilities[m, n] = 1;
												found++;
												exit = true;
											}

											// Else, chip is opposite color, continue
											else 
												exit = false;

											k += deltam;
											l += deltan;
										} // WHILE
									} // BOARD LIMITS
								} // FOR
							} // FOR
						} // OPPOSITE COLOR FOUND
					} // FOR
				} // FOR

				// And this is what your brain does when you play =)
				// (stupid computers...)
				return possibilities;
			}

			/* *******************************************************
			 *  PLACE chip ON BOARD AND FLIP LIKE THERE'S NO TOMORROW
			 *  (MORE UGLY CODE)
			 * *******************************************************/
			public int flipParty(Boolean thisplayer, int i, int j) {
				if (i < 0 || j < 0 || i >= ROWS || j >= COLS) {
					Console.WriteLine("The square " + index2letter(i,j) + " does not exist in the board!");
					return -1;
				}

				if (board[i, j] != -1) {
					Console.WriteLine("The square " + index2letter(i,j) + " is not empty!");
					return -1;
				}
				
				// Place disc in square
				int oppColor = 0;
				if (thisplayer)
					board[i, j] = 1;
				else {
					board[i, j] = 0;
					oppColor    = 1;
				}

				// Start the flip party:
				for (int m=i-1; m<=i+1; m++) {
					for (int n=j-1; n<=j+1; n++) {
	
						// Search for a chip of the opposite color
						if (m >= 0 && n >= 0 && m < ROWS && n < COLS && board[m, n] == oppColor) {
					
							int[,] flipThese = new int[ROWS, COLS];

							// See if there's a chip of the player's color
							int deltam    = m-i;
							int deltan    = n-j;
							Boolean exit  = false;
							Boolean found = false;

							// Advance in (m, n) direction
							int k = m + deltam;
							int l = n + deltan;
		
							while (k >= 0 && k < ROWS && l>=0 && l < COLS && !exit) {

								// If square is empty, there's no reason to continue searching
								if (board[k, l] == -1)
									exit = true;
								

								// If same color as player, exit and flip opposites
								else if (board[k, l] == board[i, j]) {
									found = true;
									exit  = true;
								}

								// Else, chip is opposite color, save position andcontinue
								else {
									exit = false;
									flipThese[k, l] = 1;
								}

								k += deltam;
								l += deltan;
							}

							// If the exit is because I found a player's chip
							if (found) {

								// Add the first we found
								flipThese[m, n] = 1;
								for (int u = 0; u < flipThese.GetLength(0); u++)
									for (int v = 0; v < flipThese.GetLength(1); v++)
										
										// Change the color of the chips in the stored positions to the player's color
										if (flipThese[u, v] == 1)
											board[u, v] = board[i, j];
							} // IF FOUND
						} // IF
					} // FOR
				} // FOR

				return 0;
			}

			/* **********************************
			 *  TURN METHOD
			 * **********************************/
			public int newTurn() {

				// Player starts with blacks (false)
				Boolean player = false;
				int found      = 0;
				int noMorePos  = 0;
				int[,] moves   = calculateMoves(player, out found);

				// Available squares
				if (found == 0) {
					Console.WriteLine("No positions available, you loose 1 turn");
					noMorePos++;
				} else {
					Console.Write("\nAvailable squares: ");
					for (int i=0; i<moves.GetLength(0); i++)
						for (int j=0; j<moves.GetLength(1); j++)
							if (moves[i, j] == 1)
								Console.Write(index2letter(i, j) + " ");

					printBoard(moves);

					// Choose a square
					Boolean repeat = false;
					int row = 0, col = 0;
					do {
						Console.Write("Place your chip in square: ");
						string answer = Console.ReadLine();

						if (letter2index(answer, out row, out col) == -1) {
							repeat = true;
						} else if (moves[row, col] == 0) {
							Console.WriteLine("You can not place your chip there");
							repeat = true;
						} else if (moves[row, col] == 1)
							repeat = false;

					} while (repeat);

					// Place chip
					if (flipParty(player, row, col) == -1) {
						Console.WriteLine("Exiting game");
						return -1;
					}
				}

				Console.WriteLine("Updated board:");
				printBoard();

				// Computer ------------------------------------------------------------
				Console.Write("My turn now");
				for (int i = 0; i<5; i++) {
					Console.Write(".");
					System.Threading.Thread.Sleep(500);
				}
				Console.WriteLine();

				// Available squares
				moves = calculateMoves(!player, out found);

				if (found == 0) {
					Console.WriteLine("No positions available, I loose 1 turn");
					noMorePos++;
				} else {
					// Choose a square
					ArrayList x = new ArrayList();
					for (int i=0; i<moves.GetLength(0); i++)
						for (int j=0; j<moves.GetLength(1); j++)
							if (moves[i, j] == 1)
								x.Add(new int[] { i, j });

					Random r = new Random();
					int[] y  = (int[]) x[r.Next(0, x.Count)];

					// Place chip
					if (flipParty(!player, y[0], y[1]) == -1) {
						Console.WriteLine("Exiting game");
						return -1;
					}
				}
				Console.WriteLine("Updated board:");
				printBoard();

				if (noMorePos == 2) {
					Console.WriteLine("No more positions for any of the players. Exiting game");
					return -1;
				}
				return 0;
			}

			/* **********************************
			 *  YEAH, BUT, WHO'S THE WINNER
			 * **********************************/
			public void andTheWinnerIs() {
				int blacks = 0;
				int whites = 0;

				foreach (int cell in board) {
					if (cell == 0)
						blacks++;
					else if (cell == 1)
						whites++;
				}

				if (blacks > whites)
					Console.WriteLine("--- THE BLACKS WIN! ---");
				else
					Console.WriteLine("--- THE WHITES WIN! ---");
			}
		} // CLASS BOARD
	}
}
