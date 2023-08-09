using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SilenceDetector : MonoBehaviour
{
    private AudioSource audioSource;

    [Range(0, 0.1f)]
    public float silenceThreshold = 0.01f; // Limiar de amplitude

    private float[] samples = new float[512]; // Array para armazenar as amostras de áudio

    public UnityEvent OnSilenceDetected;    // Evento chamado quando silêncio é detectado
    public UnityEvent OnSoundDetected;      // Evento chamado quando o som é detectado

    private bool wasSilent = false;         // A flag para rastrear se o último estado era silencioso

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.GetOutputData(samples, 0); // Pegue os dados de saída do AudioSource

        float average = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            average += Mathf.Abs(samples[i]); // Calcula a média das amplitudes
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
