/***************************************
 * Simple CLI Client-Server Manager
 * (Exit via CTRL+C)
 ***************************************/
#include <exception>
#include <iostream>
#include <stdlib.h>
#include <fstream>
#include "inotify-cxx.h"
using namespace std;

/***************************************
 * YE OLDEY ASCII ART!
 ***************************************/
void welcome() {
	cout << "\n\n         YE OLDE PIRATE' SERVER!" << endl << endl;
	cout << "                 uuuuuuu" << endl;
	cout << "             uu$$$$$$$$$$$uu" << endl;
	cout << "          uu$$$$$$$$$$$$$$$$$uu" << endl;
	cout << "         u$$$$$$$$$$$$$$$$$$$$$u" << endl;
	cout << "        u$$$$$$$$$$$$$$$$$$$$$$$u" << endl;
	cout << "       u$$$$$$$$$$$$$$$$$$$$$$$$$u" << endl;
	cout << "       u$$$$$$$$$$$$$$$$$$$$$$$$$u" << endl;
	cout << "       u$$$$$$\"   \"$$$\"   \"$$$$$$u" << endl;
	cout << "       \"$$$$\"      u$u       $$$$\"" << endl;
	cout << "        $$$u       u$u       u$$$" << endl;
	cout << "        $$$u      u$$$u      u$$$" << endl;
	cout << "         \"$$$$uu$$$   $$$uu$$$$\"" << endl;
	cout << "          \"$$$$$$$\"   \"$$$$$$$\"" << endl;
	cout << "            u$$$$$$$u$$$$$$$u" << endl;
	cout << "             u$\"$\"$\"$\"$\"$\"$u" << endl;
	cout << "  uuu        $$u$ $ $ $ $u$$       uuu" << endl;
	cout << " u$$$$        $$$$$u$u$u$$$       u$$$$" << endl;
	cout << "  $$$$$uu      \"$$$$$$$$$\"     uu$$$$$$" << endl;      
	cout << "u$$$$$$$$$$$uu    \"\"\"\"\"    uuuu$$$$$$$$$$" << endl;
	cout << "$$$$\"\"\"$$$$$$$$$$uuu   uu$$$$$$$$$\"\"\"$$$\"" << endl;
	cout << " \"\"\"      \"\"$$$$$$$$$$$uu \"\"$\"\"\"" << endl;
	cout << "           uuuu \"\"$$$$$$$$$$uuu" << endl;
	cout << "  u$$$uuu$$$$$$$$$uu \"\"$$$$$$$$$$$uuu$$$" << endl;
	cout << "  $$$$$$$$$$\"\"\"\"           \"\"$$$$$$$$$$$\"" << endl;
	cout << "   \"$$$$$\"                      \"\"$$$$\"\"" << endl;
	cout << "     $$$\"                         $$$$\"" << endl << endl;
}


/***************************************
 * BREAK THE FILENAME DOWN
 ***************************************/
void decode(const string& filename, string &id, string &command, string &counter, int digits) {
	unsigned found = filename.find("_");

	id      = filename.substr(0, found);
	command = filename.substr(found + 1, 1);
	counter = filename.substr(found + 3, digits);
}

/***************************************
 * SERVER CONTROL CENTER
 ***************************************/
void draw(string filename) {

	string id, command, counter;
	int digits = 5;
	decode(filename, id, command, counter, digits);

	if      ( command.compare("S") == 0 ) {
		cout << "                  .  ;  ; .                " << endl;
		cout << "                   '  .. '                 " << endl;
		cout << "     _|_          =- {  } -=       _|_     " << endl;                  
		cout << "    ``|`           .  .. .         `|``    " << endl;
		cout << "   ```|``         '  ;  ; '       ``|```   " << endl;                
		cout << "   `__!__    )'             '(    __!__`   " << endl;
		cout << "   :     := },;             ;,{ =:     :   " << endl;                
		cout << "   '.   .'                       '.   .'   " << endl;
		cout << "+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+" << endl;  
		cout << "|                                         |" << endl;
		cout << "|          Ship numbarrr: " << counter << "           |" << endl;
		cout << "|                                         |" << endl;
		cout << "+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+" << endl << endl;
	}

	else if ( command.compare("H") == 0 ) {
		cout << "                  .  ;  ; .                " << endl;
		cout << "                   '  .. '                 " << endl;
		cout << "     _|_          =- {  } -=       _|_     " << endl;                  
		cout << "    ``|`           .  .. .         `|``    " << endl;
		cout << "   ```|``         '  ;  ; '       ``|```   " << endl;                
		cout << "   `__!__    )'             '(    __!__`   " << endl;
		cout << "   :     := },;             ;,{ =:     :   " << endl;                
		cout << "   '.   .'                       '.   .'   " << endl;
		cout << "+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+" << endl;  
		cout << "                -----------                " << endl;
		cout << "               /           \\              " << endl;
		cout << "              /    Ship     \\             " << endl;
		cout << "             /   numbarrr:   \\            " << endl;
		cout << "             \\               /            " << endl;
		cout << "              \\    " << counter << "    /      " << endl;
		cout << "               \\           /              " << endl;
		cout << "                -----------                " << endl << endl;
	}

	else if ( command.compare("L") == 0 ) {
		if ( id.compare("1") == 0 )
			cout << "+~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~-=~~+" << endl << endl;
	else
		cout << "Yo Ho, Yo Ho! A pirates life for me!" << endl;
	}
}

/***************************************
 * KILL FILES
 ***************************************/
void keelhaul(string directory, string filename) {
	string path = directory;
	path += "/";
	path += filename;

	if ( ifstream(path.c_str()) ) {
		int success = remove(path.c_str());

		if (success == 0)
			cout << "Keelhauling " << path << endl;
		else
			cout << "Could not keelhaul " << path << endl;
	}
}

/***************************************
 * MAIN
 ***************************************/
int main(int argc, char *argv[]) {
	welcome();

	string directory = "./queue";
	try {
		Inotify notify;

		// Only check for creation and deletion
		InotifyWatch watch(directory, IN_CREATE | IN_DELETE);
		notify.Add(watch);

		// SUPER LEGAL INFINITE LOOP. Exit with CTRL + C
		cout << "\nArrr! I be watchin' you, " << directory << endl << endl;
		for (;;) {
			notify.WaitForEvents();

			size_t count = notify.GetEventCount();
			while (count > 0) {
				InotifyEvent event;
				bool got_event = notify.GetEvent(&event);

				if (got_event) {
					string eventype;
					event.DumpTypes(eventype);

					string filename = event.GetName();

					cout << "Avast, "          << directory << "!... ";
					cout << "Me eaye see: \""  << eventype  << "\". ";
					cout << "Ship o' name: \"" << filename  << "\"" << endl << endl;

					// Here's the parrrty
					if (eventype.compare("IN_CREATE") == 0) {
						draw(filename);
						keelhaul(directory, filename);
					}
				}

				count--;
			}
		}
	} catch (InotifyException &e) {
		cerr << "Inotify exception occured: " << e.GetMessage() << endl;
	} catch (exception &e) {
		cerr << "STL exception occured: "     << e.what() << endl;
	} catch (...) {
		cerr << "unknown exception occured"   << endl;
	}

	return 0;
}
