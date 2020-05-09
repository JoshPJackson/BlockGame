using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float speed = 0.2f;

    public float rotateSpeed = 3.0f;

    public float maxSpeed = 10.0f;

    public float jumpSpeed = 0.5f;

    public float mouseSensitivity = 0.4f;

    private Vector3 mouseStart;

    public float screenHeightMultiplier;

    public float screenWidthMultiplier;

    public Vector2 rotationRange = new Vector2(360, 180);

    public CharacterController character;

    float currentJumpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        screenHeightMultiplier = rotationRange.y / Screen.height;
        screenWidthMultiplier = rotationRange.x / Screen.width;
        character = Camera.main.GetComponent<CharacterController>();
        character.height = 2;
        character.center = new Vector3(0, -0.85f, 0);
        character.radius = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        var view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        bool isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        CharacterController character = Camera.main.GetComponent<CharacterController>();
        
        if (!isOutside)
        {
            Cursor.visible = false;
            Vector3 jump = Vector3.zero;
            Vector3 mousePosition = Input.mousePosition;
            Vector3 lookAt = Vector3.zero;

            // rotate based on mouse position
            if (mousePosition != mouseStart)
            {
                lookAt.y = screenWidthMultiplier * mousePosition.x - rotationRange.x / 2;
                lookAt.x = - screenHeightMultiplier * mousePosition.y + rotationRange.y / 2;
                lookAt.z = 0;
                character.transform.eulerAngles = lookAt;
            }

            if (!character.isGrounded)
            {
                currentJumpSpeed -= 2f * Time.deltaTime;
            }

            // apply jump
            if (character.isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                currentJumpSpeed = jumpSpeed;
            }
           
            // Rotate around y - axis
            float rightSpeed = speed * Input.GetAxis("Horizontal");
            float forwardSpeed = speed * Input.GetAxis("Vertical");
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Move forward / backward
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            character.Move(forward * forwardSpeed + right * rightSpeed + new Vector3(0, currentJumpSpeed, 0));
            
            mouseStart = Input.mousePosition;
        } else
        {
            Cursor.visible = true;
            character.SimpleMove(Vector3.zero);
        }
    }


}
