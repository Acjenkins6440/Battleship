using System;

namespace battleship
{
  class BoardSpace
  {
    private bool attackedAlready = false;

    public bool ContainsShip()
    {
      return false;
    }

    public bool IsEmpty()
    {
      return false;
    }

    public bool WasAttacked()
    {
      return false;
    }

    public string GetDisplayStr()
    {
      return "";
    }
  }
}
