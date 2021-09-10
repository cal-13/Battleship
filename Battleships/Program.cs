// callum gray
// simple battleships for newcastle building society application

using System;

namespace Battleships {
	class Program {
		public static char currentGameState = 'm';
		static void Main(string[] args) {
			currentGameState = 'm';
			// main menu
			Console.WriteLine("\n------------------------\n Welcome to Battleships \n------------------------\n");
			Console.WriteLine("Input 'new' to begin a game\nInput 'end' to exit");
			bool startLoop = true;
			while (startLoop == true) {
				string input = Input();
				switch (input) {
					case "help":
						Help(currentGameState);
						break;
					case "new":
						Game game = new Game();
						game.MainGame();
						currentGameState = 'm';
						break;
					case "end":
						startLoop = false;
						break;
					default:
						Console.WriteLine("Invalid input");
						break;
				}
			}
		}
		public static string Input() {
			Console.Write("\n> ");
			string x = Console.ReadLine();
			Console.WriteLine("");
			return x.ToLower();
		}
		public static void Help(char gameState) {
			// will output different valid commands depending on what the user is doing
			switch (gameState) {
				case 'm':
					Console.WriteLine("Input 'new' to begin a new game\nInput 'end' to exit the program");
					break;
				case 'p':
					Console.WriteLine("You must place three ships on the grid shown above, one battle ship (5 squares in length) and two destroyers (4 squares in length)\nInput two sets of coordinates seperated by a space to indicate each side of the boat being placed, for example, 'A1 A5'\nInput 'my grid' to see an updated version of your grid\n	Ships on your grid will be shown using the '#' symbol\n");
					break;
				case 'g':
					Console.WriteLine("To win the game destroy all the enemy ships\nIf the enemy destroys your ships first then you will lose\n\nInput a set of coordinates to attack the enemy on that square, for example, 'A1'\nInput 'my grid' to see your grid, input 'ai grid' to see the enemy grid\n	On your grid living ships will be shown using the '#' symbol\n	On either grid a miss will be shown using the 'O' symbol, and a hit will be shown using the 'X' symbol\nInput 'end' to end the game\n");
					break;
			}
		}
	}
}