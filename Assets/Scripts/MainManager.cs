using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    private string User;
    public Text BestUser;
    
    private bool m_Started = false;
    private int m_Points;
    private static string bestPlayer;
    private static int bestScore;
    
    private bool m_GameOver = false;

    public static MainManager Instance;

    private void Awake()
    {
        LoadStats();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        User = GameManager.Instance.playerName;
        BestUser.text = $"{bestPlayer}: {bestScore}";
        ScoreText.text = $"{User}";
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene(0);
            }
            SaveStats(bestPlayer,bestScore);
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{User}: {m_Points}";
    }

    void SetBest()
    {
        if (m_Points > bestScore){
        bestPlayer = User;
        bestScore = m_Points;
        }
    }
        

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SetBest();
    }

    private void HighestScore(){
        int Currentscore = GameManager.Instance.score;
    }


    [System.Serializable]
    class SaveData
    {
        public string bestPlayer;
        public int bestScore;
    }

    public void SaveStats(string bestPlayer, int bestScore){
        SaveData data = new SaveData();
        data.bestPlayer = bestPlayer;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
    
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadStats(){
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;

        }
    }



}
