using UnityEngine;

[CreateAssetMenu(fileName = "NodeBlueprint", menuName = "Map/Node Blueprint")]
public class NodeBlueprint : ScriptableObject
{
    public Sprite sprite;
    public NodeType nodeType;
}