using System.Collections;
using System.Collections.Generic;
using UnityEngine;
       
public class EnemyBehavior : MonoBehaviour 
{
    private bool _isChasing = false;

    public Transform PatrolRoute;
    public List<Transform> Locations;
    private int _locationIndex = 0;
    private UnityEngine.AI.NavMeshAgent _agent;

    public Transform Player;

    private int _lives = 5;
    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy defeated!");
            }
        }
    }
    
    void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Player = GameObject.Find("Player").transform;

        InitalizePatrolRoute();
        MoveToNextPatrolLocation();
    }
    void InitalizePatrolRoute()
    {
        foreach(Transform child in PatrolRoute)
        {
            Locations.Add(child);
        }
    }

    void Update()
    {
        if (!_agent.isOnNavMesh)
            return;

        if (_isChasing)
        {
            _agent.destination = Player.position;
            return;
        }
        else
        {
            if (_agent.hasPath && _agent.remainingDistance < 0.2f)
            {
                MoveToNextPatrolLocation();
            }
        }


    //     if(_agent.remainingDistance < 0.2f && !_agent.pathPending)
    //     {
    //         MoveToNextPatrolLocation();
    //     }
}

    void MoveToNextPatrolLocation()
    {
        if(Locations.Count == 0)
            return;

        _agent.destination = Locations[_locationIndex].position;
        _locationIndex = (_locationIndex + 1) % Locations.Count;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            _isChasing = true;
            _agent.destination = Player.position;
            Debug.Log("Player detected - attack!");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            _isChasing = false;
            Debug.Log("Player out of range, resume patrol");
            MoveToNextPatrolLocation();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Enemy hit! Remaining Lives: " + EnemyLives);
        }
    }
} 