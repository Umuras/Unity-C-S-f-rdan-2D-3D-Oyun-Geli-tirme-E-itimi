using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR // Bu şekilde yazıldığı zaman bu kod sadece editör de çalışacak, oyun build alındığı zaman koda dahil edilmeyecek. Sadece editörde derlenecek.
using UnityEditor; // Editor sınıfını kullanabilmek için Editor paketini yüklüyoruz.
#endif

public class EnemyControl : MonoBehaviour
{
    public int sawRotationSpeed; // Testere engelinin hızını tuttuğumuz değişken.
    GameObject[] gotothePoints; // Burada Saw gameobjectinin içinde childi olan objeleri dizinin içine yükleyip child olmaktan çıkarmak için oluşturduk.

    bool aradakiMesafeBitti = true; // aradakimesafe hesaplandıysa çıkacak.
    Vector3 aradakiMesafe; // aradaki mesafeyi hesaplamak için oluşturuldu.

    float mesafe; // İki vektörün arasındaki mesafeyi tutup bir konuma ulaştığı zaman diğer konuma hareket etmesini sağlayacağız.
    int aradakiMesafeSayac; // İki mesafe arasındaki fark çok aza ulaştığı zaman diğer konuma geçirmek için oluşturduğumuz değişken.
    bool sayacDegisim = true; // Sayac dizinin son değerine ulaşıp 0 a doğru tekrar gitmesini sağlamak için oluşturuldu.
    //bool ilerimiGerimi = true; //Sayac dizinin son değerine ulaşıp 0 a doğru tekrar gitmesini sağlamak için oluşturuldu.

    GameObject MyPlayer; // REDJAVA karakterimize erişmek için bu değişkeni oluşturduk.

    RaycastHit2D raycast2d; // Raycast işlemi yapmak için bu değişkeni oluşturduk.
    public Sprite enemyFace; // Düşman karakteri karakterimizi gördüğü zaman bu yüz gelicek.
    public Sprite backFace; // Düşman karakteri karakterimizi görmediği zaman bu yüz gelicek.
    SpriteRenderer spriteRenderer; // Yukarıda oluşturduğumuz spriteları spriteRenderer üzerinden değiştireceğiz.

    public LayerMask layermask; // Burada düşmana collider eklediğimiz karaktere doğru çizilen raycast kesiliyor onu engellemek için oluşturduk.
    int speed = 10; // Düşman karakterinin hızını kontrol etmek için oluşturduk.
    public GameObject bullet; // Düşman karakterinden çıkacak mermi için oluşturduk.
    float fireRate; // Belli bir zamanda bir ateş etmesi için oluşturduk.

