using UnityEngine;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(MeshCollider))]
public class MoveableObject : MonoBehaviour, IDragHandler, IPointerDownHandler
{
  private Vector3 screenPoint;
  private Vector3 offset;

  public void OnPointerDown(PointerEventData eventData)
  {
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
  }

  public void OnDrag(PointerEventData eventData)
  {
    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    var currentPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

    transform.position = currentPosition;
  }
}