using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealthChest : MonoBehaviour
{
    public Sprite[] spriteAnimations; //Spriteları yükleyebilmek için sprite türünde dizi oluşturuyoruz.
    SpriteRenderer _spriteRenderer; // SpriteRenderer komponentine erişmek için değişkeni oluşturuyoruz.
    float animationTime; // Sprite animasyonunun belirli bir zaman içerisinde çalışması için oluşturuyoruz. 
    int animationCounter=0; // Sprite dizisi içerisinde hareket edebilmek için oluşturuyoruz.
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>(); //spriteRenderer komponentine erişiyoruz.
    }

    
    void Update()
    {
        ChestAnim();
    }

     void ChestAnim()
    {
        
        animationTime += Time.deltaTime; //animationTime değişkenine geçen zamanı yüklüyoruz.

        if (animationTime > 0.1f) // Eğer zaman 0.1fi geçerse çalışmaya başlıyor.
        {
            _spriteRenderer.sprite = spriteAnimations[animationCounter]; //SpriteRenderer komponentinin içine spritelarımızı animationCounter sayısı doğrultusu boyunca sırayla yüklüyoruz.
            animationCounter++;// Bir diğer sprite a geçmesi için sayacı arttırıyoruz.
            if (animationCounter > spriteAnimations.Length-1) //Eğer sayacımız maksimum sprite sayısını geçtiği an
            {
                animationCounter = spriteAnimations.Length-1; // Sayacı maksimum sprite sayısına eşitliyerek animasyonunun bitmesini sağlıyoruz bu sayede sandık bir kere açılıyor ve öyle kalıyor.
            }
            animationTime = 0; // Zamanı sıfırlayarak if e tekrar girilmesini sağlıyoruz.
        }
    }
}
