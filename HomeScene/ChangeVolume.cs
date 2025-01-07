using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolume : MonoBehaviour
{
    public Slider volumeSlider;   // Slider điều chỉnh âm lượng
    public AudioSource audioSource;

    void Start()
    {
        // Gắn giá trị mặc định khi khởi chạy
        if (audioSource != null && volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;

            // Đăng ký sự kiện khi giá trị của slider thay đổi
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // Thay đổi âm lượng
    public void SetVolume(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value; // Cập nhật âm lượng của AudioSource
        }
    }
}
