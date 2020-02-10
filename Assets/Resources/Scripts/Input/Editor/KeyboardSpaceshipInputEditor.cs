using AO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace AO.SpaceGame.Input
{
    [CustomEditor(typeof(KeyboardSpaceshipInput))]
    public class KeyboardSpaceshipInputEditor: Editor
    {
        private BaseSpaceshipInput _input;

        private void OnEnable()
        {
            _input = (BaseSpaceshipInput)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            string debug = "";
            debug += "Pitch = " + _input.Picth + "\n";
            debug += "Roll = " + _input.Roll + "\n";
            debug += "Yaw = " + _input.Yaw + "\n";
            debug += "Throttle = " + _input.Throttle + "\n";
            debug += "Fire1 = " + _input.Fire1 + "\n";
            debug += "Fire2 = " + _input.Fire2 + "\n";

            GUILayout.Space(20);
            EditorGUILayout.TextArea(debug, GUILayout.Height(100));
            GUILayout.Space(20);

            Repaint();

        }
    }
}
