using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroybyContact : MonoBehaviour
{
    public GameObject Explode; // Asteroid particle efektini yükleyebilmek için oluşturduğumuz değişken.

    GameManager gameManager; // GameManager classına erişebilmek için oluşturduğumuz değişken.
    GameObject gamemanagerReach; // GameManager gameobjectine erişebilmek için oluşturduğumuz değişken.

    void Start()
    {
        //gameManager = GameObject.FindObjectOfType<GameManager>(); Benim yazdığım GameManager gameobjectine erişip oradan scripteki method a erişmek için yazdığım kod.
        gamemanagerReach = GameObject.FindGameObjectWithTag("gamemanager"); // Burada tag üzerinden gamemanagerReach gameobjectine GameManager gameobjectini yüklüyoruz.
        gameManager = gamemanagerReach.GetComponent<GameManager>(); // Burada ise gamemanagerReach de artık GameManager yüklü olduğu için GetComponent üzerinden onun componentlerine erişim sağlayıp GameManager türündeki değişkenimize yükleyerek GameManager scriptimize artık erişebilir hale geliyoruz.
    }
     void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag != "Border") //Eğer objenin tagi Border  değil ise
        {
            Destroy(other.gameObject); // ilk önce Border tagli olmayan obje yok oluyor. other parametresi ile kullanırsan asteroid isTriggerlı ve tagi enemy olan objeye değdiğinde onu yok eder.
            Destroy(gameObject); // Sonrada asteroidimiz yok oluyor.
            Instantiate(Explode,gameObject.transform.position,transform.rotation); //Burada asteroidimiz yok olduğu zaman particle efektin orada çalışmasını sağladık.
            // Explode dediğimiz particle efekt gameobjecti, gameObject.transfor.position asteroidin pozisyonu böyle yazmamızın sebebi asteroidin konumunda efektin çalışmasını istiyoruz.
            //transform.rotation rotasyon işlemi için direk asteroidin değerlerini alıyor.
            gameManager.ScoreIncrease(10); // Burada ise değişken üzerinden ScoreIncerease methoduna erişip score değerini her asteroid yok olduğunda 10ar 10 ar artmasını sağladık.
        }
    }


}
