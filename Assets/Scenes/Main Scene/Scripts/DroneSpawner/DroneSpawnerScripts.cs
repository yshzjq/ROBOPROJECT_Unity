using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DroneSpawnerScripts : MonoBehaviour
{
    public Drone_Chaser_attack drone_Chaser_Prefab; // ������ ��е� ���� ������
    public Drone_Shooter drone_Shooter_Prefab;
    public Drone_Boss_Controller drone_Boss_Prefab;

    
    public DroneChaserData[] droneChaserDatas; // ����� ���ü�̼� �¾� �����͵�
    public DroneShooterData[] droneShooterDatas;
    public DroneBossData[] droneBossDatas;

    public Transform[] spawnPoints; // ��� AI�� ��ȯ�� ��ġ��

    private List<Drone_Chaser_attack> chaserDrones = new List<Drone_Chaser_attack>(); // ������ ��е��� ��� ����Ʈ
    private List<Drone_Shooter> shooterDrones = new List<Drone_Shooter>(); // ������ ��е��� ��� ����Ʈ
    private List<Drone_Boss_Controller> bossDrones = new List<Drone_Boss_Controller>();

    public Transform bossSpawnPosition;

    private int wave; // ���� ���̺�

    public GameObject bgmManager;
    bool isBossStage = false;

    public AudioClip bossAppearBGM;

    // ���� ü�� ��
    private GameObject bossHPUI;

    public void bossBGM()
    {
        AudioSource audio = bgmManager.GetComponent<AudioSource>();

        audio.clip = bossAppearBGM;
        audio.Play();

    }

    private void Update()
    {
        // ���� ���� �����϶��� �������� ����
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
            // ��е鸦 ��� ����ģ ��� ���� ���� ����
            if (chaserDrones.Count + shooterDrones.Count <= 0)
            {
                SpawnWave();
            }

            
        }

        // UI ����
        UpdateUI();

    }

    // ���̺� ������ UI�� ǥ��
    private void UpdateUI()
    {
        // ���� ���̺�� ���� �� �� ǥ��
        UIManagerScripts.instance.UpdateWaveText(wave);
        UIManagerScripts.instance.UpdateEnemyCountText(chaserDrones.Count + shooterDrones.Count + bossDrones.Count);
    }

    // ���� ���̺꿡 ���� ��е��� ����
    private void SpawnWave()
    {
        // ���̺� 1 ����
        wave++;

        // ���̺갡 3�϶��� �ۼ��Ұ��� 

        // ���� ���̺� * 1.5�� �ݿø� �� ���� ��ŭ ��и� ����(���� ����?) ���̺갡 3�϶� ���� �����ϰ� �߰� ����

        if (wave >= 5)
        {
            isBossStage = true;
            CreateBossDrone();
        }
        else
        {
            int spawnCount = Mathf.RoundToInt(wave * 3f);

            // spawnCount ��ŭ ��е��� ����
            for (int i = 0; i < spawnCount; i++)
            {
                // ��� ���� ó�� ����

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

    // ��� ���� ����
    private void CreateDroneShooter()
    {
        // ����� ��� ������ �������� ����
        DroneShooterData droneShooterData = droneShooterDatas[Random.Range(0, droneShooterDatas.Length)];

        // ������ ��ġ�� �������� ����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ��� ü�̼� ���������κ��� ��� ����
        Drone_Shooter shooterDrone = Instantiate(drone_Shooter_Prefab, spawnPoint.position, spawnPoint.rotation);

        // ������ ����� �ɷ�ġ ����
        shooterDrone.Setup(droneShooterData);

        // ������ ��и� ����Ʈ�� �߰�
        shooterDrones.Add(shooterDrone);

        // ����� onDeath �̺�Ʈ�� �͸� �޼��� ���

        // ����� ��и� ����Ʈ���� ����
        shooterDrone.onDeath += () => shooterDrones.Remove(shooterDrone);
        // ����� ��и� 10 �� �ڿ� �ı�
        shooterDrone.onDeath += () => Destroy(shooterDrone.gameObject, 10f);
        // ��� ����� ���� ���
        shooterDrone.onDeath += () => GameManager.instance.AddScore(Random.Range(100, 500));

        shooterDrone.onDeath += () => GameManager.instance.AddCoin(Random.Range(100, 500));
    }

    private void CreateDroneChaser()
    {
        // ����� ��� ������ �������� ����
        DroneChaserData chaserDroneData = droneChaserDatas[Random.Range(0, droneChaserDatas.Length)];

        // ������ ��ġ�� �������� ����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ��� ü�̼� ���������κ��� ��� ����
        Drone_Chaser_attack chaserDrone = Instantiate(drone_Chaser_Prefab, spawnPoint.position, spawnPoint.rotation);

        // ������ ����� �ɷ�ġ ����
        chaserDrone.Setup(chaserDroneData);

        // ������ ��и� ����Ʈ�� �߰�
        chaserDrones.Add(chaserDrone);

        // ����� onDeath �̺�Ʈ�� �͸� �޼��� ���


        chaserDrone.onDeath += () => chaserDrone.sparkAttackStop();
        // ����� ��и� ����Ʈ���� ����
        chaserDrone.onDeath += () => chaserDrones.Remove(chaserDrone);
        // ����� ��и� 10 �� �ڿ� �ı�
        chaserDrone.onDeath += () => Destroy(chaserDrone.gameObject, 10f);
        // ��� ����� ���� ���
        chaserDrone.onDeath += () => GameManager.instance.AddScore(Random.Range(50, 300));
        chaserDrone.onDeath += () => GameManager.instance.AddCoin(Random.Range(50,300));
    }

    private void CreateBossDrone()
    {
        // ����� ��� ������ �������� ����
        DroneBossData bossDroneData = droneBossDatas[Random.Range(0, droneBossDatas.Length)];

        // ������ ��ġ�� �������� ����
        Transform spawnPoint = bossSpawnPosition;

        // ��� ü�̼� ���������κ��� ��� ����
        Drone_Boss_Controller bossDrone = Instantiate(drone_Boss_Prefab, spawnPoint.position, spawnPoint.rotation);

        // ������ ����� �ɷ�ġ ����
        bossDrone.Setup(bossDroneData);

        // ������ ��и� ����Ʈ�� �߰�
        bossDrones.Add(bossDrone);

        bossBGM();

        // ����� onDeath �̺�Ʈ�� �͸� �޼��� ���


        bossDrone.onDeath += () => bossDrone.sparkAttackStop();

        bossDrone.onDeath += () => bossDrones.Remove(bossDrone);
        // ����� ��и� 10 �� �ڿ� �ı�
        bossDrone.onDeath += () => Destroy(bossDrone.gameObject, 10f);
        // ��� ����� ���� ���
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
