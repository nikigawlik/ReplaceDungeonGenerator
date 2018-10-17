using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ReplaceDungeonGenerator
{
	public class Preferences : MonoBehaviour {
		private static bool prefsLoaded = false;

		// Actual preferences 
		public static Color roomBoxColor = Color.gray;
		public static Color roomLabelColor = Color.white;

		[PreferenceItem("ReDuGe")]

		public static void PreferencesGUI()
		{
			// Load the preferences
			if (!prefsLoaded)
			{
				roomBoxColor = IntToColor(EditorPrefs.GetInt("roomBoxColor", ColorToInt(roomBoxColor)));
				roomLabelColor = IntToColor(EditorPrefs.GetInt("roomLabelColor", ColorToInt(roomLabelColor)));
				prefsLoaded = true;
			}

			// Preferences GUI
			roomBoxColor = EditorGUILayout.ColorField("Room box color", roomBoxColor);
			roomLabelColor = EditorGUILayout.ColorField("Room label color", roomLabelColor);

			// Save the preferences
			if (GUI.changed) {
				EditorPrefs.SetInt("roomBoxColor", ColorToInt(roomBoxColor));
				EditorPrefs.SetInt("roomLabelColor", ColorToInt(roomLabelColor));
			}
		}

		private static int ColorToInt(Color c) {
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

		private static Color IntToColor(int i) {
			int r = (i >> 24) & 0xff;
			int g = (i >> 16) & 0xff;
			int b = (i >> 8) & 0xff;
			int a = i & 0xff;

			return new Color((float) r / 255f, (float) g / 255f, (float) b / 255f, (float) a / 255f);
		}
	}
}