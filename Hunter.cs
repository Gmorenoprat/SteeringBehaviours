using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{

    public List<Transform> waypoints;
    public float speed;
    public float maxEnergy = 50f;
    public float energy = 50f;
    private int _currentWaypoint = 0;

    public bool boidDetected;

    private StateMachine _fsm;

    void Start()
    {
        _fsm = new StateMachine();
        _fsm.AddState(StatesEnum.Rest, new RestState(_fsm,this));
        _fsm.AddState(StatesEnum.Patrol, new PatrolState(_fsm, this));
        _fsm.AddState(StatesEnum.Chase, new ChaseState(_fsm, this));
        _fsm.ChangeState(StatesEnum.Patrol);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }
    public IEnumerator Patrol()
    {
        while(energy > 0) { 
            Vector3 dir = waypoints[_currentWaypoint].position - transform.position;
            _velocity = this.transform.forward * speed;
            transform.forward = dir;
            transform.position += transform.forward * speed * Time.deltaTime;
            
            if (dir.magnitude < 0.1f)
            {
                _currentWaypoint++;
                if (_currentWaypoint > waypoints.Count - 1)
                _currentWaypoint = 0;
            }

            var ray = new Ray(this.transform.position, this.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, forwardDistance))
            {
                if (hit.transform.GetComponent<Boid>())
                {
                    followTarget = hit.transform.GetComponent<Boid>();
                    boidDetected = true;
                }
            }
            yield return null;
        }

        yield return null;
    }
    
    public IEnumerator Chase()
    {

        while(energy > 0) {
            ApplyForce(Pursuit());

            transform.position += _velocity * Time.deltaTime * speed;
            transform.forward = _velocity.normalized;

            if (Vector3.Distance( this.transform.position ,followTarget.transform.position) < 1.2f)
            {
                Destroy(followTarget.gameObject);
                boidDetected = false;
            }
            yield return null;
        }

        yield return null;
    }

}

