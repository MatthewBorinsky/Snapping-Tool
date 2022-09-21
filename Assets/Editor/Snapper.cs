using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Snapper Class to make the position of all selected objects integers
public class Snapper : EditorWindow {
    
    // Creates the Snapper tool in the menu bar
    [MenuItem( "Tools/Snapper" )]
    public static void OpenSnapper() => GetWindow<Snapper>();

    // When an object selection is made, refresh Snap Tool GUI
    void OnEnable(){
        Selection.selectionChanged += Repaint;
    }
    void OnDisable(){
        Selection.selectionChanged -= Repaint;
    }


    // Creates the buttons for the snapper tool window
    void OnGUI(){
        // Only allows the user to use the button if objects are selected
        using(new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0)){
            if(GUILayout.Button("Snap Selection")){
                SnapSelection(); 
            }
        }

    }

    // Snapping method iterates through all selected objects and makes their positions int's
    void SnapSelection(){
        foreach(GameObject go in Selection.gameObjects){
            Undo.RecordObject(go.transform, "snap objects");
            go.transform.position = go.transform.position.Round();
        }
    }
}

// Helper functions that perform operations etc.
static class helpers{

    // Rounding helper function to make a position vector all int's
    public static Vector3 Round(this Vector3 v){
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }
}
