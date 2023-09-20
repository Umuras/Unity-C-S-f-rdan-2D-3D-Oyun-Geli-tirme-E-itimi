using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAsteroids : MonoBehaviour
{
    Rigidbody physic; // Burada rigidbody değişkeni oluşturuyoruz.
    public float speed; // Burada hız değişkeni oluşturuyoruz asteroidin daha hızlı dönebilmesi için.
    void Start()
    {
        physic = GetComponent<Rigidbody>(); // physic değişkenine Rigidbody komponentindeki verileri yüklüyoruz.
        physic.angularVelocity = Random.insideUnitSphere*speed; // Burada physic.angularVelocity ile açısal hıza erişiyoruz ve oyun her çalıştığında
                                                                // Asteriod rastgele bir şekilde dairesel hareket yapmasını sağlıyoruz.
                                                                // insideUnitSphere görevi Yarıçapı 1 olan bir küre içinde rastgele bir nokta döndürür.
    }

   
}
