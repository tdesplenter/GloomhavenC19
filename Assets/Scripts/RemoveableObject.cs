using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveableObject : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    Debug.Log("Click!");
    if (eventData.clickCount >= 2)
    {
      Debug.Log("Double Click!");
      if (transform.parent?.GetComponent<Collider>() == null)
        Destroy(gameObject);
      else
        Destroy(transform.parent.gameObject);
    }
  }
}
