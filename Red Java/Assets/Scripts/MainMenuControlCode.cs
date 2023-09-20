using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControlCode : MonoBehaviour
{
    GameObject levels; // Bütün level butonlarını tek bir değişken üzerinden erişmek için oluşturduk
    GameObject locks; // Bütün kilit resimlerini tek bir değişken üzerinden erişmek için oluşturduk
    void Start()
    {
        locks = GameObject.Find("Locks");  // Burada Locks gameobjectine erişiyoruz.

        for (int i = 0; i < locks.transform.childCount; i++) //Burada locks gameobjectinin içindeki kilitlerin resminin adedince dönme işlemi yapıyoruz.
        {
            locks.transform.GetChild(i).gameObject.SetActive(false); //Burada ise transform.GetChild ile objelere erişiyoruz. birer gameobject oldukları için GetChild dan sonra .gameobject diyerek daha sonra da aktifliğini kapatıyoruz.
        }

        levels = GameObject.Find("LevelsButton"); // Burada Levels gameobjectine erişiyoruz.

        for (int i = 0; i < levels.transform.childCount; i++) // Kilit resimlerini için uygulanan sistemin aynısı uygulanıyor.
        {
            levels.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        for (int i = 0; i < PlayerPrefs.GetInt("whichLevel"); i++) // Buradaki for döngüsü sayesinde karakterimizin bulunduğu seviye değeri en son hangisi ise o kadar dönüş yapıyor.
        { // Eğer karakter 2.seviyedeyse 2 kere dönüyor ve 0. ve 1. elemana erişip o butonların interactable özelliğini true yapıyor ve tıklanır hale getiriyor.
            levels.transform.GetChild(i).GetComponent<Button>().interactable = true; //levels gameobjectinin child objelerine erişip onların tıklanabilir hale getiriyoruz.
        }
      
    }

    public void ButtonWorks(int getButton) //Burada butonlara çalışma özelliği kazandıracak methodu yazdık.
    {
        if (getButton == 1) // Eğer bu script butona atılıp parametre olarak 1 verilirse 
        {
            SceneManager.LoadScene("NewLevel"); //Oyun ilk bölümden başlayacaktır.
        }
        else if (getButton == 2) // Eğer parametre olarak 2 verilirse
        {
            for (int i = 0; i < levels.transform.childCount; i++) //döngü ile dönüp levels gameobjectinin child objesi olan level butonlarını görünür hale getiriyoruz.
            {
                levels.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 0; i < locks.transform.childCount; i++) //döngü ile dönüp Locks gameobjectinin child objesi olan lock resimlerini görünür hale getiriyoruz.
            {
                locks.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 0; i < PlayerPrefs.GetInt("whichLevel"); i++) // Burada ise eğer oyunda en son hangi seviye açıldıysa oraya kadar kilit resmini kaldırıyoruz, diğer seviyeler açılmadığı için kilit resmi kalıyor.
            {
                locks.transform.GetChild(i).gameObject.SetActive(false);
            }



        }
        else if (getButton == 3) // Eğer parametre olarak 3 verilirse
        {
            Application.Quit(); // Exit Game butonu olduğu için oyundan çıkılacaktır.
        }
        else if (getButton == 4) // Bu butona basıldığı zaman oyunun bütün kayıt sistemi silinecektir.
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void LevelsButton(int levelindex)
    {
        if (levelindex == 1) //Burada ise level1 2 3 butonlarına parametreleri atadığımızda level1 butonu Level1 i level2 butonu Level2 yi Level3 butonu Level3 ü açacaktır.
        {
            SceneManager.LoadScene("NewLevel");
        }
        else if (levelindex == 2)
        {
            SceneManager.LoadScene("NewLevel2");
        }
        else if (levelindex == 3)
        {
            SceneManager.LoadScene("NewLevel3");
        }
    }

    
}
