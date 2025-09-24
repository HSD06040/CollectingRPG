using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Map/Map Config")]
public class MapConfig : ScriptableObject
{
    [Header("Node Blueprints")]
    public List<NodeBlueprint> nodeBlueprints;
    
    [Header("Random Nodes")]
    public List<NodeType> randomNodes = new List<NodeType>
        {NodeType.Event, NodeType.Store, NodeType.MinorEnemy};
    
    [Header("Grid Settings")]
    public int minNodesPerLayer = 2;
    public int maxNodesPerLayer = 6;
}