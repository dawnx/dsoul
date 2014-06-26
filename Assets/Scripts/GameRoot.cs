using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameRoot : MonoBehaviour {

    public GameObject bloodBar;
    public UIFont font;
    public GameObject damageNumPanel;
    public GameObject bloodPanel;
    public GameObject skillPrefab;
	void Awake() {
        ObjectManager.bloodBar = bloodBar;
        ObjectManager.font = font;
        ObjectManager.damageNumPanel = damageNumPanel;
        ObjectManager.bloodPanel = bloodPanel;
        ObjectManager.skillPrefab = skillPrefab;
        Skill.init();
	}


    public void Reborn()
    {
        HashSet<Character> cs = GameObjectManager.characters;

        foreach (Character c in cs)
        {
            if (c.type == CharacterType.MONSTER && c.hp <= 0)
            {
                c.ai.Reborn();
            }
        }

    }
}
