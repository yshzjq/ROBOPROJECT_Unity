using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DroneSpawnerScripts : MonoBehaviour
{
    public Drone_Chaser_attack drone_Chaser_Prefab; // 생성할 드론들 원본 프리팹
    public Drone_Shooter drone_Shooter_Prefab;
    public Drone_Boss_Controller drone_Boss_Prefab;

    
    public DroneChaserData[] droneChaserDatas; // 사용할 드론체이서 셋업 데이터들
    public DroneShooterData[] droneShooterDatas;
    public DroneBossData[] droneBossDatas;

    public Transform[] spawnPoints; // 드론 AI를 소환할 위치들

    private List<Drone_Chaser_attack> chaserDrones = new List<Drone_Chaser_attack>(); // 생성된 드론들을 담는 리스트
    private List<Drone_Shooter> shooterDrones = new List<Drone_Shooter>(); // 생성된 드론들을 담는 리스트
    private List<Drone_Boss_Controller> bossDrones = new List<Drone_Boss_Controller>();

    public Transform bossSpawnPosition;

    private int wave; // 현재 웨이브

    public GameObject bgmManager;
    bool isBossStage = false;

    public AudioClip bossAppearBGM;

    // 보스 체력 바
    private GameObject bossHPUI;

    public void bossBGM()
    {
        AudioSource audio = bgmManager.GetComponent<AudioSource>();

        audio.clip = bossAppearBGM;
        audio.Play();

    }

    private void Update()
    {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        if (isBossStage == true)
        {
            return;
        }
        else
        {
            // 드론들를 모두 물리친 경우 다음 스폰 실행
            if (chaserDrones.Count + shooterDrones.Count <= 0)
            {
                SpawnWave();
            }

            
        }

        // UI 갱신
        UpdateUI();

    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI()
    {
        // 현재 웨이브와 남은 적 수 표시
        UIManagerScripts.instance.UpdateWaveText(wave);
        UIManagerScripts.instance.UpdateEnemyCountText(chaserDrones.Count + shooterDrones.Count + bossDrones.Count);
    }

    // 현재 웨이브에 맞춰 드론들을 생성
    private void SpawnWave()
    {
        // 웨이브 1 증가
        wave++;

        // 웨이브가 3일때를 작성할것임 

        // 현재 웨이브 * 1.5에 반올림 한 개수 만큼 드론를 생성(수정 예정?) 웨이브가 3일때 보스 등장하게 추가 예정

        if (wave >= 5)
        {
            isBossStage = true;
            CreateBossDrone();
        }
        else
        {
            int spawnCount = Mathf.RoundToInt(wave * 3f);

            // spawnCount 만큼 드론들을 생성
            for (int i = 0; i < spawnCount; i++)
            {
                // 드론 생성 처리 실행

                if (Random.Range(0, 2) == 0)
                {
                    CreateDroneChaser();
                }
                else
                {
                    CreateDroneShooter();
                }
            }
        }
        
    }

    // 드론 슈터 생성
    private void CreateDroneShooter()
    {
        // 사용할 드론 데이터 랜덤으로 결정
        DroneShooterData droneShooterData = droneShooterDatas[Random.Range(0, droneShooterDatas.Length)];

        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 드론 체이서 프리팹으로부터 드론 생성
        Drone_Shooter shooterDrone = Instantiate(drone_Shooter_Prefab, spawnPoint.position, spawnPoint.rotation);

        // 생성한 드론의 능력치 설정
        shooterDrone.Setup(droneShooterData);

        // 생성된 드론를 리스트에 추가
        shooterDrones.Add(shooterDrone);

        // 드론의 onDeath 이벤트에 익명 메서드 등록

        // 사망한 드론를 리스트에서 제거
        shooterDrone.onDeath += () => shooterDrones.Remove(shooterDrone);
        // 사망한 드론를 10 초 뒤에 파괴
        shooterDrone.onDeath += () => Destroy(shooterDrone.gameObject, 10f);
        // 드론 사망시 점수 상승
        shooterDrone.onDeath += () => GameManager.instance.AddScore(Random.Range(100, 500));

        shooterDrone.onDeath += () => GameManager.instance.AddCoin(Random.Range(100, 500));
    }

    private void CreateDroneChaser()
    {
        // 사용할 드론 데이터 랜덤으로 결정
        DroneChaserData chaserDroneData = droneChaserDatas[Random.Range(0, droneChaserDatas.Length)];

        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 드론 체이서 프리팹으로부터 드론 생성
        Drone_Chaser_attack chaserDrone = Instantiate(drone_Chaser_Prefab, spawnPoint.position, spawnPoint.rotation);

        // 생성한 드론의 능력치 설정
        chaserDrone.Setup(chaserDroneData);

        // 생성된 드론를 리스트에 추가
        chaserDrones.Add(chaserDrone);

        // 드론의 onDeath 이벤트에 익명 메서드 등록


        chaserDrone.onDeath += () => chaserDrone.sparkAttackStop();
        // 사망한 드론를 리스트에서 제거
        chaserDrone.onDeath += () => chaserDrones.Remove(chaserDrone);
        // 사망한 드론를 10 초 뒤에 파괴
        chaserDrone.onDeath += () => Destroy(chaserDrone.gameObject, 10f);
        // 드론 사망시 점수 상승
        chaserDrone.onDeath += () => GameManager.instance.AddScore(Random.Range(50, 300));
        chaserDrone.onDeath += () => GameManager.instance.AddCoin(Random.Range(50,300));
    }

    private void CreateBossDrone()
    {
        // 사용할 드론 데이터 랜덤으로 결정
        DroneBossData bossDroneData = droneBossDatas[Random.Range(0, droneBossDatas.Length)];

        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = bossSpawnPosition;

        // 드론 체이서 프리팹으로부터 드론 생성
        Drone_Boss_Controller bossDrone = Instantiate(drone_Boss_Prefab, spawnPoint.position, spawnPoint.rotation);

        // 생성한 드론의 능력치 설정
        bossDrone.Setup(bossDroneData);

        // 생성된 드론를 리스트에 추가
        bossDrones.Add(bossDrone);

        bossBGM();

        // 드론의 onDeath 이벤트에 익명 메서드 등록


        bossDrone.onDeath += () => bossDrone.sparkAttackStop();

        bossDrone.onDeath += () => bossDrones.Remove(bossDrone);
        // 사망한 드론를 10 초 뒤에 파괴
        bossDrone.onDeath += () => Destroy(bossDrone.gameObject, 10f);
        // 드론 사망시 점수 상승
        bossDrone.onDeath += () => GameManager.instance.AddScore(Random.Range(1000, 2000));
        bossDrone.onDeath += () => GameManager.instance.AddCoin(Random.Range(1000, 2000));

        bossDrone.onDeath += () => StartCoroutine("bossKill");
    }


    IEnumerator bossKill()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(3f);
        Time.timeScale = 1f;
        UIManagerScripts.instance.SetActiveVictoryUI(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameClearScene");
    }
}
