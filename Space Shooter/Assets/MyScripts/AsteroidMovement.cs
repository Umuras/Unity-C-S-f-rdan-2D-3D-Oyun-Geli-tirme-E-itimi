using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    Rigidbody physic; //Rigidbody komponentine erişmek için Rigidbody türünden değişken oluşturduk.
    public float speed; // Asteroidimizin daha hızlı gitmesi için değişken oluşturduk.
    void Start()
    {
        physic = GetComponent<Rigidbody>(); // Rigidbody componentine erişim sağladık.
        physic.velocity = transform.forward*speed; // physic.velocity diyerek asteroidimize hızına eriştik ve z ekseni yönünde hızlanmasını sağladık, transform.forward diyerek.
       // physic.velocity = Vector3.forward*speed; Benim yazdığım hareket kodu.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
