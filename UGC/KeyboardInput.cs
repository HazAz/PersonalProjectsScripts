using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private void Update()
    {
        //VirtualInputManager.Instance.MoveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        //VirtualInputManager.Instance.MoveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        //VirtualInputManager.Instance.Jump = Input.GetKey(KeyCode.Space);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            VirtualInputManager.Instance.MoveLeft = true;
        }
        else
        {
            VirtualInputManager.Instance.MoveLeft = false;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            VirtualInputManager.Instance.MoveRight = true;
        }
        else
        {
            VirtualInputManager.Instance.MoveRight = false;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            VirtualInputManager.Instance.MoveUp = true;
        }
        else
        {
            VirtualInputManager.Instance.MoveUp = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            VirtualInputManager.Instance.Jump = true;
        }
        else
        {
            VirtualInputManager.Instance.Jump = false;
        }
    }
}
