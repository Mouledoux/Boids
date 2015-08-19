using UnityEngine;
using System.Collections.Generic;

public class BetterBoid : MonoBehaviour
{

    public List<GameObject> flock;

    public GameObject boid;

    public float m_cohesionMod;     // Strength of cohestion
    public float m_alignmentMod;    // Addition to current velocity
    public float m_seperationMod;   // Strength of seperation

    public float m_sThreshold;      // Threshold of seperation

    Vector3 cohesion;
    Vector3 alignment;
    Vector3 seperation;

	// Use this for initialization
	void Start ()
    {
        AddBoids(100);

	}
	
	// Update is called once per frame
	void Update ()
    { 
	    foreach (GameObject bird in flock)
        {
            cohesion = Cohesion(bird);// *1;          // Multiply cohesion vector by mod scalar
            bird.GetComponent<Bird>().velocity += cohesion * m_cohesionMod;

            //bird.GetComponent<Bird>().Position += cohesion;  // Adds cohesion to current bird's position

            seperation = Seperation(bird) * m_seperationMod;  // Multiply seperation vector by mod scalar
            bird.GetComponent<Bird>().velocity += seperation; // Adds seperation to current bird's position

            alignment = Alignment(bird) * m_alignmentMod;
            bird.GetComponent<Bird>().velocity += alignment;

            VelocityLimit(bird);
            bird.transform.position += bird.GetComponent<Bird>().velocity;
        }
	}

    void VelocityLimit(GameObject b)
    {
        if (b.GetComponent<Bird>().speed > 2)
        {
            b.GetComponent<Bird>().velocity = b.GetComponent<Bird>().velocity.normalized * .150f;
        }
    }

    // Finds the center of mass and attracts the bird to it
    Vector3 Cohesion (GameObject bird)
    {
        Vector3 COM = Vector3.zero; // Center of mass

        foreach (GameObject b in flock)
        {
            if (b != bird)                      // If the birds are not the same
            {
                if (Vec3Dist(b.transform.localPosition, bird.transform.localPosition) < 2)
                {
                    COM += b.transform.position; // Add the position to COM
                }
            }
        }

        COM /= (flock.Count - 1); // Divid COM by n-1 to get the average
        
        return (COM - bird.transform.position) / 100; // Returns the birds current position 1% closer to COM
    }

    Vector3 Seperation (GameObject bird)
    {
        Vector3 pos = Vector3.zero; // New position after resistance

        foreach (GameObject b in flock)
        {
            if (b != bird) // If the birds are not the same
            {
                if (Vec3Dist(b.transform.position, bird.transform.position) < m_sThreshold) // If the birds are too close
                {
                    pos -= (b.transform.localPosition - bird.transform.localPosition);              // Avoid it
                         // The closer they are the harder to resist
                }
            }
        }

        return pos / 8; // Rrturns the birds current position 1% further from the other bird
    }

  
    Vector3 Alignment (GameObject bird)
    {
        Vector3 align = Vector3.zero;

        foreach (GameObject b in flock)
        {
            if (b != bird) // If the birds are not the same
            {
                align += b.GetComponent<Bird>().velocity;
            }
        }

        align /= flock.Count - 1;

        return align;
    }

    float Vec3Dist(Vector3 a, Vector3 b)
    {
        return (Mathf.Sqrt((a.x - b.x) * (a.x - b.x) +
                           (a.y - b.y) * (a.y - b.y) +
                           (a.z - b.z) * (a.z - b.z)));
    }  // Distance between 2 Vector3

    void AddBoids(int numBoids)
    {
        for (int i = 0; i < numBoids; i++)
        {
            GameObject b = Instantiate(boid) as GameObject;
            
            b.transform.parent = gameObject.transform;           
           
            b.transform.position = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
            b.GetComponent<Bird>().velocity = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));

            flock.Add(b);
        }
        
    }
}
