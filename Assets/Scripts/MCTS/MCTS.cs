using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Pong;

namespace MCTS
{
    public class MCTSPlayer : APlayer
    {
        public MCTSPlayer(bool isP1) : base(isP1)
        {

        }
        private void Init(GameState gameState)
        {
            _root = new(gameState);
            _allNodes = new List<MCTSNode>();
            _allNodes.Add(_root);
        }
        private static float _explorationFactor;
        private static int _nbSearch;
        public static int nbSimulation;
        public static float deltaTime;

        private GameState gameState => _root.GameState;
        private MCTSNode _root;
        private List<MCTSNode> _allNodes;

        public Action BestMove()
        {
            for (int it = 0; it < _nbSearch; it++)
            {
                MCTSNode explored = Select();
                MCTSNode expanded = explored.Expand(explored == _root ? isP1 : null);
                _allNodes.Add(expanded);
                if (explored.Expanded)
                    _allNodes.Remove(explored);
                explored.Simulate(isP1);
                expanded.BackPropagation();
            }
            MCTSNode best = MaxValue(_root.Childrens);
            Assert.IsNotNull(best);
            return best.ParentAction;
            Assert.IsTrue(false);
            return Action.None;
        }
        public MCTSNode Select()
        {
            /*_allNodes = new List<MCTSNode>();
            ExploreTree(_root, _allNodes);*/
            if (UnityEngine.Random.Range(0, 1f) < _explorationFactor)
            {
                return RandomNode();
            }
            else
            {
                return BestNode();
            }
        }

        private MCTSNode BestNode()
        {
            return MaxValue(_allNodes);
        }
        public static MCTSNode MaxValue(IList<MCTSNode> en)
        {
            return en.OrderByDescending(n => n.Score).First();
        }
        private MCTSNode RandomNode()
        {
            return RandomValue(_allNodes);
        }

        public static T RandomValue<T>(IList<T> en)
        {
            return en[Random.Range(0, en.Count)];
        }

        private void ExploreTree(MCTSNode root, List<MCTSNode> visited)
        {
            Assert.IsTrue(visited != null);
            if (!root.Expanded)
                visited.Add(root);
            foreach (var node in root.Childrens)
            {
                ExploreTree(node, visited);
            }
        }

        public override Action GetAction(ref GameState gameState)
        {
            Init(gameState);
            return BestMove();
        }

        public static void SetSettings(MCTSSettings mctsSettings)
        {
            _explorationFactor = mctsSettings.explorationFactor;
            _nbSearch = mctsSettings.nbSearch;
            nbSimulation = mctsSettings.nbSimulation;
            deltaTime = mctsSettings.deltaTime;
        }
    }
}
