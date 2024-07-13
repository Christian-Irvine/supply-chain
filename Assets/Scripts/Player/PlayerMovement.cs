using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private CharacterController characterController;

    private InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        moveInput = moveInput.normalized;

        Vector3 moveVector = (transform.forward * moveInput.y * baseSpeed) + (transform.right * moveInput.x * baseSpeed);

        characterController.Move(moveVector * Time.deltaTime);
    }
}
