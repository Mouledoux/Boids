using UnityEngine;
using System.Collections;

public class SpawnPost : MonoBehaviour
{
    public GameObject post;
	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(post, new Vector3(Random.Range(-50, 51), Random.Range(-30, 31), Random.Range(-50, 51)), new Quaternion(0, 0, 0, 0));
        }
	}
}
