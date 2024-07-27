using UnityEngine;

public class MousePositionToShader : MonoBehaviour
{
    [SerializeField] private Material _fonMaterial;

    void FixedUpdate()
    {
        _fonMaterial.SetVector("_MousePosition", new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2));
    }
}
