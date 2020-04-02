using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleGoals : MonoBehaviour
{
  public Image[] allGoals;
  public Sprite[] allPlayers;

  public void AssignGoals()
  {
    for (int i = 0; i < 24; i++)
    {
      allGoals[i].transform.GetChild(0).gameObject.SetActive(false);
    }

    var rand = new System.Random();
    var assignedGoals = Enumerable.Range(0, 24).OrderBy(i => rand.Next()).Take(8).ToArray();

    for (int i = 0; i < 8; i++)
    {
      var goal = allGoals[assignedGoals[i]].transform.GetChild(0);
      goal.gameObject.SetActive(true);
      goal.GetComponent<Image>().sprite = allPlayers[i / 2];
    }
  }
}
