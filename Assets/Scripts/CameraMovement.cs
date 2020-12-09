using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  Vector3 originalPosition;

  Vector3 startPositionW, endPositionW;
  LineRenderer lineRenderer;

  Camera gameCamera;
  float drawOffset = 0.7f;

  private void Start()
  {
    originalPosition = Camera.main.transform.localPosition;
    gameCamera = Camera.main;
  }

  void Update()
  {
    if (Input.GetKey(KeyCode.Keypad0))
    {
      Camera.main.transform.localPosition = originalPosition;
      return;
    }

    var xChange = Input.GetAxis("Horizontal");
    var yChange = Input.GetAxis("Vertical");
    var zChange = Input.GetAxis("Mouse ScrollWheel");

    if (xChange + yChange + zChange != 0)
    {
      //Debug.Log($"{xChange} x {yChange} x {zChange}");

      var newX = Camera.main.transform.localPosition.x + (xChange * 0.1f);
      var newY = Camera.main.transform.localPosition.y + (yChange * -0.1f);
      var newZ = Camera.main.transform.localPosition.z + (zChange * -10f);

      newX = Mathf.Clamp(newX, -50f, 50f);
      newY = Mathf.Clamp(newY, -25f, 25f);
      newZ = Mathf.Clamp(newZ, 5f, 50f);

      Camera.main.transform.localPosition = new Vector3(newX, newY, newZ);
    }

    if (Input.GetMouseButtonDown(1))
    {
      if (lineRenderer == null)
        lineRenderer = gameObject.AddComponent<LineRenderer>();

      lineRenderer.enabled = true;
      lineRenderer.positionCount = 2;
      lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;

      var startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, (gameCamera.transform.position.z - drawOffset));
      startPositionW = gameCamera.ScreenToWorldPoint(startPosition);

      startPositionW.z = drawOffset;
      
      lineRenderer.SetPosition(0, startPositionW);
      lineRenderer.useWorldSpace = true;
    }

    if (Input.GetMouseButton(1))
    {
      var endPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, (gameCamera.transform.position.z - drawOffset));
      endPositionW = gameCamera.ScreenToWorldPoint(endPosition);

      endPositionW.z = drawOffset;

      lineRenderer.SetPosition(1, endPositionW);
    }

    if (Input.GetMouseButtonUp(1))
    {
      lineRenderer.enabled = false;
    }
  }
}
