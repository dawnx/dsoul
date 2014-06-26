using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour
{

    public GameObject player;//定义一个人物的Transform
    public float yOffset = 30;
    public float zOffset = -30;

    private Transform mytransform;
    private Vector3 targetposition;
    private int movespeed;
    public int dx = 1;
    private Vector3 hit_shake1 = new Vector3(0.3f, 0f, 0.15f);
    private Vector3 hit_shake2 = new Vector3(0f, 3f, 2f);
    private AbstractAI plyaerAI;
    void Awake() {
        mytransform = this.transform;
        plyaerAI = AbstractAI.GetTargetAI(player);
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        movespeed = plyaerAI.character.moveSpeed;

        targetposition = player.transform.position;
        targetposition.y += yOffset;
        targetposition.z += zOffset;

        //transform.position = targetposition;//相机的目标位置,这两句代码的作用是让人物一直处于相机的视野下

        this.mytransform.position = Vector3.MoveTowards(this.transform.position, targetposition, Time.deltaTime * (float)this.movespeed);
    }

    public void Hitcam()
    {
        this.dx = -this.dx;
        this.mytransform.position += this.hit_shake1 * (float)this.dx;
    }
    public void Hitcam2(float _factor)
    {
        this.mytransform.position += this.hit_shake2 * _factor;
    }
}