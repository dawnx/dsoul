using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * NPC所用AI
 * 
 */
class NpcAi : AbstractAI 
{
    public override void initObj()
    {
        this.character = new Character();
        this.character.ai = this;
        this.character.atk = 100;
        this.character.def = 10;
        this.character.gameObject = gameObject;
        this.character.hp = 300;
        this.character.maxHp = 300;
        this.character.moveSpeed = 5;
        this.character.searchRange = 50;
        this.character.type = CharacterType.NPC;

        GameObjectManager.characters.Add(this.character);

        gameObject.animation["attack1"].speed = 2;
        gameObject.animation["run"].speed = 2;
    }

    public override Character getAtkTarget()
    {
        Character target = GameObjectManager.findByRange(this.transform.position, character.searchRange, CharacterType.PC);
        return target;
    }

    public override int getAtkRange()
    {

        return 4;
    }
}