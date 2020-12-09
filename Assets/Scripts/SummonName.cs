using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SummonName : MonoBehaviour, IPointerClickHandler
{
  private string summonName;
  public TextMeshProUGUI textField;
  public InputField inputField;

  // Start is called before the first frame update
  void Start()
  {
    summonName = "Summon";
    textField.text = summonName;

    inputField.gameObject.SetActive(false);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.clickCount >= 2)
    {
      inputField.text = summonName;
      inputField.gameObject.SetActive(true);
    }
  }

  public void SetName(string newName)
  {
    summonName = newName;
    textField.text = summonName;
    inputField.gameObject.SetActive(false);
  }
}
