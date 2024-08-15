using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!UIManager.Instance.IsPause)
                UIManager.Instance.Pause();
            else
                UIManager.Instance.UnPause();
        }
    }
}
