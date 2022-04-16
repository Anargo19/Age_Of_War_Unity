using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DyingCanvas : MonoBehaviour
{
    public TextMeshProUGUI textmesh;

    private void Start()
    {
        
        StartCoroutine(MoveAndDie());
    }

    private void Update()
    {
        transform.Translate(transform.up * Time.deltaTime);
    }



    IEnumerator MoveAndDie()
    {
      yield return new WaitForSeconds(1.25f);
      Destroy(gameObject);
    }
}
