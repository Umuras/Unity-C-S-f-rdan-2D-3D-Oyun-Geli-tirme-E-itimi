using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) // Eğer mermimiz herhangi bir objeye değerse görünürlüğü kapanacaktır.
    {
        gameObject.SetActive(false); //gameObject. diye yazınca script üstüne yüklü olan objeye yani mermiye etki eder. Kodun çalışması mermiyi isTrigger yapman gerekli.
    
    }
}
