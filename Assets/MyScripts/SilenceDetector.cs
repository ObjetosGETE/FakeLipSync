using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SilenceDetector : MonoBehaviour
{
    private AudioSource audioSource;

    [Range(0, 0.1f)]
    public float silenceThreshold = 0.01f; // Limiar de amplitude

    private float[] samples = new float[512]; // Array para armazenar as amostras de �udio

    public UnityEvent OnSilenceDetected;    // Evento chamado quando sil�ncio � detectado
    public UnityEvent OnSoundDetected;      // Evento chamado quando o som � detectado

    private bool wasSilent = false;         // A flag para rastrear se o �ltimo estado era silencioso

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.GetOutputData(samples, 0); // Pegue os dados de sa�da do AudioSource

        float average = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            average += Mathf.Abs(samples[i]); // Calcula a m�dia das amplitudes
        }
        average /= samples.Length;

        if (average < silenceThreshold && !wasSilent)
        {
            OnSilenceDetected?.Invoke();
            wasSilent = true;
        }
        else if (average >= silenceThreshold && wasSilent)
        {
            OnSoundDetected?.Invoke();
            wasSilent = false;
        }
    }
}
