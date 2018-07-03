using System;
using System.Collections.Generic;

namespace Battleship
{
  class Fleet
  {
    private List<Ship> _shipList = new List<Ship>(5);

    public Fleet(List<Ship> shipList)
    {
      _shipList = shipList;
    }

    public bool hasEveryShipBeenPlaced()
    {
      foreach(Ship ship in _shipList)
      {
        if(!ship.hasBeenPlaced())
        {
          return false;
        }
      }
      return true;
    }

    public List<Ship> getShipList()
    {
      List<Ship> returnList = new List<Ship>();
      foreach(Ship ship in _shipList)
      {
        if (!ship.hasBeenPlaced())
        {
          returnList.Add(ship);
        }
      }
      return returnList;
    }

    public Ship GetShipFromString(string shipString)
    {
      foreach(Ship ship in _shipList)
      {
        if(ship.Name.ToLower() == shipString)
        {
          return ship;

        }
      }
      return null;
    }

    public bool StillInList(string shipString)
    {
      foreach(Ship ship in _shipList)
      {
        if(ship.Name.ToLower() == shipString)
        {
          return true;
        }
      }
      Errors.NotAValidShipChoice();
      return false;
    }

  }
}
