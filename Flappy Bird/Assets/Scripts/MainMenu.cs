using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int HighScore; // En yüksek skoru tutacak değişkeni oluşturuyoruz.
    public Text ScoreText; // En yüksek skoru ana menüde text olarak yazdıracak Text değişkenini oluşturuyoruz.
    public Text NormalScoreText; // Normal skoru ana menüde text olarak yazdıracak Text değişkenini oluşturuyoruz.
    int normalScore; // Normal skoru tutacak değişkeni oluşturuyoruz.
    void Start()
    {
        HighScore = PlayerPrefs.GetInt("highscore"); //Control scriptinde kayıt edilmiş olan highscore a erişip değişkenimize yüklüyoruz. 
        ScoreText.text = "HIGHEST SCORE = " + HighScore; // Text üzerinde en yüksek skoru ekrana yazdırıyoruz.
        normalScore = PlayerPrefs.GetInt("normalscore"); //Control scriptinde kayıt edilmiş olan normalscore a erişip değişkenimize yüklüyoruz.
        NormalScoreText.text = "NORMAL SCORE = " + normalScore;  // Text üzerinde normal skoru(O anki elde edilmiş skor) ekrana yazdırıyoruz.
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Start Game butonuna basıldığı zaman oyun başlıyor.
    }
    public void ExitGame()
    {
        Application.Quit(); // ExitGame butonuna basıldığı zaman oyunda çıkıyor.
    }

    public void DeleteScore()
    {
        PlayerPrefs.DeleteAll(); // DeleteScore tuşuna basınca bütün skorlar sıfırlanıyor.
    }
}
