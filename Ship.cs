using System;

namespace Battleship {

	public class Ship
	{
		public string axis;
		public int axisValue;
		public int hitCount = 0;
		public bool sunk = false;
		private bool isPlaced = false;

		public virtual string Name { get; } = "";

		public virtual int ShipLength { get; } = 0;

		public virtual int[] ShipArray { get; } = new int[0];

		public bool IsSunk()
		{
			if (hitCount == this.ShipLength)
			{
				sunk = true;
				hitCount = 0;
				return true;
			}
			return false;
		}

		public bool hasBeenPlaced() => isPlaced;
	}
}
