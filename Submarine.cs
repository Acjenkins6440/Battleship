using System;

namespace Battleship
{
	public class Submarine : Ship
	{
		public override string Name { get; } = "Submarine";

		public override int ShipLength { get; } = 4;

		public override int[] ShipArray { get; } = new int[4];

		public static void SetArray(int x1, int x2, int x3, int x4, int axisValue, string cons, Submarine ship)
		{
			//ShipArray[0] is bugged for some reason, this is temporary fix while I
			//figure out what is going on with the SetShipArray function
			ship.ShipArray[0] = x2 - 1;
			ship.ShipArray[1] = x2;
			ship.ShipArray[2] = x3;
			ship.ShipArray[3] = x4;
			ship.axisValue = axisValue;
			ship.axis = cons;
		}
	}
}
