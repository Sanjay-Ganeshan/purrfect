using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewer : MonoBehaviour {

    private class InventoryDisplayEntry
    {
        public Image image;
        public InventoryItem item;
        public RectTransform trans;
        public InventoryDisplayEntry setImage(Image image)
        {
            this.image = image;
            return this;
        }
        public InventoryDisplayEntry setItem(InventoryItem item)
        {
            this.item = item;
            return this;
        }
        public InventoryDisplayEntry setTransform(RectTransform trans)
        {
            this.trans = trans;
            return this;
        }
    }

    public int NumRows, NumColumns;


    public GameObject ImagePrefab;
    public Text NameText;
    public Text DescriptionText;
    public RectTransform InventoryPictureArea;

    /*
    private InventoryItem[] displayedInventoryItems;
    private Image[] images;
    private RectTransform[] childTransforms;
    */

    private InventoryDisplayEntry[] displayedItems;

    public Sprite defaultSprite;

    private bool initialized = false;

	// Use this for initialization
	void Start () {
        InitIfNeeded();
	}
	
	// Update is called once per frame
	void Update () {
        // Update will only be called if the game object that this script is attached to is active.
        bool selectingItem = false;
        Vector3 screenSpaceMouse = Input.mousePosition;
        foreach(InventoryDisplayEntry item in this.displayedItems)
        {
            RectTransform t = item.trans;
            if(t.rect.Contains(t.InverseTransformPoint(screenSpaceMouse)))
            {
                InventoryItem hoveredItem = item.item;
                if (hoveredItem != null)
                {
                    NameText.text = hoveredItem.Name;
                    DescriptionText.text = hoveredItem.Description;
                    selectingItem = true;
                    break;
                }
            }
        }
        if(!selectingItem)
        {
            NameText.text = "";
            DescriptionText.text = "";
        }

    }

    private void SetNameAndDescription(string Name, string Description)
    {
        NameText.text = Name;
        DescriptionText.text = Description;
    }

    private void LayoutImages()
    {
        /*
        this.images = new Image[NumRows * NumColumns];
        this.childTransforms = new RectTransform[NumRows * NumColumns];
        this.displayedInventoryItems = new InventoryItem[NumRows * NumColumns];
        */
        this.displayedItems = new InventoryDisplayEntry[NumRows * NumColumns];
        float normWidth = 1.0f / NumColumns;
        float normHeight = 1.0f / NumRows;
        Vector2 firstAnchor = new Vector2(0f, 1f - normHeight);
        Vector2 addPerRow = new Vector2(0, -normHeight);
        Vector2 addPerColumn = new Vector2(normWidth, 0);
        Vector2 minToMax = new Vector2(normWidth, normHeight);
        int currentImage = 0;
        for (int rowNumber = 0; rowNumber < NumRows; rowNumber++)
        {
            for(int columnNumber = 0; columnNumber < NumColumns; columnNumber++)
            {
                this.displayedItems[currentImage] = new InventoryDisplayEntry();
                GameObject child = GameObject.Instantiate<GameObject>(ImagePrefab, InventoryPictureArea);
                Image childImg = child.GetComponent<Image>();
                RectTransform childTransform = child.GetComponent<RectTransform>();
                Vector2 anchorMin = firstAnchor + rowNumber * addPerRow + columnNumber * addPerColumn;
                Vector2 anchorMax = anchorMin + minToMax;
                
                childTransform.anchorMax = anchorMax;
                childTransform.anchorMin = anchorMin;
                this.displayedItems[currentImage++].setTransform(childTransform).setImage(childImg);
                childImg.sprite = defaultSprite;
            }
        }
    }

    private void InitializeMe()
    {
        LayoutImages();
        initialized = true;
    }

    private void InitIfNeeded()
    {
        if (!initialized) InitializeMe();
    }

    public void ShowInventory(Inventory inven)
    {
        InitIfNeeded();
        int currIndex = 0;
        foreach(InventoryItem item in inven)
        {
            if(currIndex >= this.displayedItems.Length)
            {
                break;
            }
            this.displayedItems[currIndex++].setItem(item).image.sprite = item.GetSpriteOrDefault(defaultSprite);
        }
        while(currIndex < this.displayedItems.Length)
        {
            this.displayedItems[currIndex++].setItem(null).image.sprite = defaultSprite;
        }
    }
}
