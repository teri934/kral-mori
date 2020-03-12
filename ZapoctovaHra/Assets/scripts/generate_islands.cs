using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_islands : MonoBehaviour
{
	public GameObject island;
    // Start is called before the first frame update
    void Start()
    {
		Chunk chunk = new Chunk(16,16);
		chunk.generate_islands();
		chunk.printChunk();
		for(int y=0;y<chunk.chunk_matrix.Length;y++){
			for(int x=0;x<chunk.chunk_matrix.Length;x++){
				if(chunk.chunk_matrix[y][x]==1){
					Instantiate(island,new Vector3(x*20,0,y*20),Quaternion.identity);
				}
				//Debug.Log(x + " "+  y + " "+ Mathf.PerlinNoise((float)x/16,(float)y/16));
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Chunk
{
	public int[][] chunk_matrix;
	int size;
	
	public Chunk(int x, int y){
		chunk_matrix = new int[y][];
		for(int i = 0;i<y;i++){
			chunk_matrix[i] = new int[x];
		}
		size = chunk_matrix.Length;
	}
	
	public void generate_islands(){
		System.Random rand = new System.Random();
		int seed = rand.Next(0,10000);
		for(int y=0;y<size;y++){
			for(int x=0;x<size;x++){
				chunk_matrix[y][x] = (int)Mathf.Round(Mathf.PerlinNoise(seed+5*(float)x/size,seed+5*(float)y/size)/1.3f);
				//Debug.Log(x + " "+  y + " "+ Mathf.PerlinNoise((float)x/16,(float)y/16));
			}
		}
		
	}
	
	public void printChunk(){
		string matice = System.Environment.NewLine;
		for(int i = 0; i < chunk_matrix.Length;i++){
			for(int j = 0; j < chunk_matrix[0].Length;j++){
				matice = matice + chunk_matrix[i][j] + " ";
			}
			matice += System.Environment.NewLine;
		}
		Debug.Log(matice);
	}
	
}
