using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private JoystickScript _movementJoystick;
    [SerializeField] private float _movementSpeed = 1000;


    [SerializeField] private AudioSource _walkSoundEffect;
    private GameObject _activePlayerRef;
    private NavMeshAgent _navMeshAgent;



    void OnEnable()
    {
        GameObject moveJoystick = GameObject.Find("Joystick");

        this._movementJoystick = moveJoystick.GetComponent<JoystickScript>();

        //Helps to keep track of when player switch happens
        PartyManager.Instance.OnSwitchPlayerEvent += UpdatePlayerRef;
    }

    private void OnDisable()
    {
        PartyManager.Instance.OnSwitchPlayerEvent -= UpdatePlayerRef;
    }


    void FixedUpdate()
    {
        //Move player
        Vector2 inputs = _movementJoystick.GetJoystickAxis(true);
        Vector3 move = (inputs.x * Camera.main.transform.right) + (inputs.y * Camera.main.transform.forward);
        move *= Time.deltaTime * _movementSpeed;
      



        //Orient
        _activePlayerRef.transform.LookAt(move + _activePlayerRef.transform.position); 

        this._navMeshAgent.velocity = move;
        if (this._navMeshAgent.velocity != Vector3.zero)
        {
            this._walkSoundEffect.Play();
        }
    }

    public void UpdatePlayerRef(object sender, GameObject activePlayer)
    {
        Debug.Log("Updated active player");

        this._activePlayerRef = activePlayer;

        //get move componnet //navmesh for this case
        this._navMeshAgent = activePlayer.GetComponent<NavMeshAgent>();
    }
}