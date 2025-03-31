using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioController : MonoBehaviour
{
    public AudioSource audioSource;
    public Text songTitleText;  // UI Text ����Ѻ�ʴ������ŧ
    public List<AudioClip> songList; // ��¡���ŧ
    public float volumeStep = 0.5f; // ����/Ŵ���§���� 0.5

    private int currentSongIndex = 0;
    private bool isPlaying = false; // ������ŧ���ѧ��������������

    void Start()
    {
        audioSource.volume = 0.05f; // ��駤��������鹢ͧ���§
        songTitleText.text = "Press Z or X to start music"; // ��ͤ����������
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // ����¹�ŧ价ҧ����
        {
            PreviousSong();
        }
        else if (Input.GetKeyDown(KeyCode.X)) // ����¹�ŧ价ҧ���
        {
            NextSong();
        }

        // ����͹���������� = �������§, ����͹ŧ = Ŵ���§
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
            songTitleText.text = songList[index].name; // �ʴ������ŧ
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
