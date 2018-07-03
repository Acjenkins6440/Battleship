using System;

namespace Battleship
{
	class Board
	{
		private const int _xMax = 10;
		private const int _yMax = 10;
		private BoardSpace[,] boardArray = new BoardSpace[_xMax, _yMax];

		public Board()
		{
			for (int i = 0; i < _xMax; i++)
			{
				for (int j = 0; j < _yMax; j++)
				{
					boardArray[i,j] = new BoardSpace();
				}
			}
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
			//return (board.boardArray[xVal,yVal] == 0) ? true : false;
			return false;
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
				Console.Write(String.Format("{0}|  ", Point.boardLetters[x]));
				for(int y = 0; y < _yMax; y++)
				{
					BoardSpace currSpace = board.boardArray[x,y];
					Console.Write(currSpace.DisplayStr());
					Console.Write(" ");
				}
				Console.WriteLine("");
			//go through array of x = counter, empty squares = "-", ships = "o", failed attacks = *, hit attacks = "X"
			}
		}
	}
}
