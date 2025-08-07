using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridGenerator gridGenerator = (GridGenerator)target;

        if (GUILayout.Button("Generate Grids!"))
        {
            gridGenerator.GenerateGrids();
        }
        if (GUILayout.Button("Clear Grids!"))
        {
            gridGenerator.ClearCurrentCells();
        }
    }
}
