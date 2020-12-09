using TMPro;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
  private TextMeshProUGUI messageText;
  private string currentMessage;
  private int messageCount = 1;
  private bool isVisible;

  private float timeVisible = 3f;
  private float timeShown;

  void Awake()
  {
    this.gameObject.SetActive(false);
    messageText = this.transform.Find("Message").GetComponent<TextMeshProUGUI>();
  }

  void Update()
  {
    if (isVisible)
    {
      timeShown += Time.deltaTime;

      if (timeShown >= timeVisible)
        HideMessage();
    }
  }

  public void ShowMessage(string message)
  {
    Debug.Log("Showing Popup Message");

    if (message == currentMessage)
      messageCount++;
    else
      messageCount = 1;

    currentMessage = message;
    messageText.text = currentMessage;
    if (messageCount > 1)
      messageText.text += $" ({messageCount})";

    this.gameObject.SetActive(true);
    timeShown = 0f;
    isVisible = true;
  }

  private void HideMessage()
  {
    Debug.Log("Hiding Popup Message");

    this.gameObject.SetActive(false);
    currentMessage = string.Empty;
    isVisible = false;
    messageCount = 1;
  }
}
