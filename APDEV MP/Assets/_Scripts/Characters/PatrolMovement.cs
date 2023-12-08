using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolMovement : MonoBehaviour
{
    [SerializeField] List<Transform> _wayPoints = new();
    private NavMeshAgent _agent;
    int _currWaypoint;

    private void Start()
    {
        this._agent = GetComponent<NavMeshAgent>();
        

        _currWaypoint = 0;
        _agent.destination = _wayPoints[_currWaypoint].position;

    }

    private void LateUpdate()
    {
        if(_agent.remainingDistance <= 1)
        {
            _currWaypoint++;
            if(_currWaypoint >= _wayPoints.Count)
            {
                _currWaypoint = 0;
            }

            _agent.destination = _wayPoints[_currWaypoint].position;
        }
    }
}
