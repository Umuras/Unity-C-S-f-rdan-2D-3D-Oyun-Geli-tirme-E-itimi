using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    
    public float speed; //Burada çemberimizin dönüş hızını belirleyen speed değişkenini belirledik.
    
    void Update()
    {
        transform.Rotate(0,0,speed*Time.deltaTime); //Burada ise çemberimiz z ekseninde döneceği için x ve y yi 0 yapıp z ekseninde
                                                    // speed*Time.deltaTime ile çarparak normal hızda dönmesini sağladık.
    }
}
