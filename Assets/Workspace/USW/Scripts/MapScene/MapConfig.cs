using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu]
    public class MapConfig : ScriptableObject
    {
        public List<NodeBlueprint> nodeBlueprints;
        public List<NodeType> randomNodes = new List<NodeType>
            {NodeType.Event, NodeType.Store, NodeType.MinorEnemy, NodeType.EliteEnemy};
        public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

        
        public IntMinMax numOfPreBossNodes;
        public IntMinMax numOfStartingNodes;
        
        public int extraPaths;
        public List<MapLayer> layers;
    }
}