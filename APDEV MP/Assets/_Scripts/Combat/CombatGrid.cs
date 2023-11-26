using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        
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
        CombatManager.Instance.FoundMoveRange = false;
        CombatManager.Instance.FoundAttackRange = false;

        foreach (GameObject grid in this.m_Grids)
            grid.GetComponent<GridStat>().ChangeTileState(0);
    }

    public void CheckMovementRange(int x, int y, int range)
    {
        if (range <= 0) return;

        if (this.m_Grids[x + 1, y].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x + 1, y].GetComponent<GridStat>().ChangeTileState(2);
            CheckMovementRange(x + 1, y, range - 1);
        }

        if (this.m_Grids[x - 1, y].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x - 1, y].GetComponent<GridStat>().ChangeTileState(2);
            CheckMovementRange(x - 1, y, range - 1);
        }

        if (this.m_Grids[x, y + 1].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x, y + 1].GetComponent<GridStat>().ChangeTileState(2);
            CheckMovementRange(x, y + 1, range - 1);
        }

        if (this.m_Grids[x, y - 1].GetComponent<GridStat>().IsPassable)
        {
            this.m_Grids[x, y - 1].GetComponent<GridStat>().ChangeTileState(2);
            CheckMovementRange(x, y - 1, range - 1);
        }
    }

    public GameObject[,] Grids { get { return this.m_Grids; } }
}
