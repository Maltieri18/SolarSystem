using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet")) planet.GeneratePlanet();
        if (GUILayout.Button("Print MinMax")) planet.PrintMinMax();

        DrawSettingsEditor(planet.shapeSetting, planet.OnShapeSettingUpdate, ref planet.shapeSettingFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSetting, planet.OnColorSettingUpdate, ref planet.colorSettingFoldout, ref colorEditor);
    } 

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings); //True indicataes always folded out
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout == true)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }

            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
