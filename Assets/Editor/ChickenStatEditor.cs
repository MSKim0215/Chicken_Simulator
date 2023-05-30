using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChickenStat))]
public class ChickenStatEditor : Editor
{
    private bool showGenesStats = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChickenStat chickenStat = (ChickenStat)target;

        showGenesStats = EditorGUILayout.Foldout(showGenesStats, "Genes Stats");

        if (showGenesStats)
        {
            EditorGUI.indentLevel++;
            foreach (var stat in chickenStat.GenesStats)
            {
                EditorGUILayout.LabelField(stat.Key.ToString(), stat.Value.ToString());
            }
            EditorGUI.indentLevel--;
        }
    }
}
