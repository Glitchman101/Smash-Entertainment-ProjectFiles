using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float jumpforce;
    public float jumpRaycastDistance;

    private bool OnGround;
    
    private Rigidbody rb;

    float elapsedTime;
    float shift;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        elapsedTime = 0;
        shift = 0;
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= 10 && shift == 0)
        {
            GravShiftRight();
            elapsedTime = 0;
            shift = 1;
        }
        if (elapsedTime >= 10 && shift == 1)
        {
            GravShiftUp();
            elapsedTime = 0;
            shift = 2;
        }
        if (elapsedTime >= 10 && shift == 2)
        {
            GravShiftLeft();
            elapsedTime = 0;
            shift = 3;
        }
        if (elapsedTime >= 10 && shift == 3)
        {
            GravShiftDown();
            elapsedTime = 0;
            shift = 0;
        }
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(hAxis, 0, vAxis) * speed * Time.fixedDeltaTime;

        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement);

        rb.MovePosition(newPosition);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true && shift == 0)
        {
            rb.velocity = new Vector3(0f, 15f, 0f);
            OnGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true && shift == 1)
        {
            rb.velocity = new Vector3(15f, 0f, 0f);
            OnGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true && shift == 2)
        {
            rb.velocity = new Vector3(0f, -15f, 0f);
            OnGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true && shift == 3)
        {
            rb.velocity = new Vector3(-15f, 0f, 0f);
            OnGround = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
            OnGround = true;
    }

    private void GravShiftUp()
    {
        GetComponent<ConstantForce>().force = new Vector3(0, 9.81f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    private void GravShiftDown()
    {
        GetComponent<ConstantForce>().force = new Vector3(0, -9.81f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void GravShiftLeft()
    {
        GetComponent<ConstantForce>().force = new Vector3(9.81f, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    private void GravShiftRight()
    {
        GetComponent<ConstantForce>().force = new Vector3(-9.81f, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}
