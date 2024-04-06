using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    public Dictionary<string, GameObject> view = new Dictionary<string, GameObject>();

    private void LoadAllObjects(GameObject root, string path)
    {
        foreach(Transform tf in root.transform)
        {
            if (this.view.ContainsKey(path + tf.gameObject.name))
            {
                continue;
            }

            this.view.Add(path + tf.gameObject.name, tf.gameObject);
            LoadAllObjects(tf.gameObject, path + tf.gameObject.name);
        }
    }

    public virtual void Awake()
    {
        this.LoadAllObjects(this.gameObject, "");
    }
}
