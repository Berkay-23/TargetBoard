using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    List<string> sceneNames;
    public int currentEpisode = 1;
    public Animator transition;
    private string[] musical_scenes = {
        "HomePage",
        "Episodes",
    };

    void Start()
    {
        InitialEpisodePaths();

        if (musical_scenes.Contains(SceneManager.GetActiveScene().name))
            StartCoroutine(FindObjectOfType<AudioManager>().FadeInAndPlay("MainMenu", 1f, 0.2f));
        else if (SceneManager.GetActiveScene().name.Equals("Finish"))
            StartCoroutine(FindObjectOfType<AudioManager>().FadeInAndPlay("Finish", 1f, 2f));

        currentEpisode = PlayerPrefs.GetInt(nameof(currentEpisode));
        CheckEpisodes();
    }

    public void NextLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (!currentScene.name.Equals("Episodes"))
        {
            int level = Int16.Parse(currentScene.name.Replace("Episode", ""));

            if (level == currentEpisode)
                PlayerPrefs.SetInt(nameof(currentEpisode), currentEpisode + 1);
        }

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

    }

    public void BackToEpisodes() => StartCoroutine(LoadEpisode("Episodes"));

    private void InitialEpisodePaths()
    {
        sceneNames = GetSceneNames();

        Button[] buttons = GetEpisodeButtons();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => {
                string name = button.name;
                string episode = name.Split('_')[2];

                string sceneName = $"Episode{episode}";

                if (sceneNames.Contains(sceneName))
                    StartCoroutine(LoadEpisode(sceneName));
                else
                    Debug.Log($"Scene {sceneName} not found");
            });
        }
    }

    private List<string> GetSceneNames()
    {
        List<string> names = new List<string>();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            name = path.Split("/")[2].Split(".")[0];
            names.Add(name);
        }
        return names;
    }

    private Button[] GetEpisodeButtons()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Button[] buttons = new Button[0];

        if (currentScene.name.Equals("Episodes"))
            buttons = this.GetComponentsInChildren<Button>();
        
        return buttons;
    }

    private void CheckEpisodes()
    {
        Button[] buttons = GetEpisodeButtons();
        foreach (Button button in buttons)
        {
            int buttonLevel = Int16.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text);

            if(buttonLevel > currentEpisode)
                button.interactable = false;
            else
                button.interactable = true;
        }
    }

    public void RefreshAnvance()
    {
        StartCoroutine(ButtonClickEffect());
        currentEpisode = 1;
        PlayerPrefs.SetInt(nameof(currentEpisode), currentEpisode);
        CheckEpisodes();
    }

    IEnumerator LoadLevel(int level)
    {
        FindObjectOfType<AudioManager>().Play("Click");
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(FindObjectOfType<AudioManager>().FadeInAndStop("Episode"));
        yield return new WaitForSeconds(1f);

        FindObjectOfType<AudioManager>().Play("Win");
        yield return new WaitForSeconds(2f);

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(level);
    }

    IEnumerator LoadEpisode(string sceneName) 
    {
        FindObjectOfType<AudioManager>().Play("Click");
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ButtonClickEffect()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        yield return new WaitForSeconds(0.1f);
    }
}
