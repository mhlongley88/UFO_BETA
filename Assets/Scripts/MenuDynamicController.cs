using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.Events;

public class MenuDynamicController : MonoBehaviour
{
	public enum SelectableType
	{
		TOGGLE,
		BUTTON,
		SLIDER
	}
	
	[System.Serializable]
	public class MenuItem
	{
		public SelectableType type;
		public Selectable selectable;
		public bool forceSliderInteger = false;
		
		[HideInInspector]
		public Transform transform;
		[HideInInspector]
		public Vector3 originalScale;
	}
		
	public float scalingSpeed = 4.0f;
	public float increaseOnScale = 0.2f;
    public MenuItem[] items;

    int index = 0;
    float changeRate = 0.0f;

	float horizontalRateSlider = 0.0f;

	public UnityEvent onEnable = new UnityEvent();
	public UnityEvent onDisable = new UnityEvent();

	// Start is called before the first frame update
	void Start()
    {
		if(items != null)
		{
			for (int i = 0; i < items.Length; i++)
			{
				MenuItem item = items[i];
				
				item.transform = item.selectable.gameObject.transform;
				item.originalScale = item.transform.localScale;
			}
		}
    }

	private void OnEnable()
	{
		onEnable.Invoke();

	}

	private void OnDisable()
	{
		onDisable.Invoke();
	}

	// Update is called once per frame
	void Update()
    {
        if (!PlayVideoWhileIdle.playingIdleVideo)
        {
            for (int i = 0; i < 4; i++)
            {
                var rewirePlayer = ReInput.players.GetPlayer(i);

				if (items != null && items.Length > 0)
				{
					if (changeRate < Time.time)
					{
						if (rewirePlayer.GetAxis("Vertical") > 0.1f)
						{
							//Debug.Log("HJEY!");
							index--;
							changeRate = Time.time + 0.35f;
						}
						else if (rewirePlayer.GetAxis("Vertical") < -0.1f)
						{
							index++;
							changeRate = Time.time + 0.35f;
						}

						if (index < 0) index = items.Length - 1;
						else if (index >= items.Length) index = 0;
					}

					//items[index].onClick.Invoke();
					{
						MenuItem highightedItemLocal = items[index];
						switch (highightedItemLocal.type)
						{
							case SelectableType.BUTTON:
								{
									if (rewirePlayer.GetButtonDown("Submit"))
									{
										var button = (Button)highightedItemLocal.selectable;
										button.onClick.Invoke();
									}
								}
								break;
							case SelectableType.SLIDER:
								{
									var axis = rewirePlayer.GetAxis("Horizontal");
									var slider = (Slider)highightedItemLocal.selectable;

									if (!highightedItemLocal.forceSliderInteger)
										slider.value += axis * Time.deltaTime * 1.5f;
									else
									{
										if (horizontalRateSlider < Time.time && axis != 0.0f)
										{
											slider.value += axis > 0 ? 1 : (axis < 0 ? -1 : 0);
											horizontalRateSlider = Time.time + 0.3f;
										}
									}
								}
								break;
							case SelectableType.TOGGLE:
								if (rewirePlayer.GetButtonDown("Submit"))
								{
									var toggle = (Toggle)highightedItemLocal.selectable;
									toggle.isOn = !toggle.isOn;
								}
								break;
						}

					}
				}

                //if (goToMenuWhenBack != null && rewirePlayer.GetButtonDown("Back"))
                //{
                    //gameObject.SetActive(false);
                    //goToMenuWhenBack.gameObject.SetActive(true);
                    //FocusMenu(goToMenuWhenBack);
                //}
            }
        }

		if (items != null && items.Length > 0)
		{
			for (int i = 0; i < items.Length; i++)
			{
				MenuItem item = items[i];
				if (i != index)
					item.transform.localScale = Vector3.Lerp(item.transform.localScale, item.originalScale, Time.deltaTime * scalingSpeed);
			}

			MenuItem highightedItem = items[index];
			highightedItem.transform.localScale = Vector3.Lerp(highightedItem.transform.localScale, highightedItem.originalScale + Vector3.one * increaseOnScale, Time.deltaTime * scalingSpeed);
		}
    }
	
#if UNITY_EDITOR 
	void OnValidate()
	{
		if(items != null)
		{
			for (int i = 0; i < items.Length; i++)
			{
				MenuItem item = items[i];
				
				if(item.selectable != null)
				{
					var itemType = item.selectable.GetType();
					
					if(itemType == typeof(Button)) item.type = SelectableType.BUTTON;
					if(itemType == typeof(Slider)) item.type = SelectableType.SLIDER;
					if(itemType == typeof(Toggle)) item.type = SelectableType.TOGGLE;
				}
			}
		}
	}
#endif

}