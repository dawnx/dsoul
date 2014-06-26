using UnityEngine;

public class Skill
{
    public int id = 0;
    public string name = null;//技能名
    public string effect = null; // 特效名
    public int damage = 0;// 伤害
    public int targetType = 0;// 目标类型
    public int targetNumber = 0;// 目标数量
    public int atkRange = 0;//攻击范围
    public int cd = 0;//count down 单位秒
    public string atkClip = "";
	public string hitClip = "";
	public float hitDelay = 0.3f;
	public float knockbackDistance = 1;// 击退距离
    public int effectTarget = 0;//目标类型
    public Vector3 effectOffSet = Vector3.zero;
    public const int CASTER = 1;
    public const int TARGET = 2;

    public static Skill monsterSkill1;

    public static Skill playerSkill1;
    public static Skill playerSkill2;
    public static Skill playerSkill3;
    public static Skill playerSkill4;

    public static void init() {
        monsterSkill1 = new Skill();
        monsterSkill1.name = "怪默认技能";
        monsterSkill1.effect = "";
        monsterSkill1.effectTarget = TARGET;
        monsterSkill1.targetType = CharacterType.PC;
        monsterSkill1.targetNumber = 1;
        monsterSkill1.atkRange = 2;
        //monsterSkill1.damage = 10;
        monsterSkill1.cd = 1;
        monsterSkill1.atkClip = "attack1";

        playerSkill1 = new Skill();
        playerSkill1.name = "默认技能1";
        playerSkill1.effect = "";
        playerSkill1.effectTarget = TARGET;
        playerSkill1.targetType = CharacterType.MONSTER;
        playerSkill1.targetNumber = 1;
        playerSkill1.atkRange = 2;
        //playerSkill1.damage = 100;
        playerSkill1.cd = 1;
        playerSkill1.atkClip = "attack1";


        playerSkill2 = new Skill();
        playerSkill2.name = "默认技能2";
        playerSkill2.effect = "";
        playerSkill2.effectTarget = TARGET;
        playerSkill2.targetType = CharacterType.MONSTER;
        playerSkill2.targetNumber = 1;
        playerSkill2.atkRange = 2;
        //playerSkill1.damage = 100;
        playerSkill2.cd = 1;
        playerSkill2.atkClip = "attack2";


        playerSkill3 = new Skill();
        playerSkill3.name = "默认技能3";
        playerSkill3.effect = "";
        playerSkill3.effectTarget = TARGET;
        playerSkill3.targetType = CharacterType.MONSTER;
        playerSkill3.targetNumber = 1;
        playerSkill3.atkRange = 2;
        //playerSkill1.damage = 100;
        playerSkill3.cd = 1;
        playerSkill3.atkClip = "attack3";

        playerSkill4 = new Skill();
        playerSkill4.name = "buff技能";
        playerSkill4.effect = "GroundFX_Fire01";
        playerSkill4.effectTarget = CASTER;
        playerSkill4.targetType = CharacterType.PC;
        playerSkill4.targetNumber = 1;
        playerSkill4.atkRange = 2;
        //playerSkill1.damage = 100;
        playerSkill4.cd = 1;
        playerSkill4.atkClip = "punch";
        playerSkill4.effectOffSet = new Vector3(0,1f,0);
    }

    /**
     * 这里只负责特效的播放
     */
    public void cast(Character caster, Character target)
    {
        GameObject parent = null;
        if (effectTarget == CASTER) {
            if(caster == null){
                Debug.Log("effect target is CASTER, but caster is null");
                return;
            } else 
                parent = caster.gameObject;
        }
        if (effectTarget == TARGET)
        {
            if(target == null){
                Debug.Log("effect target is TARGET, but TARGET is null");
                return;
            } else 
                parent = target.gameObject;
        }

        Debug.Log("skill atkClip :" + this.atkClip);
        Debug.Log("skill effect :" + this.effect);
        if (this.effect.Equals(""))
            return;

        //WWW.LoadFromCacheOrDownload
        //GameObject go = GameObject.Instantiate(ObjectManager.skillPrefab) as GameObject;
        Debug.Log("skill effect GroundFX_Fire01.Prefab");
        GameObject skillEffect = GameObject.Instantiate(Resources.Load(this.effect)) as GameObject;
        skillEffect.transform.position = parent.transform.position;
        skillEffect.transform.position += this.effectOffSet;
    }
}