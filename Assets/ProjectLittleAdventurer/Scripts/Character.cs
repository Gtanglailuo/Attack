using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Character : MonoBehaviour
{
    private CharacterController _cc;

    public float MoveSpeed = 5f;

    private Vector3 _movementVeclocity;

    private PlayerInput _playerInput;

    public float Gravity = -9.8f;

    private float _verticalVelocity;

    private Animator _animator;

    public bool IsPlayer = true;

    private NavMeshAgent _navMeshAgent;
    private Transform TargetPlayer;

    public float attackStartTime;

    public float AttackSlideDuration = 0.4f;

    public float AttackSlideSpeed = 0.06f;

    private Health _health;

    private DamageCaster _damageCaster;
    public enum CharacterState
    {
        Normal,Attacking,Dead,BeingHit,Slide,Spawn
    }

    public CharacterState CurrentState;

    private MaterialPropertyBlock _materialPropertyBlock;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    public GameObject ItemToDrop;

    private Vector3 impactOnCharact;

    public bool IsInvincible;

    public float invincibleDuration = 2f;

    public int Coin = 0;

    public float attackAnimationDuration;

    public float SlideSpeed;

    public float SpawnDuration = 2f;

    private float currentSpawnTime;
    private void Awake()
    {
        _health = GetComponent<Health>();

        _animator = GetComponent<Animator>();

        _damageCaster = GetComponentInChildren<DamageCaster>();

        _skinnedMeshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

        _cc = GetComponent<CharacterController>();
        if (!IsPlayer)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            TargetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = MoveSpeed;
            SwitchStateTo(CharacterState.Spawn);
        }
        else
        {
            
            _playerInput = GetComponent<PlayerInput>();
        }
    }
    private void CalculatePlayerMovement()
    {
        if (_playerInput.MouseButtonDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Attacking);
            return;
        }
        else if(_playerInput.SpaceKeyDown&&_cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Slide);
            return;
        }

        _movementVeclocity.Set(_playerInput.HorizontalInput,0f,_playerInput.VerticalInput);
        _movementVeclocity.Normalize();//归一化

        _animator.SetFloat("Speed", _movementVeclocity.magnitude);
        _movementVeclocity = Quaternion.Euler(0, -45f, 0) * _movementVeclocity;
        _movementVeclocity *= MoveSpeed * Time.deltaTime;


        if (_movementVeclocity!=Vector3.zero)
        {
             transform.rotation = Quaternion.LookRotation(_movementVeclocity);
        }

        _animator.SetBool("AirBorne",!_cc.isGrounded);

    }

    private void CalculateEnemyMovement()
    {
        float distance = Vector3.Distance(this.transform.position,TargetPlayer.position);
        if (distance>_navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(TargetPlayer.position);
            _animator.SetFloat("Speed",0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(this.transform.position);
            _animator.SetFloat("Speed", 0f);

            SwitchStateTo(CharacterState.Attacking);
        }
    
    }

    private void FixedUpdate()
    {
        
        switch (CurrentState)
        {
            case CharacterState.Normal:
                if (IsPlayer)
                {
                    CalculatePlayerMovement();      
                }
                else
                {
                    CalculateEnemyMovement();
                }
                break;
            case CharacterState.Attacking:
                if (IsPlayer)
                {
                    _movementVeclocity = Vector3.zero;

                    if (Time.time < attackStartTime + AttackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / AttackSlideDuration;
                        _movementVeclocity = Vector3.Lerp(transform.forward*AttackSlideSpeed,Vector3.zero,lerpTime);

                    }

                    if (_playerInput.MouseButtonDown && _cc.isGrounded)
                    {
                        //获取在游戏对象上播放的当前动画片段的名称
                        string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        attackAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                        if (currentClipName != "LittleAdventurerAndie_ATTACK_03"&& attackAnimationDuration>0.5f&& attackAnimationDuration<0.8f)
                        {
                            _playerInput.MouseButtonDown = false;
                            SwitchStateTo(CharacterState.Attacking);

                            CalculatePlayerMovement();
                        }



                    }
                }

                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                if (impactOnCharact.magnitude>0.2f)
                {
                    _movementVeclocity = impactOnCharact * Time.deltaTime;
                }
                impactOnCharact = Vector3.Lerp(impactOnCharact,Vector3.zero,Time.deltaTime*5);
                break;

            case CharacterState.Slide:
                Vector3 lastPos = _movementVeclocity;
                _movementVeclocity = transform.forward * SlideSpeed * Time.deltaTime;
                
                break;
            case CharacterState.Spawn:
                currentSpawnTime -= Time.deltaTime;
                if (currentSpawnTime<=0)
                {
                    SwitchStateTo(CharacterState.Normal);
                }

                break;

            default:
                break;
        }

        if (IsPlayer)
        {
            if (_cc.isGrounded == false)
            {
                _verticalVelocity = Gravity;
            }
            else
            {
                _verticalVelocity = Gravity * 0.3f;
            }
            _movementVeclocity += _verticalVelocity * Vector3.up * Time.deltaTime;

            _cc.Move(_movementVeclocity);
        }
    }

    public void SwitchStateTo(CharacterState newState)
    {
        if (IsPlayer)
        {
            _playerInput.ClearCaet();
        }

        //退出状态
        switch (CurrentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if (_damageCaster!=null)
                {
                    DisableDamageCaster();    
                }
                if (IsPlayer)
                {
                    GetComponent<VFXManger>().StopBlade();
                }
                break;

            case CharacterState.Dead:
                break;
            case CharacterState.BeingHit:
                break;
            case CharacterState.Slide:
                break;
            case CharacterState.Spawn:
                IsInvincible = false;
                break;
            default:
                break;
        }

        //进入状态
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                _animator.SetTrigger("Attack");
                if (IsPlayer)
                {
                    attackStartTime = Time.time;
                }
                if (!IsPlayer)
                {
                    Quaternion quaternion = Quaternion.LookRotation(TargetPlayer.position- transform.position);
                    transform.rotation = quaternion;
                }
                break;
            case CharacterState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.BeingHit:
                _animator.SetTrigger("BeingHit");
                if (IsPlayer)
                {
                    IsInvincible = true;
                    StartCoroutine(DelayCancelInvincible());

                }
                break;
            case CharacterState.Slide:
                _animator.SetTrigger("Slide");
                break;
            case CharacterState.Spawn:
                IsInvincible = true;
                currentSpawnTime= SpawnDuration;
                StartCoroutine(MaterialAppear());

                break;
            default:
                break;
        }

        CurrentState = newState;
    }
    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public void SlideAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public void BeingHitAnimatorEnds()
    {
        if (_health.CurrentHealth<=0)
        {
            SwitchStateTo(CharacterState.Dead);
            return;
        }
        SwitchStateTo(CharacterState.Normal);
    }
    private void AddImpact(Vector3 attackPos, float force)
    {
        Vector3 impactDir = transform.position - attackPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharact = impactDir * force;
    }
    public void ApplyDamage(int damage,Vector3 vectorPos=new Vector3())
    {
        if (IsInvincible)
        {
            return;
        }

        if (_health!=null)
        {
            if (IsPlayer)
            {
                SwitchStateTo(CharacterState.BeingHit);
                AddImpact(vectorPos,10.0f);
            }
            _health.ApplyDamage(damage);
            
        }

        if (!IsPlayer)
        {
            GetComponent<EnemyVFXManger>().PlayBeingHitVFX(vectorPos);
        }
        
        StartCoroutine(MaterialBlink());
    }

    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);
        IsInvincible = false;


    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();

    }

    public void DisableDamageCaster()
    {

        _damageCaster.DisableDamageCaster();
    }

    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink",1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        yield return new WaitForSeconds(0.2f);

        _materialPropertyBlock.SetFloat("_blink", 0.0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float a = 20f;
        float b = -10f;
        float height;
        _materialPropertyBlock.SetFloat("_enableDissolve",1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        while (currentDissolveTime<=dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            height = Mathf.Lerp(a,b,currentDissolveTime/dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height",height);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

            yield return null;
        }
        ItemDrop();
    }

    IEnumerator MaterialAppear()
    {

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float b = 20f;
        float a = -10f;
        float height;
        _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        while (currentDissolveTime <= dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            height = Mathf.Lerp(a, b, currentDissolveTime / dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height", height);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

            yield return null;
        }
        _materialPropertyBlock.SetFloat("_enableDissolve", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        SwitchStateTo(CharacterState.Normal);
    }

    private void ItemDrop()
    {
        if (ItemToDrop!=null)
        {
            Instantiate(ItemToDrop,transform);
        }
    
    }

    public void PickUpItem(PickUp item)
    {
        switch (item.Type)
        {
            case PickUp.PickUpType.Heal:
                AddHealth(item.Value);
                break;
            case PickUp.PickUpType.Coin:
                AddCoin(item.Value);
                break;
            default:
                break;
        }
    }

    private void AddHealth(int health)
    {

        _health.AddHealth(health);
        GetComponent<VFXManger>().PlayHealVFX();
    }

    private void AddCoin(int coin)
    { 
        Coin+=coin;
    }

    public void RotateTarget()
    {
        if (CurrentState!=CharacterState.Dead)
        {
            transform.LookAt(TargetPlayer,Vector3.up);
        }  
    }

    

}
