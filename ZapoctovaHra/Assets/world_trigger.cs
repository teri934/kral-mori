using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class world_trigger : MonoBehaviour
{
	Chunk[] activeChunks = new Chunk[9];
	int arrayStart = 0;
    // Start is called before the first frame update
    void Start()
    {
		int pointer = 0;
		for(int j = 160; j >= -160; j-=160){
			for(int i = -160; i <= 160; i+=160){
				Chunk chunk = new Chunk(i, j);
				chunk.generateIslands();
				chunk.InstinScene();
				activeChunks[pointer] = chunk;
				pointer += 1;
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerExit(Collider collider){
		if(collider.gameObject.tag == "ship"){
			Vector3 direction = collider.transform.position - transform.position;
			if(Mathf.Abs(direction.z) >= Mathf.Abs(direction.x)){
				//nahore
				if(direction.z > 0){
					transform.Translate(new Vector3(0, 0, 160));
				}
				//dole
				if(direction.z < 0){
					transform.Translate(new Vector3(0, 0, -160));
					activeChunks[arrayStart+0].RemoveIslands();
					activeChunks[arrayStart+1].RemoveIslands();
					activeChunks[arrayStart+2].RemoveIslands();
					
					arrayStart = (arrayStart + 3) % 9;
					int active_y = activeChunks[arrayStart + 1].pos_y - 320;
					int active_x;
					int pointer = 0;
					for(int i = -160;i<=160;i+=160){
						active_x = activeChunks[arrayStart + 1].pos_x + i;
						Chunk chunk = new Chunk(active_x, active_y);
						activeChunks[(arrayStart + 6 + pointer) % 9] = chunk;
						chunk.generateIslands();
						chunk.InstinScene();
						pointer++;
					}
				}
			}
			if(Mathf.Abs(direction.z) <= Mathf.Abs(direction.x)){
				//vlevo 
				if(direction.x > 0){
					transform.Translate(new Vector3(160,0,0));
				}
				//vpravo
				if(direction.x < 0){
					transform.Translate(new Vector3(-160,0,0));
				}
			}
		}
	}
	
}
