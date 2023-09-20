using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RedJavaControl : MonoBehaviour
{
    public Sprite[] waitAnim; // Karakterin idle animasyonunu oluşturacak spritelar
    public Sprite[] jumpingAnim; // Karakterin zıplama animasyonunu oluşturacak spritelar
    public Sprite[] runningAnim; // Karakterin koşma animasyonunu oluşturacak spritelar
    SpriteRenderer spriteRender; //Sprite Renderer komponentine erişmek için oluşturduk
    int waitingAnimCounter = 0; // idle animasyon dizisindeki elemanlar arasında hareket edebilmek için oluşturduk
    int runningAnimCounter = 0; // running animasyon dizisindeki elemanlar arasında hareket edebilmek için oluşturduk
    float horizontal = 0; // sağ ve sol ok tuşlarına basınca karakterin hareket değerlerinin oluşması için yazdık.
    Rigidbody2D physic; // Rigidbody komponentine erişmek için yazdık.
    Vector3 vec; // Karakterin belli bir yönde hareket yapabilmesi için bu vektörü oluşturduk.

    public bool isJump=true; // Zıplama durumunu kontrol eden değişken;
    float waitingAnimTime; //idle durumundaki animasyon işlemleri için oluşturulmuş zaman değişkeni
    float runningAnimTime; //running durumundaki animasyon işlemleri için oluşturulmuş zaman değişkeni
    Vector3 distanceCamera; // Kamera ile karakter arasındaki mesafeyi tutacak değişken;
    GameObject Camera; // Kamera gameobjectine erişmek için oluşturuldu.
    Vector3 cameraLastPosition; //Kameranın son konumunu bu değişken üzerinde tutuyoruz.
    public Text Health; // Can seviyemizi azaltacak texti atayacağımız;
    int health = 100; // Canımınızın değeri tutulacak;
    public Image GameOverBackground; //Karakterin canı 0 a indiği anda ekran kararıcak.
    float backgroundValue = 0; //Arka planı siyah yapan değer;
    float GotoMainMenuTime; // Karakterin canı 0 a indikten sonra belirlenen zaman sonra ana menüye geçiş sağlaması için oluşturuldu.

    AddHealthChest chest; // AddHealthChest scriptine erişebilmek için oluşturduk.
    public Text CointText; // Coin değerini ekrana yazdırmak için oluşturduk.
    int coinQuantity = 0; // Coin miktarını tutması için oluşturduk.
    public float Vertz;

    public bool inAirNotJump = true; // Karakterin havadayken zıplamaması için oluşturuldu.
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>(); //Sprite Rendere komponentine erişim sağladık.
        physic = GetComponent<Rigidbody2D>(); // Rigidbody2D komponentine eriştik.
        Camera = GameObject.FindGameObjectWithTag("MainCamera"); // Camera Gameobjectine MainCamera gameobjectini yükledik.
        
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("whichLevel")) // Burada ilk önce 1. seviyedeyken kayıt sistemimizde değer bulunmadığı için 1.seviye değeri kaydedilecek. Sonra 2 ve 3 şeklinde devam edicek. En sonunda 1.seviye tekrar oynandığında en son kayıtlı 2 veya 3 olduğu için tekrardan 1.levelı kaydetmeyecek ve diğer level butonlarının görünürlüğü kapanmayacak.
        {
        PlayerPrefs.SetInt("whichLevel",SceneManager.GetActiveScene().buildIndex);//Oyun başladığında karakter kaçıncı seviyede bulunuyorsa mevcut sahnenin index numarası tutulacak. Index numarası build settings deki Scenes in Build deki sayı.
        }
        distanceCamera = Camera.transform.position - transform.position; // Kamera ile karakterin posizyonun birbirinden çıkararak aralarındaki mesafeyi ölçtük.
        Health.text = "HEALTH: " + health; // Oyuna ilk başladığı anda HEALTH yazısını ve health değerini yazdırıyoruz
        
        Time.timeScale = 1; // GameOver olduğunda hız 0.5 e düştüğü için yeni seviye başladığı zaman tekrar normal hızda oyunun başlaması için oluşturduk

        chest = GameObject.FindGameObjectWithTag("addhealth").GetComponent<AddHealthChest>(); // FindGameObjectWithTag ile tag üzerinden gameobjecte erişip oradan AddHealthChest scriptine erişiyoruz.

        CointText.text = "COIN 30/" + coinQuantity; // Oyun ilk açıldığın ekrana yazılacak Coin yazısı ve değeri
    }
    void Update()
    {
        Vertz = SimpleInput.GetAxis("Vertical"); // Joysticler GetAxis üzerinden değer aldığı için bu şekilde yazdık.
        if (Vertz > 0.5f) //(Input.GetKeyDown(KeyCode.Space))
        {
            if (isJump && inAirNotJump)
            {
            physic.AddForce(new Vector2(0,500));// karakterin zıplaması için y ekseninde kuvvet uyguluyoruz.
            physic.velocity = Vector2.zero;
            }

            isJump= false;

        }
    }
    
    void FixedUpdate()
    {
        CharacterMovement();
        Animation();
        FollowCharacter();
        GameOver();
    }
    void GameOver()
    {
        if (health <= 0) //Eğer karakterin canı 0 eşit veya küçükse
        {
            Time.timeScale = 0.5f; // Oyun yavaşlıyor
            Health.gameObject.SetActive(false); // Can gösteren text yok oluyor.
            backgroundValue += 0.02f; // Ekranı karartan görüntü değeri yavaş yavaş 255 değerine doğru arttırılıyor.
            GameOverBackground.color = new Color(0,0,0,backgroundValue); //Burada Image gameobjectinin colorına erişip oradan Alpha kısmını 255 e doğru yükseltiyoruz bu sayede ekran kararıyor.
            
            GotoMainMenuTime += Time.deltaTime; // Burada da sayaç koyuyoruz 0 dan başlayıp oyun başladığı andan itibaren değerimiz yükseliyor
            if (GotoMainMenuTime > 1.7f) // 1.7f i geçtiğimiz an zaman olarak
            {
                SceneManager.LoadScene("MainMenu"); // MainMenu sahnesine geçiş yapıyoruz.
            }
        }
    }
    void CharacterMovement()
    {
        horizontal = SimpleInput.GetAxisRaw("Horizontal"); //GetAxis olduğundan 0 ile 1 arasındaki artış yavaş biçimde başladığı için karakter önce yavaş hareket edip sonra hızlanıyor.
                                                    //GetAxisRaw kullanıldığı zaman 0 dan 1 e direk geçiş olduğu için bu yavaştan başlama olayı ortadan kalkıyor.
        vec = new Vector3(horizontal*10,physic.velocity.y,0); // X ekseninde hareket yapmasını sağlıyoruz, bu şekilde karakterin. Eğer y ekseni 0 kalırsa yer çekiminin uyguladığı kuvveten ötürü aşağı çekilen karakteren hızla duşeceği halde hızı 0 olarak zorlandığı için yavaşça düşüyor ama y eksenindeki hızını atarsak bu sefer daha hızlı düşecektir.
        physic.velocity = vec; // Karakterimizin hızına vec değişkenimizi yükleyerek karakterin ivmelenmesini sağlıyoruz.

    }   
    void OnCollisionEnter2D(Collision2D other) //Colliderı olan herhangi bir obje ile temas ettiğinde çalışıyor.
    {
        isJump = true; //Karakter yere değdiği anda bu değişken true olup karakter tekrardan zıplayacak hale gelecektir.
    }
    void Animation()
    {
        if (isJump == true)
        {
            if (horizontal == 0) // Bu durumda karakterimizin hızı 0 olacaktır dolayısıyla idle animasyonu çalışacaktır.
        {
            waitingAnimTime += Time.deltaTime; // Burada sürekli Time.deltatime ile toplayarak değişken üzerinde zamanı yüklüyor.
            if (waitingAnimTime > 0.1f) // Eğer zaman 0.1f saniyeden büyük olduğu an aşağıdaki kodlar çalışıyor
            {
                spriteRender.sprite = waitAnim[waitingAnimCounter++]; // idle spritelarının yüklü olduğu spritelar her 0.1f saniyede 1 artıyor.
                if (waitingAnimCounter == waitAnim.Length) //Sprite dizisinin değeri ile sayaç birbirine eşit olursa sayaç sıfırlanıyor.
                {
                waitingAnimCounter = 0; // Çünkü sayaç eğer artmaya devam ederse dizinin eleman sayısından daha fazla değere çıkacağı için hata verecektir.
                }
                waitingAnimTime = 0; // Zaman sıfırlanarak 0-0.1f saniye arasında sürekli gidip gelmesi sağlanmıştır.
            }
           
        }
        else if (horizontal > 0) // Bu durumda karakterimizin hızı pozitif yönde artış olduğu için koşma animasyonu devreye girecektir.
        {
            runningAnimTime += Time.deltaTime; // Burada sürekli Time.deltatime ile toplayarak değişken üzerinde zamanı yüklüyor.
            if (runningAnimTime > 0.01f) // Eğer zaman 0.5f saniyeden büyük olduğu an aşağıdaki kodlar çalışıyor
            {
                spriteRender.sprite = runningAnim[runningAnimCounter++]; // running spritelarının yüklü olduğu spritelar her 0.1f saniyede 1 artıyor.
                if (runningAnimCounter == runningAnim.Length) //Sprite dizisinin değeri ile sayaç birbirine eşit olursa sayaç sıfırlanıyor.
                {
                runningAnimCounter = 0; // Çünkü sayaç eğer artmaya devam ederse dizinin eleman sayısından daha fazla değere çıkacağı için hata verecektir.
                }
                runningAnimTime = 0; // Zaman sıfırlanarak 0-0.1f saniye arasında sürekli gidip gelmesi sağlanmıştır.
            }
            transform.localScale = new Vector3(1,1,1); // Bu kod ile oyuncu sağ ok tuşuna bastığı zaman karakterin yüzünün sağa dönmesini sağlıyor.
        }
        else if (horizontal < 0) // Bu durumda karakterimizin hızı negatif yönde artış olduğu için koşma animasyonu devreye girecektir.
        {
            runningAnimTime += Time.deltaTime; // Burada sürekli Time.deltatime ile toplayarak değişken üzerinde zamanı yüklüyor.
            if (runningAnimTime > 0.01f) // Eğer zaman 0.1f saniyeden büyük olduğu an aşağıdaki kodlar çalışıyor
            {
                spriteRender.sprite = runningAnim[runningAnimCounter++]; // running spritelarının yüklü olduğu spritelar her 0.1f saniyede 1 artıyor.
                if (runningAnimCounter == runningAnim.Length) //Sprite dizisinin değeri ile sayaç birbirine eşit olursa sayaç sıfırlanıyor.
                {
                runningAnimCounter = 0; // Çünkü sayaç eğer artmaya devam ederse dizinin eleman sayısından daha fazla değere çıkacağı için hata verecektir.
                }
                runningAnimTime = 0; // Zaman sıfırlanarak 0-0.1f saniye arasında sürekli gidip gelmesi sağlanmıştır.
            }
            transform.localScale = new Vector3(-1,1,1); // Bu kod ile oyuncu sol ok tuşuna bastığı zaman karakterin yüzünün sola dönmesini sağlıyor.
            
        }

        }
        else // Eğer isJump false ise yani karakter havada ise
        {
            if (physic.velocity.y > 0) //Eğer karakter yukarı yönde hareket yaptıysa, yani zıpladıysa
            {
                spriteRender.sprite = jumpingAnim[0]; // Zıplama spriteı devreye giriyor.
            }
            else // Eğer karakter aşağı doğru düşüyorsa
            {
                spriteRender.sprite = jumpingAnim[1]; // Düşme spriteı devreye giriyor.
            }
            if (physic.velocity.x < 0) // Sol oka basıldığı zaman karakterin hızı -x yönünde hızlandığı için 0 dan küçük oluyor.
            {
                transform.localScale = new Vector3(-1,1,1); // Karakter zıplamış olduğu halde sol oka basıldığında yüzü sola dönüyor.
            }
            else if (physic.velocity.x > 0) // Sağ oka basıldığı zaman karakterin hızı +x yönünde hızlandığı için 0 dan büyük oluyor.
            {
               transform.localScale = new Vector3(1,1,1);   // Karakter zıplamış olduğu halde sağ oka basıldığında yüzü sağa dönüyor.
            }
            
               
            
        }
        
    }

    void FollowCharacter()
    {                                                                    //Vector3.Lerp ile yumuşatma özelliği kazandırıyorsun. 
        cameraLastPosition = transform.position + distanceCamera; // Kameranın pozisyonuna karakterin pozisyonunu ve aradaki mesafeyi yükleyerek kameranın sürekli karakter ile birlikte hareket etmesi sağlanıyor.
         //Camera.transform.position = transform.position + distanceCamera; // Karakter her hareket ettiğinde pozisyonu değiştiği için kamera da onla birlikte hareket etmiş oluyor. Bu işlemden dolayı.
        Camera.transform.position = Vector3.Lerp(Camera.transform.position,cameraLastPosition,0.08f); // Kameranın bulunduğu pozisyon ile karakter ile kamera arasındaki mesafenin toplamına kadar olan mesafeye doğru kameranın hareket etmesini sağlar.
        //Camera.transform.position a yükleyerek kameranın hareketi sağlanmış olur.
    }    


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "bullet")
        {
            health--;
            Health.text = "HEALTH: " + health;
        }
        if (col.gameObject.tag == "enemy")
        {
            health -= 10;
            Health.text = "HEALTH: " + health;
        }
        if (col.gameObject.tag == "gamefinish") //Eğer gamefinish tagli objeye değerse
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1); // Mevcut bulunduğu sahnenin bir sonraki geçmesini sağladık, bu sayede finishe gelince bir sonraki seviyeye geçiş yaptı.
        }
        if (col.gameObject.tag == "addhealth")
        {
            chest.enabled = true; // Oyun başladığı anda ilk başta chest objesine yüklü olan script kapalı oluyor onu açmak için true yapıyoruz.
            col.gameObject.GetComponent<BoxCollider2D>().enabled = false; // BoxColliderı kapatarak karakterin bir daha sandığa değerek can almasını engelliyoruz.
            Debug.Log("Sandık Açıldı");
            Destroy(col.gameObject,3); // 3 saniye sonra sandığımızı yok ediyoruz.
            if (health > 0 && health < 91) // Eğer can değerimiz 0 ile 91 arasındaysa canımız 10 ar 10 ar artıyor.
            {
                health += 10;
                Health.text = "HEALTH: " + health;
            }
            else if (health > 90 && health < 100) // Eğer 90 ile 100 arasında ise 91 e 10 ekleyip 101 olmaması için
            {
                health += (100 - health); // En yüksek değerden mevcut can değerini çıkarıp o değeri ekliyoruz bu sayede 100 ü geçmemiş oluyoruz.
                Health.text = "HEALTH: " + health;
            }
            
        }

        if (col.gameObject.tag == "coin")
        {
            coinQuantity++;
            CointText.text = "COIN 30/" + coinQuantity;
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "water")
        {
            health = 0;
        }

    }                                                               
        

    }
