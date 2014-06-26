using UnityEngine;
using System.Collections;

public class BloodBar : MonoBehaviour {

    private UIProgressBar progressBar;

    public UILabel nameLabel;

    private GameObject parent;

    private Transform mytransform;
    private float heroHeight;

    public float distance;

    void Start() {
        progressBar = GetComponent<UIProgressBar>();
        mytransform = this.transform;
    }

    public void setParent(GameObject p) {
        this.parent = p;
        heroHeight = 0;
    }

    public void setName(string name) {
        if (nameLabel != null)
            nameLabel.text = name;
    }

    public void Damaged(int _maxhp, int _hp)
    {
        if (_maxhp != 0 && progressBar!= null)
        {
            float precent = 1.0f * _hp / _maxhp;
            if (precent > 1f)
                precent = 1;
            if (precent < 0f)
                precent = 0;
            progressBar.value = precent;
        }
    }

    void Update()
    {
        if (this.parent != null && mytransform != null)
        {
            Transform ptransform = parent.transform;
            Vector3 pos = new Vector3(ptransform.position.x, ptransform.position.y + heroHeight + distance, ptransform.position.z);
            mytransform.position = pos;
            mytransform.rotation = Camera.main.transform.rotation;  
        }
    }
}
