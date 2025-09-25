using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StageMapGenerator
{
    // 맵 구성 상수
    private const int TOTAL_LAYERS = 10; 
    private const int START_LAYER = 0;   
    private const int FIRST_BATTLE_LAYER = 1;  
    private const int CONFIRMED_STORE_LAYER = 5; 
    private const int BOSS_LAYER = 9;   
    private const int ELITE_START_LAYER = 4; 
    
    // 노드 개수 설정
    private const int MIN_NODES_LAYER_1_9 = 2;
    private const int MAX_NODES_LAYER_1_9 = 6; 
    private const int MIN_NODES_OTHER = 2;     
    private const int MAX_NODES_OTHER = 4;     
    
    // 비율 설정 (확정 노드 제외 후)
    private const float ELITE_RATIO = 0.22f;  
    private const float STORE_RATIO = 0.10f;  
    private const float EVENT_RATIO = 0.25f;  
    // 나머지 43%는 모든 일반 전투

    public static Map GetCustomMap(MapConfig config)
    {
        var mapStructure = GenerateMapStructure();
        var nodeTypes = AssignNodeTypes(mapStructure);
        
        // 갈림길 선택지 다양성 검증
        EnsureDiverseChoices(nodeTypes);
        
        return CreateMapFromStructure(config, mapStructure, nodeTypes);
    }

    private static List<int> GenerateMapStructure()
    {
        List<int> layerNodeCounts = new List<int>(TOTAL_LAYERS);
        
        for (int layer = 0; layer < TOTAL_LAYERS; layer++)
        {
            int nodeCount;
            
            if (layer == START_LAYER || layer == BOSS_LAYER)
            {
                // 시작점은 1개, 보스층은 2~6개
                nodeCount = layer == START_LAYER ? 1 : Random.Range(MIN_NODES_LAYER_1_9, MAX_NODES_LAYER_1_9 + 1);
            }
            else if (layer == FIRST_BATTLE_LAYER)
            {
                // 1층은 2~6개
                nodeCount = Random.Range(MIN_NODES_LAYER_1_9, MAX_NODES_LAYER_1_9 + 1);
            }
            else if (layer == CONFIRMED_STORE_LAYER)
            {
                // 5층 상점은 적당히
                nodeCount = Random.Range(MIN_NODES_OTHER, MAX_NODES_OTHER + 1);
            }
            else
            {
                // 나머지 층들
                nodeCount = Random.Range(MIN_NODES_OTHER, MAX_NODES_OTHER + 1);
            }
            
            layerNodeCounts.Add(nodeCount);
        }
        
        Debug.Log($"맵 구조 생성 완료: {string.Join(", ", layerNodeCounts)}");
        return layerNodeCounts;
    }

    private static List<List<NodeType>> AssignNodeTypes(List<int> layerNodeCounts)
    {
        var result = new List<List<NodeType>>();
        
        // 1. 확정 노드들 배치
        for (int layer = 0; layer < TOTAL_LAYERS; layer++)
        {
            var layerNodes = new List<NodeType>();
            
            if (layer == START_LAYER)
            {
                // 0층: 시작점 (일반 전투로 처리)
                layerNodes.Add(NodeType.MinorEnemy);
            }
            else if (layer == FIRST_BATTLE_LAYER)
            {
                // 1층: 모두 일반 전투
                for (int i = 0; i < layerNodeCounts[layer]; i++)
                    layerNodes.Add(NodeType.MinorEnemy);
            }
            else if (layer == CONFIRMED_STORE_LAYER)
            {
                // 5층: 모두 상점
                for (int i = 0; i < layerNodeCounts[layer]; i++)
                    layerNodes.Add(NodeType.Store);
            }
            else if (layer == BOSS_LAYER)
            {
                // 9층: 모두 보스
                for (int i = 0; i < layerNodeCounts[layer]; i++)
                    layerNodes.Add(NodeType.Boss);
            }
            else
            {
                // 나머지 층들: 임시로 일반 전투 배치 (나중에 비율로 수정)
                for (int i = 0; i < layerNodeCounts[layer]; i++)
                    layerNodes.Add(NodeType.MinorEnemy);
            }
            
            result.Add(layerNodes);
        }
        
        // 2. 비율에 따른 노드 타입 재배치
        AssignNodesByRatio(result, layerNodeCounts);
        
        // 3. 엘리트는 4층부터만 등장하도록 제한
        RestrictEliteToLayer4Plus(result);
        
        // 4. 연속 배치 규칙 검증 및 수정
        ValidateAndFixConsecutivePlacement(result);
        
        return result;
    }

    private static void AssignNodesByRatio(List<List<NodeType>> mapLayers, List<int> layerNodeCounts)
    {
        // 확정 노드 제외하고 배정할 노드 수 계산
        int totalNodes = layerNodeCounts.Sum();
        int confirmedNodes = layerNodeCounts[START_LAYER] + layerNodeCounts[FIRST_BATTLE_LAYER] + 
                           layerNodeCounts[CONFIRMED_STORE_LAYER] + layerNodeCounts[BOSS_LAYER];
        int availableNodes = totalNodes - confirmedNodes;
        
        if (availableNodes <= 0)
        {
            Debug.Log("확정 노드만으로 맵이 구성됨");
            return;
        }
        
        // 비율 계산 (소수점 반올림)
        int eliteCount = Mathf.RoundToInt(availableNodes * ELITE_RATIO);
        int storeCount = Mathf.RoundToInt(availableNodes * STORE_RATIO);
        int eventCount = Mathf.RoundToInt(availableNodes * EVENT_RATIO);
        int battleCount = availableNodes - (eliteCount + storeCount + eventCount);
        
        Debug.Log($"맵 노드 배정: 총 {availableNodes}개 중 엘리트({eliteCount}) 상점({storeCount}) 이벤트({eventCount}) 일반전투({battleCount})");
        
        // 노드 타입 풀 생성
        var nodePool = new List<NodeType>();
        for (int i = 0; i < eliteCount; i++) nodePool.Add(NodeType.EliteEnemy);
        for (int i = 0; i < storeCount; i++) nodePool.Add(NodeType.Store);
        for (int i = 0; i < eventCount; i++) nodePool.Add(NodeType.Event);
        for (int i = 0; i < battleCount; i++) nodePool.Add(NodeType.MinorEnemy);
        
        // 랜덤 섞기
        nodePool.Shuffle();
        
        // 확정 노드가 아닌 층에 배정
        int poolIndex = 0;
        for (int layer = 0; layer < TOTAL_LAYERS; layer++)
        {
            if (layer == START_LAYER || layer == FIRST_BATTLE_LAYER || 
                layer == CONFIRMED_STORE_LAYER || layer == BOSS_LAYER)
                continue; // 확정 노드 층은 건너뛰기
                
            for (int nodeIndex = 0; nodeIndex < mapLayers[layer].Count && poolIndex < nodePool.Count; nodeIndex++)
            {
                mapLayers[layer][nodeIndex] = nodePool[poolIndex];
                poolIndex++;
            }
        }
    }

    private static void RestrictEliteToLayer4Plus(List<List<NodeType>> mapLayers)
    {
        // 4층 미만에서 엘리트 발견시 일반 전투로 교체
        for (int layer = 0; layer < ELITE_START_LAYER && layer < mapLayers.Count; layer++)
        {
            for (int nodeIndex = 0; nodeIndex < mapLayers[layer].Count; nodeIndex++)
            {
                if (mapLayers[layer][nodeIndex] == NodeType.EliteEnemy)
                {
                    mapLayers[layer][nodeIndex] = NodeType.MinorEnemy;
                    Debug.Log($"엘리트 전투를 {layer}층에서 일반 전투로 교체");
                }
            }
        }
    }

    private static void ValidateAndFixConsecutivePlacement(List<List<NodeType>> mapLayers)
    {
        // 엘리트와 상점은 연속으로 붙는거 검증
        for (int layer = 0; layer < TOTAL_LAYERS - 1; layer++)
        {
            for (int nodeIndex = 0; nodeIndex < mapLayers[layer].Count; nodeIndex++)
            {
                NodeType currentType = mapLayers[layer][nodeIndex];
                
                // 엘리트나 상점인 경우 다음 층 검사
                if (currentType == NodeType.EliteEnemy || currentType == NodeType.Store)
                {
                    // 다음 층의 모든 노드 검사 (연결 고려 안함 - 갈림길이므로)
                    for (int nextNodeIndex = 0; nextNodeIndex < mapLayers[layer + 1].Count; nextNodeIndex++)
                    {
                        NodeType nextType = mapLayers[layer + 1][nextNodeIndex];
                        
                        // 금지된 연속 배치 발견시 교체
                        if ((currentType == NodeType.EliteEnemy && nextType == NodeType.Store) ||
                            (currentType == NodeType.Store && nextType == NodeType.EliteEnemy) ||
                            (currentType == NodeType.Store && nextType == NodeType.Store))
                        {
                            SwapWithCompatibleNode(mapLayers, layer + 1, nextNodeIndex);
                        }
                    }
                }
            }
        }
    }

    private static void EnsureDiverseChoices(List<List<NodeType>> mapLayers)
    {
        // 갈림길에서 선택지는 서로 달라야 함 (확정 발판 제외)
        for (int layer = 0; layer < TOTAL_LAYERS; layer++)
        {
            // 확정 발판 층은 제외
            if (layer == START_LAYER || layer == FIRST_BATTLE_LAYER || 
                layer == CONFIRMED_STORE_LAYER || layer == BOSS_LAYER)
                continue;
                
            // 해당 층에 동일한 노드 타입이 2개 이상 있으면 다양성 부족
            var typeCounts = mapLayers[layer].GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count());
            
            foreach (var typeCount in typeCounts)
            {
                if (typeCount.Value > 1 && mapLayers[layer].Count > 1)
                {
                    DiversifyLayerChoices(mapLayers[layer]);
                    break;
                }
            }
        }
    }

    private static void DiversifyLayerChoices(List<NodeType> layerNodes)
    {
        // 가능한 노드 타입들
        var possibleTypes = new List<NodeType> 
        { 
            NodeType.MinorEnemy, NodeType.EliteEnemy, 
            NodeType.Store, NodeType.Event 
        };
        
        // 현재 층에 없는 타입들 찾기
        var unusedTypes = possibleTypes.Where(t => !layerNodes.Contains(t)).ToList();
        
        if (unusedTypes.Count > 0)
        {
            // 첫 번째 중복 노드를 다른 타입으로 교체
            var duplicateType = layerNodes.GroupBy(t => t).First(g => g.Count() > 1).Key;
            int firstDuplicateIndex = layerNodes.IndexOf(duplicateType);
            layerNodes[firstDuplicateIndex] = unusedTypes[Random.Range(0, unusedTypes.Count)];
        }
    }

    private static void SwapWithCompatibleNode(List<List<NodeType>> mapLayers, int layer, int problemNodeIndex)
    {
        NodeType problemType = mapLayers[layer][problemNodeIndex];
        
        // 같은 층에서 일반 전투나 이벤트 노드와 교체
        for (int i = 0; i < mapLayers[layer].Count; i++)
        {
            if (i != problemNodeIndex)
            {
                NodeType candidateType = mapLayers[layer][i];
                if (candidateType == NodeType.MinorEnemy || candidateType == NodeType.Event)
                {
                    // 교체
                    mapLayers[layer][problemNodeIndex] = candidateType;
                    mapLayers[layer][i] = problemType;
                    Debug.Log($"{layer}층에서 연속 배치 문제 해결: {problemType} <-> {candidateType} 교체");
                    break;
                }
            }
        }
    }

    private static Map CreateMapFromStructure(MapConfig config, List<int> layerNodeCounts, List<List<NodeType>> nodeTypes)
    {
        List<Node> allNodes = new List<Node>();
        
        // 노드 생성 및 배치
        for (int layer = 0; layer < TOTAL_LAYERS; layer++)
        {
            for (int nodeIndex = 0; nodeIndex < layerNodeCounts[layer]; nodeIndex++)
            {
                NodeType nodeType = nodeTypes[layer][nodeIndex];
                string blueprintName = GetBlueprintName(config, nodeType);
                
                Node node = new Node(nodeType, blueprintName, new Vector2Int(nodeIndex, layer))
                {
                    // 노드 위치 계산 (나중에 MapView에서 중앙 정렬 처리)
                    position = new Vector2(nodeIndex * 3f - (layerNodeCounts[layer] - 1) * 1.5f, layer * 4f)
                };
                
                allNodes.Add(node);
            }
        }
        
        // 경로 연결 생성 (모든 노드를 다음 층 모든 노드와 연결)
        GenerateFullConnections(allNodes, layerNodeCounts);
        
        Debug.Log($"맵 생성 완료: {allNodes.Count}개 노드, {TOTAL_LAYERS}개 층");
        return new Map(config.name, "Boss", allNodes, new List<Vector2Int>());
    }

    private static string GetBlueprintName(MapConfig config, NodeType nodeType)
    {
        var blueprint = config.nodeBlueprints.FirstOrDefault(b => b.nodeType == nodeType);
        return blueprint?.name ?? nodeType.ToString();
    }

    private static void GenerateFullConnections(List<Node> allNodes, List<int> layerNodeCounts)
    {
        // 각 층의 모든 노드를 다음 층의 모든 노드와 연결
        for (int layer = 0; layer < TOTAL_LAYERS - 1; layer++)
        {
            var currentLayerNodes = allNodes.Where(n => n.point.y == layer).ToList();
            var nextLayerNodes = allNodes.Where(n => n.point.y == layer + 1).ToList();
            
            foreach (var currentNode in currentLayerNodes)
            {
                foreach (var nextNode in nextLayerNodes)
                {
                    currentNode.AddOutgoing(nextNode.point);
                    nextNode.AddIncoming(currentNode.point);
                }
            }
        }
        
        Debug.Log("모든 노드 연결 완료");
    }
}