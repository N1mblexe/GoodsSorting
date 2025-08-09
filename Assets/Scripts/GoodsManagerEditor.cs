using Grid;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoodsManager))]
public class GoodsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GoodsManager goodsManager = (GoodsManager)target;

        if (GUILayout.Button("Generate Goods!"))
        {
            //goodsManager.SetGoods(GameManager.GetCellsFromObject(GridGenerator.Instance.getCurrentCells()));
        }
        //Clear
        //if (GUILayout.Button("Clear goods!"))
        //{
        //}
    }
}
