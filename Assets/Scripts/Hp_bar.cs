using UnityEngine;
using System.Collections;

public class Hp_bar : MonoBehaviour {
    private Mesh thismesh; // 当前的材质
    private Vector2[] originUV = new Vector2[4];
    private Transform mytransform; // 当前的transform
    private Transform parentmon; // 
    private float _amount;
    private Vector2 amountU;
    private Vector2 amuontV;
    private float posY;
    private int oldstatus;
    private void Awake()
    {
        this.mytransform = base.transform;
        this.thismesh = base.GetComponent<MeshFilter>().mesh;
        this.originUV = this.thismesh.uv;
        this.amuontV = Vector2.up * 0.25f;
    }
    public void Damaged(int _maxhp, int _hp, Transform _parent, float _height, int _status)
    {
        this.parentmon = _parent;
        if (_maxhp != 0)
        {
            this._amount = (1f - (float)_hp / (float)_maxhp) * 0.5f;
            this.amountU = Vector2.right * this._amount;
            if (_status == -1)
            {
                _status = this.oldstatus;
            }
            this.thismesh.uv = new Vector2[]
			{
				this.originUV[0] + this.amountU + this.amuontV * (float)_status,
				this.originUV[1] + this.amountU + this.amuontV * (float)_status,
				this.originUV[2] + this.amountU + this.amuontV * (float)_status,
				this.originUV[3] + this.amountU + this.amuontV * (float)_status
			};
        }
        this.posY = _height;
        this.oldstatus = _status;
    }
    public void FreeSelect()
    {
        this.mytransform.position = Vector3.one * 4f;
        this.parentmon = null;
    }
    private void Update()
    {
        if (this.parentmon != null)
        {
            this.mytransform.position = this.parentmon.position + new Vector3(0f, this.posY, -0.02f);
        }
    }
}  