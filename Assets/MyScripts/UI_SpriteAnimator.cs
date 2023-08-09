using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_SpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites; // Arraste seus 10 sprites para este array no Inspector
    public float animationSpeed = 0.2f; // Ajuste a velocidade da animação alterando este valor no Inspector
    public TMP_Text subtitle;

    private Image imageComponent;
    private float timer;
    private int currentSpriteIndex = 0;
    private bool isPlaying = false; // Uma variável para controlar se a animação está rodando

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        if (sprites.Length > 0)
        {
            imageComponent.sprite = sprites[0];
        }
    }

    private void Update()
    {
        if (subtitle.text == "[Silêncio]")
        {
            imageComponent.sprite = sprites[12];
            return;
        }
        else
        {
            if (!isPlaying) // Se a animação não estiver rodando, não atualize os frames
                return;

            timer += Time.deltaTime;
            if (timer >= animationSpeed)
            {
                timer = 0f;
                currentSpriteIndex++;
                if (currentSpriteIndex >= sprites.Length)
                {
                    currentSpriteIndex = 0;
                }
                imageComponent.sprite = sprites[currentSpriteIndex];
            }
        }  
            

        
    }

    // Método para pausar a animação
    public void PauseAnimation()
    {
        isPlaying = false;
        imageComponent.sprite = sprites[12];
    }

    // Método para retomar a animação
    public void PlayAnimation()
    {
        isPlaying = true;
    }
}
