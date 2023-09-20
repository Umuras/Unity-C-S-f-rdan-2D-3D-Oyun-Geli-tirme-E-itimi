using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject skyOne; // Gökyüzü gameobjectlerini erişebilmek için bu değişkenleri oluşturduk.
    public GameObject skyTwo; // Gökyüzü gameobjectlerini erişebilmek için bu değişkenleri oluşturduk.

    Rigidbody2D physicOne; // Gökyüzü gameobjectilerini fizik olaylarını gerçekleştirebilmek için bu değişkenleri oluşturduk.
    Rigidbody2D physicTwo; // Gökyüzü gameobjectilerini fizik olaylarını gerçekleştirebilmek için bu değişkenleri oluşturduk.

    float skyGameobjectlength; // Gökyüzü gameobjectinin boxcollider sizeını içinde tutabilmek için oluşturduğumuz değişkendir.

    public float skySpeed; // Gökyüzü gameobjectlerinin hareket hızı değişkeni.

    public GameObject obstacle; // Engel objemize erişebilmek için bu değişkeni oluşturduk.
    public GameObject[] obstacles; // Birden fazla engel üzerinde işlem yapabilmek için dizi oluşturuyoruz.

    public int obstacleQuantity; // Ne kadar engel oluşturacağımızı giriyoruz.

    float changeObstacleLocationTime = 0; // Engellerin belirli bir zamanda tekrar oluşmasını sağlayan değişken.

    int obstacleCounter; // Dizi içindeki engellere erişebilmek için oluşturuldu.

    public Text ScoreText; // ScoreTextine veri yükleyebilmek için oluşturduk.
    public int score; // Skor verisini tutması için oluşturuldu.
    public float obstacleSpeed; // Engelin hızı için oluşturuldu.

    public bool gameOverControl; // Oyun bittiğin engelin pozisyon değiştirip tek bir noktada toplanmasını önlemek için oluşturuldu.
    

    void Start()
    {
        physicOne = skyOne.GetComponent<Rigidbody2D>(); // Gökyüzü1 ve 2 nini Rigidbody komponentlerini erişiyoruz.
        physicTwo = skyTwo.GetComponent<Rigidbody2D>(); // Gökyüzü1 ve 2 nini Rigidbody komponentlerini erişiyoruz.

        physicOne.velocity = new Vector2(skySpeed,0); // Gökyüzü1 in geriye doğru hareket etmesini sağlıyoruz.
        physicTwo.velocity = new Vector2(skySpeed,0); // Gökyüzü2 nin geriye doğru hareket etmesini sağlıyoruz.

        skyGameobjectlength = skyOne.GetComponent<BoxCollider2D>().size.x; // Gökyüzü1 in boxcollider sizenın x değerine erişiyoruz. Çünkü x ekseni üzerinde işlem yapacağız.
                                                                           // Gökyüzü2 için bir daha yapmadık çünkü ikisininde collider boyutları aynı.

        obstacles = new GameObject[obstacleQuantity]; // Engellerimiz için Gameobject türünde dizimizi tanımlıyoruz. obstacleQuantity dizinin eleman sayısını belirliyor.

        for (int i = 0; i < obstacles.Length; i++) //Dizi uzunluğu kadar döngümüzü çalıştırıyoruz.
        {
            obstacles[i] = Instantiate(obstacle,new Vector2(-20+i,-20),Quaternion.identity); // Dizideki her bir gameobjecti belirlenen konum doğrultusun oluşturuyoruz.
            Rigidbody2D physicObstacle = obstacles[i].AddComponent<Rigidbody2D>(); // Engel objelerini Rigidbody özelliği kazandırabilmek için her engelene AddComponent ile Rigidbody ekleyerek onu bir Rigidbody türündeki değişkene yüklüyoruz.
            //Engeller dizinin içinde aynı noktada oluşturuldukları için ilk anda hepsi üst üste oluşuyor ama dizi eleman sayısı ile toplayınca üst üste birikme sorunun kurtulmuş oluyoruz. // Bu sayede her bir engel objesinde fizik türünde işlem yapabileceğiz.
            physicObstacle.gravityScale = 0; // Engeller oluşturuldukları zaman yere düşmesin diye yer çekimini sıfırlıyoruz.
            physicObstacle.velocity = new Vector2(obstacleSpeed,0); // Karakterin üzerine doğru gelmesi için hızı yeni vektör tanımlayıp x yönünde - değerde tanımlıyoruz.
        }

        // Engellerin oluşturulma işlemi bir kere burada gerçekleşiyor sonra Update kısmında sürekli yerlerini değişim işlemi yapılıyor.

        
    }

    
    void Update()
    {
        if (gameOverControl == false)
        {
            if (skyOne.transform.position.x <= -skyGameobjectlength) // Eğer gökyüzü1in x eksenindeki pozisyonu gökyüzünü1in x ekseinindeki boxcollidersizeından küçükse işlem uygulanır.
        {                                                        // boxcollidersizeın - ile çarpılmasının sebebi gökyüzü pozisyon olarak geriye doğru gittiği için küçülmektedir, eğer pozitif olarak bırakılsaydı direk işlem uygulanacağında bu şekilde ayarlanmıştır.
            skyOne.transform.position += new Vector3(2*skyGameobjectlength,0); // Burada ise gökyüzü1in pozisyonu kendi pozisyonu ve 46.08 değeri ile toplanarak gökyüzü2 nin önüne geçmesi sağlanmıştır.
        }                                                                   // Şu şekilde son gökyüzü1in pozisyonu -23.04 + 46.08 = 23.04 yeni pozisyonu olacaktır. gökyüzü2 de o anda 0 a geldiği için gökyüzü1 onun önüne geçecektir.
        if (skyTwo.transform.position.x <= -skyGameobjectlength) // Eğer gökyüzü2nin x eksenindeki pozisyonu gökyüzünü2nin x ekseinindeki boxcollidersizeından küçükse işlem uygulanır.
        {
            skyTwo.transform.position += new Vector3(2*skyGameobjectlength,0); // Burada ise gökyüzü2in pozisyonu kendi pozisyonu ve 46.08 değeri ile toplanarak gökyüzü1 nin önüne geçmesi sağlanmıştır.
        }                                                                   // Şu şekilde son gökyüzü2in pozisyonu -23.04 + 46.08 = 23.04 yeni pozisyonu olacaktır. gökyüzü1 de o anda 0 a geldiği için gökyüzü2 onun önüne geçecektir.
        


        //---------------------------------------------------------------------------------------------------------
        changeObstacleLocationTime += Time.deltaTime; // 2 saniye de bir işlem yapabilmek için bu yapıyı kullanıyoruz.
        if (changeObstacleLocationTime > 2) // 2 değişkenimiz 2 değerini geçtiği an if'in içine giriyor.
        {
            changeObstacleLocationTime = 0; // if e tekrar girebilmesi için değişkenimizi 0 a eşitliyoruz.
            float Yaxis = Random.Range(-0.6f,1.37f); // Engelimiz en alt ve en üst noktaların arasında oluşturulabilmesi için bu değişkeni oluşturuyoruz.
            obstacles[obstacleCounter].transform.position = new Vector3(14.6f,Yaxis); // Burada yukarıda instaitate ettiğimiz engellerin 2 saniyede 1 hangi konumda olması gerektiğini ayarlıyoruz.
            // Engelin her zamanın kamerada görünün 14.6 pozisyonuna düşmesinin sebebi çünkü kameranın gördüğü alandaki pozisyon değişmiyor, engeller ve gökyüzü karaktere doğru geliyor.
            obstacleCounter++; // Oluşturulan dizi içerisinde hareket edilebilmesi için bu sayacı kullanıyoruz.
            if (obstacleCounter >= obstacles.Length) // Eğer sayaç dizi eleman sayısından büyük ve eşit bir değere ulaşırsa sayacı tekrar sıfırlayarak işlemin devam etmesini sağlıyoruz.
            {
                obstacleCounter = 0;
            }
        }

        
        }
    }


    public void GameOver()
    {
        for (int i = 0; i < obstacles.Length; i++) // Bütün engellerimize for döngüsü sayesinde erişiyoruz.
        {
            obstacles[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Engellerimizin sırasıyla hızını 0 lıyoruz.
            physicOne.velocity = Vector2.zero; // Gökyüzü1 in hızını sıfırlıyoruz.
            physicTwo.velocity = Vector2.zero; // Gökyüzü2 nin hızını sıfırlıyoruz.
        }
        gameOverControl = true; // Engellerin 2 saniye bir aynı pozisyona gelerek üst üste binmesini engelliyoruz.
    }
}
