using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveWithSlider : MonoBehaviour
{
    public Toggle free;
    public Slider X;
    public Slider Y;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (free.isOn)
        {
            transform.position = Vector3.zero;
            X.value = Y.value = 0;
        }
        else
        {
            transform.position = new Vector3(X.value, Y.value, 0);
        }
	}
}
