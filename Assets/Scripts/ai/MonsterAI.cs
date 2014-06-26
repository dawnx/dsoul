using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * NPC所用AI
 * 
 */
class MonsterAI : AbstractAI 
{
    private Transform myTransform;

    public override void initObj()
    {
        this.character = new Character();
        this.character.ai = this;
        this.character.atk = 1;
        this.character.def = 10;
        this.character.gameObject = gameObject;
        this.character.hp = 300;
        this.character.maxHp = 300;
        this.character.moveSpeed = 3;
        this.character.searchRange = 25;
        this.character.type = CharacterType.MONSTER;
        this.character.bornPostion = gameObject.transform.position;
        this.character.atkSpeed = 3;
        GameObjectManager.characters.Add(this.character);

        gameObject.animation["attack1"].speed = 2;
        gameObject.animation["run"].speed = 2;

        myTransform = gameObject.transform;
        //getBloodBar().setName("monster");
    }

    public override Character getAtkTarget()
    {
        Character target = GameObjectManager.findByRange(myTransform.position, character.searchRange, CharacterType.PC);
        return target;
    }

    public override int getAtkRange()
    {
        return 2;
    }

    public override Skill getSkill()
    {
        return Skill.monsterSkill1;
    }
}