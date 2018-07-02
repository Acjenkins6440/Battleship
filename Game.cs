using System;
using System.Text.RegularExpressions;

namespace Battleship
{
	class Game
	{
		static int xCoord = 0;
		static int yCoord = 0;
		public static Player player1;
		static Player player2;
		static bool comPlayer;
		public static void Main()
		{
			InitializeGame();
			int shipChosen = 0;
			bool isInitialPositionChosen = false;
			bool isShipPlacedOnBoard = false;
			bool isGameActive = false;
		  comPlayer = (player2.GetType() == player1.GetType()) ? false : true;
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
					shipChosen = 0;
					isInitialPositionChosen = false;
					isShipPlacedOnBoard = false;
				}
				while(shipChosen == 0)
				{
					Errors.WriteErrorMessage();
					playerVariable.listShips();
					string shipChoice = Console.ReadLine().ToLower();
					shipChosen = SelectShip(shipChoice, playerVariable);
				}
				Console.WriteLine("Select starting coordinates in the following format: x,y");

				int shipLength = playerVariable.realShips[shipChosen - 1].Length;

				while(!isInitialPositionChosen)
				{
					Errors.WriteErrorMessage();
					try
					{
						string playerStartCoords = Console.ReadLine().ToLower();
						isInitialPositionChosen = PlaceShip(playerStartCoords, playerVariable);
					}
					catch(Exception)
					{
						Console.WriteLine("You can only enter numbers between 0 and 9, separated by a comma to set the start point of your ship.");
						Console.WriteLine("Try again.");
					}
				}
				Console.WriteLine("Excellent, now choose whether you want it facing N, E, S, or W.");
				while(!isShipPlacedOnBoard)
				{
					Errors.WriteErrorMessage();
					try
					{
						string direction = Console.ReadLine().ToLower();
						isShipPlacedOnBoard = playerVariable.board.SetShipDirection(xCoord, yCoord, direction, shipLength, playerVariable);
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
					if (playerVariable == player1 && !comPlayer)
					{
						Console.WriteLine("Look at your board and press any key + Enter to let player 2 enter their ships");
					}
					else{Console.WriteLine("Look at your board and press any key + Enter to start the game!");}
					Console.ReadLine();
					Console.Clear();
				}
				if (player1.areShipsEmpty() && playerVariable == player1 && !comPlayer )
				{
					playerVariable = player2;
				}
				else if (player1.areShipsEmpty() && comPlayer)
				{
					ComputerPlayer.SetUpComputerShips((ComputerPlayer)player2);
					Errors.ErrorMessage = "";
				}

			}
			isGameActive = true;
			Player activePlayer = player1;
			Player inactivePlayer = player2;
			while(isGameActive)
			{
				bool wasTurnSuccessful = false;
				InfoMessages.InfoMessage += "Player " +((activePlayer == player1) ? "1" : "2")+ ", it is your turn. Select coordinate (x,y) to attack.";
				ClearBoardAndShowMessages(activePlayer);
				try
				{
					wasTurnSuccessful = (PlayerAttack(activePlayer, inactivePlayer, Console.ReadLine()));
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
					Console.WriteLine("Congrats Player " + ((activePlayer == player1) ? "1" : "2") + ", you won!!!");
					isGameActive = false;
					Console.ReadLine();
					break;
				}
				if (wasTurnSuccessful) {PressEnterToContinue();}
				if(!comPlayer && wasTurnSuccessful)
				{
					activePlayer = (activePlayer == player1) ? player2 : player1;
					inactivePlayer = (inactivePlayer == player2) ? player1 : player2;
				}
				else if (comPlayer && wasTurnSuccessful)
				{
					ComputerPlayer.Attack((ComputerPlayer)player2);
				}
			}

		}

		public static void InitializeGame()
	  {
			int numberOfPlayers = GetNumberOfPlayers();
			SetUpPlayers(numberOfPlayers);
			//Turn 1 player mode back on by changing the "3" to a "1" in SetUpPlayers
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
			player2 = (numberOfPlayers == 1) ? new ComputerPlayer() : new Player();
			player1.getNewBoard();
			player2.getNewBoard();
			player2.getEnemyBoard();
			player1.getEnemyBoard();
			player1.MakeRealShips();
			player2.MakeRealShips();
		}

		public static void PressEnterToContinue()
		{
			if(!comPlayer)
			{
				Console.WriteLine("Press Enter to start the next player's turn!");
			}
			else
			{
				Console.WriteLine("Press Enter to let the computer have their turn.");
			}
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
				if (comPlayer && player.GetType() != ComputerPlayer && ship.IsSunk())
				{
					InfoMessages.InfoMessage = String.Format("THE COMPUTER sunk your {0}!\n", ship.Name);
				}
				else if (ship.IsSunk())
				{
					InfoMessages.InfoMessage = String.Format("You sunk their {0}!\n", ship.Name);
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
			if(comPlayer){ComputerPlayer.justHitCoords = String.Format("{0} {1}",xCoord,yCoord);}
			InfoMessages.InfoMessage += ("You got a hit!\n");
			Board.Change(player, inactivePlayer, xCoord, yCoord, "hit");
			DidAShipSink(inactivePlayer);
		}

		public static int SelectShip(string shipChoice, Player player)
		{
			if(shipChoice == player.ships[0].ToLower() || shipChoice == player.ships[1].ToLower())
			{
				shipChoice = UppercaseFirst(shipChoice);
				Console.WriteLine("{0} it is!", shipChoice);
				player.removeShip(shipChoice);
				return (shipChoice == player.realShips[0].Name ? 1 : 2);
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

		public static bool PlaceShip(string coords, Player player)
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
