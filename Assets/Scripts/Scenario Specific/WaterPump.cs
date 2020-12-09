using UnityEngine;
using UnityEngine.EventSystems;

public class WaterPump : MonoBehaviour, IPointerClickHandler
{
  private bool IsCleansed = false;
  public SpriteRenderer marker;

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.clickCount >= 2)
    {
      IsCleansed = !IsCleansed;
      marker.color = IsCleansed ? Color.green : Color.white;
    }
  }
}
