using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class AdvancedTextPreprocessor : ITextPreprocessor
{
    /// <summary>
    /// 打印延迟字典【读取标签<float>,遇到将延迟float秒后打印后面内容】
    /// </summary>
    public Dictionary<int, float> IntervalDictionary = new Dictionary<int, float>();//自定义打印标签字典

    /// <summary>
    /// 注释标签列表
    /// </summary>
    public List<RubyData> rubyList = new List<RubyData>();
    public string PreprocessText(string text)
    {
        IntervalDictionary.Clear();
        string processingText = text;
        string pattern = "<.*?>";//定义一个正则表达式 "<.*>" 用于匹配所有以 < 开头、> 结尾的字符串。贪婪模式，即它会匹配最远的 >
        Match match = Regex.Match(processingText, pattern);//首次匹配，查找所有富文本标签

        while (match.Success) //尝试匹配富文本标签
        {
            string label = match.Value.Substring(1, match.Length - 2);

            //自定义延迟打印标签【规定<float>】并记录在打印间隔标签字典中
            if (float.TryParse(label, out float result))
                IntervalDictionary[match.Index - 1] = result;
            //自定义注释标签【规定<r="">】录入注释列表
            //注释标签开启
            else if (Regex.IsMatch(label, "^r=.+")) //^：表示字符串的开头。这个符号确保匹配的字符串必须从开头开始
                rubyList.Add(new RubyData(match.Index, label.Substring(2)));

            //注释标签终止
            else if (Regex.IsMatch(label, "/r"))
            {
                if (rubyList.Count > 0)
                    rubyList[rubyList.Count - 1].endIndex = match.Index - 1;
            }

            processingText = processingText.Remove(match.Index, match.Length);//读取此打印间隔标签后，删除此标签
            if (Regex.IsMatch(label, "^sprite.+"))  //如果标签格式是精灵，需要一个占位符
                processingText.Insert(match.Index, "*");

            match = Regex.Match(processingText, pattern);//继续匹配，查找下一个标签
        }

        processingText = text;
        //正则表达式概念
        // . 代表任意字符
        //*代表前一个字符出现0次或多次
        //+代表前一个字符出现1次或多次
        //？代表前一个字符出现0次或1次

        pattern = @"(<(\d+)(\.\d+)?>)|(</r>)|(<r=.*?>)";//使用 @ 前缀可以避免在字符串中使用转义字符
                                                        //简单解释：本句代码实现了读取<>中的整数或小数的功能
        /* (\d +)：
        \d + 是一个数字匹配模式，它匹配一个或多个数字字符。+表示前面的模式（数字）可以匹配一个或多个字符。
        () 是捕获组的标记，这样 \d + 匹配到的数字部分就会被捕获到组中，可以在后续处理中使用。

        (\.\d +)?：
        \. 匹配一个字面上的点（.）。点号是一个元字符，在正则中表示任意字符，但在这里需要加 \ 进行转义，表示字面上的点号。
        \d + 匹配一个或多个数字，表示小数点后面的部分。
        () ? 表示这个捕获组是可选的，即小数部分不是必需的。如果没有小数部分，这一部分会被忽略。*/

        //将处理文本中符合pattern规则的字符串 替换为 后面的字符串
        processingText = Regex.Replace(processingText, pattern, "");

        return processingText;
    }

    /// <summary>
    /// 读取一个位置是否就是Ruby起始位
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
/// 注释数据
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
