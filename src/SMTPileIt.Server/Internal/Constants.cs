﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Internal
{
    internal static class Constants
    {
        //Wow
        //Claims to be RFC Compliant.
        //TODO: Work on shortening this, or validating that it should actually be this stupid.
        internal const string EmailAddressRegex =                   @"(?:(?:\r\n)?[ \t])*(?:(?:(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*))*@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)*\<(?:(?:\r\n)?[ \t])*(?:@(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*(?:,@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*)*:(?:(?:\r\n)?[ \t])*)?(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*))*@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*\>(?:(?:\r\n)?[ \t])*)|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)*:(?:(?:\r\n)?[ \t])*(?:(?:(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*))*@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)*\<(?:(?:\r\n)?[ \t])*(?:@(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*(?:,@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*)*:(?:(?:\r\n)?[ \t])*)?(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*))*@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*\>(?:(?:\r\n)?[ \t])*)(?:,\s*(?:(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*))*@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)*\<(?:(?:\r\n)?[ \t])*(?:@(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*(?:,@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*)*:(?:(?:\r\n)?[ \t])*)?(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r\n)?[ \t]))*""(?:(?:\r\n)?[ \t])*))*@(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*)(?:\.(?:(?:\r\n)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r\n)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r\n)?[ \t])*))*\>(?:(?:\r\n)?[ \t])*))*)?;\s*)";
        internal const string EndOfDataElement =                    ".\r\n";
        internal const string CarriageReturnLineFeed =              "\r\n";
        internal static string OKText { get { return "OK"; } }
        internal static string QuitText { get { return "Server closing transmission channel."; } }
        internal static string BeginDataText { get { return "Start mail input; end with < CRLF >.< CRLF > "; } }
    }
}
