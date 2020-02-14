using System.Collections.Generic;
using System.Linq;

namespace AO.SpaceGame
{
    public class AbilityEvaluator
    {
        private List<IAbility> _abilities = new List<IAbility>();

        public void Add(IAbility ability)
        {
            if (!Contains(ability))
            {
                _abilities.Add(ability);
                ability.Start();
            }
        }

        public void Remove<T>() where T : IAbility
        {
            foreach (var item in _abilities.ToList())
            {
                if (typeof(T) == item.GetType())
                {
                    item.Destroy();
                    _abilities.Remove(item);
                    break;
                }
            }
        }

        public bool Contains(IAbility ability)
        {
            foreach (var item in _abilities.ToList())
            {
                if (ability.GetType() == item.GetType())
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            foreach (var item in _abilities)
            {
                item.Destroy();
            }

            _abilities.Clear();
        }

        public void Update()
        {
            foreach (var item in _abilities)
            {
                item.Update();
            }
        }
    }
}
