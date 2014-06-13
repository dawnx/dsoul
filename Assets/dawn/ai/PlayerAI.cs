using UnityEngine;
using System;

/**
 * ��ҵ�AI
 */
public class PlayerAI : AbstractAI {

    public float moveSpeed = 1;//����һ�������Transform

    public override void initObj()
    {
		this.character = new Character();
        this.character.ai = this;
        this.character.atk = 100;
        this.character.def = 10;
        this.character.gameObject = gameObject;
        this.character.hp = 300;
        this.character.maxHp = 300;
        this.character.moveSpeed = 15;
        this.character.searchRange = 50;
        this.character.type = CharacterType.PC;

		GameObjectManager.characters.Add(this.character);
        //gameObject.animation["hit"].speed = 0.25f;
	}

    void Update() {

        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            updateDist();
        } else if (currentState == RUN) {

            // �Զ��ҹִ�
            atkTarget = getAtkTarget();
            if (atkTarget != null)
                currentState = ATK;
        }
    }

    void updateDist()
    {
        Vector3 cursorScreenPosition = Input.mousePosition;//�������Ļ�ϵ�λ��
        Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);//��������ڵ���Ļλ�÷���һ������
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Terrain")
            {
                var endposition = hit.point;
                //endposition.y += 4.6f;// ģ�͵����ĵ�����⣬(ģ�͵ĸ߶� * scale /2)

                setDist(endposition);
                atkTarget = null;
            }
        }
    }

    public override int getAtkRange()
    {
        return 4;
    }

    public override Character getAtkTarget()
    {
        if (this.transform == null)
            Debug.Log("this.transform is null");

        if (character == null) { // WHY???

            Debug.Log("character is null");
			return null;
		}
        Character target = GameObjectManager.findByRange(this.transform.position, character.searchRange, CharacterType.NPC);
        if (target!=null && !isInAttackArea(target.gameObject, getAtkRange()))
            return null;
        return target;
    }

}

 