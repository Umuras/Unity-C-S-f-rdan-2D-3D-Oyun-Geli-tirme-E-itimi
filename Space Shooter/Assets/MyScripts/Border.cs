using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    void OnTriggerExit(Collider other) //Bir gameobject ile temas yapıldığında ve kendi sınırından çıkıldığı anda işlem yapılır.
    {
        Destroy(other.gameObject); // Burada sınırın sonundan çıkan gameobjectler yok olacaktır.
    }
}
