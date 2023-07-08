using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayerDB", menuName = "Layer Library")]
public class LayerLibrary : Initializable
{
    [SerializeField] LayerMask obstacles;
    [SerializeField] LayerMask rotator;

    public static LayerMask Obstacles => Instance.obstacles;
    public static LayerMask Rotators => Instance.rotator;


    public static LayerLibrary Instance;
    public override void Init() { Instance = this; }
}
