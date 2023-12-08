using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header ("Combat Grid")]
    [SerializeField] private GameObject m_CombatGrid;
    [SerializeField] private CombatGrid m_CombatGridScript;

    [Header ("Combat Data")]
    [SerializeField] private GameObject m_CurrentUnitGrid = null;
    [SerializeField] private bool m_IsInCombat = false;

    [Header ("Turn Order")]
    [SerializeField] private List<GameObject> m_UnitList = new List<GameObject>();
    private int m_CurrentTurnIndex = -1;

    [Header ("Debug Settings")]
    [SerializeField] private bool m_IsViewingMoveRange = false;
    [SerializeField] private bool m_IsViewingAttackRange = false;

    [SerializeField] private int m_ActiveUnitMoves = 0;
    [SerializeField] private int m_ActiveUnitAttackRange = 0;

    private bool m_HasAttacked = false;
    private bool m_HasEndedTurn = false;

    private bool m_IsRequestingRoll = false;
    private bool m_IsEnemyTurn = false;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        GestureManager.Instance.OnTapDelegate += this.MoveUnit;
        GestureManager.Instance.OnTapDelegate += this.AttackUnit;
        GestureManager.Instance.OnTapDelegate += this.HealUnit;
    }

    private void MoveUnit(object sender, TapEventArgs args)
    {
        if (args.ObjHit != null && args.ObjHit.CompareTag("CombatTile"))
        {
            if (args.ObjHit.GetComponent<GridStat>().IsPathable && this.m_ActiveUnitMoves > 0)
            {
                GridStat m_CurrentTile = this.m_CurrentUnitGrid.GetComponent<GridStat>();
                GridStat m_TargetTile = args.ObjHit.GetComponent<GridStat>();

                this.m_ActiveUnitMoves -= Mathf.Abs(m_CurrentTile.xLoc - m_TargetTile.xLoc) + Mathf.Abs(m_CurrentTile.yLoc - m_TargetTile.yLoc);
                
                NavMeshAgent m_Agent = PartyManager.Instance.ActivePlayer.GetComponent<NavMeshAgent>();

                m_Agent.SetDestination(args.ObjHit.transform.position);
                m_Agent.stoppingDistance = 0.2f;
                m_Agent.isStopped = false;
                StartCoroutine(this.WaitForMovement(m_TargetTile.xLoc, m_TargetTile.yLoc));

                Debug.Log("MOVEMENT VALID");
            }

            else
                Debug.Log("Area is not pathable");
        }
    }

    private void AttackUnit(object sender, TapEventArgs args)
    {
        if (args.ObjHit != null && (args.ObjHit.CompareTag("CombatTile")))
        {
            GridStat m_GridStat = args.ObjHit.GetComponent<GridStat>();

            if (m_GridStat.IsTargetable && m_GridStat.HasHostileUnit && !this.m_HasAttacked)
            {
                int m_HitChance = Random.Range(1, 21);
                int m_DamageDealt = 0;
                
                CharacterData m_ActiveUnitData = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData;
                CharacterData m_DamagedUnitData = m_GridStat.UnitInTile.GetComponent<CharacterScript>().CharacterData;

                if (m_HitChance < 10 + m_DamagedUnitData.DEXMod)
                    UIManager.Instance.ChangeText($"Attack on {m_DamagedUnitData.PlayerName} has missed!");

                else if (m_HitChance == 20)
                {
                    m_DamageDealt = this.CheckDamage(m_ActiveUnitData) * 2;

                    UIManager.Instance.ChangeText($"Critical strike on {m_DamagedUnitData.PlayerName} for {m_DamageDealt}!");
                    m_DamagedUnitData.CurrHealth -= m_DamageDealt;
                }

                else
                {
                    m_DamageDealt = this.CheckDamage(m_ActiveUnitData);

                    UIManager.Instance.ChangeText($"Successful hit on {m_DamagedUnitData.PlayerName} for {m_DamageDealt}!");
                    m_DamagedUnitData.CurrHealth -= m_DamageDealt;
                }
                 
                if (m_DamagedUnitData.CurrHealth <= 0)
                {
                    m_DamagedUnitData.CurrHealth = 0;

                    this.m_UnitList.Remove(m_GridStat.UnitInTile);


                    m_GridStat.UnitInTile.GetComponent<CharacterScript>().TriggerPlayerDeath();
                    
                    //PERHAPS DEATH ANIMATION FIRST BEFORE DISABLE?
                    m_GridStat.UnitInTile.SetActive(false);


                    this.CheckCombatEnd();
                }

                this.m_HasAttacked = true;
                this.m_ActiveUnitAttackRange = 0;
            }

            else
                Debug.Log("Path is not targetable or Path has no hostile unit");
        }
    }

    private void HealUnit(object sender, TapEventArgs args)
    {
        if (args.ObjHit != null && (args.ObjHit.CompareTag("CombatTile")))
        {
            GridStat m_GridStat = args.ObjHit.GetComponent<GridStat>();

            if (m_GridStat.IsTargetable && m_GridStat.HasAllyUnit && !this.m_HasAttacked)
            {
                if (PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CharacterClass == EnumUnitClass.PALADIN)
                {
                    CharacterData m_HealedUnitData = m_GridStat.UnitInTile.GetComponent<CharacterScript>().CharacterData;
                    int m_HealAmount = Random.Range(1, 9) + PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CHAMod;
                    m_HealedUnitData.CurrHealth += m_HealAmount;

                    if (m_HealedUnitData.CurrHealth > m_HealedUnitData.MaxHealth)
                        m_HealedUnitData.CurrHealth = m_HealedUnitData.MaxHealth;

                    this.m_HasAttacked = true;
                    this.m_ActiveUnitAttackRange = 0;

                    UIManager.Instance.ChangeText($"Heal {m_HealedUnitData.PlayerName} for {m_HealAmount} points!");
                }

                else
                    Debug.Log("Active unit is not a Paladin!");
            }

            else
                Debug.Log("Path is not targetable or Path has no ally unit.");
        }
    }

    private void AttackRandomAlly()
    {
        int m_HitChance = Random.Range(1, 21);
        int m_DamageDealt = 0;

        int m_Rand = Random.Range(0,4);
        CharacterData m_AllyTargetData = null;
        CharacterData m_EnemyAttackerData = this.m_UnitList[this.m_CurrentTurnIndex].GetComponent<CharacterScript>().CharacterData;

        while (m_AllyTargetData == null)
        {
            m_Rand = Random.Range(0,4);

            if (this.m_UnitList.Contains(PartyManager.Instance.PartyEntities[m_Rand]))
                m_AllyTargetData = PartyManager.Instance.PartyEntities[m_Rand].GetComponent<CharacterScript>().CharacterData;
        }


        if (!GameSettings.IS_GODMODE_ON)
        {
            if (m_HitChance < 10 + m_AllyTargetData.DEXMod + 2)
                UIManager.Instance.ChangeText($"{m_EnemyAttackerData.PlayerName}'s attack on {m_AllyTargetData.PlayerName} has missed!");

            else if (m_HitChance == 20)
            {
                m_DamageDealt = this.CheckEnemyDamage(m_EnemyAttackerData) * 2;

                UIManager.Instance.ChangeText($"{m_EnemyAttackerData.PlayerName} critically strikes {m_AllyTargetData.PlayerName} for {m_DamageDealt}!");
                m_AllyTargetData.CurrHealth -= m_DamageDealt;
            }

            else
            {
                m_DamageDealt = this.CheckEnemyDamage(m_EnemyAttackerData);
                UIManager.Instance.ChangeText($"{m_EnemyAttackerData.PlayerName} strikes {m_AllyTargetData.PlayerName} for {m_DamageDealt}!");
                m_AllyTargetData.CurrHealth -= m_DamageDealt;
            }

            if (m_AllyTargetData.CurrHealth <= 0)
            {
                m_AllyTargetData.CurrHealth = 0;

                this.m_UnitList.Remove(PartyManager.Instance.PartyEntities[m_Rand]);
                PartyManager.Instance.PartyEntities[m_Rand].GetComponent<CharacterScript>().TriggerPlayerDeath();
                this.CheckCombatEnd();
            }
        }

        else
            UIManager.Instance.ChangeText($"{m_AllyTargetData.PlayerName} blocked the hit!");

    }

    private int CheckDamage(CharacterData data)
    {
        switch(data.CharacterClass)
        {
            case EnumUnitClass.FIGHTER:
                return Random.Range(1, 9) + data.STRMod;

            case EnumUnitClass.PALADIN:
                return Random.Range(1, 7) + data.STRMod;

            case EnumUnitClass.ROGUE:
                return Random.Range(1, 7) + data.DEXMod;

            case EnumUnitClass.MAGE:
                return Random.Range(1, 11) + data.INTMod;

            default:
                return 0;
        }
    }

    private int CheckEnemyDamage(CharacterData data)
    {
        switch(data.CharacterClass)
        {
            case EnumUnitClass.PALADIN:
            case EnumUnitClass.FIGHTER:
                return Random.Range(1, 3) + data.STRMod;

            case EnumUnitClass.ROGUE:
                return Random.Range(1, 4) + data.DEXMod;

            case EnumUnitClass.MAGE:
                return Random.Range(1, 5) + data.INTMod;

            default:
                return 0;
        }
    }

    private int CheckUnitMovementSpeed(EnumUnitClass jobclass)
    {
        switch (jobclass)
        {
            case EnumUnitClass.MAGE:
                return 2;

            case EnumUnitClass.FIGHTER:
            case EnumUnitClass.PALADIN:
            case EnumUnitClass.ROGUE:
                return 3;

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
                return 3;

            case EnumUnitClass.ROGUE:
                return 4;

            default:
                return 0;
        }
    } 

    private void SwitchNextActiveUnit()
    {
        this.m_CurrentTurnIndex++;

        if (this.m_CurrentTurnIndex >= this.m_UnitList.Count)
            this.m_CurrentTurnIndex = 0;

        if (PartyManager.Instance.PartyEntities.Contains(this.m_UnitList[this.m_CurrentTurnIndex]))
        {
            this.m_IsEnemyTurn = false;
            PartyManager.Instance.SwitchActiveCharacterByObject(this.m_UnitList[this.m_CurrentTurnIndex]);
        }

        else
        {
            this.m_IsEnemyTurn = true;
            StartCoroutine(this.EnemyAction());
        }

        UIManager.Instance.ChangeTurn($"{this.m_UnitList[this.m_CurrentTurnIndex].name}'s Turn!");

        /****************
         * CAMERA TRACK *
         * *************/
        CustomCameraSwitcher.Instance.SwitchCamera(EnumCameraID.COMBAT_CAM, this.m_UnitList[this.m_CurrentTurnIndex]); 

        this.m_ActiveUnitMoves = this.CheckUnitMovementSpeed(PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CharacterClass);
        this.m_ActiveUnitAttackRange = this.CheckUnitAttackRange(PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CharacterClass);
        this.m_CombatGridScript.ResetGrid();
    }

    private IEnumerator EnemyAction()
    {
        this.m_IsViewingAttackRange = false;
        this.m_IsViewingMoveRange = false;

        yield return new WaitForSeconds(2);
        this.AttackRandomAlly();
        this.EndTurn();
    }

    private IEnumerator WaitForMovement(int x, int y)
    {
        GridStat m_CurrentTile = this.m_CurrentUnitGrid.GetComponent<GridStat>();
        NavMeshAgent m_Agent = PartyManager.Instance.ActivePlayer.GetComponent<NavMeshAgent>();

        while (m_CurrentTile.xLoc != x && m_CurrentTile.yLoc != y)
            yield return null;

        this.m_CombatGridScript.ResetGrid();
        this.m_CombatGridScript.CheckMovementRange(x, y, this.m_ActiveUnitMoves);
    }

    private IEnumerator WaitForTurnEnd()
    {
        while ((this.m_ActiveUnitMoves > 0 || !this.m_HasAttacked) && !this.m_HasEndedTurn)
            yield return null;

        this.EndTurn();
    }

    public void EndTurn()
    {
        this.m_HasAttacked = false;
        this.SwitchNextActiveUnit();
    }

    public void BeginCombat()
    {
        this.m_IsInCombat = true;

        foreach (GameObject member in PartyManager.Instance.PartyEntities)
            member.GetComponent<NavMeshAgent>().isStopped = true;

        this.RetrieveUnits();
        this.CheckCombatEnd();

        this.SwitchNextActiveUnit();

        CustomCameraSwitcher.Instance.SwitchCamera(EnumCameraID.COMBAT_CAM);
    }

    private void CheckCombatEnd()
    {
        if (this.m_UnitList.Count == 0)
            return;

        string m_FirstUnitTag = this.m_UnitList[0].tag;

        foreach (GameObject unit in this.m_UnitList)
            if (unit.tag != m_FirstUnitTag)
                return;

        this.EndCombat();

        if (m_FirstUnitTag == "Hostile")
            SceneLoaderManager.Instance.LoadScene(GameSettings.END_SCENE_INDEX, 0, true);
    }

    public void EndCombat()
    {
        this.IsInCombat = false;
        this.m_CurrentTurnIndex = -1;

        foreach (GameObject unit in this.m_UnitList)
            unit.GetComponent<CharacterScript>().CharacterData.Initiative = 0;

        foreach (GameObject member in PartyManager.Instance.PartyEntities)
            member.GetComponent<NavMeshAgent>().isStopped = true;

        this.m_UnitList.Clear();
        MusicManager.instance.RevertBGM();



        /**********************
         *    CAMERA TEST     *
         *********************/
        CustomCameraSwitcher.Instance.SwitchCamera(EnumCameraID.PLAYER_CAM);
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
                    this.m_CombatGridScript.CheckMovementRange(m_GridStat.xLoc, m_GridStat.yLoc, this.m_ActiveUnitMoves);

                else if (this.m_IsViewingAttackRange)
                    this.m_CombatGridScript.CheckAttackRange(m_GridStat.xLoc, m_GridStat.yLoc, this.m_ActiveUnitAttackRange);

                else
                    this.m_CombatGridScript.ResetGrid();

                StartCoroutine(this.WaitForTurnEnd());
            }
        }
    }

    public GameObject CurrentUnitGrid { get { return this.m_CurrentUnitGrid; }  set { this.m_CurrentUnitGrid = value; } }
    public bool IsInCombat { get { return this.m_IsInCombat; } set { this.m_IsInCombat = value; } }
    public int ActiveUnitMoves { get { return this.m_ActiveUnitMoves; } set { this.m_ActiveUnitMoves = value; } }
    public bool IsViewingMoveRange { get { return this.m_IsViewingMoveRange; } set { this.m_IsViewingMoveRange = value; } }
    public bool IsViewingAttackRange { get { return this.m_IsViewingAttackRange; } set { this.m_IsViewingAttackRange = value; } }
    public bool HasEndedTurn { get { return this.m_HasEndedTurn;} set { this.m_HasEndedTurn = value; } }
    public bool IsRequestingRoll { get { return this.m_IsRequestingRoll; } set { this.m_IsRequestingRoll = value; } }
    public bool IsEnemyTurn { get { return this.m_IsEnemyTurn; } }
}
