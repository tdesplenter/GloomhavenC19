using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
  public static DataManager Instance;

  private SaveData _saveData;
  private JsonSaver _jsonSaver;

  public bool IsReady { get; set; } = false;

  public GameObject LoadButton;

  #region Singleton
  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    else if (Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    //DontDestroyOnLoad(gameObject);

    _saveData = new SaveData();
    _jsonSaver = new JsonSaver();
  }

  private void Start()
  {
    _jsonSaver.Load(_saveData);

    this.LoadButton?.SetActive(this.ScenarioNumber == GameManager.Instance.scenarioNumber);
  }

  private void OnDestroy()
  {
    if (Instance == this)
      Instance = null;
  }
  #endregion

  public void Load()
  {
    // Players
    var players = FindObjectsOfType<Player>();
    foreach (var player in players)
    {
      var savedPlayer = this.Players.First(x => x.Name == player.Name);
      player.transform.position = new Vector3(savedPlayer.PositionX, savedPlayer.PositionY, savedPlayer.PositionZ);
    }

    // Monsters
    var spawners = FindObjectsOfType<Monster>().Where(x => x.IsSpawner);

    foreach (var roomNumber in this.RevealedRooms)
    {
      var roomObject = GameManager.Instance.rooms[roomNumber - 1]; 

      if (roomNumber > 1)
        GameManager.Instance.RevealRoom(roomObject, true);

      // Remove monsters from room
      GameManager.Instance.Monsters.Clear();
      var oldMonsters = roomObject.GetComponentsInChildren<Monster>();
      for (int i = 0; i < oldMonsters.Length; i++)
      {
        Destroy(oldMonsters[i].gameObject);
      }

      var enemiesObject = roomObject.transform.Find("Enemies");

      // Add back the saved ones
      foreach (var monster in this.Monsters)
      {
        var spawner = spawners.FirstOrDefault(x => x.Name == monster.Name && x.isElite == monster.IsElite);
        if (spawner == null)
          continue;
        var newMonster = Instantiate(spawner, enemiesObject);
        newMonster.transform.position = new Vector3(monster.PositionX, monster.PositionY, monster.PositionZ);
        newMonster.currentPosition = newMonster.transform.position;
        newMonster.IsSpawner = false;
        newMonster.Number = monster.Number;
        newMonster.ShowNumber();
        GameManager.Instance.Monsters.Add(newMonster);
      }
    }

    this.IsReady = true;
  }

  public void Save()
  {
    this.ScenarioNumber = GameManager.Instance.scenarioNumber;

    this.RevealedRooms = GameManager.Instance.RevealedRooms.ToArray();

    this.Monsters = GameManager.Instance.Monsters.Where(x => x != null && !x.IsDead).Select(x => new MonsterPosition { RoomNumber = x.RoomNumber, Name = x.Name, IsElite = x.isElite, Number = x.Number, MaxNumber = x.maxnumber, PositionX = x.transform.position.x, PositionY = x.transform.position.y, PositionZ = x.transform.position.z }).ToArray();

    this.Players = GameManager.Instance.Players.Select(x => new PlayerPosition { Name = x.Name, PositionX = x.transform.position.x, PositionY = x.transform.position.y, PositionZ = x.transform.position.z }).ToArray();

    // TODO: Traps?

    _jsonSaver.Save(_saveData);

    this.LoadButton?.SetActive(false);
  }

  public int ScenarioNumber
  {
    get { return _saveData.ScenarioNumber; }
    set { _saveData.ScenarioNumber = value; }
  }

  public int[] RevealedRooms
  {
    get { return _saveData.RevealedRooms; }
    set { _saveData.RevealedRooms = value; }
  }

  public PlayerPosition[] Players
  {
    get { return _saveData.Players; }
    set { _saveData.Players = value; }
  }

  public MonsterPosition[] Monsters
  {
    get { return _saveData.Monsters; }
    set { _saveData.Monsters = value; }
  }
}
