using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public void DeleteGameData()
    {
        PlayerPrefs.DeleteAll(); // Bütün tutulan kaydı silmeye yarıyor. Yani kaldığınız yerden değil de 1.bölümden tekrar başlıyorsunuz.
    }
    public void PlayGame()
    {
        int SaveLevelVariable = PlayerPrefs.GetInt("savelevel"); // Burada kaydını oluşturduğumuz değere tekrar erişim sağlayıp int türünden bir değişkene atayarak daha rahat işlem yapabilmek için yapıyoruz.
        if (SaveLevelVariable == 0)
        {
            SceneManager.LoadScene(SaveLevelVariable + 1); // Eğer mevcut kayıtlı level 0 ise 0.level ana menü olduğu için 1.leveldan başlatabilmek için +1 ile topluyoruz.
        }
        else
        {
            SceneManager.LoadScene(SaveLevelVariable); // Play Game butonuna bastığı zaman en son kalınan seviye açılacaktır.
        }
        
    }
    public void ExitGame() 
    {
        Application.Quit(); // Burada ise oyun build alındığı zaman eğer Exit Game butonuna basarsa oyundan çıkacaktır.
    }
}
