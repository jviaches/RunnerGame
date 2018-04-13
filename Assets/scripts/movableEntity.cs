using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movableEntity : MonoBehaviour
{
    //public event EventHandler CollectedCoin;   // raise each time when this object "touched" coin

    private Rigidbody rb;
    private float speed = 5f;
    
    private bool isMovingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void changeDirection()
    {
        //isMovingRight = !isMovingRight;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            changeDirection();

        if (isMovingRight)
            rb.velocity = new Vector3(speed, rb.velocity.y > 0 ? 0: rb.velocity.y, 0f);
        else
            rb.velocity = new Vector3(0f, rb.velocity.y > 0 ? 0 : rb.velocity.y, speed);

        detectFreeFall();
        //detectBonus();
    }

    public bool detectFreeFall()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f) && hit.transform.gameObject.tag == "Ground")
            return false;
        else
            return true;
    }

    //private void detectBonus()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f) && hit.transform.gameObject.tag == "coin")
    //    {
    //        OnCollectedCoin();
    //        Destroy(hit.transform.gameObject);
    //    }

    //    if (Physics.Raycast(transform.position, Vector3.back, out hit, 1f) && hit.transform.gameObject.tag == "coin")
    //    {
    //        OnCollectedCoin();
    //        Destroy(hit.transform.gameObject);
    //    }

    //    if (Physics.Raycast(transform.position, Vector3.right, out hit, 1f) && hit.transform.gameObject.tag == "coin")
    //    {
    //        OnCollectedCoin();
    //        Destroy(hit.transform.gameObject);
    //    }

    //    if (Physics.Raycast(transform.position, Vector3.left, out hit, 1f) && hit.transform.gameObject.tag == "coin")
    //    {
    //        OnCollectedCoin();
    //        Destroy(hit.transform.gameObject);
    //    }
    //}

    //private void OnCollectedCoin()
    //{
    //    if (CollectedCoin != null)
    //        CollectedCoin(this, new EventArgs());
    //}
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
