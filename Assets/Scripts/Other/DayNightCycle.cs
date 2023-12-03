using UnityEngine;

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    [Header("Day/Night cycle")]
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private int day;
    [SerializeField] private float timeSpeed;

    [Header("References")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        day = 0;
    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying && gameManager.HasGameStarted())
        {
            float lastTimeOfDay = TimeOfDay;

            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime * timeSpeed;
            TimeOfDay %= 24; // Clamp between 0-24

            if (lastTimeOfDay <= 24f && lastTimeOfDay > 12f && TimeOfDay >= 0f && TimeOfDay < 12f) day++;

            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    public float GetTime() { return TimeOfDay; }

    public int GetDay() { return day; }
}