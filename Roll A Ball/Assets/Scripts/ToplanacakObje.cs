﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToplanacakObje : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45)*Time.deltaTime); //Burada transform.Rotate kodu ile objemizi girilen x y z değerleri ekseninde döndürmeyi başarıyoruz.
    }                                                             // Time.deltaTime ile normal hız ile dönmesi sağlanıyor.
}
