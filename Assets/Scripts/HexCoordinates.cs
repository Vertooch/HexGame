using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
	public int X { get { return x; } }
	public int Z { get { return z; } }
	public int Y { get { return -X - Z; } }

    [SerializeField] private int x, z;

    public HexCoordinates (int x, int z)
    {
		this.x = x;
		this.z = z;
	}

	public static HexCoordinates FromOffsetCoordinates (int x, int z)
    {
		return new HexCoordinates(x, z - x /2);
	}

	public static HexCoordinates FromPosition (Vector3 position)
    {
		float z = position.z / (HexMetrics.innerRadius * 2f);
		float y = -z;

		float offset = position.x / (HexMetrics.outerRadius * 3f);
		z -= offset;
		y -= offset;

		int iZ = Mathf.RoundToInt(z);
		int iY = Mathf.RoundToInt(y);
		int iX = Mathf.RoundToInt(-z -y);

		if (iX + iY + iZ != 0)
        {
			float dZ = Mathf.Abs(z - iZ);
			float dY = Mathf.Abs(y - iY);
			float dX = Mathf.Abs(-z -y - iX);

			if (dX > dY && dX > dZ)
            {
				iX = -iY - iZ;
			}
			else if (dZ > dY)
            {
				iZ = -iX - iY;
			}
		}

		return new HexCoordinates(iX, iZ);
	}

	public override string ToString ()
    {
		return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines ()
    {
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}
}