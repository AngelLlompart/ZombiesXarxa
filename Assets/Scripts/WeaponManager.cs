using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject fps;
    public float range;
    private float dmg = 25;
    private Vector3 posInitCam;

    [SerializeField] private Animator playerAnimator;
    void Start()
    {
        //posInitCam = playerCam.transform.localPosition;
        posInitCam = fps.transform.localPosition;
        Debug.Log(posInitCam);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (playerAnimator.GetBool("isShooting"))
        {
            //playerAnimator.SetBool("isShooting", false);
            StartCoroutine(DelayAnimation());
        }
    }

    private void Shoot()
    {
        playerAnimator.SetBool("isShooting", true);
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, transform.forward, out hit, range))
        {
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if(enemyManager != null)
            {
                enemyManager.Hit(dmg);
            }
        }

    }

    IEnumerator DelayAnimation()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        playerAnimator.SetBool("isShooting", false);
    }
    IEnumerator FpsMove()
    {
        float x = Random.Range(0, 0.05f);
        float y = Random.Range(0, 0.03f);
        fps.transform.localPosition = new Vector3(fps.transform.localPosition.x + x, fps.transform.localPosition.y + y,
            fps.transform.localPosition.z);
        //playerCam.transform.position = new Vector3(playerCam.transform.position.x + x, playerCam.transform.position.y, playerCam.transform.position.z + z);
        yield return new WaitForSecondsRealtime(0.2f);
        fps.transform.localPosition = posInitCam;
        //playerCam.transform.localPosition = posInitCam;
    }
}
