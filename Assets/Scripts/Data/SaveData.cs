using System;

[Serializable]
public class SaveData
{
  public int ScenarioNumber;
  public int[] RevealedRooms;
  public MonsterPosition[] Monsters;
  public PlayerPosition[] Players;

  public SaveData()
  {
  }
}

[Serializable]
public class MonsterPosition
{
  public int RoomNumber;
  public string Name;
  public int Number;
  public bool IsElite;
  public int MaxNumber;
  public float PositionX;
  public float PositionY;
  public float PositionZ;
}

[Serializable]
public class PlayerPosition
{
  public string Name;
  public float PositionX;
  public float PositionY;
  public float PositionZ;
}
