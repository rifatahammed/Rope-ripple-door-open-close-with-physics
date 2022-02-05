using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{   
    [SerializeField]
    GameObject partPrefab, parentObject;

    [SerializeField]
    [Range(1,100)]
    int length = 1;

    [SerializeField]
    float partDistance = 0.21f;

    [SerializeField]
    boot reset,spawn,snapFirst,snapLast;

    // Start is called before the first frame update
    // void Start()
    // {
    //     //
    // }

    // Update is called once per frame
    void Update()
    {
        if(reset)
        {
            foreach (GameObject tmp in GameObject.FindGameObjetsWithTag("Player"))
            {
                Destroy(tmp);
            }

            reset = false;
        }

        if(spawn)
        {
            Spawn();
            spawn = false;
        }
    }

    public void Spawn()
    {
        int count = (int)(length / partDistance);

        for(int x = 0; x < count; x++)
        {
            GameObject tmp;
            tmp = Instantiate(partPrefab, new Vector3(transform.position.x,transform.position.y + partDistance * (x+1) ,transform.position.z), Quaternion.identity,parentObject.transform );

            tmp.transform.eularAngles = new Vector3(180,0,0);


            tmp.name = parentObject.transform.childCount.ToString();
            if(x==0)
            {
                Destroy(tmp.GetComponent<CharacterJoint>());
            }
            else{
                tmp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find(parentObject.transform.childCount -1).ToString().GetComponent<Rigidbody>();
            }



        }
    }

}
    
