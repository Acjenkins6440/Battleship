using System;

namespace Battleship {

  class ComputerPlayer : Player
  {
    public static string justHitCoords = "10, 10";

    public static bool findingHitShip = false;

    public string ChooseCoordinateSet() => String.Format("{0},{1}",Random.NextInt(),Random.NextInt());

    public string ChooseDirection() => Random.NextString();

    public ComputerPlayer(int whichPlayer) : base(whichPlayer)
    {}

    public string ChoiceAfterAHit()
    {
      string direction = ChooseDirection();
      string coords;
      int xCoord = int.Parse(justHitCoords[0].ToString());
      int yCoord = int.Parse(justHitCoords[2].ToString());
      if(direction == "n"){coords = FormatCoords(xCoord - 1, yCoord);}
      else if(direction == "e"){coords = FormatCoords(xCoord, yCoord + 1);}
      else if(direction == "s"){coords = FormatCoords(xCoord + 1, yCoord);}
      else{coords = FormatCoords(xCoord, yCoord - 1);}
      return coords;
    }

    public string FinishOffFoundShip()
    {
      return "hello";
    }

    public string FormatCoords(int xCoord, int yCoord) => String.Format("{0},{1}", xCoord, yCoord);

    public int CoordsStringToInt(string coords) => int.Parse(coords);

    private bool CheckIfJustHit(string coords) => (coords[0] == justHitCoords[0] && coords[2] == justHitCoords[2]) ? true : false;

    public static void Attack(ComputerPlayer player)
    {
      bool wasTurnSuccessful = false;
      string coords;
      do
      {
        coords = (findingHitShip) ? player.ChoiceAfterAHit() : player.ChooseCoordinateSet();
        wasTurnSuccessful = Game.PlayerAttack(player, Game.player1, coords);
      } while (!wasTurnSuccessful);
      findingHitShip = (findingHitShip ) ? true : false;
      findingHitShip = player.CheckIfJustHit(coords);
    }

    public static void SetUpComputerShips(ComputerPlayer player)
    {
      foreach (Ship ship in player.myFleet.getShipList())
      {
        bool isInitialPositionChosen = false;
        bool isShipPlacedOnBoard = false;
        int shipLength = (ship.Name.ToLower() == "scout") ? 2 : 4;
        Game.SelectShip(ship.Name, player);
        string[] coordSet = player.ChooseCoordinateSet().Split(',');
        int[] coords = new int[2] {int.Parse(coordSet[0]), int.Parse(coordSet[1])};
        while (!isInitialPositionChosen)
        {
          try
          {
            isInitialPositionChosen = Game.PlaceShip(String.Format("{0},{1}", coordSet[0], coordSet[1]), player);
          }
          catch(Exception){}
        }
        while (!isShipPlacedOnBoard)
        {
          try
          {
            string direction = player.ChooseDirection();
            isShipPlacedOnBoard = player.board.SetShipDirection(coords[0], coords[1], direction, shipLength, player);
          }
          catch(Exception){}
        }
      }
    }


  }
}
