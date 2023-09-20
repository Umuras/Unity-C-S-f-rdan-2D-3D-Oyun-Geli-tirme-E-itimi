using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCamera : MonoBehaviour
{
    Vector3 betweenDistance;
    public GameObject ball;

    void Start()
    {
        betweenDistance = transform.position - ball.transform.position; //Burada kamera ile topun arasındaki mesafeyi hesaplıyoruz.
    }                                                                   // Topun pozisyonunu neden kameradan çıkarmadıkda kameranın pozisyonunu toptan çıkardık
                                                                        // Çünkü kameranın pozisyon değerleri daha büyük kamera toptan daha uzaktan top 0 konumuna yakın olduğu için toptan kameranın pozisyonunu çıkarırsak eğer değerimiz negatif çıkacaktı.
    
    void LateUpdate() //LateUpdate bütün uptadeler bittikten sonra çalışıyor kamera işlemlerinde kullanmak gerekiyormuş. Kameradaki titremeleri önlüyor.
    {
        transform.position = ball.transform.position + betweenDistance; // Burada eğer kameranın pozisyonun topun pozisyonuna eşitlersek kamera topun üstünde kalacaktır ama eğer üstüne bir start kısmında hesapladığımız değeride eklersek kamera sürekli o mesafe doğrultusunda topu takip edecektir.
    }
}
