using System;

namespace Battleship
{
  class Errors
  {
    private static string errorMessage = "";

    public static string ErrorMessage
    {
      get
      {
        return errorMessage;
      }
      set
      {
        errorMessage = value;
      }
    }

    public static void DumpErrorMessages()
    {
      Console.Write((ErrorMessage == "") ? errorMessage : errorMessage + "\n" );
      errorMessage = "";
    }

    public static void NotAValidShipChoice()
    {
      ErrorMessage += "That is not a valid choice, please select a ship from the list.";
    }

    public static void InvalidCoordSet()
    {
      ErrorMessage += "Please only type 2 characters, the letter and the number of the space you want.  Ex: A4 or J9";
    }
  }
}
