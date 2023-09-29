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
        private List<Action> _availableActions;
        //When we'll firstly try to get the score of the root for selecting bestNode on very first iteration we'll have a total score = to zero
        public float Score => _total != 0 ? (_wins / _total) : float.MinValue + 1;


        public MCTSNode(GameState gameState)
        {
            _gameState = new GameState(gameState);
        }
        public MCTSNode(MCTSNode parent, Action parentAction,bool isP1) : this(parent.GameState)
        {
            this._parent = parent;
            this._parentAction = parentAction;
            parent._childrens.Add(this);
            this._depth = parent._depth + 1;
            this._gameState.Tick(isP1 ? parentAction : Action.None, !isP1 ? parentAction : Action.None, MCTSPlayer.deltaTime);
        }
        public IEnumerable<Action> GetPossibleActions(bool isP1)
        {
            return _gameState.GetPossibleActions(isP1, MCTSPlayer.deltaTime);
        }

        public MCTSNode Expand(bool? forcePlayer)
        {
            bool player = forcePlayer ?? _depth % 2 == 0;
            //If never expanded i.e we never determined possible actions we do it now but we'll keep the list for the other steps, on a given game state, available actions won't change
            _availableActions ??= GetPossibleActions(player).ToList();
            for (int i = 0; i < _availableActions.Count; i++)
            {
                foreach (var c in _childrens)
                {
                    if (c._parentAction == _availableActions[i])
                    {
                        _availableActions.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            //Debug.Log("Exanding from" + this.Expanded + "," + string.Join(',', this.Childrens.Select(s => s._parentAction.ToString())) + ";;;;" + string.Join(',', GetPossibleActions(player)) + ",," + this._parentAction + " with available actions " + _availableActions.Count);
            Assert.IsTrue(_availableActions != null && _availableActions.Count != 0);
            var son = new MCTSNode(this, MCTSPlayer.RandomValue(_availableActions),player);
            if (_availableActions.Count <= 1 || (son.GameState.GameStatus != GameState.GameStatusEnum.Ongoing))
                _expanded = true;
            return son;
        }

        internal void Simulate(bool isP1)
        {

            for (int sim = 0; sim < MCTSPlayer.nbSimulation; sim++)
            {
                var currentSim = this._gameState;
                while (currentSim.GameStatus == GameState.GameStatusEnum.Ongoing)
                {
                    var a1 = p1.GetValidAction(currentSim, true, MCTSPlayer.deltaTime);
                    var a2 = p2.GetValidAction(currentSim, false, MCTSPlayer.deltaTime);
                    currentSim.Tick(a1, a2, MCTSPlayer.deltaTime);
                }
                //Debug.Log(_total + " total");
                _total += currentSim.InitialTimer;
                if (currentSim.GameStatus != GameState.GameStatusEnum.Draw)
                {
                    //Debug.Log(currentSim.Timer+", "+this.Score+",,"+_wins+" : "+ currentSim.GameStatus);
                    //Win();
                    if (isP1)
                    {
                        if (currentSim.GameStatus == GameState.GameStatusEnum.Player1Win)
                            Win(ref currentSim);
                        else
                            Lose(ref currentSim);
                    }
                    else
                    {
                        if (currentSim.GameStatus == GameState.GameStatusEnum.Player2Win)
                            Win(ref currentSim);
                        else
                            Lose(ref currentSim);
                    }
                    //Debug.Log(currentSim.Timer+", "+this.Score);
                }
            }
        }
        void Win(ref GameState sim)
        {
            // We won
            //Debug.Log("Won " + sim.Timer);
            _wins += sim.Timer;
        }
        void Lose(ref GameState sim)
        {
            //Debug.Log("Removing " + _gameState.Timer);
            //We lost
            _wins -= 0;
            _wins -=sim.Timer;
        }

        internal void BackPropagation()
        {
            BackPropagation(this._wins, this._total);
        }
        private void BackPropagation(float wins, float total)
        {
            //Debug.Log(this.Score + " current propagated score");
            if (_parent == null)
                return;
            _parent._wins += wins;
            _parent._total += total;
            _parent.BackPropagation(_wins, _total);
        }

        public MCTSNode Parent { get => _parent; }
        public Action ParentAction { get => _parentAction; }
        public GameState GameState { get => _gameState; }
        public List<MCTSNode> Childrens { get => _childrens; }
        public bool Expanded { get => _expanded; }
    }
}
