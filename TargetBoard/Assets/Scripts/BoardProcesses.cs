using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardProcesses : MonoBehaviour
{

    public Button[] buttons;
    public TextMeshProUGUI[] hitTexts;
    public TextMeshProUGUI targetTXT;
    public TextMeshProUGUI currentTXT;
    public TextMeshProUGUI episodeTXT;
    public Button nextLevel;
    public List<double> selectedValues;

    void Start()
    {
        selectedValues = new List<double>() { 0, 0, 0};
        StartCoroutine(PlaySounds());
        InitialButton();
        SetHitTexts();
        UpdateLevelPanel();
    }

    void Update(){ }

    private void InitialButton()
    {
        List<(Button, double, int)> buttonPairs = buttons.Select((button, index) =>
        {
            string buttonText = button.GetComponentInChildren<TextMeshProUGUI>().text;
            double buttonData = double.Parse(buttonText);
            return (button, buttonData, index);
        }).ToList();

        buttonPairs.ForEach(pair =>
        {
            Button button = pair.Item1;
            double score = pair.Item2;
            int index = pair.Item3;

            button.onClick.AddListener(() =>
            {
                SetSelectedValues(score);
                SetHitTexts();
                double targetPoint = double.Parse(targetTXT.text);

                Image panelImage = currentTXT.transform.parent.GetComponent<Image>();

                if (targetPoint == selectedValues.Sum() && !selectedValues.Contains(0))
                {
                    panelImage.color = new Color32(98, 211, 26, 192);
                    currentTXT.color = new Color32(0, 0, 0, 192);
                    nextLevel.gameObject.SetActive(true);
                }

                else
                {
                    panelImage.color = new Color32(239, 83, 80, 192);
                    currentTXT.color = new Color32(255, 255, 255, 255);
                    nextLevel.gameObject.SetActive(false);
                }
            });
        });
    }
    private void SetSelectedValues(double value)
    {
        if (selectedValues.Count <= 3)
        {
            if (selectedValues.Contains(value))
            {
                selectedValues[selectedValues.IndexOf(value)] = 0;
                FindObjectOfType<AudioManager>().Play("Pull");
            }

            else if (selectedValues.Contains(0))
            {
                selectedValues[selectedValues.IndexOf(0)] = value;
                FindObjectOfType<AudioManager>().Play("Hit");
            }
                
        }
    }
    private void SetHitTexts()
    {
        List<(TextMeshProUGUI, int)> pairs = hitTexts.Select((textItem, index) =>
        (textItem, index)).ToList();

        pairs.ForEach(pair =>
        {
            TextMeshProUGUI element = pair.Item1;
            int index = pair.Item2;

            if (selectedValues[index] != 0)
                element.text = $"{selectedValues[index]}";
            
            else
                element.text = "-";

            currentTXT.text = $"{selectedValues.Sum()}";
        });
    }
    private void UpdateLevelPanel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneNumber = currentScene.name.Replace("Episode","");

        episodeTXT.text = sceneNumber;

    }
    IEnumerator PlaySounds()
    {
        FindObjectOfType<AudioManager>().Play("Start");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FindObjectOfType<AudioManager>().FadeInAndPlay("Episode", 0.2f, 0.8f));
    }
}
