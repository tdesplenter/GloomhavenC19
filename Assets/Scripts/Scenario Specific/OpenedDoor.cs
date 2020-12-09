using UnityEngine;
using UnityEngine.EventSystems;

public class OpenedDoor : MonoBehaviour, IPointerClickHandler
{
  private bool IsOpened = false;
  public SpriteRenderer sprite;

  public void OnPointerClick(PointerEventData eventData)
  {
    Debug.Log("Clicked");
    if (eventData.clickCount >= 2)
    {
      IsOpened = !IsOpened;
      sprite.color = IsOpened ? Color.green : Color.white;
      Debug.Log("Sprite Changed");
    }
  }
}
