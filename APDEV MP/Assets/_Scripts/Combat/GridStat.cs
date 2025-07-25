using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GridStat : MonoBehaviour
{
    [Header("Locators")]
    [SerializeField] private int m_xLoc = 0;
    [SerializeField] private int m_yLoc = 0;

    [Header("Materials")]
    [SerializeField] private Material m_Passable;
    [SerializeField] private Material m_Targetable;
    [SerializeField] private Material m_Pathable;

    private bool m_IsPassable = true;
    private bool m_IsTargetable = false;
    private bool m_IsPathable = false;

    private bool m_IsProtruding = false;

    [SerializeField] private bool m_HasHostileUnit = false;
    [SerializeField] private bool m_HasAllyUnit = false;

    [SerializeField] private GameObject m_UnitInTile = null;

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject == PartyManager.Instance.ActivePlayer)
        //{
        //    this.m_UnitInTile = other.gameObject;
        //    CombatManager.Instance.CurrentUnitGrid = this.gameObject;
        //    return;
        //}

        //else if (other.gameObject.CompareTag("Hostile"))
        //{
        //    this.m_HasHostileUnit = true;
        //    this.m_UnitInTile = other.gameObject;
        //}

        //else if (other.gameObject.CompareTag("Ally"))
        //{
        //    this.m_HasAllyUnit = true;
        //    this.m_UnitInTile = other.gameObject;
        //}
        this.UpdateTileEntities(other.gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstruction"))
            this.m_IsPassable = false;
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == PartyManager.Instance.ActivePlayer)
    //        this.m_UnitInTile = null;

    //    if (other.gameObject.CompareTag("Hostile"))
    //    {
    //        this.m_HasHostileUnit = false;
    //        this.m_UnitInTile = null;
    //    }

    //    if (other.gameObject.CompareTag("Ally"))
    //    {
    //        this.m_HasAllyUnit = false;
    //        this.m_UnitInTile = null;
    //    }
    //}

    private async void UpdateTileEntities(GameObject obj)
    {
        float end = Time.time + 3;

        while (end > Time.time)
        {
            if (obj == null)
                return;

            if (obj == PartyManager.Instance.ActivePlayer)
            {
                this.UnitInTile = obj;
                CombatManager.Instance.CurrentUnitGrid = this.gameObject;
            }

            if (obj.CompareTag("Ally"))
            {
                this.UnitInTile = obj;
                this.m_HasAllyUnit = true;
            }

            else if (obj.CompareTag("Hostile"))
            {
                this.UnitInTile = obj;
                this.m_HasHostileUnit = true;
            }


            await Task.Yield();
        }

        this.UnitInTile = null;
        this.HasAllyUnit = false;
        this.HasHostileUnit = false;
    }

    public void ChangeTileState(int state)
    {
        Vector3 m_Leveller = new Vector3(0, 0.05f, 0);

        switch (state)
        {
            case 0:
                this.GetComponent<LineRenderer>().material = this.m_Passable;
                this.m_IsPassable = true;
                this.m_IsTargetable = false;
                this.m_IsPathable = false;

                if (this.m_IsProtruding)
                {
                    this.transform.position -= m_Leveller;
                    this.m_IsProtruding = false;
                }
                break;

            case 1:
                this.GetComponent<LineRenderer>().material = this.m_Targetable;
                this.m_IsTargetable = true;

                if (!this.m_IsProtruding)
                {
                    this.transform.position += m_Leveller;
                    this.m_IsProtruding = true;
                }

                break;

            case 2:
                this.GetComponent<LineRenderer>().material = this.m_Pathable;
                this.m_IsPassable = true;
                this.m_IsTargetable = false;
                this.m_IsPathable = true;

                if (!this.m_IsProtruding)
                {
                    this.transform.position += m_Leveller;
                    this.m_IsProtruding = true;
                }

                break;

            default:
                break;
        }
    }

    public int xLoc { get { return this.m_xLoc; } set { this.m_xLoc = value; } }
    public int yLoc { get { return this.m_yLoc; } set { this.m_yLoc = value; } }
    public bool IsPassable { get { return this.m_IsPassable; } }
    public bool IsTargetable { get { return this.m_IsTargetable; } }
    public bool IsPathable { get { return this.m_IsPathable; } }
    public bool HasHostileUnit { get { return this.m_HasHostileUnit; } set { this.m_HasHostileUnit = value; } }
    public bool HasAllyUnit { get { return this.m_HasAllyUnit; } set { this.m_HasAllyUnit = value; } }  
    public GameObject UnitInTile { get { return this.m_UnitInTile; } set { this.m_UnitInTile = value; } }
}
