using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpectrumAnalyzer : MonoBehaviour
{
    public GameObject cubePrefab;

    AudioSource source;
    GameObject[] cubes;

    public float SpectrumRefreshTime;
    private float lastUpdate = 0;
    private float[] spectrum64 = new float[64];
    private float[] spectrum128 = new float[128];
    private float[] spectrum256 = new float[256];
    private float[] spectrum512 = new float[512];
    private float[] spectrum1024 = new float[1024];
    private float[] spectrum2048 = new float[2048];
    private float[] spectrum4096 = new float[4096];

    public float[] customSample = new float[256];

    public float scaleFactor = 1;
    public float maxValue;

    public Material newMaterialRef;


    void Start()
    {
        // foreach (var device in Microphone.devices)
        // {
        //     Debug.Log("Name: " + device);
        // }

        source = GetComponent<AudioSource>();
        // source.clip = Microphone.Start("MacBook Pro Microphone", true, 10, 44100);
        // source.Play();

        cubes = new GameObject[256];
        createDisplayObjects();
                
    }

    void Update()
    {
        if (Time.time - lastUpdate > SpectrumRefreshTime)
        {
            source.GetSpectrumData(spectrum64, 0, FFTWindow.BlackmanHarris);
            source.GetSpectrumData(spectrum128, 0, FFTWindow.BlackmanHarris);
            source.GetSpectrumData(spectrum256, 0, FFTWindow.BlackmanHarris);
            source.GetSpectrumData(spectrum512, 0, FFTWindow.BlackmanHarris);
            source.GetSpectrumData(spectrum1024, 0, FFTWindow.BlackmanHarris);
            source.GetSpectrumData(spectrum2048, 0, FFTWindow.BlackmanHarris);
            source.GetSpectrumData(spectrum4096, 0, FFTWindow.BlackmanHarris);

            for (int index = 0; index < 32; index++)
            { //  32 * 8 = 256
                customSample[index + 0] = spectrum4096[index + 0];
                customSample[index + 32] = spectrum4096[index + 32];
                customSample[index + 64] = spectrum2048[index + 32];
                customSample[index + 96] = spectrum1024[index + 32];
                customSample[index + 128] = spectrum512[index + 32];
                customSample[index + 160] = spectrum256[index + 32];
                customSample[index + 192] = spectrum128[index + 32];
                customSample[index + 224] = spectrum64[index + 32];
            }

            for (int i = 0; i < customSample.Length; i++)
            {
                cubes[i].transform.localScale = new Vector3(1, customSample[i] * scaleFactor + 0.25f, 1);
                if (maxValue < customSample[i]) {
                    maxValue = customSample[i];
                }
            }
            lastUpdate = Time.time;
        }
    }

    void createDisplayObjects()
    {
        for (int i = 0; i < 256; i++)
        {
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject cube = Instantiate(cubePrefab);
            cube.transform.position = new Vector3(i-128, 0, 0);
            cube.GetComponent<Renderer>().material = newMaterialRef;

            cubes[i] = cube;
        }
    }
}