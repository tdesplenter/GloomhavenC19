using UnityEngine;
using System.Linq;

public class PopupMenu : MonoBehaviour
{
  private PopupMenuItem selectedItem;
  private Vector3 spawnPosition;

  void Start()
  {
    this.gameObject.SetActive(false);
  }

  public void Show(Vector3 position)
  {
    // Show menu
    this.transform.localPosition = position;
    this.gameObject.SetActive(true);
    this.spawnPosition = position;
  }

  public void CheckAndActivate()
  {
    // If there's a selected menu item, activate it
    if (selectedItem != null)
      selectedItem.Activate(spawnPosition);

    // Hide menu
    this.gameObject.SetActive(false);
    ClearSelection();
  }

  public void SelectItem(PopupMenuItem item)
  {
    this.selectedItem = item;
  }

  public void ClearSelection()
  {
    this.selectedItem = null;
    this.gameObject.GetComponentsInChildren<PopupMenuItem>().ToList().ForEach(x => x.DeselectItem());
  }
}
