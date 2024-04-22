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
            if (key == null) { Debug.LogError("keyがnullです"); return; }

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
            if (registrationInfo == null) { Debug.LogError("registrationInfoが初期化されていません"); return null; }

            foreach (RegistrationInformation info in registrationInfo)
            {
                if (info.getKey() == obje) { return info; }
            }

            return null;
        }

        public static GameObject Get(GameObject key, bool active = true)
        {
            RegistrationInformation info = getRegistrationInfo(key);
            if (info == null) { Debug.LogError($"{key.name}がセットされていません"); return null; }

            GameObject obje = info.getObject(active);
            if (obje == null)
            {
                // もし余っているオブジェクトがなければ新しく生成し、それを返す
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


        // 使わなくなったオブジェクトを(Key)Discionaryから取り除く
        // 利用数を同じオブジェクトを登録しようとした分から減らしていき、０になると取り除く
        public static void RemoveKey(GameObject key)
        {
            RegistrationInformation info=getRegistrationInfo(key);
            if (info == null) { Debug.LogError($"{key.name}が登録されていません");return; }

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
        // 非アクティブになっているオブジェクトを探し、アクティブ化してから返す
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