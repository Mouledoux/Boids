using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EvenBetterBoid : MonoBehaviour
{
    public List<GameObject> flock;

    public Slider cohSlide;
    public Slider sepSlide;
    public Slider alignSlide;

    public Button fire;

    public Toggle free;

    public GameObject boid;
    public GameObject predator;

    //Replaced by slider
    //public float m_cohesionMod;     // strength of cohestion
    //Replaced by slider
    //public float m_alignmentMod;    // addition to current velocity
    public float m_seperationMod;     // strength of seperation

    //Replaced by slider
    //public float m_sThreshold;      // threshold of seperation

    public float limit;

    //The 5 rules
    Vector3 tendency;
    Vector3 box;
    Vector3 cohesion;
    Vector3 alignment;
    Vector3 seperation;

    // Use this for initialization
    void Start()
    {
        AddBoids(100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject bird in flock)
        {
            cohesion = Cohesion(bird);
            bird.GetComponent<Bird>().velocity += cohesion * cohSlide.value;

            seperation = Seperation(bird);
            bird.GetComponent<Bird>().velocity += seperation * m_seperationMod;

            alignment = Alignment(bird);
            bird.GetComponent<Bird>().velocity += alignment * alignSlide.value;

            box = BoundingBox(bird);
            bird.GetComponent<Bird>().velocity += box * cohSlide.value;

            tendency = TendencyTo(bird);
            bird.GetComponent<Bird>().velocity += tendency * Random.Range(0.001f, 0.01f);

            //Virus(bird);

            VelocityLimit(bird);

            bird.transform.position += bird.GetComponent<Bird>().velocity;
            bird.transform.up = bird.GetComponent<Bird>().velocity.normalized;
            //bird.transform.rotation = Quaternion.LookRotation(bird.GetComponent<Bird>().velocity);
        }
    }

    void AddBoids(int numBoids, int numPred)
    {
        for (int i = 0; i < numBoids; i++)
        {
            GameObject b = Instantiate(boid) as GameObject;

            b.transform.parent = gameObject.transform; // parents the new boid to the manager           

            b.transform.localPosition = new Vector3(Random.Range(-20.0f, 20.0f),    // random x position
                                                    Random.Range(-20.0f, 20.0f),    // random y positoin
                                                    Random.Range(-20.0f, 20.0f));   // random z position

            b.transform.localPosition /= transform.localScale.x;

            b.GetComponent<Bird>().velocity = new Vector3(Random.Range(-10.0f, 10.0f),
                                                          Random.Range(-10.0f, 10.0f),
                                                          Random.Range(-10.0f, 10.0f));

            flock.Add(b);
        }

        for (int i = 0; i < numPred; i++)
        {
            GameObject p = Instantiate(predator) as GameObject;

            p.transform.parent = gameObject.transform; // parents the new boid to the manager           

            p.transform.localPosition = new Vector3(Random.Range(-20.0f, 20.0f),    // random x position
                                                    Random.Range(-20.0f, 20.0f),    // random y positoin
                                                    Random.Range(-20.0f, 20.0f));   // random z position

            p.transform.localPosition /= transform.localScale.x;

            p.GetComponent<Bird>().velocity = new Vector3(Random.Range(-10.0f, 10.0f),
                                                          Random.Range(-10.0f, 10.0f),
                                                          Random.Range(-10.0f, 10.0f));

            flock.Add(p);
        }

    }

    void VelocityLimit(GameObject b)
    {
        if (b.GetComponent<Bird>().velocity.magnitude > 1.1)
        {
            b.GetComponent<Bird>().velocity -= b.GetComponent<Bird>().velocity * 0.1f;
        }
    }

    //No longer in use
    void Virus(GameObject bird)
    {
        foreach (GameObject b in flock)
        {
            if (b != bird && b.GetComponent<Bird>().isPredator && !bird.GetComponent<Bird>().isPredator)
            {
                if (Vec3Dist(b.transform.position, bird.transform.position) < 1)
                {
                    bird.GetComponent<Bird>().isPredator = true;
                    bird.GetComponent<Bird>().eSpecies = Bird.Species.ePredator;
                    bird.transform.localScale = b.transform.localScale;
                    bird.GetComponent<Renderer>().material = b.GetComponent<Renderer>().material;
                }
            }
        }
    }

    Vector3 Cohesion(GameObject bird)
    {

        if (transform.position == Vector3.zero)
        {
            bird.transform.parent = transform;
        }

        if (!free.isOn)
        {
            bird.transform.parent = null;
            return (transform.position - bird.transform.position) / 100;
        }

        else
        {
            Vector3 COM = Vector3.zero; // Center of mass
            int n = 0;

            if (bird.GetComponent<Bird>().isPredator)
            {
                if (!bird.GetComponent<Bird>().target || bird.GetComponent<Bird>().target.GetComponent<Bird>().isPredator)
                {
                    bird.GetComponent<Bird>().target = flock[Random.Range(0, flock.Count)];
                }
                return (bird.GetComponent<Bird>().target.transform.position - bird.transform.position) / 100;
            }

            foreach (GameObject b in flock)
            {
                if (b != bird &&
                    Vec3Dist(b.transform.position, bird.transform.position) < sepSlide.value * 2)   // If the birds are not the same
                {
                    COM += b.transform.position;                                                    // Add the position to COM
                    n++;
                }
            }

            if (n == 0) return Vector3.zero; 

            COM /= (n);                                   // Divid COM by n-1 to get the average

            return (COM - bird.transform.position) / 100; // Returns the birds current position 1% closer to COM
        }
    }

    Vector3 Seperation(GameObject bird)
    {
        Vector3 pos = Vector3.zero; // New position after resistance

        foreach (GameObject b in flock)
        {
            if (b != bird)                                                                      // If the birds are not the same
            {
                if (Vec3Dist(b.transform.position, bird.transform.position) < sepSlide.value)   // If the birds are too close
                {
                    pos -= (b.transform.position - bird.transform.position);                    // Avoid it
                }
            }
        }

        return pos / 10;
    }

    Vector3 Alignment(GameObject bird)
    {
        Vector3 align = Vector3.zero;
        int n = 0;

        foreach (GameObject b in flock)
        {
            if (b != bird &&                                                                    // If the birds are not the same
                Vec3Dist(b.transform.position, bird.transform.position) < sepSlide.value * 2)   // If they are close enough
            {
                align += b.GetComponent<Bird>().velocity;
                n++;
            }
        }

        if (n == 0) return Vector3.zero;

        align /= n;

        return align;
    }

    Vector3 BoundingBox(GameObject bird)
    {
        Vector3 bound = Vector3.zero;
        Vector3 b = bird.transform.position;

        if (free.isOn)
        {

            //Limits movement in the + direction
            if (b.x > transform.position.x + limit)
            {
                bound.x = transform.position.x - b.x;
            }
            if (b.y > transform.position.y + limit)
            {
                bound.y = transform.position.y - b.y;
            }
            if (b.z > transform.position.z + limit)
            {
                bound.z = transform.position.z - b.z;
            }

            //Limits movement in the - direction
            if (b.x < transform.position.x - limit)
            {
                bound.x = transform.position.x - b.x;
            }
            if (b.y < transform.position.y - limit)
            {
                bound.y = transform.position.y - b.y;
            }
            if (b.z < transform.position.z - limit)
            {
                bound.z = transform.position.z - b.z;
            }
        }

        return bound / 100;
    }

    Vector3 TendencyTo(GameObject bird)
    {
        if (free.isOn)
        {
            return (transform.position - bird.transform.position) / 100;
        }

        else return Vector3.zero;
    }

    public float Vec3Dist(Vector3 a, Vector3 b)
    {
        return (Mathf.Sqrt((a.x - b.x) * (a.x - b.x) +
                           (a.y - b.y) * (a.y - b.y) +
                           (a.z - b.z) * (a.z - b.z)));
    }
}