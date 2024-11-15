using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PlayVideoAndLoadScene : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector
    public string sceneToLoad; // Name of the scene to load

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished; // Subscribe to event
        videoPlayer.Play(); // Start playing the video
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneToLoad); // Load next scene when video finishes
    }
}
