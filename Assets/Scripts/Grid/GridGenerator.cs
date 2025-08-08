using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridGenerator : MonoBehaviour
    {
        [Header("CELL")]
        [SerializeField] private GameObject cellPrefab;

        [SerializeField] private List<List<GameObject>> currentCells = new List<List<GameObject>>();

        [SerializeField] private Vector2 cellSize = Vector2.one;

        [Header("SIZE&POSITIONING")]
        [SerializeField]
        private Vector2Int[] sizeBoundaries = new Vector2Int[2]
        {
        new Vector2Int(2, 2),
        new Vector2Int(16 , 9)
        };
        [SerializeField] private Vector2Int size;

        [SerializeField] private float spacing = .1f;
        private Vector2 originOffset;
        private Vector2 spacedCellSize;

        public void GenerateGrids()
        {
            /*if (currentCells == null)
            {
                Debug.LogWarning("No instance of currentGrids");
                return;
            }*/

            if (!CheckSizeLimits())
            {
                Debug.LogError($"Invalid size : {size}");
                return;
            }

            if (cellPrefab == null)
            {
                Debug.LogError("cellPrefab is null");
                return;
            }

            ClearCurrentCells();
            PreCalculate();

            for (uint x = 0; x < size.x; x++)
            {
                List<GameObject> row = new List<GameObject>();
                for (uint y = 0; y < size.y; y++)
                {
                    var cellPositon = CalculateCellPos(new Vector2(x, y));
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

        public void ClearCurrentCells()
        {
            /*if (currentCells == null)
            {
                Debug.LogWarning("no instance of currentGrids");
                return;
            }*/

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
        private Vector2 CalculateCellPos(Vector2 cellPos)
        {
            if ((size.x <= cellPos.x || size.y <= cellPos.y) || (cellPos.x < 0 || cellPos.y < 0))
                return Vector2.negativeInfinity;

            Vector2 position = new Vector2
            (
                (cellPos.x * spacedCellSize.x) - originOffset.x,
                (cellPos.y * spacedCellSize.y) - originOffset.y
            );

            return position;
        }

        private void PreCalculate()
        {
            spacedCellSize = CalculateSpacedCellSize();
            originOffset = CalculateOriginOffset();
        }

        private Vector2 CalculateSpacedCellSize()
        {
            return new Vector2(cellSize.x + spacing, cellSize.y + spacing);
        }
        private Vector2 CalculateOriginOffset()
        {
            return new Vector2
            (
                ((size.x - 1) * spacedCellSize.x) / 2f,
                ((size.y - 1) * spacedCellSize.y) / 2f
            );
        }

        private bool CheckSizeLimits()
        {
            return size.x >= sizeBoundaries[0].x &&
                   size.y >= sizeBoundaries[0].y &&
                   size.x <= sizeBoundaries[1].x &&
                   size.y <= sizeBoundaries[1].y;
        }

        #endregion
    }
}