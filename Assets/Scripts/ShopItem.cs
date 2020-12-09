using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
  public TextMeshProUGUI CounterText;
  public ShopItemData Data;

  void Start()
  {

  }

  public void UpdateData(ShopItemData data)
  {
    this.Data = data;
    this.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShopItems/" + data.SpriteName);

    UpdateItem();
  }

  private void UpdateItem()
  {
    this.CounterText.text = this.Data.CountLeft.ToString();
    if (this.Data.CountLeft == 0)
      this.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    else
      this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
  }

  public void IncreaseCount()
  {
    if (this.Data.CountLeft < this.Data.CountTotal)
    {
      this.Data.CountLeft++;
      UpdateItem();
      ShopManager.Instance.SaveData();
    }
  }

  public void DecreaseCount()
  {
    if (this.Data.CountLeft > 0)
    {
      this.Data.CountLeft--;
      UpdateItem();
      ShopManager.Instance.SaveData();
    }
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button == PointerEventData.InputButton.Right)
    {
      Debug.Log("Right Click");
      ShopManager.Instance.ShowItem(this);
      return;
    }
  }
}
