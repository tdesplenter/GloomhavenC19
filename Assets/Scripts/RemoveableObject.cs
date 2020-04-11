using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveableObject : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.clickCount >= 2)
    {
      if (transform.parent?.GetComponent<Collider>() == null)
        Destroy(gameObject);
      else
        Destroy(transform.parent.gameObject);
    }
  }
}
