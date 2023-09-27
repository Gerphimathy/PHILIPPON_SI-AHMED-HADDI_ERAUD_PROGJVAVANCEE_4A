using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Pong;

namespace MCTS
{
    public class MCTSPlayer : IPlayer
    {
        private const float _explorationFactor = .8f;
        private const int _nbSearch = 20;
        public const int nbSimulation = 20;
        public const float deltaTime = 1 / 60f;
        private bool isP1;

        private GameState gameState => _root.GameState;
        private MCTSNode _root;
        private List<MCTSNode> _allNodes;

        public MCTSPlayer(GameState gameState, bool isP1)
        {
            _root = new(gameState);
            _allNodes = new List<MCTSNode>();
            _allNodes.Add(_root);
            this.isP1 = isP1;
        }
        public Action BestMove()
        {
            for (int it = 0; it < _nbSearch; it++)
            {
                MCTSNode explored = Select();
                MCTSNode expanded = explored.Expand();
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
            _allNodes = new List<MCTSNode>();
            ExploreTree(_root, _allNodes);
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
            return en.OrderBy(n => n.Ratio).First();
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
            foreach (var node in root.Childrens.Where(n => !n.Expanded))
            {
                visited.Add(node);
                ExploreTree(node, visited);
            }
        }

        public Action GetAction()
        {
            return BestMove();
        }
    }
}
