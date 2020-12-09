using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MonsterType
{
  None,
  AncientArtillery,
  BanditArcher,
  BanditGuard,
  BlackImp,
  CaveBear,
  CityArcher,
  CityGuard,
  Cultist,
  DeepTerror,
  EarthDemon,
  FlameDemon,
  ForestImp,
  FrostDemon,
  GiantViper,
  HarrowerInfester,
  Hound,
  InoxArcher,
  InoxGuard,
  InoxShaman,
  LivingBones,
  LivingCorpse,
  LivingSpirit,
  Lurker,
  NightDemon,
  Ooze,
  RendingDrake,
  SavvasIcestorm,
  SavvasLavaflow,
  SpittingDrake,
  StoneGolem,
  SunDemon,
  VermlingScout,
  VermlingShaman,
  WindDemon,
  Jekserah,
  DarkRider,
  SightlessEye,
  WingedHorror,
  TheGloom
}

public class Monster : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
  public MonsterType monsterType;
  private string monsterName;
  private Sprite sprite;
  private int maxNumber;

  public bool isHorizontal;
  public bool isElite;
  public bool isBoss;
  public int Number = 0;
  public bool IsDead = false;
  public bool IsSpawner = false;
  public bool IsSpawnOrSummoned = false;

  private Dictionary<MonsterType, int> maxNumbers = new Dictionary<MonsterType, int>()
  {
    { MonsterType.AncientArtillery, 6 },
    { MonsterType.BanditArcher, 6 },
    { MonsterType.BanditGuard, 6 },
    { MonsterType.BlackImp, 10 },
    { MonsterType.CaveBear, 4 },
    { MonsterType.CityArcher, 6 },
    { MonsterType.CityGuard, 6 },
    { MonsterType.Cultist, 6 },
    { MonsterType.DeepTerror, 10 },
    { MonsterType.EarthDemon, 6 },
    { MonsterType.FlameDemon, 6 },
    { MonsterType.ForestImp, 10 },
    { MonsterType.FrostDemon, 6 },
    { MonsterType.GiantViper, 10 },
    { MonsterType.HarrowerInfester, 4 },
    { MonsterType.Hound, 6 },
    { MonsterType.InoxArcher, 6 },
    { MonsterType.InoxGuard, 6 },
    { MonsterType.InoxShaman, 4 },
    { MonsterType.LivingBones, 10 },
    { MonsterType.LivingCorpse, 6 },
    { MonsterType.LivingSpirit, 6 },
    { MonsterType.Lurker, 6 },
    { MonsterType.NightDemon, 6 },
    { MonsterType.Ooze, 10 },
    { MonsterType.RendingDrake, 6 },
    { MonsterType.SavvasIcestorm, 4 },
    { MonsterType.SavvasLavaflow, 4 },
    { MonsterType.SpittingDrake, 6 },
    { MonsterType.StoneGolem, 6 },
    { MonsterType.SunDemon, 6 },
    { MonsterType.VermlingScout, 10 },
    { MonsterType.VermlingShaman, 6 },
    { MonsterType.WindDemon, 6 }
  };

  public static string GetName(MonsterType monsterType)
  {
    return string.Join(" ", Regex.Split(monsterType.ToString(), @"(?<!^)(?=[A-Z])"));
  }

  public string MonsterName
  {
    get
    {
      return monsterName;
    }
    set
    {
      monsterName = value;
    }
  }

  private Vector3 screenPoint;
  private Vector3 offset;
  public Vector3 currentPosition;

  public void Awaken()
  {
    if (isBoss)
    {
      monsterName = monsterType.ToString();
      this.sprite = Resources.Load<Sprite>($"BossTokens/{monsterName}");
      this.maxNumber = 1;
    }
    else
    {
      if (monsterType == MonsterType.None)
      {
        Debug.LogWarning("Missing Monster Type");
        return;
      }
      if (!maxNumbers.ContainsKey(monsterType))
      {
        Debug.LogWarning($"Missing Max Number: {monsterType}");
        return;
      }

      monsterName = GetName(monsterType);
      var horzOrVert = isHorizontal ? "Horz-" : "Vert-";
      this.sprite = Resources.Load<Sprite>($"MonsterTokens/{horzOrVert}{monsterName}");
      this.maxNumber = maxNumbers[monsterType];
    }

    //Debug.Log($"{monsterName}: max {maxNumber}");

    this.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    this.GetComponentInChildren<TextMeshProUGUI>().color = isElite ? Color.yellow : Color.white;

    currentPosition = transform.position;
  }

  public void AssignNumber()
  {
    if (isBoss)
    {
      this.Number = 1;
      this.GetComponentInChildren<TextMeshProUGUI>().text = "";
      return;
    }

    var existingNumbers = GameManager.Instance.Monsters.Where(x => x.Number > 0 && !x.IsDead && x.MonsterName == this.MonsterName).Select(x => x.Number);

    if (existingNumbers.Count() >= maxNumber)
    {
      GameManager.Instance.ShowMessage($"No monsters left of type {MonsterName} (max: {maxNumber})");
      Debug.LogWarning($"Could not assign number: no available numbers left (max: {maxNumber})");
      this.Number = 0;
      return;
    }

    var number = Random.Range(1, maxNumber + 1);
    while (existingNumbers.Contains(number))
      number = Random.Range(1, maxNumber + 1);

    this.Number = number;
    ShowNumber();
  }

  public void ShowNumber()
  {
    this.GetComponentInChildren<TextMeshProUGUI>().text = this.Number.ToString();
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (IsSpawner)
    {
      Debug.Log($"Spawning {this.MonsterName}");

      if (GameManager.Instance.Monsters.Count(x => !x.IsDead && x.MonsterName == this.MonsterName) >= maxNumber)
      {
        Debug.Log($"No monsters left of type {MonsterName} (max: {maxNumber})");
        GameManager.Instance.ShowMessage($"No monsters left of type {MonsterName} (max: {maxNumber})");
        return; // No monsters of this type left
      }

      var newMonster = Instantiate(this, this.transform.parent);
      newMonster.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
      newMonster.IsSpawner = false;
      newMonster.IsSpawnOrSummoned = true;
      newMonster.Number = 0;
      newMonster.maxNumber = this.maxNumber;
      newMonster.monsterName = newMonster.name = MonsterName;
      GameManager.Instance.Monsters.Add(newMonster);
      newMonster.AssignNumber();
      newMonster.currentPosition = newMonster.transform.position;

      DataManager.Instance.Save();

      return;
    }

    if (eventData.button == PointerEventData.InputButton.Right)
    {
      Debug.Log("Right Click");
      GameManager.Instance.ShowMonster(this);
      return;
    }

    if (eventData.clickCount >= 2)
    {
      Debug.Log("Double Click");

      this.IsDead = true;

      if (!IsSpawnOrSummoned)
      {
        var coinPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 0.1f);
        Debug.Log(currentPosition);
        Debug.Log(coinPosition);
        Instantiate(GameManager.Instance.coinPrefab, coinPosition, transform.rotation, transform.parent);
      }

      GameManager.Instance.MonsterKilled(this.MonsterName);

      Destroy(gameObject);

      DataManager.Instance.Save();
    }
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (IsSpawner)
      return;

    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    DataManager.Instance.Save();
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (IsSpawner)
      return;

    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    currentPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

    transform.position = currentPosition;
  }

#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    if (this.monsterType != MonsterType.None)
      Handles.Label(transform.position, GetName(this.monsterType) + "\n" + (this.isElite ? "(elite)" : "(normal)"));
  }
#endif
}
