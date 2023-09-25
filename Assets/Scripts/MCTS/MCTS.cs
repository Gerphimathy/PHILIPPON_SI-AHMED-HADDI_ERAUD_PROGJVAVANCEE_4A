using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace MCTS
{
    public struct Action
    {
        Paddle.MovementAxis _direction;

        public Action(Paddle.MovementAxis direction)
        {
            _direction = direction;
        }
    }
    public class MCTS
    {
        private const float _explorationFactor = .8f;
        private const int _nbSearch = 20;
        public const int nbSimulation = 20;
        public const float deltaTime = 1 / 60f;

        private GameState gameState => _root.GameState;
        private MCTSNode _root;
        private List<MCTSNode> _allNodes;

        public MCTS(GameState gameState)
        {
            _root = new(gameState);
        }
        public Action BestMove()
        {
            for (int it = 0; it < _nbSearch; it++)
            {
                MCTSNode current = new(gameState);
                MCTSNode explored = Select();
                MCTSNode expanded = explored.Expand();
                explored.Simulate();
                expanded.BackPropagation();
            }
            MCTSNode best = MaxValue(_root.Childrens);
            Assert.IsNotNull(best);
            return best.ParentAction;
            Assert.IsTrue(false);
            return new Action(Paddle.MovementAxis.X);
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
            return RandomValue(_allNodes);
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
    }
}
