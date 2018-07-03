using System;

namespace Battleship
{
	class Board
	{
		private const int _xMax = 10;
		private const int _yMax = 10;
		private int[,] boardArray = new int[_xMax, _yMax];
		private int[] scoutArray = new int[2];
		private int[] battleshipArray = new int[4];
		private int direct = 0;
		private Ship ship;
		public int scoutCount = 0;
		public int battleCount = 0;
		private static string[] boardLetters = new string[10]{"A","B","C","D","E","F","G","H","I","J"};

		public bool SetShipDirection(int xCoord, int yCoord, string direction, int shipLength, Player player)
		{
			int xEndPoint = 0;
			int yEndPoint = 0;
			if(direction == "e" || direction == "w")
			{yEndPoint = ChangeDirectionIntoCoords(direction, shipLength) + yCoord;}
			else if(direction == "s" || direction == "n")
			{xEndPoint = ChangeDirectionIntoCoords(direction, shipLength) + xCoord;}
			else
			{
				Errors.ErrorMessage = "Please enter one of the 4 cardinal directions only.";
				return false;
			}
			if (!Board.IsOnBoard(xEndPoint, yEndPoint))
			{
				Errors.ErrorMessage = "That would put the ship off of the board, please choose another direction.";
				return false;
			}
			if (direction == "n"){xCoord -= (shipLength-1);}
			if (direction == "w"){yCoord -= (shipLength-1);}
			if (!NoShipsInPath(shipLength, player.board, xCoord, yCoord, yEndPoint)) {return false;}
			ship = (shipLength == 2) ? player.myFleet.getShipList()[0] : player.myFleet.getShipList()[1];
			string axis = "";
			for(int i = 0; i < shipLength; i++)
			{
				int addToYCoord = (yEndPoint == 0) ? 0 : i;
				int addToXCoord = (addToYCoord == 0) ? i : 0;
				direct = (addToYCoord == 0) ? yCoord : xCoord;
				axis = (addToYCoord == 0) ? "y" : "x";
				player.board.boardArray[xCoord+addToXCoord, yCoord+addToYCoord] = 1;
				if (shipLength == 2)
				{
					scoutArray[i] = ((axis == "y") ? xCoord+addToXCoord : yCoord+addToYCoord);
				}
				if (shipLength == 4)
				{
					 battleshipArray[i] = ((axis =="y") ? xCoord+addToXCoord : yCoord+addToYCoord);
				 }
			}
			setShipArray(ship, ((shipLength == 2) ? scoutArray : battleshipArray), direct, axis);
			DisplayBoard(player.board);
			return true;
		}

		public static bool NoShipsInPath(int shipLength, Board board, int x, int y, int yEndPoint)
		{
			for (int i=0; i< shipLength; i++)
			{
				int addToYCoord = (yEndPoint == 0) ? 0 : i;
				int addToXCoord = (addToYCoord == 0) ? i : 0;
				if((!board.IsSpaceEmpty(x + addToXCoord, y + addToYCoord, board) && i != 0) || (!board.IsSpaceEmpty(x,y,board) && i == 0))
				{
					Errors.ErrorMessage = "That direction goes through another ship, please choose a different direction";
					return false;
				}
			}
			return true;
		}

		public static int ChangeDirectionIntoCoords(string direction, int shipLength)
		{
			return (direction == "e" || direction == "s") ?  shipLength - 1 : (-1 * (shipLength - 1));
		}

		public static void setShipArray(Ship ship, int[] arr, int axisValue, string cons)
		{
			if(arr.Length == 2)
			{
				Scout.SetArray(arr[0], arr[1], axisValue, cons, (Scout)ship);
			}
			else
			{
				Battleship.SetArray(arr[0], arr[1], arr[2], arr[3], axisValue, cons, (Battleship)ship);
			}

		}

		public static bool IsOnBoard(int x, int y)
		{
			return (x >= 0 && x < _xMax && y >= 0 && y < _yMax) ? true : false;
		}

		public bool IsSpaceEmpty(int xVal, int yVal, Board board)
		{
			return (board.boardArray[xVal,yVal] == 0) ? true : false;
		}

		public static void Change(Player player, Player enemyPlayer, int xCoord, int yCoord, string changeType)
		{
			player.enemyBoard.boardArray[xCoord, yCoord] = ((changeType == "miss") ? 2 : 3);
			enemyPlayer.board.boardArray[xCoord, yCoord] = player.enemyBoard.boardArray[xCoord, yCoord];
			UpdateHitCount(xCoord,yCoord, enemyPlayer);
		}

		public static void UpdateHitCount(int xCoord, int yCoord, Player player)
		{
			foreach(Ship ship in player.myFleet.getShipList())
			{
				if(Array.Exists(ship.ShipArray, element => element == xCoord) && ship.axis == "y" && ship.axisValue == yCoord)
				{
					ship.hitCount += 1;
				}
				else if(Array.Exists(ship.ShipArray, element => element == yCoord) && ship.axis == "x" && ship.axisValue == xCoord)
				{
					ship.hitCount += 1;
				}
			}
		}

		public static void DisplayBoard(Board board)
		{
			Console.Write("    ");
				for (int j = 0; j < 10; j++)
				{
					Console.Write(j+ " ");
				}
			Console.WriteLine("");
			for(int x = 0; x < _xMax; x++)
			{
				Console.Write(String.Format("{0}|  ", boardLetters[x]));
				for(int y = 0; y < _yMax; y++)
				{
					if(board.boardArray[x,y] == 0)
					{
						Console.Write("-");
					}
					else if (board.boardArray[x,y] == 1)
					{
						Console.Write("o");
					}
					else if (board.boardArray[x,y] == 2)
					{
						Console.Write("*");
					}
					else
					{
						Console.Write("X");
					}
					Console.Write(" ");
				}
				Console.WriteLine("");
			//go through array of x = counter, empty squares = "-", ships = "o", failed attacks = *, hit attacks = "X"
			}
		}
	}
}
