using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_SpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites; // Arraste seus 10 sprites para este array no Inspector
    public float animationSpeed = 0.2f; // Ajuste a velocidade da anima��o alterando este valor no Inspector
    public TMP_Text subtitle;

    private Image imageComponent;
    private float timer;
    private int currentSpriteIndex = 0;
    private bool isPlaying = false; // Uma vari�vel para controlar se a anima��o est� rodando

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
        if (subtitle.text == "[Sil�ncio]")
        {
            imageComponent.sprite = sprites[12];
            return;
        }
        else
        {
            if (!isPlaying) // Se a anima��o n�o estiver rodando, n�o atualize os frames
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

    // M�todo para pausar a anima��o
    public void PauseAnimation()
    {
        isPlaying = false;
        imageComponent.sprite = sprites[12];
    }

    // M�todo para retomar a anima��o
    public void PlayAnimation()
    {
        isPlaying = true;
    }
}
