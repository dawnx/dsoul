using UnityEngine;
using System.Collections;

/**
 * 所有AI的基类，定义AI的状态，以及通用的属性 
 * 
 * 
 */
public abstract class AbstractAI : MonoBehaviour
{

    public const int IDLE = 1;

    public const int RUN = 2;

    public const int ATK = 3;

    private float atk_ez_time = 0;

    private float hit_ez_time = 0;

    public int currentState = IDLE;

    public Character character;

    public Character atkTarget;

    private Vector3 moveTarget;

    private BloodBar bloodBar;

    private CameraCtrl cameraCtrl;

    private CharacterController myController;

    private bool isInRun = false;

    private string currentClip;

    void Start () {

        this.bloodBar = ScriptEF.CreateBloodBar(gameObject);
		gameObject.animation.wrapMode = WrapMode.Once;
        cameraCtrl = Camera.main.GetComponent<CameraCtrl>();
        myController = GetComponent<CharacterController>();
        initObj();
	}

    public virtual void initObj() { 
    
    }

    public BloodBar getBloodBar() {

        return bloodBar;
    }

    public virtual void Update()
    {
        if (IsDied())
            return;

        if (hit_ez_time > 0)
        {
            hit_ez_time -= Time.deltaTime;
            return;
        }

        if (atk_ez_time > 0)
        {
            atk_ez_time -= Time.deltaTime;
        }

        if (currentState == IDLE)
        {
            atkTarget = getAtkTarget();
            if (atkTarget != null && !atkTarget.ai.IsDied())
                currentState = ATK;
			else
				PlayClip("idle");
        }
        else if (currentState == RUN)
        {
            if (moveDelta(moveTarget))
			{
				PlayClip("run");
            }
            else {
                currentState = IDLE;
            }
        }
        else if (currentState == ATK)
        {
            if (atkTarget != null && !atkTarget.ai.IsDied())
            {
                if (!isInAttackArea(atkTarget.gameObject, getAtkRange()))
                {
                    if(moveDelta(atkTarget.gameObject.transform.position))
					    PlayClip("run");
                }
                else
                {
                    atk();
                }
            }
            else {
                atkTarget = getAtkTarget();
                if (atkTarget == null || !atkTarget.ai.IsDied())
                {
                    currentState = IDLE;
                }
            }
        }
    }

    public virtual bool PlayClip(string clip, string setClip="")
    {
        if (!gameObject.animation.IsPlaying(clip))
        {
            gameObject.animation.Stop();
            gameObject.animation.Play(clip);
            if(setClip == "")
                currentClip = clip;
            else
                currentClip = setClip;
            return true;
        }

        return false;
    }

    public virtual void SwitchClip(string clip){

        // 主要是两个地方，一个是run -> idle ，再是fight_idle状态
        if (clip == "idle")
        {
            if (currentClip == "attack1" || currentClip == "attack3" || currentClip == "attack3")
                PlayClip("fight_idle", "idle");
            else if (currentClip == "run")
                PlayClip("run_idle", "idle");
            else
                PlayClip("idle", "idle");
        } else if (clip == "run")
        {
            if (currentClip == "attack1" || currentClip == "attack3" || currentClip == "attack3")
                PlayClip("fight_run", "run");
            else if (currentClip == "idle")
                PlayClip("idle_run", "run");
            else
                PlayClip("run", "run");
        }
    }
    
