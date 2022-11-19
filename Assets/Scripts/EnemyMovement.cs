using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] target;

    public float speed;
    private int current;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != target[current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed*Time.deltaTime);
            rb.MovePosition(pos);
            transform.LookAt(target[current].position);
        }
        else
        {
            current = (current + 1) % target.Length;
        }
    }
}
