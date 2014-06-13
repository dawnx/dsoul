using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour
{

    public GameObject player;//定义一个人物的Transform
    public GameObject damageNumPanel;
    public UIFont normalFont;
    public float yOffset = 30;
    public float zOffset = -30;

    void Awake() {
        ScriptEF.damageNumPanel = damageNumPanel;
        ScriptEF.font = normalFont;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 targetposition = player.transform.position;
        targetposition.y += yOffset;
        targetposition.z += zOffset;

        transform.position = targetposition;//相机的目标位置,这两句代码的作用是让人物一直处于相机的视野下
    }
}