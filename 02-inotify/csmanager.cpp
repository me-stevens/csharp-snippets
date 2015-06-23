/***************************************
 * Simple CLI Client-Server Manager
 ***************************************/
#include <cstdlib> 

int main(int argc, char* argv[]) {
	system("gnome-terminal &"); // Client
	system("./server");         // Server
	return 0;
}
