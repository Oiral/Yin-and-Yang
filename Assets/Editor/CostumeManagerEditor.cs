using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CostumeManager))]
public class CostumeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Show the default item
        base.OnInspectorGUI();

        CostumeManager manager = (CostumeManager)target;

        Dictionary<string, Material> playerColours = new Dictionary<string, Material>();

        foreach (var costumeMaterial in manager.playerColours)
        {
            GUILayout.BeginHorizontal();

            string materialName = GUILayout.TextField(costumeMaterial.Key);

            Material materialMat = (Material)EditorGUILayout.ObjectField(costumeMaterial.Value, typeof(Material), GUILayout.MinWidth(100f), GUILayout.MaxWidth(200f));

            playerColours.Add(materialName, materialMat);

            if (GUILayout.Button("-", GUILayout.Width(20f)))
            {
                playerColours.Remove(materialName);
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("+"))
        {
            playerColours.Add((playerColours.Count + 1).ToString(), null);
        }

        manager.playerColours = playerColours;
    }

}
