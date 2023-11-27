using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatGrid : MonoBehaviour
{
    [Header("Grid Size")]
    [SerializeField] private int m_Rows = 10;
    [SerializeField] private int m_Columns = 10;
    [SerializeField] private int m_Scale = 1;

    [SerializeField] private GameObject m_GridPrefab;
    [SerializeField] private Vector3 m_BotLeftLocation = new Vector3(0, 0, 0);

    [SerializeField] private GameObject[,] m_Grids;

    private List<GameObject> m_PathableTiles = new List<GameObject>();
    private List<GameObject> m_TargetableTiles = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        this.m_Grids = new GameObject[this.m_Columns, this.m_Rows];

        if (this.m_GridPrefab != null)
        {
            this.GenerateGrid();
            this.transform.rotation = Quaternion.Euler(0, -45, 0);
        }

        else
            Debug.LogError("Missing GridPrefab in " + this.name);
    }

    private void Update()
    {
        //Currently not functional, plan is to reduce the number of tiles pathable when player moves from one to the other.
        //foreach (GameObject grid in this.m_PathableTiles)
        //{
        //    if (grid != CombatManager.Instance.CurrentUnitGrid)
        //    {
        //        CombatManager.Instance.ActiveUnitMoves--;
        //        this.RecalculateMovementRange(grid.GetComponent<GridStat>().xLoc, grid.GetComponent<GridStat>().yLoc, CombatManager.Instance.ActiveUnitMoves);
        //    }
        //}
    }

    private void OnDisable()
    {
        this.ResetGrid();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < m_Columns; i++)
        {
            for (int j = 0; j < m_Rows; j++)
            {
                Vector3 m_CellPosition = new Vector3(this.m_BotLeftLocation.x + this.m_Scale * i, this.m_BotLeftLocation.y, this.m_BotLeftLocation.z + this.m_Scale + this.m_Scale * j);
                GameObject m_ObjInstance = Instantiate(this.m_GridPrefab, m_CellPosition, this.m_GridPrefab.transform.rotation);

                m_ObjInstance.transform.SetParent(this.gameObject.transform);
                m_ObjInstance.GetComponent<GridStat>().xLoc = i;
                m_ObjInstance.GetComponent<GridStat>().yLoc = j;

                this.m_Grids[i, j] = m_ObjInstance;
            }
        }
    }

    private void ResetGrid()
    {
        CombatManager.Instance.CurrentUnitGrid = null;

        CombatManager.Instance.FoundMoveRange = false;
        CombatManager.Instance.FoundAttackRange = false;

        foreach (GameObject grid in this.m_Grids)
            grid.GetComponent<GridStat>().ChangeTileState(0);

        this.m_PathableTiles.Clear();
        this.m_TargetableTiles.Clear();
    }

    public void CheckMovementRange(int x, int y, int range)
    {
        if (range <= 0) return;

        if (this.m_Grids[x + 1, y] != null && this.m_Grids[x + 1, y].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x + 1, y].GetComponent<GridStat>().ChangeTileState(2);
            this.CheckMovementRange(x + 1, y, range - 1);

            if (!this.m_PathableTiles.Contains(this.m_Grids[x + 1, y]))
                this.m_PathableTiles.Add(this.m_Grids[x + 1, y]);
        }

        if (this.m_Grids[x - 1, y] != null && this.m_Grids[x - 1, y].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x - 1, y].GetComponent<GridStat>().ChangeTileState(2);
            this.CheckMovementRange(x - 1, y, range - 1);

            if (!this.m_PathableTiles.Contains(this.m_Grids[x - 1, y]))
                this.m_PathableTiles.Add(this.m_Grids[x - 1, y]);
        }

        if (this.m_Grids[x, y + 1] != null && this.m_Grids[x, y + 1].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x, y + 1].GetComponent<GridStat>().ChangeTileState(2);
            this.CheckMovementRange(x, y + 1, range - 1);

            if (!this.m_PathableTiles.Contains(this.m_Grids[x, y + 1]))
                this.m_PathableTiles.Add(this.m_Grids[x, y + 1]);
        }

        if (this.m_Grids[x, y - 1] != null && this.m_Grids[x, y - 1].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x, y - 1].GetComponent<GridStat>().ChangeTileState(2);
            this.CheckMovementRange(x, y - 1, range - 1);

            if (!this.m_PathableTiles.Contains(this.m_Grids[x, y - 1]))
                this.m_PathableTiles.Add(this.m_Grids[x, y - 1]);
        }
    }

    public void CheckAttackRange(int x, int y, int range)
    {
        if (range <= 0) return;

        if (this.m_Grids[x + 1, y] != null && this.m_Grids[x + 1, y].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x + 1, y].GetComponent<GridStat>().ChangeTileState(1);
            this.CheckAttackRange(x + 1, y, range - 1);

            if (!this.m_TargetableTiles.Contains(this.m_Grids[x + 1, y]))
                this.m_TargetableTiles.Add(this.m_Grids[x + 1, y]);
        }

        if (this.m_Grids[x - 1, y] != null && this.m_Grids[x - 1, y].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x - 1, y].GetComponent<GridStat>().ChangeTileState(1);
            this.CheckAttackRange(x - 1, y, range - 1);

            if (!this.m_TargetableTiles.Contains(this.m_Grids[x - 1, y]))
                this.m_TargetableTiles.Add(this.m_Grids[x - 1, y]);
        }

        if (this.m_Grids[x, y + 1] != null && this.m_Grids[x, y + 1].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x, y + 1].GetComponent<GridStat>().ChangeTileState(1);
            this.CheckAttackRange(x, y + 1, range - 1);

            if (!this.m_TargetableTiles.Contains(this.m_Grids[x, y + 1]))
                this.m_TargetableTiles.Add(this.m_Grids[x, y + 1]);
        }

        if (this.m_Grids[x, y - 1] != null && this.m_Grids[x, y - 1].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x, y - 1].GetComponent<GridStat>().ChangeTileState(1);
            this.CheckAttackRange(x, y - 1, range - 1);

            if (!this.m_TargetableTiles.Contains(this.m_Grids[x, y - 1]))
                this.m_TargetableTiles.Add(this.m_Grids[x, y - 1]);
        }
    }

    public void RecalculateMovementRange(int x, int y, int movementSpeed)
    {
        ResetGrid();
        CheckMovementRange(x, y, movementSpeed);
    }

    public GameObject[,] Grids { get { return this.m_Grids; } }
}
