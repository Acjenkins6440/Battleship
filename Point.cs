using System;

namespace Battleship
{
  class Point
  {
    public static string[] boardLetters = new string[10]{"A","B","C","D","E","F","G","H","I","J"};

    public static int X{get; protected set;}
    public static int Y{get; protected set;}

    public Point(string coordChoice)
    {
      X = coordChoice[1];
      Y = Array.IndexOf(boardLetters, coordChoice[0]);
    }

    public static bool ValidateCoordinate(string coordChoice)
    {
      if(coordChoice.Length != 2)
      {
        Errors.InvalidCoordSet();
        return false;
      }
      int number;
      bool hasNumber = Int32.TryParse(coordChoice[1].ToString(), out number);
      if(Array.Exists(boardLetters, letter => letter == coordChoice[0].ToString()) && hasNumber)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