    void Start()
    {
        gotothePoints = new GameObject[transform.childCount]; // Burada dizimizin içerisine obje adedini yüklüyoruz.
        
        for (int i = 0; i < gotothePoints.Length; i++)
        {
            gotothePoints[i] = transform.GetChild(0).gameObject; // Burada dizinin içine child objelerini gameobject olarak yüklüyoruz.
            //Burada i dendiği zaman hata vermesinin sebebi ilk başta 0,1,2,3,4 => 5 adet elemanımız var. 0.elemanı çıkardık 4 tane kaldı. 1.elemanı çıkardık. 3 tane kaldı.
            //3 tane kalınca ne oldu 0,1,2 olarak algılanıyor kod üzerinde yani 2 eleman çıkınca dizi uzunluğu bakımından 0 ve 1 gitmiyor 3 ve 4 gidiyor ve dizi uzunluğu 0,1,2 oluyor.
            //Kod üzerinden de i++ artığı için i= 0 => 0.eleman çıktı i=1 => 1.eleman çıktı i=2 => 2.eleman çıktı i=3 dizinin son eleman sayısı 0,1,2 oldu 3.elemanı bulamadığı için hata veriyor.
            //Eğer 0 yazarsak her seferinde dizinin ilk elemanını çıkaracağı için 0,1,2,3,4 0 da her zaman ilk başta olacağı için 0 gitti tekrar 0,1,2,3 kaldı, 0 gitti 0,1,2 kaldı 0 gitti 0,1 kaldı 0 gitti 0 kaldı en son 0 gitti ve dizinin tüm elemanları child olmaktan çıktı.
            gotothePoints[i].transform.SetParent(transform.parent); //Burada child objelerimizi child olmaktan kurtarıyoruz.
            
        }
        MyPlayer = GameObject.FindGameObjectWithTag("Player"); // Gameobjectin tagi üzerinden REDJAVA karakterimize erişmiş olduk.
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer komponentine erişiyoruz.
    }

    
    void FixedUpdate()
    {
        RaycastOperation();
        if (raycast2d.collider.tag == "Player") // Burada eğer çizilen raycast REDJAVA karakterinin colliderına değiyorsa
        {
            speed = 15; // Düşman karakterinin hareket hızı artıyor
            spriteRenderer.sprite = enemyFace; // Playerı gördüğü an enemyFace devreye girecek. Burada da spriteRenderer.sprite dan SpriteRenderer komponentinin spriteına erişip istediğimiz spriteı yüklüyoruz.
            Fire();
        }
        else // Eğer çizilen raycast başka bir collider a değiyorsa
        {
            speed = 5; // Düşmanın hareket hızı yavaşlıyor
            spriteRenderer.sprite = backFace; // Playerı göremediği zaman arka yüz devreye girecek. Burada da spriteRenderer.sprite dan SpriteRenderer komponentinin spriteına erişip istediğimiz spriteı yüklüyoruz.
        }
        MovementBetweenPoints();
        
    }
    void Fire()
    {
        fireRate+= Time.deltaTime; // fireRate değişkenine kronemetre sayacı gibi zaman tutuyoruz.
        if (fireRate > Random.Range(0.2f,1)) // Eğer sayaç 0.2 ile 1 saniye arasında bir oluşan değerin üstüne çıkarsa mermi oluşup fireRate tekrar sıfırlanıp tekrar saymaya başlıyor.
        {
            Instantiate(bullet,transform.position,Quaternion.identity); // Mermi düşman karakterinin doğrultusunda doğuyor.
            fireRate = 0;
        }
    }

    public Vector2 GetDirection() // Merminin gideceği alanı hesaplamak için oluşturduk.
    {
        return (MyPlayer.transform.position - transform.position).normalized; // Karakter ile düşmanın arasındaki mesafe hesaplanıp mermiye aktarıldığı zaman mermi düşmandan karaktere doğru gidecektir.
    }

