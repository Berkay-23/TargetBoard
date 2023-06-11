using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IndexMenu : MonoBehaviour
{
    private const string FIRST_RUN_KEY = "FirstRun";
    private Slider effectSlider;
    private Slider musicSlider;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(FIRST_RUN_KEY))
        {
            PlayerPrefs.SetInt(FIRST_RUN_KEY, 1);

            PlayerPrefs.SetFloat("EffectVolume", 0.75f);
            PlayerPrefs.SetFloat("MusicVolume", 0.75f);
            PlayerPrefs.SetInt("currentEpisode", 1);

            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name.Equals("SettingsPage"))
        {
            effectSlider = GameObject.Find("SliderEffect").GetComponent<Slider>();
            musicSlider = GameObject.Find("SliderMusic").GetComponent<Slider>();

            effectSlider.value = PlayerPrefs.GetFloat("EffectVolume") * 100;
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume") * 100;

        }
    }

    public void PlayGame() => StartCoroutine(LoadScene("Episodes"));
    public void GoSettings() => StartCoroutine(LoadScene("SettingsPage"));
    public void BackToMenu() => StartCoroutine(LoadScene("HomePage"));
    public void QuitGame() => StartCoroutine(LoadScene("Quit"));

    public void UpdateEffectVolume()
    {
        float value = effectSlider.value;
        PlayerPrefs.SetFloat("EffectVolume", value / 100);

        TextMeshProUGUI text = GameObject.Find("TextEffectPercentage").GetComponent<TextMeshProUGUI>();
        text.text = $"%{value}";
    }

    public void UpdateMusicVolume() 
    { 
        float value = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", value / 100);

        AudioSource audioSource = musicSlider.GetComponentInParent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");

        TextMeshProUGUI text = GameObject.Find("TextMusicPercentage").GetComponent<TextMeshProUGUI>();
        text.text = $"%{value}";
    }

    IEnumerator LoadScene(string name)
    {
        FindObjectOfType<AudioManager>().Play("Click");
        yield return new WaitForSeconds(0.1f);

        if(name != "Quit")
            SceneManager.LoadScene(name);
        else
            Application.Quit();
    }
}
