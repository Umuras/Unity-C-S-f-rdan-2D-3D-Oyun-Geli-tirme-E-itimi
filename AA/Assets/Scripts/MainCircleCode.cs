using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCircleCode : MonoBehaviour
{
    public GameObject smallCircle; // Burada çubuklu çemberi oluşturmak için Gameobject türünde değişken oluşturduk.

    GameManager gameManager; // GameManager scriptindeki methoda erişmek için oluşturduk.
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager gameobjectimizi bularak onun özelliklerini gameManager değişkenimize aktardık.
    }

    
    void Update()
    {
        SmallCircleCreate(); // Update de her frame de çağırıyoruz fonksiyonumuzu
    }


    void SmallCircleCreate() // Burada çubuklu çemberi oluşturmak için SmallCircleCreate adlı methodu oluşturduk.
    {
        if (Input.GetMouseButtonDown(0)) // Eğer sol tıka basarsan 
        {
            Instantiate(smallCircle,transform.position,Quaternion.identity); // smallCircle çubuklu çemberimiz, transform.position yazan yer MainCircle gameobjectinin konumu, Quarternion.identity de rotaston bilgileri.
            gameManager.SmallCircleAmountDecrease(); //SmallCircleAmountDecrease değişkenine erişim sağladık.
        }
    }
}
