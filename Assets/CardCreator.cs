using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class CardCreator : MonoBehaviour
{
    public Transform cameraToMove;
    public UnityEngine.UI.Text positiontext;
    public UnityEngine.UI.Text rotationtext;

    private void Start()
    {
        main = this;
        string json;

        cameraX = cameraToMove.position.x;
        rotation = cameraToMove.rotation.eulerAngles.z;

        //try
        {
            //json = File.ReadAllText(Application.dataPath + "/CardFile.txt");
        }
      //  catch (System.Exception e)
        {
            TextAsset txt = (TextAsset)Resources.Load("CardFile", typeof(TextAsset));
            json = txt.text;
        }
        AllCards newCard = JsonUtility.FromJson<AllCards>(json);
        MyCards = newCard.myCards;

        foreach (Card c in MyCards)
        {
            if (c.CardName == "Events" || c.CardName == "Event")
            {
                c.RulesText = "Events are not Kingdom cards. In a player's Buy phase, when the player"+
"can buy a card, the player can buy an Event instead. Buying an Event"+
"means paying the cost indicated on the Event and then doing the effect"+
"of the Event.The Event just stays on the table, the player does not take"+
"it; there is no way for a player to gain one or end up with one in his"+
"deck. Buying an Event uses up a Buy; normally a player can either buy"+
"a card, or buy an Event.A player with two Buys, such as after playing "+
"Ranger, could buy two cards, or buy two Events, or buy a card and an "+
"Event(in either order).The same Event can be bought multiple times in "+
"a turn if the player has the Buys and available to do it.Some Events "+
"give + Buys and so let the player buy further cards/ Events afterwards. "+
"Players cannot play further Treasures that turn after buying an Event. "+
"Buying an Event is not buying a card and so does not trigger cards like "+
"Swamp Hag or Goons(from Prosperity).Costs of Events are not "+
"affected by cards like Bridge Troll.";
            }

            if (c.CardName == "Landmarks")
            {
                c.RulesText = "Landmarks are not Kingdom cards. It is recommended" +
"that no more than two Landmarks be used per game. "+
"Players may choose how to determine what Landmarks "+
"to play with.They may shuffle them with Events and "+
"deal out 2 cards from that pile every game; they may "+
"shuffle them into the Randomizer deck and use 0 - 2 "+
"depending on how many come up before finding 10 "+
"Kingdom cards; or they may use any method they like.";
            }

            if (c.CardName == "Projects")
            {
                c.RulesText = "Projects are special, permanent, on-buy effects not " +
"attached to cards.Players can buy Projects during their " +
"Buy phase whenever they might instead buy a card or " +
"Event.When a player buys a Project, they put a wooden " +
"cube of their color on it, to track which Projects' effects " +
"they receive. Each player has only two cubes to put on " +
"Projects.";
            }

            if (c.CardName == "Boons")
            {
                c.RulesText = "Boons are a face-down deck of landscape-style " +
"instruction cards that are revealed as needed.Generally " +
"these have effects that are good for a player. " +
"Setup: If any Kingdom cards being used have the Fate " +
"type, shuffle the Boons and put them near the Supply, " +
"and put the Will - o'-Wisp (Spirit) pile near the Supply " +
"also. " +
"Fate cards can somehow give players Boons; all the Fate " +
"type means is that the Boons are shuffled at the start of " +
"the game.";
            }

            if (c.CardName == "Hexes")
            { c.RulesText = "Hexes are a face-down deck of landscape-style " +
"instruction cards that are revealed as needed.Generally " +
"these have effects that are not good for a player. " +
"Setup: If any Kingdom cards being used have the Doom " +
"type, shuffle the Hexes and put them near the Supply, " +
"and put Deluded / Envious(States) and Miserable / Twice " +
"Miserable(States) near the Supply also. " +
"Doom cards can somehow give players Hexes; all the " +
"Doom type means is that the Hexes are shuffled at the " +
"start of the game."; }

            if (c.CardName == "States")
            {
                c.RulesText = "States are deck of landscape-style cards. State cards are a " +
"way of tracking special information about players.It sits " +
"in front of a player as a reminder until it goes away. ";
            }

            //currentCards.Add(CardTemplate.GetComponent<ImageController>().CreateImage(transform, CardTemplate, c, OnLeft, false));
            if (currentCards.Count == 9)
            { ClearCards(); }
        }
    }


    public InputField CardName;
    public InputField CardText;
    public InputField Cost;
    public List<Toggle> toggleList;

    public GameObject CardTemplate;
    public GameObject VerticalTemplate;
    public Transform VerticalTransform;

    public Dropdown ExpansionDropdown;

    public List<Expansion> MyExpansions;
    public List<Card> MyCards;

    public List<GameObject> currentCards = new List<GameObject>();

    public Camera PrinterCamera;
    public static CardCreator main;
    public InputField CardSubText;

    public Toggle PotionCost;
    public InputField DebtCost;

    public GameObject CustomToggleTemplate;
    public Transform CustomSelectGrid;
    List<InputField> CreatedToggles = new List<InputField>();

    public GameObject Blinder;
    public GameObject BlinderB;

    public GameObject ExpansionListTab;
    public GameObject VertExpansionTab;

    public Toggle VerticalToggle;

    float cameraX;
    float rotation;

    public void AddCard()
    {
        int maxCount = VerticalToggle.isOn ? 8 : 9;
        if (currentCards.Count == maxCount)
        { ClearCards(); }
        Card newCard = MyCards.Find(item => item.CardName == CardName.text && item.ExpID == ExpansionDropdown.value + 1);
        if (newCard == null)
        {
            newCard = new Card();
        }

        do
        {
            newCard.ID = Random.Range(0, 100000);
        } while (MyExpansions.Find(item => item.ID == newCard.ID) != null);

        newCard.CardName = CardName.text;
        newCard.RulesText = CardText.text;
        newCard.SubText = CardSubText.text;
        newCard.PotionCost = PotionCost.isOn ? 1 : 0;
        if (DebtCost.text.Length > 0)
        {
            newCard.DebtCost = int.Parse(DebtCost.text);
        }

        if (Cost.text.Length > 0)
        {
            newCard.Cost = int.Parse(Cost.text);
        }
        newCard.myCardTypes.Clear();
        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].isOn)
            {
                newCard.myCardTypes.Add((CardType)i);
            }
        }
        newCard.ExpID = ExpansionDropdown.value + 1;
        MyCards.Add(newCard);
        GameObject Template = VerticalToggle.isOn ? VerticalTemplate : CardTemplate;

        currentCards.Add(Template.GetComponent<ImageController>().CreateImage(VerticalToggle.isOn ? VerticalTransform : transform, Template, newCard, OnLeft, VerticalToggle.isOn));

    }

    public void MoveSlider(System.Single vol)
    {
        cameraToMove.transform.position = new Vector3(cameraX + vol, cameraToMove.position.y, cameraToMove.position.z);
        positiontext.text = "" + vol;
    }

    public void RotateCamera(System.Single vol)
    {
        cameraToMove.transform.rotation = Quaternion.Euler(0,0,vol);
        rotationtext.text = ""+vol;
    }

    public int currentPage = 0;

    public List<Card> ExpansionSpecific = new List<Card>();
    string currentExpName;
    Expansion currentExpansion;

    public void LoadExpansion(Dropdown dd)
    {
        customCardPrint = false;
        currentExpansion = MyExpansions[dd.value];
        currentExpName = MyExpansions[dd.value].ExpName;
        ExpansionSpecific.Clear();
        foreach (Card c in MyCards)
        {
            if (c.ExpID - 1 == dd.value && !ExpansionSpecific.Contains(c))
            {
                ExpansionSpecific.Add(c);
            }
        }

        LoadPage(0);
    }

    public void ClearCards()
    {
        foreach (GameObject obj in currentCards)
        {
            Destroy(obj);
        }

        currentCards.Clear();
    }

    public void LoadPage(int Page)
    {
        currentPage = Page;
        ClearCards();
        GameObject Template = VerticalToggle.isOn ? VerticalTemplate : CardTemplate;
        int maxCount = VerticalToggle.isOn ? 8 : 9;
        int total = 0;
        int TempTotal = 0;
        while (TempTotal <= maxCount - total)
        {
            TempTotal = 0;

            for (int i = Page * maxCount; i < Mathf.Min((1 + Page) * maxCount, ExpansionSpecific.Count); i++)
            {
                currentCards.Add(Template.GetComponent<ImageController>().CreateImage(VerticalToggle.isOn ? VerticalTransform : transform, Template, ExpansionSpecific[i], OnLeft, VerticalToggle.isOn));
                TempTotal++;
            }
            if (total < maxCount && TempTotal < maxCount)
            {
                currentCards.Add(Template.GetComponent<ImageController>().CreateExpansionTab(VerticalToggle.isOn ? VerticalTransform : transform, VerticalToggle.isOn ? VertExpansionTab : ExpansionListTab, currentExpansion, OnLeft));
                TempTotal++;
            }
            total += TempTotal;
            Debug.Log("temp " + TempTotal + " total  " + total + "   max " + maxCount);
        }
        FillEmpty();
    }


    bool OnLeft = true;
    public void Reverse()
    {
        OnLeft = !OnLeft;
        foreach (GameObject obj in currentCards)
        {
            obj.GetComponent<ImageController>().Reverse();
        }
        SetBlinder();
        GetComponent<Image>().enabled = OnLeft;
        if (OnLeft)
        {
            Debug.Log("Right" + cameraToMove.transform.position);
            cameraToMove.transform.position = new Vector3(cameraX + (-1 * float.Parse(positiontext.text)), cameraToMove.position.y, cameraToMove.position.z);
            cameraToMove.rotation = Quaternion.Euler(0, 0, float.Parse(rotationtext.text));
            GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
            GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
            VerticalTransform.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
            VerticalTransform.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
        }
        else
        {
            cameraToMove.transform.position = new Vector3(cameraX + ( float.Parse(positiontext.text)), cameraToMove.position.y, cameraToMove.position.z);
            Debug.Log("Left" + cameraToMove.transform.position);
            cameraToMove.rotation = Quaternion.Euler(0, 0,-1* float.Parse(rotationtext.text));
            GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperRight;
            GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperRight;
            VerticalTransform.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperRight;
            VerticalTransform.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperRight;
        }

    }

    public void DefaultSide()
    {
        OnLeft = true;
        SetBlinder();
        GetComponent<Image>().enabled = OnLeft;
        if (OnLeft)
        {
            GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
            GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
            VerticalTransform.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
            VerticalTransform.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
        }
    }

    public void Print()
    {
        bool vert = VerticalToggle.isOn;
        SetBlinder();

        if (customCardPrint)
        {
            GetComponent<ScreenShotter>().RTImage("CustomPage", "CustomPage" + currentPage + (OnLeft ? "" : "Reversed"), vert);
        }
        else
        {
            GetComponent<ScreenShotter>().RTImage(currentExpName, currentExpName + "P" + currentPage + (OnLeft ? "" : "Reversed"), vert);
        }
    }

    public void SetBlinder()
    {
        if (OnLeft)
        {
            Blinder.SetActive(VerticalToggle.isOn);
            BlinderB.SetActive(false);
        }
        else
        {
            BlinderB.SetActive(VerticalToggle.isOn);
            Blinder.SetActive(false);
        }
    }

    public void Previous()
    {
        DefaultSide();
        currentPage--;
        LoadPage(currentPage);
    }

    public void Next()
    {
        DefaultSide();
        currentPage++;
        LoadPage(currentPage);

    }

    public void PopulateCustomSelect()
    {

        if (CreatedToggles.Count == 0)
        {
            foreach (Card cc in MyCards)
            {
                GameObject obj = Instantiate<GameObject>(CustomToggleTemplate, CustomSelectGrid);
                obj.GetComponentInChildren<Text>().text = cc.CardName + "-" + MyExpansions[cc.ExpID - 1].ExpName;
                obj.name = MyExpansions[cc.ExpID - 1].ExpName + " " + cc.CardName;
                CreatedToggles.Add(obj.GetComponentInChildren<InputField>());
            }
            foreach (Expansion ee in MyExpansions)
            {
                GameObject obj = Instantiate<GameObject>(CustomToggleTemplate, CustomSelectGrid);
                obj.GetComponentInChildren<Text>().text = ee.ExpName;
                CreatedToggles.Add(obj.GetComponentInChildren<InputField>());
            }
        }

    }

    public void FillEmpty()
    {
        GameObject Template = VerticalToggle.isOn ? VerticalTemplate : CardTemplate;

        int maxCount = VerticalToggle.isOn ? 8 : 9;
        int n = maxCount - currentCards.Count;
        for (int j = 0; j < n; j++)
        {
            currentCards.Add(Template.GetComponent<ImageController>().CreateImage(VerticalToggle.isOn ? VerticalTransform : transform, Template, null, OnLeft, VerticalToggle.isOn));

        }
    }

    bool customCardPrint;
    public void SelectCustomCards()
    {
        customCardPrint = true;
        ClearCards();
        GameObject Template = VerticalToggle.isOn ? VerticalTemplate : CardTemplate;

        List<Card> selected = new List<Card>();
        int i = 0;
        foreach (InputField tt in CreatedToggles)
        {
            if (tt.text.Length > 0)
            {
                if (i >= MyCards.Count)
                {
                    for (int j = 0; j <int.Parse(tt.text); j++)
                    {
                        currentCards.Add(Template.GetComponent<ImageController>().CreateExpansionTab(VerticalToggle.isOn ? VerticalTransform : transform, VerticalToggle.isOn ? VertExpansionTab : ExpansionListTab, MyExpansions[i - MyCards.Count], OnLeft));
                    }
                }
                else
                {
                    for (int j = 0; j < int.Parse(tt.text); j++)
                    {
                        currentCards.Add(Template.GetComponent<ImageController>().CreateImage(VerticalToggle.isOn ? VerticalTransform : transform, Template, MyCards[i], OnLeft, VerticalToggle.isOn));
                    }
                }
            }
            i++;
        }
        FillEmpty();
    }
 

    public void Save()
    {
        AllCards newOne = new AllCards();
        newOne.myCards = MyCards;
        string json = JsonUtility.ToJson(newOne);
        File.WriteAllText(Application.dataPath + "/CardFile.txt", json);

    }
}



[System.Serializable]
public class Expansion
{
    public string ExpName;
    public Sprite ExpIcon;
    public int ID;
}

[System.Serializable]
public class AllCards
{
    public List<Card> myCards = new List<Card>();
}

[System.Serializable]
public class Card
{
    public string CardName;
    [TextArea(2,10)]
    public string RulesText;
    public int Cost = -1;
    public int ID;
    public List<CardType> myCardTypes = new List<CardType>();
    public int ExpID;
    public string SubText;
    public int PotionCost;
    public int DebtCost;

}

[System.Serializable]        //1        2     3              4   
public enum CardType{ Action, Treasure, Victory, Reaction, Duration, Curse, Shelter, Night, Attack, Prize, Fate, Doom, Heirloom, Castle, Reserve, Traveller, Gathering }