    void MovementBetweenPoints()
    {
        //Debug.Log((gotothePoints[0].transform.position - transform.position).normalized); // Burada bize testerenin ilk gideceği yer ile kendisinin arasındaki mesafeyi söylüyor.
        //transform.position += (gotothePoints[0].transform.position - transform.position)*Time.deltaTime; // Testerenin pozisyonunu ilk gideceği obje olmasını sağlıyoruz. += yapınca A noktası ile B noktası arasındaki mesafeyi hesaplayıp A noktası ile toplayınca testere B noktasına gitmiş oluyor.
        //Testerenin sona doğru gitme anında gideceği mesafe azaldığı için üstüne eklenen pozisyon daha az oluyor ve yavaşlıyor, aynı hızda gitmesi için aradaki mesafenin normalini alarak 0 ile 1 arasında bir mesafe üretmesini sağlıyoruz.
        
        if (aradakiMesafeBitti)
        {
            aradakiMesafe = (gotothePoints[aradakiMesafeSayac].transform.position - transform.position).normalized; // Burada gidilecek bölge arasındaki ile  testere birbirinden çıkarılarak mesafe hesaplanıyor ve onun normali alınıyor bu sayede gideceği noktaya hep aynı hızda hareket ediyor testere
            aradakiMesafeBitti = false; // Hesaplama işlemi bittiğinden tekrar ife girmesin diye false yapıyoruz.
        }

        mesafe = Vector3.Distance(transform.position,gotothePoints[aradakiMesafeSayac].transform.position); // testere ile gidilecek yer arasındaki mesafe hesaplanıp float türündeki değişkende saklanıyor.
        transform.position += aradakiMesafe*Time.deltaTime*speed; // testerenin kendi pozisyonu ve aradaki mesafe toplanarak testerenin pozisyonuna aktarılınca o bölgeye doğru hareketi sağlanmıştır.
        
        if (mesafe < 0.5f) // Eğer testerenin gideceği bölgeye olan uzaklığı 0.5f den azsa buraya girecektir.
        {
            // if (sayacDegisim)
            // {
            //     aradakiMesafeSayac++;
            //     aradakiMesafeBitti = true;
            // }
             if (aradakiMesafeSayac >= 0 && aradakiMesafeSayac != gotothePoints.Length-1 && sayacDegisim) // Eğer dizimiz 0 ile 3. eleman arasında hareket ediyorsa ve sayacDegisim true ise çalışacak
        {
            aradakiMesafeSayac++; // Sayac artırılarak bir sonraki noktaya testerenin geçişi sağlanacak 0. elemandan 1.elemana geçiş sağlanacak
            aradakiMesafeBitti =true; // Değişken true yapıldığı takdirde testere ile yeni gidilecek yerin mesafesi tekrar hesaplanacak.
        }
          else if (aradakiMesafeSayac <= gotothePoints.Length-1) // Eğer sayac son elemana eşitse ve altındaysa bu kısım çalışacak.
        {
            sayacDegisim = false; // Bu değişkenin false yapıldığı anda son eleman ile bir önceki eleman arasında sürekli gidişin önü kesilmiş olacak çünkü yukarıda true olduğu anda çalışması gerektiği için yukarıdaki ife giriş yapamayacak.
            aradakiMesafeSayac--; // Sayacı küçülterek 4 3 2 1 0 şeklinde 0. gidilecek elemana doğru hareket sağlanmış olur.
            aradakiMesafeBitti = true; // Değişken true yapıldığı takdirde testere ile yeni gidilecek yerin mesafesi tekrar hesaplanacak.
            if (aradakiMesafeSayac == 0) // Sayac 0 olduğu anda ise tekrar 0 dan 4. elemana doğru hareket edebilmesi için sayacDegisimi true yaparak buradaki else if den çıkarıyoruz.
            {
                sayacDegisim=true;
            }
        }

        // // Bu kısım hocanın yazdığı kod.
        // aradakiMesafeBitti = true; // Değişken true yapıldığı takdirde testere ile yeni gidilecek yerin mesafesi tekrar hesaplanacak.
        // if (aradakiMesafeSayac == gotothePoints.Length -1) // Eğer sayacımız dizimizin uzunluğunun bir eksiğine ulaştığı zaman işlem yapılacaktır.
        // {                                                  // 1 eksiğinin olmasının sebebi .Length dendiği zaman dizinin içinde normalde 5 adet eleman var Length dediğimiz anda 5 i anlıyor ama dizinin eleman sistemi 0 dan başladığı için 0 dan 4 ekadar elemanı algılıyor. Sayac eğer 5 olursa index aşımı hatası verecektir, çünkü 4 te son buluyor.
        //     ilerimiGerimi = false; // 4. gidilecek noktaya eriştiği an false yapıp dizi içerisinde geri geri gidilmesi sağlanmıştır.
        // }
        // else if (aradakiMesafeSayac == 0) // İlk gidilecek yere ulaştığı anda çalışacaktır.
        // {
        //     ilerimiGerimi = true; // 0. gidilecek noktaya geldiği an true yapıp bu sefer dizi içerisinde son elemana doğru hareket sağlanacaktır.
        // }
        
        // if (ilerimiGerimi) // true olduğu anda çalışacaktır.
        // {
        //     aradakiMesafeSayac++; // 0. elemandan son elemana doğru hareket sağlanacaktır.
        // }
        // else // ilerimiGerimi false olduğu anda çalışacaktır.
        // {
        //     aradakiMesafeSayac--; // 4. elemandan 0.elemana doğru hareket sağlanacaktır.
        // }
        
            
            
          
            
        }
       
        


        
    }

