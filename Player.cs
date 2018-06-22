using System;

namespace Battleship {

	class Player
	{
		public string[] ships = new string[2] {"Scout", "Battleship"};
		public Ship scout = new Scout();
		public Ship battleship = new Battleship();
		public Ship[] realShips = new Ship[2];
		public Board board;
		public Board enemyBoard;

		public Board getNewBoard()
		{
			board = new Board();
			return board;
		}

		public Board getEnemyBoard()
		{
			enemyBoard = new Board();
			return enemyBoard;
		}

		public void listShips()
		{
			Console.WriteLine("Ships available:");
			foreach (string ship in ships)
			{
				if (ship != "Empty")
				{
					Console.WriteLine(ship);
				}
			}
		}

		public void removeShip(string shipToRmv)
		{
			ships[Array.IndexOf(ships,shipToRmv)] = "Empty";
		}

		public bool areShipsEmpty()
		{
			for (int i = 0; i < ships.Length; i++)
			{
				if (ships[i] != "Empty")
				{
					return false;
				}
			}
			return true;
		}

		public void MakeRealShips()
		{
			realShips[0] = scout;
			realShips[1] = battleship;
		}

		public bool areShipsFull()
		{
			return (Array.IndexOf(ships,"Empty") == -1) ? true : false;
		}
	}

}
