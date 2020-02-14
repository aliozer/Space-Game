using AO.Utilities;
using UnityEngine;

namespace AO.SpaceGame
{
    public static class AssaultWeaponFactory
    {

        public static T GetWeapon<T>(Transform parent, string name = "") where T : SpaceshipAssaultWeapon, new()
        {
            T prefab = PrefabUtil.Create<T>(string.IsNullOrEmpty(name) ? typeof(T).Name : name);

            if (prefab == null)
                return null;

            var weapon = GameObject.Instantiate(prefab);
            weapon.transform.parent = parent;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            return weapon;
        }
    }
}
