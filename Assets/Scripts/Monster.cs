using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
  public Sprite sprite;
  public int maxnumber;
  public bool isElite;
  public bool isBoss;
  public int Number = 0;
  public bool IsDead = false;
  public bool IsSpawner = false;

  public string Name
  {
    get
    {
      return this.sprite.name;
    }
  }

  public int RoomNumber
  {
    get
    {
      return int.Parse(this.transform.parent.parent.name.Split(' ')[1]);
    }
  }

  private Vector3 screenPoint;
  private Vector3 offset;
  public Vector3 currentPosition;

  private void Awake()
  {
    if (sprite != null)
      Awaken();
  }

  public void Awaken()
  {
    this.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    this.GetComponentInChildren<TextMeshProUGUI>().color = isElite ? Color.yellow : Color.white;

    currentPosition = transform.position;
  }

  public void AssignNumber()
  {
    if (isBoss)
    {
      this.Number = 1;
      this.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(0.8f, 0.8f, 1f);
      this.GetComponentInChildren<TextMeshProUGUI>().text = "Boss";
      return;
    }

    var existingNumbers = GameManager.Instance.Monsters.Where(x => x.Number > 0 && !x.IsDead && x.Name == this.Name).Select(x => x.Number);
    
    if (existingNumbers.Count() >= maxnumber)
    {
      Debug.LogWarning("Could not assign number: no available numbers left");
      this.Number = 0;
      return;
    }

    var number = Random.Range(1, maxnumber + 1);
    while (existingNumbers.Contains(number))
      number = Random.Range(1, maxnumber + 1);

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
      if (GameManager.Instance.Monsters.Count(x => !x.IsDead && x.Name == this.Name) >= maxnumber)
        return; // No monsters of this type left

      var newMonster = Instantiate(this, this.transform.parent);
      newMonster.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
      newMonster.IsSpawner = false;
      newMonster.Number = 0;
      GameManager.Instance.Monsters.Add(newMonster);
      newMonster.AssignNumber();
      newMonster.currentPosition = newMonster.transform.position;

      DataManager.Instance.Save();

      return;
    }

    if (eventData.clickCount >= 2)
    {
      this.IsDead = true;
      Instantiate(GameManager.Instance.coinPrefab, currentPosition, transform.rotation, transform.parent);

      GameManager.Instance.MonsterKilled(this.Name);

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
}
