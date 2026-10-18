using UnityEngine;

public class GameboyControl : MonoBehaviour
{
    [SerializeField] private bool isHeld;
    private bool isTurnedOn = false;
    public bool started;

    
    [SerializeField] private Transform playerSpawn;

    private Animator _gameboyAnimator;
    [SerializeField] private Animator blackScreenAnimator;

    [SerializeField] private string[] triggerNames;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject gameboyCamera;
    private CameraScript _gameboyCameraScript;
    [SerializeField] private GameObject startScreenCameraPos;
    [SerializeField] private GameObject startGameCameraPos;

    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource oldManPlayer;
    [SerializeField] private AudioClip startScreenMusic;
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip[] groans;
    [SerializeField] private AudioClip oldManDeathClip;
    
    
    [SerializeField] private SpriteRenderer helpMessage;
    [SerializeField] private Sprite pressPowerMessage;
    [SerializeField] private Sprite restartGameMessage;
    [SerializeField] private Sprite youWonMessage;
    [SerializeField] private GameObject deathMessage;
    [SerializeField] private Animator fadeAnimator;

    private bool won = false;
    
    private void Start()
    {
        _gameboyCameraScript = gameboyCamera.GetComponent<CameraScript>();
        _gameboyAnimator = GetComponent<Animator>();
        _gameboyAnimator.SetTrigger("isHeld");
        isHeld = true;
    }


    public void PressPowerButton()
    {
        if (!isHeld) return;
        if (isTurnedOn)
        {
            isTurnedOn = false;
            TurnOff();
        }
        else
        {
            isTurnedOn = true;
            TurnOn();
        }
    }


    private void TurnOn()
    {
        blackScreenAnimator.SetTrigger("OpenScreen");
        isTurnedOn = true;
        gameboyCamera.transform.position = startScreenCameraPos.transform.position;
        musicPlayer.clip = startScreenMusic;
        musicPlayer.Play();
        helpMessage.sprite = restartGameMessage;
    }

    private void TurnOff()
    {
        isTurnedOn = false;
        started = false;
        blackScreenAnimator.SetTrigger("CloseScreen");
        musicPlayer.Stop();
        
        _gameboyCameraScript.isFollowing = false;
        _gameboyCameraScript.followObject = null;
        
        
        Destroy(GameObject.FindWithTag("Player"));
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        
        helpMessage.sprite = pressPowerMessage;
    }


    private void FixedUpdate()
    {
        if (!won)
        {
            var res = Random.Range(0, 3600);
            if (res < 10)
            {
                _gameboyAnimator.SetTrigger(triggerNames[Random.Range(0, triggerNames.Length)]);
            }

            if (res < 5)
            {
                oldManPlayer.clip = groans[Random.Range(0, groans.Length)];
                oldManPlayer.Play();
            }
        }
        else
        {
            if (!oldManPlayer.isPlaying)
            {
                deathMessage.SetActive(true);
            }
        }
        
    }
    

    public void StartGame()
    {
        if (!isTurnedOn) return;
        musicPlayer.clip = mainMusic;
        musicPlayer.Play();
        started = true;
        gameboyCamera.transform.position = startGameCameraPos.transform.position;
        var spawnPoint = GameObject.FindWithTag("Player Spawn");
        var player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        player.GetComponent<PlayerMovement>().control = this;
        _gameboyCameraScript.followObject = player;
        _gameboyCameraScript.isFollowing = true;
        
        foreach (var enemySpawnPoint in GameObject.FindGameObjectsWithTag("Enemy Spawn"))
        {
            Instantiate(enemyPrefab, enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation);
        }
    }


    public void WinGame()
    {
        helpMessage.sprite = youWonMessage;
        oldManPlayer.clip = oldManDeathClip;
        musicPlayer.Stop();
        oldManPlayer.volume = 1f;
        oldManPlayer.Play();
        fadeAnimator.SetTrigger("WinGame");
        won = true;
    }
}
