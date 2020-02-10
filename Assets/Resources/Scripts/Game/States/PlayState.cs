using AO.SpaceGame.UI;
using AO.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AO.SpaceGame
{
    public class PlayState : GameState
    {
        private WeaponAttackType _currentAttackType = WeaponAttackType.Single;
        private float _currentFireRate = 2.0f;
        private float _currentFireSpeedFactor = 1.0f;
        private SpaceshipController _ghostSpaceShip;
        private SpaceshipController _spaceShip;

        private List<ToggleButton> _toggles = new List<ToggleButton>();

        public PlayState(GameController controller) : base(controller) { }


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

        }

        public override void FixedUpdate()
        {
            if (_ghostSpaceShip && _spaceShip)
            {
                Vector3 _currentVelocity = Vector3.zero;
                _ghostSpaceShip.transform.position = Vector3.SmoothDamp(_ghostSpaceShip.transform.position, _spaceShip.transform.position + _spaceShip.transform.up * 4f, ref _currentVelocity, Time.deltaTime * 5f);
                _ghostSpaceShip.transform.rotation =  Quaternion.Slerp(_ghostSpaceShip.transform.rotation, _spaceShip.transform.rotation, Time.deltaTime * 20f);
            }

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
                var spaceshipPrefab = PrefabUtil.Create<SpaceshipController>("GhostSpaceship");
                _ghostSpaceShip = GameObject.Instantiate(spaceshipPrefab, _spaceShip.transform.position + _spaceShip.transform.up, _spaceShip.transform.rotation);
                _ghostSpaceShip.Input = Controller.Input;

                if (Controller.UIController.CrossToggle.IsOn)
                    _ghostSpaceShip.TopAssaultWeapon = GetAssaultWeapon<SpaceshipTopAssaultWeapon>(_ghostSpaceShip.transform, "GhostSpaceshipTopAssaultWeapon");

                _ghostSpaceShip.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
                _ghostSpaceShip.ChangeWeaponFireRates(_currentFireRate);
                _ghostSpaceShip.ChangeWeaponAttackTypes(_currentAttackType);
            }
            else
            {
                GameObject.Destroy(_ghostSpaceShip.gameObject);
                _ghostSpaceShip = null;
            }
        }

        private void ChangeWeaponsFireSpeedFactors(bool isEnabled)
        {
            _currentFireSpeedFactor = isEnabled ? 1.5f : 1.0f;
            _spaceShip.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
            if (_ghostSpaceShip)
                _ghostSpaceShip.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
        }

        private void IncreaseFireRate(bool enabled)
        {
            _currentFireRate = enabled ? 1.0f : 2.0f;
            _spaceShip.ChangeWeaponFireRates(_currentFireRate);
            if (_ghostSpaceShip)
                _ghostSpaceShip.ChangeWeaponFireRates(_currentFireRate);
        }

        private void ChangeAttackType(bool isBrust)
        {
            _currentAttackType = isBrust ? WeaponAttackType.Brust : WeaponAttackType.Single;
            _spaceShip.ChangeWeaponAttackTypes(_currentAttackType);
            if (_ghostSpaceShip)
                _ghostSpaceShip.ChangeWeaponAttackTypes(_currentAttackType);
        }

        private void ChangeCrossWeapon(bool isEnabled)
        {
            if (isEnabled)
            {
                _spaceShip.TopAssaultWeapon = GetAssaultWeapon<SpaceshipTopAssaultWeapon>(_spaceShip.transform);
                if (_ghostSpaceShip)
                    _ghostSpaceShip.TopAssaultWeapon = GetAssaultWeapon<SpaceshipTopAssaultWeapon>(_ghostSpaceShip.transform, "GhostSpaceshipTopAssaultWeapon");
            }
            else if (_spaceShip.TopAssaultWeapon)
            {
                GameObject.DestroyImmediate(_spaceShip.TopAssaultWeapon.gameObject);
                if (_ghostSpaceShip)
                    GameObject.DestroyImmediate(_ghostSpaceShip.TopAssaultWeapon.gameObject);
            }
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

        private T GetAssaultWeapon<T>(Transform parent, string name = "") where T : SpaceshipAssaultWeapon, new()
        {
            T prefab = PrefabUtil.Create<T>(string.IsNullOrEmpty(name) ? typeof(T).Name : name);

            if (prefab == null)
                return null;

            var weapon = GameObject.Instantiate(prefab);
            weapon.transform.parent = parent;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.AttackType = _currentAttackType;
            weapon.FireRate = _currentFireRate;
            weapon.SpeedFactor = _currentFireSpeedFactor;
            return weapon;
        }

        private void UIController_Exit()
        {
            Controller.State = new WaitingGameState(Controller);
        }

        public override void Stop()
        {
            _toggles.Clear();
            Controller.UIController.Exit -= UIController_Exit;

            Controller.Camera.transform.parent = null;
            Controller.Camera.transform.position =  new Vector3(0f, _cameraHeight, _cameraDistance);
            Controller.Camera.transform.rotation = Quaternion.identity;

            if (_spaceShip)
                GameObject.Destroy(_spaceShip.gameObject);

            if (_ghostSpaceShip)
            {
                GameObject.Destroy(_ghostSpaceShip.gameObject);
                _ghostSpaceShip = null;
            }
        }
    }
}
