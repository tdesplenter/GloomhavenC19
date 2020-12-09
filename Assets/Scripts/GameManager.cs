using System.Collections.Generic;
using System.Linq;
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

  public Image BigMonster;

  public int scenarioNumber;

  private int killCounter = 0;
  public TextMeshProUGUI KillCounterText;

  public PopupMenu popupMenu;
  public PopupMessage popupMessage;

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

    if (BigMonster != null)
      BigMonster.gameObject.SetActive(false);

    // Hide all rooms
    for (int i = 0; i < rooms.Length; i++)
      rooms[i].SetActive(false);
  }

  void Start()
  {
    // Reveal the first room
    RevealRoom(rooms[0]);

    if (popupMenu == null)
      Debug.LogError("Popup Menu is missing!");
    else
      popupMenu.gameObject.SetActive(false);
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(2))
    {
      // Show Popup Menu
      if (popupMenu != null)
      {
        var screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
        var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        spawnPosition.z = 1f;
        popupMenu.Show(spawnPosition);
      }
    }

    if (Input.GetMouseButtonUp(2))
    {
      // Activate Popup Menu Item (if any)
      if (popupMenu != null)
        popupMenu.CheckAndActivate();
    }
  }

  public void SetActiveItem(PopupMenuItem item)
  {
    if (popupMenu != null)
      popupMenu.SelectItem(item);
  }

  public void ClearPopupMenuSelection()
  {
    if (popupMenu != null)
      popupMenu.ClearSelection();
  }

  private void AwakenMonsters(GameObject room)
  {
    foreach (var monster in room.GetComponentsInChildren<Monster>())
    {
      if (monster.Number > 0)
        continue; // already added

      monster.Awaken();
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

  public void ShowMonster(Monster monster)
  {
    BigMonster.sprite = Resources.Load<Sprite>($"Monsters/{monster.MonsterName}");
    BigMonster.gameObject.SetActive(true);
  }

  public void ShowCharacter(Player player)
  {
    var playerName = player.gameObject.GetComponentInChildren<SpriteRenderer>().sprite.name;
    BigMonster.sprite = Resources.Load<Sprite>($"Characters/{playerName}");
    BigMonster.gameObject.SetActive(true);
  }

  public void ShowMessage(string message)
  {
    if (popupMessage != null)
      popupMessage.ShowMessage(message);
  }
}
