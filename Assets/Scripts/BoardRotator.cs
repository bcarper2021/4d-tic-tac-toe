using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotator : MonoBehaviour {

    private bool _isRotating;

    public GameObject rotationParent;
    float rotationSpeed = 20;

    void Update()
    {
        if (_isRotating)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.deltaPosition;

                float rotationX = pos.x * rotationSpeed * Mathf.Deg2Rad;
                float rotationY = pos.y * rotationSpeed * Mathf.Deg2Rad;

                rotationParent.transform.Rotate(Vector3.up, -rotationX, Space.World);
                rotationParent.transform.Rotate(Vector3.right, rotationY, Space.World);
            }
        }
    }

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
}
