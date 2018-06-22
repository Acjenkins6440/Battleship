using System;
using System.Text.RegularExpressions;

namespace Battleship
{
	class Game
	{
		private static string[] ships = new string[2]{"Scout", "Battleship"};
		static int xCoord = 0;
		static int yCoord = 0;
		static Player player1;
		static Player player2;
		public static void Main()
		{
			InitializeGame();
			int isValidChoice1 = 0;
			bool isValidChoice2 = false;
			bool isValidChoice3 = false;
			bool gameIsActive = false;
			bool step1 = false;
			bool nextPlayer;
			Player playerVariable = player1;

			while(playerVariable.areShipsEmpty() == false)
			{
				Console.Clear();
				Board.DisplayBoard(playerVariable.board);
				if(playerVariable.areShipsFull())
				{
					Console.WriteLine("Player {0}, you must choose where to put your ships", (playerVariable == player1) ? 1 : 2);
				}
				Console.WriteLine("Which ship would you like to place?");
				if (playerVariable.areShipsEmpty() == false)
				{
					isValidChoice1 = 0;
					isValidChoice2 = false;
					isValidChoice3 = false;
				}
				while(isValidChoice1 == 0)
				{
					Errors.WriteErrorMessage();
					playerVariable.listShips();
					string shipChoice = Console.ReadLine().ToLower();
					isValidChoice1 = SelectShip(shipChoice, playerVariable);
				}
				Console.WriteLine("Select starting coordinates in the following format: x,y");

				int shipLength = playerVariable.realShips[isValidChoice1 - 1].Length;

				while(isValidChoice2 == false)
				{
					Errors.WriteErrorMessage();
					try
					{
						string playerStartCoords = Console.ReadLine().ToLower();
						isValidChoice2 = PlaceShip(playerStartCoords, playerVariable);
					}
					catch(Exception)
					{
						Console.WriteLine("You can only enter numbers between 0 and 9, separated by a comma to set the start point of your ship.");
						Console.WriteLine("Try again.");
					}
				}
				Console.WriteLine("Excellent, now choose whether you want it facing N, E, S, or W.");
				while(isValidChoice3 == false)
				{
					Errors.WriteErrorMessage();
					try
					{
						string direction = Console.ReadLine().ToLower();
						isValidChoice3 = playerVariable.board.SetShipDirection(xCoord, yCoord, direction, shipLength, playerVariable.board, playerVariable);
					}
					catch(FormatException)
					{
						Errors.ErrorMessage = "Please only write 'N', 'E', 'S', or 'W'";
					}

				}
				if (playerVariable.areShipsEmpty() == true)
				{
					Console.Clear();
					Board.DisplayBoard(playerVariable.board);
					Errors.WriteErrorMessage();
					InfoMessages.WriteInfoMessage();
					if (playerVariable == player1)
					{
						Console.WriteLine("Look at your board and press any key + Enter to let player 2 enter their ships");
					}
					else{Console.WriteLine("Look at your board and press any key + Enter to start the game!");}
					Console.ReadLine();
					Console.Clear();
				}
				if (player1.areShipsEmpty() == true && playerVariable == player1)
				{
					playerVariable = player2;
					nextPlayer = true;
				}

			}
			gameIsActive = true;
			playerVariable = player1;
			while(gameIsActive)
			{
				while(step1 == false)
				{
					bool wasTurnSuccessful = false;
					Player activePlayer = (playerVariable == player1) ? player1 : player2;
					Player inactivePlayer = (playerVariable == player1) ? player2 : player1;
					InfoMessages.InfoMessage += "Player " +((playerVariable == player1) ? "1" : "2")+ ", it is your turn. Select coordinate (x,y) to attack.";
					ClearBoardAndShowMessages(activePlayer);
					//implement a thing here to let them type "show" to see their own board and then press enter to go back to the game
					try
					{
						if(PlayerAttack(activePlayer, inactivePlayer, Console.ReadLine()))
						{
							playerVariable = (playerVariable == player2) ? player1 : player2;
							wasTurnSuccessful = true;
						}
					}
					catch(Exception)
					{
						FormatErrorMessage();
					}
					if(wasTurnSuccessful)
					{
						ClearBoardAndShowMessages(activePlayer);
					}
					if (DidAnyoneWin(inactivePlayer))
					{
						Console.WriteLine("Congrats Player " + ((playerVariable == player1) ? "1" : "2") + ", you won!!!");
						gameIsActive = false;
					}
					if(wasTurnSuccessful) {PressEnterToContinue();}
				}
			}

		}

		public static void InitializeGame()
	  {
			int numberOfPlayers = GetNumberOfPlayers();
			SetUpPlayers(numberOfPlayers);
	  }

		public static int GetNumberOfPlayers()
		{
			bool numOfPlayersChosen = false;
			int numOfPlayers = 0;
			Console.WriteLine("1 Player game or 2?");
			while (!numOfPlayersChosen)
			{
				ShowMessages();
				int.TryParse(Console.ReadLine(), out numOfPlayers);
				if (numOfPlayers == 1 || numOfPlayers == 2)
				{numOfPlayersChosen = true;}
				else
				{Errors.ErrorMessage = "Please enter either '1 or '2'";}
			}
			return numOfPlayers;
		}

