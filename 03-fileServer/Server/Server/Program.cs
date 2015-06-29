/***************************************
 * Simple CLI Client-Server Manager
 ***************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// File System Watcher
using System.IO;
using System.Security.Permissions;

namespace Server {

	class Program {

		static void Main(string[] args) {
			Program p = new Program();
			p.welcome();
			p.Run();
		}

		/***************************************
		* SERVER MAIN ROUTINE
		***************************************/
		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		public void Run() {

			// Create a new FileSystemWatcher and set the directory.
			FileSystemWatcher watcher = new FileSystemWatcher();
			watcher.Path = "..\\..\\..\\..\\queue";

			// Only check for creation and deletion
			watcher.NotifyFilter = NotifyFilters.FileName;
			
			// Watch any file.
			watcher.Filter = "*.*";

			// Add event handlers.
			watcher.Created += new FileSystemEventHandler(OnChanged);
			watcher.Deleted += new FileSystemEventHandler(OnChanged);

			// Begin watching.
			watcher.EnableRaisingEvents = true;

			// Wait for the user to quit the program.
			Console.WriteLine("Press \'q\' to quit.");
			while (Console.Read()!='q');
		}

		/***************************************
		* EVENT HANDLER
		***************************************/
		private void OnChanged(object source, FileSystemEventArgs e) {
			string fullpath = e.FullPath.Substring(0, e.FullPath.LastIndexOf("\\") );
			Console.Write("\nAvast, "        + fullpath     + "!... \n");
			Console.Write("Me eaye see: \'"  + e.ChangeType + "\'. ");
			Console.Write("Ship o' name: \'" + e.Name       + "\'\n\n");

			// Here's the parrrty
			if (String.Compare(e.ChangeType.ToString(), "Created") == 0) {
				draw(e.Name);
				keelhaul(e.FullPath);
			}
		}

		/***************************************
		* SERVER CONTROL CENTER
		***************************************/
		private void draw(string filename) {
			string id = "", command = "", counter = "";
			decode(filename, out id, out command, out counter);

			if (String.Compare(command, "S", true) == 0) {
				Console.WriteLine("                  .  ;  ; .                ");
				Console.WriteLine("                   '  .. '                 ");
				Console.WriteLine("     _|_          =- {  } -=       _|_     ");
				Console.WriteLine("    ``|`           .  .. .         `|``    ");
				Console.WriteLine("   ```|``         '  ;  ; '       ``|```   ");
				Console.WriteLine("   `__!__    )'             '(    __!__`   ");
				Console.WriteLine("   :     := },;             ;,{ =:     :   ");
				Console.WriteLine("   '.   .'                       '.   .'   ");
				Console.WriteLine("+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+");
				Console.WriteLine("|                                         |");
				Console.WriteLine("|          Ship numbarrr: " + counter + "           |");
				Console.WriteLine("|                                         |");
				Console.WriteLine("+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+\n");
			} else if (String.Compare(command, "H", true) == 0) {
				Console.WriteLine("                  .  ;  ; .                ");
				Console.WriteLine("                   '  .. '                 ");
				Console.WriteLine("     _|_          =- {  } -=       _|_     ");
				Console.WriteLine("    ``|`           .  .. .         `|``    ");
				Console.WriteLine("   ```|``         '  ;  ; '       ``|```   ");
				Console.WriteLine("   `__!__    )'             '(    __!__`   ");
				Console.WriteLine("   :     := },;             ;,{ =:     :   ");
				Console.WriteLine("   '.   .'                       '.   .'   ");
				Console.WriteLine("+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+");
				Console.WriteLine("                -----------                ");
				Console.WriteLine("               /           \\              ");
				Console.WriteLine("              /    Ship     \\             ");
				Console.WriteLine("             /   numbarrr:   \\            ");
				Console.WriteLine("             \\               /            ");
				Console.WriteLine("              \\    " + counter + "    /      ");
				Console.WriteLine("               \\           /              ");
				Console.WriteLine("                -----------                \n");
			} else if (String.Compare(command, "L", true) == 0) {
				if (String.Compare(id, "1") == 0)
					Console.WriteLine("+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+\n");
				else
					Console.WriteLine("Yo Ho, Yo Ho! A pirates life for me!");
			}
		}

		/***************************************
		* DECODE FILENAME
		***************************************/
		private void decode(string filename, out string id, out string command, out string counter) {
			int found = filename.IndexOf("_");
			id        = filename.Substring(0, found);
			command   = filename.Substring(found + 1, 1);
			counter   = filename.Substring(found + 3, 5);
		}

		/***************************************
		* KILL FILES
		***************************************/
		private void keelhaul(string path) {
			try {
				Console.WriteLine("Keelhauling " + path);
				File.Delete(path);
			} catch (Exception e) {
				Console.WriteLine("Could not keelhaul " + path);
				Console.WriteLine(e.Message);
			}
		}

		/***************************************
		 * YE OLDEY ASCII ART!
		***************************************/
		public void welcome() {
			Console.WriteLine("\n\n         YE OLDE PIRATE' SERVER!\n");
            Console.WriteLine("                 uuuuuuu");
            Console.WriteLine("             uu$$$$$$$$$$$uu");
            Console.WriteLine("          uu$$$$$$$$$$$$$$$$$uu");
            Console.WriteLine("         u$$$$$$$$$$$$$$$$$$$$$u");
            Console.WriteLine("        u$$$$$$$$$$$$$$$$$$$$$$$u");
            Console.WriteLine("       u$$$$$$$$$$$$$$$$$$$$$$$$$u");
            Console.WriteLine("       u$$$$$$$$$$$$$$$$$$$$$$$$$u");
            Console.WriteLine("       u$$$$$$\"   \"$$$\"   \"$$$$$$u");
            Console.WriteLine("       \"$$$$\"      u$u       $$$$\"");
            Console.WriteLine("        $$$u       u$u       u$$$");
            Console.WriteLine("        $$$u      u$$$u      u$$$");
            Console.WriteLine("         \"$$$$uu$$$   $$$uu$$$$\"");
            Console.WriteLine("          \"$$$$$$$\"   \"$$$$$$$\"");
            Console.WriteLine("            u$$$$$$$u$$$$$$$u");
            Console.WriteLine("             u$\"$\"$\"$\"$\"$\"$u");
            Console.WriteLine("  uuu        $$u$ $ $ $ $u$$       uuu");
            Console.WriteLine(" u$$$$        $$$$$u$u$u$$$       u$$$$");
            Console.WriteLine("  $$$$$uu      \"$$$$$$$$$\"     uu$$$$$$");      
            Console.WriteLine("u$$$$$$$$$$$uu    \"\"\"\"\"    uuuu$$$$$$$$$$");
            Console.WriteLine("$$$$\"\"\"$$$$$$$$$$uuu   uu$$$$$$$$$\"\"\"$$$\"");
            Console.WriteLine(" \"\"\"      \"\"$$$$$$$$$$$uu \"\"$\"\"\"");
            Console.WriteLine("           uuuu \"\"$$$$$$$$$$uuu");
            Console.WriteLine("  u$$$uuu$$$$$$$$$uu \"\"$$$$$$$$$$$uuu$$$");
            Console.WriteLine("  $$$$$$$$$$\"\"\"\"           \"\"$$$$$$$$$$$\"");
            Console.WriteLine("   \"$$$$$\"                      \"\"$$$$\"\"");
            Console.WriteLine("     $$$\"                         $$$$\"\n");
		}
	}
}
