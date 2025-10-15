using UnityEngine;

public class Locomotion : MonoBehaviour
{
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }


    public void SetVelocity()
    {
        // _characterController.velocity;
    }

}
