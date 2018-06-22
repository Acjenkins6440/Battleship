using System;

namespace Battleship {

  class ComputerPlayer : Player
  {
    private int[] justHit = int[2];
    public string ChooseCoordinateSet()
    { return String.Format("{0} {1}",Random.NextInt(),Random.NextInt());}

    public string ChooseStartingDirection() => Random.NextString();

    public string ShipChoice() => realShips[Random.NextShip()].Name;

    public void ChoiceAfterAHit(string[] coords)
    {

    }

    public static void Attack(ComputerPlayer player)
    {
      PlayerAttack(player, Game.player1, ChooseCoordinateSet());
    }

    public static void SetUpComputerShips(ComputerPlayer player)
    {
      foreach (string ship in player.ships)
      {
        bool isValidChoice2 = false;
        bool isValidChoice3 = false;
        int shipLength = (ship.ToLower() == "scout") ? 2 : 4;
        Game.SelectShip(ship, player);
        string[] coordSet = player.ChooseCoordinateSet().Split(' ');
        int[] coords = new int[2] {int.Parse(coordSet[0]), int.Parse(coordSet[1])};
        while (!isValidChoice2)
        {
          try
          {
            isValidChoice2 = Game.PlaceShip(String.Format("{0},{1}", coordSet[0], coordSet[1]), player);
          }
          catch(Exception){}
        }
        while (!isValidChoice3)
        {
          try
          {
            string direction = player.ChooseStartingDirection();
            isValidChoice3 = player.board.SetShipDirection(coords[0], coords[1], direction, shipLength, player);
          }
          catch(Exception){}
        }
      }
    }


  }
}
