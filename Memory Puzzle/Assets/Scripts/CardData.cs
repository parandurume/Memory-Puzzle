// CardData.cs
// 개별 카드의 정보를 저장하는 코드
// 카드의 정보: 이미지, 과일의 갯수, 과일의 종류
// 목표: 각각의 카드의 정보를 저장하고 카드의 데이터를 다른 코드에서 가져와 사용할 수 있도록 함.

using UnityEditor.U2D;
using UnityEngine;

[System.Serializable] // Inpector에서 해당 클래스를 직렬화하여 보여줄 수 있도록 함.
public class CardData
{
    public Sprite image; // 카드 앞면에 사용할 이미지 (Sprite 타입)
    public int quantity; // 과일의 개수를 나타내는 정수형 변수
    public string fruitType; // 과일의 유형을 나타내는 문자열 (사과, 바나나, 포도, 수박)

    public CardData(Sprite image, int quantity, string fruitType)
    {
        this.image = image;
        this.quantity = quantity;
        this. fruitType = fruitType;
    }
}
