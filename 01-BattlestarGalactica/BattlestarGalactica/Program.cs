/***************************************
 * BATTLESTAR GALACTICA
 ***************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattlestarGalactica {

	class Program {

		static void Main(string[] args) {

			// To call the non-static methods
			Program p = new Program();
			
			// HEADER
			p.TypeWrite("\n----------------------------------\n");
			p.TypeWrite("\n        BATTLESTAR GALACTICA\n");
			p.TypeWrite("\n----------------------------------\n");
			System.Threading.Thread.Sleep(30);
			p.TypeWrite("\nAttention! Cylon troops on sight!");
			p.TypeWrite("\nCondition 1 set throughout the fleet!");
			p.TypeWrite("\nGalactica's best pilots are sent to battle!\r\n\n");

			// Create a fleet of 20 Raiders
			Battleship[,] cylonRaiderSquadron = new CylonRaider[4, 5];
			Random rnd = new Random();
			int id = 0;
			for (int i = 0; i<cylonRaiderSquadron.GetLength(0); i++) {
				for (int j = 0; j<cylonRaiderSquadron.GetLength(1); j++) {
					cylonRaiderSquadron[i, j] = new CylonRaider(id, rnd.Next(1, 50));
					id++;
				}
			}

			Console.WriteLine("{0} raiders are coming at us!!", CylonRaider.CylonRaiderCounter);
			CylonRaider.PrintSquadron(cylonRaiderSquadron);
			Console.WriteLine();

			// Create some vipers
			Viper viper1         = new Viper("Starbuck", "DIIIIIEEEEE MUTHERFUCKAAAASS!!!", 200);
			Viper viper2         = new Viper("Apollo", "Target on sight, GO! GO! GO!!!", 100);
			Viper viper3         = new Viper("Katraine", "Take that, you bag of shit!!", 70);
			Battleship[,] vipers = new Viper[1, 3] { { viper1, viper2, viper3 } };

			Console.WriteLine("\nPress Enter to start shooting!");
			Console.ReadLine();

			// START OF Combat ------------------------------------------------------------------			
			if ( p.Combat(cylonRaiderSquadron, vipers) == 0) {
				
				Console.WriteLine("\n----------------------------------------");
				Console.WriteLine("\n\nVIPER SURVIVORS:\n");
				foreach (Viper viper in vipers)
					if (viper.Life > 0)
						Console.WriteLine("   {0}, life: {1}", viper.Pilot, viper.Life);

				Console.WriteLine("\n\nCYLON SURVIVORS:\n");
				Console.WriteLine("   " + CylonRaider.CylonRaiderCounter + " raiders.");
				p.TypeWrite("\n\n\tWe'll live to see another day.\n");
			}
			// END OF Combat ------------------------------------------------------------------

			Console.ReadLine();
		}

		/* *********************************************************
		*                       METHOD TypeWrite
		* *********************************************************/
		public void TypeWrite(string text) {
			for (int i = 0; i<text.Length; i++) {
				Console.Write(text[i]);
				System.Threading.Thread.Sleep(30);
			}
		}

		/* *********************************************************
		*                       METHOD Combat
		* *********************************************************/
		public int Combat(Battleship[,] cylonRaiderSquadron, Battleship[,] vipers) {
			int turn = 0;

			foreach (CylonRaider raider in cylonRaiderSquadron) {
				turn++;
				Console.WriteLine("\n-----------------------");
				Console.WriteLine("\n\tROUND {0}", turn);
				Console.WriteLine("\n-----------------------\n");
								
				// One raider shoots one of the vipers
				if (raider.Life > 0) {
					raider.Shoot(vipers);
				}

				if (Viper.viperCounter == 0) {
					Console.WriteLine("\n----------------------------------------");
					TypeWrite("\n  Cylons win, Human race is wiped out\n\tof the galaxy.\n");
					Console.WriteLine("\n----------------------------------------");
					return 1;
				}//----------------------------------

				foreach (Viper viper in vipers) {
					if (viper.Life > 0) {
						Console.WriteLine("\n" + viper.Pilot + ": " + viper.PilotWarCry);

						// One viper shoots one of the cylons
						viper.Shoot(cylonRaiderSquadron);
						if (CylonRaider.CylonRaiderCounter == 0) {
							Console.WriteLine("\n----------------------------------------");
							TypeWrite("\n  Humans win, Cylons are wiped out\n\tof the galaxy  ...FOR NOW.\n");
							Console.WriteLine("\n----------------------------------------");
							return 2;
						}//-----------------------------------
					}
				}

				Console.WriteLine("\nThere are {0} cylons alive!!", CylonRaider.CylonRaiderCounter);
				CylonRaider.PrintSquadron(cylonRaiderSquadron);

				// Output pseudo control
				if (turn % 5 == 0 && turn % 20 != 0) {
					Console.Write("\nPress enter for 5 more turns ");
					Console.ReadLine();
				}
			}
			return 0;
		}
	}


	/* *********************************************************
	 * CLASS BATTLESHIP
	 * *********************************************************/
	class Battleship {

		// The special way C sharp has to define an attribute
		// as well as its setter and getter in one go:
		public int Life {
			get;
			set;
		}

		public Battleship() : this(10) {
		}

		public Battleship(int Life) {
			this.Life = Life;
		}

		virtual public void Shoot(Battleship[,] targets) {
			Random rnd    = new Random();
			Boolean found = true;
			do {
				int i     = rnd.Next(0, targets.GetLength(0));
				int j     = rnd.Next(0, targets.GetLength(1));
				if (targets[i, j].Life > 0) {
					Console.Write("Hit!!");
					targets[i, j].Receive();
					found = false;
				}
			} while (found);
		}

		virtual public void Receive() {
			// Implemented by child classes
		}
	}

	/* *********************************************************
	 * CLASS CYLONRAIDER
	 * *********************************************************/
	class CylonRaider : Battleship {

		// Battery of spaceships
		public static int CylonRaiderCounter = 0;

		// Pilot and ship are one in CylonRaiders
		private readonly int ID;

		public CylonRaider(int ID) : this(ID, 5) {
		}

		public CylonRaider(int ID, int Life) : base(Life) {
			this.ID = ID;
			CylonRaiderCounter++;
		}

		public int getID() {
			return ID;
		}

		override public void Receive() {
			Life /= 2;
			Console.WriteLine(" Cylon {0}'s life reduced to {1}", ID, Life);
			if (Life <= 0) {
				CylonRaiderCounter--;
				Console.WriteLine("\tR. I. P. Cylon {0}", ID);
			}
		}

		public static void PrintSquadron(Battleship[,] squadron) {
			int col = 0;
			Console.Write("\n\t");
			foreach (CylonRaider ship in squadron) {
				if (ship.Life <= 0)
					Console.Write("X ");
				else
					Console.Write("C ");

				col++;
				if (col % 5 == 0)
					Console.Write("\n\n\t");
			}
		}
	}

	/* *********************************************************
	 * CLASS VIPER
	 * *********************************************************/
	class Viper : Battleship {

		public static int viperCounter = 0;

		// Vipers may have many pilots
		public string Pilot {
			get;
			set;
		}

		public string PilotWarCry {
			get;
			set;
		}

		public Viper(string Pilot, string PilotWarCry) : this(Pilot, PilotWarCry, 10) {
		}

		public Viper(string Pilot, string PilotWarCry, int Life) : base(Life) {
			this.Pilot = Pilot;
			this.PilotWarCry = PilotWarCry;
			viperCounter++;
			Console.WriteLine("{0}, GO AHEAD!", Pilot);
		}

		override public void Receive() {
			Life /= 2;
			Console.WriteLine(" {0}'s life reduced to {1}", Pilot, Life);
			if (Life <= 0) {
				viperCounter--;
				Console.WriteLine("\tR. I. P. {0}", Pilot);
			}
		}
	}
}
