using UnityEditor;
using UnityEditor.AnimatedValues;

[CanEditMultipleObjects]
[CustomEditor(typeof(MovementController))]
public class MovenentControllerEditor : Editor
{
    private SerializedProperty _rigidbodyComponent;
    private SerializedProperty _movementSpeed;
    private SerializedProperty _acceleration;
    private SerializedProperty _turnSpeed;
    private SerializedProperty _velocityDependent;
    private SerializedProperty _rotateWithMovement;
    private SerializedProperty _rotationType;
    private SerializedProperty _alignToCamera;

    private AnimBool _rotateWithMovementGroup;

    private void OnEnable()
    {
        _rigidbodyComponent = serializedObject.FindProperty(nameof(_rigidbodyComponent));
        _movementSpeed = serializedObject.FindProperty(nameof(_movementSpeed));
        _acceleration = serializedObject.FindProperty(nameof(_acceleration));
        _turnSpeed = serializedObject.FindProperty(nameof(_turnSpeed));
        _velocityDependent = serializedObject.FindProperty(nameof(_velocityDependent));
        _rotateWithMovement = serializedObject.FindProperty(nameof(_rotateWithMovement));
        _rotationType = serializedObject.FindProperty(nameof(_rotationType));
        _alignToCamera = serializedObject.FindProperty(nameof(_alignToCamera));

        _rotateWithMovementGroup = new AnimBool(_rotateWithMovement.boolValue, Repaint);
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_rigidbodyComponent);
        EditorGUILayout.PropertyField(_movementSpeed);
        EditorGUILayout.PropertyField(_acceleration);
        EditorGUILayout.PropertyField(_turnSpeed);
        EditorGUILayout.PropertyField(_velocityDependent);
        EditorGUILayout.PropertyField(_rotateWithMovement);

        _rotateWithMovementGroup.target = _rotateWithMovement.boolValue;
        if (EditorGUILayout.BeginFadeGroup(_rotateWithMovementGroup.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_rotationType);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFadeGroup();

        EditorGUILayout.PropertyField(_alignToCamera);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}