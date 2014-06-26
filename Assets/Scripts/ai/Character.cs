using UnityEngine;
using System.Collections;

/**
 * wtf
 */
public class Character {

    public int type;
    public GameObject gameObject;
    public int hp;
    public int maxHp;
    public int atk; // 攻击力
    public int def; // 防御力
    public int moveSpeed;
    public int searchRange;
    public AbstractAI ai;
    public Vector3 bornPostion;
    public float atkSpeed;
}
