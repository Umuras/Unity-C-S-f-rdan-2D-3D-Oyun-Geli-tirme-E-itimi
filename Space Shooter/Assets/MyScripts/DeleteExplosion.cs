using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteExplosion : MonoBehaviour
{
    
    void Start()
    {
        Destroy(gameObject,3f); //Burada oyunda particle efektlerin oyunda kalmayıp 3 saniye sonra yok edilmesi için yazılmıştır,
                                // Ama bende şuanda particle efeklerde yok olduğu için bu kodu particle efekt prefablarının içine yüklemeyeceğim.
    }

    
}
