using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class AutoRenumberAssets : EditorWindow
{
    private string targetFolder = "Assets/APP/Script/ScriptableObject/Itemdata/EquipmentData/";
    private string suffix = "Head";
    private int startIndex = 1;
    private int padding = 3; // 001, 002 のように3桁にする

    [MenuItem("Tools/Auto Renumber ScriptableObjects")]
    public static void ShowWindow()
    {
        GetWindow<AutoRenumberAssets>("Auto Renumber");
    }

    private void OnGUI()
    {
        GUILayout.Label("フォルダ内のScriptableObjectを自動で再番号付け", EditorStyles.boldLabel);
        targetFolder = EditorGUILayout.TextField("対象フォルダ", targetFolder);
        suffix = EditorGUILayout.TextField("接尾辞 (Suffix)", suffix);
        startIndex = EditorGUILayout.IntField("開始番号", startIndex);
        padding = EditorGUILayout.IntField("桁数 (例: 3 → 001)", padding);

        if (GUILayout.Button("番号を自動整理"))
        {
            Renumber();
        }
    }

    private void Renumber()
    {
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { targetFolder });

        // 対象フォルダ内のアセットをファイル名順で並べ替え
        var assets = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .OrderBy(path => Path.GetFileNameWithoutExtension(path))
            .ToList();

        int index = startIndex;
        foreach (var path in assets)
        {
            string newName = $"{index.ToString().PadLeft(padding, '0')}_{suffix}";
            AssetDatabase.RenameAsset(path, newName);
            Debug.Log($"Renamed: {path} → {newName}");
            index++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("自動再番号付けが完了しました！");
    }
}
