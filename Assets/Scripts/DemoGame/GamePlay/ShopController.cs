using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private int id, coin;
    [SerializeField]
    private float money;
    [SerializeField]
    private Button btBuy;
    [SerializeField]
    private GameObject coinAnim;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = coin + "";
        btBuy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = money + "$";
        btBuy.onClick.AddListener(delegate
        {
            _BuyCoinsInDemo();
        });
        coinAnim.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = coin + "";
    }

    public void _BuyCoinsInDemo()
    {
        PlayerPrefsDemo.instance.audioSource.PlayOneShot(GameplayDemo.instance.clickButtonClip);
        string idSkull = "knil_coins_" + coin;
        //int coins = 0;
        //switch (id)
        //{
        //    case 0:
        //        idSkull = "tengame_coins_199";
        //        coins = 100;
        //        break;
        //    case 1:
        //        idSkull = "tengame_coins_999";
        //        coins = 500;
        //        break;
        //    case 2:
        //        idSkull = "tengame_coins_1499";
        //        coins = 1000;
        //        break;
        //    case 3:
        //        idSkull = "tengame_coins_2999";
        //        coins = 2500;
        //        break;
        //    case 4:
        //        idSkull = "tengame_coins_4999";
        //        coins = 5000;
        //        break;
        //    case 5:
        //        idSkull = "tengame_coins_9999";
        //        coins = 10000;
        //        break;
        //    default:
        //        idSkull = "";
        //        coins = 0;
        //        break;
        //}
        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            // phần thưởng trong gói mà user đã mua
            PlayerPrefsDemo.instance._SetCoinsInPossession(coin, true);
            _BuySuccessfully();
        });
        ACEPlay.Bridge.BridgeController.instance.PurchaseProduct(idSkull, e);
    }
    
    void _BuySuccessfully()
    {
        GameplayDemo.instance.shop.transform.GetChild(4).gameObject.SetActive(false);
        var item = Instantiate(coinAnim, new Vector3(-120, 0, 0) / 96, Quaternion.identity, GameplayDemo.instance.shop.transform);

        item.transform.GetChild(0).GetComponent<Image>().DOFade(.5f, .5f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(delegate
            {
                item.transform.GetComponent<Transform>().DOMove(new Vector3(-68, 844, 0) / 96, 1f).SetEase(Ease.InOutQuad).SetUpdate(true).SetDelay(1);
                item.transform.GetChild(0).GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.InOutQuad).SetUpdate(true).SetDelay(1)
                .OnComplete(delegate 
                {
                    Destroy(item);
                    Time.timeScale = 1;
                    GameplayDemo.instance._UpdateCoin();
                });
            });
    }
}
