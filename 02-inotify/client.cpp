/***************************************
 * Simple CLI Client-Server Manager
 ***************************************/
#include <iostream>
#include <fstream>
#include <cstdlib>
#include <string>
using namespace std;

/***************************************
 * CHECKING INPUT
 ***************************************/
bool check(int argc, char *argv[]) {

	if (argc < 3) {
		cout << "\nAvast, ye landlubber! To enter me ship, type: ./client -i <ID>" << endl << endl; 
		return 0;
	}
	char * c;
	long i = strtol(argv[2], &c, 10);
	if (*c) {
		cout << "\nMe can't read \"" << argv[2] << "\", is not a numbarrr!" << endl << endl; 
		return 0;
	}
	if (i < 0) {
		cerr << "\nMe can't read negative " << i << endl << endl; 
		return 0;
	}
	else
		return 1;
}

/***************************************
 * CLIENT READS A COMMAND
 * Checks if it is "S", "H" or "L"
 * (sounds like a joke, but it isn't)
 ***************************************/
string readCommand() {
	string command = "";
	
	cout << "\nAhoy matey! choose yer rum:" << endl; 
	cout << "S, H or L?: "; 
	do {
		cin.clear();
		getline (cin, command);

		// Check the input
		if( command.compare("S") != 0 &&
			command.compare("s") != 0 &&
			command.compare("H") != 0 &&
			command.compare("h") != 0 &&
			command.compare("L") != 0 &&
			command.compare("l") != 0 ) {
				command = "0";
				cout << "Avast, ye landlubber! Please choose S, H or L?: ";
		}
	} while(command.compare("0") == 0);

	// If the input is correct
	command[0] = toupper(command[0]);
	cout << "Arrr! Here's what ye chose: " << command << endl << endl; 
	return command;
}

/***************************************
 * GET THE FILENAME
 ***************************************/
string getFilename(const string& filename) {
	unsigned found = filename.find_last_of("/\\");
	return filename.substr(found + 1);
}

/***************************************
 * MAIN
 * Where all the bad things happen
 ***************************************/
int main(int argc, char *argv[]) {

	if ( !check(argc, argv) ) 
		return 1;

	char * c;
	long i = strtol(argv[2], &c, 10);
	int counter = 0;
	string directory = "./queue";
	string command, filename, answer;

	do {
		command = readCommand();

		// If the ID is not 1 and the command is L, discard message.
		if ( i != 1 && command.compare("L") == 0 )
			cout << "Swim with the fishes, ye scurvy dog, mwahuahuahaha..." << endl;

		// Else, create message.
		else {
			// Build filename. If file exists, regenerate (not very elegant, BUT)
			do {
				char counter_str[10];
				sprintf(counter_str, "%05d", counter);

				filename = directory + "/" + argv[2] + "_" + command + "_" + string( counter_str ) + ".txt";

				counter++;
				if(counter > 99999)
					counter = 0;
			} while ( ifstream(filename.c_str()) );
	
			// Now that filename doesn't exist, create file
			ofstream file(filename.c_str());
			if (!file)
				cout << "Thar be nothin' in the sea! Unable to create ship" << endl;
			else {
				cout << "Ship ahoy! It's called '" << getFilename(filename) << "'." << endl;
				file.close();
			}
		}

		cout << "\nWrite 'aye' to drink moar rum or 'nope' to leave me ship: ";
		cin.clear();
		getline (cin, answer);

	} while(answer.compare("aye") == 0);

	return 0;
}
