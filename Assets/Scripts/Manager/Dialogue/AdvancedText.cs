using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class AdvancedText : TextMeshProUGUI
{
    int typingIndex;
    float defalutInterval = 0.08f;
    public Coroutine typingCor;//存储打字携程，易于中断
    Action OnFinish;
    AdvancedTextPreprocessor selfPreprocessor => (AdvancedTextPreprocessor)textPreprocessor;

    private void Init()
    {
        SetText("");
        ClearRubyText();
    }

    public AdvancedText()
    {
        textPreprocessor = new AdvancedTextPreprocessor();
    }

    /// <summary>
    /// 快速隐藏本句对话
    /// </summary>
    public void TextDisAppear()
    {
        for (; typingIndex < m_characterCount; typingIndex++)
            SetSingleCharacterAlpha(typingIndex, 0);
        ClearRubyText();
    }

    /// <summary>
    /// 快速显示本句对话
    /// </summary>
    public void TextQuickShow()
    {
        for (; typingIndex < m_characterCount; typingIndex++)
            SetSingleCharacterAlpha(typingIndex, 255);
        SetAllRubyTexts();
    }

    public IEnumerator ShowText(DialogueDataSequenceSO sequence)
    {
        if (typingCor != null)
        {
            StopCoroutine(typingCor);
        }
        typingCor = null;
        yield return null;
        //Debug.Log(sequence.currentIndex);
        SetText(sequence.dialogueLine[sequence.currentIndex].content);
        TextDisAppear();
        switch (sequence.displayType)
        {
            case E_DisplayType.Defalut:
                DefalutDisplay();
                SetAllRubyTexts();
                break;
            case E_DisplayType.Fading:
                FadingDisplay(sequence.fadeDuration);
                SetAllRubyTexts();
                break;
            case E_DisplayType.Typing:
                typingCor = StartCoroutine(TypingDisplay(sequence.needTypeWithFade, sequence.needTypeWithScale, sequence.fadeDuration));
                break;
            default:
                break;
        }
    }

    public IEnumerator ShowText(string content, E_DisplayType displayType, bool needTypeWithFade ,bool needTypeWithScale,float fadeDuration,float scaleDuration)
    {
        if (typingCor != null)
        {
            StopCoroutine(typingCor);
        }
        typingCor = null;
        yield return null;
        SetText(content);
        TextDisAppear();
        switch (displayType)
        {
            case E_DisplayType.Defalut:
                DefalutDisplay();
                SetAllRubyTexts();
                break;
            case E_DisplayType.Fading:
                FadingDisplay(fadeDuration);
                SetAllRubyTexts();
                break;
            case E_DisplayType.Typing:
                typingCor = StartCoroutine(TypingDisplay(needTypeWithFade, needTypeWithScale, fadeDuration, scaleDuration));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 文本整体直接显示
    /// </summary>
    void DefalutDisplay(Action action = null)
    {
        for (int i = 0; i < m_characterCount; i++)
            SetSingleCharacterAlpha(i, 255);
        action?.Invoke();
    }

    /// <summary>
    /// 文本整体淡入显示
    /// </summary>
    void FadingDisplay(float fadeDuration, Action action = null)
    {
        for (int i = 0; i < m_characterCount; i++)
            StartCoroutine(FadeInCharacter(i, fadeDuration));
        action?.Invoke();
    }

    /// <summary>
    /// 文本打字机显示
    /// </summary>
    /// <returns></returns>
    IEnumerator TypingDisplay(bool needFade,bool needScale,float fadeDuration=0.2f,float scaleDuration=0.1f ,Action action = null)
    {
        ForceMeshUpdate();
        for (int i = 0; i < m_characterCount; i++)
        {
            SetSingleCharacterAlpha(i, 0);

        }
        if(needScale)
        {
            for (int i = 0; i < m_characterCount; i++)
            {
                SetSingleCharacterScale(i, Vector2.one);

            }
        }

        typingIndex = 0;
        while (typingIndex < m_characterCount)
        {
            if (needFade)
                StartCoroutine(FadeInCharacter(typingIndex, fadeDuration)); //淡入打字机效果

            if(needScale)
                StartCoroutine(ScaleInCharacter(typingIndex, scaleDuration)); //缩放打字机效果

            if(!needFade && !needScale)
            {
                SetSingleCharacterAlpha(typingIndex, 255);   //无淡入打字机效果
            }


            if (selfPreprocessor.IntervalDictionary.TryGetValue(typingIndex, out float result))
                yield return new WaitForSecondsRealtime(result);
            else
                yield return new WaitForSecondsRealtime(defalutInterval);

            typingIndex++;
        }

        typingCor = null;
        action?.Invoke();
    }

    /// <summary>
    /// 设置单字符的透明度（每个字符都是由网格（含4个顶点）渲染）
    /// </summary>
    /// <param name="index"></param>
    /// <param name="newAlpha">newalpha范围为0~255</param>
    void SetSingleCharacterAlpha(int index, byte newAlpha)
    {
        TMP_CharacterInfo character = textInfo.characterInfo[index];//获取文本内容索引下的单个字符
        if (!character.isVisible)
            return;
        int matIndex = character.materialReferenceIndex;//获取字符材质索引
        int vertexIndex = character.vertexIndex;//获取字符顶点索引
        for (int i = 0; i < 4; i++)
        {
            textInfo.meshInfo[matIndex].colors32[vertexIndex + i].a = newAlpha;
        }
        UpdateVertexData();//更新顶点数据
    }


    /// <summary>
    /// 设置单个字符的缩放（基于原始顶点中心点）
    /// </summary>
    void SetSingleCharacterScale(int index, Vector2 scale)
    {
        TMP_CharacterInfo character = textInfo.characterInfo[index];
        if (!character.isVisible) return;

        int matIndex = character.materialReferenceIndex;
        int vertexIndex = character.vertexIndex;
        Vector3[] vertices = textInfo.meshInfo[matIndex].vertices;

        // 动态计算原始包围盒（基于第一次调用时的顶点数据）
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue);

        // 遍历所有顶点找到实际包围盒
        for (int i = 0; i < 4; i++)
        {
            Vector3 vertex = vertices[vertexIndex + i];
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        Vector3 center = (min + max) / 2f; // 包围盒中心点
        Vector3 size = max - min;          // 原始尺寸

        // 应用缩放
        for (int i = 0; i < 4; i++)
        {
            Vector3 offset = vertices[vertexIndex + i] - center;
            offset.x *= scale.x;
            offset.y *= scale.y;
            vertices[vertexIndex + i] = center + offset;
        }

        // 更新网格
        textInfo.meshInfo[matIndex].mesh.vertices = vertices;
        UpdateVertexData();
    }

    /// <summary>
    /// 改进的缩放动画协程（带缓动效果）
    /// </summary>
    IEnumerator ScaleInCharacter(int index, float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / duration);

            // 使用缓动函数（二次缓入缓出）
            float scaleValue = t < 0.5f ?
                2 * t * t :
                1 - Mathf.Pow(-2 * t + 2, 2) / 2;

            SetSingleCharacterScale(index, new Vector2(scaleValue, scaleValue));
            yield return null;
        }

        // 强制最终状态
        SetSingleCharacterScale(index, Vector2.one);
    }


    //}


    /// <summary>
    /// 单字符淡入
    /// </summary>
    /// <param name="index"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator FadeInCharacter(int index, float duration)
    {
        //如果找到Ruby起始位，设置Ruby预制件
        if (selfPreprocessor.TryGetRubyText(index, out RubyData data))
            SetRubyText(data);

        if (duration <= 0)
            SetSingleCharacterAlpha(index, 255);
        else
        {
            float timer = 0;
            while (timer < duration)
            {
                timer = Mathf.Min(duration, timer + Time.unscaledDeltaTime);
                SetSingleCharacterAlpha(index, (byte)(255 * (timer / duration)));
                yield return null;
            }
        }
    }

    void SetRubyText(RubyData data)
    {
        GameObject ruby = Instantiate(Resources.Load<GameObject>("RubyText"), transform);
        ruby.GetComponent<TextMeshProUGUI>().SetText(data.rubyContent);
        ruby.GetComponent<TextMeshProUGUI>().color = textInfo.characterInfo[data.startIndex].color;
        ruby.transform.localPosition = (textInfo.characterInfo[data.startIndex].topLeft + textInfo.characterInfo[data.endIndex].topRight) / 2 - new Vector3(0, 10, 0);
    }

    /// <summary>
    /// 清除当前的所有注释
    /// </summary>
    void ClearRubyText()
    {
        foreach (var item in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (item != this)
                Destroy(item.gameObject);
        }
        //Debug.Log(selfPreprocessor.rubyList.Count);
        selfPreprocessor.rubyList.Clear();
    }

    /// <summary>
    /// 用于跳过对话直接显示所有注释
    /// </summary>
    void SetAllRubyTexts()
    {
        foreach (var item in selfPreprocessor.rubyList)
        {
            SetRubyText(item);
        }
    }
}