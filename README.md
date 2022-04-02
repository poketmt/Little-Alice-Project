# Little-Alice-Project-Script
C#과 유니티를 배우고 처음 진행했던 프로젝트의 담당파트 소스코드입니다.

# StoryBoard
상속을 통해 연결된 클래스들의 기능을 하나의 메서드로 묶어 enum타입을 통해 선택한 기능만을 실행시킵니다.
</br>string값(storyNum, nextStoryNum)을 기준으로 스토리보드가 순차적으로 연결되는 형식을 통해 다양한 스토리 연출을 구성할 수 있습니다.
</br></br>예시)
</br>
![storyboard01](https://user-images.githubusercontent.com/94150816/161369829-0030ef97-72f2-4daa-b0e4-173baa69e146.png)
![StoryBoard03 gif](https://user-images.githubusercontent.com/94150816/161370283-e831318e-878f-4e35-9d85-719ec3d0ca56.gif)

# CameraBoard
카메라 액션에 관한 이동, 전환, 흔들림 등 여러 기능을 제공합니다.
</br>StoryBoard에 의한 연결을 통해 사용하거나 필요한 클래스에서 Manager의 Instance를 직접 호출하여 사용합니다.
</br></br>예시)
</br>
![Camera01](https://user-images.githubusercontent.com/94150816/161372467-1ea116f6-f419-4a20-9729-e543814b8451.gif)
![Camera02](https://user-images.githubusercontent.com/94150816/161372470-b5d5e861-06d2-4713-8d9d-153836811dcb.gif)

# AudioBoard
오디오 재생에 관한 기능을 제공합니다.
</br>StoryBoard에 의한 연결을 통해 사용하거나 필요한 클래스에서 Manager의 Instance를 직접 호출하여 사용합니다.
</br></br>예시)
</br>
<img src="https://user-images.githubusercontent.com/94150816/161372690-8c3a4479-bba0-4af4-b25d-73cb7db2e3af.png" width="600">

# LightBoard
라이트의 밝기나 회전, 스카이박스의 조절을 통한 전체적인 환경조명의 관리 등 조명연출에 관한 기능을 제공합니다.
</br>StoryBoard에 의한 연결을 통해 사용하거나 필요한 클래스에서 Manager의 Instance를 직접 호출하여 사용합니다.
</br></br>예시)
</br>
![Light01](https://user-images.githubusercontent.com/94150816/161373102-f943cac2-2d01-4b7c-859c-22863ec459bc.gif)

# NPC
아이템에 따라 상호작용하는 NPC가 구현되어있습니다.
</br>마찬가지로 StoryBoard나 Manager의 Instance호출을 통해 필요한 곳에 사용합니다.
</br></br>예시)
![NPC](https://user-images.githubusercontent.com/94150816/161374588-7e7961f7-2e06-4ba9-bdd0-73b55f68eea0.png)</br>
![NPC01](https://user-images.githubusercontent.com/94150816/161374528-670ba83c-4842-454d-8114-eeffc97715b9.gif)</br>
![NPC02](https://user-images.githubusercontent.com/94150816/161374530-25accb81-fe7a-4e36-ab5e-53e846597725.gif)</br>
