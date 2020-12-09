using System.Linq;
using TMPro;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
  public MonsterType monsterType;

  void Awake()
  {
    this.transform.Find("Canvas/Title").GetComponent<TextMeshProUGUI>().text = Monster.GetName(monsterType);

    this.GetComponentsInChildren<Monster>().ToList().ForEach(x => { x.monsterType = monsterType; x.Awaken(); });
  }
}

