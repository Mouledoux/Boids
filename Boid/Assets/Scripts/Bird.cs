using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
    public Vector3 velocity;
    public GameObject target;
    public float speed;

    public enum Species
    {
        eBird,
        ePredator,
        eAlpha,
    }

    public enum State
    {
        eFlock,
        eHostel,
        eProtect,
    }

    public bool isPredator = false;

    void Update()
    {
        speed = velocity.magnitude;
    }

}