using UnityEngine;

public class MapTileScript : MonoBehaviour
{
  // Start is called before the first frame update
  void Awake()
  {
    GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    GetComponent<Renderer>().receiveShadows = true;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
