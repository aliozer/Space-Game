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
        private WeaponAttackType _currentAttackType = WeaponAttackType.Single;
        private float _currentFireRate = 2.0f;
        private float _currentFireSpeedFactor = 1.0f;
        private SpaceshipController _spaceShip;

        private List<ToggleButton> _toggles = new List<ToggleButton>();

        public PlayState(GameController controller) : base(controller) { }

        private AbilityEvaluator _evaluator = new AbilityEvaluator();

        private float _cameraDistance = -20f;
        private float _cameraHeight = 6f;

        public override void Start()
        {
            Controller.Input.enabled = true;

            var spaceshipPrefab = PrefabUtil.Create<SpaceshipController>("Spaceship");
            _spaceShip = GameObject.Instantiate(spaceshipPrefab, Vector3.zero, Quaternion.identity);
            _spaceShip.Input = Controller.Input;
            _spaceShip.StartEngine();


            PrepareCamera();

            Controller.AstroidController.Create();

            _toggles.Add(Controller.UIController.CrossToggle);
            _toggles.Add(Controller.UIController.DoubleToggle);
            _toggles.Add(Controller.UIController.FireRateToggle);
            _toggles.Add(Controller.UIController.SpeedToggle);
            _toggles.Add(Controller.UIController.GhostToggle);


            Controller.UIController.Exit += UIController_Exit;

        }

        public override void Update()
        {
            if (Controller.Input.GetPower1KeyDown())
            {
                Controller.UIController.CrossToggle.Toggle();
                ChangeCrossWeapon(Controller.UIController.CrossToggle.IsOn);
                CheckPowerButtonsInreractable();
            }
            else if (Controller.Input.GetPower2KeyDown())
            {
                Controller.UIController.DoubleToggle.Toggle();
                ChangeAttackType(Controller.UIController.DoubleToggle.IsOn);
                CheckPowerButtonsInreractable();
            }
            else if (Controller.Input.GetPower3KeyDown())
            {
                Controller.UIController.FireRateToggle.Toggle();
                IncreaseFireRate(Controller.UIController.FireRateToggle.IsOn);
                CheckPowerButtonsInreractable();
            }
            else if (Controller.Input.GetPower4KeyDown())
            {
                Controller.UIController.SpeedToggle.Toggle();
                ChangeWeaponsFireSpeedFactors(Controller.UIController.SpeedToggle.IsOn);
                CheckPowerButtonsInreractable();
            }
            else if (Controller.Input.GetPower5KeyDown())
            {
                Controller.UIController.GhostToggle.Toggle();
                ChangeGhostSpaceship(Controller.UIController.GhostToggle.IsOn);
                CheckPowerButtonsInreractable();
            }

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

        private void ChangeGhostSpaceship(bool isEnabled)
        {
            if (isEnabled)
            {
                var ability = new GhostSpaceshipAbility(_spaceShip, Controller.UIController.CrossToggle.IsOn);
                ability.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
                ability.ChangeWeaponFireRates(_currentFireRate);
                ability.ChangeWeaponAttackTypes(_currentAttackType);

                _evaluator.Add(ability);
            }
            else
                _evaluator.Remove<GhostSpaceshipAbility>();
        }

        private void ChangeWeaponsFireSpeedFactors(bool isEnabled)
        {
            _currentFireSpeedFactor = isEnabled ? 1.5f : 1.0f;
            if (isEnabled)
                _evaluator.Add(new FireSpeedWeaponAbility(_spaceShip, _currentFireSpeedFactor));
            else
                _evaluator.Remove<FireSpeedWeaponAbility>();
        }

        private void IncreaseFireRate(bool enabled)
        {
            _currentFireRate = enabled ? 1.0f : 2.0f;
            if (enabled)
                _evaluator.Add(new IncreaseFireRateWeaponAbility(_spaceShip, _currentFireRate));
            else
                _evaluator.Remove<IncreaseFireRateWeaponAbility>();
        }

        private void ChangeAttackType(bool isBrust)
        {
            _currentAttackType = isBrust ? WeaponAttackType.Brust : WeaponAttackType.Single;
            if (isBrust)
                _evaluator.Add(new AttackTypeWeaponAbility(_spaceShip, _currentAttackType));
            else
                _evaluator.Remove<AttackTypeWeaponAbility>();
        }

        private void ChangeCrossWeapon(bool isEnabled)
        {
            if (isEnabled)
            {
                var ability = new CrossWeaponAbility(_spaceShip);
                ability.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
                ability.ChangeWeaponFireRates(_currentFireRate);
                ability.ChangeWeaponAttackTypes(_currentAttackType);

                _evaluator.Add(ability);

            }
            else
                _evaluator.Remove<CrossWeaponAbility>();
        }

        private void CheckPowerButtonsInreractable()
        {
            int count = 0;
            foreach (var toggle in _toggles)
                count += toggle.IsOn ? 1 : 0;

            foreach (var toggle in _toggles)
            {
                if (!toggle.IsOn)
                    toggle.Interactable = !(count >= 3);
            }

        }

        private void UIController_Exit()
        {
            Controller.State = new WaitingGameState(Controller);
        }

        public override void Stop()
        {
            _evaluator.Clear();
            _toggles.Clear();
            Controller.UIController.Exit -= UIController_Exit;

            Controller.Camera.transform.parent = null;
            Controller.Camera.transform.position = new Vector3(0f, _cameraHeight, _cameraDistance);
            Controller.Camera.transform.rotation = Quaternion.identity;

            if (_spaceShip)
                GameObject.Destroy(_spaceShip.gameObject);

        }
    }
}
