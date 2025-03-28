// GameManager.cs는 게임을 진행하는 코드
// 카드의 정보를 불러와서, 카드의 정답을 확인하는 등의 게임의 전반을 담당

using System.Collections; 
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 변수 선언: 게임을 진행하는데 필요한 다양한 정보와 기능을 변수로 만들어서 Unity의 Inspecter에서 연결해주어야 코드가 정상 작동한다. 
    // Unity에 연결하지 않고 코드만 작성되어있으면, Unity에서 작동하지 않는다. 

    public static GameManager Instance { get; private set; } // GameManager.cs를 다른 코드에서 Instance선언만으로 자유롭게 불러오도록 하기 위해서 선언
    private void Awake() // GameManager.cs 중복 선언되지 않도록 하기 위해
    {
        if (Instance == null)
        {
            Instance = this;
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Card Information")] // Header을 선언하면 변수들의 제목을 달아주는것으로 변수의 의미를 알기 쉬워진다. 
    public GameObject cardPrefab;   // cardPrefab을 연결
    public Transform cardParent;    // cardPrefab을 생성할 부모 오브젝트를 저장하는 변수
    public List<CardData> cardDataList; // 카드의 정보를 저장한 리스트를 가져옴. CardData.cs에서 리스트를 가져옴
    public Sprite backImageSprite;      // 카드 뒷면 이미지 저장. 카드 뒷면의 이미지는 모두 같음.

    [Header("InGame Cards")]
    private List<Card> cards = new List<Card>(); // 각 라운드에 생성된 카드의 정보를 저장, 정답 카드가 사라지면 리스트가 갱신됨, <Card>는 Card.cs를 가져오는 것을 말함
    private Card firstCard, secondCard; // 플레이어가 선택한 첫번째, 두번째 카드를 선택
    private int currentRound = 1; // 현재 라운드를 저장하는 변수, 1>1라운드, 2>2라운드

    [Header("UI")]
    public GameObject introPanel;   // 인트로UI
    public Button startGameButton;  // 게임 시작 버튼
    public GameObject round1Panel;  // 1라운드 시작 안내 UI
    public Button nextGameButton;   // 다음 라운드 버튼
    public GameObject round2Panel;  // 2라운드 시작 안내 UI
    public GameObject GameOverPanel;// 게임 종료 UI

    [Header("Time Score")]
    public TMP_Text GameOverText;   // 게임 종료 UI에 소요시간 Text추가
    private float round1Time = 0, round2Time = 0; // 각 라운드의 소요시간

    private void Start()
    {
        round1Time = 0; // 1라운드 시간 0으로 초기화
        round2Time = 0; // 2라운드 시간 0으로 초기화
    }
    public void StartGame()
    {
        introPanel.SetActive(false); // intro 패널 숨김
        StartCoroutine(ShowRound1Panel()); // ShowRound1Panel() coroutine 실행

        round1Time = Time.time; // 1라운드 시작 시간 기록
    }
    void CreatCards()
    {
        // 라운드가 생겼으므로 오류를 방지하기 위해 카드를 생성하기 전에 카드 리스트를 비워줌
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();

        List<CardData> pairedCards = new List<CardData>(); // 카드 데이터를 가져옴

        if (currentRound == 1)
        {
            // 카드를 짝지어서 저장함
            List<CardData> availableCards = new List<CardData>(cardDataList); // 카드 원본 리스트를 복사
            List<CardData> selectedPairs = new List<CardData>(); // 선택된 카드 쌍을 저장해줌

            while (selectedPairs.Count < 8 && availableCards.Count > 1) // 선택된 카드가 8장보다 적고 && 남은 카드가 1장보다 많다면...(루프 반복)
            {
                CardData first = availableCards[0]; // 첫번째 카드 선택
                availableCards.RemoveAt(0);         // 선택된 카드는 리스트에서 제거

                for (int j = 0; j < availableCards.Count; j++) // 카드를 한장씩 체크해가면서 남은 카드가 선택한 카드보다 많다면...(루프 반복)
                {
                    // 과일 종류를 체크해서 쌍으로 저장
                    if (first.fruitType == availableCards[j].fruitType)
                    {
                        selectedPairs.Add(first); // 선택된 카드와
                        selectedPairs.Add(availableCards[j]); // 같은 fruitType인 카드를 쌍으로 저장하고
                        availableCards.RemoveAt(j); // 쌍이 된 카드를 삭제
                        break;
                    }
                }
            }
            pairedCards.AddRange(selectedPairs); // 최종 선택된 카드 쌍을 보관
        }
        else if (currentRound == 2)
        {
            // 카드를 짝지어서 저장함
            List<CardData> availableCards = new List<CardData>(cardDataList); // 카드 원본 리스트를 복사
            List<CardData> selectedPairs = new List<CardData>(); // 선택된 카드 쌍을 저장해줌

            while (selectedPairs.Count < 8 && availableCards.Count > 1) // 선택된 카드가 8장보다 적고 && 남은 카드가 1장보다 많다면...(루프 반복)
            {
                CardData first = availableCards[0]; // 첫번째 카드 선택
                availableCards.RemoveAt(0);         // 선택된 카드는 리스트에서 제거

                for (int j = 0; j < availableCards.Count; j++) // 카드를 한장씩 체크해가면서 남은 카드가 선택한 카드보다 많다면...(루프 반복)
                {
                    // 과일 종류를 체크해서 쌍으로 저장
                    if (first.quantity == availableCards[j].quantity)
                    {
                        selectedPairs.Add(first); // 선택된 카드와
                        selectedPairs.Add(availableCards[j]); // 같은 fruitType인 카드를 쌍으로 저장하고
                        availableCards.RemoveAt(j); // 쌍이 된 카드를 삭제
                        break;
                    }
                }
            }
            pairedCards.AddRange(selectedPairs); // 최종 선택된 카드 쌍을 보관
        }
        
        pairedCards = ShuffleList(pairedCards); // 카드 섞기

        foreach (CardData cardData in pairedCards)
        {
            GameObject newCardObj = Instantiate(cardPrefab, cardParent); // 카드 생성
            Card card = newCardObj.GetComponent<Card>(); // Card.cs 가져오기

            card.SetCard(cards.Count, cardData); // Card.cs를 가져오는 card에서 SerCard 함수를 호출
            //          (현재까지 생성된 카드 개수를 ID로 사용, 현재 카드에 적용할 데이터)
            cards.Add(card); // 새로 생성된 카드를 추가하여 관리

            Image backImage = newCardObj.transform.Find("BackImage")?.GetComponent<Image>(); // 생성된 카드에서 뒷면을 찾는다.
            if (backImage != null) // 뒷면에 이미지를 넣어준다.
            {
                backImage.sprite = backImageSprite;
            }

            newCardObj.GetComponent<Button>().onClick.AddListener(card.OnClickCard); // 카드를 클릭하면 카드가 뒤집히는 함수를 Card.cs에서 불러옴
        }
    }

    List<T> ShuffleList<T>(List<T> list) // 리스트 순서대로 나오던 카드들을 랜덤으로 섞어주는 역할을 하는 코드
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count); // 랜덤 인덱스를 선택한다. 
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // 원래 리스트 순서와 랜덤으로 뽑인 리스트의 순서를 바꿔서 원래 리스트를 랜덤 리스트로 변환한다. 
        //  (원래 리스트 순서, 랜덤 리스트 순서) = (랜덤 리스트 순서, 원래 리스트 순서) 
        }
        return list;
    }

    IEnumerator ShowRound1Panel()
    {
        round1Panel.SetActive(true);        // 1라운드 설명 패널 출력
        yield return new WaitForSeconds(2f); // 게임 시작전 게임 설명을 2초간 보여주기 위해 코루틴 함수로 변경 
        round1Panel.SetActive(false);        // 1라운드 설명 패널 숨김
        CreatCards();
        StartCoroutine(ShowCardThenHide()); // 카드의 앞면을 보여주고 숨기기
    }

    IEnumerator ShowRound2Panel()
    {
        round2Panel.SetActive(true);        // 2라운드 설명 패널 출력
        yield return new WaitForSeconds(2f); // 게임 시작전 게임 설명을 2초간 보여주기 위해 코루틴 함수로 변경 
        round2Panel.SetActive(false);        // 2라운드 설명 패널 숨김
        CreatCards();
        StartCoroutine(ShowCardThenHide()); // 카드의 앞면을 보여주고 숨기기
    }

    // 카드가 정답이면 비활성화
    IEnumerator DisableCards(Card first, Card second) // coroutine 함수로 yield(대기함수) 를 사용할 수 있다. 
    {
        yield return new WaitForSeconds(0.5f); // 카드가 뒤집힌 후 0.5초간 대기
        first.DisableCard(); // 첫번째 카드 비활성화
        second.DisableCard(); // 두번째 카드 비활성화 
        CheckAllCardsMatched(); // 
    }

    // 카드가 오답이면 다시 뒷면으로 뒤집기
    IEnumerator FilpBackCards(Card first, Card second)
    {
        yield return new WaitForSeconds(0.5f); // 카드가 뒤집힌 후 0.5초간 대기
        first.FilpToBack(); // 첫번째 카드 뒤집기
        second.FilpToBack(); // 두번째 카드 뒤집기
    }

    // 카드가 정답인지 확인하는 코드가 필요
    public void CheckCard(Card selectedCard) // 이 코드가 카드에 적용되도록 해야한다. 
    {
        if (firstCard == null) // 첫번째 카드를 선택했는데 null이라면...(첫번째로 선택했다면...) // 전에 선택된 카드가 있다면 firstCard != null 이다.
        {
            firstCard = selectedCard; // 선택한 카드를 첫번째 카드로 지정한다. 
        }
        else // 첫번째 카드가 선택 되었다면...(두번째 카드 선택이라면...)
        {
            secondCard = selectedCard; // 선택한 카드를 두번째 카드로 지정한다. 

            bool isMatch = false; // 두 카드가 정답이라면(조건에 부합한다면) 비활성화 시킨다.

            // 카드 정답기준
            if (currentRound == 1)
            {
                isMatch  = (firstCard.cardData.fruitType == secondCard.cardData.fruitType); // 과일 이미지가 같다면 정답
            }
            else if (currentRound == 2)
            {
                isMatch  = (firstCard.cardData.quantity == secondCard.cardData.quantity); // 과일 개수가 같다면 정답
            }
           

            if (isMatch) // 두 카드가 정답이라면...
            {
                StartCoroutine(DisableCards(firstCard, secondCard)); // 카드를 비활성화시키는 코드를 실행
            }
            else // 두 카드가 오답이라면...
            {
                StartCoroutine(FilpBackCards(firstCard, secondCard)); // 카드를 다시 뒤집는 코드를 실행
            }
            
            // 카드의 정답과 오답을 확인한 뒤 첫번째 카드와 두번째 카드 자리를 null로 비운다. 다음에 선택된 카드를 다시 비교할 수 있도록 초기화 한다. 
            firstCard = null;
            secondCard = null;
        }
    }

    // 게임이 시작할 때 플레이어에게 카드를 한번 확인시켜주어야한다. 
    IEnumerator ShowCardThenHide()
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 동안 대기하면서 카드를 확인시켜준다.

        foreach (Card card in cards) // 리스트에 있는 모든 카드를 앞면으로 뒤집는다...
        {
            card.FilpToFront();
        }

        yield return new WaitForSeconds(1.5f); // 1.5초 동안 대기하면서, 앞면을 유지한다. 

        foreach (Card card in cards) // 리스트에 있는 모든 카드를 뒷면으로 뒤집는다...
        {
            card.FilpToBack();
        }
    }


        // 모두 정답이면 게임이 종료되도록 한다. 
    void CheckAllCardsMatched()
    {
        bool allMatched = true; // 모든 카드가 맞춰진 상태 = true;

        foreach (var card in cards) // 모든 카드의 상태를 검사
        {
            if (card.gameObject.activeSelf && !card.IsDisabled) // 만약 카드가 활성화 되어있다면 && 비활성화된 카드가 없다면...
            {
                allMatched = false; // CheckAllCardsMatched 함수는 false
                break; // CheckAllCardsMatched 함수 종료
            }
        }

        if (allMatched) // 모든 카드가 맞춰졌다면
        {
            if (currentRound == 1) // 1라운드가 끝나면 2라운드 실행 버튼 활성화
            {
                nextGameButton.gameObject.SetActive(true); // 모든 카드가 비활성화 되면 "Next" 버튼 활성화
            }
            else if (currentRound == 2)
            {
                round2Time = Time.timeSinceLevelLoad - round1Time - 3.5f; // 모든 카드가 비활성화 되면 소요시간에 1라운드 시간을 빼서 2라운드 소요시간을 계산
                // (게임을 설명해주는 1.5초 + 카드를 보여주는 2초) = 3.5초를 빼야 정확한 시간 측정 가능
                ShowGameOver(); // 게임 종료 함수 실행
            }
        }
    }

    public void ShowGameOver() // 게임 종료 UI
    {
        GameOverPanel.SetActive(true);

        GameOverText.text = $"Round 1: {round1Time:F1} Second\n" +
                            $"Round 1: {round2Time:F1} Second";     // 소요시간을 출력하는 텍스트
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // (괄호)안의 씬을 로드해줌
        // SceneManager.GetActiveScene() > 현재 실행중인 Scene의 정보를 가져옴
        // .name > 현재 실행중인 Scene의 이름을 가져옴.
        // 현재 Scene을 다시 실행시키는 함수
    }

    public void NextRound()
    {
        foreach (var card in cards)
        {
            Destroy(card.gameObject); // 현재 생성되어 있는 카드를 제거(비활성화 상태의 카드 제거)
        }
        cards.Clear(); // 카드 리스트를 비움

        currentRound = 2;
        nextGameButton.gameObject.SetActive(false); // 2라운드가 시작되었기 때문에 "next" 버튼 숨김
        StartCoroutine(ShowRound2Panel()); // ShowRound2Panel() coroutine 실행

        round1Time = Time.timeSinceLevelLoad - 3.5f; // 1라운드 종료시간을 저장한다. 2라운드 시작전 1라운드 시간 종료 
        // (게임을 설명해주는 1.5초 + 카드를 보여주는 2초) = 3.5초를 빼야 정확한 시간 측정 가능
    }
}
