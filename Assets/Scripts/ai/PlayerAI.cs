using UnityEngine;
using System;

/**
 * ��ҵ�AI
 */
public class PlayerAI : AbstractAI {

    public float moveSpeed = 1;//����һ�������Transform

    public Skill currentSkill;
    public Skill nextSkill;

    public override void initObj()
    {
		this.character = new Character();
        this.character.ai = this;
        this.character.atk = 100;
        this.character.def = 10;
        this.character.gameObject = gameObject;
        this.character.hp = 3000;
        this.character.maxHp = 3000;
        this.character.moveSpeed = 10;
        this.character.searchRange = 25;
        this.character.type = CharacterType.PC;
        this.character.atkSpeed = 1;

		GameObjectManager.characters.Add(this.character);
        //gameObject.animation["hit"].speed = 0.25f;
        gameObject.animation["run"].speed = 1.75f;
        getBloodBar().setName("player");
	}

    void Update() {

        base.Update();

        if (IsDied())
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            updateDist(Input.mousePosition);
        } 
        else if (isTouch())
        {
        }
        else if (currentState == RUN) {
			
            // �Զ��ҹִ�
            atkTarget = getAtkTarget();
            if (atkTarget != null && !atkTarget.ai.IsDied())
                currentState = ATK;
        }
    }

    bool isTouch(){
        //need check
        foreach(Touch t in Input.touches){
            if(t.phase == TouchPhase.Canceled){
                updateDist(new Vector3(t.position.x, t.position.y, 0));
                return true;
            }
        }

        return false;
    }

    void updateDist(Vector3 screenPos)
    {
        //Vector3 cursorScreenPosition = Input.mousePosition;//�������Ļ�ϵ�λ��
        Ray ray = Camera.main.ScreenPointToRay(screenPos);//��������ڵ���Ļλ�÷���һ������
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            String tag = hit.collider.gameObject.tag;
            Debug.Log("click tag is:" + tag + "hoverred object is :" + (UICamera.hoveredObject == null));
            if (tag == "Terrain" && UICamera.hoveredObject == null)
            {
                var endposition = hit.point;
                //endposition.y += 4.6f;// ģ�͵����ĵ�����⣬(ģ�͵ĸ߶� * scale /2)

                setDist(endposition);
                atkTarget = null;
            }
            else if (tag == "Enemy")
            {
                AbstractAI ai = GetTargetAI(hit.collider.gameObject);
                if (ai != null)
                {
                    atkTarget = ai.character;
                    this.currentState = ATK;
                }
            }
        }
    }

    public override int getAtkRange()
    {
        return 2;
    }

    public override Character getAtkTarget()
    {
        if (this.transform == null)
            Debug.Log("this.transform is null");

        if (character == null) { // WHY???

            Debug.Log("character is null");
			return null;
		}
        Character target = GameObjectManager.findByRange(this.transform.position, character.searchRange, CharacterType.MONSTER);
        if (target!=null && !isInAttackArea(target.gameObject, getAtkRange()))
            return null;
        return target;
    }

    public void OnSkill1Click(){
        Debug.Log("skill1 click");
    }

    public void OnSkill2Click()
    {
        nextSkill = Skill.playerSkill4;
        Debug.Log("skill2 click");
    }

    public void OnSkill3Click()
    {
        Debug.Log("skill3 click");
    }

    public void OnSkill4Click()
    {
        Debug.Log("skill4 click");
    }

    public override bool PlayClip(string clip)
    {
        bool ret = base.PlayClip(clip);

        return true;
    }

    public override Skill getSkill() {

        if (currentSkill == Skill.playerSkill1)
            currentSkill = Skill.playerSkill2;
        else if (currentSkill == Skill.playerSkill2)
            currentSkill = Skill.playerSkill3;
        else if (currentSkill == Skill.playerSkill3)
            currentSkill = Skill.playerSkill1;
        else
            currentSkill = Skill.playerSkill1;

        if (nextSkill != null)
        {
            currentSkill = nextSkill;
            nextSkill = null;
        }
        return currentSkill;
    }
}

 