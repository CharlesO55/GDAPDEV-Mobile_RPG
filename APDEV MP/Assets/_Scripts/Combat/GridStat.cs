using System.Collections;
using System.Collections.Generic;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == PartyManager.Instance.ActivePlayer)
        {
            CombatManager.Instance.CurrentUnitGrid = this.gameObject;
            return;
        }

        if (!other.gameObject.CompareTag("CombatTile"))
            this.m_IsPassable = false;
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
    public bool IsPathable { get { return this.m_IsPathable; } }
}
