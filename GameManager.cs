using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header("Screen values")]
    public float screenUpBound = 20;
    public float screenDownBound = -20;
    public float screenLeftBound = -20;
    public float screenRightBound = 20;


    //boids stuff
    [Header("Boid Values")]
    public float globalViewDistance;
    public float globalSeparationWeight;
    public float globalCohesionWeight;
    public float globalAlignWeight;
    public float globalMaxSpeed;
    [Range(0.01f, 1f)]
    public float globalMaxForce;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

}
