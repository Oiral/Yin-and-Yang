using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CostumeManager))]
public class CostumeManagerEditor : Editor
{
    public static bool materialFoldedOut;
    public static bool modelFoldedOut;

    float numberWidth = 40f;
    float nameWidth = 100f;

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

            GUILayout.BeginHorizontal();

            GUILayout.Label("Name", GUILayout.Width(nameWidth));
            GUILayout.Label("Cost", GUILayout.Width(numberWidth));
            GUILayout.Label("Material");

            GUILayout.EndHorizontal();

            foreach (var costumeMaterial in manager.playerColours)
            {
                GUILayout.BeginHorizontal();

                string testName = costumeMaterial.Key;

                PlayerMaterial editingMaterial = costumeMaterial.Value;


                

                string materialName = GUILayout.TextField(costumeMaterial.Key, GUILayout.Width(nameWidth));

                editingMaterial.cost = EditorGUILayout.IntField(editingMaterial.cost, GUILayout.Width(numberWidth));

                editingMaterial.material = (Material)EditorGUILayout.ObjectField(costumeMaterial.Value.material, typeof(Material), GUILayout.MinWidth(100f), GUILayout.MaxWidth(200f));

                

                playerColours.Add(materialName, editingMaterial);

                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    playerColours.Remove(materialName);
                    EditorUtility.SetDirty(target);

                    Undo.RecordObject(target, "Remove");
                }

                if (testName != materialName || editingMaterial != costumeMaterial.Value)
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

            GUILayout.BeginHorizontal();

            GUILayout.Label("Name", GUILayout.Width(nameWidth));
            GUILayout.Label("Cost", GUILayout.Width(numberWidth));
            GUILayout.Label("Model (Prefab)");

            GUILayout.EndHorizontal();

            foreach (var playerModel in manager.playerModels)
            {
                GUILayout.BeginHorizontal();

                

                string testName = playerModel.Key;


                PlayerModel editingModel = playerModel.Value;

                string modelName = GUILayout.TextField(playerModel.Key, GUILayout.Width(nameWidth));

                editingModel.cost = EditorGUILayout.IntField(editingModel.cost, GUILayout.Width(numberWidth));


                editingModel.modelPrefab = (GameObject)EditorGUILayout.ObjectField(editingModel.modelPrefab, typeof(GameObject), GUILayout.MinWidth(100f), GUILayout.MaxWidth(200f));


                

                playerModels.Add(modelName, editingModel);

                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    playerModels.Remove(modelName);
                    EditorUtility.SetDirty(target);

                    Undo.RecordObject(target, "Remove");
                }

                //If the value has been changed, We want to set as dirty
                if (testName != modelName || editingModel != playerModel.Value)
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
