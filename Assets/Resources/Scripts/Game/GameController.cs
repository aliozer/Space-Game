using AO.Input;
using AO.SpaceGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.SpaceGame
{

    public class GameController : MonoBehaviour
    {

        [SerializeField]
        private SpaceshipController _spaceShip;
        public SpaceshipController Spaceship { get => _spaceShip; set => _spaceShip = value; }

        [SerializeField]
        private SpaceshipController _ghostSpaceShip;
        public SpaceshipController GhostSpaceship { get => _ghostSpaceShip; set => _ghostSpaceShip = value; }
        [SerializeField]
        private BaseSpaceshipInput _input;
        public BaseSpaceshipInput Input => _input;
        [SerializeField]
        private UIController _uiController;
        public UIController UIController => _uiController;
        [SerializeField]
        private AstroidController _astroidController;
        public AstroidController AstroidController => _astroidController;


        private GameState state;
        public GameState State {
            get { return state; }
            set {
                if (state != null)
                    state.Stop();
                state = value;
                state.Start();
            }
        }

        private void Start()
        {

            State = new WaitingGameState(this);
        }

        private void Update()
        {
            if (State != null)
                State.Update();
        }

    }
}
