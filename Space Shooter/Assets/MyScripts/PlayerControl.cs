using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    Rigidbody physic; //Rigidbody componentine erişebilmek için değişken oluşturduk.
    float horizontal = 0f; // Karakterin x ekseninde hareketi için değişken oluşturduk.
    float vertical = 0f; // Karakterin z ekseninde hareketi için değişken oluşturduk.
    public float playerSpeed; // Karakterin daha hızlı hareket etmesi için değişken oluşturduk.
    public float minX; // Bu değişken karakterimizin minimum gideceği x eksenini belirliyor.
    public float maxX; // Bu değişken karakterimizin maximum gideceği x eksenini belirliyor.
    public float minZ; // Bu değişken karakterimizin minimum gideceği z eksenini belirliyor.
    public float maxZ; // Bu değişken karakterimizin maximum gideceği z eksenini belirliyor.
    public float slope; // Bu değişken karakterimizin sağa gittiğinde sağa yatmasını sola gittiğinde sola yatmasını sağlayan eğim değişkeni.

    float fireTime = 0f; // Burada normal geçen zaman ile karşılaştıracağımız bir değişken oluşturduk.
    public float fireelapsedTime; // Burada normal zaman ile toplayıp fireTime değişkenine yükleyerek zamanı yükseltecek değişken oluşturduk.
    public GameObject Bullet; // Burada mermiye erişmek için değişken oluşturduk.
    public GameObject BulletExitPoint; // Buradada merminin çıkış noktasına erişmek için değişken oluşturduk.

    public GameObject PlayerExplode; // Burada gemimizin patlama efekti için bu değişkeni oluşturduk.

    AudioSource BulletSound; // Bu değişken gemi ateş ettiğinde oyunda ses çıksın diye oluşturuldu;

    GameManager gameManager; // Burada gameManager scriptindeki değişkene erişebilmek için yazdık.




    void Start()
    {
        physic = GetComponent<Rigidbody>(); //Rigidbody komponentine erişebilmek için değişkene bu şekilde aktardık.
        BulletSound = GetComponent<AudioSource>(); // AudioSource komponenyine erişebilmek için değişken bu şekilde aktardık.
        gameManager = FindObjectOfType<GameManager>(); // GameManager gameobjcetine erişmek için yazdık.
    }

   
    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time > fireTime) // Burada sol tıka basıldığı zaman ve normal zaman eğer fireTime zamanından yüksekse gemi ateş edecek.
        {
            fireTime = Time.time + fireelapsedTime; // Burada Time.time ın üstüne çıkmak için yani merminin çıkışını bekletmek için bu kodu yazdık. Burada normal zaman ile istediğimiz zaman değerini toplayarak Time.time ın üstüne çıkıp merminin o normal zamanda o süreye ulaşmadan çıkmasını engelledik.
            Instantiate(Bullet,BulletExitPoint.transform.position,Quaternion.identity); // Burada Instantiate fonksiyonu ile Mermimize eriştik,Merminin hangi pozisyondan çıkacağını belirledik ve hangi rotasyonda gitmesini söyledik.
            BulletSound.Play(); // Bu kod sayesinde oyunda ateş edildiği zaman mermi sesi çalınacak;
        }
    }


    void FixedUpdate() // Fizik işlemleri kullanacağımız için FixedUpdate yaptık.
    {
        horizontal = Input.GetAxis("Horizontal"); // +1 ile -1 arasında x ekseninde değer elde ettik.
        vertical = Input.GetAxis("Vertical"); // +1 ile -1 arasında z ekseninde değer elde ettik.

        Vector3 vec = new Vector3(horizontal,0,vertical); //Yeni bir vector oluşturarak x ve z ekseninde hareket edebilmeyi sağladık.

        physic.velocity = vec*playerSpeed; // velocity bir method olmadığı için eşitlik olarak vermemiz gerekiyor, addforce ise bir fonksiyon olduğu için parametre veriyoruz.
                                           // physic.velocity yazarak karakterimize hız kazandırmış olduk ve bu değere oluşturduğumuz vektörü ve hız ile çarpılmış halini aktararak karakterin x ve z yönünde hareketi gerçekleştirilmiş oldu.

        physic.position = new Vector3(Mathf.Clamp(physic.position.x,minX,maxX), // Burada karaktermizin x ve z ekseninde ekrandan dışarı çıkmamasını sağlıyoruz.
        0.0f,                                                                  // Minimum ve maksinmum x ve z değerlerini belirliyoruz.
        Mathf.Clamp(physic.position.z,minZ,maxZ)); //Math.Clamp komutu ile de karakterimizin x ve z konumunu bildirip onu verdiğimiz min ve max değerleri ile sınırlandırıyoruz.

        physic.rotation = Quaternion.Euler(0,0,physic.velocity.x*-slope); //Burada karakterimizin x ekseni doğrultusunda sağa ve sola doğru eğrilmesini sağlıyoruz.
        //physic.rotation karakterimizin rotasyonuna erişiyoruz.
        //Quaternion.Euler ile açısına erişiyoruz. z ekseninde rotasyon işlemi yapacağımız için z ye yazıyoruz.
        // Ama karakter x ekseninde hareket ettiği için sağa ve sola giderken physic.velocity.x i referans alıyoruz.

    }   

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy") // Eğer gemiye enemy tagli obje çarparsa isTriggeri aktif olan
        {
            Instantiate(PlayerExplode,gameObject.transform.position,Quaternion.identity); // Gemimizde patlama efekti gerçekleşecek
            gameManager.GameOverText.gameObject.SetActive(true); //GameOverText aktif olmadığı için gemi patlayınca aktif olsun diye yazdık.
            gameManager.GameOverText.text = "Game Over"; // Aktif olduktan sonra bu kod sayesinde ekranda Game Over yazacaktır.
            gameManager.cancelingAsteroidCreating = true; // Gemi asteroidle çarpıştığı zaman bu değişken true olarak asteroidlerin bir daha oluşumu engellenecek.
        }
    }

}