    void RaycastOperation()
    {
        Vector3 raycastDirection = MyPlayer.transform.position - transform.position; // Burada karakterimiz ile düşmanımızın arasındaki mesafeyi tutuyoruz.
        raycast2d = Physics2D.Raycast(transform.position, raycastDirection, 1000, layermask); // Düşman karakterimizin başlangıç noktası alıp yönüne raycastLocation üzerinden alıyor, 1000 birimlik mesafe kadar çizdirebiliyor.
        Debug.DrawLine(transform.position,raycast2d.point,Color.magenta); // Burada ise düşman karakteri ile REDJAVA karakterimiz arasında çizgi çiziyoruz.
                                                                          // transform.position düşmanımızın posizyonu, raycast2d.point ise düşmanımızdan çıkan rayin karakterimize çarptığı collider kısmı oluyor. Color.magenta ile de bu çizginin rengini belirliyoruz.
    }
    

#if UNITY_EDITOR //Sadece editörde çalışıyor.
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++) // Saw gameobjectinin child obje sayısı kadar dönücek for döngüsü
        {
            Gizmos.color = Color.red; //Kırmızı rengi yüklüyoruz, Gizmosa
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);// Çemberin merkezini Saw gameobjectinin child objelerinin pozisyonundan alıyor ve 1 yarıçap değerinde çizdiriyor.
        }
        for (int i = 0; i < transform.childCount-1; i++) // Saw gameobjectinin child obje sayısının 1 eksiği kadar dönücek for döngüsü, 
        {                                                // Bunun sebebi aşağıda i+1 şeklinde çalıştığı için eğer son obje sayısına geldiğimizde mesela 0,1,2. ye geldiği 2 son olmasına rağmen i+1 den 3. ye geçeceği için 3. obje de olmadığı için hata verecektir. Bunu önlemek için 1 eksiğinde durdurduk.
            Gizmos.color = Color.blue; //Mavi rengi yüklüyoruz, Gizmosa
            Gizmos.DrawLine(transform.GetChild(i).transform.position,transform.GetChild(i+1).transform.position);
            //Burada oluşan child gameobjectinin birinden birine çizgi çizilmesi sağlanmıştır. 1. objeden 2.objeye doğru çizgi üretilecektir.
        }
    }
#endif

}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyControl))]
[System.Serializable]
class enemyControlEditor : Editor //sawEditor sınıfımıza Editor sınıfını kalıtım ederek Editor sınıfındaki değişken methodları kullanabiliyoruz.
{
    
    public override void OnInspectorGUI() //Sanal olarak yazılmış methodun üzerine veri yazıyoruz.
    {
        EnemyControl script = (EnemyControl)target; // Kendi scriptimzin türünde değişken oluşturup target değişkenin buna cast edip değişkenimize eşitliyoruz. Yukarıdaki scriptimizdeki verilere erişiyoruz.
        EditorGUILayout.Space();
        if (GUILayout.Button("URET",GUILayout.MaxWidth(50),GUILayout.MaxHeight(50))) //Inspector panelinde isimi ve boyutlarını belirlediğimiz bir buton oluşturuyoruz.
        {
            GameObject newObject = new GameObject(); //Bu butona tıklanıldığı zaman hierarchy de Hello isminde boş bir gameobject üretiyor.
            newObject.transform.parent = script.transform; // Butona tıkladığımızda üretilen yeni boş gameobjectler Saw adlı SawControl scriptimizin olduğu gameobjectin childi olarak üretiliyor.
            newObject.transform.position = script.transform.position; // Saw gameobjectinin pozisyonu yeni boş gameobjectine yükleniyor.
            newObject.name = script.transform.childCount.ToString(); // Saw gameobjectinin childlarının sayısını aldık ve bu sayıları isim olarak oluşturduğumuz child gameobjectine atadık. name string bir ifade olduğu için childCount int olduğu için ToString yaparak string bir ifadeye dönüştürdük.

        }
        EditorGUILayout.Space(); //Inspector paneli üzerindeki kendi scriptimizde oluşan butonlar veya public değişkenlerin arasında boşluk oluşturmak için kullanıyoruz.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));// Eğer editor kodu yazdıysan public bir değişkeni kendi scriptin üzerinde göstermek için bu kodu yazmak zorundasın.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyFace"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("backFace"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bullet"));
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
#endif


