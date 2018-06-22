using System;

namespace Battleship
{
	class Board
	{
		private const int _xVal = 10;
		private const int _yVal = 10;
		private int[,] boardArray = new int[_xVal, _yVal];
		private int[] scoutArray = new int[2];
		private int[] battleshipArray = new int[4];
		private int direct = 0;
		private Ship ship;
		public int scoutCount = 0;
		public int battleCount = 0;

		public bool SetShipDirection(int x, int y, string direction, int shipLength, Player player)
		{
			int xDirect = 0;
			int yDirect = 0;
			if(direction == "e" || direction == "w")
			{yDirect = ChangeDirectionIntoCoords(direction, shipLength);}
			else if(direction == "s" || direction == "n")
			{xDirect = ChangeDirectionIntoCoords(direction, shipLength);}
			else
			{
				Errors.ErrorMessage = "Please enter one of the 4 cardinal directions only.";
				return false;
			}
			if (!Board.IsOnBoard(x+xDirect, y+yDirect))
			{
				Errors.ErrorMessage = "That would put the ship off of the board, please choose another direction.";
				return false;
			}
			if (direction == "n"){x -= (shipLength-1);}
			if (direction == "w"){y -= (shipLength-1);}
			if (!NoShipsInPath(shipLength, player.board, x, y, yDirect)) {return false;}
			for(int i=0; i < shipLength; i++)
			{
				int j = (yDirect == 0) ? 0 : i;
				int q = (j == 0) ? i : 0;
				if(player.board.IsSpaceEmpty(x+q, y+j, player.board)){}
				else
				{
					Errors.ErrorMessage = "That direction goes through another ship, please choose a different direction";
					return false;
				}
			}
			ship = (shipLength == 2) ? player.realShips[0] : player.realShips[1];
			string p = "";
			for(int i=0; i < shipLength; i++)
				//also add the ship's coords to the ship's array
			{
				int j = (yDirect == 0) ? 0 : i;
				int q = (j == 0) ? i : 0;
				direct = (j == 0) ? y : x;
				p = (j == 0) ? "y" : "x";
				player.board.boardArray[x+q, y+j] = 1;
				if (shipLength == 2) { scoutArray[i] = ((direct == y) ? x+q : y+j); }
				if (shipLength == 4) { battleshipArray[i] = ((direct == y) ? x+q : y+j); }
			}
			setShipArray(ship, ((shipLength == 2) ? scoutArray : battleshipArray), direct, p);
			DisplayBoard(player.board);
			return true;
		}

		public static bool NoShipsInPath(int shipLength, Board board, int x, int y, int yDirect)
		{
			for (int i=0; i< shipLength; i++)
			{
				int j = (yDirect == 0) ? 0 : i;
				int q = (j == 0) ? i : 0;
				if(!board.IsSpaceEmpty(x + q, y + j, board))
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

		public static void setShipArray(Ship ship, int[] arr, int direction, string cons)
		{
			if(arr.Length == 2)
			{
				Scout.SetArray(arr[0], arr[1], direction, cons, (Scout)ship);
			}
			else
			{
				Battleship.SetArray(arr[0], arr[1], arr[2], arr[3], direction, cons, (Battleship)ship);
			}

		}

		public static bool IsOnBoard(int x, int y)
		{
			return (x >= 0 && x < _xVal && y >= 0 && y < _yVal) ? true : false;
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
			foreach(Ship ship in player.realShips)
			{
				if(Array.Exists(ship.ShipArray, element => element == xCoord) && ship.axis == "y" && ship.direction == yCoord)
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
			for(int x = 0; x < _xVal; x++)
			{
				Console.Write(x+ "|  ");
				for(int y = 0; y < _yVal; y++)
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
