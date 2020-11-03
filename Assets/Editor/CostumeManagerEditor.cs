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

        Dictionary<string, PlayerMaterial> playerColours = new Dictionary<string, PlayerMaterial>();

        foreach (var costumeMaterial in manager.playerColours)
        {
            GUILayout.BeginHorizontal();

            string testName = costumeMaterial.Key;

            string materialName = GUILayout.TextField(costumeMaterial.Key);



            Material testMaterial = costumeMaterial.Value.material;
            Material materialMat = (Material)EditorGUILayout.ObjectField(costumeMaterial.Value.material, typeof(Material), GUILayout.MinWidth(100f), GUILayout.MaxWidth(200f));

            playerColours.Add(materialName, new PlayerMaterial(materialMat));

            if (GUILayout.Button("-", GUILayout.Width(20f)))
            {
                playerColours.Remove(materialName);
                EditorUtility.SetDirty(target);

                Undo.RecordObject(target, "Remove");
            }

            if (testName != materialName || testMaterial != materialMat)
            {
                EditorUtility.SetDirty(target);

                Undo.RecordObject(target, "Edit");
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("+"))
        {
            playerColours.Add((playerColours.Count + 1).ToString(), new PlayerMaterial());
            EditorUtility.SetDirty(target);
            Undo.RecordObject(target, "Add");
        }

        manager.playerColours = playerColours;
    }

}
