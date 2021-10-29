using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    
    public Vector3 _velocity;
    public float maxSpeed;
    public float maxForce;

    public float arriveDistance;

    public Agent followTarget;
    public Agent evadeTarget;
    public Transform arriveTarget;

    public float futureTime;
    public GameObject futurePosObject;

    public float SeekWeight;
    public float FleeWeight;
    public float forwardDistance;
    

    // Update is called once per frame
    //void Update()
    //{
    //    float fw = 0;
    //    if ((target.transform.position - transform.position).magnitude < 2)
    //    {
    //        fw = FleeWeight;
    //    }
    //    //
    //    ApplyForce(Seek(target2) * SeekWeight);
    //    ApplyForce(Flee(target) * fw);
    //    //}
    //    // else 

    //    transform.position += _velocity * Time.deltaTime;
    //    transform.forward = _velocity.normalized;

    //}

    protected Vector3 Seek(GameObject tar)
    {
        Vector3 desired = tar.transform.position - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        //ApplyForce(steering);
        return steering;
    }

    protected Vector3 Flee(GameObject tar)
    {
        Vector3 desired = tar.transform.position - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        desired = -desired;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    protected Vector3 Arrive()
    {
        Vector3 desired;
        if (arriveTarget != null) desired = arriveTarget.position - transform.position;
        else { desired = transform.position; }

        if (desired.magnitude < arriveDistance)
        {

            float speed = Map(desired.magnitude, 0, arriveDistance, 0, maxSpeed);
            desired.Normalize();
            desired *= speed;
        }
        else
        {
            desired.Normalize();
            desired *= maxSpeed;
        }

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
        
    }


    protected Vector3 Pursuit()
    {
        //basado en la distancia entre target y yo, cambio dist por futureTime
        // Vector3 dist = pursuitTarget.transform.position - transform.position;

        Vector3 futurePos = followTarget.transform.position + (followTarget.GetVelocity() * futureTime * Time.deltaTime);


        //futurePosObject.transform.position = futurePos;

        Vector3 desired = futurePos - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;

    }

    protected Vector3 Evade()
    {

        Vector3 futurePos = evadeTarget.transform.position + (evadeTarget.GetVelocity() * futureTime * Time.deltaTime);

        Vector3 desired = futurePos - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        desired *= -1;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;

    }



    //protected Vector3 ObstacleAvoidance()
    //{
    //    Vector3 desired = new Vector3();

    //    if (Physics.Raycast(transform.position, transform.forward, forwardDistance))
    //    {
    //        desired = transform.position + transform.right * 10;
    //        desired.Normalize();
    //        desired *= maxSpeed;
    //    }
    //    else
    //    {
    //        return desired;
    //    }

    //    Vector3 steering = desired - _velocity;
    //    steering = Vector3.ClampMagnitude(steering, maxForce);

    //    return steering;
    //}


    protected void ApplyForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, maxSpeed);
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }



    float Map(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (from - toMin) / (fromMax - fromMin) * (toMax - toMin) + fromMin;
    }
}
