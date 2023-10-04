# HalloweenMystery
> 미스터리 판타지 할로윈 호러 2D 액션 탑다운 어드벤처 전략 RPG 로그라이크

# 개요 
> 할로윈에 사라진 친구들을 찾아 떠나는 어드벤쳐 게임<br>

<p>
  <img src="https://user-images.githubusercontent.com/115692722/272451159-fe70fd6c-43e3-4477-a0f1-cf31f1980809.gif" width="49%" height="250" />
  <img src="https://user-images.githubusercontent.com/115692722/272449284-0c1ac259-4973-49da-8917-da2ef34c0c6a.gif" width="49%" height="250" />  
</p>


# 게임 플레이
https://noun06.itch.io/halloweenmystery



# ⏰ 개발 기간 
- 2023.09.25 ~ 2023.10.04

# 🧑‍🤝‍🧑 개발 인원 
- 👑박희원, 김대열, 김나운, 유채연, 이경현

# ⚙️ 개발 환경 
- Unity 2022.3.2f1

# 설계
- 컴포넌트를 기반으로 설계하여 각 게임 오브젝트들에 필요한 컴포넌트를 부착하는 방식으로 설계했습니다.

  <p></p>
<img width="32%" height = "250"  src="https://github.com/phw97123/HalloweenMystery/assets/115692722/1a608e57-b9eb-470a-9547-85a1a66613cc">

- 엔터티 설계도
<img width="32%" height = "250" src="https://github.com/phw97123/HalloweenMystery/assets/115692722/4dd298eb-4091-43be-8d36-17d36a29e811">

- 맵 생성 설계도
<img width="32%" height = "250"  src="https://github.com/phw97123/HalloweenMystery/assets/115692722/09b0f4d8-bfba-410b-b0d4-4e96d39ce207">

# 핵심기능

- Intro

  <img width="32%" height = "250" src = "https://user-images.githubusercontent.com/115692722/272461620-f93b1989-699c-4973-a043-3e7992177aee.gif">
  
  담당 : 박희원<br>
  [클래스](https://github.com/phw97123/HalloweenMystery/blob/main/Assets/Scripts/Entities/Timeline/TimelineController.cs)


- 전투

  <p>
    <img width="32%" height = "250" src = "https://user-images.githubusercontent.com/115692722/272449284-0c1ac259-4973-49da-8917-da2ef34c0c6a.gif">
    <img width="32%" height = "250" src = "https://user-images.githubusercontent.com/115692722/272451159-fe70fd6c-43e3-4477-a0f1-cf31f1980809.gif">
  </p>

  담당 : 김대열, 김나운<br>
  
  [클래스](https://github.com/phw97123/HalloweenMystery/tree/main/Assets/Scripts/Entities/Attacks)
  
- PlayerRoom

  담당 : 유채연<br>
  
  [클래스](https://github.com/phw97123/HalloweenMystery/blob/main/Assets/Scripts/Managers/RoomManager.cs)

- Item And Enhance Weapon

  담당 : 유채연<br>
  
  [클래스](https://github.com/phw97123/HalloweenMystery/tree/main/Assets/PCG)
  
- InGame UI

  담당 : 김대열, 김나운<br>
  
  [클래스](https://github.com/phw97123/HalloweenMystery/tree/main/Assets/Scripts/UI)

- Player State Management

    담당 : 박희원<br>
    [클래스](https://github.com/phw97123/HalloweenMystery/blob/928db51d34bfd134c2283768178bb9fbfa0a429f/Assets/Scripts/Managers/GameManager.cs#L210C1-L244C2)

- Achievement And Weapon Unlock

    담당 : 박희원<br>
    [클래스](https://github.com/phw97123/HalloweenMystery/blob/main/Assets/Scripts/Managers/AchiveManager.cs)

- Player Combat

  담당 : 김대열, 김나운<br>
  
  [클래스](https://github.com/phw97123/HalloweenMystery/tree/main/Assets/Scripts/Entities/Attacks)

- Enemy AI
  
  담당 : 김나운<br>
  [클래스](https://github.com/phw97123/HalloweenMystery/tree/main/Assets/Scripts/Entities/Enemies)
  - 추적
  - 순간이동
  - 원거리 공격

- MapGeneration
  담당 : 이경현<br>
  [클래스](https://github.com/phw97123/HalloweenMystery/tree/main/Assets/PCG)
  
- Sound

  담당 : 박희원<br>
  
  [클래스](https://github.com/phw97123/HalloweenMystery/blob/main/Assets/Scripts/Managers/SoundManager.cs)

- Data Management

  담당 : 박희원<br>
  [클래스](https://github.com/phw97123/HalloweenMystery/blob/main/Assets/Scripts/Managers/DataManager.cs)





  
