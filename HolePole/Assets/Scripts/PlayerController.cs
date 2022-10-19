using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotateSpeed;
    public Vector3 moveDirection;

    [SerializeField] private CharacterController characterController;

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(horizontal, 0.0f, vertical);
        moveDirection.Normalize();
        moveDirection *= speed;

        characterController.Move(moveDirection * Time.fixedDeltaTime);
        //transform.rotation = Quaternion.Euler(0f, Mathf.,0f);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toRotation,
                rotateSpeed * Time.fixedDeltaTime
            );
        }
    }
}
