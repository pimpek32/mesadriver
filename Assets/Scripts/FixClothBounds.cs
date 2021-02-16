using UnityEngine;

public class FixClothBounds : MonoBehaviour
{

    private SkinnedMeshRenderer[] Renderers;
    [SerializeField]
    private Vector3 meshBounds;
    void Awake()
    {
        this.Renderers = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        this.GetComponent<Cloth>().enabled = true;
    }

    void OnRenderObject()
    {
        foreach (SkinnedMeshRenderer smr in this.Renderers)
        {
            smr.localBounds = new Bounds(Vector3.zero, meshBounds); // Paste your meshes real bounds here
        }
    }

}