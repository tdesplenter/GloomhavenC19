using TMPro;
using UnityEngine;

public class DestructableItem : MonoBehaviour
{
  public string title = "";
  public int initialLife = 16;

  private int lifeLeft;

  private TextMeshProUGUI counterText;
  private TextMeshProUGUI titleText;
  private SpriteRenderer background;

  void Start()
  {
    counterText = this.transform.Find("Canvas/CounterImage/CounterText").GetComponent<TextMeshProUGUI>();
    titleText = this.transform.Find("Canvas/Title").GetComponent<TextMeshProUGUI>();
    background = this.transform.Find("object/sprite").GetComponent<SpriteRenderer>();

    if (string.IsNullOrEmpty(title))
      titleText.gameObject.SetActive(false);
    else
      titleText.text = title;

    lifeLeft = initialLife;

    UpdateItem();
  }

  private void UpdateItem()
  {
    counterText.text = this.lifeLeft.ToString();
    if (lifeLeft == 0)
      background.color = new Color(1, 0, 0, 1);  // Red
    else
      background.color = new Color(0, 1, 0, 1);  // Green
  }

  public void IncreaseCount()
  {
    lifeLeft++;
    UpdateItem();
  }

  public void DecreaseCount()
  {
    if (lifeLeft > 0)
    {
      lifeLeft--;
      UpdateItem();
    }
  }
}
