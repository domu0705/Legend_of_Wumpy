using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPlacer : MonoBehaviour
{
    [SerializeField] private Vector3[] hintOffset = null;
    [SerializeField] private GameObject hintPrefab = null;
    private List<GameObject> hints = new List<GameObject>();
    private float radius = 0.25f;

    private void OnEnable()
    {
        foreach(Vector3 offset in hintOffset)
        {
            if (Physics.CheckSphere(transform.position + offset, radius, LayerMask.GetMask("Tile")))
            {
                hints.Add(Instantiate(hintPrefab));
                hints[hints.Count - 1].transform.position = transform.position + offset;
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject hint in hints)
        {
            Destroy(hint);
        }

        hints.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < hintOffset.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.position + hintOffset[i], radius);
        }
    }
}
