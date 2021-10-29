using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : Agent
{
    private StateMachine _fsm;
    public float viewDistance;
    

    public float separationWeight;
    public float cohesionWeight;
    public float alignWeight;

    public List<Boid> _nearbyBoids = new List<Boid>();
    public bool nearbyEnemy = false;
    public bool nearbyFood = false;

    void Start()
    {

        _fsm = new StateMachine();
        _fsm.AddState(StatesEnum.Flocking, new FlockingState(_fsm, this));
        _fsm.AddState(StatesEnum.Evade, new EvadeState(_fsm, this));
        _fsm.AddState(StatesEnum.Arrive, new ArriveState(_fsm, this));
        _fsm.ChangeState(StatesEnum.Flocking);

        Vector3 randomForce = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        randomForce.Normalize();
        randomForce *= maxSpeed;

        randomForce = Vector3.ClampMagnitude(randomForce, maxForce);

        ApplyForce(randomForce);
    }

    void Update()
    {
        CheckBounds();
        CheckSourround();
        UpdateValues();

        _fsm.OnUpdate();

    }

    void UpdateValues()
    {
        separationWeight = GameManager.instance.globalSeparationWeight;
        cohesionWeight = GameManager.instance.globalCohesionWeight;
        alignWeight = GameManager.instance.globalAlignWeight;
        maxForce = GameManager.instance.globalMaxForce;
        maxSpeed = GameManager.instance.globalMaxSpeed;
        viewDistance = GameManager.instance.globalViewDistance;

    }

    #region FLOCKING
    Vector3 Cohesion()
    {
        Vector3 desired = new Vector3();
        int nearbyBoids = 0;
        foreach (var item in _nearbyBoids)
        {
            if (item!= null && item != this && Vector3.Distance(item.transform.position, transform.position) < viewDistance)
            {
                nearbyBoids++;
                desired += item.transform.position;
            }

        }
        desired.y = 0;
        if (nearbyBoids == 0) return Vector3.zero;

        desired /= nearbyBoids;
        desired = desired - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        // Vector3 steering = Seek(desired);
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    Vector3 Separation()
    {
        Vector3 desired = new Vector3();

        int boidsNearby = 0;
        foreach (var boid in _nearbyBoids)
        {
            if (boid != null)
            {
                Vector3 distance = (boid.transform.position - transform.position);

                if (boid != this && distance.magnitude < viewDistance)
                {
                    desired.x += distance.x;
                    desired.z += distance.z;
                    boidsNearby++;
                }
            }
        }

        if (boidsNearby == 0) return Vector3.zero;

        desired /= boidsNearby;
        desired.Normalize();
        desired *= maxSpeed;
        desired *= -1;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        //steering.y = 0;
        return steering;
    }

    Vector3 Align()
    {
        Vector3 desired = new Vector3();

        int boidsNearby = 0;
        foreach (var item in _nearbyBoids)
        {
            if (item != this && item != null)
            {
                Vector3 dist = item.transform.position - transform.position;
                if (Vector3.Magnitude(dist) < viewDistance)
                {
                    boidsNearby++;
                    desired.x += item._velocity.x;
                    desired.z += item._velocity.z;
                }
            }
        }
        if (boidsNearby == 0) return Vector3.zero;
        desired = desired / boidsNearby;
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 steering = Vector3.ClampMagnitude(desired - _velocity, maxForce);

        return steering;
    }

    #endregion

    public IEnumerator Flocking()
    {
        while (!nearbyEnemy || !nearbyFood)
        {
         
            ApplyForce(Cohesion() * cohesionWeight + Separation() * separationWeight + Align() * alignWeight);

            transform.position += _velocity * Time.deltaTime;
            transform.forward = _velocity.normalized;

            yield return null;
        }

        yield return null;
    }
    public IEnumerator EvadeCoroutine()
    {
        while (nearbyEnemy)
        {
            ApplyForce(Evade());

            transform.position += _velocity * Time.deltaTime;
            transform.forward = _velocity.normalized;

            if (Vector3.Distance(evadeTarget.transform.position, this.transform.position) < viewDistance*3)
            {
                nearbyEnemy = false;
            };

            yield return null;
        }

        yield return null;
    }
    public IEnumerator ArriveCoroutine()
    {
        while (!nearbyEnemy)
        {
            ApplyForce(Arrive());

            transform.position += _velocity * Time.deltaTime;
            transform.forward = _velocity.normalized;

            if (arriveTarget == null || Vector3.Distance(arriveTarget.transform.position, this.transform.position) < 0.1f )
            {
                if(arriveTarget != null) Destroy(arriveTarget.transform.gameObject);
                nearbyFood = false;
            };

            yield return null;
        }

        yield return null;
    }
    void CheckBounds()
    {
        float leftBound = GameManager.instance.screenLeftBound;
        float rightBound = GameManager.instance.screenRightBound;
        float upBound = GameManager.instance.screenUpBound;
        float downBound = GameManager.instance.screenDownBound;
        if (transform.position.x > rightBound) transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        if (transform.position.x < leftBound) transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
        if (transform.position.z < downBound) transform.position = new Vector3(transform.position.x, transform.position.y, upBound);
        if (transform.position.z > upBound) transform.position = new Vector3(transform.position.x, transform.position.y, downBound);
    }


    void CheckSourround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, viewDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Boid>())
            {
                Boid boid = hitCollider.gameObject.GetComponent<Boid>();
                if (!_nearbyBoids.Contains(boid)) { _nearbyBoids.Add(boid); }
            }
            else if (hitCollider.gameObject.GetComponent<Hunter>())
            {
                evadeTarget = hitCollider.gameObject.GetComponent<Hunter>();
                nearbyEnemy = true;
            }
            else if (hitCollider.gameObject.name == "Food")
            {
                nearbyFood = true;
                arriveTarget = hitCollider.transform;
            }
        }
    }


    //void OnDrawGizmos()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(transform.position, viewDistance);
    //}
}
