using AO.Utilities;
using System;
using UnityEngine;

namespace AO.SpaceGame.UI
{
    public interface IUIAbility
    {
        Type GetAbilityType();
    }

    public abstract class BaseUIAbility: IUIAbility
    {
        protected AbilityButton _button;

        public event Action<BaseUIAbility, bool> Changed;

        public bool IsOn => _button.IsOn;

        public bool Interactable { get => _button.Interactable; set => _button.Interactable = value; }
        public Func<bool> InputAction { get; }

        public BaseUIAbility(Func<bool> inputAction)
        {
            InputAction = inputAction;
        }

        public void Instantiate(Transform parent)
        {
            GameObject preafab = PrefabUtil.Create<GameObject>("UI/AbilityButton");
            _button = GameObject.Instantiate(preafab).GetComponent<AbilityButton>();
            _button.transform.parent = parent;
            _button.Changed += AbilityButton_Changed;
        }

        private void AbilityButton_Changed(bool value)
        {
            Changed?.Invoke(this, value);
        }

        public void Update()
        {
            if (InputAction())
                _button.IsOn = !_button.IsOn;
        }

        public abstract Type GetAbilityType();

    }
    public class UIAbility<T> : BaseUIAbility where T : IAbility
    {
        public UIAbility(Func<bool> inputAction) : base(inputAction)
        {
        }

        public override Type GetAbilityType()
        {
            return typeof(T);
        }

    }
}