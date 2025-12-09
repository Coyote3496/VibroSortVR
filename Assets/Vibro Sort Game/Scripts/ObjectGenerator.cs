using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ObjectCategory {
    Concrete,
    Squishy,
    Chalky,
    Bruisy,
    Alien
}

public class ObjectGenerator : MonoBehaviour
{
    [System.Serializable]
    public class VibroPattern
    {
        public float[] amp;
        public float[] freq;

        public VibroPattern(float[] a, float[] f)
        {
            amp = a;
            freq = f;
        }
    }

    private VibroPattern[] binPatterns;
    private Dictionary<ObjectCategory, int> categoryToBinPosition =
        new Dictionary<ObjectCategory, int>();

    public int numObjsToSpawn = 12;
    public int minPerCat = 1;

    public Scoreboard scoreboard;

    public Material[] categoryMaterials; 
    public GameObject[] bins;     
    public GameObject[] objectPrefabs;   

    private Collider spawnCollider;

    void Start()
    {
        spawnCollider = GetComponent<Collider>();
        binPatterns = new VibroPattern[]
        {
            new VibroPattern( new float[]{ 0.8f, 0.0f },                   new float[]{ 0.5f, 0.0f } ),
            new VibroPattern( new float[]{ 0.8f, 0.0f, 0.8f, 0.0f },             new float[]{ 0.5f, 0.5f, 0.0f } ),
            new VibroPattern( new float[]{ 0.8f, 0.8f, 0.8f, 0.0f },        new float[]{ 0.5f, 0.5f, 0.5f, 0.0f } ),
            new VibroPattern( new float[]{0.3f,0.1f,0.3f,0.1f,0.0f},   new float[]{0.3f,0.1f,0.3f,0.1f,0.0f} ),
            new VibroPattern( new float[]{0.9f,0.9f,0.3f,0.9f,0.0f},   new float[]{1.0f,0.7f,0.2f,1.0f,0.0f} )
        };
    }
    public void AssignBins()
    {
        categoryToBinPosition.Clear();

        // Randomized order of categories (colors)
        List<ObjectCategory> randomizedCategories =
            System.Enum.GetValues(typeof(ObjectCategory))
            .Cast<ObjectCategory>()
            .OrderBy(c => Random.value)
            .ToList();

        for (int pos = 0; pos < bins.Length; pos++)
        {
            ObjectCategory assignedCategory = randomizedCategories[pos];
            bins[pos].GetComponent<Renderer>().material =
                categoryMaterials[(int)assignedCategory];

            BinCheck bc = bins[pos].transform.GetChild(0).GetComponent<BinCheck>();
            bc.binCategory = assignedCategory;

            categoryToBinPosition[assignedCategory] = pos;
        }
    }
    public void InstantiateObjects()
    {
        scoreboard.SetNumObjectsToSort(numObjsToSpawn);
        int left = numObjsToSpawn;

        foreach (ObjectCategory category in System.Enum.GetValues(typeof(ObjectCategory)))
        {
            SpawnObject(category);
            left--;
        }

        while (left > 0)
        {
            ObjectCategory cat = (ObjectCategory)Random.Range(0, 5);
            SpawnObject(cat);
            left--;
        }
    }

    private void SpawnObject(ObjectCategory cat)
    {
        Quaternion rot = Quaternion.Euler(
            Random.Range(0f,360f),
            Random.Range(0f,360f),
            Random.Range(0f,360f));

        Vector3 pos = RandomPointInBounds(spawnCollider.bounds);

        GameObject obj = Instantiate(objectPrefabs[(int)cat], pos, rot);

        SortInteractable si = obj.GetComponent<SortInteractable>();
        si.objectCategory = cat;

        int binPosition = categoryToBinPosition[cat];

        VibroPattern pattern = binPatterns[binPosition];

        HapticInteractable hi = obj.GetComponent<HapticInteractable>();
        hi.hapticBuffer_amp = pattern.amp;
        hi.hapticBuffer_freq = pattern.freq;
    }

    public static Vector3 RandomPointInBounds(Bounds b)
    {
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            Random.Range(b.min.z, b.max.z)
        );
    }
}
