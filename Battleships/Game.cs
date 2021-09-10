using System;

namespace Battleships {
	class Game : Program {
		char[] lettersX = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'};
		char[] numbersY = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
		int[,] userShips = new int[10,10];
		bool[] userShipsAlive = {true, true, true};
		int[,] aiShips = new int[10,10];
		bool[] aiShipsAlive = {true, true, true};
		Random r = new Random();
		int currentShip;
		public void MainGame() {
			Console.WriteLine("New game started\n");
			currentGameState = 'p';
			// ship placement
			DrawGrid(userShips, true);
			Console.WriteLine("\nWhere would you like to place your first ship? (5 squares long)\nInput 'help' at any time during the program for information on valid commands");
			currentShip = 1;
			while (currentShip < 4) {
				string input = Input();
				switch (input) {
					case "help":
						Help(currentGameState);
						switch (currentShip) {
							case 1:
								Console.WriteLine("Where would you like to place your first ship? (5 squares long)");
								break;
							case 2:
								Console.WriteLine("Where would you like to place your second ship? (4 squares long)");
								break;
							case 3:
								Console.WriteLine("Where would you like to place your third ship? (4 squares long)");
								break;
						}
						break;
					case "my grid":
						DrawGrid(userShips, true);
						break;
					case "end":
						return;
					default:
						try {
							PlaceShip(currentShip, input);
						}
						catch {
							Console.WriteLine("Invalid input");
						}
						break;
				}
			}
			Console.WriteLine("All user ships placed successfully");
			try {
				GenerateShips();
				Console.WriteLine("All ai ships placed successfully");
			}
			catch {
				Console.WriteLine("ERROR: Could not place ai ships, new game required");
				return;
			}
			currentGameState = 'g';
			// game
			Console.WriteLine("\nInput 'ai grid' or 'my grid' at any time to see the respective game boards\nInput 'help' for information on how to proceed, or begin your attack");
			while (currentGameState == 'g') {
				string input = Input();
				switch (input) {
					case "help":
						Help(currentGameState);
						break;
					case "my grid":
						DrawGrid(userShips, true);
						break;
					case "ai grid":
						DrawGrid(aiShips, false);
						break;
					case "end":
						return;
					default:
						try {
							UserAttack(input);
						}
						catch {
							Console.WriteLine("Invalid input");
						}
						break;
				}
			}
			if (currentGameState == 'w') {
				Console.WriteLine("\nYou have won!");
				return;
			}
			else if (currentGameState == 'l') {
				Console.WriteLine("\nYou have lost");
				return;
			}
			else {
				Console.WriteLine("ERROR: Issue encountered, new game required");
				return;
			}
		}
		public void DrawGrid(int[,] ships, bool player) {
			Console.Write("  ");
			foreach (char letter in lettersX) {
				Console.Write(Char.ToUpper(letter) + " ");
				if (letter == 'j') {
					Console.WriteLine();
				}
			}
			for (int y = 0; y < 10; y++) {
				Console.Write(numbersY[y] + " ");
				for (int x = 0; x < 10; x++) {
					int position = ships[y,x];
					// depending on what is at the position a different symbol will be displayed
					switch (position) {
						case 0:
							// the square is empty
							Console.Write("~ ");
							break;
						case 1:
							// the square contains a ship
							goto case 3;
						case 2:
							// the square contains a ship
							goto case 3;
						case 3:
							// the square contains a ship
							if (player) {
								Console.Write("# ");
							}
							else {
								Console.Write("~ ");
							}
							break;
						case 8:
							// the square was a hit 
							Console.Write("X ");
							break;
						case 9:
							// the square was a miss
							Console.Write("O ");
							break;
						default:
							Console.WriteLine("\n\nERROR: Could not display grid, new game required");
							return;
					}
				}
				Console.WriteLine();
			}
		}
		public void PlaceShip(int ship, string strCoordinates) {
			string[] splitString = strCoordinates.Split();
			char[] coordinates1 = (splitString[0]).ToCharArray();
			char[] coordinates2 = (splitString[1]).ToCharArray();
			char direction;
			int shipLength;
			// simple check to see which way the ship is facing
			if (coordinates1[0] == coordinates2[0]) {
				direction = 'v';
			}
			else if (coordinates1[1] == coordinates2[1]) {
				direction = 'h';
			}
			else {
				Console.WriteLine("ERROR: Could not place ship, invalid coordinates");
				return;
			}
			if (ship == 1) {
				// ship is 5 squares
				shipLength = 5;
			}
			else if (ship == 2 || ship == 3) {
				// ship is 4 squares
				shipLength = 4;
			}
			else {
				Console.WriteLine("ERROR: Could not place ships, new game required");
				return;
			}
			// validation check for vertical coordinates
			if (direction == 'v') {
				int twinIndex1 = Array.IndexOf(lettersX, coordinates1[0]);
				int twinIndex2 = Array.IndexOf(lettersX, coordinates2[0]);
				if (twinIndex1 == -1 || twinIndex2 == -1) {
					Console.WriteLine("ERROR: Could not place ship, invalid coordinates");
					return;
				}
				else {
					int index1 = Array.IndexOf(numbersY, coordinates1[1]);
					int index2 = Array.IndexOf(numbersY, coordinates2[1]);
					if (Math.Abs(index1 - index2) == shipLength-1) {
						// place ship
						if (index1 > index2) {
							int temp = index1;
							index1 = index2;
							index2 = temp;
						}
						bool spaceClear = true;
						for (int i = index1; i <= index2; i++) {
							if (userShips[i, twinIndex1] != 0) {
								spaceClear = false;
							}
						}
						if (spaceClear == true) {
							for (int i = index1; i <= index2; i++) {
								userShips[i, twinIndex1] = currentShip;
							}
							currentShip++;
							Console.WriteLine("Coordinates accepted");
							switch (currentShip) {
								case 2:
									Console.WriteLine("Where would you like to place your second ship? (4 squares long)");
									break;
								case 3:
									Console.WriteLine("Where would you like to place your third ship? (4 squares long)");
									break;
							}
							return;
						}
						else {
							Console.WriteLine("ERROR: Could not place ship, space already occupied");
							return;
						}
					}
					else {
						Console.WriteLine("ERROR: Could not place ship, invalid coordinates");
						return;
					}
				}
			}
			// validation check for horizontal coordinates
			else if (direction == 'h') {
				int twinIndex1 = Array.IndexOf(numbersY, coordinates1[1]);
				int twinIndex2 = Array.IndexOf(numbersY, coordinates2[1]);
				if (twinIndex1 == -1 || twinIndex2 == -1 || twinIndex1 != twinIndex2) {
					Console.WriteLine("ERROR: Could not place ship, invalid coordinates");
				}
				else {
					int index1 = Array.IndexOf(lettersX, coordinates1[0]);
					int index2 = Array.IndexOf(lettersX, coordinates2[0]);
					if (Math.Abs(index1 - index2) == shipLength-1) {
						// place ship
						if (index1 > index2) {
							int temp = index1;
							index1 = index2;
							index2 = temp;
						}
						bool spaceClear = true;
						for (int i = index1; i <= index2; i++) {
							if (userShips[twinIndex1, i] != 0) {
								spaceClear = false;
							}
						}
						if (spaceClear == true) {
							for (int i = index1; i <= index2; i++) {
								userShips[twinIndex1, i] = currentShip;
							}
							currentShip++;
							Console.WriteLine("Coordinates accepted");
							switch (currentShip) {
								case 2:
									Console.WriteLine("Where would you like to place your second ship? (4 squares long)");
									break;
								case 3:
									Console.WriteLine("Where would you like to place your third ship? (4 squares long)");
									break;
							}
							return;
						}
						else {
							Console.WriteLine("ERROR: Could not place ship, space already occupied");
							return;
						}
					}
					else {
						Console.WriteLine("ERROR: Could not place ship, invalid coordinates");
					}
				}
			}
		}
		public void GenerateShips() {
			for (int i = 1; i < 4; i++) {
				int directionRand = r.Next(0, 2);
				int xRand = r.Next(0, 10);
				int yRand = r.Next(0, 10);
				int shipLength;
				int index1;
				int index2;
				if (i == 1) {
					// ship is 5 squares
					shipLength = 5;
				}
				else {
					// ship is 4 squares
					shipLength = 4;
				}
				if (directionRand == 0) {
					// vertical ship
					if (yRand <= 4) {
						index1 = yRand;
						index2 = yRand+(shipLength-1);
					}
					else {
						index1 = yRand-(shipLength-1);
						index2 = yRand;
					}
					bool spaceClear = true;
					for (int j = index1; j <= index2; j++) {
						if (aiShips[j, xRand] != 0) {
							spaceClear = false;
						}
					}
					if (spaceClear == true) {
						for (int j = index1; j <= index2; j++) {
							aiShips[j, xRand] = i;
						}
					}
					else {
						i--;
						continue;
					}
				}
				else {
					// horiziontal ship
					if (xRand <= 4) {
						index1 = xRand;
						index2 = xRand+(shipLength-1);
					}
					else {
						index1 = xRand-(shipLength-1);
						index2 = xRand;
					}
					bool spaceClear = true;
					for (int j = index1; j <= index2; j++) {
						if (aiShips[yRand, j] != 0) {
							spaceClear = false;
						}
					}
					if (spaceClear == true) {
						for (int j = index1; j <= index2; j++) {
							aiShips[yRand, j] = i;
						}
					}
					else {
						i--;
						continue;
					}
				}
			}
		}
		public void UserAttack(string strCoordinates) {
			char[] coordinates = (strCoordinates).ToCharArray();
			int index1 = Array.IndexOf(lettersX, coordinates[0]);
			int index2 = Array.IndexOf(numbersY, coordinates[1]);
			switch (aiShips[index2, index1]) {
				case 0:
					// the square is empty
					aiShips[index2, index1] = 9;
					Console.WriteLine("You missed");
					AiAttack();
					break;
				case 1:
					goto case 3;
				case 2:
					goto case 3;
				case 3:
					// the square contains a ship
					aiShips[index2, index1] = 8;
					Console.WriteLine("Successful hit!");
					aiShipsAlive = ShipCheck(aiShips, aiShipsAlive, false);
					AiAttack();
					break;
				case 8:
					// the square was a hit already
					Console.WriteLine("You have already attacked this square\nIt was a successful hit");
					break;
				case 9:
					// the square was a miss already
					Console.WriteLine("You have already attacked this square\nIt was a miss");
					break;
				default:
					Console.WriteLine("ERROR: Attack failed, new game required");
					return;
			}
		}
		public void AiAttack() {
			bool shot = false;
			while (shot == false) {
				int xShot = r.Next(0, 10);
				int yShot = r.Next(0, 10);
				if (userShips[yShot, xShot] == 8 || userShips[yShot, xShot] == 9) {
					continue;
				}
				shot = true;
				if (userShips[yShot, xShot] == 0) {
					userShips[yShot, xShot] = 9;
				}
				else {
					System.Console.WriteLine("\nYou have been hit");
					userShips[yShot, xShot] = 8;
				}
			}
		}
		public bool[] ShipCheck(int[,] ships, bool[] shipsAlive, bool player) {
			for (int i = 1; i < 4; i++) {
				if (shipsAlive[i-1] == false) {
					continue;
				}
				else {
					bool found = false;
					foreach (int ship in ships) {
						if (ship == i) {
							found = true;
							break;
						}
					}
					if (found == false) {
						shipsAlive[i-1] = false;
						if (player == false) {
							if (AllSank(shipsAlive)) {
								Console.WriteLine("All enemy ships have sunk!");
								currentGameState = 'w';
							}
							else {
								Console.WriteLine("An enemy ship has sunk!");
							}
						}
						else {
							if (AllSank(shipsAlive)) {
								Console.WriteLine("All enemy ships have sunk!");
								currentGameState = 'l';
							}
							else {
								Console.WriteLine("One of your ships has sunk");
							}
						}
						return shipsAlive;
					}
				}
			}
			return shipsAlive;
		}
		public bool AllSank(bool[] shipsAlive) {
			foreach (bool ship in shipsAlive) {
				if (ship) {
					return false;
				}
			}
			return true;
		}
	}
}