using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogParser : MonoBehaviour
{
    public Dialog[] Parse(string csvFileName)
    {
        List<Dialog> dialogList = new List<Dialog>();       // ��� ����Ʈ ����
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName);     // csv ���� ��������

        string[] data = csvData.text.Split(new char[] { '\n' });        // ���� �������� �ɰ�
        
        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Dialog dialog = new Dialog();

            dialog._name = row[1];

            List<string> contextList = new List<string>();

            do
            {
                contextList.Add(row[2]);
   
                if (++i < data.Length)
                    row = data[i].Split(new char[] { ',' });

                else break;
            } while (row[0].ToString() == "");

            dialog._contexts = contextList.ToArray();

            dialogList.Add(dialog);
        }

        return dialogList.ToArray();
    }
}
