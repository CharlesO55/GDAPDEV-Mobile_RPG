using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStat : MonoBehaviour
{
    [Header("Locators")]
    [SerializeField] private int m_xLoc = 0;
    [SerializeField] private int m_yLoc = 0;

    [Header("Materials")]
    [SerializeField] private Material m_Passable;
    [SerializeField] private Material m_Impassable;
    [SerializeField] private Material m_Pathable;

    private bool m_IsPlayerWithin = false;
    private bool m_IsPassable = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == PartyManager.Instance.ActivePlayer)
        {
            this.m_IsPlayerWithin = true;
            ChangeTileState(2);
            return;
        }


        this.ChangeTileState(1);
    }

    public void ChangeTileState(int state)
    {
        Vector3 m_Leveller = new Vector3(0, 0.1f, 0);

        switch (state)
        {
            case 0:
                this.GetComponent<LineRenderer>().material = this.m_Passable;
                this.m_IsPassable = true;
                break;

            case 1:
                this.GetComponent<LineRenderer>().material = this.m_Impassable;
                this.m_IsPassable = false;
                break;

            case 2:
                this.GetComponent<LineRenderer>().material = this.m_Pathable;
                this.m_IsPassable = true;
                break;

            default:
                break;
        }
    }

    public int xLoc { get { return this.m_xLoc; } set { this.m_xLoc = value; } }
    public int yLoc { get { return this.m_yLoc; } set { this.m_yLoc = value; } }
    public bool IsPlayerWithin { get { return this.m_IsPlayerWithin; } }
    public bool IsPassable { get { return this.m_IsPassable; } }
}
