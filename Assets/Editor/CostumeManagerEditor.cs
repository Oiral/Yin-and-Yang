using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CostumeManager))]
public class CostumeManagerEditor : Editor
{
    bool materialFoldedOut;
    bool modelFoldedOut;

    public override void OnInspectorGUI()
    {
        //Show the default item
        base.OnInspectorGUI();

        CostumeManager manager = (CostumeManager)target;




        materialFoldedOut = EditorGUILayout.BeginFoldoutHeaderGroup(materialFoldedOut, "Materials");
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (materialFoldedOut)
        {
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



        modelFoldedOut = EditorGUILayout.BeginFoldoutHeaderGroup(modelFoldedOut, "Models");
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (modelFoldedOut)
        {
            Dictionary<string, PlayerModel> playerModels = new Dictionary<string, PlayerModel>();

            foreach (var playerModel in manager.playerModels)
            {
                GUILayout.BeginHorizontal();

                string testName = playerModel.Key;

                string modelName = GUILayout.TextField(playerModel.Key);


                GameObject testMaterial = playerModel.Value.modelPrefab;
                GameObject modelPrefab = (GameObject)EditorGUILayout.ObjectField(playerModel.Value.modelPrefab, typeof(GameObject), GUILayout.MinWidth(100f), GUILayout.MaxWidth(200f));

                playerModels.Add(modelName, new PlayerModel(modelPrefab));

                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    playerModels.Remove(modelName);
                    EditorUtility.SetDirty(target);

                    Undo.RecordObject(target, "Remove");
                }

                if (testName != modelName || testMaterial != modelPrefab)
                {
                    EditorUtility.SetDirty(target);

                    Undo.RecordObject(target, "Edit");
                }

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+"))
            {
                playerModels.Add((playerModels.Count + 1).ToString(), new PlayerModel());
                EditorUtility.SetDirty(target);
                Undo.RecordObject(target, "Add");
            }
            manager.playerModels = playerModels;
        }

       

    }

}
