using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [SerializeField] private GameObject m_CombatGrid;
    [SerializeField] private CombatGrid m_CombatGridScript;
    [SerializeField] private bool m_IsInCombat = false;

    private bool m_FoundMoveRange = false;
    private bool m_FoundAttackRange = false;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(this.gameObject);

        GestureManager.Instance.OnTapDelegate += this.CheckMoveablePath;
    }

    private void CheckMoveablePath(object sender, TapEventArgs args)
    {
        if (args.ObjHit != null && args.ObjHit.CompareTag("CombatTile"))
        {
            if (args.ObjHit.GetComponent<GridStat>().IsPathable)
                PartyManager.Instance.ActivePlayer.GetComponent<NavMeshAgent>().SetDestination(args.ObjHit.transform.position);

            else
                Debug.Log("Area is not pathable");
        }
    }

    public void CheckUnitMoveRange(GridStat grid)
    {
        if (grid.IsPlayerWithin)
            this.m_CombatGridScript.CheckMovementRange(grid.GetComponent<GridStat>().xLoc, 
                                                       grid.GetComponent<GridStat>().yLoc, 
                                                       this.CheckUnitMovementSpeed(PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CharacterClass));
    }

    private int CheckUnitMovementSpeed(EnumUnitClass jobclass)
    {
        switch(jobclass)
        {
            case EnumUnitClass.FIGHTER:
            case EnumUnitClass.MAGE:
                return 1;

            case EnumUnitClass.PALADIN:
            case EnumUnitClass.ROGUE:
                return 2;

            default:
                return 0;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        this.m_CombatGrid.SetActive(this.m_IsInCombat);
    }

    public bool IsInCombat { get { return this.m_IsInCombat; } set { this.m_IsInCombat = value; } }
    public bool FoundMoveRange { get { return this.m_FoundMoveRange;} set { this.m_FoundMoveRange = value; } }
    public bool FoundAttackRange { get { return this.m_FoundAttackRange; } set { this.m_FoundAttackRange = value; } }
}
