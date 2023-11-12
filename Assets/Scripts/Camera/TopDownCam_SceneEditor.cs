using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SYC
{
    [CustomEditor(typeof(TopDownCam))]
    public class TopDownCam_SceneEditor : Editor
    {
        TopDownCam _targetCam;

        public override void OnInspectorGUI()
        {
            _targetCam = (TopDownCam)target;
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            if (!_targetCam || !_targetCam._target) return;

            Transform camTarget = _targetCam._target;
            Vector3 targetPos = camTarget.position;
            targetPos.y += _targetCam._lookAtHeight;

            Handles.color = new Color(1f, 0f, 0f, 0.15f);
            Handles.DrawSolidDisc(targetPos, Vector3.up, _targetCam._distance);

            Handles.color = new Color(0f, 1f, 0f, 0.75f);
            Handles.DrawWireDisc(targetPos, Vector3.up, _targetCam._distance);

            Handles.color = new Color(1f, 0f, 0f, 0.5f);
            _targetCam._distance = Handles.ScaleSlider(_targetCam._distance, 
                targetPos, 
                -camTarget.forward, 
                Quaternion.identity, 
                _targetCam._distance, 0.1f);
            _targetCam._distance = Mathf.Clamp(_targetCam._distance, 2f, float.MaxValue);

            Handles.color = new Color(0f, 0f, 1f, 0.5f);
            _targetCam._height = Handles.ScaleSlider(_targetCam._height,
                targetPos,
                Vector3.up,
                Quaternion.identity,
                _targetCam._height, 0.1f);
            _targetCam._height = Mathf.Clamp(_targetCam._height, 2f, float.MaxValue);

            GUIStyle lableStyle = new GUIStyle();
            lableStyle.fontSize = 15;
            lableStyle.normal.textColor = Color.white;
            lableStyle.alignment = TextAnchor.UpperCenter;

            Handles.Label(targetPos + (-camTarget.forward * _targetCam._distance), "Distance", lableStyle);

            lableStyle.alignment = TextAnchor.MiddleRight;
            Handles.Label(targetPos + (Vector3.up * _targetCam._height), "Height", lableStyle);

            _targetCam.HandleCamera();
        }
    }
}
