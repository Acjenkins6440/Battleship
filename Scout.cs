using System;

namespace Battleship
{
	public class Scout : Ship
	{
		public override string Name { get; } = "Scout";

		public override int Length { get; } = 2;

		public override int[] ShipArray { get; } = new int[2];

		public static void SetArray(int x1, int x2, int direc, string axis, Scout ship)
		{
			ship.ShipArray[0] = x1;
			ship.ShipArray[1] = x2;
			ship.direction = direc;
			ship.axis = axis;
		}
	}
}
