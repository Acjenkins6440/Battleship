using System;

namespace Battleship {

  class ComputerPlayer : Player
  {
    public string ChooseStartingPoint()
    {
      return String.Format("{$0},{$1}",Random.NextInt(),Random.NextInt());
    }

    public string ChooseStartingDirection()
    {
      return (Random.NextString());
    }
  }
}
