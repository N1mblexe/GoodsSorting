using System.Collections.Generic;
using Grid;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{

    //TODO REMOVE THIS
    [SerializeField] private GameObject tempItemPrefab;
    public static GoodsManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Multiple Instance of GoodsManager");
    }
    List<GameObject> goods = new List<GameObject>();

    public void SetGoods(List<List<Cell>> grid)
    {
        SetGoods(grid, Random.Range(0, 99999));
    }

    //TODO IMPLEMENT THIS FUNCTION
    public void SetGoods(List<List<Cell>> grid, int seed)
    {
        foreach (var row in grid)
        {
            foreach (var cell in row)
            {
                var goods = new Item[] {
                    Instantiate(tempItemPrefab).GetComponent<Item>(),
                    Instantiate(tempItemPrefab).GetComponent<Item>(),
                    Instantiate(tempItemPrefab).GetComponent<Item>(),
                 };

                cell.SetGoods(goods);
            }
        }
    }
}
