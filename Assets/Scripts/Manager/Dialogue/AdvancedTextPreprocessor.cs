using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class AdvancedTextPreprocessor : ITextPreprocessor
{
    /// <summary>
    /// ��ӡ�ӳ��ֵ䡾��ȡ��ǩ<float>,�������ӳ�float����ӡ�������ݡ�
    /// </summary>
    public Dictionary<int, float> IntervalDictionary = new Dictionary<int, float>();//�Զ����ӡ��ǩ�ֵ�

    /// <summary>
    /// ע�ͱ�ǩ�б�
    /// </summary>
    public List<RubyData> rubyList = new List<RubyData>();
    public string PreprocessText(string text)
    {
        IntervalDictionary.Clear();
        string processingText = text;
        string pattern = "<.*?>";//����һ��������ʽ "<.*>" ����ƥ�������� < ��ͷ��> ��β���ַ�����̰��ģʽ��������ƥ����Զ�� >
        Match match = Regex.Match(processingText, pattern);//�״�ƥ�䣬�������и��ı���ǩ

        while (match.Success) //����ƥ�主�ı���ǩ
        {
            string label = match.Value.Substring(1, match.Length - 2);

            //�Զ����ӳٴ�ӡ��ǩ���涨<float>������¼�ڴ�ӡ�����ǩ�ֵ���
            if (float.TryParse(label, out float result))
                IntervalDictionary[match.Index - 1] = result;
            //�Զ���ע�ͱ�ǩ���涨<r="">��¼��ע���б�
            //ע�ͱ�ǩ����
            else if (Regex.IsMatch(label, "^r=.+")) //^����ʾ�ַ����Ŀ�ͷ���������ȷ��ƥ����ַ�������ӿ�ͷ��ʼ
                rubyList.Add(new RubyData(match.Index, label.Substring(2)));

            //ע�ͱ�ǩ��ֹ
            else if (Regex.IsMatch(label, "/r"))
            {
                if (rubyList.Count > 0)
                    rubyList[rubyList.Count - 1].endIndex = match.Index - 1;
            }

            processingText = processingText.Remove(match.Index, match.Length);//��ȡ�˴�ӡ�����ǩ��ɾ���˱�ǩ
            if (Regex.IsMatch(label, "^sprite.+"))  //�����ǩ��ʽ�Ǿ��飬��Ҫһ��ռλ��
                processingText.Insert(match.Index, "*");

            match = Regex.Match(processingText, pattern);//����ƥ�䣬������һ����ǩ
        }

        processingText = text;
        //������ʽ����
        // . ���������ַ�
        //*����ǰһ���ַ�����0�λ���
        //+����ǰһ���ַ�����1�λ���
        //������ǰһ���ַ�����0�λ�1��

        pattern = @"(<(\d+)(\.\d+)?>)|(</r>)|(<r=.*?>)";//ʹ�� @ ǰ׺���Ա������ַ�����ʹ��ת���ַ�
                                                        //�򵥽��ͣ��������ʵ���˶�ȡ<>�е�������С���Ĺ���
        /* (\d +)��
        \d + ��һ������ƥ��ģʽ����ƥ��һ�����������ַ���+��ʾǰ���ģʽ�����֣�����ƥ��һ�������ַ���
        () �ǲ�����ı�ǣ����� \d + ƥ�䵽�����ֲ��־ͻᱻ�������У������ں���������ʹ�á�

        (\.\d +)?��
        \. ƥ��һ�������ϵĵ㣨.���������һ��Ԫ�ַ����������б�ʾ�����ַ�������������Ҫ�� \ ����ת�壬��ʾ�����ϵĵ�š�
        \d + ƥ��һ���������֣���ʾС�������Ĳ��֡�
        () ? ��ʾ����������ǿ�ѡ�ģ���С�����ֲ��Ǳ���ġ����û��С�����֣���һ���ֻᱻ���ԡ�*/

        //�������ı��з���pattern������ַ��� �滻Ϊ ������ַ���
        processingText = Regex.Replace(processingText, pattern, "");

        return processingText;
    }

    /// <summary>
    /// ��ȡһ��λ���Ƿ����Ruby��ʼλ
    /// </summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryGetRubyText(int index, out RubyData data)
    {

        data = new RubyData(0, "");
        foreach (RubyData item in rubyList)
        {

            if (item.startIndex == index)
            {
                data = item;
                return true;
            }
        }
        return false;
    }
}

/// <summary>
/// ע������
/// </summary>
public class RubyData
{
    public RubyData(int _startIndex, string _content)
    {
        startIndex = _startIndex;
        rubyContent = _content;
    }
    public int startIndex { get; }
    public string rubyContent { get; }
    public int endIndex { get; set; }
}
