using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UseObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        static List<RegistrationInformation> registrationInfo;

        public static void Init()
        {
            registrationInfo = new List<RegistrationInformation>();
        }

        public static void AllDeactive()
        {
            foreach (var info in registrationInfo)
            {
                info.allDeactive();
            }
        }

        public static void Set(GameObject key)
        {
            if (key == null) { Debug.LogError("key��null�ł�"); return; }

            RegistrationInformation info = getRegistrationInfo(key);
            if (info == null)
            {
                info = new RegistrationInformation(key);
                registrationInfo.Add(info);
            }

            info.increaseUsers();
        }

        public static RegistrationInformation getRegistrationInfo(GameObject obje)
        {
            if (registrationInfo == null) { Debug.LogError("registrationInfo������������Ă��܂���"); return null; }

            foreach (RegistrationInformation info in registrationInfo)
            {
                if (info.getKey() == obje) { return info; }
            }

            return null;
        }

        public static GameObject Get(GameObject key, bool active = true)
        {
            RegistrationInformation info = getRegistrationInfo(key);
            if (info == null) { Debug.LogError($"{key.name}���Z�b�g����Ă��܂���"); return null; }

            GameObject obje = info.getObject(active);
            if (obje == null)
            {
                // �����]���Ă���I�u�W�F�N�g���Ȃ���ΐV�����������A�����Ԃ�
                obje = Instantiate(key);
                info.addObject(obje);
            }

            return obje;
        }

        public static bool checkSetting(GameObject key)
        {
            RegistrationInformation info = getRegistrationInfo(key);
            return info != null;
        }


        // �g��Ȃ��Ȃ����I�u�W�F�N�g��(Key)Discionary�����菜��
        // ���p���𓯂��I�u�W�F�N�g��o�^���悤�Ƃ��������猸�炵�Ă����A�O�ɂȂ�Ǝ�菜��
        public static void RemoveKey(GameObject key)
        {
            RegistrationInformation info=getRegistrationInfo(key);
            if (info == null) { Debug.LogError($"{key.name}���o�^����Ă��܂���");return; }

            foreach (GameObject obje in info.getList())
            {
                Destroy(obje);
            }
            registrationInfo.Remove(info);
        }
    }
}

public class RegistrationInformation
{
    public RegistrationInformation(GameObject _key)
    {
        keyObject = _key;
        objectList = new List<GameObject>();
        parent = new GameObject().transform;
        parent.name = keyObject.name;
    }

    public void allDeactive()
    {
        foreach (GameObject obje in objectList)
        {
            obje.SetActive(false);
            numberOfUsers = 0;
        }
    }

    public void addObject(GameObject obje)
    {
        objectList.Add(obje);
        obje.transform.SetParent(parent);
    }

    public GameObject getObject(bool active = true)
    {
        // ��A�N�e�B�u�ɂȂ��Ă���I�u�W�F�N�g��T���A�A�N�e�B�u�����Ă���Ԃ�
        foreach (GameObject array in objectList)
        {
            if (array.activeSelf == false)
            {
                array.SetActive(active);
                array.transform.SetParent(parent);
                return array;
            }
        }

        return null;
    }

    public List<GameObject> getList()
    {
        return objectList;
    }

    public GameObject getKey()
    {
        return keyObject;
    }

    public void increaseUsers()
    {
        numberOfUsers++;
    }

    public void decreaseUsers()
    {
        numberOfUsers--;
    }

    private GameObject keyObject;
    private List<GameObject> objectList;
    private Transform parent;
    private int numberOfUsers;
}