using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public Button backButton;
    public VisualElement mainMenu;
    public VisualElement optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake() {
        var root = this.GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("PlayBtn");
        optionsButton = root.Q<Button>("OptionsBtn");
        quitButton = root.Q<Button>("QuitBtn");
        backButton = root.Q<Button>("BackBtn");
        mainMenu = root.Q<VisualElement>("MainMenu");
        optionsMenu = root.Q<VisualElement>("OptionsMenu");

        playButton.clicked += PlayButtonPressed;
        optionsButton.clicked += OptionsButtonPressed;
        quitButton.clicked += QuitButtonPressed;
        backButton.clicked += BackButtonPressed;

    }

    void PlayButtonPressed() {
        SceneManager.LoadScene("Level1");
    }

    void OptionsButtonPressed() {
        mainMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.Flex;
    }

    void QuitButtonPressed() {
        Debug.Log("Quit!");
        Application.Quit();
    }

    void BackButtonPressed() {
        mainMenu.style.display = DisplayStyle.Flex;
        optionsMenu.style.display = DisplayStyle.None;
    }
}
