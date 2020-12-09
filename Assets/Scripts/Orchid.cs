using TMPro;
using UnityEngine;

public class Orchid : MonoBehaviour
{
  public int StartingLife = 10; // 4 + (2 x L);
  private int LifeLeft;
  public TextMeshProUGUI CounterText;

  // Start is called before the first frame update
  void Start()
  {
    this.LifeLeft = StartingLife;

    UpdateItem();
  }

  private void UpdateItem()
  {
    this.CounterText.text = this.LifeLeft.ToString();
  }

  public void IncreaseCount()
  {
    this.LifeLeft++;
    UpdateItem();
  }

  public void DecreaseCount()
  {
    if (this.LifeLeft > 0)
    {
      this.LifeLeft--;
      UpdateItem();
    }
  }
}
