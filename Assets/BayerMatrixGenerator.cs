using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BayerMatrixGenerator : MonoBehaviour {

	// 2 x 2 Bayer Matrix, it is used as a base to calculate power of 2 bayer matrices
	public float[] mat = {0,3,2,1};
	public float[] newMat = { };

	public int matrixSize = 4;

	[ContextMenu("Calculate Matrix")]
	public void Calculate()
	{
		int targetSize = matrixSize;
		int currentSize = 2;

		newMat = mat;

		while (targetSize != currentSize) {
			
			newMat = GenerateMatrix (newMat, currentSize, currentSize * 2);
			currentSize *= 2;
		}

		GetComponent<Renderer> ().sharedMaterial.SetFloatArray ("_Array", newMat);
		GetComponent<Renderer> ().sharedMaterial.SetFloat ("_ArrayLength", targetSize*targetSize);
		GetComponent<Renderer> ().sharedMaterial.SetFloat ("_ArraySide", targetSize);
	}

	float[] GenerateMatrix(float[] prevMatrix, int prevSize, int size)
	{
		List<float> newMatrix = new List<float> ();
		for (int i = 0; i < size * size; i++) {
			newMatrix.Add(0);
		}

		for (int j = 0; j < prevSize; j++) 
		{
			for (int i = 0; i < prevSize; i++) 
			{
				float prevValue = prevMatrix[(i%prevSize) + ((j%prevSize) * prevSize)];
				if (i < prevSize && j < prevSize)
				{
					newMatrix [(i + j * size)] = prevValue * 4;
				}
			}
		}


		for (int j = 0; j < size; j++) 
		{
			for (int i = 0; i < size; i++) 
			{
				float prevValue = newMatrix[(i%prevSize) + ((j%prevSize) * size)];
				if (i < prevSize && j < prevSize)
				{
					// ignore this case, for simplicity pf the alghoritm it was pre-calculated during previous iteration
				} 
				else if (i >= prevSize && j < prevSize)
				{
					newMatrix [(i + j * size)] = prevValue + 3;
				}
				else if (j >= prevSize && i < prevSize)
				{
					newMatrix [(i + j * size)] = prevValue + 2;
				}
				else
				{
					newMatrix [(i + j * size)] = prevValue + 1;
				}
			}
		}

		return newMatrix.ToArray();
	}
}
