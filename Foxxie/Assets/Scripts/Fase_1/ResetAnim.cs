using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnim : MonoBehaviour
{
    public void ResetaAnimFalhaTrocaDim()
    {
        GetComponent<Animator>().SetBool("falha", false);
    }
}
