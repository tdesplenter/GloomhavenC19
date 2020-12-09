using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenuItem : MonoBehaviour
{
  private Vector3 originalScale;

  public GameObject spawnToken;
  public bool isSubMenu;
  public bool isHorizontal;

  public bool HasSubMenu
  {
    get
    {
      return children.Any();
    }
    private set { }
  }

  private List<PopupMenuItem> children;

  private void Start()
  {
    originalScale = this.transform.localScale;
    children = this.transform.GetComponentsInChildren<PopupMenuItem>(true).ToList();
    children.Remove(this);
    DeselectItem();
  }

  public void SelectItem()
  {
    if (spawnToken != null)
    {
      // Individual item
      this.transform.localScale = originalScale * 1.4f;
    }
    else if (HasSubMenu)
    {
      // Submenu
      children.ForEach(x => x.gameObject.SetActive(true));
    }
  }

  public void DeselectItem()
  {
    if (spawnToken != null)
    {
      // Individual item
      this.transform.localScale = originalScale;
    }
    else if (HasSubMenu)
    {
      // Submenu
      children.ForEach(x => x.gameObject.SetActive(false));
    }
  }

  private void OnMouseEnter()
  {
    if (!isSubMenu)
      GameManager.Instance.ClearPopupMenuSelection();
    GameManager.Instance.SetActiveItem(this);
    SelectItem();
  }

  private void OnMouseExit()
  {
    if (!HasSubMenu)
      DeselectItem();
  }

  public void Activate(Vector3 position)
  {
    //Debug.Log("Popup Item Activate");

    if (spawnToken != null)
    {
      position.z = 0.6f;

      var newSpawn = Instantiate(spawnToken);
      newSpawn.transform.position = position;
      newSpawn.transform.rotation = isHorizontal ? Quaternion.Euler(0, 90, 90) : Quaternion.Euler(90, 90, 90);

      // Extra logic
      if (newSpawn.GetComponent<Trap>() != null)
      {
        newSpawn.GetComponent<Trap>().IsSpawner = false;
        // TODO: rotate effect marker when isHorizontal? (0, 0, 90)
      }

      if (newSpawn.GetComponent<Monster>() != null)
      {
        var monster = newSpawn.GetComponent<Monster>();
        monster.IsSpawner = false;
        monster.IsSpawnOrSummoned = true;
        monster.Number = 0;
        GameManager.Instance.Monsters.Add(monster);
        monster.AssignNumber();
        monster.currentPosition = monster.transform.position;
      }
    }
  }
}
