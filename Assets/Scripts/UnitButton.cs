using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour {
    public UnitData unitData;  
    public Image unitImageComponent;  

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void Setup(UnitData data) {
        unitData = data;

        if (unitData.unitImage != null && unitImageComponent != null) {
            unitImageComponent.sprite = unitData.unitImage;
            unitImageComponent.preserveAspect = true;  
            AdjustSizeToImage();
        } else {
            Debug.LogWarning("Unit image or Image component missing on " + gameObject.name);
        }
    }

    private void AdjustSizeToImage() {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null && unitImageComponent.sprite != null) {
            float spriteWidth = unitImageComponent.sprite.rect.width / unitImageComponent.sprite.pixelsPerUnit;
            float spriteHeight = unitImageComponent.sprite.rect.height / unitImageComponent.sprite.pixelsPerUnit;
            rectTransform.sizeDelta = new Vector2(spriteWidth, spriteHeight);
        }
    }

    private void OnButtonClick() {
        bool purchaseSuccessful = ShopManager.Instance.BuyUnit(unitData);

        if (purchaseSuccessful) {
            gameObject.SetActive(false); 
        }
    }
}