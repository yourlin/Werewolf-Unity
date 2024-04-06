using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UICreator : EditorWindow
{
    public static string filePath = "/Scripts/Game/UIControllers";
    public static string suffix = "_Ctrl";

    [MenuItem("linye/UICreator")]
    public static void CreateUI()
    {
        UICreator win = EditorWindow.GetWindow<UICreator>();
        win.titleContent.text = "UI Creator";
        win.Show();
    }

    public void OnGUI()
    {
        if (Selection.activeGameObject != null)
        {
            GUILayout.Label(Selection.activeGameObject.name);
            GUILayout.Label(filePath + Selection.activeGameObject.name + suffix + ".cs");
        }
        else
        {
            GUILayout.Label("没有选中的UI节点，无法生成");
        }

        if (GUILayout.Button("生成UI代码文件"))
        {
            if (Selection.activeGameObject != null)
            {
                string className = Selection.activeGameObject.name + suffix;
                UICreatorUtil.GenUICtrlFile(filePath, className);
            }
        }
    }

    public void OnSelectionChange()
    {
        Repaint();
    }
}

