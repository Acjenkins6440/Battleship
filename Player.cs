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
		private Fleet myFleet;
		private string _name;

		public Player(int whichPlayer)
		{
			_name = String.Format("Player{0}", whichPlayer);
			this.CreateNewBoard();
			this.CreateNewEnemyBoard();
			this.MakeRealShips();
		}

		public void SetupShips()
		{
			Console.Clear();
			Board.DisplayBoard(this.board);
		}

		public Board CreateNewBoard() => board = new Board();

		public Board CreateNewEnemyBoard() => enemyBoard = new Board();

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
