using UnityEngine;


public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Transform cam; //カメラのTransform

    public float speed = 10f;
    public float rotationSpeed = 10f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movePlayer = new Vector3(horizontal, 0, vertical).normalized;
        

        if (movePlayer != Vector3.zero)
        {
            Vector3 camForward = cam.forward;
            Vector3 camRight = cam.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * movePlayer.z + camRight * movePlayer.x; 

            rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = targetRot;
        }
    }
}