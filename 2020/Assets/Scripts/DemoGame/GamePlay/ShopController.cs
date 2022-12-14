using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public static ShopController instance;
    public AudioClip buyCoinsClip;

    [SerializeField]
    private int id, coin;
    [SerializeField]
    private float money;
    [SerializeField]
    private Button btBuy;
    [SerializeField]
    private GameObject coinBought;

    void _MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Awake()
    {
        _MakeInstance();
    }

    void Start()
    {
        transform.GetChild(1).GetComponent<Text>().text = coin + "";
        btBuy.transform.GetChild(0).GetComponent<Text>().text = money + "$";
        btBuy.onClick.AddListener(delegate
        {
            _BuyCoinsInDemo();
        });
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
        PlayerPrefsDemo.instance.audioSource.PlayOneShot(buyCoinsClip);
        GameplayDemo.instance.shop.transform.GetChild(4).gameObject.SetActive(false);
        var item = Instantiate(coinBought, GameplayDemo.instance.startCoinAnimPos.position, Quaternion.identity, GameplayDemo.instance.shop.transform);
        item.transform.GetChild(1).GetComponent<Text>().text = coin + "";
        item.transform.DOScale(new Vector3(96, 96, 96) * 1.5f, .5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        item.transform.GetChild(1).DOScale(new Vector3(0.02f, 0.02f, 1) / 1.5f, .5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        item.transform.GetChild(0).GetComponent<Image>().DOFade(.5f, .5f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(delegate
            {
                item.transform.DOScale(new Vector3(96, 96, 96), .5f).SetEase(Ease.OutBounce).SetUpdate(true);
                item.transform.GetChild(1).DOScale(new Vector3(0.02f, 0.02f, 1), .5f).SetEase(Ease.InOutQuad).SetUpdate(true);
                item.transform.GetComponent<Transform>().DOMove(GameplayDemo.instance.endCoinAnimPos.position, .75f).SetEase(Ease.InOutQuad).SetUpdate(true).SetDelay(.5f);
                item.transform.GetChild(0).GetComponent<Image>().DOFade(0f, .75f).SetEase(Ease.InOutQuad).SetUpdate(true).SetDelay(.5f)
                .OnComplete(delegate 
                {
                    Destroy(item);
                    Time.timeScale = 1;
                    GameplayDemo.instance._UpdateCoin();
                });
            });
    }
}
