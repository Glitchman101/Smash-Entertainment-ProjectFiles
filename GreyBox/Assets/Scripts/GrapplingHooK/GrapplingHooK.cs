using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHooK : MonoBehaviour
{
    public GameObject hook;
    public GameObject hookHolder;

    public float hookTravelSpeed;
    public float playerTravelSpeed;

    public static bool fired;
    public bool hooked;
    public GameObject hookedObj;

    public float maxDistance;
    private float currentDistance;

    public ParticleSystem hookParticle;

    private bool grounded;

    private void Update()
    {
        //firing the hook
        if (Input.GetMouseButtonDown(1) && fired == false)
            fired = true;

        if (fired)
        {
            LineRenderer rope = hook.GetComponent<LineRenderer>();
            rope.SetVertexCount(2);
            rope.SetPosition(0, hookHolder.transform.position);
            rope.SetPosition(1, hook.transform.position);

        }

        if (fired == true && hooked == false)
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed);
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if (currentDistance >= maxDistance)
                ReturnHook();
        }

        if (hooked == true)
        {
            hook.transform.position = hookedObj.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);
            hookParticle.Play();

            this.GetComponent<Rigidbody>().useGravity = false;

            if (distanceToHook < 1.5)
            {
                ReturnHook();
            }
        }
        else {
            hook.transform.parent = hookHolder.transform;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void ReturnHook()
    {
        hook.transform.rotation = hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;

        LineRenderer rope = hook.GetComponent<LineRenderer>();
        rope.SetVertexCount(0);

        hookParticle.Stop();

    }

    void CheckIfGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            grounded = true;
        } else {
            grounded = false;
        }
    }

}
