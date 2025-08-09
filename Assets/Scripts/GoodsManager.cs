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

    List<GameObject> goods = new List<GameObject>();

    //TODO IMPLEMENT THIS FUNCTION
    public void SetGoods(List<List<Cell>> grid)
    {
        foreach (var row in grid)
        {
            foreach (var cell in row)
            {
                uint itemsOnCell = (uint)random.Next(0, (int)(maxGoodsPerCell + 1));

                for (int i = 0; i < itemsOnCell; i++)
                {
                    Item item = Instantiate(itemPrefabs[random.Next(itemPrefabs.Count)]);
                    cell.AddItem(item);
                }

            }
        }
    }
}
