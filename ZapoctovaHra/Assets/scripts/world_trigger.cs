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
				//chunk.generateIslands();
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
				//up
				if(direction.z > 0){
					transform.Translate(new Vector3(0, 0, 160));
					activeChunks[(arrayStart + 6) % 9].RemoveIslands();
					activeChunks[(arrayStart + 7) % 9].RemoveIslands();
					activeChunks[(arrayStart + 8) % 9].RemoveIslands();

					arrayStart = (arrayStart + 6) % 9;
					int active_y = activeChunks[(arrayStart + 3) % 9].pos_y + 160;
					int active_x;
					int pointer_activeChunks = 0;
					for (int i = -160; i <= 160; i += 160)
					{
						active_x = activeChunks[arrayStart + 1].pos_x + i;
						Chunk chunk = new Chunk(active_x, active_y);
						activeChunks[(arrayStart + pointer_activeChunks) % 9] = chunk;
						chunk.InstinScene();
						pointer_activeChunks++;
					}
				}
				//down
				if(direction.z < 0){
					transform.Translate(new Vector3(0, 0, -160));
					activeChunks[(arrayStart+0) % 9].RemoveIslands();
					activeChunks[(arrayStart + 1) % 9].RemoveIslands();
					activeChunks[(arrayStart + 2) % 9].RemoveIslands();
					
					arrayStart = (arrayStart + 3) % 9;
					int active_y = activeChunks[arrayStart + 0].pos_y - 320;
					int active_x;
					int pointer_activeChunks = 0;
					for(int i = -160; i <= 160; i += 160){
						active_x = activeChunks[arrayStart + 1].pos_x + i;
						Chunk chunk = new Chunk(active_x, active_y);
						activeChunks[(arrayStart + 6 + pointer_activeChunks) % 9] = chunk;
						chunk.InstinScene();
						pointer_activeChunks++;
					}
				}
			}
			if(Mathf.Abs(direction.z) <= Mathf.Abs(direction.x)){
				//left
				if(direction.x < 0){
					transform.Translate(new Vector3(-160,0,0));
					activeChunks[(arrayStart + 2) % 9].RemoveIslands();
					activeChunks[(arrayStart + 5) % 9].RemoveIslands();
					activeChunks[(arrayStart + 8) % 9].RemoveIslands();

					activeChunks[(arrayStart + 2) % 9] = activeChunks[(arrayStart + 1) % 9];
					activeChunks[(arrayStart + 5) % 9] = activeChunks[(arrayStart + 4) % 9];
					activeChunks[(arrayStart + 8) % 9] = activeChunks[(arrayStart + 7) % 9];

					activeChunks[(arrayStart + 1) % 9] = activeChunks[(arrayStart + 0) % 9];
					activeChunks[(arrayStart + 4) % 9] = activeChunks[(arrayStart + 3) % 9];
					activeChunks[(arrayStart + 7) % 9] = activeChunks[(arrayStart + 6) % 9];

					int active_y;
					int active_x = activeChunks[arrayStart].pos_x - 160;
					int pointer_coordinates = 0;
					for (int i = 0; i <= 6; i += 3)
					{
						active_y = activeChunks[arrayStart + 1].pos_y + pointer_coordinates;
						Chunk chunk = new Chunk(active_x, active_y);
						activeChunks[(arrayStart + i) % 9] = chunk;
						chunk.InstinScene();
						pointer_coordinates -= 160;
					}
				}
				//right
				if(direction.x > 0){
					transform.Translate(new Vector3(160,0,0));
					activeChunks[(arrayStart + 0) % 9].RemoveIslands();
					activeChunks[(arrayStart + 3) % 9].RemoveIslands();
					activeChunks[(arrayStart + 6) % 9].RemoveIslands();

					activeChunks[(arrayStart + 0) % 9] = activeChunks[(arrayStart + 1) % 9];
					activeChunks[(arrayStart + 3) % 9] = activeChunks[(arrayStart + 4) % 9];
					activeChunks[(arrayStart + 6) % 9] = activeChunks[(arrayStart + 7) % 9];

					activeChunks[(arrayStart + 1) % 9] = activeChunks[(arrayStart + 2) % 9];
					activeChunks[(arrayStart + 4) % 9] = activeChunks[(arrayStart + 5) % 9];
					activeChunks[(arrayStart + 7) % 9] = activeChunks[(arrayStart + 8) % 9];


					int active_y;
					int active_x = activeChunks[arrayStart].pos_x + 320;
					int pointer_coordinates = 0;
					for (int i = 2; i <= 8; i += 3)
					{
						active_y = activeChunks[arrayStart + 1].pos_y + pointer_coordinates;
						Chunk chunk = new Chunk(active_x, active_y);
						activeChunks[(arrayStart + i) % 9] = chunk;
						chunk.InstinScene();
						pointer_coordinates -= 160;
					}
				}
			}
		}
	}
	
}
