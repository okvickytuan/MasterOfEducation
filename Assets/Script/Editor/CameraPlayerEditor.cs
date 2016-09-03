using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraPlayer))]
public class CameraPlayerEditor : Editor {

	private CameraPlayer _cameraPlayer;
	private Transform _transTarget;

	void OnEnable() 
	{
		_cameraPlayer = (CameraPlayer)target;
		_transTarget = _cameraPlayer.transform;
		if (_cameraPlayer.PosPlayer == null) 
		{
			_cameraPlayer.PosPlayer = new Vector3[4];	
			_cameraPlayer.RotPlayer = new Quaternion[4];	
		}
	}

	public override void OnInspectorGUI () {
		EditorGUILayout.LabelField("Transform player:");

		for (int i=0; i<4; i++) 
		{
			EditorGUILayout.Vector3Field ("Position player " + (i+1) + ":", _cameraPlayer._position[i]);
			if (GUILayout.Button ("Save player " + (i+1))) {
				_cameraPlayer._position[i] = new Vector3(_transTarget.position.x, _transTarget.position.y, _transTarget.position.z);
				_cameraPlayer.RotPlayer[i] = new Quaternion(_transTarget.rotation.x, _transTarget.rotation.y, _transTarget.rotation.z, _transTarget.rotation.w);
			}
		}
		if (GUI.changed) 
		{
			EditorUtility.SetDirty(_cameraPlayer);
		}
	}
}
