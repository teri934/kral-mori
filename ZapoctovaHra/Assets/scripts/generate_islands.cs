using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class generate_islands : MonoBehaviour
{
	private GameObject new_island;
	int type_of_island;
	Tuple<GameObject, int> parameters;
	public GameObject isolated;
	public GameObject hill;
	public GameObject L;
	public GameObject one_line;
	public GameObject two_lines;
	public GameObject full;

	// Start is called before the first frame update
	void Start()
    {
		List<Tuple<GameObject, int>> models = CreateModel();
		Chunk chunk = new Chunk(16,16);
		chunk.generateIslands();
		chunk.printChunk();
		for(int y=0;y<chunk.chunk_matrix.Length;y++){
			for(int x=0;x<chunk.chunk_matrix.Length;x++){
				if(chunk.chunk_matrix[y][x]==1)
				{
					type_of_island = chunk.TypeOfIsland(y, x);
					parameters = models[type_of_island];
					new_island = Instantiate(parameters.Item1,new Vector3(x*10,0,y*10),Quaternion.identity);
					new_island.GetComponent<Transform>().rotation = Quaternion.Euler(0f, (float) parameters.Item2, 0f);
				}
				//Debug.Log(x + " "+  y + " "+ Mathf.PerlinNoise((float)x/16,(float)y/16));
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public List<Tuple<GameObject, int>> CreateModel()
	{
		List<Tuple<GameObject, int>> models = new List<Tuple<GameObject, int>>();


		models.Add(new Tuple<GameObject, int>(isolated, 0));
		models.Add(new Tuple<GameObject, int>(hill, 270));
		models.Add(new Tuple<GameObject, int>(hill, 0));
		models.Add(new Tuple<GameObject, int>(L, 90));
		models.Add(new Tuple<GameObject, int>(hill, 90));
		models.Add(new Tuple<GameObject, int>(two_lines, 0));
		models.Add(new Tuple<GameObject, int>(L, 180));
		models.Add(new Tuple<GameObject, int>(one_line, 270));
		models.Add(new Tuple<GameObject, int>(hill, 180));
		models.Add(new Tuple<GameObject, int>(L, 0));
		models.Add(new Tuple<GameObject, int>(two_lines, 90));
		models.Add(new Tuple<GameObject, int>(one_line, 180));
		models.Add(new Tuple<GameObject, int>(L, 270));
		models.Add(new Tuple<GameObject, int>(one_line, 90));
		models.Add(new Tuple<GameObject, int>(one_line, 0));
		models.Add(new Tuple<GameObject, int>(full, 0));

		return models;
	}
}

public class Chunk
{
	public int[][] chunk_matrix;
	int size;
	int[] value_array = new int[4] { 4, 8, 2, 1 };
	


	public Chunk(int x, int y){
		chunk_matrix = new int[y][];
		for(int i = 0;i<y;i++){
			chunk_matrix[i] = new int[x];
		}
		size = chunk_matrix.Length;
	}
	
	public void generateIslands(){
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
