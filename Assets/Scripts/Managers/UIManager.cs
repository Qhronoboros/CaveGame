using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool uiIsActive = false;
    
    private void Awake()
    {
		if (GameManager.uiManager == null)
			GameManager.uiManager = this;
		else 
		{
			Debug.LogError($"A UIManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}
    }

    void Start()
    {
        
    }
}
