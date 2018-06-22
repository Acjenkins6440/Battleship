using System;

namespace Battleship
{
	public class Battleship : Ship
	{
		public override string Name { get; } = "Battleship";

		public override int Length { get; } = 4;

		public override int[] ShipArray { get; } = new int[4];

		public static void SetArray(int x1, int x2, int x3, int x4, int direc, string cons, Battleship ship)
		{
			ship.ShipArray[0] = x1;
			ship.ShipArray[1] = x2;
			ship.ShipArray[2] = x3;
			ship.ShipArray[3] = x4;
			ship.direction = direc;
			ship.axis = cons;
		}
	}
}
