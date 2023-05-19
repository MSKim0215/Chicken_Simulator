using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject feedPrefab;
    public GameObject chickPrefab;

    private void Start()
    {
        for(int i = 0; i < 15; i++)
        {
            Vector3 randVec = UnityEngine.Random.insideUnitSphere;
            randVec *= 5;
            randVec.y = 0;

            Quaternion randomRot = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0);
            GameObject feed = Instantiate(feedPrefab, randVec, randomRot, transform);
            feed.transform.position = new Vector3(feed.transform.position.x, 0.05f, feed.transform.position.z);
        }

        for (int i = 0; i < 15; i++)
        {
            Vector3 randVec = UnityEngine.Random.insideUnitSphere;
            randVec *= 5;
            randVec.y = 0;

            Quaternion randomRot = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0);
            GameObject feed = Instantiate(chickPrefab, randVec, randomRot, transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 5f);
    }
}