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
            GUILayout.Label("û��ѡ�е�UI�ڵ㣬�޷�����");
        }

        if (GUILayout.Button("����UI�����ļ�"))
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

