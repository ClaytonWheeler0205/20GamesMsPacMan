using Godot;
using Util.ExtensionMethods;

namespace Game.Ghosts
{

    public class GhostStateMachineImpl : GhostStateMachine
    {
        private Godot.Collections.Dictionary<string, GhostState> _states;
        [Export]
        private NodePath _initialStatePath;
        private GhostState _initialState;
        private GhostState _currentState;

        public override void _Ready()
        {
            InitializeStatesDictionary();
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            _currentState = _initialState;
        }

        private void InitializeStatesDictionary()
        {
            _states = new Godot.Collections.Dictionary<string, GhostState>();
            for (int i = 0; i < GetChildCount(); i++)
            {
                Node child = GetChild(i);
                if (child.IsValid() && child is GhostState state)
                {
                    _states[state.Name.ToLower()] = state;
                }
            }
        }

        private void SetNodeReferences()
        {
            _initialState = GetNode<GhostState>(_initialStatePath);
        }

        private void CheckNodeReferences()
        {
            if (!_initialState.IsValid())
            {
                GD.PrintErr("ERROR: Ghost State Machine Initial State is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            foreach (GhostState state in _states.Values)
            {
                state.Connect("Transitioned", this, nameof(OnStateTransitioned));
            }
        }

        public override void _Process(float delta)
        {
            if (IsMachineActive)
            {
                _currentState.UpdateState(delta);
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (IsMachineActive)
            {
                _currentState.PhysicsUpdateState(delta);
            }
        }


        public override void SetIsMachineActive(bool isActive)
        {
            IsMachineActive = isActive;
            if (isActive)
            {
                _currentState.EnterState();
            }
        }

        public void OnStateTransitioned(GhostState currentState, string newStateName)
        {
            if (currentState != _currentState)
            {
                return;
            }
            else
            {
                _currentState.ExitState();
                GhostState newState = _states[newStateName];
                if (newState.IsValid())
                {
                    _currentState = newState;
                    _currentState.EnterState();
                }
            }
        }

    }
}