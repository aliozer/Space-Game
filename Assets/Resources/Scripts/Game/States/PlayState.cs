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

        private List<ToggleButton> _toggles = new List<ToggleButton>();

        public PlayState(GameController controller) : base(controller) { }

        public override void Start()
        {
            Controller.Input.enabled = true;

            var spaceshipPrefab = PrefabUtil.Create<SpaceshipController>("Spaceship");
            Controller.Spaceship = GameObject.Instantiate(spaceshipPrefab, Vector3.zero, Quaternion.identity);
            Controller.Spaceship.Input = Controller.Input;

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

        private void ChangeGhostSpaceship(bool isEnabled)
        {
            if (isEnabled)
            {
                var spaceshipPrefab = PrefabUtil.Create<SpaceshipController>("GhostSpaceship");
                Controller.GhostSpaceship = GameObject.Instantiate(spaceshipPrefab, Controller.Spaceship.transform.position + Controller.Spaceship.transform.up * 3f, Controller.Spaceship.transform.rotation);
                Controller.GhostSpaceship.Input = Controller.Input;
                Controller.GhostSpaceship.SetVelocity(Controller.Spaceship.Velocity, Controller.Spaceship.AngularVelocity);

                if (Controller.UIController.CrossToggle.IsOn)
                    Controller.GhostSpaceship.TopAssaultWeapon = GetAssaultWeapon<SpaceshipTopAssaultWeapon>(Controller.GhostSpaceship.transform, "GhostSpaceshipTopAssaultWeapon");

                Controller.GhostSpaceship.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
                Controller.GhostSpaceship.ChangeWeaponFireRates(_currentFireRate);
                Controller.GhostSpaceship.ChangeWeaponAttackTypes(_currentAttackType);
            }
            else
            {
                GameObject.Destroy(Controller.GhostSpaceship.gameObject);
            }
        }

        private void ChangeWeaponsFireSpeedFactors(bool isEnabled)
        {
            _currentFireSpeedFactor = isEnabled ? 1.5f : 1.0f;
            Controller.Spaceship.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
            if (Controller.GhostSpaceship)
                Controller.GhostSpaceship.ChangeWeaponFireSpeedFactors(_currentFireSpeedFactor);
        }

        private void IncreaseFireRate(bool enabled)
        {
            _currentFireRate = enabled ? 1.0f : 2.0f;
            Controller.Spaceship.ChangeWeaponFireRates(_currentFireRate);
            if (Controller.GhostSpaceship)
                Controller.GhostSpaceship.ChangeWeaponFireRates(_currentFireRate);
        }

        private void ChangeAttackType(bool isBrust)
        {
            _currentAttackType = isBrust ? WeaponAttackType.Brust : WeaponAttackType.Single;
            Controller.Spaceship.ChangeWeaponAttackTypes(_currentAttackType);
            if (Controller.GhostSpaceship)
                Controller.GhostSpaceship.ChangeWeaponAttackTypes(_currentAttackType);
        }

        private void ChangeCrossWeapon(bool isEnabled)
        {
            if (isEnabled)
            {
                Controller.Spaceship.TopAssaultWeapon = GetAssaultWeapon<SpaceshipTopAssaultWeapon>(Controller.Spaceship.transform);
                if (Controller.GhostSpaceship)
                    Controller.GhostSpaceship.TopAssaultWeapon = GetAssaultWeapon<SpaceshipTopAssaultWeapon>(Controller.GhostSpaceship.transform, "GhostSpaceshipTopAssaultWeapon");
            }
            else if (Controller.Spaceship.TopAssaultWeapon)
            {
                GameObject.DestroyImmediate(Controller.Spaceship.TopAssaultWeapon.gameObject);
                if (Controller.GhostSpaceship)
                    GameObject.DestroyImmediate(Controller.GhostSpaceship.TopAssaultWeapon.gameObject);
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

            if (Controller.Spaceship)
                GameObject.Destroy(Controller.Spaceship.gameObject);

            if (Controller.GhostSpaceship)
                GameObject.Destroy(Controller.GhostSpaceship.gameObject);
        }
    }
}
