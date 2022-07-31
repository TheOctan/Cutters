using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CuttingController))]
public class CuttingControllerEditor : Editor
{
    private CuttingController _cuttingController;

    private SerializedProperty _castHeight;
    private SerializedProperty _castDistance;
    private SerializedProperty _castRadius;

    private void OnEnable()
    {
        _cuttingController = target as CuttingController;
        _castHeight = serializedObject.FindProperty(nameof(_castHeight));
        _castDistance = serializedObject.FindProperty(nameof(_castDistance));
        _castRadius = serializedObject.FindProperty(nameof(_castRadius));
    }

    private void OnSceneGUI()
    {
        Transform transform = _cuttingController.transform;
        Vector3 position = transform.position + Vector3.up * _castHeight.floatValue;

        Handles.color = Color.green;
        Handles.DrawWireDisc(position, Vector3.up, _castDistance.floatValue);
    }
}