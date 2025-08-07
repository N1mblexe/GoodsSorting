using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private List<List<GameObject>> currentCells = new List<List<GameObject>>();
    [SerializeField] private Vector2 cellSize = Vector2.one;
    [SerializeField] private Vector2Int size;
    private static readonly Vector2Int[] sizeLimites = new Vector2Int[]
    {
        new Vector2Int(2, 2),
        new Vector2Int(16 , 9)
    };
    public void GenerateGrids()
    {
        if (!CheckSizeLimits(size))
        {
            Debug.LogError($"Invalid size : {size}");
            return;
        }
        //if (currentCells == null)
        //{
        //    Debug.LogWarning("No instance of currentGrids");
        //    return;
        //}
        if (cellPrefab == null)
        {
            Debug.LogError("cellPrefab is null");
            return;
        }

        ClearCurrentCells();

        for (uint x = 0; x < size.x; x++)
        {
            List<GameObject> row = new List<GameObject>();
            for (uint y = 0; y < size.y; y++)
            {
                var cellPositon = CalculateCellPos(size, new Vector2(x, y));
                if (cellPositon == Vector2.negativeInfinity)
                {
                    Debug.LogError($"Invalid cell position relative to the size : ({x} , {y})");
                    continue;
                }

                var cell = Instantiate(cellPrefab, this.transform);
                cell.transform.position = cellPositon;

                row.Add(cell);
            }
            currentCells.Add(row);
        }

    }

    private void ClearCurrentCells()
    {
        //if (currentCells == null)
        //{
        //    Debug.LogWarning("no instance of currentGrids");
        //    return;
        //}
        //Delete cells
        foreach (var list in currentCells)
        {
            foreach (var cell in list)
            {
                if (Application.isPlaying) Destroy(cell);
                else DestroyImmediate(cell);
            }
        }
        currentCells.Clear();
    }

    #region HelperFunctions

    /// <summary>
    /// returns the cell transform position
    /// </summary>
    /// <param name="size"></param>
    /// <param name="celllPos">position in the array(not the transform position)</param>
    /// <returns></returns>
    private Vector2 CalculateCellPos(Vector2Int size, Vector2 cellPos)
    {
        //cell pos is out of bounds
        if ((size.x < cellPos.x || size.y < cellPos.y) || (cellPos.x < 0 || cellPos.y < 0))
            return Vector2.negativeInfinity;

        Vector2 originOffset = new Vector2
        (
            ((size.x - 1) * cellSize.x) / 2,
            ((size.y - 1) * cellSize.y) / 2
        );

        Vector2 position = new Vector2
        (
            (cellPos.x * cellSize.x) - originOffset.x,
            (cellPos.y * cellSize.y) - originOffset.y
        );


        return position;
    }

    private bool CheckSizeLimits(Vector2 size)
    {
        return size.x >= sizeLimites[0].x &&
               size.y >= sizeLimites[0].y &&
               size.x <= sizeLimites[1].x &&
               size.y <= sizeLimites[1].y;
    }

    #endregion
}
