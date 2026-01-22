using UnityEngine;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;


public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    //public AudioSource audioSource;
    public MMF_Player JumpFeedback;


    public void OnPointerEnter(PointerEventData eventData)
    {
        /*  if (!audioSource.isPlaying)
             audioSource.Play(); */

        JumpFeedback?.PlayFeedbacks();
    }


}
