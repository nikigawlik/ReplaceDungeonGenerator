using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
    public class Preferences : MonoBehaviour
    {
        private static bool prefsLoaded = false;

        // Actual preferences 
        private static Color roomBoxColor = Color.gray;
        private static Color roomLabelColor = Color.white;
        private static float roomBoxSize = .1f;

        public static Color RoomBoxColor
        {
            get
            {
                return IntToColor(EditorPrefs.GetInt("roomBoxColor", ColorToInt(roomBoxColor)));
            }
        }

        public static Color RoomLabelColor
        {
            get
            {
                return IntToColor(EditorPrefs.GetInt("roomLabelColor", ColorToInt(roomLabelColor)));
            }
        }

        public static float RoomBoxSize
        {
            get
            {
                return EditorPrefs.GetFloat("roomBoxSize", roomBoxSize);
            }
        }

#if UNITY_EDITOR
        [PreferenceItem("ReDuGe")]

        public static void PreferencesGUI()
        {
            // Load the preferences
            if (!prefsLoaded)
            {
                roomBoxColor = IntToColor(EditorPrefs.GetInt("roomBoxColor", ColorToInt(roomBoxColor)));
                roomLabelColor = IntToColor(EditorPrefs.GetInt("roomLabelColor", ColorToInt(roomLabelColor)));
                roomBoxSize = EditorPrefs.GetFloat("roomBoxSize", roomBoxSize);
                prefsLoaded = true;
            }

            // Preferences GUI
            roomBoxSize = EditorGUILayout.FloatField("Room box size", roomBoxSize);
            roomBoxColor = EditorGUILayout.ColorField("Room box color", roomBoxColor);
            roomLabelColor = EditorGUILayout.ColorField("Room label color", roomLabelColor);

            // Save the preferences
            if (GUI.changed)
            {
                EditorPrefs.SetFloat("roomBoxSize", roomBoxSize);
                EditorPrefs.SetInt("roomBoxColor", ColorToInt(roomBoxColor));
                EditorPrefs.SetInt("roomLabelColor", ColorToInt(roomLabelColor));
            }
        }
#endif

        private static int ColorToInt(Color c)
        {
            int r = (int)(c.r * 255);
            int g = (int)(c.g * 255);
            int b = (int)(c.b * 255);
            int a = (int)(c.a * 255);

            return
                (r << 24) |
                (g << 16) |
                (b << 8) |
                a
            ;
        }

        private static Color IntToColor(int i)
        {
            int r = (i >> 24) & 0xff;
            int g = (i >> 16) & 0xff;
            int b = (i >> 8) & 0xff;
            int a = i & 0xff;

            return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
        }
    }
}