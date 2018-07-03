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
  }
}