    public bool isInAttackArea(GameObject target, float atkRange)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        //Debug.Log(direction);
        if (distance <= atkRange)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;//normalized规格化
            float direction = Vector3.Dot(dir, transform.forward);//点 //transform.forward前方
            if (direction >= 0)
            {
                return true;
            }
        }

        return false;
    }

    // 设置目标点
    public void setDist(Vector3 targetposition)
    {
        // 移动模型
        moveTarget = targetposition;
        if (this.character.gameObject.transform.position != moveTarget)
            currentState = RUN;
    }
    // 移动
    public bool moveDelta(Vector3 dist) {

        if (Vector3.Distance(dist, transform.position) < 0.2)
            return false;

        lookAtEx(dist);
        //Vector3 oldPos = transform.position;
        //得到角色控制器组件
        //Vector3 v = Vector3.MoveTowards(transform.position, dist, Time.deltaTime * character.moveSpeed);
        //可以理解为主角行走或奔跑了一步
        //CollisionFlags flag = myController.Move(v - transform.position);
        //Debug.Log("flag:" + flag + "  bool:" + (flag == CollisionFlags.None) + " v-:" + (v - transform.position));
        //return flag == CollisionFlags.None;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        myController.SimpleMove(forward * character.moveSpeed);
        return true;
    }

    public void atk()
    {
        if (atkTarget == null)
        {
            Debug.Log("target is null");
            currentState = IDLE;
            return;
        }

        if (atk_ez_time > 0)
            return;

        GameObject target = atkTarget.gameObject;

        if (currentState != ATK)
        {
            currentState = ATK;
        }
        else if (isInAttackArea(target.gameObject, getAtkRange()) && !atkTarget.ai.IsDied())
		{
			//Debug.Log("is attacked " + target.gameObject);
            currentState = ATK;
            //atk_ez_time = GetAnimaLen("attack1") + 0.2f; // TODO 如果有其它的硬直存在？， TODO，这里应该为技能的CD、攻击的CD等设置
            atk_ez_time = character.atkSpeed;

            AbstractAI targetAI = atkTarget.ai;

            if (!targetAI.IsDied())
            {
                Skill skill = getSkill();
                PlayClip(skill.atkClip);
                animation.PlayQueued("idle");

                float delay = 0.5f;// TODO 这里的delay应该是动作播放到到受击帧的时间
                StartCoroutine(LateAtk(atkTarget, delay, skill));
            }
		}

        gameObject.transform.LookAt(target.gameObject.transform.position);
    }

    public void lookAtEx(Vector3 targetposition)
    {
        targetposition.y = this.transform.position.y;

        gameObject.transform.LookAt(targetposition);
    }

    // 获取攻击范围
    public abstract int getAtkRange();

    // 获取攻击目标
    public abstract Character getAtkTarget();

    private IEnumerator LateAtk(Character _target, float delay, Skill skill)
    {

        skill.cast(this.character, _target);
        yield return new WaitForSeconds(delay);
        
        AbstractAI targetAI = _target.ai;
        if (this.IsDied() || targetAI.IsDied() || !isInAttackArea(_target.gameObject, getAtkRange()))
            yield break;
        
        bool isCrit = IsCrit(skill, _target);
        bool isDodge = IsDodge(skill, _target);
        int damage = GetDamage(atkTarget, skill, isCrit, isDodge);

        _target.hp -= damage;
        Vector3 attackdir = this.transform.position - _target.gameObject.transform.position;
        attackdir[1] = 0f;
        attackdir = Vector3.Normalize(attackdir);
        if (_target.type == CharacterType.MONSTER && isCrit && !isDodge)
            cameraCtrl.Hitcam();
        targetAI.OnHit(damage, attackdir, character, isCrit, isDodge);
    }

    private void OnHit(int damage, Vector3 attackdir, Character attacker, bool isCrit, bool isDodge)
    {

        // 反击
        if (this.currentState!=RUN && (this.atkTarget == null || this.atkTarget.ai.IsDied())) {
            this.atkTarget = attacker;
            this.currentState = ATK;
        }

        ScriptEF.CreateDamageNum(this.transform.position, 2, damage, attackdir, isCrit, isDodge);
        this.bloodBar.Damaged(character.maxHp, character.hp);

        if (isDodge)
            return;

        if (!IsDied())
        {
            if (animation.IsPlaying("idle") || animation.IsPlaying("run"))
                PlayClip("hit");
        }
        else {
            PlayClip("die1");
            this.bloodBar.gameObject.SetActive(false);
            Vector3[] path = new Vector3[2];
            Ray ray = new Ray();
            ray.origin = attacker.gameObject.transform.position;
            ray.direction = this.transform.position - attacker.gameObject.transform.position;
            path[0] = ray.GetPoint(3);
            path[0].y = 1.5f;
            path[1] = ray.GetPoint(4); ;
            iTween.MoveTo(this.gameObject, iTween.Hash("path", path, "time", 1.5f));
            myController.enabled = false;
        }
    }

    private float GetAnimaLen(string name) {
        float len = gameObject.animation.GetClip(name).length;
        float speed = gameObject.animation[name].speed;
        return len / speed;
    }

    int GetDamage(Character target, Skill skill, bool isCrit, bool isDodge) {

        if (isDodge)
            return 0;
        
        if (this.character != null && target != null)
        {
            int damage = character.atk - target.def;

            if (isCrit)
                damage = damage * 15 / 10;

            damage += Random.Range(-10, 10);

            if (damage < 1)
                damage = 1;

            return damage;
        }

        return 0;
    }

    public bool IsDied() {
        if (character == null)
            return true;

        return character.hp <= 0;
    }

    public static AbstractAI GetTargetAI(GameObject go) {

        if (go == null)
            return null;

        AbstractAI ai = go.GetComponent<MonsterAI>();

        if (ai == null)
            ai = go.GetComponent<PlayerAI>();

        return ai;
    
    }

    public void Reborn() {
        character.hp = character.maxHp;
        this.bloodBar.gameObject.SetActive(true);
        this.transform.position = character.bornPostion;
        this.bloodBar.Damaged(character.maxHp, character.maxHp);
        iTween.Stop(this.gameObject);
        myController.enabled = true;
    }

    private bool IsCrit(Skill skill, Character _target) {
        return Random.Range(0, 100) < 50;
    }

    private bool IsDodge(Skill skill, Character _target)
    {
        return Random.Range(0, 100) < 20;
    }

    public virtual Skill getSkill() {
        return null;
    }
}

