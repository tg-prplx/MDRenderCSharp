using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDRender
{
    internal static class TextStyles
    {
        public const string Bold = "\x1b[1m";
        public const string ResetBold = "\x1b[22m";

        public const string Cursive = "\x1b[3m";
        public const string ResetCursive = "\x1b[23m";

        public const string Underline = "\x1b[4m";
        public const string ResetUnderline = "\x1b[24m";

        public const string Crossed = "\x1b[9m";
        public const string ResetCrossed = "\x1b[29m";

        public const string InlineCode = "\x1b[48;5;240m\x1b[38;5;15m";
        public const string ResetInlineCode = "\x1b[0m";

        public const string QuoteStyle = "\x1b[90m";
        public const string ResetQuote = "\x1b[0m";

        public const string Header1 = "\x1b[1;34m";
        public const string Header2 = "\x1b[1;32m";
        public const string Header3 = "\x1b[1;36m";

        public const string ResetHeader = "\x1b[0m";

        public const string CodeBlock = "\x1b[48;5;240m\x1b[30m";
        public const string ResetCodeBlock = "\x1b[0m";
    }
}
