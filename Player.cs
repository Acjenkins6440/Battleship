using System;
using System.Collections.Generic;

namespace Battleship {

	class Player
	{
		public Board board;
		public Board enemyBoard;
		public Fleet myFleet;
		private string _name;

		public Player(int whichPlayer)
		{
			_name = String.Format("Player {0}", whichPlayer);
			this.CreateNewBoard();
			this.CreateNewEnemyBoard();
			this.CreateFleet(DefaultShipList());
		}

		public void SetupShips()
		{
			Game.ClearBoard(this);
			Ship ship = ChooseShip();
			Console.ReadLine();
		}

		private Ship ChooseShip()
		{
			InfoMessages.InfoMessage += this._name + ", you must place your ships.\n";
			bool isValidEntry = false;
			string shipChoice = null;
			while(!isValidEntry)
			{
				DisplayShipChoices();
				shipChoice = Game.UppercaseFirst(Console.ReadLine());
				isValidEntry = myFleet.StillInList(shipChoice);
			}
			return myFleet.GetShipFromString(shipChoice);
		}

		private void DisplayShipChoices()
		{
			InfoMessages.InfoMessage += "Available Ships: \n";
			foreach(Ship ship in myFleet.getShipList())
			{
				InfoMessages.InfoMessage += ship.Name + "\n";
			}
			Game.ShowMessages();
		}

		private Ship ChosenShip(string shipChoice)
		{
			foreach(Ship ship in myFleet.getShipList())
			{
				if(ship.Name == shipChoice.ToUpper())
				{
					return ship;
				}
			}
			return null;
		}

		private Board CreateNewBoard() => board = new Board();

		private Board CreateNewEnemyBoard() => enemyBoard = new Board();

		public bool areShipsEmpty() => myFleet.hasEveryShipBeenPlaced();

		private void CreateFleet(List<Ship> shipList)
		{
			myFleet = new Fleet(shipList);
		}

		private List<Ship> DefaultShipList()
		{
			List<Ship> defaultShips = new List<Ship>(5);
			defaultShips.Add(new Scout());
			defaultShips.Add(new Battleship());
			defaultShips.Add(new Submarine());
			defaultShips.Add(new Destroyer());
			defaultShips.Add(new Carrier());
			return defaultShips;
		}
	}
}
