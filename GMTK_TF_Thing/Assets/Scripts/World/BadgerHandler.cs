using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgerHandler : MonoBehaviour
{
    private Stack<Vector3> positions = new Stack<Vector3>();
    

    public void AddPos(Vector3 pos)
    {
        positions.Push(pos);
    }

    public void EndIt()
    {
        transform.SetParent(transform.parent.parent);
        StartCoroutine(WalkTilEnd());
    }

    public void SetVuln()
    {
        vuln = true;
    }

    private bool vuln;

    IEnumerator WalkTilEnd()
    {
        while(positions.Count > 0)
        {
            var lastPos = positions.Pop();
            yield return WalkTo(lastPos);
            if (vuln && IsCaught())
            {
                FindObjectOfType<Finale>().GameOver(true);
                yield break;
            }
        }
    }

    private bool IsCaught()
    {
        return Physics.OverlapSphere(transform.position, 0.6f, LayerLibrary.Player).Length > 0;
    }

    IEnumerator WalkTo(Vector3 pos)
    {
        var dif = pos - transform.position;
        int steps = 16;
        float totalTime = 0.28f;

        var smallDif = new Vector2(dif.x / (float)steps, dif.y / (float)steps);

        float progress = 0f;

        float p = 1 / totalTime;

        while (progress < 1f)
        {

            transform.position += new Vector3(dif.x, 0, dif.z) * Time.deltaTime * p;
            progress += Time.deltaTime * p;

            yield return null;

        }

        transform.position = new Vector3(pos.x, 1f, pos.z);
    }
}
