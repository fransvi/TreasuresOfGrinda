using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour {


    [SerializeField]
    Sprite[] _torchSprites;

    Sprite _activeSprite;

    float _interval = 0.5f;
    float _nextTime = 0;

	void Update () {

        if (Time.time >= _nextTime)
        {
            StartCoroutine(TorchFlicker());
            _nextTime += _interval;

        }
    }

    IEnumerator TorchFlicker()
    {
        GetComponent<SpriteRenderer>().sprite = _torchSprites[0];
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().sprite = _torchSprites[1];
        yield return new WaitForSeconds(0.2f);
    }

}
