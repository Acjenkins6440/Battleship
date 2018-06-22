using System;

namespace Battleship{
  static class Random
  {
    private static System.Random _random = new System.Random();

    public static int NextInt()
    {
      return _random.Next(10);
    }

    public static int NextShip()
    {
      return _random.Next(2);
    }

    public static string NextString()
    {
      int directionNumber = _random.Next(4);
      return RandomIntToDirection(directionNumber);
    }

    private static string RandomIntToDirection(int directionNumber)
    {
      if (directionNumber == 0)
      {return "n";}
      else if (directionNumber == 1)
      {return "e";}
      else if (directionNumber == 2)
      {return "s";}
      else
      {return "w";}
    }
  }
}
