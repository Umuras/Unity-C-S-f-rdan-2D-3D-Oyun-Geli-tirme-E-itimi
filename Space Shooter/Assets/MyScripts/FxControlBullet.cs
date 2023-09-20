using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxControlBullet : MonoBehaviour
{
    Rigidbody physic; //Rigidbody komponentine erişmek için Rigidbody türünden değişken oluşturduk.
    public float speed; // Mermimizin daha hızlı gitmesi için değişken oluşturduk.
    void Start()
    {
        physic = GetComponent<Rigidbody>(); // Rigidbody componentine erişim sağladık.
        physic.velocity = transform.forward*speed; // physic.velocity diyerek mermimize hızına eriştik ve z ekseni yönünde hızlanmasını sağladık, transform.forward diyerek.
    }

    // void OnCollisionEnter(Collision other) //Burası benim yazdığım mermi yok etme kodu
    // {
    //     if(other.gameObject.tag == "Border")
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // void OnTriggerEnter(Collider other) //Benim kodum
    // {
    //     if (other.gameObject.tag == "enemy") //Eğer objenin tagi enemy ise
    //     {
    //         Destroy(other.gameObject); // ilk önce enemy tagli obje yok oluyor. other parametresi ile kullanırsan mermi isTriggerlı ve tagi enemy olan objeye değdiğinde onu yok eder.
    //         Destroy(gameObject); // Sonrada mermimiz yok oluyor.
    //     }
    // }
    
   
}
