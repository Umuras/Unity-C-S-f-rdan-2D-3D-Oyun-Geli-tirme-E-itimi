using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControl : MonoBehaviour
{

    Rigidbody physic; // Rigidbody componentine erişmek için değişken oluşturduk.
    public int speed; // Topumuzun daha hızlı hareket edebilmesi için değişken oluşturduk.
    int counter = 0; // Toplanan objelerin sayısını tutması için değişken oluşturduk.
    public int collectableObjectsNumbers; // Toplam toplanacak obje sayısını tutacağımız bir değişken oluşturduk.

    public Text counterText; // UI olarak ekranda yazı yazdıracağımız Text değişkeni oluşturduk.
    public Text gameoverText; // UI olarak ekranda yazı yazdıracağımız Text değişkeni oluşturduk.

    void Start()
    {
        physic = GetComponent<Rigidbody>(); // Rigidbody komponentinin verilerini physic değişkenine aktardık bu sayede erişim sağlamış olduk.
    }

    
    void FixedUpdate() //Fizik hareketlerini fixeduptade üzerinden yapıcaz. Aynı zamanda sabit hızda çalışan bir uptade türü
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //Burada sağ ve sol yönünde +1 ile -1 değeri arasında değer elde ediyoruz.
        float vertical = Input.GetAxisRaw("Vertical"); //Burada ise yukarı ve aşağı yönünden +1 ile -1 değeri arasında değer elde ediyoruz.

        Vector3 vec = new Vector3(horizontal, 0, vertical); // Burada Vector3 türünde yeni bir vec adında değişken oluşturuyoruz ve x ve z kısımlarına horizontal ve vertical adlı değişkenlerimizi yüklüyoruz.
                                                            // Bu sayede topumuz x ve z yönünde -1 ile +1 değeri arasında hareket edebilecekler.


        physic.AddForce(vec*speed); //Bu kod ile topumuza kuvvet uyguluyoruz. vec değişkenimiz topumuzu x ve z yönünde hareketini sağlıyor. speed değişkeni ile topumuzun daha hızlı hareket etmesini sağlıyoruz. 

    }

    void OnTriggerEnter(Collider other) //Burada isTrigger özelliği açık olan obje üzerinde işlem yapacağız. O objeye değdiği anda işlem başlar.
    {
        if (other.gameObject.tag == "enemy") // tag enemy ise işlem yapacak
        {
            other.gameObject.SetActive(false); // Burada topun değdiği objenin yok etmek yerine aktifliğini kapatacak.
            //Destroy(other.gameObject);// Bu kod ile objeye değildiği anda obje yok olur.
            counter++; // Her toplanacak obje toplandığında objenin sayaç 1 artacak.

            counterText.text = "COUNTER = " + counter; // Burada counterText değişkenimize COUNTER yazısını ve counter değişkenindeki sayı değerini sürekli olarak yazdırıyoruz. Her obje toplandığında

            if (counter == collectableObjectsNumbers) // Eğer sayacımız ile toplayacak olduğumuz objelerin sayısı birbiri ile aynı olursa
            {
                gameoverText.gameObject.SetActive(true); //gameoverText gameobjectimizi açıyoruz.
                gameoverText.text = "GAME OVER"; // ekrana GAME OVER yazdırıyoruz.
            }
        }
         
    }
}
