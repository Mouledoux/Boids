using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
    public GameObject[] flock;

    public float cohesionMod;
    public float alignmentMod;
    public float seperationMod;
    public int threshold;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (GameObject b in flock)
        {
            //b.transform.position += Cohesion(b) * cStr;
            b.transform.position += Seperation(b);// *sepStr;
            //b.transform.position += Alignment(b);
            //b.transform.position += ( Cohesion(b) //+ Alignment(b) + Seperation(b)  );

        }
       // Fly();
	}

    Vector3 Cohesion(GameObject bird)
    {
        Vector3 COM = Vector3.zero;

        foreach (GameObject b in flock)
        {
            if(b != bird)
            {
                COM += b.transform.position;
            }
        }

        COM /= flock.Length - 1; // Divid by n-1 to find average

        return (COM - bird.transform.position) / 100;
    }
    Vector3 Alignment(GameObject bird)
    {
        Vector3 align = Vector3.zero;

        foreach (GameObject b in flock)
        {
            if (b != bird)
            {
                //align += b.GetComponent<Bird>().velocity;
            }
        }

        align /= flock.Length - 1;

        return (align);// - bird.GetComponent<Bird>().velocity) / 8;
    }
    Vector3 Seperation(GameObject bird)
    {
        Vector3 pos = Vector3.zero;

        foreach (GameObject b in flock)
        {
            if (b != bird)
            {
                if (Vector3.Distance( bird.transform.position, b.transform.position) < threshold)
                {
                    pos -= (bird.transform.position - b.transform.position);
                }
            }
        }

        return pos;
    }

    public float cStr = -1.0f;
    public float sepStr = 1.0f;
    void Fly()
    {
        
    }
}
