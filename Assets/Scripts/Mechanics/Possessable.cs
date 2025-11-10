using UnityEngine;

public class Possessable : MonoBehaviour
{
    protected PlayerController ghost;
    protected bool isPossessed = false;

    public virtual void OnPossessed(PlayerController controller)
    {
        ghost = controller;
        isPossessed = true;
    }

    public virtual void OnReleased()
    {
        isPossessed = false;
    }
}
