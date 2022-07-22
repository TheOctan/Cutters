using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldGenerator))]
public class FieldGeneratorEditor : Editor
{
    private FieldGenerator _field;
    private bool _autoGenerate = true;

	private void OnEnable()
	{
        _field = target as FieldGenerator;
    }

	public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector() && _autoGenerate)
        {
            _field.GenerateField();
        }

        _autoGenerate = EditorGUILayout.Toggle("Auto Generate", _autoGenerate);

        if(GUILayout.Button("Generate Map"))
        {
            _field.GenerateField();
        }
    }
}
