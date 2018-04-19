using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TravelMap : MonoBehaviour {
    public static TravelMap instance;
    private List<Town> towns;
    private List<Contract> contracts;

	private List<Transform> locations;
	private CanvasGroup groupComponent;
	public static bool mapShown = true;

    public Transform townParent;
    public Transform contractParent;
    public GameObject townPrefab;
    public GameObject contractPrefab;

    [Range(0f, 1f)]
    public float locationDensity = 0.5f;
    public int width = 100;
    public int height = 100;

	void Start() {
        GenerateTravelMap();
		groupComponent = GetComponent<CanvasGroup> ();
		mapShown = true;
        instance = this;

		Toggle ();

        towns = new List<Town>();
        contracts = new List<Contract>();
    }

	void Update() {
		if (Input.GetKeyDown (KeyCode.M)) {
			Toggle ();
		}
	}

	public void Toggle() {
		if (mapShown) {
			mapShown = false;
			groupComponent.alpha = 0;
			groupComponent.interactable = false;
			groupComponent.blocksRaycasts = false;
		} else {
			mapShown = true;
			groupComponent.alpha = 1;
			groupComponent.interactable = true;
			groupComponent.blocksRaycasts = true;
		}
	}

    public void GenerateTravelMap()
    {
        for(int i = townParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(townParent.GetChild(i).gameObject);
        }

        for (int i = contractParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(contractParent.GetChild(i).gameObject);
        }

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(Random.Range(0f, 100f) < locationDensity)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        GenerateContract(new Vector2(x, y));
                    }
                    else
                    {
                        GenerateCity(new Vector2(x, y));
                    }
                }
            }
        }
    }

    private void GenerateCity(Vector2 location)
    {
        Town town = Instantiate(townPrefab, townParent).GetComponent<Town>();
        RectTransform townRect = town.GetComponent<RectTransform>();
        location.x = (location.x - 50f)/width * townParent.GetComponent<RectTransform>().rect.width;
        location.y = (location.y - 50f)/height * townParent.GetComponent<RectTransform>().rect.height;
        townRect.anchoredPosition = location;

        town.townName = "Town (" + location.x + " | " + location.y + ")";
        town.population = Random.Range(100, 20000);
        town.kingdom = "Human";
        town.prosperity = Town.Prosperity.Sustainable;
        town.location = location;
    }
    
    private void GenerateContract(Vector2 location)
    {
        Contract contract = Instantiate(contractPrefab, contractParent).GetComponent<Contract>();
        RectTransform townRect = contract.GetComponent<RectTransform>();
        location.x = (location.x - 50)/width * townParent.GetComponent<RectTransform>().rect.width;
        location.y = (location.y - 50)/height * townParent.GetComponent<RectTransform>().rect.height;
        townRect.anchoredPosition = location;

        System.Array values = System.Enum.GetValues(typeof(Contract.Tileset));
        System.Random random = new System.Random();

        contract.targetName = "Contract (" + location.x + " | " + location.y + ")";
        contract.payment = Random.Range(500, 5000);
        contract.timeframe = Random.Range(3, 15);
        contract.difficulty = Contract.Difficulty.Regular;
        contract.location = location;
        contract.tileset = (Contract.Tileset)values.GetValue(random.Next(values.Length));
    }

    //Map Generation
    //Editor Tools
#if UNITY_EDITOR
    [CustomEditor(typeof(TravelMap))]
    public class TravelMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            TravelMap myScript = (TravelMap)target;
            if (GUILayout.Button("Generate New Map"))
            {
                myScript.GenerateTravelMap();
            }
        }
    }
#endif
}
