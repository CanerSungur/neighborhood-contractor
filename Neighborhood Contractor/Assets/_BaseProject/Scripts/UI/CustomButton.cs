using UnityEngine;
using UnityEngine.UI;
using System;
using ZestGames.Utility;

/// <summary>
/// Plays sound and animation first, then executes the action given.
/// Give the action as a parameter of CustomButton's event.
/// </summary>
public class CustomButton : Button
{
    private Animation anim;
    private float animationDuration = 0.2f;
    public event Action<Action> OnClicked;
    private Image _image;

    protected override void OnEnable()
    {
        anim = GetComponent<Animation>();
        _image = GetComponent<Image>();

        OnClicked += Clicked;
    }

    protected override void OnDisable()
    {
        OnClicked -= Clicked;
    }

    public void SetInteractable()
    {
        interactable = true;
        _image.color = new Color(1, 1, 1, 1f);
    }
    public void SetNotInteractable()
    {
        interactable = false;
        _image.color = new Color(1, 1, 1, .5f);
    }

    private void Clicked(Action action)
    {
        // Play audio
        AudioHandler.PlayAudio(AudioHandler.AudioType.Button_Click);

        // Reset and Play the animation
        anim.Rewind();
        anim.Play();

        // Do the action with delay
        Delayer.DoActionAfterDelay(this, animationDuration, () => action());
    }

    public void ClickTrigger(Action action) => OnClicked?.Invoke(action);
}
