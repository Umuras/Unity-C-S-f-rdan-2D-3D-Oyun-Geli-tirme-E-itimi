using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    float HorizontalInput = 0, VerticalInput = 0; // Yatay ve dikey değerleri tutacak değişkenleri oluşturduk. Karakterin hareketi için
    public float speed = 2; // Karakterin hareket hızını tutan değişken;
    Animator _animator; //Animator komponentine erişmek için oluşturduk.
    Rigidbody _physic; //Rigidbody komponentine erişmek için oluşturduk.
    public GameObject headCam; // KafaKamerası objesine erişmek için bu değişkeni oluşturduk.
    float headCamUpDown, headCamRightLeft; // Kameranın sağa sola ve yukarı aşağı hareket değerlerini tutacak değişken.
    Vector3 headCamDistance; // Kamera ile karakter arasındaki mesafeyi tutacak değişken;
    RaycastHit hit; // Raycast fonksiyonunda kullanmak için yazıldı.
    RaycastHit hitFire; // Raycast fonksiyonunda kullanmak için yazıldı.
    public bool shootControl; // Kameranın değişimi için oluşturuldu
    public GameObject position1, position2; // Kameranın sağ tıka basıldığı anda ve bırakıldığı anda gideceği pozisyonlar.
    //Gameobject camera,pos1,pos2 private olarak gameobject erişme şeklini yazmak için oluşturdum.
    //[Range(1,3)] public float speed2; // bu şekilde yazarak slider şeklinde 1 ile 3 arasında Unity üzerinden değer atabilirsin.
    //Transform can; //Canvas üzerinden Sight Image objesine erişebilmek için oluşturuldu.
    public GameObject sight; // Nişangah gameobjectini kapatıp açmak için oluşturuldu.
    Transform skeleton; // Karakterin kemiklerine erişebilmek için oluşturduk.
    public Vector3 offset; // Bu değişken ile karakterimizin kemiklerini hareket ettirebileceğiz.
    public RuntimeAnimatorController whenFired; // RuntimeAnimatorController sayesinde AnimatorController arasında değişim yapabileceğiz. Ateş ettiğinde biri ateş etmediğinde diğeri çalışacak.
    public RuntimeAnimatorController whennotFired; // RuntimeAnimatorController sayesinde AnimatorController arasında değişim yapabileceğiz. Ateş ettiğinde biri ateş etmediğinde diğeri çalışacak.

    public GameObject bullet; //Ateş edildiğinde namlunun ucunda mermi oluşacak.
    public GameObject outOfAmmo; // Merminin silahtan çıkış noktası.
    public bool runControl = true; // LeftShiftin parametre karışımını önlemek için oluşturuldu
    Coroutine storageCoroutine; // Coroutine fonksiyonunu bir değişkende tutabilmek için oluşturduk.
    public GameObject[] bulletPoolArray; // Kurşun havuzunda oluşan mermileri bir dizi içerisinde tutup sürekli onlara erişebilmek için oluşturuldu.
    int bulletPoolArrayCounter; // Kurşun havuzunda hareket edebilmek için bir sayaç oluşturuyoruz.
    void Start()
    {
        _animator = GetComponent<Animator>(); // Animator komponentine eriştik.
        _physic = GetComponent<Rigidbody>(); // Rigidbody komponentine eriştik.
        headCamDistance = headCam.transform.position - transform.position; // Kamera ile karakterin arasındaki pozisyonu hesapladık.
        //camera = Camera.main; diyerek değişkenimize kamera özelliğini kazandırıyoruz.
        //pos1 = headCam.transform.Find("Position1").gameobject; // Bu şekilde HeadCam gameobjecti içindeki Position1 gameobjectine erişiyoruz. Eğer .gameobject demezsen objeye değer transform türünde yüklenir.
        //pos2 = headCam.transform.Find("Position2").gameobject; // Bu şekilde HeadCam gameobjecti içindeki Position2 gameobjectine erişiyoruz.
        //pos1 = transform.Find("Positon1"); // Eğer böyle dersek karakterimiz içindeki gameobject üzerinde arama yapar ve bu gameobject olmadığı için hata verir.
        //can = Canvas.FindObjectOfType<Canvas>().gameObject.transform.Find("Sight"); Canvasa erişip canvas üzerinden Sight gameobjectine erişiyoruz Transform türünde.

        skeleton = _animator.GetBoneTransform(HumanBodyBones.Chest); // skeleton değişkenimize karakterimizin üst gövdesini kemik yapısını aktarmış olduk.

        // if (skeleton == null)
        // {
        //     Debug.Log("Skeleton is null"); //Skeletonun null gelme sebebi karakterimizin animasyon rig türünün generic olarak ayarlı kalmasıdır, onları karakterimiz insan olduğu için humanoid olarak ayarladığımızda bu durum düzelecektir.
        //     // Generic'ği  örnek olarak ahtapot gibi iskelet sistemi bulunmayan canlı türleri için kullanabiliriz.
        //     //Animation Type kısmını Humanoid yaptığın zaman Animasyon dosyasındaki Avatarları kullanman gerekiyor.
        // }

        BulletPool(); // Oyun açıldığı anda kurşun havuzumuzun oluşması için Startta fonksiyonumuzu çağırıyoruz.
    }

    void BulletPool()
    {
        bulletPoolArray = new GameObject[10]; //10 elemanlı bir dizi oluşturuyoruz dizi kapasitemiz doğrultusunda mermi üretiyoruz.
        for (int i = 0; i < bulletPoolArray.Length; i++) //Dizinin eleman sayısı kadar çalışıyor.
        {
          GameObject bulletObj = Instantiate(bullet); //Instanitate fonksiyonu ile sadece mermi üretip pozisyon ve rotasyon olmadan bir gameobjecte atıyoruz.
          bulletObj.SetActive(false); // Sonra merminin görünürlüğünü kapatıyoruz.
          bulletPoolArray[i] = bulletObj; // Oluşan her mermiyi dizimizin içine yüklüyoruz.
        }
       
    }
    public void LateUpdate() //Animasyon işlemleri en son gerçekleştiği yaptığımız değişiklikleri görebilmek için LateUpdate üzerinde çalıştırmalıyız iskelet rotasyon işlemimizi
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (hitFire.distance > 3) // Bunu yapmamızın sebebi karakterimiz duvarın dibine girdiği zaman vücudu dönüyor o kadar mesafe yaklaştığında dönüş işleminin gerçekleşmesini engelliyoruz.
            {
            skeleton.LookAt(hitFire.point);// Burada karakterin iskeleti hitFire.point noktasına doğru kendisini çevirecek.
            skeleton.rotation = skeleton.rotation*Quaternion.Euler(offset); // Bu kod ile karaktermizin iskelet rotasyonuna erişip Quartenion.Euler ile offset Vector3 değişkenimizi atayarak onun hareketini sağlıyoruz.
            }
           
        }
        
    }
    void Update()
    {
        Animation();
        CameraChange();
    }
    
    
    void FixedUpdate() // Fizik olaylarını fixeduptade olarak çalıştırdığın zaman kameradaki titreme yok oluyor.
    {
        Movement();
        Rotation();
    }
   

    void Movement()
    {
        HorizontalInput = Input.GetAxis("Horizontal"); // x ekseninde -1 ile 1 arasında değer elde etmek için oluşturduk.
        VerticalInput = Input.GetAxis("Vertical"); // z ekseninde -1 ile 1 arasında değer elde etmek için oluşturduk.

        Vector3 vec = new Vector3(HorizontalInput,0,VerticalInput); // Karakterin hareket edebileceği x ve z ekseninde çalışacak bir vektör oluşturduk, x ve z değerlerinin oluşacağı değişkenleri atadık.
        vec = transform.TransformDirection(vec); // Bu kod sayesinde karakterimizin yönü ne tarafa çevirilirse gideceği yönde o şekilde ayarlanmış oluyor ve transform.Translate methodu kullanmamıza gerek kalmıyor.
        vec.Normalize(); // Bu kodu yazmamızın sebebi karakter çapraz yönde hareket yaptığı zaman sağa sola ya da yukarı aşağı gittiğinden daha hızlı hareket ediyor, bunu önlemek için normalini alarak 0 ile 1 arasında değer oluşmasını sağladık.

        //transform.position += vec*Time.deltaTime*speed; // Kendi pozisyonumuzla vektörümüzü sürekli toplayarak karakterin hareketini sağladık.
        _physic.position += vec*Time.fixedDeltaTime*speed; // transform yerine fizik üzerinden işlem yapmamızın sebebi fizik işlemlerinin transforma göre daha hızlı hesaplanmasından dolayıdır.
        //transform.Translate(vec*Time.deltaTime*speed); //Translate methodu sayesinde karaktere bağlı olan kamera hangi yöne çevrilirse karakterde o yöne doğru hareket edecektir.

    }

    void Animation()
    {
        _animator.SetFloat("Horizontal",HorizontalInput); //Karakterin yürüme veya koşma animasyonunun x ekseninde çalışabilmesi için ilk önce parametre ismini daha sonra o parametreyi değiştirecek değişkeni atadık.
        _animator.SetFloat("Vertical",VerticalInput); //Karakterin yürüme veya koşma animasyonunun z ekseninde çalışabilmesi için ilk önce parametre ismini daha sonra o parametreyi değiştirecek değişkeni atadık.

        //Input işlemleri FixedUpdate yerine normal updatede yapmak daha iyi oluyormuş, çünkü bazen algılamama sorunu yaşanıyor.
        if(Input.GetKey(KeyCode.LeftShift) && runControl == true) // Karakterin yürüme animasyonundan koşma animasyonuna geçmesi için bu şartı yazdık.
        { //Sağ tıka basılı değilken koşma tuşuna basarsan runControl ilk başta true olduğu için ve sağ tıka basılmadığı anda da true olduğu için o anda koşabileceksin ama sağ tıka bastığında koşamayacaksın.
            _animator.SetBool("RunState",true); // LeftShift tuşuna basıldığı zaman RunState Bool parametresine erişip true yaparak koşma animasyonuna geçiş yapacak.
            speed = 4;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && runControl == true) //LeftShift tuşunu bıraktığın zaman koşma animasyonu sona erecek, runControlun eklenme sebebi sağ tıktayken leftshifte basıp bıraktığın zaman bu koda giriyor ve o RunState parametresini sağ tıka basıldığı andaki animasyonda bulamadığı için hata veriyor, onu önlemek için yazıldı
        {
            _animator.SetBool("RunState",false); //LeftShift tuşundan elini çektiği zaman tekrar yürüme animasyonuna geçiş yapacak.
            speed = 2;
        }
        if (Input.GetKeyDown(KeyCode.Space)) // Space bastığın zaman jump up animasyonu çalışacak ve aşağıdaki JumpUpNormal eventi çalışacak animasyon eventi
        {
            _animator.SetBool("IsJump",true);
        }
        else if (Input.GetKeyUp(KeyCode.Space)) // Space i bıraktığın anda animasyonu false yapıyoruz çünkü true kaldığı zaman karakter sürekli zıplıyor.
        {
            _animator.SetBool("IsJump",false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && shootControl == true) // Sağ tıka basılmadığı sürece sol tık çalışmayacak
        {
            if (storageCoroutine != null) // Mousun sol tıkına ilk tıkladığında StopFire Coroutinei hiç başlamadığı için null değerde olacağından null değilse diyerek ilk başlangıcı eliyoruz.
            {
                StopCoroutine(storageCoroutine); //2.kez mouseun sol tıkına bastığından zaman burası çalışıyor. Bunun yapılma sebebi mouseun sol tıkına basıp çektiğin anda animasyon direk kapanırsa sol tıktan elini çeker çekmez animasyon kapanacak ve oyuncu sol tıka sürekli tıklayarak ateş edemeyecek ama bu şekilde 0.2 saniye bekleme koyduğumuzda eğer 0.2 saniyeyi geçmeden tekrar basarsa oyuncu sol tıka tekrardan animasyon false olmadan devam edip karakter ateş edecektir.
            }
            _animator.SetBool("FireParam",true); // Animasyon çağrıldığı anda event otomatik olarak çalışıyor. Ayrıca fonksiyon artık event olduğu için animasyon çalıştığı sürece sürekli çalışıyor bu sayede normalde burada çağrıldığı şekilde olsaydı mouse dowwn ayarında olduğu için mousea sürekli basmak zorunda kalacaktık, bu sayede basılı tutarak çalışmış oluyor.
            //CreateBullet(); // Her sol tıka basıldığında mermi üretilecek //Artık bir evefire animasyon eventi olduğu için fonksiyonu çağırmıyoruz.
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && shootControl == true) // Sağ tıka basılmadığı sürece sol tık çalışmayacak
        {
            storageCoroutine = StartCoroutine(StopFire()); //Mouseun sol tıkından elini bırakdığın ateş animasyonunu durdurma kodu başlıyor.
        }
        
    }
    IEnumerator StopFire() // Burada 0.2 saniye bekledikten sonra ateş etme animasyonunu durduruyoruz.
    {
        yield return new WaitForSeconds(0.2f);
        _animator.SetBool("FireParam",false);

    }

    void CreateBullet() // Bu fonksiyonu evefire animasyonunda event olarak oluşturduğumuz için her sol tıka basıldığında evefire animasyonu çağırıldığında otomatik olarak bu fonksiyonda çağırılacak
    {
        bulletPoolArray[bulletPoolArrayCounter].transform.position = outOfAmmo.transform.position; // Burada mousa her sol tıklandığında merminin pozisyonuna silah namlusunun ucunun pozisyonunu aktarıyoruz.
        bulletPoolArray[bulletPoolArrayCounter].transform.rotation = outOfAmmo.transform.rotation; // Burada mousa her sol tıklandığında merminin rotasyonuna silah namlusunun ucunun rotasyonunu aktarıyoruz.
        bulletPoolArray[bulletPoolArrayCounter].SetActive(true); // Mouseda her sol tıka basıldığında mermilerin görünür olmasını sağlıyoruz.
        Rigidbody bulletPhysic = bulletPoolArray[bulletPoolArrayCounter].GetComponent<Rigidbody>(); //Burada bütün mermi havuzundaki mermilerin rigidbodysine erişiyoruz ve değişkene yüklüyoruz.
        bulletPhysic.AddForce((hitFire.point-outOfAmmo.transform.position).normalized*1000); // Rigidbody değişkeni üzerinden o anki yüklenen mermiye kuvvet uygulayarak hareket etmesini sağlıyoruz.

        //bulletPoolArray[bulletPoolArrayCounter].gameObject.AddForce((hitFire.point-outOfAmmo.transform.position).normalized*1000); // Bu şekilde mermilerimiz hareket etmiş olucak.
        bulletPhysic.velocity = Vector2.zero; // Mermi tekrar kullanılacağı zaman hızını sıfırlayarak o anki değerinde değilde başlangıçta oluşmuş gibi hareket etmesini sağlıyoruz.
        bulletPoolArrayCounter++; // Sayacı arttırarak her sol tıka basıldığında dizi içerisindeki oluşan diğer mermiye geçiş yapmasını sağlıyoruz.

        if (bulletPoolArrayCounter == bulletPoolArray.Length) // Burada da sayacımız 10 a ulaştığı zaman dizi içerisinde 0 dan başladığı için en fazla 9 a kadar değer var 10 da hata vericektir. Bundan dolayı dizinin uzunluğu da 10 a eşit olduğu için birbirlerine eşit oldukları an çalışacaktır.
        {
            bulletPoolArrayCounter = 0; // Buradaki amaç sürekli o 10 tane oluşan mermi arasında gidip gelebilmek 0 a eşitlediğimiz anda 0 dan 9. elemana kadar gelicek sonra tekrar 0 a geçip 0.eleman en son nerede oluşturulduysa orada yok olup en son nerede sol tıka basarsak orada tekrar oluşturulacak.
        }

        //Instantiate(bullet,outOfAmmo.transform.position,outOfAmmo.transform.rotation).gameObject.GetComponent<Rigidbody>().AddForce((hitFire.point-outOfAmmo.transform.position).normalized*1000);
        //bullet.gameObject.GetComponent<Rigidbody>().AddForce((hitFire.point-outOfAmmo.transform.position).normalized*1000); // Burada kurşun gameobjectimiz üzerinden rigidbody komponentine erişerek AddForce methodu ile kurşuna hareket kazandırıyoruz.
                                                               //hitFire.point ile merminin çıkış noktasının pozisyonlarını çıkarıp normalini alarak merminin gideceği yönü hesaplıyoruz.
        //bullet.gameObject.GetComponent<Rigidbody>().AddForce((hitFire.point-outOfAmmo.transform.position).normalized*1000); bu şekilde yazarak instantiate edilmiş objeyi hareket ettiremiyorsan. Ya Instantie fonksyonunu bir Gameobjecte atıp yapıcak bu işlemi ya da yukardaki gibi uzunca yazıcaksın.
    }

    void Rotation()
    {
        headCamRightLeft += Input.GetAxis("Mouse X")*Time.fixedDeltaTime*150; // Mouseun yukarı ve aşağı dönme değerlerini değişkene yüklüyoruz. //fixedDeltatime yapmamızın sebebi uptade artık fixedupdate olduğu için sabit bir süre oluyor, bu yüzden deltatimeın bir önemi kalmıyor. Artık sabit bir zaman üretiliyor.
        headCamUpDown += Input.GetAxis("Mouse Y")*Time.fixedDeltaTime*-150; // Mouseun sağa ve sola dönme değerlerini değişkene yüklüyoruz. //Normal update farklı zamanlarda çalışıyor, deltatime da ona göre değer üretiyor ama fixedupdate sabit olduğu için deltatime kullanmamıza gerek yok. fixeddelta time sabit bir değer.

        headCamUpDown = Mathf.Clamp(headCamUpDown,-20,20); // Mouseun yukarı aşağı dönme değerlerini kısıtlarayarak yukarı ve aşağı yönde tam tur atmasını engelliyoruz.

        headCam.transform.rotation = Quaternion.Euler(headCamUpDown,headCamRightLeft,transform.eulerAngles.z); //headCam objesinin rotasyonuna erişip bu kod sayesinde x ve y ekseninde hareket yapmasını sağlıyoruz ve z eksenine karakterin rotasyon bilgisini yüklüyoruz.
                                                                                            //Ben rotaion kullanmıştım hoca eulerAngles kullanmış

        headCam.transform.position = transform.position + headCamDistance; // headCamin pozisyonuna karakterin kendi pozisyonunu ve aradaki mesafeyi yükleyince kamera sürekli olarak o aradaki mesafe değeri doğrultusunda karakteri hareket edecektir.


        if (HorizontalInput != 0 || VerticalInput != 0 || Input.GetKey(KeyCode.Mouse1)) //Bu sayede karakter durduğu anda karakter kamera yönünde dönmeyecek, veya sağ tıka basılmamışsa eğer basılırsa nişan aldığı için tekrar dönüş yapar hale gelecek.
        {
            Physics.Raycast(Vector3.zero, headCam.transform.GetChild(0).forward,out hit);
        //Buranın çalışma şekli şu şekilde ilk önce rayin çıkacağı yani ışının çıkacağı başlangıç noktasını belirliyoruz Vector3.zero o oluyor.
        //Daha sonra hangi objenin yönüne göre ışının o yöne dönmesini sağlayacak pozisyonu giriyoruz. Burada hedef aldığımız yer headCam gameobjectinin içindeki Main Cameranın pozisyonu
        // out hit ise ilk başta yukarıda tanımladığımız gibi boş geliyor ama bu iki işlem hesaplandıktan sonra o bilgiler bu hit değişkeninin içinde tutuluyor.
        // outlu methodlardaki değişkenlerde birden fazla return edecek değer şeklinde kullanabiliriz hem toplama hem çarpma değeri gibi
        //headcam kısmı position olarak ayarlandığı zaman kameraya doğru bakıyor karakter onu düzeltmek için kameranın önüne bakacak şekilde ayarlanmalı bunun forward yazıyor Raycast kısmına
        //headCam.position dendiği zaman raycast kamera nerde ise oraya doğru çizim yapıyor ama forward olarak seçtiğimiz zaman kameranın önüne doğru çizim yapıyor.


        //transform.rotation = Quaternion.LookRotation(new Vector3(hit.point.x,0,hit.point.z)); // ilk başta sadece hit.point yazınca hitpointin y eksenindeki değeri de karaktere yükleniyor ve karakter y ekseninde de dönüyor bunu durdurmak için y ye 0 vermemiz lazım.
        //Quaternion.LookRotation ile hit.point nereye temas ederse karakterimizde o noktaya bakacak.
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(new Vector3(hit.point.x,0,hit.point.z)),0.5f); //Bu kod sayesinde kamerayı karakter hareket etmiyorken döndürdüğümüzde ve karakteri hareket ettirince karakterin o yöne daha yavaş bir dönüş elde ediyoruz.

        Debug.DrawLine(Vector3.zero,hit.point,Color.black);//Bu da 0.noktadan hitdeki bilgilere göre çizim işlemi gerçekleştiriyor. hit.point temas ettiğin nokta oluyor

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f)); // Bu kodda kameranın görüş açısındaki orta nokta baz alınarak yani nişangağımızın bulunduğu konuma ray çizdiriliyor. Camera da Viewport Rectde tekabül ediyor.
        Physics.Raycast(ray,out hitFire); // rayin oluşumu sağlanıyor ve hitFire da veriler depolanıyor.
        Debug.DrawLine(ray.origin,hitFire.point,Color.red); // Rayin başlangıç konumundan rayin çarptığı konuma doğru çizgi çizdiriliyor.



        }
    }

    void JumpUpNormal() // Bu kod animasyonda eventdeki ismi üzerinden çağırıldığı için direk çalışıyor, JumpUp animasyonunda belirlediğimiz saniyede çalışıyor.
    {
        _physic.AddForce(Vector3.up*150);
        //_physic.AddForce(0, Time.deltaTime*10000,0) hocanın yazdığı zıplama kodu
    }

    void CameraChange()
    {
        if (Input.GetKey(KeyCode.Mouse1)) // Mouse da sağ tıka bastığın zaman kameranın pozisyonu nişan alma pozisyonu olarak ayarlanıyor.
        {
            shootControl = true; // Sağ tıka basılmadan sol tıkla ateş edilmemesi için bu şekilde kullandık.
            runControl = false; // Sağ tıka basıldığında false olarak LeftShift e basıldığında ya da bırakıldığında o kodların çalışması engellendi ve sağ tıka basılığı andaki animationcontroller2deki animasyonlarındaki parametlere bakmamış oldu.
            _animator.runtimeAnimatorController = whenFired; // Ateş edildiği anda seçtiğimiz animasyon kontroller açılacak.
            Camera.main.gameObject.transform.position = Vector3.Lerp(Camera.main.gameObject.transform.position,position2.transform.position,0.1f); // Vector3.Lerp sayesinde kamera ile pozisyon arasında yumuşakça bir geçiş sağlıyoruz.
            sight.gameObject.SetActive(true); // Sağ tıka basılınca nişangah açılır.
        }
        else // Sağ tıkı bıraktığı anda ise eski pozsiyonuna geri dönüyor.
        {
            shootControl = false; // Sağ tıkı bıraktığında false olacak ve sol tık tekrar etkisiz hale gelecek.
            runControl = true; //Sağ tıktan elini çektiği anda ise tekrar koşabilir hale geldi karakter.
            _animator.runtimeAnimatorController = whennotFired; // Ateş edilmediği anda seçtiğimiz animasyon kontroller açılacak.
            Camera.main.gameObject.transform.position = Vector3.Lerp(Camera.main.gameObject.transform.position,position1.transform.position,0.1f);
            sight.gameObject.SetActive(false); // Sağ tıktan elini çekince nişangah kapanır.
        }
    }

    
}
