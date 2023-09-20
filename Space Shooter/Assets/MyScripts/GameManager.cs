using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Asteroid; // Burada asteroidi GameManager içine koyup işlem yapabilmek için oluşturuyoruz.
    public Vector3 randomPos; // Burada asteroidimizin x ve z ekseninde hangi noktalarda doğmasını belirlemek için bu vektörü oluşturuyoruz ve inspector paneli üzerinden değerleri giriyoruz.
    public float initialTime; // Kod başladığında döngüye girmeden beklediği saniye;
    public float asterioidCreatingTime; // Asteroidleri hangi zaman aralığında oluşturacağı saniye;
    public float startLoopWaitTime; // For döngüsü tamamlanıp tekrar for döngsüne girmeden bekleme süresi, yani 10 adet asteroidin oluşma süresi bittiken sonra bekleyip tekrar oluşmasını sağlayan süre;
    private int score; //Burada gemimiz asteroidlere vurduğu zaman ekranda gözükecek skor değeri için oluşturulmuş bir değişkendir.

    public Text ScoreText; // Burada ekranda gözükecek olan skor textimize değer yazdırabilmek için oluşturduk.

    public Text GameOverText; // Burada ekranda gemi ve asteroid çarpıştığı zaman Gameover texti gözükecek.
    public Text TryAgainText; // Burada ekranda gemi ve asteroid çarpıştığı zaman Gameover textinden biraz sonra gözükecek.

    public bool cancelingAsteroidCreating = false; // Burada asteroid oluşumunu engellemek için oluşturduk.

    public bool tryAgainGame = false; // Burada R tuşuna basıldığında oyunu yeniden başlatabilmek için oluşturduğumuz değişken.
    void Start()
    {
        score = 0; // Oyun başladığında skor değerimizin 0 gözükmesi için sıfıra eşitliyoruz.
        ScoreText.text = "Score:" + score; // Oyun başladığı anda ekrana Score ve skorun 0 olduğu değeri yazdırıyoruz.
       //Create(); // Bu şekilde de başlangıçta bir kere başlatmış oluyoruz.
        //InvokeRepeating("Create",3,1); Bu fonksiyon ile her 3 saniye 1 kere sürekli olarak asteroidimizi rastgele bölgeler de üretmiş oluyoruz.
        StartCoroutine("Create"); // Burada Coroutine başlatmak için yazdığımız kod.
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && tryAgainGame) // Burada ise R tuşuna bastığımızda ve değişkenimiz true ise oyunu yeniden başlatıyoruz.
        {
            SceneManager.LoadScene("SampleScene"); // Burada ise SampleScene adlı sahnemizi yükleterek  oyunun yeniden başlamasını sağlıyoruz.
        }
    }


    IEnumerator Create() //Burada IEnumerator yapmamızın sebebi zamanı kullanarak işlem yapmamız içindir.
    {
        yield return new WaitForSeconds(initialTime); //Fonksiyon ilk başladığında 2 saniye bekliyoruz.

        while(true) // Asteroid üretimi sürekli tekrarlansın diye while(true) yazarak sonsuz döngüye sokuyoruz.
        {
            for (int i = 0; i < 10; i++) //Buradaki for döngüsünde 10 tane asteroid oluşmasını sağlıyoruz.
            {
                Vector3 randomVec = new Vector3(Random.Range(-randomPos.x,randomPos.x),0,randomPos.z); // Burada yeni bir vektör oluşturup yukarıda belirlemiş olduğumuz vektör değerleri üzerinden Rastgele bir şekilde x de -6 ile 6 arasında, z ise direk sadece 12 değerinde asteroid konumlandırıyoruz.
                Instantiate(Asteroid,randomVec,Quaternion.identity); // Burada ise Asteroid objemiz ilk başta, randomVec ise rastgele oluşacak asteroidimizin konum bilgilerini tutan vektör3 türünde değişken oluyor.
                yield return new WaitForSeconds(asterioidCreatingTime); // Her asteroid oluştuktan sonra 1 saniye beklemesini istiyoruz.
            }
            if (cancelingAsteroidCreating == true) // Burada gemimiz eğer asteroid ile çarpıştıysa if içindeki değişken true olup içine giriyor.
            {
                tryAgainGame = true; // Bu değişkeni true yaparak R tuşuna bastığımızda oyunu yeniden başlatıyoruz.
                TryAgainText.text = "Press 'R' Button for Try Again"; // Ekrana yazıyı yazdırıyoruz.
                break; // break komutu ile while döngüsünün içinden çıkarak bir daha asteroid oluşumunu engelliyoruz.
            }


            yield return new WaitForSeconds(startLoopWaitTime); // 10 adet asteroid oluştuktan sonra 5 saniye bekleyip tekrardan fora girip 10 adet daha asteroid oluşuyor.


        }
    
       
    }

    public void ScoreIncrease(int incomingScore) // Bu methodu mermimiz asteroidi yok ettiği zaman skorumuzu arttırabilmek için yazıyoruz ve başka bir scriptten erişebilmek için public olarak ayarlıyoruz.
    {
        score += incomingScore; // Skor değerimiz methodumuz parametre değeri ne verilirse her asteroid yok olduğu zaman değeri o kadar artacaktır.
        ScoreText.text = "Score:" + score; // Skorumuz her arttığında yeni skor değeri ekrana yazdırılıyor.
    }



}
