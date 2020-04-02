using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance = null;
  public GameObject[] rooms;
  public GameObject coinPrefab;
  public List<Monster> Monsters;
  public List<int> RevealedRooms;

  public int scenarioNumber;

  private int killCounter = 0;
  public TextMeshProUGUI KillCounterText;

  public Player[] Players
  {
    get
    {
      return FindObjectsOfType<Player>();
    }
  }

  void Awake()
  {
    // Singleton
    if (Instance == null)
      Instance = this;
    else if (Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    //DontDestroyOnLoad(gameObject);

    // Hide all rooms
    for (int i = 0; i < rooms.Length; i++)
      rooms[i].SetActive(false);

    // And then reveal only the first room
    RevealRoom(rooms[0]);
  }

  private void AwakenMonsters(GameObject room)
  {
    foreach (var monster in room.GetComponentsInChildren<Monster>())
    {
      if (monster.Number > 0)
        continue; // already added

      monster.AssignNumber();

      if (monster.Number == 0)
        Destroy(monster.gameObject);
      else
        Monsters.Add(monster);
    }
  }

  public void RevealRoom(GameObject roomObject)
  {
    this.RevealRoom(roomObject, false);
  }

  public void RevealRoom(GameObject roomObject, bool fromSave)
  {
    roomObject.SetActive(!roomObject.activeSelf);

    var roomNumber = int.Parse(roomObject.name.Split(' ')[1]);
    if (!RevealedRooms.Contains(roomNumber))
      RevealedRooms.Add(roomNumber);

    if (fromSave)
      return; // don't add monsters

    AwakenMonsters(roomObject);

    if (roomNumber > 1)
      DataManager.Instance.Save();
  }

  public void MonsterKilled(string name)
  {
    if (scenarioNumber == 43 && name.ToLower().Contains("drake"))
    {
      killCounter++;
      KillCounterText.text = killCounter.ToString();
    }
  }
}
