using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody _rb;
    protected CapsuleCollider _collider;


    [SerializeField] protected CharacterData _characterData;
    public CharacterData CharacterData { get { return this._characterData; } }

    protected void Update()
    {
        if (this.TryGetComponent<NavMeshAgent>(out NavMeshAgent m_Agent))
        {
            if (this.gameObject != PartyManager.Instance.ActivePlayer)
            {
                if (!CombatManager.Instance.IsInCombat)
                {
                    m_Agent.SetDestination(PartyManager.Instance.ActivePlayer.transform.position);
                    m_Agent.stoppingDistance = 2.5f;
                    m_Agent.isStopped = false;
                }

                if (m_Agent.velocity != Vector3.zero)
                    this.GetComponent<Animator>().SetBool("isRunning", true);

                else
                    this.GetComponent<Animator>().SetBool("isRunning", false);
            }

            else
            {
                if (!CombatManager.Instance.IsInCombat)
                    m_Agent.isStopped = true;
            }

            if (CombatManager.Instance.IsInCombat)
            {
                if (m_Agent.velocity != Vector3.zero)
                    this.GetComponent<Animator>().SetBool("isRunning", true);

                else
                    this.GetComponent<Animator>().SetBool("isRunning", false);
            }
        }


        this.AdditionalUpdateFunctions();
    }

    protected virtual void AdditionalUpdateFunctions() { }

    public void Init(CharacterData characterData)
    {
        this._characterData = characterData;
        this.name = characterData.PlayerName;




        //RIGIDBODY INITIALIZATION
        if (this._rb == null)
        {
            this._rb = this.gameObject.AddComponent<Rigidbody>();
            _rb.useGravity = true;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            //DRAG SET TO RANDOM VALUE FOR NOW
            _rb.drag = 0.05f;
        }

        //COLLIDER INITIALIZATION
        if(this._collider == null)
        {
            this._collider = this.gameObject.AddComponent<CapsuleCollider>();
            _collider.isTrigger = false;
        }
    }



    //CHECK IF ALIVE AND NOT STUNNED OR OTHER FACTORS IF EVER
    public bool CanCharacterAct()
    {
        if(this.CharacterData.CurrHealth <= 0)
        {
            return false;
        }

        return true;
    }
    public string GetDetails()
    {
        string details = "\nName: " + this._characterData.PlayerName + " Class: " + this._characterData.CharacterClass;
        details += "\nHP: " + this._characterData.CurrHealth + "/" + this._characterData.MaxHealth;
        details += "\nStr: " + this._characterData.Strength + " Con: " + this._characterData.Constitution;
        details += "\nChr: " + this._characterData.Charisma + " Dex: " + this._characterData.Dexterity;
        details += "\nWis: " + this._characterData.Wisdom + " Int: " + this._characterData.Intelligence;

        return details;
    }

    public void TriggerPlayerDeath()
    {
        if(!this.TryGetComponent<Renderer>(out Renderer _renderer))
        {
            _renderer = this.GetComponentInChildren<Renderer>();
        }
        _renderer.enabled = false;
    }
}