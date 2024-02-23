using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderController : MonoBehaviour
{
    public GameObject player; // Player object
    public AudioSource cylinderSource; // Cylinder audio source
    public Material onMaterial; // Material when cylider is on
    public Material offMaterial; // Material when cylinder is off
    public float maxDistance = 2f; // Maximum distance the player can be from the cylinder to turn it on
 

    private Renderer objectRenderer; // Renderer component of the cylinder object
 
    

    // Start is called before the first frame update
    void Start()
    {
 

    }

    // Update is called once per frame
    void Update()
    {


    }
}