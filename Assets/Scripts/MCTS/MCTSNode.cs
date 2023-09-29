using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Pong;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;

namespace MCTS
{
    public class MCTSNode
    {
        private int _depth = 0;
        private MCTSNode _parent;
        private List<MCTSNode> _childrens = new List<MCTSNode>();
        private GameState _gameState;
        private Action _parentAction;
        private bool _expanded = false;
        private float _wins;
        private float _total;
        private RandomPlayer p1 = new RandomPlayer(true);
        private RandomPlayer p2 = new RandomPlayer(false);
        //When we'll firstly try to get the score of the root for selecting bestNode on very first iteration we'll have a total score = to zero
        public float Score => _total!=0 ? _wins / _total : float.MinValue+1;


        public MCTSNode(GameState gameState)
        {
            _gameState = gameState;
        }
        public MCTSNode(MCTSNode parent, Action parentAction) : this(parent.GameState)
        {
            this._parent = parent;
            this._parentAction = parentAction;
            parent._childrens.Add(this);
            this._depth = parent._depth + 1;
        }
        public IEnumerable<Action> GetPossibleActions(bool isP1)
        {
            return _gameState.GetPossibleActions(isP1, MCTSPlayer.deltaTime).ToList();
        }

        public MCTSNode Expand(bool? forcePlayer)
        {
            bool player=forcePlayer ?? _depth%2==0;
            var actions = GetPossibleActions(player).ToList();
            for (int i = 0; i < actions.Count; i++)
            {
                foreach(var c in _childrens)
                {
                    if (c._parentAction == actions[i])
                    {
                        actions.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            //Debug.Log("Exanding from" + this.Expanded + "," + string.Join(',', this.Childrens.Select(s=>s._parentAction.ToString()))+";;;;"+ string.Join(',', GetPossibleActions(player))+",," +this._parentAction+" with available actions "+actions.Count);
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
                    var a1 = p1.GetValidAction(ref _gameState, true, MCTSPlayer.deltaTime);
                    var a2 = p2.GetValidAction(ref _gameState, false, MCTSPlayer.deltaTime);
                    _gameState.Tick(a1, a2, MCTSPlayer.deltaTime);
                }
                _total += _gameState.InitialTimer;
                if (_gameState.GameStatus != GameState.GameStatusEnum.Draw)
                {
                    if (isP1 ^ _gameState.GameStatus == GameState.GameStatusEnum.Player2Win)
                    {
                        // We won
                        //Debug.Log("Adding " + _gameState.Timer);
                        _wins += _gameState.Timer;
                    }
                    else
                    {
                        //Debug.Log("Removing " + _gameState.Timer);
                        //We lost
                        _wins -= _gameState.Timer;
                    }
                }
            }
        }

        internal void BackPropagation()
        {
            BackPropagation(this._wins, this._total);
        }
        private void BackPropagation(float wins, float total)
        {
            if (_parent == null)
                return;
            _parent._wins += wins;
            _parent._total += total;
            _parent.BackPropagation(wins, total);
        }

        public MCTSNode Parent { get => _parent; }
        public Action ParentAction { get => _parentAction; }
        public GameState GameState { get => _gameState; }
        public List<MCTSNode> Childrens { get => _childrens; }
        public bool Expanded { get => _expanded; }
    }
}
