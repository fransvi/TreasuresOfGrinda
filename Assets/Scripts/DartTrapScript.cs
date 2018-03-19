using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrapScript : MonoBehaviour {
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _shootPoint;
    [SerializeField]
    private float _shootForce;
    [SerializeField]
    private bool _shootDirection;

    private bool _triggered;

    void Start()
    {
        _triggered = false;
    }

    private void Shoot()
    {
            GameObject go = Instantiate(_bullet, _shootPoint.gameObject.transform.position, _shootPoint.gameObject.transform.rotation);
            go.GetComponent<BulletScript>().createBullet(false, _shootForce, 0, true, 4);
            Destroy(go, 3.0f);
    }

    IEnumerator BurstShot()
    {
        Shoot();
        yield return new WaitForSeconds(1f);
        Shoot();
        yield return new WaitForSeconds(1f);
        Shoot();
        yield return new WaitForSeconds(1f);
    }

    public void TriggerTrap()
    {
        if (!_triggered)
        {
            StartCoroutine(BurstShot());
            _triggered = true;
        }


    }
    
}
