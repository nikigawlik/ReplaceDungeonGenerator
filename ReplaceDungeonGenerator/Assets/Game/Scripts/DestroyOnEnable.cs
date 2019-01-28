using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnable : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Destroy(this.gameObject);
    }
}
