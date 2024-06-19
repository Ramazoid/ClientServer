using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

namespace UnityToolbarExtender.Examples
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 16,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchLeftButton
	{
		static SceneSwitchLeftButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Select"))
			{
				SceneView.lastActiveSceneView.FrameSelected();
			}
			if (GUILayout.Button("Player"))
			{
				Selection.activeGameObject = GameObject.FindGameObjectWithTag("Player");
				SceneView.lastActiveSceneView.FrameSelected();
			}
			if (GUILayout.Button("Camera"))
			{
				Selection.activeGameObject = Camera.main.gameObject;
				SceneView.lastActiveSceneView.FrameSelected();
			}
			if (GUILayout.Button("Duplicate"))
			{
				GameObject old = Selection.activeGameObject;
				GameObject nG = GameObject.Instantiate(Selection.activeGameObject,old.transform.parent);
				Tools.current = Tool.Move;

				nG.transform.position = old.transform.position;
				Selection.activeGameObject = nG;
			}
			if (GUILayout.Button("|X"))
			{
				float x = (Selection.objects[0] as GameObject).transform.position.x;
				foreach (GameObject g in Selection.objects)
				{

					if (g.transform.position.x > x) x = g.transform.position.x;
				}

				foreach (GameObject g in Selection.objects) g.transform.position = g.transform.position.SetX(x);
				//SceneView.lastActiveSceneView.FrameSelected();
			}
			if (GUILayout.Button("X|"))
			{
				float x = (Selection.objects[0] as GameObject).transform.position.x;
				foreach (GameObject g in Selection.objects)
				{

					if (g.transform.position.x < x) x = g.transform.position.x;
				}

				foreach (GameObject g in Selection.objects) g.transform.position = g.transform.position.SetX(x);
				//SceneView.lastActiveSceneView.FrameSelected();
			}
			if (GUILayout.Button("Group"))
			{
				GameObject group = new GameObject("Group");
				foreach (GameObject g in Selection.objects)
				{

					g.transform.SetParent(group.transform);
				}

				Selection.activeGameObject = group;
				//SceneView.lastActiveSceneView.FrameSelected();

			}
			Texture but = (Texture)Resources.Load("Rotate90.png");
			Rect rec = new Rect(0, 0, 64, 64);
			GUIContent cont = new GUIContent("r90", but);
			if (GUILayout.Button("R90"))
			{
				GameObject group = new GameObject("Group");
				foreach (GameObject g in Selection.objects)
				{

					g.transform.SetParent(group.transform);
				}

				Selection.activeGameObject = group;
				//SceneView.lastActiveSceneView.FrameSelected();

			}
			if (GUILayout.Button("SAVE"))
			{
				EditorApplication.ExecuteMenuItem("File/Save");
				EditorApplication.ExecuteMenuItem("File/Save Project");

			}
			if (GUILayout.Button("   BUILD   "))
			{
				EditorApplication.ExecuteMenuItem("File/Save");
				EditorApplication.ExecuteMenuItem("File/Build Settings...");

			}

			if (GUILayout.Button("   B&Run   "))
			{
				EditorApplication.ExecuteMenuItem("File/Save");
				EditorApplication.ExecuteMenuItem("File/Build And Run");

			}
		}
	}
	public static class Vector3Extension
    {
		public static Vector3 SetX(this Vector3 v,float newX)
        {

			return new Vector3(newX, v.y, v.z);
        }
    }

	static class SceneHelper
	{
		static string sceneToOpen;

		public static void StartScene(string sceneName)
		{
			if(EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}

			sceneToOpen = sceneName;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			if (sceneToOpen == null ||
			    EditorApplication.isPlaying || EditorApplication.isPaused ||
			    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			EditorApplication.update -= OnUpdate;

			if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				// need to get scene via search because the path to the scene
				// file contains the package version so it'll change over time
				string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
				if (guids.Length == 0)
				{
					Debug.LogWarning("Couldn't find scene file");
				}
				else
				{
					string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
					EditorSceneManager.OpenScene(scenePath);
					EditorApplication.isPlaying = true;
				}
			}
			sceneToOpen = null;
		}
	}
}