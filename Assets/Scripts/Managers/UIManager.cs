using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _uiGameplay;
    [SerializeField] private GameObject _safe;
    [SerializeField] private GameObject _desk;
    
    [SerializeField] private GameObject _noteBookUIObject;
    [SerializeField] private GameObject _pauseButtonObject;
    [SerializeField] private GameObject _pauseMenuObject;

    [SerializeField] private GameObject _WinScreen;
    [SerializeField] private GameObject _LoseScreen;

    private GameObject _currentActiveUIGameplay;
    public bool uiIsActive = false;

    private UIDocument _document;
    private Button _button;
    
    private void Awake()
    {
		if (GameManager.uiManager == null)
			GameManager.uiManager = this;
		else 
		{
			Debug.LogError($"A UIManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}

        // _document = GetComponent<UIDocument>();
        // _button = _document.rootVisualElement.Q("RestartButton") as Button;
        // _button.RegisterCallback<ClickEvent>(OnClickRestart);
        
        // _document.rootVisualElement.Q("Container").style.visibility = Visibility.Hidden;
    }

    void Start()
    {
		GameManager.instance.OnPlayerWin += UnhideWinScreen;
		GameManager.instance.OnPlayerCaught += UnhideCaughtScreen;
		
		GameManager.instance.OnCutsceneStart += DisableUI;
		GameManager.instance.OnCutsceneEnd += EnableUI;
    }

    // Restarts the current scene
    private void OnClickRestart(ClickEvent evt)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetUIGameplayActive(string interactGame)
    {
        if (uiIsActive) return;
    
        GameManager.playerController.DisableMovement();
    
        _uiGameplay.SetActive(true);
    
        if (interactGame == "Safe")
            _currentActiveUIGameplay = _safe;
        else if (interactGame == "Desk")
            _currentActiveUIGameplay = _desk;
        
        if (_currentActiveUIGameplay != null)
            _currentActiveUIGameplay.SetActive(true);
            
        SetUIIsActive(true);
    }

    public void SetUIGameplayInactive()
    {
        GameManager.playerController.EnableMovement();
        _uiGameplay.SetActive(false);
    
        if (_currentActiveUIGameplay == null) return;
         
        _currentActiveUIGameplay.SetActive(false);
        _currentActiveUIGameplay = null;
        
        SetUIIsActive(false);
    }

    public void SetUIIsActive(bool value) => uiIsActive = value;

    public void EnableUI()
    {
        _noteBookUIObject.SetActive(true);
        _pauseButtonObject.SetActive(true);
    }

    public void DisableUI()
    {
        _noteBookUIObject.SetActive(false);
        _pauseButtonObject.SetActive(false);
    }

    public void UnhideCaughtScreen()
    {
        _noteBookUIObject.SetActive(false);
        _pauseButtonObject.SetActive(false);
        _pauseMenuObject.SetActive(false);
        _LoseScreen.SetActive(true);
        // _document.rootVisualElement.Q("HelperText").style.display = DisplayStyle.None;
        // _document.rootVisualElement.Q("Container").style.visibility = Visibility.Visible;
        // _document.rootVisualElement.Q<Label>("Reason").text = GameManager.instance.Caughtreason;
        // _document.rootVisualElement.Q("CaughtText").style.display = DisplayStyle.Flex;
    }
    
    public void UnhideWinScreen()
    {
        _noteBookUIObject.SetActive(false);
        _pauseButtonObject.SetActive(false);
        _pauseMenuObject.SetActive(false);
        _WinScreen.SetActive(true);
        // _document.rootVisualElement.Q("HelperText").style.display = DisplayStyle.None;
        // _document.rootVisualElement.Q("Container").style.visibility = Visibility.Visible;
        // _document.rootVisualElement.Q("WinText").style.display = DisplayStyle.Flex;
    }

    private void OnDisable()
    {
        // _button.UnregisterCallback<ClickEvent>(OnClickRestart);
        GameManager.instance.OnPlayerCaught -= UnhideCaughtScreen;
        GameManager.instance.OnPlayerWin -= UnhideWinScreen;
        
        GameManager.instance.OnCutsceneStart -= DisableUI;
		GameManager.instance.OnCutsceneEnd -= EnableUI;
    }
}
