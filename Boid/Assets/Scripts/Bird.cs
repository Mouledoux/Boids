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
        eProtector,
        eApex,
    }

    public enum State
    {
        eFlock,
        eHostel,
        eProtect,
    }

    public Species eSpecies;
    public State eState;
    public bool isPredator = false;

    void Update()
    {
        speed = velocity.magnitude;
        name = eSpecies.ToString();
    }
}