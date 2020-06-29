using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public Image icon;
    public Text name;
    public Text cost;

    private UIShop shopRef;
    private EntitiesDatabaseSO.EntityData myData;

    public void Setup(EntitiesDatabaseSO.EntityData myData, UIShop shopRef)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();

        this.myData = myData;
        this.shopRef = shopRef;
    }

    public void OnClick()
    {
        //Tell the shop!
        shopRef.OnCardClick(this, myData);
    }
}
