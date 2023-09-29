using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Pong;

namespace MCTS
{
    public class MCTSNode
    {
        private MCTSNode _parent;
        private List<MCTSNode> _childrens=new List<MCTSNode>();
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
        public static List<Action> GetPossibleActions(bool isP1)
        {
            if(isP1)
            {

            }
            else
            {

            }
            Debug.LogWarning("Placeholder");
            return new List<Action>() { Action.Up, Action.Down, Action.None };
        }

        public MCTSNode Expand()
        {
            var actions = GetPossibleActions(UnityEngine.Random.Range(0,2)==0);
            Assert.IsTrue(actions != null && actions.Count != 0);
            var son = new MCTSNode(this, MCTSPlayer.RandomValue(actions));
            if (actions.Count == 1)
                _expanded = true;
            return son;
        }

        internal void Simulate(bool isP1)
        {
            for (int sim = 0; sim < MCTSPlayer.nbSimulation; sim++)
            {
                while (_gameState.GameStatus != GameState.GameStatusEnum.Ongoing)
                {
                    var a1 = MCTSPlayer.RandomValue(GetPossibleActions(true));
                    var a2 = MCTSPlayer.RandomValue(GetPossibleActions(false));
                    _gameState.Tick(a1, a2, MCTSPlayer.deltaTime);
                }
                _total++;
                if(isP1 ^ _gameState.GameStatus == GameState.GameStatusEnum.Player2Win)
                {
                    Debug.LogWarning("To do, add draw");
                    _wins++;
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
