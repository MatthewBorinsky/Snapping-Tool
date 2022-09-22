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

    // Align buttons array
    int alignGridInt = -1;
    int alignConfirm = -1;
    string[] alignStrings = {"Align X", "Align Y", "Align Z"};
    string[] confirmStrings = {"Confirm", "Cancel"};

    // Creates the buttons for the snapper tool window
    void OnGUI(){
        // Only allows the user to use the snap button if objects are selected
        using(new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0)){
            if(GUILayout.Button("Snap Selection")){
                SnapSelection(); 
            }
        }

        // Creates a bunch of buttons for align tool
        GUILayout.Label("Select which plane to align");
        alignGridInt = GUILayout.SelectionGrid(alignGridInt, alignStrings, 3);
        alignConfirm = GUILayout.SelectionGrid(alignConfirm, confirmStrings, 2);

        // Determines if a plane was selected
        if (alignGridInt != -1){
            // If confirm selected, align all selected game objects in chosen plane and reset buttons
            if (alignConfirm == 0){
                chooseAlign(alignGridInt);
                alignGridInt = -1;
                alignConfirm = -1;
            }else if(alignConfirm == 1){
                alignGridInt = -1;
                alignConfirm = -1;
            }
        // If confirm or cancel is selected without a plane, reset buttons
        }else if (alignConfirm != -1){
            alignConfirm = -1;
        }
    }

    // Snapping method iterates through all selected objects and makes their positions int's
    void SnapSelection(){
        foreach(GameObject go in Selection.gameObjects){
            Undo.RecordObject(go.transform, "snap objects");
            go.transform.position = go.transform.position.Round();
        }
    }

    // Helper function to determine which plane to align
    void chooseAlign(int plane){
        if (plane == 0){
            AlignX();
        } else if(plane == 1){
            AlignY();
        } else if(plane == 2){
            AlignZ();
        }
    }

    // Aligns all selected objects X plane by finding the average plane then rounding to int
    void AlignX(){
        float averageSpace = 0;  // Average space of all objects in the plane

        // Finds the average place for the objects to go on the plane
        foreach(GameObject go in Selection.gameObjects){
            averageSpace += go.transform.position.x;
        }
        averageSpace = averageSpace / Selection.gameObjects.Length;
        averageSpace = Mathf.Round(averageSpace);

        // Transforms all selected objects selected plane to the average
        foreach(GameObject go in Selection.gameObjects){
            Undo.RecordObject(go.transform, "align objects");
            var pos = go.transform.position;
            pos.x = averageSpace;
            go.transform.position = pos;
        }
    }

    // Aligns all selected objects Y plane by finding the average plane then rounding to int
    void AlignY(){
        float averageSpace = 0;  // Average space of all objects in the plane

        // Finds the average place for the objects to go on the plane
        foreach(GameObject go in Selection.gameObjects){
            averageSpace += go.transform.position.y;
        }
        averageSpace = averageSpace / Selection.gameObjects.Length;
        averageSpace = Mathf.Round(averageSpace);

        // Transforms all selected objects selected plane to the average
        foreach(GameObject go in Selection.gameObjects){
            Undo.RecordObject(go.transform, "align objects");
            var pos = go.transform.position;
            pos.y = averageSpace;
            go.transform.position = pos;
        }
    }

    // Aligns all selected objects Z plane by finding the average plane then rounding to int
    void AlignZ(){
        float averageSpace = 0;  // Average space of all objects in the plane

        // Finds the average place for the objects to go on the plane
        foreach(GameObject go in Selection.gameObjects){
            averageSpace += go.transform.position.z;
        }
        averageSpace = averageSpace / Selection.gameObjects.Length;
        averageSpace = Mathf.Round(averageSpace);

        // Transforms all selected objects selected plane to the average
        foreach(GameObject go in Selection.gameObjects){
            Undo.RecordObject(go.transform, "align objects");
            var pos = go.transform.position;
            pos.z = averageSpace;
            go.transform.position = pos;
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