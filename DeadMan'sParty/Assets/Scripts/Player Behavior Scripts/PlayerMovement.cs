using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Movement Variables
    Rigidbody2D rb2d;

    [SerializeField]
    int movementForce = 3;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component to access it
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // fixed updates update at a constant rate and not bound by local framerate
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * movementForce * Time.deltaTime, 0f, 0f));
        }

        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * movementForce * Time.deltaTime, 0f));
        }
    }
}