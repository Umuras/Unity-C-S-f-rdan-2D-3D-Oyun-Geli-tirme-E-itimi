using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public Sprite[] BirdsSprites; // Burada karakterin spritelarını dizi içine yükleyebilmek için oluşturduk.
    SpriteRenderer birdsprite; // Burada sprite komponentine erişebilmek için oluşturduk.
    int birdsCounter = 0; // Dizinin içindeki elemanların arasında geçiş yapabilmek için oluşturduk.
    bool forwardbackward = true; // Dizide sona ya da ilk başa gelince tekrardan dizide yer değiştirebilmek için bu değişkeni oluşturduk.

    float birdanimationtime = 0; // Oyunda karakter çok hızlı kanat çırptığı için onun hızını yavaşlatabilmek için oluşturduk.

    GameManager gameManager; // GameManager scriptindeki verilere ulaşabilmek için oluşturduk.

    private bool birdStop = true; // Engele çarpan karakteri durdurmak için oluşturduk.

    Rigidbody2D physic; // Rigidbody komponentine erişebilmek için oluşturduk.

    AudioSource[] sourceSounds; // Sesleri çaldırabilmek için oluşturduk. Birden fazla AudioSource komponentine erişip hepsini ayrı ayrı çaldırabilmek için oluşturduk.

    int highestScore = 0; // En yüksek skoru tutacak değişken.

    
    void Start()
    {
        birdsprite = GetComponent<SpriteRenderer>(); // Değişkene sprite komponentinin özelliklerini kazandırdık.
        physic = GetComponent<Rigidbody2D>(); // Değişkene rigidbody komponentinin özelliklerini kazandırdık.
        gameManager = FindObjectOfType<GameManager>(); //GameManager ismindeki Gameobjecti bulup onun özelliklerini değişkene aktarmış olduk.
        sourceSounds = GetComponents<AudioSource>(); // Birden fazla AudioSource olduğu için GetComponents yapısını kullanıyoruz.
        highestScore = PlayerPrefs.GetInt("highscore"); // Oyun başladığında 0 olan highestscoreu değil de yüklü olan değerde başlaması için bu kodu kullanıyoruz.
        //gameManager.score = 0; // Sıfır değerinin algılaması için score değişkenine oyun başladığında 0 değerini atıyoruz.
    }

    
    void Update()
    {
        BirdsSpritesAnimation();
        Movement();
    }
    void BirdsSpritesAnimation() // Spritelar arasında sürekli geçiş yaparak animasyon oluşturabilmek için bu methodu oluşturduk.
    {
        birdanimationtime += Time.deltaTime; // Burada değişkenimiz zamanla toplanarak 0,1 + 0,2 +0,3 gibi en sonunda 1 i geçtiği anda tekrar sıfır yaparak 1 saniye de bir sprite geçişini sağlıyoruz.
        if (birdanimationtime > 0.2f) // Son hali 0.2f e indirerek saniyemizi animasyonun daha hızlı gözükmesini sağlıyoruz.
        {
            birdanimationtime = 0;
            if (forwardbackward) // Eğer doğru ise değişkenimiz
        {
            birdsprite.sprite = BirdsSprites[birdsCounter]; // BirdsSprites dizisindeki spriteları sırayla sprite Renderer komponentine yüklüyoruz.
            birdsCounter++; // Sayaç sayesinde 0.1.2. dizi elemanlarına doğru hareket edebiliyoruz.
            if (birdsCounter == BirdsSprites.Length) // Eğer sayacımız 3 e ulaşırsa forwardbackward = false yapıp else durumuna geçiyoruz.
            {
                birdsCounter--; // Burada değeri bir eksiltmemizin sebebi else durumunda dizinin değeri 2 değeri olarak çalışıyor 
                //ama zaten yukarıdaki dizi kısmında en son 2 değeri çalıştığı için 2 saniye boyunca aynı spriteda kalmış gibi gözüküyor.
                forwardbackward =false;    
            }
            
        }
        else // Burada ise 0 1 2 şeklinde giden dizi üzerinde 2 1 0 şeklinde giderek sürekli olarak çalışan bir animasyon oluşumunu sağlıyoruz.
        {
            birdsCounter--; // Sayacı 1 azaltıp 2 değerini elde ederek son spriteı ilk başta ekranda görüyoruz.
            birdsprite.sprite = BirdsSprites[birdsCounter]; // 2 1 0 şeklinde hareket ediyoruz.
            if (birdsCounter == 0) // Eğer 0 olursa forwardbackward true yaparak 0 1 2 şeklinde hareket eden kısma geçiş yapıyoruz.
            {
                birdsCounter++; // Burada da aynı şekilde 0 a ulaşınca yukarıda tekrar 0. spriteı ekranda göstermesin diye arttırım yapıyoruz.
                forwardbackward = true;
            }
        }
        }
        
    }

    void Movement()
    {
        if (Input.GetMouseButtonDown(0) && birdStop == true)
        {
            physic.velocity = Vector2.zero; // Hızı sıfıra eşitlememizin sebebi düşen cismin hızı artığından dolayı bu yüzden uygulanan yer çekimini daha fazla olduğu için hızı sıfırlıyoruz.
            physic.AddForce(Vector2.up*200); // Y ekseni yönünde güç uyguluyarak karakterin yukarı doğru çıkmasını sağlıyoruz.
            sourceSounds[0].Play(); // İlk AudioSource komponentine erişerek kanat çırpma sesini çalacaktır.
        }
          if (physic.velocity.y > 0) // y eksenindeki hızımız 0 dan büyük olduğu anda çalışacaktır.
            {
                transform.eulerAngles = new Vector3(0,0,45); // Karakterin yukarı doğru rotasyonu z ekseni üzerinden olmaktadır.
            }
            else // y ekseninde hızımız 0 dan küçük olduğu anda çalışacaktır.
            {
                transform.eulerAngles = new Vector3(0,0,-45); // Karakter aşağı doğru düştüğünde ise aşağı dönük bir şekilde düşecektir.
            }
            //mouse sol tık ifin içinden çıkarmamızın sebebi sol tıka her basıldığında hız sıfır oluyor ve else durumu aktif oluyor bu yüzden çıkarmak zorundaydık.
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "obstacle") //Engellerin videosunun son kısmına tekrar bak ve engele çarpınca karakter neden durmuyor bak.
        {
            // Eğer engele çarparsa karakterimiz, engeller ve gökyüzünün hareketi duracak.
            birdStop = false; // Bu şekilde karakterimize bir daha tıklayamıyoruz.
           // gameManager.skySpeed = 0; // Gökyüzünün hareket hızını sıfırlayarak hareketini durduruyoruz.
           // gameManager.obstacleSpeed = 0; // Engelin hareket hızını sıfırlayarak hareketini durduruyoruz.
            gameManager.GameOver();
            sourceSounds[1].Play(); // İkinici AudioSource komponentine erişerek çarpma sesini çalacaktır.
            GetComponent<CircleCollider2D>().enabled = false; // Bunu yapmamızın sebebi karakterin colliderını kaldırarak yer düşerken başka engel tagli collidera sahip objelere değince sesin çalmasını engellemek için yazıldı.
            if (highestScore < gameManager.score)
            {
                highestScore = gameManager.score; // skoru highestScore a yükleyerek en yüksek skoru aktarmış oluyoruz.
                PlayerPrefs.SetInt("highscore",highestScore); // En yüksek skoru bu kod ile kaydediyoruz.
                
                Debug.Log("En yüksek yeni skor= " + highestScore); // Ekrana yazdırıyoruz.
            }
            else
            {
                Debug.Log("En yüksek skor:" + highestScore); // Eğer yukarıdaki şart sağlanmazsa direk en son yüklenen yüksek skoru yazdırıyoruz.
            }
            
            Invoke("ReturnMainMenu",2); // 2 saniye geçtikten sonra oyun ana menüye geçecek.
            
            
        }
        if (col.gameObject.tag == "score")
        {
            gameManager.score++; // Eğer engelin içinden geçerse skoru artırıp ekrana yazdırıyoruz.
            sourceSounds[2].Play(); // Üçüncü AudioSource komponentine erişerek puan toplama sesini çalacaktır.
            gameManager.ScoreText.text = "SCORE: " + gameManager.score;
        }
        
    }
    //PlayerPrefs yukarı kısımdayken  -if (col.gameObject.tag == "score") bunun içinde engellerin içindeki puan alacağı kısımdan geçmeden çalışmadığı için 0 değerini kaydedemeyip Texte yazdıramıyorduk.
    void ReturnMainMenu() // Invoke methodunda çalışması için bu methodu oluşturduk.
    {
        PlayerPrefs.SetInt("normalscore",gameManager.score); // Ama burada engele çarptığı an bu kod çalıştığı için ve skor değeri 0 a eşit olduğu için bu değer tutulup texte yazdırılıyor.
        SceneManager.LoadScene("MainMenu"); //2 saniye sonra MainMenu sahnesine geçecek.
    }
}
