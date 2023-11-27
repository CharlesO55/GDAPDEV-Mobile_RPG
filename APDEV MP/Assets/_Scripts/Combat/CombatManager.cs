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
    private bool m_FoundMoveRange = false;
    private bool m_FoundAttackRange = false;

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

    private void SelectNextActiveUnit()
    {

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
            if (!this.m_FoundMoveRange)
            {
                int m_Range = this.CheckUnitMovementSpeed(PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CharacterClass);
                
                if (this.m_CurrentUnitGrid != null)
                {
                    this.RetrieveUnits();
                    GridStat m_Stat = this.m_CurrentUnitGrid.GetComponent<GridStat>();

                    this.m_FoundMoveRange = true;
                    this.m_CombatGridScript.CheckMovementRange(m_Stat.xLoc, m_Stat.yLoc, m_Range);
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
    public bool FoundMoveRange { get { return this.m_FoundMoveRange;} set { this.m_FoundMoveRange = value; } }
    public bool FoundAttackRange { get { return this.m_FoundAttackRange; } set { this.m_FoundAttackRange = value; } }
}
