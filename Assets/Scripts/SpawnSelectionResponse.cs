using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSelectionResponse : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab = null;
    [SerializeField] private float offset = 1;
    private GameObject _spawn;

    public void OnSelect()
    {
        Vector3 pos = transform.position + Vector3.up * offset;

        _spawn = Instantiate(spawnPrefab);
        _spawn.transform.SetParent(transform);
        _spawn.transform.position = pos;
    }

    public void OnDeselect()
    {
        if (_spawn != null)
        {
            Destroy(_spawn);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 delta1 = Vector3.up * offset;
        Gizmos.DrawLine(transform.position, transform.position + delta1);
    }
}
