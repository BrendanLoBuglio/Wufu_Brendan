using UnityEngine;
using System.Collections;

public class CellManager : MonoBehaviour {
	public GameObject cellPrefab;
	public int cellGridColumns = 10;
	public int cellGridRows = 10;
	public float cellAlpha = 0.1f;
	
	public GameObject[,] cells;

	private float cellWidth;
	void Start () 
	{
		cellWidth = (cellPrefab.renderer.bounds.max.x - cellPrefab.collider.bounds.min.x) * 2f;		
		InstantiateCells(cellGridColumns, cellGridRows);
	}
	
	void InstantiateCells(int rows, int columns)
	{
		cells = new GameObject[rows,columns];
		
		Vector3 startingPoint = transform.position - new Vector3(0.5f*rows*cellWidth,0 , 0.5f*columns*cellWidth);
		for(int x = 0; x < columns; x++)
		{
			for(int y = 0; y < rows; y++)
			{
				Vector3 nextCellPosition = new Vector3(startingPoint.x+x*cellWidth, startingPoint.y, startingPoint.z+y*cellWidth);
				cells[x,y] = (GameObject)Instantiate(cellPrefab, nextCellPosition, Quaternion.identity);
				cells[x,y].renderer.material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f), cellAlpha);
			}
		}
	}
}
