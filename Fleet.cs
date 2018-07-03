using System;
using System.Collections.Generic;

namespace Battleship
{
  class Fleet
  {
    private int fleetLength;
    private List<Ship> _shipList = new List<Ship>();

    public int FleetLength { get; set; } = 5;

    public Fleet(List<Ship> shipList)
    {
      _shipList = shipList;
    }
  }
}
