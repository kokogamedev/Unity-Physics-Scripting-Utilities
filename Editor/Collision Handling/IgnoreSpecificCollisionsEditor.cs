using System.Collections;
using UnityEngine;
using PsigenVision.Utilities.PhysX.Collisions;

namespace PsigenVision.Utilities.PhysX.Editor
{
	using UnityEditor;
	using Unity.EditorCoroutines.Editor;
	
	[CustomEditor(typeof(IgnoreSpecificCollisions))]
	public class IgnoreSpecificCollisionsEditor: Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var script = (IgnoreSpecificCollisions)target;
			if (GUILayout.Button("Collect Colliders"))
			{
				EditorCoroutineUtility.StartCoroutine(CollectColliders(), this);
			}

			IEnumerator CollectColliders()
			{
				yield return script.CollectColliders();
				EditorUtility.SetDirty(target);
			}
		}
	}
}
