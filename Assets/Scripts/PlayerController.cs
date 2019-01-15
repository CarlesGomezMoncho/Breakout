using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        if (horizontal != 0)
        {
            transform.position = new Vector2(transform.position.x + horizontal * movementSpeed * Time.deltaTime, transform.position.y);
        }
    }

    private void FixedUpdate()
    {
        
    }
}
