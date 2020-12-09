using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Summon : MonoBehaviour, IPointerClickHandler
{
  private Vector3 screenPoint;
  private Vector3 offset;

  private string summonName;
  private int movement;

  public TextMeshProUGUI textField;
  public InputField inputField;
  public Button deleteButton;

  public Button moveMinusButton;
  public Button movePlusButton;
  public Text movementText;

  void Start()
  {
    summonName = "Summon";
    textField.text = summonName;

    movement = 0;

    inputField.gameObject.SetActive(false);
    deleteButton.gameObject.SetActive(false);
    moveMinusButton.gameObject.SetActive(false);
    movePlusButton.gameObject.SetActive(false);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.clickCount >= 2)
    {
      inputField.text = summonName;
      inputField.gameObject.SetActive(true);
      deleteButton.gameObject.SetActive(true);
      moveMinusButton.gameObject.SetActive(true);
      movePlusButton.gameObject.SetActive(true);
    }
  }

  public void SetName(string newName)
  {
    summonName = newName;
    textField.text = summonName;
    inputField.gameObject.SetActive(false);
    deleteButton.gameObject.SetActive(false);
    moveMinusButton.gameObject.SetActive(false);
    movePlusButton.gameObject.SetActive(false);
  }

  public void Unsummon()
  {
    Destroy(gameObject);
  }

  public void MovementUp()
  {
    movement++;
    this.movementText.text = movement.ToString();
  }

  public void MovementDown()
  {
    if (movement > 0)
      movement--;
    this.movementText.text = movement.ToString();
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
  }
}
