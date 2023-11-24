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

    [SerializeField] GameObject[,] m_Grids;

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

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject a in this.m_Grids)
        {
            if (a.GetComponent<GridStat>().IsPlayerWithin)
                this.CheckNeighbor(a.GetComponent<GridStat>().xLoc, a.GetComponent<GridStat>().yLoc);
        }
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

    private void CheckNeighbor(int x, int y)
    {
        if (this.m_Grids[x + 1, y].GetComponent<GridStat>().IsPassable)
            this.m_Grids[x + 1, y].GetComponent<GridStat>().ChangeTileState(2);

        if (this.m_Grids[x - 1, y].GetComponent<GridStat>().IsPassable)
            this.m_Grids[x - 1, y].GetComponent<GridStat>().ChangeTileState(2);

        if (this.m_Grids[x, y + 1].GetComponent<GridStat>().IsPassable)
            this.m_Grids[x, y + 1].GetComponent<GridStat>().ChangeTileState(2);

        if (this.m_Grids[x, y - 1].GetComponent<GridStat>().IsPassable)
            this.m_Grids[x, y - 1].GetComponent<GridStat>().ChangeTileState(2);

    }
}
