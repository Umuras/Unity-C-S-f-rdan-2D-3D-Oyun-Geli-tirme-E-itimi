using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //MainCircleCode MainCircle; // MainCircleCodeu oyun bittiğinde devre dışı bırakabilmek için oluşturuldu. BENİM YAZDIĞIM KOD
    //Spin SpiningCircle; // Spin scriptini oyun bittiğinde devre dışı bırakabilmek için oluşturuldu. BENİM YAZDIĞIM KOD
    GameObject spiningCircle; // Burada spiningCircle Gameobjectini oluşturarak Spin scriptine erişmeyi amaçladık.
    GameObject mainCircle; // Burada mainCircle Gameobjectini oluşturararak MainCircleCode scriptine erişmeyi amaçladık.

    public Animator animator; // Gamemanager scriptine MainCameranın animatorunu koyabilmek için oluşturduk.

    public Text LevelInfo; // Dönen çember üzerinde hangi seviyede olduğunu ekrana yazdıracak.

    public Text FirstCircle; // En tepedeki çemberin sayı değerini tutacak text;

    public Text SecondCircle; // Ortadaki çemberin sayı değerini tutacak text;
    public Text ThirdCircle; // En alttaki çemberin sayı değerini tutacak text;

    public int smallCircleAmount; // Kaç tane küçük çemberin oluşmasını belirleyen değişken.
    private bool realGameOver = true; // Eğer 0 a ulaştığı anda çubuklu çemberle çarpışırsa ana menüye dönecek.



    void Start()
    {
        PlayerPrefs.SetInt("savelevel",int.Parse(SceneManager.GetActiveScene().name)); // Burada kayıt sistemi oluşturmak için bu kodu yazdık.
        // İlk önce string ifademiz , den sonra bulunan sayı değerini tutuyor, yani mevcut sahne değerimizi tutuyor.
       // MainCircle = FindObjectOfType<MainCircleCode>(); // Buradan scriptimize eriştik. BENİM YAZDIĞIM KOD
       // SpiningCircleS = FindObjectOfType<Spin>(); // Buradan scriptimize eriştik. BENİM YAZDIĞIM KOD
        spiningCircle = GameObject.FindGameObjectWithTag("spiningcircletag"); // Tag üzerinden aratarak SpiningCircle gameobjectine eriştik.
        mainCircle = GameObject.FindGameObjectWithTag("maincircletag"); // Tag üzerinden aratarak MainCircle gameobjectine eriştik.
        LevelInfo.text = SceneManager.GetActiveScene().name; // Bu şekilde ise Text gameobjectimize hangi seviyedeysek o seviye yazdırılacak.

         if (smallCircleAmount < 2) // Eğer çubuklu çemberimizin toplam istenen adedi 2 den küçükse sadece ilk çember textine değer yazdırıyoruz.
         {
             FirstCircle.text = smallCircleAmount.ToString(); // Burada smallCircleAmount int türünde olduğu için ToString ile string yapıyoruz.
         }
         else if (smallCircleAmount < 3) // Eğer çubuklu çemberimizin toplam istenen adedi 3 den küçükse ilk ve 2.çemberin 1 eksiğine değer yazdırıyoruz.
         {
             FirstCircle.text = smallCircleAmount.ToString(); // Burada smallCircleAmount int türünde olduğu için ToString ile string yapıyoruz.
             SecondCircle.text = (smallCircleAmount-1).ToString(); // Burada smallCircleAmount int türünde olduğu için ToString ile string yapıyoruz.
         }                                                         // Buradaki mantık şu şekilde eğer 2 denirse ilk çemberde 2 yazacak ikincisinde 1 yazacak -1 ile çıkarıldığı için son çembere değer atanmadığı için boş kalacak.
         else // Burada ise smallCircleAmount >= 3 ise  
         {
             FirstCircle.text = smallCircleAmount.ToString(); // İlk çember 3
             SecondCircle.text = (smallCircleAmount-1).ToString(); // ikincisi 2 
             ThirdCircle.text = (smallCircleAmount-2).ToString(); // üçüncüsü 1 şeklinde yazdıracak.
         }
         // İlk girilen smallcircle değerinin gözükebilmesi için bu kodun durması gerekiyor.
    }

    public void SmallCircleAmountDecrease()
    {
        smallCircleAmount--; // Mouse her tıklandığında değeri 1 azaltarak 0 değerine ulaşabilmek için yazılmıştır.
        if (smallCircleAmount < 2) // Eğer çubuklu çemberimizin toplam istenen adedi 2 den küçükse sadece ilk çember textine değer yazdırıyoruz.
        {
            FirstCircle.text = smallCircleAmount.ToString(); // Burada smallCircleAmount int türünde olduğu için ToString ile string yapıyoruz.
            SecondCircle.text = ""; // Boş string değer eklemimizin sebebi FirstCircle 1 değerine geldiği zaman Second ve Third Circle değer yazmaması için.
            ThirdCircle.text = "";
        }
        else if (smallCircleAmount < 3) // Eğer çubuklu çemberimizin toplam istenen adedi 3 den küçükse ilk ve 2.çemberin 1 eksiğine değer yazdırıyoruz.
        {
            FirstCircle.text = smallCircleAmount.ToString(); // Burada smallCircleAmount int türünde olduğu için ToString ile string yapıyoruz.
            SecondCircle.text = (smallCircleAmount-1).ToString(); // Burada smallCircleAmount int türünde olduğu için ToString ile string yapıyoruz.
            ThirdCircle.text = ""; // Burada da 2 değerine ulaştığı zaman ThirdCircle da değer yazmaması için yapıldı.
        }                                                         // Buradaki mantık şu şekilde eğer 2 denirse ilk çemberde 2 yazacak ikincisinde 1 yazacak -1 ile çıkarıldığı için son çembere değer atanmadığı için boş kalacak.
        else // Burada ise smallCircleAmount >= 3 ise  
        {
            FirstCircle.text = smallCircleAmount.ToString(); // İlk çember 3
            SecondCircle.text = (smallCircleAmount-1).ToString(); // ikincisi 2 
            ThirdCircle.text = (smallCircleAmount-2).ToString(); // üçüncüsü 1 şeklinde yazdıracak.
        }
        if (smallCircleAmount == 0)
        {
            StartCoroutine(NewLevel());
        }
    }

    private IEnumerator NewLevel() //IEnumerator yapmamızın sebebi yeni seviyeye geçerken pat diye değil biraz bekleyerek geçmesini sağlamamız içindir.
    {
        spiningCircle.GetComponent<Spin>().enabled = false; // Eriştiğimiz Gameobject üzerinden Spin komponentine erişerek o da Spin scripti oluyor, onu false yapıp devre dışı kalmasını sağladık.
        mainCircle.GetComponent<MainCircleCode>().enabled = false; // Eriştiğimiz Gameobject üzerinden MainCircleCode komponentine erişerek o da MainCircleCode scripti oluyor, onu false yapıp devre dışı kalmasını sağladık.
        yield return new WaitForSeconds(0.5f); // 2 saniye bekletip ondan sonra yeni sahneye geçiş sağlıyoruz.
        if (realGameOver)
        {
            animator.SetTrigger("newlevel"); // Burada eğer yeni levela geçeceksek animasyonumuzu aktif hale getirdik. Trigger adını girerek.
            yield return new WaitForSeconds(2f); // Burada ise animasyonun çalışmasını beklemek için 2 saniye bekleme işlemi uyguladık.
            SceneManager.LoadScene(int.Parse(SceneManager.GetActiveScene().name)+1); // Burada Getactivescene ile mevcut seviyeye erişip onu +1 ile toplayarak her yeni seviyeye geçiceğimiz zaman mevcut seviyeye +1 eklememiz sayesinde yeni seviyeye geçişi sağlamış oluyoruz. 
                              //int.Parse ile GetActiveScene.name string türünden int türüne çevirmiş olup iki int değerinin toplamını sağlıyoruz.
        }
        
    }

    public void GameOver()
    {
        StartCoroutine(InvokedMethod());
    }
    public IEnumerator InvokedMethod() // Çubuklu çemberler birbiri ile çarpıştığı zaman alttaki iki kod devre dışı kalarak dönen çemberin durup, çubuklu çemberin de bir daha üretilmesi engellenmiş oldu.
    {
        //MainCircle.enabled = false; // Bu kod sayesinde kodumuzun aktifliğini kapatmış olduk. BENİM YAZDIĞIM KOD
        //SpiningCircle.enabled = false; // Bu kod sayesinde kodumuzun aktifliğini kapatmış olduk. BENİM YAZDIĞIM KOD
        spiningCircle.GetComponent<Spin>().enabled = false; // Eriştiğimiz Gameobject üzerinden Spin komponentine erişerek o da Spin scripti oluyor, onu false yapıp devre dışı kalmasını sağladık.
        mainCircle.GetComponent<MainCircleCode>().enabled = false; // Eriştiğimiz Gameobject üzerinden MainCircleCode komponentine erişerek o da MainCircleCode scripti oluyor, onu false yapıp devre dışı kalmasını sağladık.
        animator.SetTrigger("gameover"); // Oyun bittiğinde Triggeri aktifleştirerek yani tetikleyerek animasyonun çalışmasını sağladık.
        realGameOver = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu"); // Oyun bittiği zaman Main Menuye dönsün diye yazıldı.
    }
}
