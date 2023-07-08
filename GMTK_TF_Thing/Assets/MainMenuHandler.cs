using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject[] menuItems; 
    private Vector3[] originalPositions;
    private Vector3[] originalRotations;
    private Vector3[] originalScales;

    public GameObject selected;
    public int _selected;

    // Start is called before the first frame update
    void Start()
    {
        selected = menuItems[_selected];

        originalPositions = new Vector3[menuItems.Length];
        originalRotations = new Vector3[menuItems.Length];
        originalScales = new Vector3[menuItems.Length];
        int i = 0;
        foreach(GameObject obbo in menuItems) {
            originalPositions[i] = obbo.transform.position;
            originalRotations[i] = obbo.transform.eulerAngles;
            originalScales[i] = obbo.transform.localScale;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            moveCursor(1);    
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            moveCursor(-1);    
        }

        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Space)) {
            Select();
        }

        int i = 0;

        Vector3 o = Vector3.zero;
        Vector3 s = Vector3.one;
        foreach(GameObject obbo in menuItems) {
            if (selected == obbo) {
                o = originalPositions[i];
                s = originalScales[i];
                continue;
                i++;
            }
            i++;

             //obbo.transform.position = Vector3.Lerp(obbo.transform.position, originalPositions[i], Time.deltaTime);
             //obbo.transform.eulerAngles = originalRotations[i];
             obbo.transform.localScale = Vector3.Lerp(obbo.transform.localScale, originalScales[i], Time.deltaTime* 4); 
        }

        //selected.transform.position = Vector3.Lerp(selected.transform.position, (o + -Vector3.forward) , Time.deltaTime);

        selected.transform.localScale = Vector3.Lerp(selected.transform.localScale, (s * 1.4f), Time.deltaTime * 4); 

    }

    private void Select()
    {
        if (_selected == 0) {
            // Play...
        }

        if (_selected == 1) {
            Application.Quit();
        }
    }

    void moveCursor(int dir) { 
        Debug.Log("mo");
        _selected += dir;
        Debug.Log(_selected);
        if (_selected < 0) {
            Debug.Log("mo1");
            _selected = menuItems.Length;
        }

        if (_selected >= menuItems.Length) {
            Debug.Log("mo2");
            _selected = 0;
        }

        selected = menuItems[_selected];

    }
}
