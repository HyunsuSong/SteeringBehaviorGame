# 기본 정보

이름: 송현수

학번: 20161642

# 구성

구현한 조종 행동은 Arrive, Flee, Evade, Pursuit, Wander, Wall_avoidance, offsetPursuit

총 7개 구현했습니다. Seek는 Arrive가 대체했습니다.

데모 내용은

맵 자체는 부활존과 벽, 맵 두 곳에는 플레이어를 공격하는 총알을 발사하는 타워 2개가 있습니다.

플레이어는 체력 10을 가지고 플레이어의 총알을 제외한 어떠한 총알에 맞아도 체력이 1씩 차감됩니다.  ` 키를 이용해 총알을 발사할 수 있습니다.

플레이어의 체력이 모두 닳았을 경우 부활존으로 가며, 3초간 대기하게 되고 원래의 위치로 리스폰이 됩니다.

등장 개체는 모두 Wall_Avoidance를 기본으로 하며, 파란색 5개체는 Wander 이용합니다.

파란 개체는 체력 5를 가지고 플레이어의 총알에 한번 맞을때마다 체력 1씩 차감됩니다.

초록색 1개체는 마지막 보스이며, 검은색 4개체는 초록색 개체를 따라다니며 플레이어를 공격하는 개체입니다.

초록색 개체는 플레이어가 클릭 입력을 할 경우, Pursuit를 기본으로 실행하며, Pursuit에는 Arrive로 구현이 되어 있습니다. 또한, 플레이어가 검은 개체까지 모두 죽였을 경우 Evade를 사용하며, Flee로 Evade가 구현이 되어 있습니다.

초록색 개체는 검은색 개체까지 모두 죽어야만 대미지를 입으며, 대미지는 플레이어의 공격에 의해서가 아닌, 중앙 타워 2곳에서 발사하는 총알에 맞아야만 체력이 1씩 차감됩니다. > 캐릭터와 동일한 조건에 의해서 죽으며, 플레이어는 초록 개체를 쫒으면서 초록 개체가 맞도록 유도해야 합니다. / 초록 개체는 체력을 15 가지고 있습니다.

검은색 개체는 초록 개체를 선두로 쫒아다니며 플레이어를 공격하는 개체입니다. 체력은 10을 가지고 있으며, offsetPursuit로 구현이 되어 있습니다.