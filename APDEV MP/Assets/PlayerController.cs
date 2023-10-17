using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] JoystickScript _movementJoystick;
    [SerializeField] private float _movementSpeed = 5;


    private GameObject _activePlayerRef;

    // Start is called before the first frame update
    void Start()
    {
        GameObject moveJoystick = GameObject.Find("Joystick");

        this._movementJoystick = moveJoystick.GetComponent<JoystickScript>();

        //_activePlayerRef = PartyManager.Instance.ActivePlayer;
        PartyManager.Instance.OnSwitchPlayerEvent += UpdatePlayerRef;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move player
        Vector2 inputs = _movementJoystick.GetJoystickAxis(true);
        Vector3 move = new Vector3(inputs.x, 0, inputs.y) * Time.deltaTime * _movementSpeed;


        Debug.Log(move);
        _activePlayerRef.transform.position += move/*move * Time.deltaTime * _movementSpeed*/;
    }

    void UpdatePlayerRef(object sender, GameObject activePlayer)
    {
        this._activePlayerRef = activePlayer;

        //get move componnet
    }
}