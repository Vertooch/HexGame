using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public int width = 6;
	public int height = 6;

	public Color defaultColor = Color.white;

    public HexMapEditor mapEditor;
	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	HexCell[] cells;

	Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

        // Create grid
		for (int x = 0, i = 0; x < height; x++) {
			for (int z = 0; z < width; z++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start () {
		hexMesh.Triangulate(cells);
	}

	public void ColorCell (Vector3 position, Color color) {
        // Find cell at positoin
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.Z + coordinates.X * height + coordinates.X / 2;
		HexCell cell = cells[index];

        // Update cell color
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

	void CreateCell (int x, int z, int i) {
        // Create position vector
		Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = 0f;
        position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

        // Instantiate cell
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = mapEditor.RandomColor();

        // Set neighbor connections
        if (z > 0)
        {
            cell.SetNeighbor(HexDirection.S, cells[i - 1]);
        }
        if (x > 0)
        {
            if ((x & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.NW, cells[i - height]);
                if (z > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - height - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - height]);
                if (z < height - 1)
                {
                    cell.SetNeighbor(HexDirection.NW, cells[i - height + 1]);
                }
            }
        }

        // Add label
        Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}