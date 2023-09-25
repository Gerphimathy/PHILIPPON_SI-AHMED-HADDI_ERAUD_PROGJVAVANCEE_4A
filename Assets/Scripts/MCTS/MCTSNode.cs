using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MCTS
{
    public class MCTSNode
    {
        private MCTSNode _parent;
        private List<MCTSNode> _childrens;
        private GameState _gameState;
        private Action _parentAction;
        private bool _expanded = false;
        private int _wins;
        private int _total;
        public float Ratio => _wins / (float)_total;

        public MCTSNode(GameState gameState)
        {
            _gameState = gameState;
        }
        public MCTSNode(MCTSNode node, Action parentAction) : this(node.GameState)
        {
            this._parent = node;
            this._parentAction = parentAction;
            node._childrens.Add(this);
        }
        public List<Action> GetPossibleActions(int playerIndex)
        {
            if(playerIndex == 0)
            {

            }
            else
            {

            }
            Debug.LogError("Placeholder");
            return new List<Action>(){ new Action(Paddle.MovementAxis.X), new Action(Paddle.MovementAxis.Z) };
        }

        public MCTSNode Expand()
        {
            var actions = GetPossibleActions(0);
            Assert.IsTrue(actions != null && actions.Count != 0);
            var son = new MCTSNode(this, MCTS.RandomValue(actions));
            if (actions.Count == 1)
                _expanded = true;
            return son;
        }

        internal void Simulate()
        {
            for (int sim = 0; sim < MCTS.nbSimulation; sim++)
            {
                while (!_gameState.HasGameEnded)
                {
                    var a1 = MCTS.RandomValue(GetPossibleActions(0));
                    var a2 = MCTS.RandomValue(GetPossibleActions(1));
                    _gameState.Tick(a1, a2, MCTS.deltaTime);
                }
            }
        }

        internal void BackPropagation()
        {
            BackPropagation(this._wins, this._total);
        }
        private void BackPropagation(int wins,int total)
        {
            if (_parent == null)
                return;
            _parent.BackPropagation(this._wins, this._total);
        }

        public MCTSNode Parent { get => _parent; }
        public Action ParentAction { get => _parentAction; }
        public GameState GameState { get => _gameState; }
        public List<MCTSNode> Childrens { get => _childrens; }
        public bool Expanded { get => _expanded; }
    }
}
