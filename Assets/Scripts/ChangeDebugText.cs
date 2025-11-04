using TMPro;
using UnityEngine;

public class ChangeDebugText : MonoBehaviour
{
    [SerializeField] private TMP_Text _debugText;

    private void Awake()
    {
        if (GameManager.changeDebugText == null)
            GameManager.changeDebugText = this;
        else
        {
            Debug.LogError($"A ChangeDebugText already exists, deleting self: {name}");
            Destroy(gameObject);
        }
    }

    public void ChangeText(string newtext) => _debugText.text = newtext;
}
