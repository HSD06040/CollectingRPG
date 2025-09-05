#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class HierarchyColorTag : MonoBehaviour
{
    public enum ColorType
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
        Orange
    }

    [SerializeField]
    public ColorType colorType = ColorType.None;
}
#endif


