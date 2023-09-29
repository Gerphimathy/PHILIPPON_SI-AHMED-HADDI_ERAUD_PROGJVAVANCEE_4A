using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Pong;
using System.Linq;

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
        private static float _explorationFactor = .5f;
        private static int _nbSearch = 200;
        public static int nbSimulation = 30;
        public static float deltaTime = 1 / 10f;

        private GameState gameState => _root.GameState;
        private MCTSNode _root;
        private List<MCTSNode> _allNodes;

        public Action BestMove()
        {
            for (int it = 0; it < _nbSearch; it++)
            {
                MCTSNode ex = Select();
                MCTSNode expanded = ex.Expand(ex == _root ? isP1 : null);
                _allNodes.Add(expanded);
                if (ex.Expanded)
                    _allNodes.Remove(ex);
                expanded.Simulate(isP1);
                expanded.BackPropagation();
            }
            MCTSNode best = MaxValue(_root.Childrens);
            Debug.LogWarning("Optimal is : "+ best + " : " + best.Score);
            return best.ParentAction;
            return Action.None;
        }
        public MCTSNode Select()
        {
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
            if (en.Count == 1)
                return en[0];
            Assert.IsTrue(en.Any(e => !float.IsNaN(e.Score)));
            var max = float.MinValue;
            var maxInd = -1;
            for (int i = 0; i < en.Count; i++)
            {
                if (en[i].Score > max)
                {
                    max = en[i].Score;
                    maxInd = i;
                }
            }
            return en[maxInd];
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

        public void SetSettings(MCTSSettings mctsSettings)
        {
            _explorationFactor = mctsSettings.explorationFactor;
            _nbSearch = mctsSettings.nbSearch;
            nbSimulation = mctsSettings.nbSimulation;
            deltaTime = mctsSettings.deltaTime;
        }
    }
}
