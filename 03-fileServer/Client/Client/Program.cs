/***************************************
 * Simple CLI Client-Server Manager
 ***************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client {

	class Program {
		
		static void Main(string[] args) {
	
			// Checking input
			if (args.Length < 3) {
				Console.WriteLine("\nAvast, ye landlubber! To enter me ship, type: client.exe -i <ID>");
				Console.ReadLine();
				return;
			}
			int i;
			if ( !int.TryParse(args[2], out i) ) {
				Console.WriteLine("\nMe can't read \"" + args[2] + "\", is not a numbarrr!");
				Console.ReadLine();
				return;
			}
			if (i < 0) {
				Console.WriteLine("\nMe can't read negative " + i); 
				Console.ReadLine();
				return;
			}

			int counter = 0;
			string directory = "..\\..\\..\\..\\queue";
			string command, filename, answer;	
			
			do {
				command = readCommand();

				// If the ID is not 1 and the command is L, discard message.
				if ( i != 1 && String.Compare(command, "L") == 0 )
					Console.WriteLine("Swim with the fishes, ye scurvy dog, mwahuahuahaha...");

				// Else, create message.
				else {
					try{
						// Build filename. If file exists, regenerate (not very elegant, BUT)
						do {
							// The faster method for string concatenation is Join()
							filename = String.Join("_", new String[] { args[2], command.ToUpper(), counter.ToString("D5") });
							filename = String.Join("\\", new String[] { directory, filename });
							filename = String.Join(".", new String[] { filename, "txt" });
							counter++;
							if(counter > 99999)
								counter = 0;
						} while ( File.Exists(filename) );

						// Now that filename doesn't exist, create file
						// Preferred method or best practice for file creation in C#:
						using ( var newFile = File.Create(filename) ) {};
						
						Console.WriteLine("Ship ahoy! It's called '" + filename.Substring(filename.LastIndexOf("\\")+1) + "'."); 
					
					} catch (Exception e) {
						Console.WriteLine("Thar be nothin' in the sea! Unable to create ship");
						Console.WriteLine(e.ToString());
					}
				}
				
				Console.Write("\nWrite 'aye' to drink moar rum or 'nope' to leave me ship: ");
				answer = Console.ReadLine();

			} while(String.Compare(answer,"aye") == 0);
		}
	
		/***************************************
		* CLIENT READS A COMMAND
		* Checks if it is "S", "H" or "L"
		* (sounds like a joke, but it isn't)
		***************************************/
		public static string readCommand() {
			string command = "";
	
			Console.WriteLine("\nAhoy matey! choose yer rum:"); 
			Console.Write("S, H or L?: "); 
			do {
				command = Console.ReadLine();

				// Check the input
				if( String.Compare(command, "S", true) != 0 &&
					String.Compare(command, "H", true) != 0 &&
					String.Compare(command, "L", true) != 0) {
						command = "0";
						Console.Write("Avast, ye landlubber! Please choose S, H or L?: ");
				}
			} while(String.Compare(command, "0") == 0);

			// If the input is correct
			command = command.ToUpper();
			Console.WriteLine("Arrr! Here's what ye chose: " + command + "\n"); 
			return command;
		}
	}
}
