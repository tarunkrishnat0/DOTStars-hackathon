using TMPro;
using UnityEngine;
using Unity.Profiling;

public class StatsAndExitCtrl : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private TMP_Text _showHideStatsText;
    [SerializeField] private TMP_Text _robotsCount;
    [SerializeField] private EnergySystemAndRobotSpawnCtrl _spawnCtrl;

    private double deltaTime = 0;
    private int currentFPS = 0;
    private int averageFPS = 0;

    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private int bestFPS = 0, worstFPS = 0;

    FrameTiming[] m_FrameTimings = new FrameTiming[60];

    private void Awake()
    {
        frameDeltaTimeArray = new float[60];
        lastFrameIndex = 0;
    }

    private void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.unscaledDeltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        if (Time.frameCount % 3 != 0)
        {
            return;
        }

        currentFPS = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
        averageFPS = Mathf.RoundToInt(GetFPS());

        if(Time.frameCount > 200)
        {
            if (worstFPS > currentFPS)
            {
                worstFPS = currentFPS;
            }

            if (bestFPS < currentFPS)
            {
                bestFPS = currentFPS;
            }
        }

        // _fpsText.text = $"FPS : cur {currentFPS.ToString()} | avg {averageFPS.ToString()} | <color=#005500>{bestFPS.ToString()}</color>,<color=\"red\">{worstFPS.ToString()}</color>";
        _fpsText.text = $"FPS : {averageFPS.ToString()} | <color=#005500>{bestFPS.ToString()}</color>,<color=\"red\">{worstFPS.ToString()}</color>";

        if (Time.frameCount == 200)
        {
            worstFPS = int.MaxValue;
            bestFPS = 0;
        }

        FrameTimingManager.CaptureFrameTimings();
        var ret = FrameTimingManager.GetLatestTimings((uint)m_FrameTimings.Length, m_FrameTimings);
        if (ret > 0)
        {
            //var bottleneck = DetermineBottleneck(m_FrameTimings[0]);
            // Your code logic here
            _fpsText.text += SetStatsInUI();
        }

        _robotsCount.text = "Alive Robots: " + _spawnCtrl.GetRobotsCount().ToString();
    }

    public void OnClickOfExit()
    {
        Application.Quit();
    }

    public void ToggleStatsDisplay()
    {
        bool nextState = !gameObject.activeInHierarchy;
        gameObject.SetActive(nextState);
        _showHideStatsText.text = nextState == false ? "Show Stats" : "Hide Stats";
    }

    private string SetStatsInUI()
    {
        double cpuFrameTime = 0f;
        double cpuMainThreadFrameTime = 0f;
        double cpuRenderThreadFrameTime = 0f ;
        double gpuFrameTime = 0f;

        for(int i = 0; i < m_FrameTimings.Length; i++)
        {
            cpuFrameTime += m_FrameTimings[i].cpuFrameTime;
            cpuMainThreadFrameTime += m_FrameTimings[i].cpuMainThreadFrameTime;
            cpuRenderThreadFrameTime += m_FrameTimings[i].cpuRenderThreadFrameTime;
            gpuFrameTime += m_FrameTimings[i].gpuFrameTime;
        }
        cpuFrameTime /= m_FrameTimings.Length;
        cpuMainThreadFrameTime /= m_FrameTimings.Length;
        cpuRenderThreadFrameTime /= m_FrameTimings.Length;
        gpuFrameTime /= m_FrameTimings.Length;

        return
            $"\nCPU: {cpuFrameTime:00.00}" +
            $"\nMain Thread: {cpuMainThreadFrameTime:00.00}" +
            $"\nRender Thread: {cpuRenderThreadFrameTime:00.00}" +
            $"\nGPU: {gpuFrameTime:00.00}";
    }

    private float GetFPS()
    {
        float total = 0f;
        foreach (float deltatime in frameDeltaTimeArray)
        {
            total += deltatime;
        }
        return frameDeltaTimeArray.Length / total;
    }
}
