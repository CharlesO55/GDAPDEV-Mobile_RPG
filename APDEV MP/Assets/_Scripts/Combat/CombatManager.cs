using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header ("Combat Grid")]
    [SerializeField] private GameObject m_CombatGrid;
    [SerializeField] private CombatGrid m_CombatGridScript;

    [Header ("Combat Data")]
    [SerializeField] private GameObject m_CurrentUnitGrid = null;
    [SerializeField] private bool m_IsInCombat = false;

    [Space]
    [SerializeField] private List<GameObject> m_UnitList = new List<GameObject>();
    private int m_CurrentTurnIndex = -1;

    [SerializeField] private bool m_IsViewingMoveRange = false;
    [SerializeField] private bool m_IsViewingAttackRange = false;

    private int m_AcitveUnitMoves = 0;

    [Header("Debug Settings")]
    [SerializeField] int player = 1;
    [SerializeField] private bool mswitch = false;

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

    private int CheckUnitAttackRange(EnumUnitClass jobclass)
    {
        switch (jobclass)
        {
            case EnumUnitClass.FIGHTER:
            case EnumUnitClass.PALADIN:
                return 1;

            case EnumUnitClass.MAGE:
            case EnumUnitClass.ROGUE:
                return 2;

            default:
                return 0;
        }
    } 

    private void SwitchNextActiveUnit()
    {
        this.m_CurrentTurnIndex++;

        if (this.m_CurrentTurnIndex > this.m_UnitList.Count)
            this.m_CurrentTurnIndex = 0;
    }

    public void BeginCombat()
    {
        this.IsInCombat = true;

        foreach (GameObject unit in this.m_UnitList)
        {
            int m_UnitInitiative = Random.Range(1, 21);
            int m_DexModifier = unit.GetComponent<CharacterScript>().CharacterData.DEXMod;

            unit.GetComponent<CharacterScript>().CharacterData.Initiative = m_UnitInitiative + m_DexModifier;
        }
    }

    public void EndCombat()
    {
        this.IsInCombat = false;
        this.m_CurrentTurnIndex = -1;

        this.m_UnitList.Clear();

        foreach (GameObject unit in this.m_UnitList)
            unit.GetComponent<CharacterScript>().CharacterData.Initiative = 0;
    }

    //Adjust accordingly if enemies will not have CharacterScriptComponent
    private void RetrieveUnits()
    {
        CharacterScript[] m_Units = FindObjectsOfType<CharacterScript>();

        foreach (CharacterScript unit in m_Units)
            this.m_UnitList.Add(unit.gameObject);

        foreach (GameObject unit in this.m_UnitList)
        {
            int m_UnitInitiative = Random.Range(1, 21);
            int m_DexModifier = unit.GetComponent<CharacterScript>().CharacterData.DEXMod;

            unit.GetComponent<CharacterScript>().CharacterData.Initiative = m_UnitInitiative + m_DexModifier;
        }

        this.m_UnitList.Sort((a, b) => b.GetComponent<CharacterScript>().CharacterData.Initiative.CompareTo(a.GetComponent<CharacterScript>().CharacterData.Initiative));
    }

    // Update is called once per frame
    private void Update()
    {
        this.m_CombatGrid.SetActive(this.m_IsInCombat);

        if (this.IsInCombat)
        {
            if (this.m_CurrentUnitGrid != null)
            {
                if (this.m_UnitList.Count == 0)
                    this.RetrieveUnits();

                GridStat m_GridStat = this.m_CurrentUnitGrid.GetComponent<GridStat>();

                if (this.m_IsViewingMoveRange)
                {
                    this.m_IsViewingAttackRange = false;

                    int m_MoveRange = this.CheckUnitMovementSpeed(PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CharacterClass);
                    this.m_CombatGridScript.CheckMovementRange(m_GridStat.xLoc, m_GridStat.yLoc, m_MoveRange);
                }

                if (this.m_IsViewingAttackRange)
                {
                    this.m_IsViewingMoveRange = false;

                    int m_AttackRange = this.CheckUnitAttackRange(PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CharacterClass);
                    this.m_CombatGridScript.CheckAttackRange(m_GridStat.xLoc, m_GridStat.yLoc, m_AttackRange);
                }
            }
        }

        if (this.mswitch)
        {
            PartyManager.Instance.SwitchActiveCharacter(this.player);
            this.mswitch = false;
        }
    }

    public GameObject CurrentUnitGrid { get { return this.m_CurrentUnitGrid; }  set { this.m_CurrentUnitGrid = value; } }
    public bool IsInCombat { get { return this.m_IsInCombat; } set { this.m_IsInCombat = value; } }
    public int ActiveUnitMoves { get { return this.m_AcitveUnitMoves; } set { this.m_AcitveUnitMoves = value; } }
    public bool IsViewingMoveRange { get { return this.m_IsViewingMoveRange; } set { this.m_IsViewingMoveRange = value; } }
    public bool IsViewingAttackRange { get { return this.m_IsViewingAttackRange; } set { this.m_IsViewingAttackRange = value; } }
}
