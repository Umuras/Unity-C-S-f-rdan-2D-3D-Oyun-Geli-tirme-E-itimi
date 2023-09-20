using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLineCode : MonoBehaviour
{
    Rigidbody2D physic; //Burada Rigidbody komponentine erişmek için bu değişkeni oluşturuyoruz.
    public float speed; // Burada çembere hareket edecek olan çubuklu çembere hız kazandırmak için bu değişkeni oluşturuyoruz.

    private bool movementControl = false; // Çubuklu çember büyük çembere değdiği zaman çubuklu çemberi durdurmak için yazıyoruz.

    GameManager gameManager; // GameOver methoduna erişebilmek için GameManager türünde değişken oluşturuyoruz.

    void Start()
    {
        physic = GetComponent<Rigidbody2D>(); // physic değişkenine Rigidbody2D komponentinin özelliklerini aktarıyoruz.
        gameManager = FindObjectOfType<GameManager>(); // Bu şekilde GameManager gameobjectimize erişiyoruz.
    }

    
    void FixedUpdate() //Fizik işlemleri yaptığımız için FixedUpdate de yazıyoruz.
    {
        if (!movementControl) // Eğer movementControl false ise çalışıyor.
        {
            //physic.velocity = Vector2.up*speed;
            physic.MovePosition(physic.position+Vector2.up*speed*Time.deltaTime); //physic.MovePosition objenin bulunduğu pozisyondan vektör,hız ve zamanın birbiri ile çarpılıp pozisyona eklenmesi ile objenin hareketi sağlanmıştır.
        }
            
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "spiningcircletag") // Eğer çubuklu çember spiningcircletagli büyük çembere çarparsa bu işlem gerçekleşiyor.
        {
            movementControl = true; //Değişkeni true yaparak çubuklu çemberin hareketini sonlandırıyoruz.
            transform.SetParent(col.transform); // Dönen çember ile çubuklu çember çarpıştığı zaman SetParent(col.transform) diyerek
                                                // Çubuklu çemberi dönen çemberin child objesi haline getirerek onla birlikte dönmesini sağlıyoruz.
        }
        if (col.gameObject.tag == "smallcircletag") // smallcircletag indeki objeler birbiri ile çarpıştıkları zaman GameOver methodu çalışacak.
        {
            gameManager.GameOver(); // GameManager scriptindeki GameOver methoduna eriştik.
        }
    }


    
}
