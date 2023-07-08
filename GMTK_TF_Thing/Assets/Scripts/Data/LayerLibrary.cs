using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayerDB", menuName = "Layer Library")]
public class LayerLibrary : Initializable
{
    [SerializeField] LayerMask obstacles;

    public static LayerMask Obstacles => Instance.obstacles;


    public static LayerLibrary Instance;
    public override void Init() { Instance = this; }
}
