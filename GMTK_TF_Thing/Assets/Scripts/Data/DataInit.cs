using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInit : MonoBehaviour
{
    [SerializeField] List<Initializable> initializables;

    private void Awake()
    {
        foreach (var n in initializables)
            n.Init();
    }
}
