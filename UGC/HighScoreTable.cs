using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreTransformList;

    private void Awake()
    {
        entryContainer = transform;
        entryTemplate = transform.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // Load high scores and sort by time
        Highscores highScores = new Highscores();
        string json = PlayerPrefs.GetString("highscoreTable");
        if (!string.IsNullOrWhiteSpace(json))
        {
            highScores = JsonUtility.FromJson<Highscores>(json);
        }
        var highscoreEntryListSorted = highScores.highscoreEntryList.OrderBy(x => x.time).Take(15).ToList();

        highscoreTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryListSorted)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;

        Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }

            // Set text
            entryTransform.Find("posText").GetComponent<Text>().text = rankString;
            entryTransform.Find("timeText").GetComponent<Text>().text = highscoreEntry.time;
            entryTransform.Find("nameText").GetComponent<Text>().text = highscoreEntry.name;

            // Set background active if odd
            entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

            // Set first place to green
            if (rank == 1)
            {
                entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("timeText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
            }

            transformList.Add(entryTransform);
    }

    public static void AddHighscoreEntry(string time, string name)
    {
        // Create high score
        HighscoreEntry highscoreEntry = new HighscoreEntry { time = time, name = name };
        
        // Load existing high scores
        Highscores highscores = new Highscores();
        string json = PlayerPrefs.GetString("highscoreTable");
        if (!string.IsNullOrWhiteSpace(json))
        {
            highscores = JsonUtility.FromJson<Highscores>(json);
        }

        // Add new high score
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save high scores
        PlayerPrefs.SetString("highscoreTable", JsonUtility.ToJson(highscores));
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList = new List<HighscoreEntry>();
    }

    // Represents a single high score entry 
    [System.Serializable]
    private class HighscoreEntry
    {
        public string time;
        public string name;
    }
}
