using System.Text;

namespace MDRender
{
    public class MRender
    {
        StringBuilder sb;
        private string InputText;
        private HashSet<string> EnabledStyles = new HashSet<string>();
        private static readonly char[] TrigerSymbols = { '`', '#', '*', '_', '>', '\n', '~', '\\' };
        private static readonly string[] HeaderStyles = { TextStyles.Header1, TextStyles.Header2, TextStyles.Header3 };

        private char _GetNextChar(ref int index)
        {
            if (index + 1 < InputText.Length)
                return InputText[++index];
            return '\0';
        }

        private void _SVChar(char c, ref int Index)
        {
            int startIdx = Index;
            switch (c)
            {
                case '`':
                    if (_GetNextChar(ref Index) == '`' && _GetNextChar(ref Index) == '`') // code block
                    {
                        if (!EnabledStyles.Contains(TextStyles.CodeBlock))
                        {
                            sb.Append(TextStyles.CodeBlock);
                            EnabledStyles.Add(TextStyles.CodeBlock);
                        }
                        else
                        {
                            sb.Append(TextStyles.ResetCodeBlock);
                            EnabledStyles.Remove(TextStyles.CodeBlock);
                        }
                    }
                    else // inline code
                    {
                        Index = startIdx;
                        if (!EnabledStyles.Contains(TextStyles.InlineCode))
                        {
                            sb.Append(TextStyles.InlineCode);
                            EnabledStyles.Add(TextStyles.InlineCode);
                        }
                        else
                        {
                            sb.Append(TextStyles.ResetInlineCode);
                            EnabledStyles.Remove(TextStyles.InlineCode);
                        }
                    }
                    break;

                case '*':
                    if (_GetNextChar(ref Index) == '*') // bold
                    {
                        if (!EnabledStyles.Contains(TextStyles.Bold))
                        {
                            sb.Append(TextStyles.Bold);
                            EnabledStyles.Add(TextStyles.Bold);
                        }
                        else
                        {
                            sb.Append(TextStyles.ResetBold);
                            EnabledStyles.Remove(TextStyles.Bold);
                        }
                    }
                    else // italic
                    {
                        Index = startIdx;
                        if (!EnabledStyles.Contains(TextStyles.Cursive))
                        {
                            sb.Append(TextStyles.Cursive);
                            EnabledStyles.Add(TextStyles.Cursive);
                        }
                        else
                        {
                            sb.Append(TextStyles.ResetCursive);
                            EnabledStyles.Remove(TextStyles.Cursive);
                        }
                    }
                    break;

                case '\\': // escaping
                    if (Index + 1 < InputText.Length)
                    {
                        sb.Append(InputText[Index + 1]);
                        Index++;
                    }
                    break;

                case '\n':
                    if (EnabledStyles.Overlaps(HeaderStyles))
                    {
                        sb.Append(TextStyles.ResetHeader);
                        foreach (var h in HeaderStyles) EnabledStyles.Remove(h);
                    }
                    else if (EnabledStyles.Contains(TextStyles.QuoteStyle))
                    {
                        sb.Append(TextStyles.ResetQuote);
                        EnabledStyles.Remove(TextStyles.QuoteStyle);
                    }
                    sb.Append('\n');
                    break;

                case '>':
                    if (!EnabledStyles.Contains(TextStyles.QuoteStyle))
                    {
                        sb.Append(TextStyles.QuoteStyle);
                        EnabledStyles.Add(TextStyles.QuoteStyle);
                    }
                    else
                    {
                        sb.Append(TextStyles.ResetQuote);
                        EnabledStyles.Remove(TextStyles.QuoteStyle);
                    }
                    break;

                case '~':
                    if (_GetNextChar(ref Index) == '~')
                    {
                        if (!EnabledStyles.Contains(TextStyles.Crossed))
                        {
                            sb.Append(TextStyles.Crossed);
                            EnabledStyles.Add(TextStyles.Crossed);
                        }
                        else
                        {
                            sb.Append(TextStyles.ResetCrossed);
                            EnabledStyles.Remove(TextStyles.Crossed);
                        }
                    }
                    else
                    {
                        Index = startIdx;
                        sb.Append('~');
                    }
                    break;
                case '#':
                    int start = Index;
                    int count = 1;

                    while (_GetNextChar(ref Index) == '#')
                        count++;

                    if (Index + 1 < InputText.Length && InputText[Index + 1] == ' ')
                        Index++;

                    switch (count)
                    {
                        case 1:
                            sb.Append(TextStyles.Header1);
                            EnabledStyles.Add(TextStyles.Header1);
                            break;
                        case 2:
                            sb.Append(TextStyles.Header2);
                            EnabledStyles.Add(TextStyles.Header2);
                            break;
                        case 3:
                            sb.Append(TextStyles.Header3);
                            EnabledStyles.Add(TextStyles.Header3);
                            break;
                        default:
                            Index = start;
                            sb.Append('#');
                            break;
                    }
                    break;
            }
        }

        public string GetMDTerminalText(string Text)
        {
            sb = new StringBuilder();
            InputText = Text ?? "";
            EnabledStyles.Clear();

            for (int i = 0; i < InputText.Length; i++)
            {
                char ch = InputText[i];
                if (TrigerSymbols.Contains(ch))
                {
                    _SVChar(ch, ref i);
                }
                else
                {
                    sb.Append(ch);
                }
            }

            if (EnabledStyles.Count > 0)
                sb.Append("\x1b[0m");

            return sb.ToString();
        }
    }
}
