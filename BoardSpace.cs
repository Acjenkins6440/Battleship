using System;

namespace Battleship
{
  class BoardSpace
  {
    private bool hasBeenAttacked = false;
    private bool hasAShip = false;

    public bool ContainsShip()
    {
      return hasAShip;
    }
    //probably redundant, could just go with !ContainsShip()
    public bool IsEmpty()
    {
      return true;
    }

    public bool WasAttacked()
    {
      return hasBeenAttacked;
    }

    public void Attack()
    {
      hasBeenAttacked = true;
    }

    public void PlaceShip()
    {
      hasAShip = true;
    }

    public string DisplayStr()
    {
      if(IsEmpty() && WasAttacked())
      {
        return "*";
      }
      else if(ContainsShip() && WasAttacked())
      {
        return "X";
      }
      else if (ContainsShip())
      {
        return "o";
      }
      else
      {
        return "-";
      }
    }
  }
}
