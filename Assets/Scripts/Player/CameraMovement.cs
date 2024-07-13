using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField][Range(0, 20)] private float horizontalOffset;
    [SerializeField][Range(0, 20)] private float verticalOffset;

    void Update()
    {
        Vector3 newPos = new Vector3 (transform.position.x, transform.position.y + verticalOffset, transform.position.z - horizontalOffset);

        cam.transform.position = newPos;
    }
}
