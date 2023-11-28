using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    private VisualElement MainUI;
    private Slider VolumeSlider;
    private Button ResumeButton;
    private Button backButton;
    private VisualElement PauseDialog;
    public AudioMixer Mixer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            PauseDialog.style.display = DisplayStyle.Flex;
            Time.timeScale = 0f;
        }
    }

    private void Awake() {
        var root = this.GetComponent<UIDocument>().rootVisualElement;
        MainUI = root.Q<VisualElement>("MainUI");
        PauseDialog = root.Q<VisualElement>("PauseDialog");
        VolumeSlider = root.Q<Slider>("VolumeSlider");
        ResumeButton = root.Q<Button>("ResumeButton");
        backButton = root.Q<Button>("BackButton");
        ResumeButton.clicked += ResumeButtonPressed;
        backButton.clicked += BackButtonPressed;
        VolumeSlider.RegisterValueChangedCallback(VolumeSliderChanged);
    }

    void VolumeSliderChanged(ChangeEvent<float> evt) {
        Mixer.SetFloat("MainVolume", evt.newValue);
    }

    void BackButtonPressed() {
        PauseDialog.style.display = DisplayStyle.None;
        SceneManager.LoadScene(0);
    }

    void ResumeButtonPressed() {
        PauseDialog.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
    }
}