		public static void SetUpPlayers(int numberOfPlayers)
		{
			player1 = new Player();
			player2 = (numberOfPlayers == 1) ? new Player() : new ComputerPlayer();
			player1.getNewBoard();
			player2.getNewBoard();
			player2.getEnemyBoard();
			player1.getEnemyBoard();
			player1.MakeRealShips();
			player2.MakeRealShips();
		}

		public static void PressEnterToContinue()
		{
			Console.WriteLine("Press Enter to start the next player's turn!");
			Console.ReadLine();
		}

		public static bool DidAnyoneWin(Player player)
		{
			int winCount = 0;
			foreach(Ship ship in player.realShips)
			{
				winCount += (ship.sunk) ? 1 : 0;
			}
			return (winCount == player.realShips.Length) ? true : false;
		}

		public static bool DidAShipSink(Player player)
		{
			foreach (Ship ship in player.realShips)
			{
				if (ship.IsSunk())
				{
					InfoMessages.InfoMessage += String.Format("\nYou sunk their {0}!", ship.Name);
					return true;
				}
			}
			return false;
		}

		public static bool PlayerAttack(Player activePlayer, Player inactivePlayer, string coords)
		{
			try
			{
				string[] coordArray = CleanUpCoords(coords).Split(' ');
				xCoord = int.Parse(coordArray[0]);
				yCoord = int.Parse(coordArray[1]);
				bool spaceIsEmpty = activePlayer.enemyBoard.IsSpaceEmpty(xCoord, yCoord, activePlayer.enemyBoard);
				bool theirSpaceIsEmpty = inactivePlayer.board.IsSpaceEmpty(xCoord, yCoord, inactivePlayer.board);
				if (Board.IsOnBoard(xCoord, yCoord))
				{
					if(!spaceIsEmpty) {return WasAlreadyAttacked();}
					else if(theirSpaceIsEmpty) {AttackMissed(activePlayer, inactivePlayer, xCoord, yCoord);}
					else {ItWasAHit(activePlayer, inactivePlayer, xCoord, yCoord);}
					return true;
				}
			}
			catch(FormatException){}
			return FormatErrorMessage();
		}

		public static bool WasAlreadyAttacked()
		{
			Errors.ErrorMessage = "That space has been attacked already. Choose again.";
			return false;
		}

		public static void AttackMissed(Player player, Player inactivePlayer, int xCoord, int yCoord)
		{
			InfoMessages.InfoMessage += "That attack has missed.\n";
			Board.Change(player, inactivePlayer, xCoord, yCoord, "miss");
		}

		public static void ItWasAHit(Player player, Player inactivePlayer, int xCoord, int yCoord)
		{
			InfoMessages.InfoMessage += ("You got a hit!\n");
			Board.Change(player, inactivePlayer, xCoord, yCoord, "hit");
			DidAShipSink(inactivePlayer);
		}

		private static int SelectShip(string shipChoice, Player player)
		{
			if(shipChoice == ships[0].ToLower() || shipChoice == ships[1].ToLower())
			{
				string shipz = UppercaseFirst(shipChoice);
				Console.WriteLine("{0} it is!", shipz);
				player.removeShip(shipz);
				return (shipChoice == ships[0].ToLower() ? 1 : 2);
			}
			else
			{
				Errors.ErrorMessage = "That is not a valid option, try again.";
				return 0;
			}

		}

		private static void ClearBoardAndShowMessages(Player player)
		{
			ClearBoard(player);
			ShowMessages();
		}
		private static void ClearBoard(Player player)
		{
			Console.Clear();
			Board.DisplayBoard(player.enemyBoard);
			Board.DisplayBoard(player.board);
		}
		private static void ShowMessages()
		{
			Errors.WriteErrorMessage();
			InfoMessages.WriteInfoMessage();
		}

		private static bool FormatErrorMessage()
		{
			Errors.ErrorMessage = "Please enter coordinates between 0 and 9 in the following format: 'x,y'";
			return false;
		}

		private static bool PlaceShip(string coords, Player player)
		{
			string[] coordArray = CleanUpCoords(coords).Split(' ');
			xCoord = int.Parse(coordArray[0]);
			yCoord = int.Parse(coordArray[1]);
			if (Board.IsOnBoard(xCoord, yCoord) && player.board.IsSpaceEmpty(xCoord, yCoord, player.board))
			{
				return true;
			}
			else if(Board.IsOnBoard(xCoord, yCoord) == false)
			{
				Errors.ErrorMessage = "That is out of the range of the board.  Numbers must be between 0 and 9.";
				return false;
			}
			else
			{
				Errors.ErrorMessage = "That space is already taken. Please choose another coordinate set.";
				return false;
			}
		}

		public static string CleanUpCoords(string coordinates)
		{
			string[] coordArray = coordinates.Split(',');
		  int xCoordinate = int.Parse(Regex.Match(coordArray[0], @"\d+").Value);
			int yCoordinate = int.Parse(Regex.Match(coordArray[1], @"\d+").Value);
			return String.Format("{0} {1}", xCoordinate, yCoordinate);
		}

		public static string UppercaseFirst(string s) => char.ToUpper(s[0]) + s.Substring(1);

	}
}
