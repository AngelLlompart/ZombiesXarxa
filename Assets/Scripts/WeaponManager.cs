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
    private AudioSource _audio;
    [SerializeField] private GameObject bloodParticleSystem;
    void Start()
    {
        //posInitCam = playerCam.transform.localPosition;
        posInitCam = fps.transform.localPosition;
        _audio = GetComponent<AudioSource>();
        Debug.Log(posInitCam);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAnimator.GetBool("isShooting"))
        {
            //playerAnimator.SetBool("isShooting", false);
            StartCoroutine(DelayAnimation());
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        playerAnimator.SetBool("isShooting", true);
        _audio.Play();
        RaycastHit hit;
        transform.Find("shootParticle").GetComponent<ParticleSystem>().Play();
        if (Physics.Raycast(playerCam.transform.position, transform.forward, out hit, range))
        {
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if(enemyManager != null)
            {
                enemyManager.Hit(dmg);
                GameObject particleInstance =
                    Instantiate(bloodParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                particleInstance.transform.parent = hit.transform;
                particleInstance.GetComponent<ParticleSystem>().Play();
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
