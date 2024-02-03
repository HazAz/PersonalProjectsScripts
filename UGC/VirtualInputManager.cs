using UnityEngine;

public class VirtualInputManager : MonoBehaviour
{
    public static VirtualInputManager Instance = null;

    public bool MoveRight;
    public bool MoveLeft;
    public bool MoveUp;
    public bool Jump;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
