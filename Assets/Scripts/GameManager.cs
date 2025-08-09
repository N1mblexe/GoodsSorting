using System;
using System.Collections.Generic;
using System.Threading;
using Grid;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private enum Difficulty { EASY = 40, NORMAL = 60, HARD = 90 }
    [SerializeField] private static uint difficulty = (uint)Difficulty.NORMAL.GetHashCode();
    private static Utilities.GameState gameState;
    public static Utilities.GameState getGameState() { return gameState; }
    public static void SetDifficulty(uint percentage)
    {
        if (percentage >= 10 && percentage <= 95)
            difficulty = percentage;
    }

    //use _mainThreadActions.Enqueue for unity dependant utils
    //also use this in update function
    //while (_mainThreadActions.Count > 0)
    //{
    //    var action = _mainThreadActions.Dequeue();
    //    action?.Invoke();
    //}
    [Obsolete]
    public static Thread StartGameAsync()
    {
        Thread thread = new Thread(new ThreadStart(StartGame));
        thread.Start();
        return thread;
    }

    public static void StartGame()
    {
        GridGenerator gridGenerator = GridGenerator.Instance;
        GoodsManager goodsManager = GoodsManager.Instance;

        gridGenerator.GenerateGrids();
        var grid = gridGenerator.getCurrentCells();

        List<List<Cell>> cells = GetCellsFromObject(grid);

        goodsManager.SetGoods(cells, difficulty);
    }

    void Start()
    {
        gameState = Utilities.GameState.LOADING;
        StartGame();
        gameState = Utilities.GameState.PLAYING;
    }


    #region HelperFunctions

    public static List<List<Cell>> GetCellsFromObject(List<List<GameObject>> objects)
    {
        var cells = new List<List<Cell>>();

        //fill the cells 2D array using grid
        for (int x = 0; x < objects.Count; x++)
        {
            List<Cell> row = new List<Cell>();
            for (int y = 0; y < objects[x].Count; y++)
            {
                var component = objects[x][y].GetComponent<Cell>();
                if (component == null)
                {
                    Debug.LogError($"Missing Cell component at : {x} , {y}");
                    continue;
                }

                row.Add(component);
            }
            cells.Add(row);
        }

        return cells;
    }

    #endregion
}