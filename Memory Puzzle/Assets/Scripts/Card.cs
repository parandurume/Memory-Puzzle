// Card.cs
// 카드의 상태를 나타내는 코드
// 카드의 상태: 카드가 앞면 or 뒷면, 카드가 정답 or 오답, 정답이라면...비활성화, 오답이라면...다시 뒷면으로
//              카드가 선택 되었는가..등

using UnityEngine;
using UnityEngine.UI;
// 카드의 상태를 보여주는 컴포넌트
public class Card : MonoBehaviour
{
    public CardData cardData;   // 카드 데이터: 카드의 이미지, 과일종류, 개수 등의 여러 정보는 보관하는 CardData 코드에서 정보를 받아옴.
    public Image frontImage;    // 카드 앞면에 들어가는 이미지를 정해주는 변수
    public Image backImage;     // 카드 뒷면에 들어가는 이미지를 정해주는 변수

    private bool isFlipped = false; // 카드의 현재 상태를 나타내는 변수 (true = 앞면, false = 뒷면)
    public bool IsDisabled { get; private set; } // 정답 카드가 비활성화 되었는지 여부 = 카드가 정답이 되어 사라졌는가?

    // 카드 초기 세팅 함수: 카드의 id와 데이터 입력 후, 
    // 카드 앞명의 이미지를 설정 및 초기 상태(뒷면)로 전환>게임이 시작하면 카드가 뒤집혀 있다. 
    public void SetCard(int id, CardData data)
    {
        cardData = data; // 입력된 cardData를 data로 저장

        if (frontImage != null) // 만약, frontImage가 비어있다면...
        {
            frontImage.sprite = data.image; // frontImage로 사용되는 sprite를 data의 image로 저장시킨다. 
        }
        FilpToBack(); // 게임이 시작할때 카드를 뒷면으로 설정해서 숨김 처리하도록 셋팅
    }

    // 카드 클릭 시 카드가 뒤집히는(앞면으로) 함수(카드가 비활성화되지 않은 경우 뒤집기 실행, 비활성화 된 경우 실행 안됨)
    public void OnClickCard()
    {
        Debug.Log($"🖱 카드 클릭됨: {cardData.fruitType}");
    
        if (!IsDisabled) // 비활성화 된 카드는 클릭해도 반응x
        {
            FlipCard(); // 카드를 앞면/뒷면 으로 전환
        }
    }
    
    // 카드의 상태를 전환하는 함수가 필요함 > 현재 상태(앞>뒤, 뒤>앞)을 반전시키는 함수
    void FlipCard()
    {
        isFlipped = !isFlipped; // 카드를 뒤집음(앞>뒤, 뒤>앞)
        frontImage.gameObject.SetActive(isFlipped); // isFlipped가 true면 앞면 표시
        backImage.gameObject.SetActive(!isFlipped); // isFlipped가 false면 뒷면 표시

        //해당 함수에서 카드의 정답을 확인하도록 함, 플레이어의 클릭에 반응하는 함수 > 카드를 각각 반응시킴(한장씩)
        if(isFlipped)
        {
            // GameManager에서 카드의 상태를 확인해서 정답을 확인해야함.
            GameManager.Instance.CheckCard(this);
        }
    }
    
    // 카드를 뒷면으로 뒤집는 함수, 게임 시작전이나 특정 상황(2라운드)에서 카드 전체를 뒤집음
    public void FilpToBack()
    {
        isFlipped = false; // 카드를 뒷면으로
        frontImage.gameObject.SetActive(false); // 앞면 이미지 숨김 (뒷면이미지와 겹침을 방지)
        backImage.gameObject.SetActive(true); // 뒷면 이미지 표시
    }

    // 카드를 앞면으로 뒤집는 함수, 게임 시작전이나 특정상황(카드 확인 절차)에서 카드 전체를 뒤집음
    public void FilpToFront()
    {
        isFlipped = true; // 카드를 앞면으로
        frontImage.gameObject.SetActive(true); // 앞면 이미지 표시 
        backImage.gameObject.SetActive(false); // 뒷면 이미지 숨김 (앞면이미지와 겹침을 방지)
    }

    // 카드의 정답을 맞춘 경우, 카드가 비활성화 되도록 하는 함수
    public void DisableCard()
    {
        IsDisabled = true; // 카드를 비활성화
        GetComponent<Button>().interactable = false; // 버튼클릭(카드 = 버튼)기능 차단
        frontImage.gameObject.SetActive(false); // 앞면 이미지 숨김 
        backImage.gameObject.SetActive(false); // 뒷면 이미지 숨김
    }
    // 정답 카드를 제거하지 않고 비활성화(투명화)하는 이유: 정답 카드를 제거하게 되면 남은 카드가 재정렬됨 > 남은 카드끼리 다시 섞임
}
