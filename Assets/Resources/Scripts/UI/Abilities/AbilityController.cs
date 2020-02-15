using System;
using System.Collections;
using System.Collections.Generic;
using AO.Extensions;
using AO.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AO.SpaceGame.UI
{

    public class AbilityController : MonoBehaviour
    {
        public event Action<IUIAbility, bool> Changed;

        private List<BaseUIAbility> _abilities = new List<BaseUIAbility>();

        [SerializeField]
        private BaseSpaceshipInput _input;

        private void Awake()
        {
            _abilities = new List<BaseUIAbility>()
            {
                new UIAbility<CrossWeaponAbility>(_input.GetPower1KeyDown),
                new UIAbility<AttackTypeWeaponAbility>(_input.GetPower2KeyDown),
                new UIAbility<IncreaseFireRateWeaponAbility>(_input.GetPower3KeyDown),
                new UIAbility<FireSpeedWeaponAbility>(_input.GetPower4KeyDown),
                new UIAbility<GhostSpaceshipAbility>(_input.GetPower5KeyDown)
            };
        }

        private void Update()
        {
            foreach (var item in _abilities)
            {
                item.Update();
            }
        }

        public void Ability_Changed(BaseUIAbility ability, bool value)
        {
            Changed?.Invoke(ability, value);
            CheckPowerButtonsInreractable();
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void Initialize()
        {

            foreach (var item in _abilities)
            {
                item.Instantiate(transform);
                item.Changed += Ability_Changed;
            }

        }

        public void Clear()
        {
            foreach (var item in _abilities)
            {
                item.Changed -= Ability_Changed;
            }

            transform.Clear();
        }

        private void CheckPowerButtonsInreractable()
        {
            int count = 0;
            foreach (var item in _abilities)
                count += item.IsOn ? 1 : 0;

            foreach (var item in _abilities)
            {
                if (!item.IsOn)
                    item.Interactable = !(count >= 3);
            }

        }
    }
}