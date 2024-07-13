using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField][Range(0, 20)] private float horizontalOffset;
    [SerializeField][Range(0, 20)] private float verticalOffset;
    [SerializeField][Range(0, 20)] private float followSpeed;

    void Update()
    {
        Vector3 goalPos = new Vector3 (transform.position.x, transform.position.y + verticalOffset, transform.position.z - horizontalOffset);

        float followTime = followSpeed * Time.deltaTime;
        Vector3 newPos = new Vector3(
            Mathf.Lerp(cam.transform.position.x, goalPos.x, followTime),
            Mathf.Lerp(cam.transform.position.y, goalPos.y, followTime),
            Mathf.Lerp(cam.transform.position.z, goalPos.z, followTime)
        );

        cam.transform.position = newPos;



    }
}
