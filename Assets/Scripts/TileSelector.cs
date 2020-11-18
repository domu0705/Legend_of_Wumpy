using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1;
    private Transform selectedTile = null;
    private Vector3 dir = Vector3.zero;
    public Transform SelectedTile { get { return selectedTile; } }
    public bool reponseEnabled = true;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, dir, out hit, rayDistance, LayerMask.GetMask("Tile", "Camo")))
        {
            if (selectedTile != null)
            {
                selectedTile.GetComponent<SpawnSelectionResponse>().OnDeselect();
            }
            selectedTile = hit.transform;
            if (reponseEnabled)
            {
                selectedTile.GetComponent<SpawnSelectionResponse>().OnSelect();
            }
            dir = Vector3.zero;
        }
    }

    public virtual void SetDir(string dirString)
    {
        switch (dirString)
        {
            case "W":
                dir = Vector3.left;
                break;
            case "A":
                dir = Vector3.back;
                break;
            case "S":
                dir = Vector3.right;
                break;
            case "D":
                dir = Vector3.forward;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for(int i = 0; i < 4; i++)
        {
            Vector3 delta =  Quaternion.Euler(0, 90 * i, 0) * Vector3.forward * rayDistance;
            Gizmos.DrawLine(transform.position, transform.position + delta);
        }
    }

    private void OnDisable()
    {
        if(selectedTile != null){
            selectedTile.GetComponent<SpawnSelectionResponse>().OnDeselect();
            selectedTile = null;
        }
    }
}
