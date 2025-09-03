using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        AutoBind();
    }

    private void AutoBind()
    {
        var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        foreach (var field in fields)
        {
            var attr = field.GetCustomAttribute<UIBindAttribute>();
            if (attr == null) continue;

            string path = attr.Path ?? field.Name;

            var target = transform.Find(path);

            if (target == null)
            {
                Debug.LogError($"[UIBase] 경로를 찾을 수 없습니다.: {path} : {field.Name} 의 {GetType().Name}");
                continue;
            }

            var component = target.GetComponent(field.FieldType);

            if (component == null)
            {
                Debug.LogError($"[UIBase] 컴포넌트를 찾을 수 없습니다.: {path} : {field.Name} 의 {GetType().Name}");
                continue;
            }

            field.SetValue(this, component);
        }
    }
}
