using AO.SpaceGame.UI;
using AO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AO.SpaceGame
{

    public class PlayState : GameState
    {
        private SpaceshipController _spaceShip;
        private AbilityEvaluator _evaluator = new AbilityEvaluator();
        private float _cameraDistance = -20f;
        private float _cameraHeight = 6f;

        public PlayState(GameController controller) : base(controller) { }

        public override void Start()
        {
            Controller.Input.enabled = true;

            var spaceshipPrefab = PrefabUtil.Create<SpaceshipController>("Spaceship");
            _spaceShip = GameObject.Instantiate(spaceshipPrefab, Vector3.zero, Quaternion.identity);
            _spaceShip.Input = Controller.Input;
            _spaceShip.StartEngine();

            PrepareCamera();

            Controller.AstroidController.Initialize();
            Controller.UIController.Exit += UIController_Exit;
            Controller.UIController.AbilityController.Changed += AbilityController_Changed;

        }

        private void AbilityController_Changed(IUIAbility ability, bool value)
        {
            if (value)
            {
                var newAbility = (IAbility)Activator.CreateInstance(ability.GetAbilityType(), new object[] { _spaceShip });
                _evaluator.Add(newAbility);
            }
            else
                _evaluator.Remove(ability.GetAbilityType());
        }

        public override void Update()
        {
            
            if (_evaluator != null)
                _evaluator.Update();
        }

        public override void FixedUpdate()
        {
            if (_spaceShip)
                UpdateCameraPositionAndRotation();
        }

        private void PrepareCamera()
        {

            GameObject cameraContainer = new GameObject("Camera");
            Controller.Camera.transform.parent = cameraContainer.transform;
            cameraContainer.transform.parent = _spaceShip.transform;
            Controller.Camera.transform.localPosition = new Vector3(0f, _cameraHeight, _cameraDistance);
        }


        private void UpdateCameraPositionAndRotation()
        {

            float smoothFactor = 2.0f;
            float angle = 18f;

            var cameraEuler = Quaternion.Euler(0f, 0f, Controller.Input.Roll * angle);
            Controller.Camera.transform.localRotation = Quaternion.Lerp(Controller.Camera.transform.localRotation, cameraEuler, Time.deltaTime * smoothFactor);

            var cameraParentEuler = Quaternion.Euler(-Controller.Input.Picth * angle, -Controller.Input.Yaw * angle, 0f);
            Controller.Camera.transform.parent.localRotation = Quaternion.Lerp(Controller.Camera.transform.parent.localRotation, cameraParentEuler, Time.deltaTime * smoothFactor);

            Controller.Camera.transform.localPosition = Vector3.Lerp(Controller.Camera.transform.localPosition, new Vector3(0f, _cameraHeight, _cameraDistance - (angle * Controller.Input.Throttle)), Time.deltaTime * smoothFactor);
        }

        private void UIController_Exit()
        {
            Controller.State = new WaitingGameState(Controller);
        }

        public override void Stop()
        {
            _evaluator.Clear();
            Controller.UIController.Exit -= UIController_Exit;

            Controller.Camera.transform.parent = null;
            Controller.Camera.transform.position = new Vector3(0f, _cameraHeight, _cameraDistance);
            Controller.Camera.transform.rotation = Quaternion.identity;

            if (_spaceShip)
                GameObject.Destroy(_spaceShip.gameObject);

        }
    }
}
