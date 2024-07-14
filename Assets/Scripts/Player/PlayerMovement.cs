using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private CharacterController characterController;
    [SerializeField, Range(0, 20)] private int rotateSpeed;

    private InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        moveInput = moveInput.normalized;

        Vector3 moveVector = new Vector3(moveInput.x * baseSpeed, 0, moveInput.y * baseSpeed);

        characterController.Move(moveVector * Time.deltaTime);

        transform.forward += moveVector * rotateSpeed * Time.deltaTime;

        //transform.rotation = Quaternion.Euler(Vector3.RotateTowards(transform.position, moveVector, rotateSpeed * Time.deltaTime, 0));
                             //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveVector.x, 0, moveVector.y)), rotateSpeed * Time.deltaTime);
                             //Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(moveVector.x, 0, moveVector.y)), rotateSpeed * Time.deltaTime);
            
            //Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, moveVector.x + moveVector.y, 0) * 90), rotateSpeed * Time.deltaTime);
            
    }
}
