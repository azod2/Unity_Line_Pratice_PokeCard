using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//在編輯模式執行
[ExecuteInEditMode]
public class CardSystem : MonoBehaviour
{
    /// <summary>
    /// 撲克牌 :所有撲克牌 52張
    /// </summary>
    public List<GameObject> cards = new List<GameObject>();

    /// <summary>
    /// 花色: 黑桃、方塊、愛心、梅花
    /// </summary>
    private string[] type = { "Spades", "Diamond", "Heart", "Club" };

    private void Start()
    {
        GetAllCard();
    }

    /// <summary>
    /// 取得所有撲克牌
    /// </summary>
    private void GetAllCard()
    {
        if (cards.Count ==52)
        {
            return;
        }
        //跑花色
        for (int i = 0; i < type.Length; i++)
        {
            //跑數字
            for (int j = 1; j < 14; j++)
            {
                string number = j.ToString();                       //數字=j.轉字串
                
                switch (j)
                {
                    case 1:
                        number = "A";                                      //數字1改為A

                        break;
                    case 11:
                        number = "J";                                       //數字11改為J
                        break;
                    case 12:
                        number = "Q";                                       //數字12改為Q
                        break;
                    case 13:
                        number = "K";                                       //數字13改為K
                        break;
                }

                //卡牌 = 素材.載入<遊戲物件>("素材名稱")
                GameObject card =  Resources.Load<GameObject>("PlayingCards_" + number + type[i]);
                //撲克牌.添加(卡牌)
                cards.Add(card);
            }

        }
    }

    /// <summary>
    /// 透過花色選取卡牌
    /// </summary>
    /// <param name="type"></param>
    public void ChooseCardByType(string type)
    {
        DeleteChild();                                                                        //刪除所有子物件

        //暫存排組 = 撲克牌.哪裡((物件) => 物件.名稱.包含(花色))
        var temp = cards.Where((x) => x.name.Contains(type));

        foreach (var item in temp)
        {
            Instantiate(item, transform);
        }

        StartCoroutine(SetChildPosition());

    }

    /// <summary>
    /// 洗牌
    /// </summary>
    public void Shuffle()
    {
        DeleteChild();
        //參考型別ref - ToArray() 轉為陣列實質型別 ToList()轉回清單
        List<GameObject> shuffle = cards.ToArray().ToList();                        //另存一份洗牌用原始排組
        List < GameObject > newCards= new List<GameObject>();              //儲存洗牌後的新牌組

        for (int i = 0; i < 52; i++)
        {
        int r = Random.Range(0, shuffle.Count);                                             //從洗牌用牌組隨機挑一張

        GameObject temp = shuffle[r];                                                             //挑出來的隨機卡牌
        newCards.Add(temp);                                                                          //添加到新牌組
        shuffle.RemoveAt(r);                                                                            //刪除挑出來的排
        }


        foreach (var item in newCards)
        {
            Instantiate(item, transform);
        }

        StartCoroutine(SetChildPosition());
    }


    public void Sort()
    {
        DeleteChild();
        var sort = from card in cards
                   where card.name.Contains(type[3]) || card.name.Contains(type[2]) || card.name.Contains(type[1]) || card.name.Contains(type[0])
                   select card;

        foreach (var item in sort)
        {
            Instantiate(item, transform);
        }
        StartCoroutine(SetChildPosition());

    }


    /// <summary>
    /// 刪除所有子物件
    /// </summary>
    private void DeleteChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator SetChildPosition()
    {
        yield return new WaitForSeconds(0.1f);                                  //避免刪除這次的卡牌

        for (int i = 0; i < transform.childCount; i++)                              //迴圈執行每一個子物件
        {
            Transform child = transform.GetChild(i);                            //取得子物件
            child.eulerAngles = new Vector3(180, 0, 0);                      //設定角度
            child.localScale=Vector3.one *20;                                       //設定尺寸

            //x = i % 13 每13張都從1開始
            //(i-6)*間距
            float x = i % 13;
            //y = i / 13 取得每一排的高度
            //4 - y * 間距
            float y = i  /  13;
            child.position = new Vector3((x - 6) *1.3f, 4-y*2, 0);                    

            yield return null;
        }
    }

}
