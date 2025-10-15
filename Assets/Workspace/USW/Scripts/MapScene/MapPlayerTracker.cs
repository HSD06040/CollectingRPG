using System;
using System.Linq;
using DG.Tweening;
using map;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : InGameSingleton<MapPlayerTracker>
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;        
        public UnitManager unitManager;       
        public MapView view;

        // 각 이벤트가 끝날때 호출
        public static Action OnEventEnded;

        public bool Locked { get; set; }

        protected override void Awake()
        {
            base.Awake();
            OnEventEnded += Unlock;
        }        

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                Vector2Int currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                Node currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            if (mapNode.Node.nodeType == NodeType.Store)
            {
                EnterNode(mapNode);
            }
            else
            {
                DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
            }
        }

        private static void EnterNode(MapNode mapNode)
        {
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            Manager.Data.StageGameData.CurrentFloor.Value++;

            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                case NodeType.EliteEnemy:
                case NodeType.Boss:
                    if (Instance != null)
                    {
                        Instance.Locked = true;
                        Instance.unitManager.EnemyController.SetUnit(mapNode.Node.gridData);
                        Instance.unitManager.GameStandby();                        
                    }
                    break;                
                case NodeType.Store:
                    if (Instance != null)
                        Instance.Locked = true;

                    if (StageShopPanel.Instance != null)
                    {
                        StageShopPanel.Instance.OpenShop();
                    }
                    break;                                   
                case NodeType.Event:
                    if(Instance != null)
                        Instance.Locked = true;

                    if(EventPanel.Instance != null)
                    {
                        EventPanel.Instance.ShowEvent(mapNode.Node.eventData);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }

        private void Lock() { Locked = true; }
        private void Unlock() { Locked = false; }
    }
}