using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    EnemyControl enemy; //EnemyControl scriptine ulaşmak için oluşturduk.
    Rigidbody2D physic; // Rigidbody komponentine ulaşmak için oluşturduk.
    void Start()
    {
        physic = GetComponent<Rigidbody2D>(); // Rigidbody2D komponentine erişiyoruz.
        enemy = FindObjectOfType<EnemyControl>(); // EnemyControl scriptine erişiyoruz.

        physic.AddForce(enemy.GetDirection()*1000); // Mermimize güç uyguluyoruz.
        
    }

    
    void Update()
    {
        
    }

   
}
