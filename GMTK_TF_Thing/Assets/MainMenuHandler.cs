using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] CanvasGroup can;
    public GameObject[] menuItems; 
    private Vector3[] originalPositions;
    private Vector3[] originalRotations;
    private Vector3[] originalScales;

    private GameObject selected;
    public GameObject logo;
    private Vector3 logoOriginalScale;
    private float logoT = 1f;
    private int _selected;

    private bool isLoading;

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

        logoOriginalScale = logo.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading) return;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            moveCursor(1);
            AudioHandler.TryPlaySound(SoundIdentifier.Button_Move);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            moveCursor(-1);
            AudioHandler.TryPlaySound(SoundIdentifier.Button_Move);
        }

        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)) {
            Select();
            AudioHandler.TryPlaySound(SoundIdentifier.Button_Start);
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

        //logoT += Time.deltaTime * 0.5f;
        //logo.transform.localScale = logoOriginalScale + (logoOriginalScale * (Mathf.Sin(logoT)) * 0.12f);

    }

    private void Select()
    {
        if (_selected == 0) {
            PlayScene();
        }

        if (_selected == 1) {
            Application.Quit();
        }
    }

    private void PlayScene()
    {
        isLoading = true;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        while(can.alpha < 1)
        {
            can.alpha += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

    void moveCursor(int dir) { 
        Debug.Log("mo");
        _selected += dir;
        Debug.Log(_selected);
        if (_selected < 0) {
            _selected = menuItems.Length - 1;
        }

        if (_selected >= menuItems.Length) {
            _selected = 0;
        }

        selected = menuItems[_selected];

    }
}
