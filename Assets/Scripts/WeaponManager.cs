using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class WeaponManager : MonoBehaviour, IOnEventCallback
{
    public GameObject playerCam;
    public GameObject fps;
    public float range;
    private float dmg = 25;
    private Vector3 posInitCam;

    [SerializeField] private Animator playerAnimator;
    private AudioSource _audio;
    [SerializeField] private GameObject bloodParticleSystem;
    [SerializeField] private GameObject rockParticleSystem;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadText;
    
    private int maxAmmo = 30;
    public int ammo;
    public PhotonView photonView;

    public GameManager gameManager;

    private const byte VFX_EVENT = 0;
    void Start()
    {
        //posInitCam = playerCam.transform.localPosition;
        posInitCam = fps.transform.localPosition;
        _audio = GetComponent<AudioSource>();
        ammo = maxAmmo;
        ammoText.text = "Ammo: "+ ammo;
        reloadText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }
        if (playerAnimator.GetBool("isShooting"))
        {
            //playerAnimator.SetBool("isShooting", false);
            StartCoroutine(DelayAnimation());
        }

        if (!gameManager.paused && !gameManager.gameOver){
            if (Input.GetButtonDown("Fire1"))
            {
                if (ammo > 0)
                {
                    Shoot();
                }
            }
        }

    if (ammo == 0)
        {
            reloadText.gameObject.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammo < maxAmmo)
            {
                ammo = maxAmmo;
                reloadText.gameObject.SetActive(false);
                ammoText.text = "Ammo: " + ammo;
            }
        }
    }

    private void Shoot()
    {
        if (PhotonNetwork.InRoom)
        {
            int viewID = photonView.ViewID;

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };

            PhotonNetwork.RaiseEvent(VFX_EVENT, viewID, raiseEventOptions, sendOptions);
            //photonView.RPC("WeaponShootVFX", RpcTarget.All, photonView.ViewID);
        }
        else
        {
            ShootVFX(photonView.ViewID);
        }
        playerAnimator.SetBool("isShooting", true);
        RaycastHit hit;
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
            else
            {
                GameObject particleInstance =
                    Instantiate(rockParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                particleInstance.transform.parent = hit.transform;
                particleInstance.GetComponent<ParticleSystem>().Play();
            }
        }

        ammo--;
        ammoText.text = "Ammo: " + ammo;

    }

    public void ShootVFX(int viewID)
    {
        if (photonView.ViewID == viewID)
        {
            _audio.Play();
            transform.Find("shootParticle").GetComponent<ParticleSystem>().Play(); 
        }
    }

    IEnumerator DelayAnimation()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        playerAnimator.SetBool("isShooting", false);
    }
   /* IEnumerator FpsMove()
    {
        float x = Random.Range(0, 0.05f);
        float y = Random.Range(0, 0.03f);
        fps.transform.localPosition = new Vector3(fps.transform.localPosition.x + x, fps.transform.localPosition.y + y,
            fps.transform.localPosition.z);
        //playerCam.transform.position = new Vector3(playerCam.transform.position.x + x, playerCam.transform.position.y, playerCam.transform.position.z + z);
        yield return new WaitForSecondsRealtime(0.2f);
        fps.transform.localPosition = posInitCam;
        //playerCam.transform.localPosition = posInitCam;
    }*/

   void IOnEventCallback.OnEvent(EventData photonEvent)
   {
       if (photonEvent.Code == VFX_EVENT)
       {
           int viewID = (int)photonEvent.CustomData;
           ShootVFX(viewID);
       }
   }
   private void OnEnable()
   {
       PhotonNetwork.AddCallbackTarget(this);
   }

   private void OnDisable()
   {
       PhotonNetwork.RemoveCallbackTarget(this);
   }
}
