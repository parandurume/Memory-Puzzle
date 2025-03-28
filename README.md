# Memory-Puzzle
Unity와 Chat GPT를 활용한 MCI 게임 만들기

1. 프로젝트 준비 및 환경 설정 Unity 프로젝트 생성: 2D 프로젝트 선택 필요한 폴더 구조 설정 Scripts/ → C# 스크립트 저장 Sprites/ → 카드 이미지 저장 Prefabs/ → 카드 프리팹 저장 UI/ → 게임 UI 요소 저장

2. 카드 데이터 모델 (CardData) 설계 개별 카드의 정보를 저장할 CardData.cs 클래스를 생성 Unity Inspector에서 편집할 수 있도록 [System.Serializable] 속성 추가 카드 정보를 저장할 변수 정의 이미지(Sprite) 과일 종류(string) 과일 개수(int)

3. 카드 오브젝트 (Card) 구현 Card 프리팹 생성 앞면 (FrontImage): 과일 이미지 표시 뒷면 (BackImage): 공통된 카드 뒷면 이미지 텍스트 (QuantityText): 과일 개수 표시 Button 컴포넌트 추가: 카드 클릭 가능하도록 설정
Card.cs 스크립트 구현 SetCard() → 카드의 데이터를 설정 (이미지 & 개수 적용) FlipToFront() → 카드 앞면을 보이게 함 FlipToBack() → 카드 뒷면을 보이게 함 OnClickCard() → 카드 클릭 시 FlipCard() 실행 DisableCard() → 정답 맞춘 카드 비활성화

4. 게임 매니저 (GameManager) 구현 GameManager.cs 스크립트 생성 카드 데이터를 저장할 리스트 생성 (List) 카드 프리팹을 담을 변수 생성 (GameObject cardPrefab) 카드가 배치될 부모 오브젝트 설정 (Transform cardParent) 게임 UI 관리 (Intro, Round Panel, Game Over Panel) 게임 진행 변수 (currentRound, round1Time, round2Time)
게임 로직 구현 StartGame() → 게임 시작 & 카드 생성 CreateCards() → 라운드에 맞는 카드 리스트 생성 및 배치 ShowCardsThenHide() → 게임 시작 시 모든 카드를 보여줬다가 숨김 CheckCard() → 카드 두 개가 선택되었을 때 정답 비교 NextRound() → 2라운드로 진행 ShowGameOver() → 게임 종료 화면 표시

5. UI 및 게임 흐름 구성 게임 시작 화면 (Intro Panel) 시작 버튼 (StartGame()) 클릭 시 게임 시작
라운드 진행 UI Round 1 Panel → 1라운드 시작 안내 Round 2 Panel → 2라운드 시작 안내
게임 종료 UI Game Over Panel → 최종 결과 & 재시작 버튼 걸린 시간 (round1Time, round2Time) 표시

6. 카드 비교 및 정답 처리 카드 클릭 시 CheckCard() 실행 첫 번째 카드 선택 → firstCard에 저장 두 번째 카드 선택 → secondCard에 저장 두 개의 카드 비교 정답이면 → DisableCard() 실행 후 체크 오답이면 → FlipBackCards() 실행
모든 카드가 맞춰지면 다음 라운드 버튼 표시 (NextRound())
2라운드까지 종료되면 ShowGameOver() 실행
