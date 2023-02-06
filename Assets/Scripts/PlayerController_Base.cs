using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController_Base : MonoBehaviour
{


    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform cameraTransform;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        //Referencia ao transforme da Main camera
        cameraTransform = Camera.main.transform;

        //Referencia da variavel moveAction para playerInput (mapa de inputs)
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }
        

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        MovePlayer();
        JumpPlayer();
        RotationCamera();
    }
    //Funcao de movimentacao, nos ixos X e Y
    private void MovePlayer()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        //Mover o player no mesmo sentido da camera
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        //Aceleração do player
        controller.Move(move * Time.deltaTime * playerSpeed);


    }

    //Funcao rotacionar em torno da direcao da camera
    private void RotationCamera()
    {
        
        
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);//Criar a rotaão referencial da camera.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);//Aplicar a rotação da camera, no transforme do player.


    }

    //Funcao de pulo
    private void JumpPlayer()
    {

        // Mudando a altitude do player. 
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            
        }
        //aplicando a gravidade no player.
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}
