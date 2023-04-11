using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Dhs5.Utility;
using Dhs5.Utility.SaveSystem;
using NaughtyAttributes;

namespace Dhs5.Test
{
    public class Test : MonoBehaviour
    {
        //public Dico<string, int> dico = new(0);
        //public bool intActive = true;
        //[DrawIf("intActive")]
        //public int theInt;
        //[DrawIf("intActive", HidingType.READ_ONLY)]
        //public int theOtherInt;
        //public bool testIntActive = true;
        //[ShowIf("testIntActive")]
        //public int theTestInt;
        public Teest teest;
        // Start is called before the first frame update
        void Start()
        {
            SaveClassTest save1 = new();
            SaveClassTest save2 = new();
            save1.color = Color.red;
            save2.color = Color.blue;
            save1.rect = new Rect();
            save2.rect = new Rect();
            SavesRepertory<SaveClassTest, SaveInfo<SaveClassTest>> savesRepertory = new();
            savesRepertory.Add("SecondSave", save1);
            savesRepertory.Add("FirstSave", save2);
            //savesRepertory.Remove("ThirdSave");
            //if (savesRepertory.TryGetInfo("FirstSave", out SaveInfo<SaveClassTest> saveInfo))
            //    Debug.Log(saveInfo.Save.color);
            //if (savesRepertory.TryGetLastSaveInfo(out SaveInfo<SaveClassTest> saveInfo2))
            //    Debug.Log(saveInfo2.Save.color);
            ZDebug.Log(savesRepertory.GetInfosNameList(), true, "/-/");
            ZDebug.Log("/", 5, "*", "yt", 5, 8, 37.8f);
            int[][] jaggedArray2 = new int[][]
                {
                new int[] { 1, 3, 5, 7, 9 },
                new int[] { 0, 2, 4, 6 },
                new int[] { 11, 22 }
                };
            ZDebug.Log<int>(jaggedArray2, true);
            int[,] multiArray = new int[,] { { 1, 2, 3, 4 }, { 2, 12, 5, 6 }, { 3, 7, 8, 9 } };
            ZDebug.LogE(multiArray);
            ZDebug.LogG(ZDebugGroup.TEST, multiArray);
            ZDebug.LogS(multiArray);
            Invoke(nameof(Blabla), 2);
            Invoke(nameof(Blabla), 4);
            //foreach (string s in savesRepertory.GetInfosNameList())
            //{
            //    Debug.Log(s);
            //}
            //Debug.Log(save1.color);
            //savesRepertory.TryCopyFromSave("FourthSave", save1);
            //Debug.Log(save1.color);
            //dico.Serialize();
            //save.dico = dico;
            //File.WriteAllText(Application.persistentDataPath + "/save.json", JsonUtility.ToJson(save));
            //Debug.Log(JsonUtility.FromJson<SaveClassTest>(File.ReadAllText(Application.persistentDataPath + "/save.json")).color);
        }

        private void Blabla()
        {
            ZDebug.LogS("I'm Rick James BITCH");
        }
    }

    [System.Serializable]
    public class SaveClassTest : SaveClass
    {

        public Color color;
        public Rect rect;
        //public Dico<string, int> dico;

        protected override void CopyOperation(SaveClass saveClass)
        {
            SaveClassTest other = saveClass as SaveClassTest;
            color = other.color;
            rect = other.rect;
        }
    }

    [System.Serializable]
    public class Teest
    {
        public bool testIntActive = true;
        [ShowIf("testIntActive")]
        [AllowNesting]
        public int theTestInt;
    }
}
