using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicVolumeControl : MonoBehaviour
{
    public static MusicVolumeControl instance;
    public AudioMixer audioMixer;
    public Slider musicSlider;
    private AudioSource audioSource;

    private const string MUSIC_VOLUME_PARAM = "MusicVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // ตรวจสอบและตั้งค่า AudioSource
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // เล่นเพลงถ้ายังไม่เล่น
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
                audioSource.Play();
            }
        }
        else
        {
            Destroy(gameObject); // ลบทิ้งถ้ามีอยู่แล้ว
            return;
        }
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, 0.75f);
        musicSlider.value = savedVolume;
        SetMusicVolume(savedVolume);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float volume = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, volume);

        PlayerPrefs.SetFloat(MUSIC_VOLUME_PARAM, sliderValue);
        PlayerPrefs.Save();
    }
}