using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float jumpforce;
    public float jumpRaycastDistance;

    public bool OnGround;

    public GameObject objectToRotate;
    private bool rotating;

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
        if(elapsedTime >= 15 && shift == 0)
        {
            GravShiftLeft();
            elapsedTime = 0;
            shift = 1;
        }
        if (elapsedTime >= 15 && shift == 1)
        {
            GravShiftUp();
            elapsedTime = 0;
            shift = 2;
        }
        if (elapsedTime >= 15 && shift == 2)
        {
            GravShiftRight();
            elapsedTime = 0;
            shift = 3;
        }
        if (elapsedTime >= 15 && shift == 3)
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
            rb.velocity = new Vector3(-15f, 0f, 0f);
            OnGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true && shift == 2)
        {
            rb.velocity = new Vector3(0f, -15f, 0f);
            OnGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true && shift == 3)
        {
            rb.velocity = new Vector3(15f, 0f, 0f);
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
        //transform.rotation = Quaternion.Euler(0, 0, 180);
        StartRotation();
    }

    private void GravShiftDown()
    {
        GetComponent<ConstantForce>().force = new Vector3(0, -9.81f, 0);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        StartRotation();
    }

    private void GravShiftLeft()
    {
        GetComponent<ConstantForce>().force = new Vector3(9.81f, 0, 0);
        //transform.rotation = Quaternion.Euler(0, 0, 90);
        StartRotation();
    }

    private void GravShiftRight()
    {
        GetComponent<ConstantForce>().force = new Vector3(-9.81f, 0, 0);
        //transform.rotation = Quaternion.Euler(0, 0, -90);
        StartRotation();
    }

    private IEnumerator Rotate(Vector3 angles, float duration)
    {
        rotating = true;
        Quaternion startRotation = objectToRotate.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            objectToRotate.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        objectToRotate.transform.rotation = endRotation;
        rotating = false;
    }
    public void StartRotation()
    {
        if (!rotating)
            StartCoroutine(Rotate(new Vector3(0, 0, 90), 1));
    }
}
