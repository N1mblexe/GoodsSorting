using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{

    //TODO REMOVE THIS
    [SerializeField] private List<Item> itemPrefabs = new List<Item>();
    public static readonly uint maxGoodsPerCell = 3;

    [SerializeField] private int seed = 1111;
    private System.Random random;

    public static GoodsManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Multiple Instance of GoodsManager");
    }
    void Start()
    {
        random = new System.Random(seed);
    }

    public void SetGoods(List<List<Cell>> grid, uint fillPercent)
    {
        fillPercent = (uint)Mathf.Clamp((int)fillPercent, 10, 95);
        if (itemPrefabs == null || itemPrefabs.Count == 0) return;

        List<Cell> cells = FlattenGrid(grid);
        if (cells.Count == 0) return;

        int targetItemCount = CalculateTargetItemCount(cells.Count, (int)fillPercent);
        List<Item> pool = CreateItemPool(targetItemCount);
        Shuffle(pool);
        Shuffle(cells);

        DistributeItemsToCells(cells, pool);
    }

    #region Helpers

    private List<Cell> FlattenGrid(List<List<Cell>> grid)
    {
        List<Cell> cells = new List<Cell>();
        foreach (var row in grid)
            cells.AddRange(row);
        return cells;
    }

    private int CalculateTargetItemCount(int totalCells, int fillPercent)
    {
        int capacity = totalCells * (int)maxGoodsPerCell;
        int target = Mathf.RoundToInt(capacity * (fillPercent / 100f));

        if (target % maxGoodsPerCell != 0)
            target = Mathf.RoundToInt((float)target / maxGoodsPerCell) * (int)maxGoodsPerCell;

        return target;
    }

    private List<Item> CreateItemPool(int targetItemCount)
    {
        int totalChunks = targetItemCount / (int)maxGoodsPerCell;
        int prefabCount = itemPrefabs.Count;
        int[] chunksPerPrefab = new int[prefabCount];

        for (int i = 0; i < totalChunks; i++)
        {
            int p = random.Next(prefabCount);
            chunksPerPrefab[p]++;
        }

        List<Item> pool = new List<Item>();
        for (int p = 0; p < prefabCount; p++)
        {
            int qty = chunksPerPrefab[p] * (int)maxGoodsPerCell;
            for (int k = 0; k < qty; k++)
                pool.Add(itemPrefabs[p]);
        }
        return pool;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void DistributeItemsToCells(List<Cell> cells, List<Item> pool)
    {
        int remaining = pool.Count;
        int poolIndex = 0;

        for (int idx = 0; idx < cells.Count; idx++)
        {
            int remainingCells = cells.Count - idx;
            int allowedMin = Math.Max(0, remaining - (remainingCells - 1) * (int)maxGoodsPerCell);
            int allowedMax = Math.Min((int)maxGoodsPerCell, remaining);

            int itemsOnCell = (allowedMax >= allowedMin)
                ? random.Next(allowedMin, allowedMax + 1)
                : 0;

            List<Item> itemsForCell = new List<Item>();
            for (int k = 0; k < itemsOnCell; k++)
            {
                Item prefab = pool[poolIndex++];
                itemsForCell.Add(prefab);
            }
            remaining -= itemsOnCell;

            EnforceDifferentPrefabRule(itemsForCell, pool, ref poolIndex);
            AddItemsToCell(cells[idx], itemsForCell);
        }
    }

    private void EnforceDifferentPrefabRule(List<Item> itemsForCell, List<Item> pool, ref int poolIndex)
    {
        if (itemsForCell.Count == maxGoodsPerCell && itemsForCell.TrueForAll(i => i == itemsForCell[0]))
        {
            for (int searchIndex = poolIndex; searchIndex < pool.Count; searchIndex++)
            {
                if (pool[searchIndex] != itemsForCell[0])
                {
                    itemsForCell[itemsForCell.Count - 1] = pool[searchIndex];
                    pool[searchIndex] = itemsForCell[0];
                    break;
                }
            }
        }
    }

    private void AddItemsToCell(Cell cell, List<Item> items)
    {
        foreach (var prefab in items)
        {
            Item inst = Instantiate(prefab);
            cell.AddItem(inst);
        }
    }

    #endregion




}
