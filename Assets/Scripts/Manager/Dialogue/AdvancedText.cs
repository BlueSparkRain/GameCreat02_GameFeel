using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class AdvancedText : TextMeshProUGUI
{
    int typingIndex;
    float defalutInterval = 0.08f;
    public Coroutine typingCor;//�洢����Я�̣������ж�
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
    /// �������ر���Ի�
    /// </summary>
    public void TextDisAppear()
    {
        for (; typingIndex < m_characterCount; typingIndex++)
            SetSingleCharacterAlpha(typingIndex, 0);
        ClearRubyText();
    }

    /// <summary>
    /// ������ʾ����Ի�
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
    /// �ı�����ֱ����ʾ
    /// </summary>
    void DefalutDisplay(Action action = null)
    {
        for (int i = 0; i < m_characterCount; i++)
            SetSingleCharacterAlpha(i, 255);
        action?.Invoke();
    }

    /// <summary>
    /// �ı����嵭����ʾ
    /// </summary>
    void FadingDisplay(float fadeDuration, Action action = null)
    {
        for (int i = 0; i < m_characterCount; i++)
            StartCoroutine(FadeInCharacter(i, fadeDuration));
        action?.Invoke();
    }

    /// <summary>
    /// �ı����ֻ���ʾ
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
                StartCoroutine(FadeInCharacter(typingIndex, fadeDuration)); //������ֻ�Ч��

            if(needScale)
                StartCoroutine(ScaleInCharacter(typingIndex, scaleDuration)); //���Ŵ��ֻ�Ч��

            if(!needFade && !needScale)
            {
                SetSingleCharacterAlpha(typingIndex, 255);   //�޵�����ֻ�Ч��
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
    /// ���õ��ַ���͸���ȣ�ÿ���ַ����������񣨺�4�����㣩��Ⱦ��
    /// </summary>
    /// <param name="index"></param>
    /// <param name="newAlpha">newalpha��ΧΪ0~255</param>
    void SetSingleCharacterAlpha(int index, byte newAlpha)
    {
        TMP_CharacterInfo character = textInfo.characterInfo[index];//��ȡ�ı����������µĵ����ַ�
        if (!character.isVisible)
            return;
        int matIndex = character.materialReferenceIndex;//��ȡ�ַ���������
        int vertexIndex = character.vertexIndex;//��ȡ�ַ���������
        for (int i = 0; i < 4; i++)
        {
            textInfo.meshInfo[matIndex].colors32[vertexIndex + i].a = newAlpha;
        }
        UpdateVertexData();//���¶�������
    }


    /// <summary>
    /// ���õ����ַ������ţ�����ԭʼ�������ĵ㣩
    /// </summary>
    void SetSingleCharacterScale(int index, Vector2 scale)
    {
        TMP_CharacterInfo character = textInfo.characterInfo[index];
        if (!character.isVisible) return;

        int matIndex = character.materialReferenceIndex;
        int vertexIndex = character.vertexIndex;
        Vector3[] vertices = textInfo.meshInfo[matIndex].vertices;

        // ��̬����ԭʼ��Χ�У����ڵ�һ�ε���ʱ�Ķ������ݣ�
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue);

        // �������ж����ҵ�ʵ�ʰ�Χ��
        for (int i = 0; i < 4; i++)
        {
            Vector3 vertex = vertices[vertexIndex + i];
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        Vector3 center = (min + max) / 2f; // ��Χ�����ĵ�
        Vector3 size = max - min;          // ԭʼ�ߴ�

        // Ӧ������
        for (int i = 0; i < 4; i++)
        {
            Vector3 offset = vertices[vertexIndex + i] - center;
            offset.x *= scale.x;
            offset.y *= scale.y;
            vertices[vertexIndex + i] = center + offset;
        }

        // ��������
        textInfo.meshInfo[matIndex].mesh.vertices = vertices;
        UpdateVertexData();
    }

    /// <summary>
    /// �Ľ������Ŷ���Э�̣�������Ч����
    /// </summary>
    IEnumerator ScaleInCharacter(int index, float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / duration);

            // ʹ�û������������λ��뻺����
            float scaleValue = t < 0.5f ?
                2 * t * t :
                1 - Mathf.Pow(-2 * t + 2, 2) / 2;

            SetSingleCharacterScale(index, new Vector2(scaleValue, scaleValue));
            yield return null;
        }

        // ǿ������״̬
        SetSingleCharacterScale(index, Vector2.one);
    }


    //}


    /// <summary>
    /// ���ַ�����
    /// </summary>
    /// <param name="index"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator FadeInCharacter(int index, float duration)
    {
        //����ҵ�Ruby��ʼλ������RubyԤ�Ƽ�
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
    /// �����ǰ������ע��
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
    /// ���������Ի�ֱ����ʾ����ע��
    /// </summary>
    void SetAllRubyTexts()
    {
        foreach (var item in selfPreprocessor.rubyList)
        {
            SetRubyText(item);
        }
    }
}