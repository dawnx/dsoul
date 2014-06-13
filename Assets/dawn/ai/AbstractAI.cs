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

    public Material hpMaterial;
    private Transform hpbar;
    private Hp_bar script_hpbar;
	void Start () {


        hpbar = ScriptEF.CreatHpbar(new Vector2(2.0f, 0.25f), false, hpMaterial);
        this.script_hpbar = this.hpbar.GetComponent<Hp_bar>();
        this.script_hpbar.Damaged(100, 100, this.transform, 5f, 0);
		gameObject.animation.wrapMode = WrapMode.Once;
        initObj();
	}

    public virtual void initObj() { 
    
    }

    public void Update()
    {
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
            if (atkTarget != null)
                currentState = ATK;
			else
				PlayClip("idle");
        }
        else if (currentState == RUN)
        {
            if (this.character.gameObject.transform.position != moveTarget)
            {
                moveDelta();
				PlayClip("run");
            }
            else {
                currentState = IDLE;
            }
        }
        else if (currentState == ATK)
        {
            if (atkTarget != null)
            {
                if (!isInAttackArea(atkTarget.gameObject, getAtkRange()))
                {
                    setDist(atkTarget.gameObject.transform.position);
                    moveDelta();
					PlayClip("run");
                }
                else
                {
                    atk();
                }
            }
            else {
                currentState = IDLE;
            }
        }
    }

    public void PlayClip(string clip)
    {
        if (!gameObject.animation.IsPlaying(clip))
        {
            gameObject.animation.Stop();
            gameObject.animation.Play(clip);
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
                //Debug.Log("direction:" + direction + "  true");
                return true;
            }

            //Debug.Log("direction:" + direction + "  false");
        }

        return false;
    }

    // 设置目标点
    public void setDist(Vector3 targetposition)
    {
        // 移动模型
        lookAtEx(targetposition);
        moveTarget = targetposition;
        if (this.character.gameObject.transform.position != moveTarget)
            currentState = RUN;
    }

    // 移动
    public void moveDelta() {

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveTarget, Time.deltaTime * character.moveSpeed);

        if (this.character.gameObject.transform.position == moveTarget)
        {
            currentState = IDLE;
        }

        float dis = Vector3.Distance(character.gameObject.transform.position, moveTarget);
     
        if (dis < getAtkRange())
        {
            atk();
        }
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
        else if (isInAttackArea(target.gameObject,  getAtkRange()))
		{
			Debug.Log("is attacked " + target.gameObject);

			AbstractAI ai = target.gameObject.GetComponent<NpcAi>();
			if(ai == null)
			    ai = target.gameObject.GetComponent<PlayerAI>();

			if (ai != null)
			{
			    Vector3 attackdir = this.transform.position - target.gameObject.transform.position;
			    attackdir[1] = 0f;
			    attackdir = Vector3.Normalize(attackdir);

			    ai.onHit(99, attackdir);
				PlayClip("attack1");
			    currentState = ATK;
                atk_ez_time = getAnimaLen("attack1") + 0.2f; // TODO 如果有其它的硬直存在？， TODO，这里应该为技能的CD、攻击的CD等设置
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

    public void onHit(int damage, Vector3 attackdir)
    {
        StartCoroutine(onHit(damage, attackdir, 0)); // 这里的delay应该是动作播放到到受击帧的时间
        hit_ez_time = 0.2f;
    }

    private IEnumerator onHit(int damage, Vector3 attackdir, float delay)
    {
        delay = 0.5f;// TODO
        yield return new WaitForSeconds(delay);

        ScriptEF.CreateDamageNum(this.transform.position, 5, damage, attackdir);
        this.script_hpbar.Damaged(100, 50, transform, 5f, 0);
        if (!animation.IsPlaying("attack1"))
            PlayClip("hit");
    }

    private float getAnimaLen(string name) {
        float len = gameObject.animation.GetClip(name).length;
        float speed = gameObject.animation[name].speed;
        return len / speed;
    }
}

