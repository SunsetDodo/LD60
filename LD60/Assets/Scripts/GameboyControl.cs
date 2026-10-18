using UnityEngine;

public class GameboyControl : MonoBehaviour
{
    [SerializeField] private bool isHeld;
    [SerializeField] private bool isTurnedOn;

    
    [SerializeField] private Transform playerSpawn;

    private Animator _gameboyAnimator;
    [SerializeField] private Animator blackScreenAnimator;

    [SerializeField] private string[] triggerNames;
    
    private void Start()
    {
        _gameboyAnimator = GetComponent<Animator>();
    }


    public void PressPowerButton()
    {
        if (!isHeld) return;
        if (isTurnedOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
        
        isTurnedOn = !isTurnedOn;
    }


    private void TurnOn()
    {
        blackScreenAnimator.SetTrigger("OpenScreen");
    }

    private void TurnOff()
    {
        blackScreenAnimator.SetTrigger("CloseScreen");
    }


    private void FixedUpdate()
    {
        var res = Random.Range(0, 360);
        if (res == 0)
        {
            _gameboyAnimator.SetTrigger(triggerNames[Random.Range(0, triggerNames.Length)]);
        }
    }
    
    void Update()
    {
        if (isHeld) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {   
            _gameboyAnimator.SetTrigger("isHeld");
            isHeld = true;
        }
    }
}
