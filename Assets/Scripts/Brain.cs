using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public DNA dna;
    public GameObject eyes;
    private LayerMask ignore = 6;

    private (bool left, bool forward, bool right) seeFeed;
    private bool canMove = false;

    public float feedsFound = 0;

    public void Init()
    {
        dna = new DNA();
    }

    private void Update()
    {
        //seeFeed = (false, false, false);
        //bool left = false;
        //bool front = false;
        //bool right = false;
        //canMove = true;

        //RaycastHit hit;
        //Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 1f, Color.red);

        //if(Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 1f, ~ignore))
        //{
        //    if(hit.collider.gameObject.CompareTag("Feed"))
        //    {
        //        front = true;
        //        canMove = false;
        //    }
        //}

        //if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.right, out hit, 1f, ~ignore))
        //{
        //    if (hit.collider.gameObject.CompareTag("Feed"))
        //    {
        //        right = true;
        //    }
        //}

        //if (Physics.SphereCast(eyes.transform.position, 0.1f, -eyes.transform.right, out hit, 1f, ~ignore))
        //{
        //    if (hit.collider.gameObject.CompareTag("Feed"))
        //    {
        //        left = true;
        //    }
        //}
        //seeFeed = (left, front, right);
    }

    private void FixedUpdate()
    {
        //transform.Rotate(0, dna.genes[seeFeed], 0);
        //if(canMove)
        //{
        //    transform.Translate(0f, 0f, 0.1f);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Feed"))
        {
            feedsFound++;
            other.gameObject.SetActive(false);
        }
    }
}