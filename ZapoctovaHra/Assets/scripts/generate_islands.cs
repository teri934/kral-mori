using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class generate_islands : MonoBehaviour
{

	// Start is called before the first frame update
	void Start()
    {
		
		//Chunk chunk = new Chunk(16,16);
		//chunk.generateIslands();
		//chunk.printChunk();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

public class Chunk : ScriptableObject
{
	public int[][] chunk_matrix;
	int size;
	int[] value_array = new int[4] { 4, 8, 2, 1 };
	List<GameObject> existingIslands = new List<GameObject>();
	public int pos_x;
	public int pos_y;
	
	static System.Random rand = new System.Random();
	
	private GameObject new_island;
	int type_of_island;
	Tuple<GameObject, int> parameters;
	
	static GameObject isolated = Resources.Load("prefabs/isolated") as GameObject;
	static GameObject hill = Resources.Load("prefabs/hill") as GameObject;
	static GameObject L = Resources.Load("prefabs/L") as GameObject;
	static GameObject one_line = Resources.Load("prefabs/shore") as GameObject;
	static GameObject two_lines = Resources.Load("prefabs/bridge") as GameObject;
	static GameObject full = Resources.Load("prefabs/inside") as GameObject;
	
	
	static Tuple<GameObject, int>[] models = new Tuple<GameObject, int>[16]{
		new Tuple<GameObject, int>(isolated, 0),
		new Tuple<GameObject, int>(hill, 270),
		new Tuple<GameObject, int>(hill, 0),
		new Tuple<GameObject, int>(L, 90),
		new Tuple<GameObject, int>(hill, 90),
		new Tuple<GameObject, int>(two_lines, 0),
		new Tuple<GameObject, int>(L, 180),
		new Tuple<GameObject, int>(one_line, 270),
		new Tuple<GameObject, int>(hill, 180),
		new Tuple<GameObject, int>(L, 0),
		new Tuple<GameObject, int>(two_lines, 90),
		new Tuple<GameObject, int>(one_line, 180),
		new Tuple<GameObject, int>(L, 270),
		new Tuple<GameObject, int>(one_line, 90),
		new Tuple<GameObject, int>(one_line, 0),
		new Tuple<GameObject, int>(full, 0)
	};
	
	//inits matrix of given size
	public Chunk(int pos_x, int pos_y){
		this.pos_x = pos_x;
		this.pos_y = pos_y;
		chunk_matrix = new int[16][];
		for(int i = 0;i<16;i++){
			chunk_matrix[i] = new int[16];
		}
		size = chunk_matrix.Length;
	}
	
	public void InstinScene(){	
	
	for(int y=0;y<chunk_matrix.Length;y++){
			for(int x=0;x<chunk_matrix.Length;x++){
				if(chunk_matrix[y][x]==1)
				{
					type_of_island = TypeOfIsland(y, x);
					parameters = models[type_of_island];
					new_island = Instantiate(parameters.Item1,new Vector3(pos_x + x*10,0,pos_y + y*10),Quaternion.identity);
					existingIslands.Add(new_island);
					new_island.GetComponent<Transform>().rotation = Quaternion.Euler(0f, (float) parameters.Item2, 0f);
				}
				//Debug.Log(x + " "+  y + " "+ Mathf.PerlinNoise((float)x/16,(float)y/16));
			}
		}
	}
	
	public void RemoveIslands(){
		foreach (GameObject island in existingIslands){
			Destroy(island);
		}
	
	}
	
	//fills matrix with random islands
	public void generateIslands(){
		int seed = rand.Next(0,10000);
		for(int y=0;y<size;y++){
			for(int x=0;x<size;x++){
				chunk_matrix[y][x] = (int)Mathf.Round(Mathf.PerlinNoise(seed+5*(float)x/size,seed+5*(float)y/size)/1.3f);
				//Debug.Log(x + " "+  y + " "+ Mathf.PerlinNoise((float)x/16,(float)y/16));
			}
		}
		
	}
	//prints matrix to Debug.Log
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

	public int TypeOfIsland(int x, int y)
	{
		int value_of_island = 0;
		int pointer = 0;
		for (int dx = -1; dx < 2; dx++)
		{
			for (int dy = -1; dy < 2; dy++)
			{
				if (Mathf.Abs(dx) + Mathf.Abs(dy) == 1)
				{
					if ((x + dx < chunk_matrix.Length) && (y + dy < chunk_matrix.Length) && (x + dx > -1) && (y + dy > -1))
					{
						if (chunk_matrix[x + dx][y + dy] == 1)
						{
							value_of_island += value_array[pointer];
						}
					}
				pointer += 1;
				}
			}
		}
		return value_of_island;
	}
	
}
