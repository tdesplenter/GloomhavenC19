using UnityEngine;
using UnityEngine.EventSystems;

public class Trap : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler
{
  public bool IsSpawner = false;

  private Vector3 screenPoint;
  private Vector3 offset;
  private Vector3 currentPosition;

  public void OnPointerClick(PointerEventData eventData)
  {
    if (IsSpawner)
    {
      var newTrap = Instantiate(this, this.transform.parent);
      newTrap.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
      newTrap.IsSpawner = false;
      return;
    }

    if (eventData.clickCount >= 2)
    {
      Debug.Log("Double Click!");
      if (transform.parent?.GetComponent<Collider>() == null)
        Destroy(gameObject);
      else
        Destroy(transform.parent.gameObject);
    }
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (IsSpawner)
      return;

    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
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
