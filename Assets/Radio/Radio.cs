using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioController : MonoBehaviour
{
    public AudioSource audioSource;
    public Text songTitleText;  // UI Text สำหรับแสดงชื่อเพลง
    public List<AudioClip> songList; // รายการเพลง
    public float volumeStep = 0.5f; // เพิ่ม/ลดเสียงทีละ 0.5

    private int currentSongIndex = 0;
    private bool isPlaying = false; // เช็คว่าเพลงกำลังเล่นอยู่หรือไม่

    void Start()
    {
        audioSource.volume = 0.05f; // ตั้งค่าเริ่มต้นของเสียง
        songTitleText.text = "Press Z or X to start music"; // ข้อความเริ่มต้น
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // เปลี่ยนเพลงไปทางซ้าย
        {
            PreviousSong();
        }
        else if (Input.GetKeyDown(KeyCode.X)) // เปลี่ยนเพลงไปทางขวา
        {
            NextSong();
        }

        // เลื่อนล้อเมาส์ขึ้น = เพิ่มเสียง, เลื่อนลง = ลดเสียง
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            AdjustVolume(volumeStep);
        }
        else if (scroll < 0f)
        {
            AdjustVolume(-volumeStep);
        }
    }

    void PlaySong(int index)
    {
        if (index >= 0 && index < songList.Count)
        {
            audioSource.clip = songList[index];
            audioSource.Play();
            songTitleText.text = songList[index].name; // แสดงชื่อเพลง
            isPlaying = true;
        }
    }

    void NextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songList.Count;
        PlaySong(currentSongIndex);
    }

    void PreviousSong()
    {
        currentSongIndex = (currentSongIndex - 1 + songList.Count) % songList.Count;
        PlaySong(currentSongIndex);
    }

    void AdjustVolume(float change)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume + change, 0f, 1f);
    }
}
