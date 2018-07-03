using System;

namespace Battleship
{
  class InfoMessages
  {
    private static string infoMessage = "";

    public static string InfoMessage
    {
      get
      {
        return infoMessage;
      }
      set
      {
        infoMessage = value;
      }
    }

    public static void DumpInfoMessages()
    {
      Console.Write((infoMessage == "") ? infoMessage : infoMessage + "\n" );
      infoMessage = "";
    }
  }
}
