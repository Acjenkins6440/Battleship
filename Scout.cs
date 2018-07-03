using System;

namespace Battleship
{
	public class Scout : Ship
	{
		public override string Name { get; } = "Scout";

		public override int ShipLength { get; } = 2;

		public override int[] ShipArray { get; } = new int[2];

		public static void SetArray(int x1, int x2, int axisValue, string axis, Scout ship)
		{
			//ShipArray[0] is bugged for some reason, this is temporary fix while I
			//figure out what is going on with the SetShipArray function
			ship.ShipArray[0] = x2 - 1;
			ship.ShipArray[1] = x2;
			ship.axisValue = axisValue;
			ship.axis = axis;
		}
	}
}
