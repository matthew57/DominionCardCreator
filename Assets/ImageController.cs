using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageController : MonoBehaviour {

    public Text CardNameA;
    public Text CostA;
    public Image BannerBackgroundA;
    public Image ExpansionA;
    public Image CoinImageA;
    public Image HalfBannerA;
    public GameObject SideA;
    public Text subtextA;
    public Image PotionA;
    public Image DebtA;
    public Text DebtTExtA;

    public Text CardNameB;
    public Text CostB;
    public Image BannerBackgroundB;
    public Image ExpansionB;
    public Image CoinImageB;
    public GameObject SideB;
    public Image HalfBannerB;
    public Text subtextB;
    public Image PotionB;
    public Image DebtB;
    public Text DebtTExtB;

    public Text CardType;
    public Text rulesText;
    public GameObject BigGold;
    public GameObject DebtIcon;
    public GameObject PotionIcon;
    public Card myCard;

    //public Sprite Potion;


    public GameObject CreateExpansionTab(Transform parent, GameObject prefab, Expansion exp, bool reverse)
    {
        GameObject obj = GameObject.Instantiate<GameObject>(prefab, parent);
        ImageController controller = obj.GetComponent<ImageController>();

        controller.ExpansionA.sprite = exp.ExpIcon;
        controller.CardNameA.text = exp.ExpName;

        string cardList = "";
        List<string> sorter = new List<string>();


        foreach(Card cc in CardCreator.main.MyCards)
        {
            if (cc.ExpID == exp.ID)
            {
                sorter.Add(cc.CardName);
                //cardList += ", " + cc.CardName;
            }

        }
        sorter.Sort();
        foreach (string s in sorter)
        {
            cardList += ", " +s;
        }
            cardList = cardList.Substring(1);

        controller.rulesText.text = cardList;

        return obj;
    }

    bool FirstOne = true;
    bool isVertical;
    public GameObject CreateImage(Transform parent, GameObject prefab, Card newCard, bool reverse, bool isVertical)
    {
        FirstOne = true;
        // Debug.Log("Adding " + newCard.CardName);
        GameObject obj = GameObject.Instantiate<GameObject>(prefab, parent);
        ImageController controller = obj.GetComponent<ImageController>();
        controller.isVertical = isVertical;
        if (newCard == null)
        {
            controller.CardNameA.text = "";
            controller.CardNameB.text ="";
            controller.subtextA.text = "";
            controller.subtextB.text = "";
            controller.rulesText.text = "";
            controller.CostA.text = "";
            controller.CostB.text = "";
            controller.CardType.text = "";
            return obj;

        }
        obj.name = "Card " + newCard.CardName;
        controller.CardNameA.text = newCard.CardName;
        controller.CardNameB.text = newCard.CardName;
        controller.subtextA.text = newCard.SubText;
        controller.subtextB.text = newCard.SubText;
        controller.myCard = newCard;
        if (newCard.Cost != -1)
        {
            controller.CostA.text = newCard.Cost + "";
            controller.CostB.text = newCard.Cost + "";
        }
        else
        {
            controller.CoinImageA.enabled = false;
            controller.CoinImageB.enabled = false;
            controller.CostA.text = "";
            controller.CostB.text = "";
        }

        if (newCard.DebtCost > 0)
        {
            controller.DebtA.enabled = true;
            controller.DebtB.enabled = true;
            controller.DebtTExtA.text = newCard.DebtCost + "";
            controller.DebtTExtB.text = newCard.DebtCost + "";
        }

        controller.rulesText.text = newCard.RulesText;

        controller.formatMoney();


        controller.ExpansionA.sprite = CardCreator.main.MyExpansions[newCard.ExpID-1].ExpIcon;
        controller.ExpansionB.sprite = CardCreator.main.MyExpansions[newCard.ExpID -1].ExpIcon;

        if (newCard.PotionCost > 0)
        {
            Debug.Log("Cost is " + newCard.PotionCost);
            controller.PotionA.gameObject.SetActive(true);
            controller.PotionB.gameObject.SetActive(true);
        }


        string cardTypeString = "";

        for (int i = 0; i < newCard.myCardTypes.Count; i++)
        {
            cardTypeString += newCard.myCardTypes[i];
            if (i < newCard.myCardTypes.Count - 1)
            {
                cardTypeString += " - ";
            }
        }


        controller.CardType.text = cardTypeString;
        

        if (newCard.myCardTypes.Contains(global::CardType.Reaction))
        {
            PaintBanner(controller, new Color(.6f, .6f, 1), true);
            if (newCard.myCardTypes.Contains(global::CardType.Treasure))
            {
                PaintBanner(controller, Color.yellow, false);
            }
            else if (newCard.myCardTypes.Contains(global::CardType.Victory))
            {
                PaintBanner(controller, new Color(.5f, 1, .5f), false);
            }
        }
        else if (newCard.myCardTypes.Contains(global::CardType.Treasure))
        {

            PaintBanner(controller, Color.yellow, true);
            if (newCard.myCardTypes.Contains(global::CardType.Action))
            {
                PaintBanner(controller, Color.white, false);
            }
            else if (newCard.myCardTypes.Contains(global::CardType.Victory))
            {
                PaintBanner(controller, new Color(.5f, 1, .5f), false);
            }
        }
        else if (newCard.myCardTypes.Contains(global::CardType.Victory))
        {
            PaintBanner(controller, new Color(.5f, 1, .5f), true);
            if (newCard.myCardTypes.Contains(global::CardType.Action))
            {
                PaintBanner(controller, Color.white, false);
            }
        }
        
        else if (newCard.myCardTypes.Contains(global::CardType.Curse))
        {
            PaintBanner(controller, new Color(0.99f, .4009f, 0.8758712f), true);
        }

        else if (newCard.myCardTypes.Contains(global::CardType.Night))
        {
            PaintBanner(controller, new Color(.4f, .4f, 0.4f), true);
            controller.CardNameA.color = Color.white;
            controller.CardNameB.color = Color.white;
            controller.subtextA.color = Color.white;
            controller.subtextB.color = Color.white;
        }

        if (newCard.myCardTypes.Contains(global::CardType.Attack))
        {
            PaintBanner(controller, new Color(1, .25f, 0.25f), FirstOne);
        }

        if (newCard.myCardTypes.Contains(global::CardType.Duration))
        {
            PaintBanner(controller, new Color(1, 0.65f, 0), FirstOne);
        }

        if (newCard.myCardTypes.Contains(global::CardType.Traveller))
        {
            PaintBanner(controller, new Color(1, .9f, .9f), FirstOne);
        }
        if (newCard.myCardTypes.Contains(global::CardType.Reserve))
        {
            PaintBanner(controller, new Color(1, .6f, .5f), FirstOne);
        }


        if (reverse)
        {
             controller.SideA.SetActive(reverse);
             controller.SideB.SetActive(!reverse);
        }
        return obj;
    }

    public void PaintBanner(ImageController controller, Color c, bool firstType)
    {
        if (firstType)
        {
            controller.BannerBackgroundA.color = c;
            controller.BannerBackgroundB.color = c;
            FirstOne = false;
        }
        else
        {
            controller.HalfBannerA.enabled = true;
            controller.HalfBannerB.enabled = true;
            controller.HalfBannerA.color = c;
            controller.HalfBannerB.color = c;
        }
    }

    public void formatMoney()
    {
        List<int> indexes = new List<int>();
        List<int> DebtIndexes = new List<int>();
        List<int> PotionIndexes = new List<int>();

        for (int i = 0; i < rulesText.text.Length; i++)
        {
            if (rulesText.text[i] == '$')
            {

                rulesText.text = rulesText.text.Insert(i + 2, " ");
            }

            else if (rulesText.text[i] == '&')
            {
                rulesText.text = rulesText.text.Insert(i + 2, " ");
            }
            else if (rulesText.text[i] == '@')
            {
                rulesText.text = rulesText.text.Insert(i, " ");
                i++;
            }
        }



        for (int i = 0; i < rulesText.text.Length; i++)
        {
            if (rulesText.text[i] == '$')
            {
                indexes.Add(i + indexes.Count + DebtIndexes.Count + PotionIndexes.Count + 2);
            }
            if (rulesText.text[i] == '&')
            {
                DebtIndexes.Add(i + indexes.Count + DebtIndexes.Count + PotionIndexes.Count + 2);
            }
            if (rulesText.text[i] == '@')
            {
                PotionIndexes.Add(i + indexes.Count + DebtIndexes.Count + PotionIndexes.Count + 2);
            }
        }

        StartCoroutine(secondWait(indexes, DebtIndexes, PotionIndexes));
        string c = rulesText.text;

        c = c.Replace("$", "  ");
        c = c.Replace("&", "  ");
        c = c.Replace("@", "  ");

        rulesText.text = c;
    }

   

    IEnumerator secondWait(List<int> indexes, List<int> debtIndexes, List<int> potions)
    {
        yield return null;
        TextGenerator tg = rulesText.cachedTextGenerator;

        for (int i = 0; i < indexes.Count; i++)
        {

                AddGold(tg.characters[indexes[i]].cursorPos - (isVertical ? new Vector2(322,572): new Vector2(423, 446)));           
        }

        for (int i = 0; i < debtIndexes.Count; i++)
        {
            AddDebt(tg.characters[debtIndexes[i]].cursorPos - (isVertical ? new Vector2(365, 572) : new Vector2(465, 444)));
        }

        for (int i = 0; i < potions.Count; i++)
        {
            AddPotion(tg.characters[potions[i]].cursorPos - (isVertical ? new Vector2(322, 572) : new Vector2(465, 444)));
        }
    }

    public void AddGold(Vector2 position)
    {

        GameObject obj = Instantiate<GameObject>(BigGold,  transform);
 
        obj.transform.localPosition = obj.transform.worldToLocalMatrix * position;

        obj.transform.parent = rulesText.transform.parent;
        obj.transform.SetSiblingIndex(0);

    }

    void AddDebt(Vector2 position)
    {
        GameObject obj = Instantiate<GameObject>(DebtIcon, transform);

        obj.transform.localPosition = obj.transform.worldToLocalMatrix * position;

        obj.transform.parent = rulesText.transform.parent;
        obj.transform.SetSiblingIndex(0);

    }

    void AddPotion(Vector2 position)
    {
        GameObject obj = Instantiate<GameObject>(PotionIcon, transform);

        obj.transform.localPosition = obj.transform.worldToLocalMatrix * position;

        obj.transform.parent = rulesText.transform.parent;
        obj.transform.SetSiblingIndex(0);

    }

    public void Reverse()
    {
        if (SideA && SideB)
        {
            SideA.SetActive(!SideA.activeSelf);
            SideB.SetActive(!SideA.activeSelf);
        }
    }

}
