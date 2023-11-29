using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private JoystickScript _movementJoystick;
    [SerializeField] private float _movementSpeed = 8;


    [SerializeField] private AudioSource _walkSoundEffect;

    private GameObject _currActivePlayerRef;
    private NavMeshAgent _navMeshAgent;


    void OnEnable()
    {
        GameObject moveJoystick = GameObject.Find("Joystick");

        this._movementJoystick = moveJoystick.GetComponent<JoystickScript>();
    }

    private void Start()
    {
        PartyManager.Instance.OnSwitchPlayerEvent += UpdatePlayerRef;
    }

    private void OnDestroy()
    {
        PartyManager.Instance.OnSwitchPlayerEvent -= UpdatePlayerRef;
    }


    void FixedUpdate()
    {
        if(this._movementJoystick != null && this._currActivePlayerRef != null)
        {
            this.MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector2 inputs = _movementJoystick.GetJoystickAxis(true);
        if (inputs.magnitude == 0)
        {
            if (_walkSoundEffect.isPlaying)
            {
                this._walkSoundEffect.Stop();
            }
            return;
        }

        Vector3 move = (inputs.x * Camera.main.transform.right) + (inputs.y * Camera.main.transform.forward);
        move *= Time.deltaTime * _movementSpeed;
        if (move != Vector3.zero && !_walkSoundEffect.isPlaying)
        {
            this._walkSoundEffect.Play();
        }
        

        //Orient & Move
        this._currActivePlayerRef.transform.LookAt(move + this._currActivePlayerRef.transform.position);
        _navMeshAgent.Move(move);
    }

    private void UpdatePlayerRef(object sender, GameObject activePlayer)
    {
        Debug.Log("Updated active player");

        //get move componnet //navmesh for this case
        this._currActivePlayerRef = activePlayer;
        this._navMeshAgent = activePlayer.GetComponent<NavMeshAgent>();
    }
}