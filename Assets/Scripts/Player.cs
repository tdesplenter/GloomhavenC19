using UnityEngine;

public class Player : MonoBehaviour
{
  private Vector3 screenPoint;
  private Vector3 offset;

  public string Name
  {
    get
    {
      return this.name;
    }
  }

  void OnMouseDown()
  {
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
  }

  void OnMouseDrag()
  {
    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

    if (transform.GetComponent<Collider>() == null)
      transform.parent.position = curPosition;
    else
      transform.position = curPosition;

    DataManager.Instance.Save();
  }
}
